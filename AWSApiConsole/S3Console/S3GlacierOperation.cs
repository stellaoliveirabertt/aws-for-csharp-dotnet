using System;
using System.Configuration;
using System.IO;
using Amazon.Glacier;
using Amazon.Glacier.Model;
using Amazon.Glacier.Transfer;
using Amazon.Runtime;

namespace S3Console
{
    public class S3GlacierOperation
    {
        AmazonGlacierClient client;
        public BasicAWSCredentials credentials = new BasicAWSCredentials(ConfigurationManager.AppSettings["accessId"], ConfigurationManager.AppSettings["secretKey"]);
        const string vaultName = "stellaoliveirabertth";

        public S3GlacierOperation()
        {
            client = new AmazonGlacierClient(credentials, Amazon.RegionEndpoint.USWest1);
        }

        public void CreateVault()
        {
            CreateVaultRequest request = new CreateVaultRequest
            {
                VaultName = vaultName,
                AccountId = "-"
            };

            var response = client.CreateVault(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("Vault Created SuccessFully!");
            }
        }

        public void UploadVaultObject()
        {
            var stream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "\\test.txt");
            UploadArchiveRequest request = new UploadArchiveRequest
            {
                VaultName = vaultName,
                AccountId = "-",
                ArchiveDescription = "test desc",
                Checksum = TreeHashGenerator.CalculateTreeHash(stream),
                Body = stream
            };

            request.StreamTransferProgress += OnUploadProgress;

            var response = client.UploadArchive(request);

            if (response.HttpStatusCode.IsSuccess())
            {
                Console.WriteLine("\nArchive Uploaded successfully");
                Console.WriteLine($"\nRequestId: {response.ResponseMetadata.RequestId}");

                foreach (var item in response.ResponseMetadata.Metadata)
                {
                    Console.WriteLine($"\n{item.Key}:{item.Value}");
                }
            }

        }

        private void OnUploadProgress(object sender, StreamTransferProgressArgs args)
        {
            Console.WriteLine($"\nPercentDone: {args.PercentDone}");
            Console.WriteLine($"\nTotal Transfer: {args.TransferredBytes}/{args.TotalBytes}");
            Console.WriteLine($"\nIncrementTransferred: {args.IncrementTransferred}");
        }

        public void DownloadArchive()
        {
            var manager = new ArchiveTransferManager(credentials, Amazon.RegionEndpoint.USWest1);
            manager.Download(vaultName, "Fwu7VQI4XKOgZkf-TzQxZeiGkB4Ker1T2P79gYwMYzRnyQaYSNC_JyKlYs99ITvFOJrqAFy1iaW8tQIZGsekEbBdyKhQ1d32vOtFraaN5u7sYXYnCGsWNZ4OC7FVWw_HHxg50TkTfA", AppDomain.CurrentDomain.BaseDirectory + "\\test-galcier.txt");
            Console.WriteLine("File downloaded successfully!");
        }
    }
}
