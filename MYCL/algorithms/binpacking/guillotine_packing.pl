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
    (D, W, H, _)=Container,
    CVol is D*W*H,
    percentage_occupied([CP|RP], NPP, CVol, PO), !.

%Non recursive guillotine_packaging predicate. Used to insert the last package
guillotine_packaging([CP], Container, RC, MC, NPP, RNPP) :-
    (CD, CW, _, CWeight)=Container,
    (   (   (   guil(CP, Container, RC, MC)   %Try to insert package vertically (along the z axis)
            ;   addPackage(CP,
                           Container,              %Add package along the container's width (y axis)
                           (_, CD, CW, 0, CW, CD, CWeight, 0),
                           RC,
                           MC)
            )
        ;   addFront(CP,
                     Container,
                     (_, CD, CW, 0, CW, CD, CWeight, 0),
                     RC,
                     (_, CD, CW, 0, CW, CD, CWeight, 0),
                     MC)
        ), !,
        RNPP=NPP
    ;   MC=RC,
        append(NPP, [CP], RNPP)
    ).

%Recursive guillotine_packaging predicate
%RNPP - List of packages that weren't inserted in the container
guillotine_packaging([CP|RP], Container, RC, MC, NPP, RNPP) :-
    (CD, CW, CH, CWeight)=Container,
    CP=(_, _, _, _, Weight, _),
    (   (   (   guil(CP, Container, RC, LC)   %Try to insert package vertically (along the z axis)
            ;   addPackage(CP,
                           Container,              %Add package along the container's width (y axis)
                           (_, CD, CW, 0, CW, CD, CWeight, 0),
                           RC,
                           LC)
            )
        ;   addFront(CP,
                     Container,
                     (_, CD, CW, 0, CW, CD, CWeight, 0),
                     RC,
                     (_, CD, CW, 0, CW, CD, CWeight, 0),
                     LC)
        ), !,
        SNPP=NPP,    %If the package is inserted, the not placed list doesn't suffer changes
        NewWeight is CWeight-Weight
    ;   LC=RC,      %If the package is inserted, update the list with inserted products
        append(NPP, [CP], SNPP), %Updates not placed products list
        NewWeight=CWeight,
        true
    ),
    guillotine_packaging(RP,            %Recursively call the predicate
                         (CD, CW, CH, NewWeight),
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
    (_, D, W, H, _, _)=CP,
    (   not(member(CP, NPP)),
        Vol is D*W*H,
        RVol is Vol+PVol
    ;   true,
        RVol=PVol
    ).

%Recursive spacial_occupation predicate. Used to calculate all package's volumes except for the last
spacial_occupation([CP|RP], NPP, PVol, RVol) :-
    (_, D, W, H, _, _)=CP,
    (   not(member(CP, NPP)),
        Vol is D*W*H,
        SVol is Vol+PVol
    ;   true,
        SVol=PVol
    ),
    spacial_occupation(RP, NPP, SVol, RVol).

%Creates First Cut
guil(P, C, [], [(((ID, D, W, H, W, D, Weight, Prior), []), [])]) :- !,
    (ID, D, W, H, Weight, Prior)=P,
    (CD, CW, CH, _)=C,
    D=<CD,
    W=<CW,
    H=<CH.  

%Tries to place a package along the z axis
guil(P, C, [(((CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior), []), T)|CT], [(((CID, CutD, CutW, CutH, RW, RD, TWeight, FinalPrior), [(((ID, ND, NW, TH, W, D, Weight, Prior), []), [])]), T)|CT]) :- !,
    (ID, D, W, H, Weight, Prior)=P,
    (_, _, CH, _)=C, !,
    TWeight is RemainingWeight-Weight,
    TWeight>=0,
    Prior>=TotalPrior,
    FinalPrior=Prior,
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
guil(P, C, [(((CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior), [Child|T]), E)|CT], [(((CID, CutD, CutW, CutH, RW, RD, TWeight, FinalPrior), TL), E)|CT]) :- !,
    (_, D, _, _, Weight, Prior)=P,
    RemainingWeight>0,
    Prior>=TotalPrior,
    D=<RD,
    (   (   guil(P, C, [Child|T], TL) % Tries to insert a package along the z axis
        ;   addPackage(P,
                       C,
                       (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior), % Tries to insert a package along the y axis
                       [Child|T],
                       TL)
        )
    ;   addFront(P,
                 C,
                 (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior), % Tries to insert a package along the x axis
                 [Child|T],
                 (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior),
                 TL)
    ),
    TWeight is RemainingWeight-Weight,
    FinalPrior=Prior.

%Non recursive addFront predicate. Adds a package to the container along the x axis
addFront(P, C,  (_, _, CutW, CutH, _, RD, RemainingWeight, TotalPrior), [(((CID, X, Y, Z, NRW, RX, BWeight, BPrior), TP), SP)], Previous, NL) :- !,
    (ID, D, W, H, Weight, Prior)=P,
    (_, _, _, _, _, PD, _, _)=Previous,
    (_, CW, CH, _)=C,
    TWeight is RemainingWeight-Weight,
    TWeight>=0,
    Prior>=TotalPrior,
    Prior>=BPrior,
    TW is Y-NRW+W,
    TW=<CW,
    TD is D+X,
    TD=<PD,
    TD=<RD,
    TW=<CutW,
    TH is H+CutH,
    TH=<CH,
    NL=[(((CID, X, Y, Z, NRW, RX, BWeight, Prior), TP), SP),  (((ID, TD, TW, TH, W, D, Weight, Prior), []), [])].

%Recursive addFront predicate. Tries to add a package to the container along the x axis
addFront(P, C,  (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior), [(((RID, X, Y, Z, RY, RX, BWeight, BPrior), TP), SP)|RC], Prev, [(((RID, X, Y, Z, RY, RX, BWeight, Prior), TP), SP)|NL]) :- !,
    RC\==[],
    (_, D, _, _, Weight, Prior)=P,
    Prior>=TotalPrior,
    Prior>=BPrior,
    TWeight is RemainingWeight-Weight,
    TWeight>=0,
    (   D=<X,
        (   RC\==[],
            guil(P, C, RC, NL)
        ;   RC\==[],
            addPackage(P,
                       C,
                       (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior),
                       RC,
                       NL)
        )
    ;   RC\==[],
        addFront(P,
                 C,
                 (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior),
                 RC,
                 Prev,
                 NL)
    ).
%Non recursive addPackage predicate. Adds a package to the container along the y axis
addPackage(P, C,  (_, _, _, CutH, RW, _, RemainingWeight, TotalPrior), [(((RID, X, Y, Z, NRW, RX, SWeight, SPrior), T), [])|FP], NL) :- !,
    (ID, D, W, H, Weight, Prior)=P,
    (_, _, CH, _)=C,
    TWeight is RemainingWeight-Weight,
    TWeight>=0,
    Prior>=TotalPrior,
    Prior>=SPrior,
    D=<RX,
    TW is W+Y,
    TW=<RW,
    TH is H+CutH,
    TH=<CH,
    TD is X-RX+D,
    [(((RID, X, Y, Z, NRW, RX, SWeight, Prior), T), [(((ID, TD, TW, TH, W, D, Weight, Prior), []), [])])|FP]=NL.

%Recursive addPackage predicate. Tries to add a package to the container along the y axis
addPackage(P, C,  (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior), [(((RID, X, Y, Z, NRW, RX, SWeight, SPrior), TC), SC)|RC], [(((RID, X, Y, Z, NRW, RX, SWeight, Prior), TC), NL)|RC]) :- !,
    (_, D, _, _, Weight, Prior)=P,
    D=<RX,
    TWeight is RemainingWeight-Weight,
    TWeight>=0,
    Prior>=TotalPrior,
    Prior>=SPrior,
    (   (   SC\==[],
            guil(P, C, SC, NL)
        ;   SC\==[],
            addPackage(P,
                       C,
                       (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior),
                       SC,
                       NL)
        )
    ;   SC\==[],
        addFront(P,
                 C,
                 (CID, CutD, CutW, CutH, RW, RD, RemainingWeight, TotalPrior),
                 SC,
                 (RID, X, Y, Z, NRW, RX, SWeight, SPrior),
                 NL)
    ).
