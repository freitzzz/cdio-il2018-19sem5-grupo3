% Computes the shortest city between a set of cities
compute_shortest_city(CityX,Cities,ShortestCity):-
    compute_location_distances(CityX,Cities,Distances),
    sort(2,@=<,Distances,[ShortestCity|_]),
    !.

% Computes the distances between the locations of a set of cities and a certain city
compute_location_distances(_,[],[]).
compute_location_distances(LocationX,[H|T],Distances):-
    compute_location_distances(LocationX,T,Distances1),
    city(_,LTX,LOX)=LocationX,
    city(C,LTY,LOY)=H,
    distance(LTX,LOX,LTY,LOY,Distance),
    append([(H,Distance)],Distances1,Distances).