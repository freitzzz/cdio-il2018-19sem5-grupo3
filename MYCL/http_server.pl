:- use_module(library(http/thread_httpd)). % Modulo Pedidos HTTP sobre threads
:- use_module(library(http/http_dispatch)). % Modulo para lançar os pedidos HTTP
:- use_module(library(http/http_parameters)). % Modulo para permitir configuracao dos parametros em pedidos HTTP
:- use_module(library(http/json)).
:- use_module(library(http/json_convert)).
:- use_module(library(http/http_json)).
:- use_module(library(http/websocket)).

:- http_handler('/asd', say_hi, []).		% (1)
%:- http_handler('/mycl/algorithms',display_available_algorithms,[]).
:- http_handler('/mycl/algorithms/',display_algorithm_by_id,[prefix]).

% Loads required knowledge bases
carregar:-['intersept.pl'],['cdio-tsp.pl'],load_json_objects.

load_json_objects:-['json_algorithms.pl'].


server(Port) :-						% (2)
        http_server(http_dispatch, [port(Port)]).

say_hi(_Request) :-					% (3)
        format('Content-type: application/json'),
        prolog_to_json(basic_algorithm(25,"50"),X),
        json_to_prolog(X,Y),
        reply_json(X).


% Displays all algorithm available
display_available_algorithms(_Request):-
    format('Content-type: application/json'),
    get_available_algorithms(Alg),
    prolog_to_json(Alg,AlgJSON),
    reply_json(AlgJSON).

% Displays an algorithm by its ID
display_algorithm_by_id(Request):-
    format('Content-type: application/json'),
    memberchk(path_info(Id), Request), 
    write(Id),
    get_algorithm_by_id(Id,Alg),
    prolog_to_json(Alg,AlgJSON),
    reply_json(AlgJSON).

