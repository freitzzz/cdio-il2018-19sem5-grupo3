## 2. Identificar o nº máximo de cidades que o predicado tsp1 tem capacidade para resolver

Algoritmo de Pesquisa Exaustiva utilizado: Branch & Bound

Para resolver esta alínea, correu-se o predicado tsp1 aumentando o nº de cidades da base de conhecimento a cada iteração. Além disto, recorrendo ao predicado get_time, calculou-se o tempo que o predicado demorava a chegar à solução menos custosa.
O percurso calculado teve sempre como Origem a cidade de Bruxelas.

|Nº Cidades|Tempo(s)|Distância(m)|
|----|---|-------------|
|5|0,0077|5889474|
|6|0,026|5890401|
|7|0,179|6297948|
|8|4,14|6347199|
|9|218,17|8400202|
|10|--|------|

Tendo em conta que o algoritmo **B&B** (Branch & Bound) com 10 cidades, após 20 minutos ainda não tinha acabado a pesquisa, decidiu-se parar de testar a mesma com mais cidades. Podemos então concluir, que a partir de 10 cidades, o predicado tsp1 já não é eficiente. Isto deve-se à complexidade do algoritmo B&B ser, na notação **Big Ω(2ⁿ)** (complexidade exponencial). É de notar que a diferença de tempo entre 9 e 8 cidades (37 - 0,00946 ~ 36.99054 segundos) já é bastante significativa.
