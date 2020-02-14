using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using System.Configuration;
using Amazon.Runtime;
using Amazon.DynamoDBv2;

namespace DDBConsole
{
    public class DDBOperation
    {
        AmazonDynamoDBClient client;
        const string tableName = "DBDynamoDB";

        public BasicAWSCredentials credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);
        public DDBOperation()
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
                    },
                },
                BillingMode = BillingMode.PAY_PER_REQUEST
            };
        }
    }
}
