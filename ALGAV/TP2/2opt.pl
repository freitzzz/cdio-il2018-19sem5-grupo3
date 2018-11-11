%Carregar base de conhecimento
carregar:-['cdio-tsp.pl'],['intersept.pl'],['greedy.pl'].

tsp3(C, D, L):- tsp2(C, L1, D1),
                opt2_segment(L1, [H|T]), %segments the list into entries (C1, C2)
		!,
                opt2_combinations(H, T, [H|T], L2, D1, D), %calculates the path
		rGraph(C, L2, L). %reorders the path

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


degrees2radians(Deg,Rad):-
	Rad is Deg*0.0174532925.

% distance(latitude_first_point,longitude_first_point,latitude_second_point,longitude_second_point,distance in meters)

distance(Lat1, Lon1, Lat2, Lon2, Dis2):-
	degrees2radians(Lat1,Psi1),
	degrees2radians(Lat2,Psi2),
	DifLat is Lat2-Lat1,
	DifLon is Lon2-Lon1,
	degrees2radians(DifLat,DeltaPsi),
	degrees2radians(DifLon,DeltaLambda),
	A is sin(DeltaPsi/2)*sin(DeltaPsi/2)+ cos(Psi1)*cos(Psi2)*sin(DeltaLambda/2)*sin(DeltaLambda/2),
	C is 2*atan2(sqrt(A),sqrt(1-A)),
	Dis1 is 6371000*C,
	Dis2 is round(Dis1).

% distance(50.8462807,4.3547273,50.0878114,14.4204598,D).
% Online: http://www.movable-type.co.uk/scripts/latlong.html
%



%---------------------------------------------------------------------------------------------------------------------
% As fórmulas que verificam se dois segmentos se intersectam são para o plano, temos por isso de converter a latitude
% e longitude para o plano.
%
% Predicado auxiliar que a partir da latitude e longitude dá as coordenadas no plano através de uma conversão aproximada
% com base numa esfera
%
%  Conversão elipsoide -> plano: http://www.apsalin.com/convert-geodetic-to-cartesian.aspx
%
%  linearCoord(lisbon,X,Y).
%  X = 4908,
%  Y = -789.

linearCoord(City,X,Y):-
    city(City,Lat,Lon),
    geo2linear(Lat,Lon,X,Y).

geo2linear(Lat,Lon,X,Y):-
    degrees2radians(Lat,LatR),
    degrees2radians(Lon,LonR),
    X is round(6370*cos(LatR)*cos(LonR)),
    Y is round(6370*cos(LatR)*sin(LonR)).


% Detect intersections

% Given three colinear points p, q, r, the function checks if
% point q lies on line segment 'pr'
%onSegment(P, Q, R)
onSegment((PX,PY), (QX,QY), (RX,RY)):-
    QX =< max(PX,RX),
    QX >= min(PX,RX),
    QY =< max(PY,RY),
    QY >= min(PY,RY).


% To find orientation of ordered triplet (p, q, r).
% The function returns following values
% 0 --> p, q and r are colinear
% 1 --> Clockwise
% 2 --> Counterclockwise

orientation((PX,PY), (QX,QY), (RX,RY), Orientation):-
	Val is (QY - PY) * (RX - QX) - (QX - PX) * (RY - QY),

	(
		Val == 0, !, Orientation is 0;
		Val >0, !, Orientation is 1;
		Orientation is 2
	).

orientation4cases(P1,Q1,P2,Q2,O1,O2,O3,O4):-
    orientation(P1, Q1, P2,O1),
    orientation(P1, Q1, Q2,O2),
    orientation(P2, Q2, P1,O3),
    orientation(P2, Q2, Q1,O4).


% The main function that returns true if line segment 'p1q1'
% and 'p2q2' intersect.
doIntersect(P1,Q1,P2,Q2):-
    % Find the four orientations needed for general and
    % special cases
	orientation4cases(P1,Q1,P2,Q2,O1,O2,O3,O4),

	(
    % General case
    O1 \== O2 , O3 \== O4,!;

    % Special Cases
    % p1, q1 and p2 are colinear and p2 lies on segment p1q1
    O1 == 0, onSegment(P1, P2, Q1),!;

    % p1, q1 and p2 are colinear and q2 lies on segment p1q1
    O2 == 0, onSegment(P1, Q2, Q1),!;

    % p2, q2 and p1 are colinear and p1 lies on segment p2q2
    O3 == 0, onSegment(P2, P1, Q2),!;

     % p2, q2 and q1 are colinear and q1 lies on segment p2q2
    O4 == 0, onSegment(P2, Q1, Q2),!
    ).

%----------------------------------------------------------------------------------------------------
% rGraph(Origin,UnorderedListOfEdges,OrderedListOfEdges)
%
rGraph(Orig,[(Orig,Z)|R],R2):-!,
	reorderGraph([(Orig,Z)|R],R2).
rGraph(Orig,R,R3):-
	member((Orig,X),R),!,
	delete(R,(Orig,X),R2),
	reorderGraph([(Orig,X)|R2],R3).
rGraph(Orig,R,R3):-
	member((X,Orig),R),
	delete(R,(X,Orig),R2),
	reorderGraph([(Orig,X)|R2],R3).


reorderGraph([],[]).

reorderGraph([(X,Y),(Y,Z)|R],[(X,Y)|R1]):-
	reorderGraph([(Y,Z)|R],R1).

reorderGraph([(X,Y),(Z,W)|R],[(X,Y)|R2]):-
	Y\=Z,
	reorderGraph2(Y,[(Z,W)|R],R2).

reorderGraph2(_,[],[]).
reorderGraph2(Y,R1,[(Y,Z)|R2]):-
	member((Y,Z),R1),!,
	delete(R1,(Y,Z),R11),
	reorderGraph2(Z,R11,R2).
reorderGraph2(Y,R1,[(Y,Z)|R2]):-
	member((Z,Y),R1),
	delete(R1,(Z,Y),R11),
	reorderGraph2(Z,R11,R2).

