:-set_prolog_flag(answer_write_options,[quoted(true),portray(true),spacing(next_argument)]).

:- dynamic city/4. % Requires city/4 to be dynamic
:- dynamic fabrica/4. % Requires fabrica/4 to be dynamic
:- dynamic cidade/4. % Requires cidade/4 to be dynamic
:- dynamic encomenda/3. % Requires encomenda/3 to be dynamic
:- dynamic caixa/6. % Requires caixa/6 to be dynamic


%fabrica(Id,Name,Latitude,Longitude).
%cidade(Id,Nome,Latitude,Longitude).
%encomenda(Id,CidadeId,Data).
%caixa(Id,EncomendaId,Width,Height,Depth,Weight).

% Loads required algorithms
carregar_delivery:-
    carregar_tsp,
    carregar_packing.
    

% Loads TSP solutions
carregar_tsp:-
    carregar_tsp_utilities,
    carregar_bb,
    carregar_greedy,
    carregar_2opt,
    carregar_genetics.

% Loads required TSP utilities
carregar_tsp_utilities:-['tsp/cdio-tsp.pl','tsp/intersept.pl','tsp/parameters.pl'].

% Loads Branch & Bound heuristic algorithm (TSP #1)
carregar_bb:-['tsp/branch_and_bound.pl'].

% Loads Greedy heuristic algorithm (TSP #2)
carregar_greedy:-['tsp/greedy.pl'].

% Loads Greedy heuristic algorithm 2cuts-optimization (TSP #3)
carregar_2opt:-['tsp/2opt.pl'].

% Loads Genetics heuristic algorithm (TSP #4)
carregar_genetics:-['tsp/genetics.pl'].


% Loads Bin Packing solutions
carregar_packing:-
    carregar_guillotine,
    carregar_simulated_annealing.

% Loads Guillotine Heuristic (First Fit)
carregar_guillotine:-['packing/guillotine_packing.pl'].

% Loads Simulated Annealing (W/Guillotine Packing)
carregar_simulated_annealing:-['packing/simulated_annealing.pl'].

%calculate_priority(Rota,LP):-
    

% Transforms all kb package facts in a package tuple list L([(ID,W,H,D,WE,P)])
package_facts_to_package_list(L):-
    findall((ID,W,H,D,WE),package(ID,W,H,D,WE),L).








%fabrica(Id,Latitude,Longitude,CityId?).
%cidade(Id,Nome,Latitude,Longitude).
%encomenda(Id,CidadeId,Data).
%caixa(Id,EncomendaId,Width,Height,Depth,Weight).


% 1) Agrupar encomendas
agrupar_encomendas(EA):-
    findall(cidade(Id,Nome,Latitude,Longitude),cidade(Id,Nome,Latitude,Longitude),Cidades),
    agrupar_encomendas(Cidades,EA).

agrupar_encomendas([],[]):-!.

agrupar_encomendas([H|T],EA):-
    H=cidade(Id,Nome,Latitude,Longitude),
    findall((OrderId,Id,Data),encomenda(OrderId,Id,Data),LO),
	agrupar_encomendas(T,EA1),
	(LO\==[],
    sort(2,@=<,LO,[(_,_,DM)|_]),
    append([((Id,Nome,Latitude,Longitude),DM,LO)],EA1,EA);true).


% 2) Calcular peso

calc_peso(Cidade,Fabrica,Data,Peso):-
    (_,LAC,LOC,_)=Cidade,
    (_,LAF,LOF)=Fabrica,
    distance(LAC,LOC,LAF,LOF,Dist),
    parse_time(Data,DataValue),
    Peso is Dist*DataValue.


assert_city_facts:-
    agrupar_encomendas(EA),
    assert_city_facts(EA).

assert_city_facts([]):-!.

assert_city_facts([H|T]):-
    H=((_,Nome,Latitude,Longitude),DM,_),
    assert(city(Nome,Latitude,Longitude,DM)),
    assert_city_facts(T).
    


% 4) Inverter caminho e obter prioridades

caminho_as_prioridades([],[]):-!.

caminho_as_prioridades([_|[]],[]):-!.

caminho_as_prioridades([H|T],CaminhoPriodades):-
    length(T,PID),
    caminho_as_prioridades(T,CaminhoPriodades1),
    append([(H,PID)],CaminhoPriodades1,CaminhoPriodades).


%fabrica(Id,Latitude,Longitude,CityId?).
%cidade(Id,Nome,Latitude,Longitude).
%encomenda(Id,CidadeId,Data).
%caixa(Id,EncomendaId,Width,Height,Depth,Weight).


% 5) Associar prioridade a pacotes

associar_prioridade_pacotes([],[]).

