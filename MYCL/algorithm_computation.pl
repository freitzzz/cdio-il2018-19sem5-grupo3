compute_algorithm(1,Cities,CitiesToTravel,Distance):-
    assert_cities(Cities),
    Cities=[N|_].
    %tsp1(N,CitiesToTravel,Distance),
    %retractall(Cities).


assert_cities([]).
assert_cities([H|T]):-
    assert(H),
    assert_cities(T).