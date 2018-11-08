:- use_module(library(http/json)).
:- use_module(library(http/json_convert)).

% JSON Object with the basic information regarding an algorithm
:- json_object
        basic_algorithm(id:integer, name:string).

% JSON Object with the detailed information regarding an algorithm
:- json_object
        detailed_algorithm(id:integer, name:string).

% JSON Object with the detailed information regarding a city
:- json_object
        city_object(name:atom, latitude:number,longitude:number).

% JSON Object with the basic information regarding a city
:- json_object
        basic_city_object(name:atom).

% JSON Object with the information regarding the request cities computation body request
:- json_object
        cities_body_request(algorithmID:integer,cities:list).

% JSON Object with the information regarding the distance of a circuit
:- json_object
        distance_object(value:number,unit:atom).

% JSON Object with the information regarding the request cities computation body response
:- json_object
        cities_body_response(algorithmID:integer,citiesToTravel:list,distance:distance_object/2).

% JSON Object with the information regarding the compute shortest factory body request
:- json_object
        factories_body_request(city:city_object/3, factories:list).

% JSON Object with the information regarding the compute shortest factory body response
:- json_object
        factories_body_response(factory:city_object/3, distance:distance_object/2).

% Available algorithms
algorithm(1,"Branch & Bound").
algorithm(2,"Shortest Path Greedy Heuristic").
algorithm(3,"Shortest Path Greedy Heuristic Intersections Optimization").
algorithm(4,"Genetic Algorithm").

% Gets all available algorithms
get_available_algorithms(Alg):-
    findall(basic_algorithm(Id,Name),algorithm(Id,Name),Alg).

% Gets an algorithm by his ID
get_algorithm_by_id(Id,Alg):-
    algorithm(Id,Name),
    Alg=basic_algorithm(Id,Name).

% Gets an algorithm by his name
get_algorithm_by_name(Name,Alg):-
    algorithm(Id,Name),
    Alg=basic_algorithm(Id,Name).