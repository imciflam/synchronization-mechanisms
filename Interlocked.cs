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
        static int intFull = 0; 
        static int intEmpty = 1;  
        static bool finish = false;
        static void Read()
        {
            List<string> locReadArr = new List<string>(); 
            while (!finish)

                if (Interlocked.CompareExchange(ref intFull, 0, 1) == 1)
                {
                    locReadArr.Add(buffer);
                    intEmpty = 1; 
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
                if (Interlocked.CompareExchange(ref intEmpty, 0, 1) == 1) 
                {
                    buffer = locWriArr[i++];
                    intFull = 1; 
                } 
        }
        static void Main()
        {
            start = DateTime.Now;
            for (int i = 0; i < writerNum; i++)
            {
                Writers[i] = new Thread(Write);
                Writers[i].Name = i.ToString();
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