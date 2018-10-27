# Makefile for building a docker image using Tsung for Stress Testing MYCO API

# Name of Docker Repository
DOCKER_REPO=myc

# Name of Docker Repository Tag
DOCKER_REPO_TAG=tsung_myco

# Dockerfile Location
DOCKERFILE=$(shell pwd)/dockerfile

# Tsung Input Location
TSUNG_INPUT=../..

help:
	@echo This makefile provides options for building and running a Docker Image using Tsung for Stress Testing MYCO API

build:
	cd $(TSUNG_INPUT) && docker build -t $(DOCKER_REPO):$(DOCKER_REPO_TAG) -f $(DOCKERFILE) .

run:
	docker run $(DOCKER_REPO):$(DOCKER_REPO_TAG)