 
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
    