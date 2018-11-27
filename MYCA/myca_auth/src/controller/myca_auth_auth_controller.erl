% Current Application myca_auth
% Current Module auth

% So route entrypoint is module auth

-module(myca_auth_auth_controller, [Req]).
-compile(export_all).

% Routes a GET method to a "default" endpoint
% By accessing /auth/default we get the defined output
default('GET', []) ->
    {output, "Hello, world!<br>This is a simple route configuration"}.