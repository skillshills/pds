import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, finalize } from 'rxjs/operators';
import { HttpResponse } from '@angular/common/http';

import { PersonService } from 'src/app/services/person.service';
import { DepartmentService } from 'src/app/services/department.service';
import { DepartmentViewModel } from 'src/app/models/department-view-model';
import { LoadingOverlayService } from '../../services/loading-overlay.service';
import { ToastService } from '../../services/toast.service';
import { PersonViewModel } from 'src/app/models/person-view-model';

@Component({
  selector: 'app-person-form',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.scss']
})
export class PersonComponent implements OnInit {
  personForm: FormGroup;
  isUpdateMode: boolean = false;
  loadingData: boolean = false;
  personId: number | null = null;
  departments: DepartmentViewModel[] = [];
  days: number[] = [];
  months = Array.from({ length: 12 }, (_, i) => {
    const date = new Date(0, i); // Create a date for each month (0-based index)
    return { name: date.toLocaleString('default', { month: 'long' }), value: i + 1 };
  });
  years: number[] = [];
  dobInvalid: boolean = false;
  dobInvalidMessage: string = 'required';
  formSubmitted: boolean = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private personService: PersonService,
    private departmentService: DepartmentService,
    private loadingOverlayService: LoadingOverlayService,
    private toastService: ToastService
  ) {
    this.personForm = this.fb.group({
      firstname: ['', Validators.required],
      lastname: ['', Validators.required],
      day: ['', Validators.required],
      month: ['', Validators.required],
      year: ['', Validators.required],
      departmentId: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.loadDepartments();

    this.route.params.subscribe((params) => {
      this.personId = params['id'];
      this.isUpdateMode = !!this.personId;

      if (this.isUpdateMode && this.personId) {
          this.loadPersonData(this.personId);
      }
    });

    // Populate days
    this.days = Array.from({ length: 31 }, (_, i) => i + 1);

    // Populate years (last 100 years)
    const currentYear = new Date().getFullYear();
    this.years = Array.from({ length: 100 }, (_, i) => currentYear - i);

    // Validate DOB whenever form changes
    this.personForm.valueChanges.subscribe(() => {
      this.validateDOB();
    });
  }

  loadDepartments() {
    this.departmentService.getDepartmentList().subscribe({
      next: (data) => (this.departments = data),
      error: (err) => console.error('Error loading departments', err),
    });
  }

  loadPersonData(id: number) {
    this.loadingData = true;

    setTimeout(() => {
      this.personService.getById(id).pipe(
        filter((response: HttpResponse<any>) => response.status === 200),
        finalize(() => this.loadingData = false)
      ).subscribe({
        next: (data) => {
          this.personForm.patchValue({
            firstname: data.body.firstName,
            lastname: data.body.lastName,
            departmentId: data.body.departmentId,
            day: new Date(data.body.dateOfBirth).getDate(),
            month: new Date(data.body.dateOfBirth).getMonth() + 1,
            year: new Date(data.body.dateOfBirth).getFullYear()
          });
        },
        error: (err) => {
          if (err.status === 404) {
            this.router.navigate([`/person/${id}/not-found`]);
          } else {
            this.toastService.addToast('Error loading person data', 'error', false);
          }
        }
      });
    }, 400);
  }

  formControlIsValid(fieldName: string) {
    return this.personForm.get(fieldName)?.invalid && this.personForm.get(fieldName)?.touched;
  }

  isFieldEmpty(fieldName: string): boolean {
    const value = this.personForm.get(fieldName)?.value;
    return value === '' || value === null || value === undefined;
  }

  validateDOB() {
    this.dobInvalid = false;
    this.dobInvalidMessage = 'required';

    // Check if any date field is empty and has been touched
    const dayEmpty = this.isFieldEmpty('day');
    const monthEmpty = this.isFieldEmpty('month');
    const yearEmpty = this.isFieldEmpty('year');
    
    // If any field is invalid and touched, mark DOB as invalid
    if (this.formControlIsValid('day') || this.formControlIsValid('month') || this.formControlIsValid('year')) {
      this.dobInvalid = true;
      return;
    }
    
    // If any field is empty, we can't validate the date yet
    if (dayEmpty || monthEmpty || yearEmpty) {
      // Only mark as invalid if they've attempted to submit the form
      this.dobInvalid = !!this.personForm.get('day')?.touched || 
                        !!this.personForm.get('month')?.touched || 
                        !!this.personForm.get('year')?.touched;
      return;
    }
  
    const { day, month, year } = this.personForm.value;
    const dateOfBirth = new Date(year, (month - 1), day);

    if (!(dateOfBirth.getFullYear() === Number(year) 
          && dateOfBirth.getMonth() === Number(month - 1) 
          && dateOfBirth.getDate() === Number(day))) {
      this.dobInvalid = true;
      this.dobInvalidMessage = 'invalid date';
      return;
    }

    const today = new Date();
  
    if(dateOfBirth >= today) {
      this.dobInvalid = true;
      this.dobInvalidMessage = 'must be in the past';
      return;
    };
  }

  // Mark all form controls as touched to trigger validation messages
  markAllAsTouched(): void {
    Object.values(this.personForm.controls).forEach(control => {
      control.markAsTouched();
    });
  }

  onSubmit() {
    this.formSubmitted = true;
    this.markAllAsTouched();
    this.validateDOB(); // Explicitly validate DOB on submit

    if (this.personForm.valid && !this.dobInvalid) {
      const { day, month, year, ...rest } = this.personForm.value;
      const dateOfBirth = `${year}-${String(month).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
      const personData = { ...rest, dateOfBirth };

      this.loadingOverlayService.show(this.isUpdateMode ? 'Updating' : 'Creating');

      if (this.isUpdateMode && this.personId) {
        // Call update service
        personData.id = this.personId;

        this.updatePerson(personData);
      } 
      else 
      {
        // Call create service
        this.createPerson(personData);
      }
    }
  }

  updatePerson(personData: PersonViewModel) {
    setTimeout(() => {
      this.personService.updatePerson(personData.id, personData).pipe(
        filter((response: HttpResponse<any>) => response.status === 204),
        finalize(() => this.loadingOverlayService.hide())
      ).subscribe({
        next: () => {
          this.toastService.addToast('Person updated successfully', 'success');
          this.router.navigate(['/']);
        },
        error: () => {
          this.toastService.addToast('Error updating person', 'error', false);
        }
      });
    }, 400);
  }

  createPerson(personData: PersonViewModel) {
    setTimeout(() => {
      this.personService.createPerson(personData).pipe(
        filter((response: HttpResponse<any>) => response.status === 201),
        finalize(() => this.loadingOverlayService.hide())
      ).subscribe({
        next: () => {
          this.toastService.addToast('Person created successfully', 'success');
          this.router.navigate(['/']);
        },
        error: () => {
          this.toastService.addToast('Error creating person', 'error', false);
        }
      });
    }, 400);
  }  

  backToHome() {
    this.router.navigate(['/']);
  }
}
