:- use_module(library(random)). % Biblioteca para uso de valores aleartórios
:- dynamic cities/1. % Factos dinâmicos de modo a permitir inserção e remoção em runtime


tsp4:-tsp4(_,_,_).

% Aplica o algoritmo genético no TSP
tsp4(C,L,D):-
    city(C,_,_),
    tspG(C,L,D),
    !.

% Aplica o algoritmo genético no TSP
% TEMP SOLUTION
%tsp4(C,L,D):-
%    city(C,_,_),
%    (
%        tspG(C,L,D),!
%        ;
%        tsp4(C,L,D),!
%    ),
%    !.

% Aplica o algoritmo genético no TSP
tsp4(C,L,D):-
    city(C,_,_),
    tspG(C,L,D),
    !.

% Gera "gerações"
tspG(C,L,D) :-
    numberOfCities(N),
    retractall(cities(_)),
    assert(cities(N)),
    generate_pop(Pop),
    evaluate_pop(Pop,PopEv),
    sort_population(PopEv,PopOrd),
    generations(NG),
    generate_gen(NG,PopOrd,TMC),
    finish_circuits(C,TMC,FTMC),
    sort_population(FTMC,[L*D|_]),
    !.

% Gera populacoes
generate_pop(Pop):-
    population(SizePop),
    findall(City,city(City,_,_),CityList),
    generate_pop(SizePop,CityList,Pop).

% Condição de Paragem de geração de população
generate_pop(0,_,[]):-!.

generate_pop(SizePop,CityList,[Ind|Others]):-
    SizePop1 is SizePop-1,
    generate_pop(SizePop1,CityList,Others),
    generate_ind(CityList,Ind), not(member(Ind,Others)).

% Gera um individuo (Baralhar - Roleta)
generate_ind(CityList,Ind):-
    random_permutation(CityList,Ind).

% Condição de Paragem de gerar uma geração
generate_gen(0,_,[]):-!.
    %write('Generation '),write(0),write(':'),nl,
    %write(Pop),nl.

% Gera gerações
generate_gen(G,Pop,TG):-
    %write('Generation '),write(G),write(':'),nl,
    %write(Pop),nl,
    crossover(Pop,NPop1),
    mutation(NPop1,NPop),
    evaluate_pop(NPop,NPopEv),
    sort_population(NPopEv,NPopSort),
    G1 is G-1,
    generate_gen(G1,NPopSort,TG1),
    [MP|_]=NPopSort,
    append([MP],TG1,TG).

% Condição de Paragem Crossover
crossover([],[]).

crossover([Ind*_],[Ind]). %Ind*_ 

crossover([Ind1*_,Ind2*_|Other],[NInd1,NInd2|Other1]):-
    generate_cutpoints(P1,P2),
    crossing_prob(Pcross),Pc is random(1),
    ((Pc =< Pcross,!, cross(Ind1,Ind2,P1,P2,NInd1),
        cross(Ind2,Ind1,P1,P2,NInd2))
    ;
    (NInd1=Ind1,NInd2=Ind2)),
    crossover(Other,Other1).

% Crossover por ordem
cross(Ind1,Ind2,P1,P2,NInd1):-
    sublist(Ind1,P1,P2,Sub1),
    cities(NumT), R is NumT-P2,
    rotate_right(Ind2,R,Ind21),
    subtract(Ind21,Sub1,Sub2),
    P3 is P2-1,
    insert(Sub1,Sub2,P3,NInd1).

% Condição de paragem de avaliação de uma população
evaluate_pop([],[]).
evaluate_pop([Ind|Other],[Ind*V|Other1]):-
    eval(Ind,V), evaluate_pop(Other,Other1).

% Avalia um individuo
eval(Seq,V):- eval2(Seq,V).

% Condição de paragem da avaliação de um individuo
eval2([_|[]],0).

