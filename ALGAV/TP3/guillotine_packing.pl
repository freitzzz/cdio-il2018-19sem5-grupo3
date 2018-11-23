:- set_prolog_flag(answer_write_options,
                   [quoted(true), portray(true), spacing(next_argument)]).

%Main predicate that runs the guillotine heuristic with first fit heuristic
%and calculates the percentage of occupied space of a container that's filled
%with a given list of packages
%A package has an identifier and xyz dimensions
%CP - Current Package
%RP - Remaining Packages
%Container - Container of all packages
%MC - Map of Guillotine Cuts
%PO - Percentage of Occupation
%NPP - Not Placed Products
startup([CP|RP], Container, MC, PO, NPP) :-
    guillotine_packaging([CP|RP],
                         Container,
                         [],
                         MC,
                         [],
                         NPP),
    (D, W, H)=Container,
    CVol is D*W*H,
    percentage_occupied([CP|RP], NPP, CVol, PO), !.

%Non recursive guillotine_packaging predicate. Used to insert the last package
guillotine_packaging([CP], Container, RC, MC, NPP, RNPP) :-
    (   (_,X,Y,Z) = CP,
        guil((X,Y,Z), Container, RC, MC), !,
        RNPP=NPP
    ;   MC=RC,
        append(NPP, [CP], RNPP)
    ).

%Recursive guillotine_packaging predicate
%RNPP - List of packages that weren't inserted in the container
guillotine_packaging([CP|RP], Container, RC, MC, NPP, RNPP) :-
    (CD, CW, _)=Container,(_,X,Y,Z) = CP,
    (   (   
            guil((X,Y,Z), Container, RC, LC)   %Try to insert package vertically (along the z axis)
        ;   addPackage((X,Y,Z),               
                       Container,              %Add package along the container's width (y axis)
                       (CD, CW, 0, CW, CD),
                       RC,
                       LC)
        ), !,
        SNPP=NPP    %If the package is inserted, the not placed list doesn't suffer changes
    ;   LC=RC,      %If the package is inserted, update the list with inserted products
        append(NPP, [CP], SNPP), %Updates not placed products list
        true
    ),
    guillotine_packaging(RP,            %Recursively call the predicate
                         Container,
                         LC,
                         MC,
                         SNPP,
                         RNPP), !.

%Calculates the percentage of the space that's occupied in a container filled
%with packages given a list of packages (LP), list of not placed packages (NPP)
%and container volume (CVol)
percentage_occupied(LP, NPP, CVol, PO) :-
    spacial_occupation(LP, NPP, 0, PVol),
    PO is PVol/CVol*100.

%Non recursive spacial_occupation predicate. Used to calculate the last package's volume
spacial_occupation([CP], NPP, PVol, RVol) :-
    (_,D, W, H)=CP,
    (   not(member(CP, NPP)),
        Vol is D*W*H,
        RVol is Vol+PVol
    ;   true,
        RVol=PVol
    ).

%Recursive spacial_occupation predicate. Used to calculate all package's volumes except for the last
spacial_occupation([CP|RP], NPP, PVol, RVol) :-
    (_,D, W, H)=CP,
    (   not(member(CP, NPP)),
        Vol is D*W*H,
        SVol is Vol+PVol
    ;   true,
        SVol=PVol
    ),
    spacial_occupation(RP, NPP, SVol, RVol).

%Creates First Cut
guil(P, C, [], [[((D, W, H, W, D), [])]]) :-
    (D, W, H)=P,
    (CD, CW, CH)=C,
    D=<CD,
    W=<CW,
    H=<CH.  

%Tries to place a package along the z axis
guil(P, C, [[((CutD, CutW, CutH, RW, RD), [])]|CT], [[((CutD, CutW, CutH, RW, RD), [[((D, W, TH, W, D), [])]])]|CT]) :-
    (D, W, H)=P,
    (_, _, CH)=C, !,
    D=<CutD,
    W=<RW,
    TH is CutH+H,
    TH=<CH.
    
%Predicate used to know whether a package is going to be inserted along the x,y or z axes
guil(P, C, [[((CutD, CutW, CutH, RW, RD), [Child|T])]|CT], [[((CutD, CutW, CutH, RW, RD), NL)]|CT]) :-
    (D, _, _)=P,
    D=<CutD,
    (   (   guil(P, C, [Child|T], NL) % Tries to insert a package along the z axis
        ;   addPackage(P,                           
                       C,
                       (CutD, CutW, CutH, RW, RD), % Tries to insert a package along the y axis
                       [Child],
                       NL)
        )
    ;   addFront(P,
                 C,
                 (CutD, CutW, CutH, RW, RD), % Tries to insert a package along the x axis
                 [Child|T],
                 NL)
    ).

%Non recursive addFront predicate. Adds a package to the container along the x axis
addFront(P, C,  (_, _, CutH, RW, RD), [Child], [Child, [((TD, W, H, W, D), [])]]) :-
    (D, W, H)=P,
    [((X, _, _, _, _), _)]=Child,
    (_, _, CH)=C,
    TD is D+X,
    TD=<RD,
    W=<RW,
    TH is H+CutH,
    TH=<CH.

%Recursive addFront predicate. Tries to add a package to the container along the x axis
addFront(P, C,  (CutD, CutW, CutH, RW, RD), [Child|RC], RL) :-
    RC\==[],
    (D, _, _)=P,
    (   [((X, _, _, _, _), _)]=Child
    ;   [[((X, _, _, _, _), _)]]=Child
    ),
    (   D=<X,
        (   guil(P, C, [Child], NL),
            RL=[NL|RC]
        ;   addPackage(P,
                       C,
                       (CutD, CutW, CutH, RW, RD),
                       RC,
                       NL),
            RL=[Child|NL]
        )
    ;   addFront(P,
                 C,
                 (_, _, CutH, RW, RD),
                 RC,
                 NL),
        RL=[Child|NL]
    ).

%Non recursive addPackage predicate. Adds a package to the container along the y axis
addPackage(P, C,  (_, _, CutH, RW, _), [Child], NL) :-
    (   guil(P, C, [Child], NL)
    ;   (D, W, H)=P,
        [((X, Y, _, _, _), _)]=Child,
        (_, _, CH)=C,
        D=<X,
        TW is W+Y,
        TW=<RW,
        TH is H+CutH,
        TH=<CH,
        append([Child],
               [[((D, TW, TH, W, D), [])]],
               NL)
    ).

%Recursive addPackage predicate. Tries to add a package to the container along the y axis
addPackage(P, C,  (CutD, CutW, CutH, RW, RD), [Child|RC], RL) :-
    RC\==[],
    (D, _, _)=P,
    (   [((X, _, _, _, _), _)]=Child
    ;   [[((X, _, _, _, _), _)]]=Child
    ),
    D=<X,
    (   guil(P, C, [Child], NL),
        RL=[NL|RC]
    ;   addPackage(P,
                   C,
                   (CutD, CutW, CutH, RW, RD),
                   RC,
                   NL),
        RL=[Child|NL]
    ).
