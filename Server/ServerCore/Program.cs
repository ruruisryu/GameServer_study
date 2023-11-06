using System.Threading;

namespace ServerCore
{
    class Program
    {
        static void MainThread(object state)
        {
            for(int i=0; i<5; i++)
            {
                Console.WriteLine("Hello Thread!");
            }
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);
            
            for (int i = 0; i < 5; i++)
            {
                // TaskCreationOptions.LongRunning: 이 Task는 아주 오래 걸리는 작업임을 명시해줌으로써, Threadpool의 쓰레드를 차지하지않고 별도로 실행되도록 해줌.
                Task t = new Task(() => {while (true) { } }, TaskCreationOptions.LongRunning);
                t.Start();
            }
           
            // 정상적으로 실행됨
            ThreadPool.QueueUserWorkItem(MainThread);
        }
    }
}