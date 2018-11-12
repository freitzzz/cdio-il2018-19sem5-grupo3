% Computes B&B for a provided list of cities
compute_algorithm(1,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[city(N,_,_)|_],
    tsp1(N,CitiesToTravel,Distance),
    retract_cities(Cities),
    !.

% Computes Genetic Algorithm for a provided list of cities
compute_algorithm(1,Cities,_,_):-
    retract_cities(Cities),
    !.

% Computes Greedy Heuristic for a provided list of cities
compute_algorithm(2,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[city(N,_,_)|_],
    tsp2(N,CitiesToTravel,Distance),
    retract_cities(Cities),
    !.

% Computes Genetic Algorithm for a provided list of cities
compute_algorithm(2,Cities,_,_):-
    retract_cities(Cities),
    !.  

% Computes Greedy Heuristic 2 OPT for a provided list of cities
compute_algorithm(3,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[city(N,_,_)|_],
    tsp3(N,Distance,CitiesToTravel1),
    tuple_list_to_single_list(CitiesToTravel1,CitiesToTravel),
    format(user_output,CitiesToTravel),
    retract_cities(Cities),
    !.

% Computes Greedy Heuristic 2 OPT for a provided list of cities
compute_algorithm(3,Cities,_,_):-
    retract_cities(Cities),
    !.

% Computes Genetic Algorithm for a provided list of cities
compute_algorithm(4,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[city(N,_,_)|_],
    tsp4(N,CitiesToTravel,Distance),
    retract_cities(Cities),
    !.

% Computes Genetic Algorithm for a provided list of cities
compute_algorithm(4,Cities,_,_):-
    retract_cities(Cities),
    !.

% Asserts a list of cities into the current knowledge base
assert_cities([]).
assert_cities([H|T]):-
    assert(H),
    assert_cities(T).

% Retracts a list of cities from the current knowledge base
retract_cities([]).
retract_cities([H|T]):-
    retract(H),
    retract_cities(T).

% Transforms a tuple list into a single list
tuple_list_to_single_list([],[]).

tuple_list_to_single_list([(X,_)|T],SL):-
    tuple_list_to_single_list(T,SL1),
    append([X],SL1,SL).