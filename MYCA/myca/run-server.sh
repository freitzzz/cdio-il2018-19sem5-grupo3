#!/bin/bash

mvn clean install 

cd webservices

mvn exec:java -Dexec.mainClass="cdiomyc.webservices.WebservicesStarter"

cd ..
