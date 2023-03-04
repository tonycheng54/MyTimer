using System.Diagnostics;

namespace Tool.MyTimer
{
    public class TimerException : Exception
    {
        public TimerException(string? message) : base(message)
        {
        }
    }
    public class MyTimer
    {
        public delegate Task TimeOutDelegateAsync(object obj);
        public delegate void TimeOutDelegate(object obj);

        public TimeOutDelegate TimeOutEvent;
        public TimeOutDelegateAsync TimeOutEventAsync;

        private Stopwatch _Sw = new Stopwatch();
        private string _TimerName;

        private int _Interval;

        public int Interval
        {
            get { return _Interval; }
            set { _Interval = value; }
        }


        public string Name
        {
            get { return _TimerName; }
        }

        private Thread _TimerProcThread;


        public MyTimer(string name = null)
        {
            _TimerName = name; 
        }

        public void Start()
        {
            if(_Interval <= 0)
            {
                throw new Exception("Interval not setting.");
            }

            _TimerProcThread = new Thread(TimerProcess);
            _TimerProcThread.Start(this);

        }

        private void TimerProcess(object obj)
        {
            double duration = _Interval;
            if (_Sw.IsRunning == false)
                _Sw.Start();
            while (true)
            {
                Spin(_Sw, duration);

                Thread t = new Thread(Callback);
                t.Start(obj);
            }
        }

        private void Callback(object obj)
        {
            if (TimeOutEvent != null)
                TimeOutEvent(obj);
        }

        private async Task CallbackAsync(object obj)
        {
            if (TimeOutEvent != null)
                await TimeOutEventAsync(obj);
        }

        private static async void Spin(Stopwatch w, double duration/*, object obj*/)
        {
            var current = w.ElapsedMilliseconds;
            while ((w.ElapsedMilliseconds - current) < duration)
                Thread.SpinWait(10);
        }
    }

    /*
    public class MyTimer1
    {

        public delegate void TimerEventDelegate(object obj);
        public event TimerEventDelegate TimerEvent;

        private Stopwatch _Sw = new Stopwatch();
        private string _Name;

        public string Name
        {
            get { return _Name; }
        }

        private int _Interval;

        public int Interval
        {
            get { return _Interval; }
            set { _Interval = value; }
        }

        private bool _TimerStart = false;

        private Thread _TimerProcThread;

        public MyTimer1(string name = null)
        {
            if (name != null)
                _Name = name;

        }
        public void Start()
        {
            if (_Interval == 0)
                throw new TimerException("未設定Interval");
            _TimerStart = true;
            _TimerProcThread = new Thread(TimerProcess);
            _TimerProcThread.Start(this);

        }

        /// <summary>
        /// 停止Timer
        /// </summary>
        public void Stop()
        {
            _TimerStart = false;
            this.TimerEvent = null;
        }

        private void TimerProcess(object obj)
        {
            double duration = _Interval;
            if (_Sw.IsRunning == false)
                _Sw.Start();
            DateTime start, end;
            while (true)
            {
                start = DateTime.Now;
                Spin(_Sw, duration, obj);
                //Task.Run(() => Callback(obj));

                Thread t = new Thread(Callback);
                t.Start(obj);

                end = DateTime.Now;
                //Console.WriteLine(Name +  " cost: " + (end - start).TotalMinutes);
                //var diff = (end - start).TotalMilliseconds - _Interval;
                //var tmp = _Interval - diff;
                //duration = (_Interval - diff) > 0 ? (_Interval - diff) : 0.001;
                //var feedback = (end - start).TotalMinutes - _Interval;
                //duration = (_Interval - feedback) > 0 ? (_Interval - feedback) : 0.01;
                //Console.WriteLine("feedback: ");
                //Console.WriteLine(" endtmie: {0},  startTime: {1}, cost, {2}", end.ToString("HH:mm:ss.fff"), start.ToString("HH:mm:ss.fff"), (end - start).TotalMilliseconds);
                //Console.WriteLine(" Interval: {0}, feedback: {1}", _Interval, feedback);
                //Console.WriteLine("   check: " + (_Interval - feedback));
                //Console.WriteLine(" duration: " + duration);
            }
        }

        private static async void Spin(Stopwatch w, double duration, object obj)
        {
            var current = w.ElapsedMilliseconds;
            while ((w.ElapsedMilliseconds - current) < duration)
                Thread.SpinWait(10);

        }

        private void Callback(object obj)
        {
            if (TimerEvent != null && _TimerStart == true)
                TimerEvent(obj);
        }
    }*/
}