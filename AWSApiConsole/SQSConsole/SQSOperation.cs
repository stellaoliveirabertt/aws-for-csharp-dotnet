
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SQSConsole
{
    public class SQSOperation
    {
        const string QueueName = "newappqueue";
        AmazonSQSClient client;
        public BasicAWSCredentials credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);

        public SQSOperation()
        {
            client = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast1);
        }

        public void CreateSQSQueue()
        {
            CreateQueueRequest request = new CreateQueueRequest
            {
                QueueName = QueueName
            };

            var response = client.CreateQueue(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"SQS Queue Created Successfully!");
                Console.WriteLine($"SQS Queue URL: {response.QueueUrl}");
            }
        }

        public void SendMessage()
        {
            SendMessageRequest request = new SendMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/491483104165/newappqueue",
                MessageBody = "Message Queue Body"
            };

            var response = client.SendMessage(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Message Sent Successfully! \nMessageId: {response.MessageId}");
            }
        }

        public void ReceivedMessage()
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/491483104165/newappqueue"
            };

            var response = client.ReceiveMessage(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Message(s) Received Successfully");

                foreach (var item in response.Messages)
                {
                    Console.WriteLine($"Message Content: {item.Body}");
                    Console.WriteLine($"MessageId: {item.MessageId}");
                }
            }
        }

        public void SendBatchMessages()
        {
            SendMessageBatchRequest request = new SendMessageBatchRequest();
            request.QueueUrl = "https://sqs.us-east-1.amazonaws.com/491483104165/newappqueue";

            request.Entries = new List<SendMessageBatchRequestEntry>
            {
                new SendMessageBatchRequestEntry{Id = Guid.NewGuid().ToString(), MessageBody = "First Message"},
                new SendMessageBatchRequestEntry{Id = Guid.NewGuid().ToString(), MessageBody = "Second Message"}
            };

            var response = client.SendMessageBatch(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Message queued successfully");
                foreach (var item in response.Successful)
                {
                    Console.WriteLine($"\nID: {item.Id} \nMessageId: {item.MessageId}");
                }
            }
        }
    }
}
