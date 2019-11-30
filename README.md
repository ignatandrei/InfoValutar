# InfoValutar
Exchange rates for everyone
GitHub Actions :
![](https://github.com/ignatandrei/InfoValutar/workflows/.NET%20Core/badge.svg)

Azure Build
[![Build Status](https://dev.azure.com/ignatandrei0674/InfoValutar/_apis/build/status/ignatandrei.InfoValutar?branchName=master)](https://dev.azure.com/ignatandrei0674/InfoValutar/_build/latest?definitionId=5&branchName=master)
![Azure DevOps tests (branch)](https://img.shields.io/azure-devops/tests/ignatandrei0674/InfoValutar/5/master)
![Azure DevOps cc](https://img.shields.io/azure-devops/coverage/ignatandrei0674/InfoValutar/5/master)

Docker :
![Docker Pulls](https://img.shields.io/docker/pulls/ignatandrei/infovalutar)
docker run --rm -d -p 8080:8080 ignatandrei/infovalutar:latest
http://localhost:8080/swagger

[![Try in PWD](https://cdn.rawgit.com/play-with-docker/stacks/cff22438/assets/images/button.png)](https://labs.play-with-docker.com/?stack=https://raw.githubusercontent.com/ignatandrei/InfoValutar/master/PlayWithDocker/WebAPI.yml)
( press the Login/Start, then 8080 link . Close the session / Refresh the session page if not works  )


## Site

https://infovalutar.azurewebsites.net/

## To run the project ( for developers )

If you have Visual Studio 2019 ( at least Community ) and .NET Core 3, then run

dotnet tool restore

in the folder with InfoValutar.sln file . Then you can open InfoValutar.sln 

If you have Visual Studio Code - then install Remote Explorer extension and open the folder with InfoValutar.sln