associar_prioridade_pacotes([H|T],ListaPacotes):-
    (NomeCidade,PID)=H,
    cidade(CidadeId,NomeCidade,_,_),
    findall(EncomendaId,encomenda(EncomendaId,CidadeId,_),ListaEncomendas),
    associar_prioridade_pacotes(T,ListaPacotes1),
    associar_prioridade_pacotes_encomendas(ListaEncomendas,ListaPacotes2,PID),
    append(ListaPacotes2,ListaPacotes1,ListaPacotes).


% Recursive predicate for all packages of a set of orders

associar_prioridade_pacotes_encomendas([],[],_):-!.

associar_prioridade_pacotes_encomendas([EncomendaId|T],ListaPacotes,PID):-
    % (Id,W,H,D,WE,PID) => Package Tuple required by simulated annealing
    findall((CaixaId,Width,Height,Depth,Weight,PID),caixa(CaixaId,EncomendaId,Width,Height,Depth,Weight),ListaPacotes2),
    associar_prioridade_pacotes_encomendas(T,ListaPacotes1,PID),
    append(ListaPacotes2,ListaPacotes1,ListaPacotes).

% Fills a set of trucks with packages

fill_camioes([],_,[]):-!.

fill_camioes(_,[],[]):-!.


fill_camioes([H|T],Pacotes,FilledCamioes):-
    (TruckId,TruckWidth,TruckHeight,TruckDepth,TruckWeight)=H,
    simulated_annealing((TruckWidth,TruckHeight,TruckDepth,TruckWeight),Pacotes,PO,RL,NRL),
    remove_filled_packages(RL,Pacotes,NewPacotes),
    fill_camioes(T,NewPacotes,FilledCamioes1),
    append([(H,PO,RL,NRL)],FilledCamioes1,FilledCamioes).


% Removes already filled packages from the initial package list

remove_filled_packages(_,[],[]):-!.

remove_filled_packages(Pacotes,[H|T],NewPacotes):-
    (
        (PacoteId,_,_,_,_,_)=H,
        not(member((PacoteId,_,_,_,_,_),Pacotes)),
        remove_filled_packages(Pacotes,T,NewPacotes1),
        append([H],NewPacotes1,NewPacotes),!
    ;
        remove_filled_packages(Pacotes,T,NewPacotes),
        !
    ).


%fabrica(Id,Name,Latitude,Longitude).
%cidade(Id,Nome,Latitude,Longitude).
%encomenda(Id,CidadeId,Data).
%caixa(Id,EncomendaId,Width,Height,Depth,Weight).


% Asserts a list of cities as cidades facts

assert_cidades([]):-!.

assert_cidades([H|T]):-
    (Id,Name,Latitude,Longitude)=H,
    assert(cidade(Id,Name,Latitude,Longitude)),
    assert_cidades(T).

% Asserts a list of orders as encomendas facts

assert_encomendas([]):-!.

assert_encomendas([H|T]):-
    (Id,Packages,CityId,DeliveryDate)=H,
    assert(encomenda(Id,CityId,DeliveryDate)),
    assert_caixas(Packages,Id),
    assert_encomendas(T).

% Asserts a list of packages as caixas facts of a certain order

assert_caixas([],_):-!.

assert_caixas([H|T],OrderId):-
    (Id,Width,Height,Depth,Weight)=H,
    assert(caixa(Id,OrderId,Width,Height,Depth,Weight)),
    assert_caixas(T,OrderId).



% Plans a delivery trip using Branch & Bound heuristic
plan(Cities,ProductionFactory,Orders,Trucks,TruckPathList,1):-
    assert_cidades(Cities),
    (_,Initial,FLatitude,FLongitude,_)=ProductionFactory,
    get_time(X),stamp_date_time(X,Date,'UTC'),format_time(atom(RealDate),'%FT%T%z',Date,posix),
    assert_encomendas(Orders),
	assert(city(Initial,FLatitude,FLongitude,RealDate)),
    assert_city_facts,
    assert_cidades([(0,Initial,FLatitude,FLongitude)]),
    (_,Initial,_,_)=ProductionFactory,
	
    fill_truck(Initial,Trucks,[],TruckPathList),
	
    %fill_camioes(Trucks,LPP,FilledCamioes),
    %filled_trucks_to_pretty_tuples(FilledCamioes,TrucksPlan),
    retractFacts,
    !. % 6) Correr o algoritmo de empacotamento (C/ Simulated Annealing)
	
	
