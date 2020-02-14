using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Configuration;

namespace SNSConsole
{
    public class SNSOperation
    {
        const string TopicName = "TopicApp";
        AmazonSimpleNotificationServiceClient client;
        public BasicAWSCredentials credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);

        public SNSOperation()
        {
            client = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USWest1);
        }

        public void CreateTopic()
        {
            CreateTopicRequest request = new CreateTopicRequest
            {
                Name = TopicName
            };

            var response = client.CreateTopic(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Topic Created Successfullt! \nTopic ARN: {response.TopicArn}");
            }
        }

        public void SubscribeToTopic()
        {
            SubscribeRequest request = new SubscribeRequest
            {
                TopicArn = "arn:aws:sns:us-west-1:491483104165:TopicApp",
                Protocol = "email",
                Endpoint = "stella@rarolabs.com.br"
            };

            var response = client.Subscribe(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Subscribe Created Successfullt! \nARN: {response.SubscriptionArn}");
            }

        }

        public void PublishToTopic()
        {
            PublishRequest request = new PublishRequest
            {
                TopicArn = "arn:aws:sns:us-west-1:491483104165:TopicApp",
                Subject = "New Topic Message",
                Message = "New Message"
            };

            var response = client.Publish(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Message sent successfullt! \nMessageId: {response.MessageId}");
            }
        }

        public void ListTopics()
        {
            ListTopicsRequest request = new ListTopicsRequest
            {
            };

            var response = client.ListTopics(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                foreach (var item in response.Topics)
                {

                    Console.WriteLine($"{item.TopicArn}");
                }
            }
        }

        public void ListSubscriptionsByTopicRequest()
        {
            ListSubscriptionsByTopicRequest request = new ListSubscriptionsByTopicRequest
            {
                TopicArn = "arn:aws:sns:us-west-1:491483104165:TopicApp",
            };

            var response = client.ListSubscriptionsByTopic(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                foreach (var item in response.Subscriptions)
                {
                    Console.WriteLine($"\n\n---------- Subscription ----------\n");
                    Console.WriteLine($"{item.Owner}");
                    Console.WriteLine($"{item.Protocol}");
                    Console.WriteLine($"{item.SubscriptionArn}");
                    Console.WriteLine($"{item.Endpoint}");
                }
            }
        }

        public void Unsubscribe()
        {
            var subscription = "arn:aws:sns:us-west-1:491483104165:TopicApp:cc9e61a2-7e1a-4dfb-80da-4923749d0ae7";

            UnsubscribeRequest request = new UnsubscribeRequest
            {
                SubscriptionArn = subscription
            };

            var response = client.Unsubscribe(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Unsubscribe successfully");
            }
        }

        public void DeletedTopic()
        {
            DeleteTopicRequest request = new DeleteTopicRequest
            {
                TopicArn = "arn:aws:sns:us-west-1:491483104165:TopicApp"
            };

            var response = client.DeleteTopic(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine($"Topic has ben deleted successfullt!");
                ListTopics();
            }
        }
    }
}
