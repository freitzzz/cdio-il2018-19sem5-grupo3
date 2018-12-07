#!/bin/bash

cd webservices

mvn exec:java -Dexec.mainClass="cdiomyc.webservices.WebservicesStarter"

cd ..
