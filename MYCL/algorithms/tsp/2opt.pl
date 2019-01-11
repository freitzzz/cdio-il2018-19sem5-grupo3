tspd3(C, D, L):- tspd2(C, L1, D1),
                opt2_segment(L1, [H|T]), %segments the list into entries (C1, C2)
		!,
                opt2_combinations(H, T, [H|T], L2, D1, D), %calculates the path
		rGraph(C, L2, L4),
		tuple_list_to_single_list(L4,L5),
		city(CSL,_,_,_),
		append(L5,[CSL],L),
		!. %reorders the path

opt2_segment([H1|[H]], [(H1, H)]).
opt2_segment([H1|[H|T]], [(H1, H)|SL]):- opt2_segment([H|T], SL).

%executes when the list of segments has only one element
opt2_combinations((C1,C2),[(C3,C4)] , L, L1, D, Daux):- linearCoord(C1,X1,Y1), linearCoord(C2,X2,Y2), %gets linear coordinates
						        linearCoord(C3,X3,Y3), linearCoord(C4,X4,Y4), %gets linear coordinates
							doIntersect((X1,Y1),(X2,Y2),(X3,Y3),(X4,Y4)), %checks if the paths intersect
							opt2_analysis(L, L1, (C1, C2), (C3, C4), D, Daux). %decides if the paths should be replaced

opt2_combinations((_,_),[(_,_)] , L1, L1, D1, D1).

%executes when the list of segments has many elements
opt2_combinations((C1, C2), [(C3, C4)|_], L, L1, D, D1) :- linearCoord(C1,X1,Y1), linearCoord(C2,X2,Y2), %gets linear coordinates
							   linearCoord(C3,X3,Y3), linearCoord(C4,X4,Y4), %gets linear coordinates
							   doIntersect((X1,Y1),(X2,Y2),(X3,Y3),(X4,Y4)), %checks if the paths intersect
							   opt2_analysis(L, L1, (C1, C2), (C3, C4), D, D1). %decides if the paths should be replaced

opt2_combinations((C1, C2), [(C3, C4)|T], [H1|T1], L, D, D2) :- opt2_combinations((C1, C2), T, [H1|T1], [H2|T2], D, D1), %checks all combinations between the first path and every other path on the list
	                                                        ([H2|T2] \== [H1|T1] -> opt2_combinations(H2, T2, [H2|T2], L, D1, D2); %if the returned list is different than the provided one, it starts over
								opt2_combinations((C3, C4), T, [H2|T2], L, D1, D2)). %if not, checks all combinations between the second path and every other path


opt2_evaluate_new_segment(L, C1, C2, C3, (C1, C3), D2) :- dist_cities(C1,C2,D1), %computes the distance between C1 and C2
	                                                  dist_cities(C1,C3,D2), %computes the distance between C1 and C3
					                  D1 > D2, %D1 must be greater than D2
						          not(has_circuit(L, C1, C3)), %there must not be an indirect path between C1 and C3
							  not(has_circuit(L, C3, C1)). %there must not be an indirect path between C3 and C1

opt2_evaluate_new_segment(L, C1, C2, _, (C1, C2), D) :- dist_cities(C1, C2, D), %computes the distance between C1 and C2
							not(has_circuit(L, C1, C2)), %there must not be an indirect path between C1 and C2
							not(has_circuit(L, C2, C1)). %there must not be an indirect path between C2 and C1

opt2_evaluate_new_segment(L, C1, _, C3, (C1, C3), D) :- dist_cities(C1, C3, D), %computes the distance between C1 and C3
							not(has_circuit(L, C1, C3)), %there must not be an indirect path between C1 and C3
							not(has_circuit(L, C3, C1)). %there must not be an indirect path between C3 and C1

opt2_analysis(L, Z, (C1, C2), (C3, C4), D, Daux) :- C2 \== C3, C1 \== C3, %the cities must be different
						    C2 \== C4, C1 \== C4,
						    dist_cities(C1, C2, D1), %computes the distance between C1 and C2
						    dist_cities(C3, C4, D2), %computes the distance between C3 and C4
						    D3 is D1 + D2, %sums the distances D1 and D2
						    delete(L, (C1, C2), Laux2), %deletes the path between C1 and C2
						    delete(Laux2, (C3, C4), Laux3), %deletes the path between C3 and C4
						    opt2_evaluate_new_segment(Laux3, C2, C3, C4, E1, D4), %checks if the intersections should be replaced with new edges
						    opt2_evaluate_new_segment(Laux3, C1, C3, C4, E2, D5),
						    D6 is D4 + D5, %sums the distanced D4 and D5
						    D3 > D6 -> Daux is D - D3 + D6, %checks if D3 is greater than D6 and if it is, replaces the intersection with a new path (2 new edges)
						    delete(L, (C1, C2), X),
						    delete(X, (C3, C4), Y),
						    append([E1], Y, W),
						    append([E2], W, Z).

has_circuit(_, C, C).

has_circuit(L, C1, C2):- member((C1, X),L),
                         has_circuit(L, X, C2).


% Transforms a tuple list into a single list
tuple_list_to_single_list([],[]).

tuple_list_to_single_list([(X,_)|T],SL):-
    tuple_list_to_single_list(T,SL1),
    append([X],SL1,SL).