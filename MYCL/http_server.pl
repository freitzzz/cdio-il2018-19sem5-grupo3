:- use_module(library(http/thread_httpd)). % Needed to process HTTP Requests with the use of threads
:- use_module(library(http/http_dispatch)). % Needed to dispatch HTTP Requests
:- use_module(library(http/http_parameters)). % Needed to process HTTP Requests Parameteres
:- use_module(library(http/json)). % Need to process JSON Objects
:- use_module(library(http/json_convert)). % Need to convert JSON Objects in JSON strings
:- use_module(library(http/http_json)). % % Need to process JSON Objects in HTTP predicates
:- use_module(library(http/http_error)). % Needed for stack traces (Thanks to Jan for the tip)

:- http_handler('/mycl/api/algorithms',display_available_algorithms,[]). % Endpoint to display all algorithms available
:- http_handler('/mycl/api/travel',compute_algorithm,[time_limit(0)]). % Endpoint to compute a city circuit
:- http_handler('/mycl/api/factories',shortest_factory,[time_limit(0)]). % Endpoint to compute the shortest factory

% Loads required knowledge bases
carregar:-['intersept.pl'],['cdio-tsp.pl'],['parameters.pl'],load_computations,load_algorithms,load_json_objects.

% Loads required defined json objects
load_json_objects:-['json_algorithms.pl'].

% Loads required algorithms
load_algorithms:- ['algorithms/branch_and_bound.pl'],['algorithms/greedy.pl'],['algorithms/2opt.pl'],['algorithms/genetics.pl'].

% Loads computation predicates
load_computations:- ['algorithm_computation.pl'],['location_computation.pl'].

% Starts the server
server(Port) :-						% (2)
        http_server(http_dispatch, [port(Port)]).



% Displays all algorithm available
display_available_algorithms(_Request):-
        format('Content-type: application/json'),
        get_available_algorithms(Alg),
        prolog_to_json(Alg,AlgJSON),
        reply_json(AlgJSON).

% Computes a city circuit with a provided algorithm
compute_algorithm(Request):-
        http_read_json(Request, JSONIn,[json_object(cities_body_request)]),
        json_to_prolog(JSONIn, CC),
        CC=cities_body_request(Id,L),
        json_cities_to_cities(L,Cities),
        compute_algorithm(Id,Cities,CitiesToTravel,Distance),
        cities_to_json_cities(CitiesToTravel,JSONCitiesToTravel),
        DistanceJSON=distance_object(Distance,'KM'),
        prolog_to_json(cities_body_response(Id,JSONCitiesToTravel,DistanceJSON),RSP),
        format(user_output,"Request is: ~p~n",[JSONCitiesToTravel]),
        reply_json(RSP).


shortest_factory(Request):-
        http_read_json(Request, JSONIn,[json_object(factories_body_request)]),
        json_to_prolog(JSONIn, FF),
        FF=factories_body_request(JCity,JFactories),
        json_cities_to_cities([JCity],[City]),
        json_cities_to_cities(JFactories,Factories),
        compute_shortest_city(City,Factories,(ShortestFactory,Distance)),
        city_to_city_json_object(ShortestFactory,JShortestFactory),
        JDistance=distance_object(Distance,'KM'),
        prolog_to_json(factories_body_response(JShortestFactory,JDistance),SFJ),
        reply_json(SFJ).

% Checks the query parameters that can be extracted from the available algorithms URI
check_available_algorithms_query_parameters(Request,Id):-
        http_parameters(Request, [
                id(Id, [ optional(true) ])
        ]).

% Parses a list of json_object as cities to a city facts
json_cities_to_cities([],[]).
json_cities_to_cities([H|T],Cities):-
        json_to_prolog(H,H1),
        H1=city_object(N,LT,LO),
        json_cities_to_cities(T,Cities1),
        append([city(N,LT,LO)],Cities1,Cities).

% Parses a list of city facts as json_object cities
cities_to_json_cities([],[]).
cities_to_json_cities([H|T],JSONCities):-
        cities_to_json_cities(T,JSONCities1),
        append([basic_city_object(H)],JSONCities1,JSONCities).

% Parses a city fact into a json city object
city_to_city_json_object(city(Name,LT,LO),city_object(Name,LT,LO)).