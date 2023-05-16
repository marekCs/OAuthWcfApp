# OAuthWcfApp
I am working on the following project: Provide OAUTH 2.0 sample using WCF .NET Framework 3.0 and javascript AJAX.

- Client: Victorio S. P. 
- Developer: Marek

Project Summary:
- OAuth 2.0 is a protocol that allows a user to grant limited access to their resources on one site, to another site, without having to expose their credentials.
The project includes the following implementation:

1) Authorization Request: The client requests authorization from the page to the wcf endpoint with the login and password credencials.

2) Authorization Grant: If the resource owner authorizes the request, the WCF server issues an authorization grant, which is sent back to the client. The grant is a credential representing the resource owner's authorization. It contains, among other things, a timestamp - the token's validity and the user's role.

3) Access Token Request: The client requests an access token from the authorization server by presenting the authorization grant. The client also authenticates itself to the authorization server at this stage.

4) Access Token: If the client is authenticated and the authorization grant is valid, the authorization server issues an access token to the client. The access token is a string representing the authorization granted to the client by the resource owner.

5) Protected Resource Request: The client requests the protected resources from the resource server (the server hosting the user's resources) and authenticates by presenting the access token.

6) Protected Resource: If the access token is valid, the resource server serves the requested resources to the client.
