# Example using .Net Core 2 Web Socket with AWS Lambda

This implement based from 

- https://github.com/normj/netcore-simple-websockets-chat-app
- https://github.com/radu-matei/websocket-manager

## Must have

- Amazon.Lambda.APIGatewayEvents Version 1.2.0 (This only version to support API Gateway Web Socket)

## Setup

1. Create Environment Variables
- key: TEST_LAMBDA_DBCONNECTION 
- value: server={HOST};userid={USERNAME};password={PASSWORD};database={DATABASE_NAME};convert zero datetime=True; CharSet=utf8;

2. Push to AWS Lambda

4. test on Web Socket Test Tools such as https://chrome.google.com/webstore/detail/simple-websocket-client/pfdhoblngboilpfeibdedpjgfnlcodoo?hl=en

5. connect and send data such as
```json
{"action":"sendmessage","data":"hello"}
```

## Acknowledge
- I don't know how to test on localhost
