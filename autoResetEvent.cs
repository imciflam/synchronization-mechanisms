using System;
using System.Collections.Generic;
using System.Threading;
namespace Lab3
{
    class Program
    {
        static DateTime dt1, dt2;
        static int writerNum = 3;
        static int readersNum = 3;
        static int numMessages = 10000;
        static string buffer;
        static Thread[] Writers = new Thread[writerNum];
        static Thread[] Readers = new Thread[readersNum];
        //сигнальные сообщения
        static AutoResetEvent evFull;
        static AutoResetEvent evEmpty;   
        static bool finish = false;
        static void Read(object state)
        {
            var evFull = ((object[])state)[0] as AutoResetEvent;
            var evEmpty = ((object[])state)[1] as AutoResetEvent;
            List<string> locReadArr = new List<string>(); 
            while (!finish)
            {
                evFull.WaitOne(); 
                if (finish) 
                {
                    evFull.Set(); 
                    break;
                }
                locReadArr.Add(buffer);
                evEmpty.Set(); 
            } 
        }
        static void Write(object state)
        {
            var evFull = ((object[])state)[0] as AutoResetEvent;
            var evEmpty = ((object[])state)[1] as AutoResetEvent;
            string[] locWriArr = new string[numMessages];//локальный массив писателя
            for (int j = 0; j < numMessages; j++)
                locWriArr[j] = j.ToString();
            int i = 0;
            while (i < numMessages)
            {
                evEmpty.WaitOne(); 
                buffer = locWriArr[i++];
                evFull.Set();
            } 
        }
        static void Main()
        {

            dt1 = DateTime.Now;
            evFull = new AutoResetEvent(false); 
            evEmpty = new AutoResetEvent(true); 

            for (int i = 0; i < writerNum; i++)
            {
                Writers[i] = new Thread(Write);
                Writers[i].Start(new object[] { evFull, evEmpty });
            }
            for (int i = 0; i < readersNum; i++)
            {
                Readers[i] = new Thread(Read);
                Readers[i].Start(new object[] { evFull, evEmpty });
            }
            for (int i = 0; i < writerNum; i++)
                Writers[i].Join();
            finish = true; 
            evFull.Set(); 
            for (int i = 0; i < readersNum; i++)
                Readers[i].Join();
            dt2 = DateTime.Now;
            Console.WriteLine((dt2 - dt1).TotalMilliseconds); 
            Console.ReadKey();
        }
    }
}
