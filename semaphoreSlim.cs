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
        static Thread[] Writers = new Thread[writerNum];
        static Thread[] Readers = new Thread[readersNum]; 
        static SemaphoreSlim ssEmpty; 
        static bool bEmpty = true;
        static bool finish = false;
        static void Read(object o)
        {
            var ssRead = o as SemaphoreSlim;
            List<string> locReadArr = new List<string>(); 
            while (!finish)
                if (!bEmpty)
                {
                    ssRead.Wait();
                    if (!bEmpty)
                    {
                        bEmpty = true;
                        locReadArr.Add(buffer);
                    }
                    ssRead.Release();
                } 
        }
        static void Write(object o)
        {
            var ssWrit = o as SemaphoreSlim;
            string[] locWriArr = new string[numMessages]; 
            for (int j = 0; j < numMessages; j++)
                locWriArr[j] = j.ToString();
            int i = 0;
            while (i < numMessages)
                if (bEmpty)
                {
                    ssWrit.Wait();
                    if (bEmpty)
                    {
                        buffer = locWriArr[i++];
                        bEmpty = false;
                    }
                    ssWrit.Release();
                } 
        }
        static void Main()
        {
            start = DateTime.Now;
            ssEmpty = new SemaphoreSlim(1); 
            for (int i = 0; i < writerNum; i++)
            {
                Writers[i] = new Thread(Write);
                Writers[i].Start(ssEmpty);
            }
            for (int i = 0; i < readersNum; i++)
            {
                Readers[i] = new Thread(Read);
                Readers[i].Start(ssEmpty);
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
