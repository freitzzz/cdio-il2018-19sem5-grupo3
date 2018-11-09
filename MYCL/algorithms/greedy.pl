%Caminho heurístico (vizinho mais próximo)

tsp2(Orig,L,DistanciaTotal):- todasAsCidades(NCidades), NCPorVisitar is NCidades - 1, %Começa-se por descobrir o nº de cidades (sem considerar a origem)
													percorrerCircuito(Orig,L1,NCPorVisitar,DistanciaTotal1,[Orig]),  %De seguida percorremos o circuito hamiltoniano
													reverse(L1,[H|_]), %Obtemos a ultima cidade a ser percorrida
													dist_cities(H,Orig,Distancia), %Calculamos a distancia entre essa cidade e a origem
													DistanciaTotal is DistanciaTotal1 + Distancia, %Calculo da distancia final com base na distancia do circuito hamiltoniano com a distancia do destino à origem
													append([Orig],L1,L2), %Adicionamos a origem à lista do circuito hamiltoniano
													append(L2,[Orig],L),%Finalmente,adicionamos a origem à cauda do caminho hamiltoniano
													!.%Cut no final serve para evitar encontrar mais soluções

todasAsCidades(NCidades):- findall(X, city(X, _, _), L), length(L, NCidades). %Percorre a BC e retorna o número de cidades existente

percorrerCircuito(_,[],0,0,_). %Condição de paragem: Quando o número de cidades por visitar for 0, foi alcançado o objetivo
percorrerCircuito(Orig,Lista,NCPorVisitar,DistanciaTotal,ListaVisitados):-findall((Distancia,ProxNo),temLigacao(Orig,ProxNo,ListaVisitados,Distancia),L), %Encontramos todos os destinos possíveis a partir de um determinado nó (sem repetição)
								sort(L,[(D,C)|_]),%Ordenamos pela distância
								NCPorVisitar1 is NCPorVisitar-1, %Decrementamos o numero de cidades por visitar
								percorrerCircuito(C,Lista1,NCPorVisitar1,DistanciaTotal1,[C|ListaVisitados]), %Recursivamente são encontradas as próximas soluções a partir da cidade mais próxima (menor distância)
								DistanciaTotal is DistanciaTotal1 + D,%Somamos à distância total a distância do caminho.
								append([C],Lista1,Lista).%Adicionamos a cidade à lista

temLigacao(Orig,ProxNo,L,Distancia):- dist_cities(Orig,ProxNo,Distancia),not(member(ProxNo,L)). %Predicado que calcula a distancia entre dois nós mas verifica se o nó destino já foi visitado

