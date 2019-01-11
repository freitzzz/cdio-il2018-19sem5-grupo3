# MYC
Customizing your own closet was never so easy!

## About 📜

MYC (Make Your Closet) is a system which allows users to build and integrate their desires on a customized closet

## Components 

MYC is divided in three base components:

- **MYCM** - ***Make Your Closet Products Management*** which manages users closet construction and all the products that it can be complemented with
- **MYCO** - ***Make Your Closet Orders Management*** which manages users closet orders and their states
- **MYCL** - ***Make Your Closet Logistics Management*** which serves as a complement for **MYCO** in terms of computing the best routes between the orders production and their delivery
- **MYCA** - ***Make Your Closet Authentication & Authorization*** which manages all **MYC** users authentications and authorizations

### MYCM 🗄️

MYCM provides a *delightful* **REST API** allowing users to build their closets and keep track of all possible customizations

It's infrastructure follows the **MVC** pattern and is built in **.NET** being covered with **.NET Core** *framework*

MYCM also consumes **MYCA** ***Web API*** in order to grant **users** *authorization*

The documentation for it can be found [here](https://bitbucket.org/pafomaio/cdio-il2018-19sem5-grupo3/wiki/Requirements_Engineering/MYCM_WebservicesAPI.md)

### MYCO 📦

Being built with **Node.JS**, MYCO provides a **fast** and ***reliable*** **REST API** allowing users to keep track of their orders and for managers to prepare the orders shipment

MYCO also consumes **MYCL** and **MYCA** ***Web APIs*** in order to choose the best *decisions* regarding the **orders** delivery routes and **production sites** and to grant **users** *authorization*

The documentation for it can be found [here](https://bitbucket.org/pafomaio/cdio-il2018-19sem5-grupo3/wiki/Requirements_Engineering/MYCO_WebservicesAPI.md)

### MYCL 🚚

MYCL is an independent system which allows solution approaches regarding the **TSP** (***Travelling Salesman Problem***)

It's built with **SWI-Prolog** and provices a Web API which can be consumed by everyone

The documentation for it can be found [here](https://bitbucket.org/pafomaio/cdio-il2018-19sem5-grupo3/wiki/Requirements_Engineering/MYCL_WebservicesAPI.md)

### MYCA 🕵️

Holding all logic between **MYC** **users** ***authentication*** and ***authorization***, MYCA provides a fast and secure **Web API** which grants users authorization through their *lifetime* using MYC systems

It's built with **Java** and provides an **unique** *authentication* system which can be either used by MYC **users** or **Third Parties Users (e.g. Google OAuth2.0)**

The documentation for it can be found [here](https://bitbucket.org/pafomaio/cdio-il2018-19sem5-grupo3/wiki/Requirements_Engineering/MYCA_WebservicesAPI.md)

## Deployment ☁️

MYC Components are currently deployed in [**Microsoft Azure**](https://azure.microsoft.com/en-us/)

|Component|Service|Status|
|---------|-------|------|
|[MYCM](https://mycm-api.azurewebsites.net)|Web API|Off|
|[MYCO](http://cdio-myco.westeurope.cloudapp.azure.com/)|VM|Off|
|[MYCL](http://mycl-api.ukwest.cloudapp.azure.com/)|VM|Off|
|[MYCA](https://myca-api.azurewebsites.net)|Web API|Off|

## The Team

The whole project was developed by the following soon-to-be software engineers:

- [***Gil Durão***](https://bitbucket.org/RicGD/)
- [***António Sousa***](https://bitbucket.org/i161371/)
- [***Rita Gonçalves***](https://bitbucket.org/ritagsoraia/)
- [***João Moreira***](https://bitbucket.org/JSMoreira/)
- [***Margarida Guerra***](https://bitbucket.org/MargaridaGuerra)
- [***Vera Ricardina***](https://bitbucket.org/Ricardina)
- [***João Freitas***](https://bitbucket.org/joaomagfreitas)