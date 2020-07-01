CI-CD BUILD STATUS: ![.NET Core](https://github.com/DeanHavelock/CheckItOut/workflows/.NET%20Core/badge.svg)

Setup Instructions.
1. Setup local database on Dev Machine: using cmd execute "dotnet ef database update" at the following folder locations
  1.1. CheckItOut.Payments.Infrastructure
  1.2. Merchant.Infrastructure
2. run xunit integration and unit tests
  2.1. CheckItOut.Payments.IntegrationTests
  2.2. CheckItOut.Payments.UnitTests
  2.3. Merchant.IntegrationTests
3. access swagger via the /swagger endpoint
  3.1. inspect swagger docs/endpoint for CheckItOut.Payment.Api at https://localhost:44379/swagger
4: use the merchant UI or tests to make an order, follow the flow through. Idempotency, Client Endpoint Authorization and HTTP retries implemented.

Implemented Design Considerations:
 - Onion Architecture, seperating Domain, Application, UI and Infrastructure concerns
 - Dependancy Inversion Principle to reduce coupling of components and increase easability of testing with Test Doubles.
 - Oauth2.0 Client Authorization on CheckItOut.Payments.Api
 - Idempotency on Payments implemented.
 - Http Retries Implemented.
 - Swagger endpoint documentation.
 - Create Payment Endpoint Returns link to Created Payment.
 - Tye (run in docker)

Future Design Considerations:
 - IFrame should not take amount in the url, instead CheckItOut.Payment.Api should have its own basket methods protected with client authentication for merchants to use and protect against client tampering of amount at checkout.
 - Domain model is anemic, could provide better encapsulation of state.
 - Domain model could use event sourcing to track changes over time for payment auditing and bug reproducability.
 - (could combine steps 1.1 + 1.2 "dotnet db updates" to setup powershell script)
 - Should add an IFrame for the CheckItOut.Payment.Api within the Merchant.Ui and webhooks with signalR for payment updates back to Merchant.Api and Merchant.Web.Ui.
 - realease version should upgrade security implementation to 3DS2 for full PCI DSS complaince (+ using IFrame described in previous step on checkout to capture payment information).
 - Could use IdentityServer4 for full Oauth2.0 flows, including Identity user login on MerchantUi for Authentication and Authorisation.