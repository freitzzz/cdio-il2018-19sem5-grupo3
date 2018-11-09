#!/bin/bash
dotnet ef migrations add InitialCreate --context MyCContext --output-dir Migrations
dotnet ef database update