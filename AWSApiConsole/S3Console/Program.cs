using System.Configuration;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Amazon.Runtime;
using Amazon.S3;
using System;

namespace S3Console
{
    class Program
    {
        static void Main(string[] args)
        {
            #region [S3 - BucketOperation]
            //S3BucketOperation s3BucketOperation = new S3BucketOperation();
            //s3BucketOperation.UploadFile();
            //#pragma warning disable CS4014 // Como esta chamada não é esperada, a execução do método atual continua antes de a chamada ser concluída
            //s3BucketOperation.DownloadFileAsync();
            //#pragma warning restore CS4014
            //s3BucketOperation.GeneratePreSignedUrl();
            //s3BucketOperation.GetObjectTagging();
            //s3BucketOperation.UpdateObjectTagging();
            //s3BucketOperation.UpdateObjectACL();
            //s3BucketOperation.BucketVersioning();
            //s3BucketOperation.BucketAccelerate();
            #endregion

            #region [S3 - GlacierOperation]
            //S3GlacierOperation s3GlacierOperation = new S3GlacierOperation();
            //s3GlacierOperation.CreateVault();
            //s3GlacierOperation.UploadVaultObject();
            //s3GlacierOperation.DownloadArchive();
            #endregion

            Console.ReadLine();
        }
    }
}
