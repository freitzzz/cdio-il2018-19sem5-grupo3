
% Problema do caixeiro viagante (TSP) com o uso de pesquisa exaustiva (c/ B&B)
tsp1(Orig,Cam,Custo):-
    get_time(X), % Tempo atual antes de executar a pesquisa exaustiva (Remover quando necessario)
    findall(
        (Cam1,Custo1),
        (city(C,_,_),C\==Orig,tsp1(Orig,C,Cam1,Custo1)),
        LTSP),
    sort(2,@=<,LTSP,LSTSP),
    [(Cam,Custo)|_]=LSTSP,
    get_time(Y), % Tempo depois da execução da pesquisa exaustiva
    Z is Y-X,
    W is Z,
    write("Required Time"),nl,
    write(W),write(" s"),nl,
    !.

% Executa o B&B entre uma cidade origem e destino
tsp1(Orig,Dest,Cam,Custo):-
                            findall((Cam1,Custo1) % Obtém todas as soluções do B&B
                                ,bnb(Orig,Dest,Cam1,Custo1)
                                ,AP)
                            ,rup(AP,SWP) % Das solucoes obtidas do B&B remove as que nao sao precisas (todas as que nao percorrem todas as cidades)
                            ,reverse(SWP,[(Cam,Custo)|_]) % Invertemos as solucoes possiveis e extraimos a primeira (A primeira solucao é sempre a melhor visto que o B&B)
                            ,!.

% Predicado para remover todas as possibilidades de caminho que nao sao precisas (Queremos vistar todas as cidades)
rup(AP,SWP):-sort(2,@>=,AP,SAP) % Ordenamos a lista de tuplos por ordem decrescente com o tamanho da lista de cada possibilidade
            ,[(C,_)|_]=SAP
            ,length(C,MC) % Descobrimos o numero de cidades a serem passadas (Maior length de cada lista de possibilidades)
            ,rup(SAP,SWP,MC).

rup([],[],_).

rup([(C,D)|T],SWP,TC):-length(C,LC) % Determinamos o numero de cidades da proxima possibilidade
                        ,TC==LC
                        ,rup(T,SWP1,TC)
                        ,append([(C,D)],SWP1,SWP). % Caso sejam iguais (os tamanhos) adicionados a possibilidade

rup([_|T],SWP,TC):-rup(T,SWP,TC).

% BRANCH & BOUND
bnb(Orig,Dest,Cam,Custo):- bnb2(Dest,[(0,[Orig])],Cam1,Custo1)
                            ,dist_cities(Orig,Dest,Dist)
                            ,append(Cam1,[Orig],Cam)
                            ,Custo is Custo1+Dist.
%condicao final: destino = nó à cabeça do caminho actual 

bnb2(Dest,[(Custo,[Dest|T])|_],Cam,Custo):-
    %caminho actual está invertido
    reverse([Dest|T],Cam). 

bnb2(Dest,[(Ca,LA)|Outros],Cam,Custo):-
    LA=[Act|_],
    %calcular todos os nodos adjacentes nao visitados e
    %gerar tuplos c/ um caminho novo juntado o nodo + caminho actual
    % o custo de cada caminho é o custo acumulado + peso do ramo
    findall((CaX,[X|LA]),
        (Dest\==Act,edge(Act,X,CustoX),\+ member(X,LA),CaX is CustoX + Ca),Novos),
    %os novos caminhos sao adicionados aos caminhos não explorados
    append(Outros,Novos,Todos),
    %a ordenação (não sendo o mais eficiente) garante que 
    % o melhor caminho fica na primeira posição
    sort(Todos,TodosOrd),
    %chamada recursiva
    bnb2(Dest,TodosOrd,Cam,Custo).

% Predicado que determina o peso do ramo entre cada cidade
edge(C1,C2,D):-dist_cities(C1,C2,D). 