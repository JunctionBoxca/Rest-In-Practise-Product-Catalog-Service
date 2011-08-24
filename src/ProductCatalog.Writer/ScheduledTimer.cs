using System;
using System.Reflection;
using System.Threading;
using log4net;

namespace ProductCatalog.Writer
{
    public class ScheduledTimer : ITimer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public event EventHandler TimerFired;

        private readonly int intervalMilliseconds;
        private readonly Timer timer;

        public ScheduledTimer(int intervalMilliseconds)
        {
            this.intervalMilliseconds = intervalMilliseconds;
            timer = new Timer(RaiseTimerFired, null, Timeout.Infinite, Timeout.Infinite);
            TimerFired += delegate { };
        }

        private void RaiseTimerFired(object state)
        {
            EventHandler handler = TimerFired;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Start()
        {
            timer.Change(intervalMilliseconds, Timeout.Infinite);
        }

        public void Dispose()
        {
            Log.Debug("Disposing ScheduledTimer...");
            if (timer != null)
            {
                timer.Dispose();
            }
        }
    }
}