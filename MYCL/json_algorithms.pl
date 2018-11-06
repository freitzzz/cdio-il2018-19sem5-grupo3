% JSON Object com a informação basica de um algoritmo
:- json_object
        basic_algorithm(id:integer, name:string).

% JSON Object com a informação detalhada de um algoritmo
:- json_object
        detailed_algorithm(id:integer, name:string).


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