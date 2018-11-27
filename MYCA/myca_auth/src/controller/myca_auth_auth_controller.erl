% Current Application myca_auth
% Current Module auth

% So route entrypoint is module auth

-module(myca_auth_auth_controller, [Req]).
-compile(export_all).

% Routes a GET method to a "default" endpoint
% By accessing /auth/default we get the defined output
default('GET', []) ->
    L=boss_db:find(simpleauth,[]), % Finds all simpleauth records
    {json, [{auths,L}]}. % Serializes a JSON with the available simpleauth records