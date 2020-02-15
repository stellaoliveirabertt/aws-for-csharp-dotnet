using System.Collections.Generic;
using System.Configuration;
using Amazon.SQS.Model;
using Amazon.Runtime;
using Amazon.SQS;
using System;

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
                    Console.WriteLine($"ReceipHandle: {item.ReceiptHandle}");
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

        public void DeleteMessage()
        {
            DeleteMessageRequest request = new DeleteMessageRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/491483104165/newappqueue",
                ReceiptHandle = "AQEBX0c2vCstgyAbPoE5sjQEiXlWGfCpZi5ze6SnNQUKjsZMfEix+HM9+DdHcmA7bNvZUsema/fBIrUGDJRIVZQjsgrqLk2KTVBXNJA/Uoe4T4tjAoWwnwhxvcmJR96vkSys9se+fekvg52sTwcxQxkiQ6VNwez5q9BtoRp4LkZFTI+2YaExQDtNX3e07w0MCeffJss2ZJBz3kIo1laZCt7jnNcI02wsyJrZgupm3P0PiwVBsr07lEkzxG1rRAZ5ZAsdeT/YGYsKzGGK4sTD1wjQdw9tfX6rXsmUUFulVBX95QkaOMu/6RgINq3Q9SbKhPHU0sEDyJXg50GZhFZ9gInOd8KuP7JrZrCfQ3UEMn82js6n4hKq3MbHwpWq+vRWI4YnNZ7s4lc7Ipwr5BWhpXBX5w=="
            };

            var response = client.DeleteMessage(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Message deleted successfully");
            }
        }

        public void PurgeMessages()
        {
            PurgeQueueRequest request = new PurgeQueueRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/491483104165/newappqueue"
            };

            var response = client.PurgeQueue(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Message(s) queued successfully");
            }
        }

        public void ListQueues()
        {
            ListQueuesRequest request = new ListQueuesRequest { };

            var response = client.ListQueues(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                foreach (var item in response.QueueUrls)
                {
                    Console.WriteLine($"\nURL: {item}");
                }
            }
        }

        public void DeleteQueues()
        {
            DeleteQueueRequest request = new DeleteQueueRequest
            {
                QueueUrl = "https://sqs.us-east-1.amazonaws.com/491483104165/newappqueue"
            };

            var response = client.DeleteQueue(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"\nQueue has been deleted successfully");
            }
        }
    }
}
