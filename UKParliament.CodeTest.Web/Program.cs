using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Application.Services;
using UKParliament.CodeTest.Domain.Repositories;
using UKParliament.CodeTest.Domain.Services;
using UKParliament.CodeTest.Domain.ViewModels;
using UKParliament.CodeTest.Infrastructure.DataContexts;
using UKParliament.CodeTest.Infrastructure.Repositories;
using UKParliament.CodeTest.Web.Middleware;
using UKParliament.CodeTest.Web.Validators;

namespace UKParliament.CodeTest.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add exception handling middleware globally
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddScoped<IPersonService, PersonService>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();

        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        // Add validators for controllers
        builder.Services.AddScoped<IValidator<PersonViewModel>, PersonRequestValidator>();

        builder.Services.AddDbContext<PersonManagerContext>(op => op.UseInMemoryDatabase("PersonManager"));

        builder.Services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });

        var app = builder.Build();

        // Create database so the data seeds
        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            using var context = serviceScope.ServiceProvider.GetRequiredService<PersonManagerContext>();
            context.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // Add exception handling middleware globally
        app.UseExceptionHandler();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}