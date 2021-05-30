using System;
using System.Collections.Generic;
using System.Threading;
namespace Lab3
{
    class Program
    { 
        static DateTime start, fin;
        static int writerNum = 3;  
        static int readersNum = 3; 
        static int numMessages = 10000; 
        static string buffer;
        static bool isEmpty = true;
        static bool finish = false;
        static Thread[] Writers = new Thread[writerNum];
        static Thread[] Readers = new Thread[readersNum];  
        static void Read()
        {
            List<string> locReadArr = new List<string>(); 
            while (!finish)
                if (!isEmpty)
                {
                    lock ("read")
                    {
                        if (!isEmpty)
                        {
                            isEmpty = true;
                            locReadArr.Add(buffer);
                        }
                    }
                } 
        }
        static void Write()
        {
            string[] locWriArr = new string[numMessages];
            for (int j = 0; j < numMessages; j++)
            {
                locWriArr[j] = j.ToString();
            }
            int i = 0;
            while (i < numMessages)
                lock ("write")
                { 
                    if (isEmpty)
                    {
                        buffer = locWriArr[i++];
                        isEmpty = false;
                    }
                } 
        }
        static void Main()
        {
            start = DateTime.Now;
            for (int i = 0; i < writerNum; i++)
            {
                Writers[i] = new Thread(Write);
                Writers[i].Start();
            }
            for (int i = 0; i < readersNum; i++)
            {
                Readers[i] = new Thread(Read);
                Readers[i].Start();
            }
            for (int i = 0; i < writerNum; i++)
            {
                Writers[i].Join();
            }
            finish = true;
            for (int i = 0; i < readersNum; i++)
            {
                Readers[i].Join();
            }
            fin = DateTime.Now;
            Console.WriteLine((fin - start).TotalMilliseconds);
            Console.ReadKey();
        }
    }
}
