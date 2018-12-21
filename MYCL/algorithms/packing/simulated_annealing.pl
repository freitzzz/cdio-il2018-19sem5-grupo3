:- set_prolog_flag(answer_write_options,
                   [quoted(true), portray(true), spacing(next_argument)]).


%Main predicate that runs the simulated annealing algorithm
simulated_annealing(Container,PL, PO, RL,NRL) :-
    startup(PL, Container, MC, APO, NRL), %Get initial's solution cost (c_old)
    %write("PO original "),
    %write(APO),
    while(1.0,
          APO,
          Container,
          PL,
          _,
          PO,
          MC,
          RC),%Go into recursive while
    get_list(RC, [], RL), !. 

%Recursive while loop predicate to run the simulated annealing algorithm
while(T, CPO, Container, PL, RPL, RPO, MC, RC) :-
    T>0.01,                 %While the temperature is bigger than 0.01
    random_permutation(PL, GPL), %Generate new solution
    startup(GPL, Container, DC, DPO, _), %Get new solution's cost (c_new)
    (   DPO>CPO,                        %If c_new > c_cold
        NCPO=DPO,                        %then initial solution = new solution
        NPL=GPL,
        NC=DC
    ;   calculate_number(CPO, DPO, T, N),
        Ap is truncate(e**N), %else use acceptance probability to maybe
        random(0.1, 1.0, Ran),            %move to the next solution
        (   Ap>Ran,
            NCPO=DPO,                      %if the acceptance probability function returns
            NPL=GPL,                        %a value greater than a random number between 0 and 1
            NC=DC                         %then initial solution = new solution and c_new = c_old
        ;   NCPO=CPO,
            NPL=PL,
            NC=MC
        )
    ),
    %write("PO intermédia "),
    %write(NCPO),
    T1 is T*0.85,              %Calculate new temperature value
    round(T1, T2, 3),
    (   while(T2,
              NCPO,
              Container,
              NPL,
              RPL,
              RPO,
              NC,
              RC)    %Recursive call to predicate
    ;   RPO=NCPO,
        RPL=NPL,
        RC=NC
    ).

while(T, CPO, _, _, PL, PL, CPO, MC, MC) :-     %Stopping condition
    T=<0.01.


%Auxiliary Predicates

%Creates a random package(x,y,z) with values between 1 and MAX
randomPackage(MAX, X, Y, Z, Weight, Priority) :-
    random(1, MAX, X),
    random(1, MAX, Y),
    random(1, MAX, Z),
    random(1, MAX, Weight),
    random(1, MAX, Priority).

%Creates a random package list with MAX number of orders
randomPackageList(N, MAX, PL) :-
    randomPackageList(1, MAX, N, [], PL), !.


randomPackageList(ID, MAX, 1, PL, RPL) :-
    randomPackage(MAX, X, Y, Z, Weight, Priority),
    P=(ID, X, Y, Z, Weight, Priority),
    append(PL, [P], RPL).

randomPackageList(ID, MAX, N, PL, RPL) :-
    N1 is N-1,
    randomPackage(MAX, X, Y, Z, Weight, Priority),
    P=(ID, X, Y, Z, Weight, Priority),
    ID1 is ID+1,
    append(PL, [P], SPL),
    randomPackageList(ID1, MAX, N1, SPL, RPL).

round(X, Y, D) :-
    Z is X*10^D,
    round(Z, ZA),
    Y is ZA/10^D.

calculate_number(N1, N2, T, R) :-
    N is N1-N2,
    size_factor(N, 10, SF),
    R is T*SF.


size_factor(C, _, 1) :-
    C<10, !.
size_factor(C, N, SF) :-
    R is C/N,
    size_factor(R, N, SF1),
    SF is SF1*N.

%Sorts a list in ascending order
ascending_sort(PL, SPL) :-
    sort(0, @=<, PL, SPL).

%Sorts a list in descending order
descending_sort(PL, SPL) :-
    sort(0, @>=, PL, SPL).

get_list([], L, L).

get_list(MC, L, RL) :-
    MC=[((P, TL), SL)|FL],
    P=(ID,X,Y,Z,_,_,WE,PID),
    append([(ID,X,Y,Z,WE,PID)], L, NL),
    get_list(TL, NL, NNL),
    get_list(SL, NNL, NNNL),
    get_list(FL, NNNL, RL).