using System;

namespace ProductCatalog.Tests.Writer.Utility
{
    public static class Repeat
    {
        public static ITimesImpl Action(Action action)
        {
            return new TimesImpl(action);
        }

        public static IActionImpl Times(int count)
        {
            return new ActionImpl(count);
        }


        private class TimesImpl : ITimesImpl
        {
            private readonly Action action;

            public TimesImpl(Action action)
            {
                this.action = action;
            }

            public void Times(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    action.Invoke();
                }
            }
        }


        private class ActionImpl : IActionImpl
        {
            private readonly int count;

            public ActionImpl(int count)
            {
                this.count = count;
            }

            public void Action(Action action)
            {
                for (int i = 0; i < count; i++)
                {
                    action.Invoke();
                }
            }
        }
    }

    public interface ITimesImpl
    {
        void Times(int count);
    }

    public interface IActionImpl
    {
        void Action(Action action);
    }
}