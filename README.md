# MYC
Customizing your own closet was never so easy!

## About 📜

MYC (Make Your Closet) is a system which allows users to build and integrate their desires on a customized closet

## Components 

MYC is divided in three base components:

- **MYCM** - ***Make Your Closet Products Management*** which manages users closet construction and all the products that it can be complemented with
- **MYCO** - ***Make Your Closet Orders Management*** which manages users closet orders and their states
- **MYCL** - ***Make Your Closet Logistics Management*** which serves as a complement for **MYCO** in terms of computing the best routes between the orders production and their delivery

### MYCM 🗄️

MYCM provides a *delightful* **REST API** allowing users to build their closets and keep track of all possible customizations

It's infrastructure follows the **MVC** pattern and is built in **.NET** being covered with **.NET Core** *framework*

The documentation for it can be found [here](https://bitbucket.org/pafomaio/cdio-il2018-19sem5-grupo3/wiki/Requirements_Engineering/MYCM_WebservicesAPI.md)

### MYCO 📦

Being built with **Node.JS**, MYCO provides a **fast** and ***reliable*** **REST API** allowing users to keep track of their orders and for managers to prepare the orders shipment

MYCO also consumes **MYCL** ***Web API*** in order to choose the best decisions regarding the orders delivery routes and production sites

The documentation for it can be found [here](https://bitbucket.org/pafomaio/cdio-il2018-19sem5-grupo3/wiki/Requirements_Engineering/MYCO_WebservicesAPI.md)

### MYCL 🚚

MYCL is an independent system which allows solution approaches regarding the **TSP** (***Travelling Salesman Problem***)

It's built with **SWI-Prolog** and provices a Web API which can be consumed by everyone

The documentation for it can be found [here](https://bitbucket.org/pafomaio/cdio-il2018-19sem5-grupo3/wiki/Requirements_Engineering/MYCL_WebservicesAPI.md)

## Deployment ☁️

MYC Components are currently deployed in [**Microsoft Azure**](https://azure.microsoft.com/en-us/)

|Component|Service|Status|
|---------|-------|------|
|[MYCM](https://mycm-api.azurewebsites.net)|Web API|Off|
|[MYCO](http://cdio-myco.westeurope.cloudapp.azure.com/)|VM|Off|
|[MYCL](http://mycl-api.ukwest.cloudapp.azure.com/)|VM|Off|