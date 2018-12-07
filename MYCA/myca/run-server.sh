#!/bin/bash

cd webservices

mvn clean install && mvn exec:java -Dexec.mainClass="cdiomyc.webservices.WebservicesStarter"

cd ..
