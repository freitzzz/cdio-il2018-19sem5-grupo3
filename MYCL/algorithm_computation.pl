% Computes B&B for a provided list of cities
compute_algorithm(1,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[city(N,_,_)|_],
    tsp1(N,CitiesToTravel,Distance),
    retract_cities(Cities).

% Computes Greedy Heuristic for a provided list of cities
compute_algorithm(2,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[city(N,_,_)|_],
    tsp2(N,CitiesToTravel,Distance),
    retract_cities(Cities).

% Computes Greedy Heuristic 2 OPT for a provided list of cities
compute_algorithm(3,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[city(N,_,_)|_],
    tsp3(N,CitiesToTravel,Distance),
    retract_cities(Cities).

% Computes Genetic Algorithm for a provided list of cities
compute_algorithm(4,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[city(N,_,_)|_],
    tsp4(N,CitiesToTravel,Distance),
    retract_cities(Cities).

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