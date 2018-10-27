# Makefile for building a docker image using Tsung for all MYC API's

# Name of Docker Repository
DOCKER_REPO=myc

# Name of Docker Repository Tag
DOCKER_REPO_TAG=tsung

# Dockerfile Location
DOCKERFILE=$(shell pwd)/dockerfile

# Tsung Input Location
TSUNG_INPUT=../..

help:
	@echo This makefile provides options for building a Docker Image using Tsung for all MYC APIs

build:
	cd $(TSUNG_INPUT) && docker build -t $(DOCKER_REPO):$(DOCKER_REPO_TAG) -f $(DOCKERFILE) .