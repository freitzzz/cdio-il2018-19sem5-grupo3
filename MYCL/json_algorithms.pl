:- use_module(library(http/json)).
:- use_module(library(http/json_convert)).

% JSON Object with the basic information regarding an algorithm
:- json_object
        basic_algorithm(id:integer, name:string).

% JSON Object with the detailed information regarding an algorithm
:- json_object
        detailed_algorithm(id:integer, name:string).

% JSON Object with the detailed information regarding a city
:- json_object
        city_object(name:atom, latitude:number,longitude:number).

% JSON Object with the basic information regarding a city
:- json_object
        basic_city_object(name:atom).

% JSON Object with the information regarding the request cities computation body request
:- json_object
        cities_body_request(algorithmID:integer,cities:list).

% JSON Object with the information regarding the request city circuit computation body request
:- json_object
        city_circuit_body_request(algorithmID:integer,initialCity:city_object/3,cities:list).

% JSON Object with the information regarding the request cities computation body request
:- json_object
        cities_body_request(algorithmID:integer,cities:list).

% JSON Object with the information regarding the distance of a circuit
:- json_object
        distance_object(value:number,unit:atom).

% JSON Object with the information regarding the request cities computation body response
:- json_object
        cities_body_response(algorithmID:integer,citiesToTravel:list,distance:distance_object/2).

% JSON Object with the information regarding the compute shortest factory body request
:- json_object
        factories_body_request(city:city_object/3, factories:list).

% JSON Object with the information regarding the compute shortest factory body response
:- json_object
        factories_body_response(factory:city_object/3, distance:distance_object/2).


% ##############      BIN PACKING      ################


% JSON Object with the container object information
:- json_object
        container_object(width:integer,height:integer,depth:integer,weight:integer).

% JSON Object with the package object information
:- json_object
        package_object(id:integer,width:integer,height:integer,depth:integer,priorityId:integer,weight:integer).

:- json_object
        package_response_object(id:integer,x:integer,y:integer,z:integer,priorityId:integer,weight:integer).

% JSON Object with the package object information which is inserted on a certain container position
:- json_object
        container_package_object(id:integer,x:integer,y:integer,z:integer).

% JSON Object with the package object information which is inserted on a certain container position (Pretty Printing Version)
:- json_object
        container_package_pp_object(id:integer,x:integer,y:integer,z:integer,toppackages:list).

% JSON Object with the bin packing algorithm computation request body
:- json_object
        bin_packing_request(algorithmID:integer,container:container_object/4,packages:list).

% JSON Object with the bin packing algorithm computation request response
:- json_object
        bin_packing_response(maxoccupation:number,container:container_object/4,packages:list).

% JSON Object with the bin packign algoritm computation request response (Pretty Printing Version)
:- json_object
        bin_packing_pp_response(maxoccupation:number,container:container_object/3,packages:list).



% ################################### DELIVERIES PLAN #########################################

% ########### REQUESTS ###########

% JSON Object with the city information used in Delivery Plan request body
:-json_object
        delivery_plan_city_request(id:number,name:atom,latitude:number,longitude:number).


% JSON Object with the production factory information used in Delivery Plan request body
:-json_object
        delivery_plan_production_factory_request(id:number,name:atom,latitude:number,longitude:number,cityId:number).


% JSON Object with the order information used in Delivery Plan request body
:-json_object
        delivery_plan_order_request(id:number,packages:list,cityId:number,deliveryDate:atom).

% JSON Object with the package information used in Delivery Plan request body
:-json_object
        delivery_plan_package_request(id:number,width:number,height:number,depth:number,weight:number).


% JSON Object with the truck information used in Delivery Plan request body
:-json_object
        delivery_plan_truck_request(id:number,width:number,depth:number,height:number,weight:number).


% JSON Object with the city information used in Delivery Plan request body
:-json_object
        delivery_plan_request(cities:list,productionFactory:delivery_plan_production_factory_request/5,orders:list,trucks:list).


% ###################################                 #########################################


% JSON Object with a message
:- json_object
        message_object(message:string).


% Available algorithms
algorithm(1,"Branch & Bound").
algorithm(2,"Shortest Path Greedy Heuristic").
algorithm(3,"Shortest Path Greedy Heuristic Intersections Optimization").
algorithm(4,"Genetic Algorithm").
algorithm(5,"Bin Packing Guillotine First Fit Heuristic").

% Gets all available algorithms
get_available_algorithms(Alg):-
    findall(basic_algorithm(Id,Name),algorithm(Id,Name),Alg).

% Gets an algorithm by his ID
get_algorithm_by_id(Id,Alg):-
    algorithm(Id,Name),
    Alg=basic_algorithm(Id,Name).

% Gets an algorithm by his name
get_algorithm_by_name(Name,Alg):-
    algorithm(Id,Name),
    Alg=basic_algorithm(Id,Name).