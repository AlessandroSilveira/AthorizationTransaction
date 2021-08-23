# NUBANK AUTHORIZER TRANSACTIONS 

Software to manager an account and transactions rules.

## Context

I chose Clean Architecture, Repository Pattern methodologies to build the code design, I could reproduce the business domain in the code as a
ubiquitous language and structure, easily testable code, and expansive, maintainable, and decoupled software.

- [Net 5] (https://dotnet.microsoft.com/download/dotnet/5.0): Single, unified platform for creating applications that run on all platforms (Windows, Linux) and devices (IoT, Mobile).
- [EntitiFramework Core InMemory](https://docs.microsoft.com/pt-br/ef/core/): Database provider allows the Entity Framework Core to be used with an in-memory database. 
- [NewtonSoft Json](https://www.newtonsoft.com/json): Deserializer and Serializer Json
- [NUnit](https://nunit.org/): Test software
- [Moq](https://github.com/Moq/moq4/wiki/Quickstart): Mock objects in tests

## Instructions

### - Running

1) Verify if there are dependencies installed `Docker` and `Docker Compose`

2) In project root directory, run this command to create docker image from application 
   ```shell
    docker build -t authorizationtransaction -f Dockerfile .
   ```
 
   2.1) After build finish. please run the image:
   ```
   docker run -ti authorizationtransaction
   ```
3) Put the transaction in the prompt to run: 
   
{ "account": { "activeCard": true, "availableLimit": 100 } }
{ "transaction": { "merchant": "Burger King", "amount": 20, "time": "2019-02-13T10:00:00.000Z" } }
{ "transaction": { "merchant": "Habbib's", "amount": 60, "time": "2019-02-13T11:00:00.000Z" } }
   


