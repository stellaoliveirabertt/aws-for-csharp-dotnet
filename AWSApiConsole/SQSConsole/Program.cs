using System;

namespace SQSConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            SQSOperation sQSOperation = new SQSOperation();
            //sQSOperation.CreateSQSQueue();
            //sQSOperation.SendMessage();
            //sQSOperation.ReceivedMessage();
            //sQSOperation.SendBatchMessages();
            //sQSOperation.DeleteMessage();
            //sQSOperation.PurgeMessages();
            //sQSOperation.ListQueues();
            //sQSOperation.DeleteQueues();

            Console.ReadLine();
        }
    }
}
