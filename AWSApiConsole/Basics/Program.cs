using System.Configuration;
using Amazon.Runtime;
using Amazon.S3;
using System;

namespace Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);
            using (AmazonS3Client client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest1))
            {
                foreach (var bucket in client.ListBuckets().Buckets)
                {
                    Console.WriteLine("\nBucket: \n");
                    Console.WriteLine(bucket.BucketName + " " + bucket.CreationDate.ToShortDateString());
                }
            }
            
            Console.ReadLine();
        }
    }
}
