% Current Application myca_auth
% Current Module auth

% So route entrypoint is module auth

-module(myca_auth_auth_controller, [Req]).
-compile(export_all).

% Routes a GET method to a "default" endpoint
% By accessing /auth/default we get the defined output
default('GET', []) ->
    L=boss_db:find(simpleauth,[]), % Finds all simpleauth records
    {json, L}. % Serializes a JSON with the available simpleauth records

% Routes a POST method to a "default2" endpoint
post('POST',[]) ->
    ReqBody=Req:request_body(),
    JSON=jsx:decode(ReqBody),
    Username=proplists:get_value(<<"username">>,JSON),
    Password=proplists:get_value(<<"password">>,JSON),
    SimpleAuth=simpleauth:new(id,"!!!",""),
    case SimpleAuth:save() of
        {ok,SavedSimpleAuth} ->
            {json,SavedSimpleAuth};
        {error,ErrorList} ->
            {json,[{"message",lists:flatten(ErrorList)}]}
    end.