CheckItOut

Setup Instructions.
1. Setup local database on Dev Machine: using cmd execute "dotnet ef database update" at the following folder locations
  1.1. CheckItOut.Payments.Infrastructure
  1.2. Merchant.Infrastructure
2. run projectX... 
3. access swagger via the /docs endpoint
  3.1. in swagger use the security/token and get an access token
4: use access token to use payment endpoint

Design Considerations:
 - (could add dotnet db updates to powershell script)
 - Could add an IFrame for the Payment within the Merchant.Ui and webhooks with signalR for payment updates