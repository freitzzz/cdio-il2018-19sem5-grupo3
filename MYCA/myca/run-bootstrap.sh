#!/bin/bash

mvn clean install 

cd bootstrapper

mvn exec:java -Dexec.mainClass="cdiomyc.bootstrapper.Bootstrapper"

cd ..
