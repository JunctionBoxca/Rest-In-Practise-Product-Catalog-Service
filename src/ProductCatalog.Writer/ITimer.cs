using System;

namespace ProductCatalog.Writer
{
    public interface ITimer : IDisposable
    {
        event EventHandler TimerFired;
        void Stop();
        void Start();
    }
}