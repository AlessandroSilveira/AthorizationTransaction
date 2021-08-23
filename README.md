# NUBANK AUTHORIZER TRANSACTIONS 

Software to manager an account and transactions rules.

## Context

I chose Clean Architecture methodology to build the code design, I could reproduce business domain in the code as a
ubiquitous language and structure, I have so easily test code and expansive software.

## Instructions

### - Running

1) Verify if there are dependencies installed `Docker` and `Docker Compose`

2) In project root directory, run this command to create docker image from application 
   ```shell
    docker build -t authorizationtransaction -f Dockerfile .
   ```
 
   2.1) After build finish. please run the image
   ```
   docker run -ti authorizationtransaction
   ```
3) Put the transaction in the prompt to run 
   ```pyhton
{ "account": { "activeCard": true, "availableLimit": 100 } }
{ "transaction": { "merchant": "Burger King", "amount": 20, "time": "2019-02-13T10:00:00.000Z" } }
{ "transaction": { "merchant": "Habbib's", "amount": 60, "time": "2019-02-13T11:00:00.000Z" } }
    ```


