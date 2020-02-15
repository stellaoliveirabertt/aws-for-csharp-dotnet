using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DynamoDBConsole
{
    public class DBOperation
    {
        AmazonDynamoDBClient client;
        public BasicAWSCredentials credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);
        const string tableName = "AppTable";

        public DBOperation()
        {
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USWest1);
        }

        public void CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N"
                    },
                    new AttributeDefinition
                    {
                        AttributeName = "Username",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Username",
                        KeyType = "RANGE"
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 20,
                    WriteCapacityUnits = 10
                }
            };

            var response = client.CreateTable(request);

            if (response.HttpStatusCode.IsSuccess()) { }
            {
                Console.WriteLine("Table created successfully");
            }
        }

        public void InsertItem()
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue{ N = "3" } },
                    {"Username", new AttributeValue{ S = "admin" } }
                }
            };

            var response = client.PutItem(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Item added successfully");
            }
        }

        public void GetItem()
        {
            GetItemRequest request = new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue {N = "3"} },
                    {"Username", new AttributeValue {S = "admin"} }
                }
            };

            var response = client.GetItem(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                if (response.Item.Count > 0)
                {
                    Console.WriteLine("Item(s) retrived successfully \n");
                    foreach (var item in response.Item)
                    {
                        Console.WriteLine($"Key: {item.Key}, Value: {item.Value.N}, {item.Value.S}");
                    }
                }
                else
                {
                    Console.WriteLine("Not found! \n");
                }
            }
        }

        public void DeleteItem()
        {
            DeleteItemRequest request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue {N = "3"} },
                    {"Username", new AttributeValue {S = "admin"} }
                }
            };

            var response = client.DeleteItem(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Item(s) deleted successfully \n");
            }
        }
    }
}