fill_truck(_,[],TPL,TPL).
fill_truck(Initial,[Truck|T],TPL,RTPL):-tspd1(Initial,L,_), % 3) Determinar caminho da rota para obtenção das prioridades
		caminho_as_prioridades(L,LP), % 4) Inverter o caminho da rota de modo a atribuir uma prioridade a cada uma das cidades
		associar_prioridade_pacotes(LP,LPP), % 5) Atribuir as prioridades a pacotes 
		(TruckId,TruckWidth,TruckHeight,TruckDepth,TruckWeight)=Truck,
		simulated_annealing((TruckWidth,TruckHeight,TruckDepth,TruckWeight),LPP,PO,RL,NRL),
		remove_filled_packages(RL,LPP,NewPacotes),
		truck_route_to_pretty_tuples(L,TrucksRoute),
		append(TPL,[(Truck,PO,RL,NRL,TrucksRoute)],PL),
		fill_truck(Initial,T,PL,RTPL).
		



% Plans a delivery trip using Greedy heuristic
plan(Initial,FilledCamioes,2):-
    tspd2(Initial,L,_), % 3) Determinar caminho da rota para obtenção das prioridades
    caminho_as_prioridades(L,LP), % 4) Inverter o caminho da rota de modo a atribuir uma prioridade a cada uma das cidades
    associar_prioridade_pacotes(LP,LPP), % 5) Atribuir as prioridades a pacotes 
    fill_camioes([(200,200,200,200),(200,200,200,200),(200,200,200,200)],LPP,FilledCamioes),!. % 6) Correr o algoritmo de empacotamento (C/ Simulated Annealing)

% (ID,X,Y,Z,WE,PID)

% Plans a delivery trip using Branch & Bound heuristic
plan(Initial,FilledCamioes,3):-
    tspd3(Initial,_,L), % 3) Determinar caminho da rota para obtenção das prioridades
    caminho_as_prioridades(L,LP), % 4) Inverter o caminho da rota de modo a atribuir uma prioridade a cada uma das cidades
    associar_prioridade_pacotes(LP,LPP), % 5) Atribuir as prioridades a pacotes 
    fill_camioes([(200,200,200,200),(200,200,200,200),(200,200,200,200)],LPP,FilledCamioes),!. % 6) Correr o algoritmo de empacotamento (C/ Simulated Annealing)

% Plans a delivery trip using Branch & Bound heuristic
plan(Initial,FilledCamioes,4):-
    tspd4(Initial,L,_), % 3) Determinar caminho da rota para obtenção das prioridades
    caminho_as_prioridades(L,LP), % 4) Inverter o caminho da rota de modo a atribuir uma prioridade a cada uma das cidades
    associar_prioridade_pacotes(LP,LPP), % 5) Atribuir as prioridades a pacotes 
    fill_camioes([(200,200,200,200),(200,200,200,200),(200,200,200,200)],LPP,FilledCamioes),!. % 6) Correr o algoritmo de empacotamento (C/ Simulated Annealing)


%fabrica(Id,Name,Latitude,Longitude).
%cidade(Id,Nome,Latitude,Longitude).
%encomenda(Id,CidadeId,Data).
%caixa(Id,EncomendaId,Width,Height,Depth,Weight).



plan(_,_,_,_,_,_,_):-
    retractFacts.
    


retractFacts:-
    abolish(city/4),
    abolish(cidade/4),
    abolish(encomenda/3),
    abolish(caixa/6),
    abolish(city/4).
















% Parses a list of filled trucks into a list of filled trucks as tuples

filled_trucks_to_pretty_tuples([],[]):-!.

filled_trucks_to_pretty_tuples([H|T],FilledTrucksTuples):-
    ((TruckId,TruckWidth,TruckHeight,TruckDepth,TruckWeight),PO,RL,_)=H,
    filled_packages_to_pretty_tuples(RL,FilledPackagesTuples),
    filled_trucks_to_pretty_tuples(T,FilledTrucksTuples1),
    get_time(X),stamp_date_time(X,Date,'UTC'),format_time(atom(RealDate),'%FT%T%z',Date,posix),
    append([(TruckId,TruckWidth,TruckHeight,TruckDepth,TruckWeight,PO,RealDate,FilledPackagesTuples)],FilledTrucksTuples1,FilledTrucksTuples).



% Parses a list of filled packages into a list of filled packages as tuples

filled_packages_to_pretty_tuples([],[]):-!.

filled_packages_to_pretty_tuples([H|T],FilledPackagesTuples):-
    (PID,PX,PY,PZ,_,_)=H,
    filled_packages_to_pretty_tuples(T,FilledPackagesTuples1),
    append([(PID,PX,PY,PZ)],FilledPackagesTuples1,FilledPackagesTuples).




% Parses a truck route into pretty tuples

truck_route_to_pretty_tuples([],[]):-!.

truck_route_to_pretty_tuples([H|T],TruckRouteTuples):-
    cidade(CidadeId,H,CidadeLatitude,CidadeLongitude),
    truck_route_to_pretty_tuples(T,TruckRouteTuples1),
    append([(CidadeId,H,CidadeLatitude,CidadeLongitude)],TruckRouteTuples1,TruckRouteTuples).