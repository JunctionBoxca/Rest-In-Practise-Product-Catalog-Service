using System;
using ProductCatalog.Writer;

namespace ProductCatalog.Tests.Writer.Utility
{
    public class FakeTimer : ITimer
    {
        public void Dispose()
        {
            //Do nothing
        }

        public event EventHandler TimerFired;

        public void Stop()
        {
            //Do nothing
        }

        public void Start()
        {
            //Do nothing
        }

        public void Fire()
        {
            EventHandler handler = TimerFired;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}