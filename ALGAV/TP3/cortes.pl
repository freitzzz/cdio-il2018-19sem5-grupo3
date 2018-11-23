% get_corte(+ListaCortes,+Package,Corte)

% Caso se percorra os cortes todos e nao haja mais nenhum, entao deve ser criado num novo corte
get_corte([],(_,W,H),(W,H)):-!.

% Caso a largua seja menor à de um corte entao pertence a esse corte
get_corte([(DW,DH|_)],(_,W,_),D):-
    W=<DW,
    H=<DH,
    !.


% Percorre o mapa de cortes
get_corte([_|T],P,Corte):-
    get_corte(T,P,Corte).

add_corte([H|T],Corte):-
        