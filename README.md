# UK Parliament - Product Team Home Exercise

## Dependencies
Please ensure you have the following installed:
* .NET 8 SDK (you may need to ensure your Visual Studio installation is fully up to date)
* Node v20.16.0 LTS

## Project Structure

The solution follows Clean Architecture principles, separating concerns into distinct layers with dependencies flowing inward. Domain is the core with no external dependencies, while outer layers may depend on inner layers but not vice versa.

### UKParliament.CodeTest.Domain

Contains the core business models, entities, and interfaces that represent the business concepts and rules. This project is framework-independent and includes:

* Domain models in the Models directory
* Repository interfaces in the Repositories directory
* Service interfaces in the Services directory

### UKParliament.CodeTest.Infrastructure

Provides implementations for interfaces defined in the Domain layer, focusing on:

* Data access repositories
* Database context and migrations

### UKParliament.CodeTest.Application

Contains application-specific logic that orchestrates domain entities to fulfill business use cases:

* Service implementations that coordinate business workflows
* Business logic that spans multiple domain entities
* Mapping configurations between domain entities and DTOs

### UKParliament.CodeTest.Web

The presentation layer built with ASP.NET Core and Angular:

* Backend controllers handling HTTP requests
* Frontend Angular application in ClientApp directory
* API endpoints for client-server communication
* API validation using FluentValidation
* Global Exception Handler

### UKParliament.CodeTest.Tests
Contains unit tests and integration tests for verifying the functionality of the application:

* Unit tests for domain models and services
* Integration tests for repository implementations
* Mock implementations for testing isolated components

## Getting Started for Development
* Clone this repository, and open the solution
* Set **UKParliament.CodeTest.Web** as the startup project
* Build and run it (NPM should install all the dependencies automatically)

## Running Tests
* Use the Test Explorer in Visual Studio to run the unit tests in the UKParliament.CodeTest.Tests project
* Run the UI tests via the command line in the clientApp directory:

```bash
# Run the Angular tests
ng test