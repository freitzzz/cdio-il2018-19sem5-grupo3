% JSON Object com a informação basica de um algoritmo
:- json_object
        basic_algorithm(id:integer, name:string).

% JSON Object com a informação detalhada de um algoritmo
:- json_object
        detailed_algorithm(id:integer, name:string).

% JSON Object com a informação sobre uma cidade
:- json_object
        city_object(name:string, latitude:number,longitude:number).

% JSON Object com a informacao sobre um pedido de computação de um circuito sobre cidades
:- json_object
        cities_body_request(algorithmID:integer,cities:list).


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