CI-CD BUILD + TEST STATUS: ![.NET Core](https://github.com/DeanHavelock/CheckItOut/workflows/.NET%20Core/badge.svg)

Setup + Run Instructions.
1. Setup local database on Dev Machine: using cmd execute "dotnet ef database update" at the following folder locations
  1.1. CheckItOut.Payments.Infrastructure
  1.2. Merchant.Infrastructure
2. run xunit integration and unit tests
  2.1. CheckItOut.Payments.IntegrationTests
  2.2. CheckItOut.Payments.UnitTests
  2.3. Merchant.IntegrationTests
3. use the merchant UI or tests to make an order, follow the Pci-Dss flow through. See Idempotency, Client Endpoint Authorization and HTTP retries implemented.
3. Checkout and Pay for an Order using the User Interface:
  3.1 Browse to Checkout and Pay for your order through the Pci-Dss flow at the following location: https://localhost:44388/checkout (user interface flow working from checkout to payment and order submitted).
  3.2 Browse https://localhost:44388/merchant to see orders for a merchant.
4. view swagger payment api endpoint via the /swagger endpoint
  4.1. inspect swagger docs/endpoint for CheckItOut.Payment.Api at https://localhost:44379/swagger
5. view IdentityServer operations at https://localhost:5001/.well-known/openid-configuration
6. view current Build + Tests status at the top of this readme file.

Implemented Design:
 - Onion Architecture, seperating Domain, Application, UI and Infrastructure concerns
 - Dependancy Inversion Principle to reduce coupling of components and increase easability of testing with Test Doubles.
 - Test doubles for Unit Tests & Integration Tests using XUnit and Moq.
 - Persistance: Entity Framework Core with EF Migrations for Database Schema Updates.
 - CQRS (seperation of commands an queries, but in order to realise the benefits associated with this, these commandHandlers and QueryServices should be moved into their own projects, so they can be scaled independently).
 - Oauth2.0 Client Authorization on CheckItOut.Payments.Api
 - Idempotency on Payments implemented.
 - Https + Authorised Retries Implemented.
 - Swagger endpoint documentation.
 - Create Payment Endpoint Returns link to Created Payment.
 - CI-CD Build + Test Implemented with GitHub Actions using .github/workflows/dotnet-core.yml (status can be seen on GitHub ReadMe (Success Badge)), could extend deployment to push docker images to dockerHub, then publish to environments dev, test, uat + live.

Future Design Considerations:
 - Domain models are anemic, should provide better encapsulation of state and validation (constructor / named static constructor methods (opportunity for immutability)+ state transformation methods(encapsulation)).
 - Domain model could use event sourcing to track changes over time for payment auditing and bug reproducability, this would be a list of events within the domain model with eventHandlers within the domain model to update state, these domain events would be stored in an EventStore.
 - Could combine steps 1.1 + 1.2 "dotnet db updates" to setup powershell script
 - Hard coded url's should come from appsettings to allow for json config transformations (appsettings) for dev, test and live environment variables.
 - Could use an IFrame within Merchant.Ui for the CheckItOut.Payment and webhooks with signalR for payment updates back to Merchant.Api and Merchant.Web.Ui for user interface update.
 - Pci-Dss, the release version should upgrade security implementation to 3DS2 for full PCI DSS complaince, there is some security in place but it's possible there could be more requirements like database secure data encryption.
 - Could use IdentityServer4 for full Oauth2.0 flows, including Identity user login on MerchantUi for Authentication and Authorisation for example on the merchant orders page (have implemented this on a previous project which was released and used in live).
 - Should add more integration and unit tests around business rules with valid and non valid cases (put domain logic into entity methods and domain services, tests ensure existing functionality).
 - Tye (run in docker), update localhost urls to tye urls.