eval2([C1|[C2|T]],D):-
    dist_cities(C1,C2,D1),
    eval2([C2|T],D2),
    D is D1+D2.
    

% Condição de paragem da mutação
mutation([],[]).

% Aplica a mutação de troca (Swap Mutation - Sequence Genes)
mutation([Ind|Rest],[NInd|Rest1]):-
    mutation_prob(Pmut),
    ((maybe(Pmut),!,mutation1(Ind,NInd)) ; NInd=Ind),
    mutation(Rest,Rest1).

mutation1(Ind,NInd):-
    generate_cutpoints(P1,P2),
    mutation22(Ind,P1,P2,NInd).

mutation22([G1|Ind],1,P2,[G2|NInd]):-
    !,P21 is P2-1, mutation23(G1,P21,Ind,G2,NInd).

mutation22([G|Ind],P1,P2,[G|NInd]):-
    P11 is P1-1, P21 is P2-1, mutation22(Ind,P11,P21,NInd).

mutation23(G1,1,[G2|Ind],G2,[G1|Ind]):-!.

mutation23(G1,P,[G|Ind],G2,[G|NInd]):-
    P1 is P-1,
    mutation23(G1,P1,Ind,G2,NInd).

% Indica o numero de cidades atual
numberOfCities(N):-findall(X,city(X,_,_),CT),length(CT,N).

% Ordena uma população
sort_population(P,PS):-sort(2,@=<,P,PS). %sort/4 (Elemento,Ordem,ListaNaoSorted,ListaSorted)

% Gera pontes de corte aleartorios conforme o numero de genes dos cromossomas (cidades)
generate_cutpoints(P1,P2):-cities(N),random(0,N,P1),random(0,N,P2),P1 < P2.

generate_cutpoints(P1,P2):-generate_cutpoints(P1,P2).

% Gera uma sublista conforme um indice de uma lista

sublist([_|_], 0, 0, []).

sublist([X|LS],0,P2,[X|LSB]):-
    P2 > 0,
    P21 is P2 - 1,
    sublist(LS,0,P21,LSB).

sublist([_|SL],P1,P2,LSB):-
    P1 > 0,
    P11 is P1-1,
    P21 is P2-1,
    sublist(SL,P11,P21,LSB).

% Roda os elementos de uma lista X posicoes para a esquerda
rotate_left(L,R,LR):-
    length(Back,R),
    append(Back,Front,L),
    append(Front,Back,LR),
    !.

% Roda os elementos de uma lista X posicoes para a direita
rotate_right(L,R,LR):-
    length(Back,R),
    append(Front,Back,L),
    append(Back,Front,LR),
    !.

% Insera uma sublista noutra sublista conforme um ponte de corte
insert(Sub1,Sub2,P2,NInd1):-
    append(Sub1,Sub2,Sub3),
    rotate_right(Sub3,P2,NInd1).

% Descobre a posição (indice) de um elemento numa lista
element_position(Element,[Element|_],0):-!.

element_position(Element,[_|T],N):-
    element_position(Element,T,N1),
    N is N1+1.

% Fecha um conjunto de circuitos com base na cidade inicial

finish_circuits(_,[],[]).

finish_circuits(City,[H*_|T],FinishCircuits):-
    finish_circuits(City,T,FinishCircuits1),
    generate_mc(City,H,MC),
    finish_circuit(City,MC,FinishCircuit),
    eval(FinishCircuit,FD),
    append([FinishCircuit*FD],FinishCircuits1,FinishCircuits).

% Fecha uma circuito com base numa cidade
finish_circuit(City,UFinishCircuit,FinishCircuit):-
    append(UFinishCircuit,[City],FinishCircuit).  

% Diante o melhor cromossoma de uma população, roda o caminho de modo a começar a na cidade onde é desejada
generate_mc(Cidade,MCromossoma,GMCromossoma):-
    element_position(Cidade,MCromossoma,Position),
    rotate_left(MCromossoma,Position,GMCromossoma).