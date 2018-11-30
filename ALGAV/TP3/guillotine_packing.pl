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
    (CD, CW, _)=Container,
    (   (_, X, Y, Z)=CP,
        (   (   guil((X, Y, Z),
                     Container,
                     RC,
                     MC)   %Try to insert package vertically (along the z axis)
            ;   addPackage((X, Y, Z),
                           Container,              %Add package along the container's width (y axis)
                           (CD, CW, 0, CW, CD),
                           RC,
                           MC)
            )
        ;   addFront((X, Y, Z),
                     Container,
                     (CD, CW, 0, CW, CD),
                     RC,
                     (CD, CW, 0, CW, CD),
                     MC)
        ), !,
        RNPP=NPP
    ;   MC=RC,
        append(NPP, [CP], RNPP)
    ).

%Recursive guillotine_packaging predicate
%RNPP - List of packages that weren't inserted in the container
guillotine_packaging([CP|RP], Container, RC, MC, NPP, RNPP) :-
    (CD, CW, _)=Container,
    (_, X, Y, Z)=CP,
    (   (   (   guil((X, Y, Z),
                     Container,
                     RC,
                     LC)   %Try to insert package vertically (along the z axis)
            ;   addPackage((X, Y, Z),
                           Container,              %Add package along the container's width (y axis)
                           (CD, CW, 0, CW, CD),
                           RC,
                           LC)
            )
        ;   addFront((X, Y, Z),
                     Container,
                     (CD, CW, 0, CW, CD),
                     RC,
                     (CD, CW, 0, CW, CD),
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
    (_, D, W, H)=CP,
    (   not(member(CP, NPP)),
        Vol is D*W*H,
        RVol is Vol+PVol
    ;   true,
        RVol=PVol
    ).

%Recursive spacial_occupation predicate. Used to calculate all package's volumes except for the last
spacial_occupation([CP|RP], NPP, PVol, RVol) :-
    (_, D, W, H)=CP,
    (   not(member(CP, NPP)),
        Vol is D*W*H,
        SVol is Vol+PVol
    ;   true,
        SVol=PVol
    ),
    spacial_occupation(RP, NPP, SVol, RVol).

%Creates First Cut
guil(P, C, [], [(((D, W, H, W, D), []), [])]) :-
    !,(D, W, H)=P,
    (CD, CW, CH)=C,
    D=<CD,
    W=<CW,
    H=<CH.  

%Tries to place a package along the z axis
guil(P, C, [(((CutD, CutW, CutH, RW, RD), []), T)|CT], [(((CutD, CutW, CutH, RW, RD), [(((ND, NW, TH, W, D), []), [])]), T)|CT]) :-
    !,(D, W, H)=P,
    (_, _, CH)=C, !,
    D=<RD,
    W=<RW,
    TH is CutH+H,
    TH=<CH,
    (   CutW==RW,
        NW=W
    ;   CutW\==RW,
        NW is CutW-RW+W
    ),
    (   CutD==RD,
        ND=D
    ;   CutD\==RD,
        ND is CutD-RD+D
    ).
    
%Predicate used to know whether a package is going to be inserted along the x,y or z axes
guil(P, C, [(((CutD, CutW, CutH, RW, RD), [Child|T]), E)|CT], [(((CutD, CutW, CutH, RW, RD), TL), E)|CT]) :-
    !,(D, _, _)=P,
    D=<RD,
    (   (   guil(P, C, [Child|T], TL) % Tries to insert a package along the z axis
        ;   addPackage(P,
                       C,
                       (CutD, CutW, CutH, RW, RD), % Tries to insert a package along the y axis
                       [Child|T],
                       TL)
        )
    ;   addFront(P,
                 C,
                 (CutD, CutW, CutH, RW, RD), % Tries to insert a package along the x axis
                 [Child|T],
                 (CutD, CutW, CutH, RW, RD),
                 TL)
    ).

%Non recursive addFront predicate. Adds a package to the container along the x axis
addFront(P, C,  (_, _, CutH, RW, RD), [(((X, Y, Z, NRW, RX), TP), SP)], Previous, NL) :-
    !,(D, W, H)=P,
    (_, _, _, _, PD)=Previous,
    (_, _, CH)=C,
    TW is Y-NRW + W,
    TD is D+X,
    TD=<PD,
    TD=<RD,
    W=<RW,
    TH is H+CutH,
    TH=<CH,
    NL=[(((X, Y, Z, NRW, RX), TP), SP),  (((TD, TW, TH, W, D), []), [])].

%Recursive addFront predicate. Tries to add a package to the container along the x axis
addFront(P, C,  (CutD, CutW, CutH, RW, RD), [(((X, Y, Z, RY, RX), TP), SP)|RC], _, [(((X, Y, Z, RY, RX), TP), SP)|NL]) :-
    !,RC\==[],
    (D, _, _)=P,
    (   D=<X,
        (   RC\==[],
            guil(P, C, RC, NL)
        ;   RC\==[],
            addPackage(P,
                       C,
                       (CutD, CutW, CutH, RW, RD),
                       RC,
                       NL)
        )
    ;   RC\==[],
        addFront(P,
                 C,
                 (_, _, CutH, RW, RD),
                 RC,
                 (_, _, CutH, RW, RD),
                 NL)
    ).
%Non recursive addPackage predicate. Adds a package to the container along the y axis
addPackage(P, C,  (_, _, CutH, RW, _), [(((X, Y, Z, NRW, RX), T), [])|FP], NL) :-
    !,(D, W, H)=P,
    (_, _, CH)=C,
    D=<RX,
    TW is W+Y,
    TW=<RW,
    TH is H+CutH,
    TH=<CH,
    TD is X-RX+D,
    [(((X, Y, Z, NRW, RX), T), [(((TD, TW, TH, W, D), []), [])])|FP]=NL.

%Recursive addPackage predicate. Tries to add a package to the container along the y axis
addPackage(P, C,  (CutD, CutW, CutH, RW, RD), [(((X, Y, Z, NRW, RX), TC), SC)|RC], [(((X, Y, Z, NRW, RX), TC), NL)|RC]) :-
    !,(D, _, _)=P,
    D=<RX,
    (   (   SC\==[],
            guil(P, C, SC, NL)
        ;   SC\==[],
            addPackage(P,
                       C,
                       (CutD, CutW, CutH, RW, RD),
                       SC,
                       NL)
        )
    ;   SC\==[],
        addFront(P,
                 C,
                 (CutD, CutW, CutH, RW, RD),
                 SC,
                 (X, Y, Z, NRW, RX),
                 NL)
    ).
