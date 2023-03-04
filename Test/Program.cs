using Tool.MyTimer;

namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyTimer timer = new MyTimer();
            timer.TimeOutEvent = TimeOut;
            timer.Interval = 10;
            timer.Start();

            MyTimer timer2 = new MyTimer();
            timer2.TimeOutEvent = TimeOut2;
            timer2.Interval = 1;
            timer2.Start();
        }

        static void TimeOut(object obj)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") +  " Timeout");
            Thread.Sleep(1000);
        }

        static void TimeOut2(object obj)
        {
            //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.ffffff") + " Timeout2");
            Thread.Sleep(2000);
        }
    }
}