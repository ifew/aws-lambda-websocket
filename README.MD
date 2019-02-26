# Example using .Net Core 2 Web Socket with AWS Lambda

This implement based from https://github.com/normj/netcore-simple-websockets-chat-app

## Must have

- Amazon.Lambda.APIGatewayEvents Version 1.2.0 (This only version to support API Gateway Web Socket)

## Setup

1. Create Environment Variables
- key: TEST_LAMBDA_DBCONNECTION 
- value: server={HOST};userid={USERNAME};password={PASSWORD};database={DATABASE_NAME};convert zero datetime=True; CharSet=utf8;

2. Push to AWS Lambda

## Acknowledge
- I don't know how to create Unit Test in SendMessage Function (Only SendMessage Function not have Unit Test)
- I don't know how to test on localhost