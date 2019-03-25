# CrowdLending API ASP.NET Core example
You can visit the swagger UI at https://*api_base_url*/swagger to discover and test the API.

## Authentication
The API supports the OpenID standard for token authentication.
A token can be requested from the `/token` endpoint by providing user name and password.
Authenticated requests must contain the token inside the *Authorization* header.

## Mock Users
There are some mock users that you can use:
- dave@test.local Password123!
- sarah@test.local Password123!
- john@test.local Password123!