:- set_prolog_flag(answer_write_options,
                   [quoted(true), portray(true), spacing(next_argument)]).

%Loads guillotine_packing.pl
carregar :-
    ['guillotine_packing.pl'].

%Main predicate that runs the simulated annealing algorithm
simulated_annealing(Container, PO, RPL) :-
    (MAX, _, _)=Container,
    random(1, 100, N), %lists with 100 packages max
    randomPackageList(N, MAX, PL), %Generate initial solution
    descending_sort(PL, APL), %Sort list in descending order (x (depth) value is considered)
    startup(APL, Container, _, APO, _), %Get initial's solution cost (c_old)
    while(1.0, APO, Container, N, APL, RPL, PO), !. %Go into recursive while

%Recursive while loop predicate to run the simulated annealing algorithm
while(T, CPO, Container, N, PL, RPL, RPO) :-
    T>0.01,                 %While the temperature is bigger than 0.01
    (MAX, _, _)=Container,
    randomPackageList(N, MAX, GPL), %Genereate new solution
    descending_sort(GPL, DPL),
    startup(DPL, Container, _, DPO, _), %Get new solution's cost (c_new)
    (   DPO<CPO,                        %If c_new < c_cold
        CPO=DPO,                        %then initial solution = new solution
        PL=DPL
    ;   Ap is truncate(e**((CPO-DPO)/T)), %else use acceptance probability to maybe
        random(0.1, 1.0, Ran),            %move to the next solution
        (   Ap>Ran,                         
            CPO=DPO,                      %if the acceptance probability function returns
            PL=DPL                        %a value greater than a random number between 0 and 1
        ;   true                          %then initial solution = new solution and c_new = c_old
        )
    ),
    T1 is truncate(T*0.85),              %Calculate new temperature value
    while(T1,                            %Recursive call to predicate
          CPO,
          Container,
          N,
          PL,
          RPL,
          RPO).

while(T, CPO, _, _, PL, PL, CPO) :-     %Stopping condition
    T=<0.01.


%Auxiliary Predicates

%Creates a random package(x,y,z) with values between 1 and MAX
randomPackage(MAX, X, Y, Z) :-
    random(1, MAX, X),
    random(1, MAX, Y),
    random(1, MAX, Z).

%Creates a random package list with MAX number of orders
randomPackageList(N, MAX, PL) :-
    randomPackageList(1,MAX, N, [], PL), !.


randomPackageList(ID,MAX, 1, PL, RPL) :-
    randomPackage(MAX, X, Y, Z),
    P=(ID,X, Y, Z),
    append(PL, [P], RPL).

randomPackageList(ID,MAX, N, PL, RPL) :-
    N1 is N-1,
    randomPackage(MAX, X, Y, Z),
    P=(ID,X, Y, Z),
    ID1 is ID + 1,
    append(PL, [P], SPL),
    randomPackageList(ID1,MAX, N1, SPL, RPL).

%Sorts a list in ascending order
ascending_sort(PL, SPL) :-
    sort(0, @=<, PL, SPL).

%Sorts a list in descending order
descending_sort(PL, SPL) :-
    sort(0, @>=, PL, SPL).
