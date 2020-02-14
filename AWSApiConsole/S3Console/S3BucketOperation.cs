using System.Collections.Generic;
using System.Threading.Tasks;
using System.Configuration;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Amazon.Runtime;
using System.IO;
using Amazon.S3;
using System;

namespace S3Console
{
    public class S3BucketOperation : IDisposable
    {

        AmazonS3Client client;
        const string bucketName = "stellaoliveirabertth";
        public BasicAWSCredentials credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);

        public S3BucketOperation()
        {
            client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest1);
        }

        public void Create()
        {
            {
                if (!AmazonS3Util.DoesS3BucketExistV2(client, "stellaoliveirabertth"))
                {
                    var bucket = new PutBucketRequest { BucketName = "stellaoliveirabertth", UseClientRegion = true };
                    var bucketResponse = client.PutBucket(bucket);

                    if (bucketResponse.HttpStatusCode.IsSuccess())
                    {
                        Console.WriteLine("Bucket Created Success");
                    }
                }
                else
                {
                    Console.WriteLine("Bucket exists!");
                }
            }

            Console.ReadLine();
        }

        public void UploadFile()
        {
            var transfereUtil = new TransferUtility(client);
            //transfereUtil.UploadDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\files", bucketName);
            //transfereUtil.Upload(AppDomain.CurrentDomain.BaseDirectory + "\\test.txt", bucketName);
            var fileTransferRequest = new TransferUtilityUploadRequest
            {
                FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\test.txt",
                CannedACL = S3CannedACL.PublicRead,
                BucketName = bucketName
            };

            transfereUtil.Upload(fileTransferRequest);
            Console.WriteLine("File Uploaded Successfully !");
        }

        public async Task DownloadFileAsync()
        {
            string content = string.Empty;
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = "test.txt"
            };

            using (GetObjectResponse response = await client.GetObjectAsync(request))
            {
                using (Stream responseStream = response.ResponseStream)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string contentType = response.Headers["Content-Type"];
                        content = reader.ReadToEnd();
                        Console.WriteLine($"\nFile Content-Type: {contentType} \n File Content: {content}");
                    }
                }
            }

            Console.WriteLine("File Uploaded Successfully !");
        }

        public void GeneratePreSignedUrl()
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = "test.txt",
                Expires = DateTime.Now.AddMinutes(1),
            };

            var url = client.GetPreSignedURL(request);
            Console.WriteLine($"Sharing URL: {url}");
        }

        public void GetObjectTagging()
        {
            GetObjectTaggingRequest tagRequest = new GetObjectTaggingRequest
            {
                BucketName = bucketName,
                Key = "test.txt"
            };

            GetObjectTaggingResponse objectTags = client.GetObjectTagging(tagRequest);
            if (objectTags.Tagging.Count > 0)
            {
                foreach (var tag in objectTags.Tagging)
                {
                    Console.WriteLine($"Key: {tag.Key} \n Value: {tag.Value}");
                }
            }
            else
            {
                Console.WriteLine("\nNo Tags Found!\n");
            }
        }

        public void UpdateObjectTagging()
        {
            GetObjectTagging();
            Tagging tags = new Tagging();
            tags.TagSet = new List<Tag>
            {
                new Tag { Key = "Key1", Value = "Value1"},
                new Tag { Key = "Key2", Value = "Value2" },
                new Tag {Key = "Key3", Value = "Value3"}
            };

            PutObjectTaggingRequest request = new PutObjectTaggingRequest
            {
                BucketName = bucketName,
                Key = "test.txt",
                Tagging = tags
            };

            PutObjectTaggingResponse response = client.PutObjectTagging(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Object Tags updated successfully");
            }

            GetObjectTagging();
        }

        public void UpdateObjectACL()
        {
            PutACLRequest request = new PutACLRequest
            {
                BucketName = bucketName,
                Key = "test.txt",
                CannedACL = S3CannedACL.PublicRead
            };

            var response = client.PutACL(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Object ACL Update Successfully");
            }
        }

        public void BucketVersioning()
        {
            PutBucketVersioningRequest request = new PutBucketVersioningRequest
            {
                BucketName = bucketName,
                VersioningConfig = new S3BucketVersioningConfig
                {
                    EnableMfaDelete = false,
                    Status = VersionStatus.Enabled
                }
            };

            var response = client.PutBucketVersioning(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Bucket versioning successful !");
            }
        }

        public void BucketAccelerate()
        {
            PutBucketAccelerateConfigurationRequest request = new PutBucketAccelerateConfigurationRequest
            {
                BucketName = bucketName,
                AccelerateConfiguration = new AccelerateConfiguration
                {
                    Status = BucketAccelerateStatus.Enabled
                }
            };
            var response = client.PutBucketAccelerateConfiguration(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Bucket accelerate successful !");
            }
        }

        public void Dispose() => client.Dispose();
    }
}
