using System;

namespace ProductCatalog.Writer.Model
{
    public class Event
    {
        private readonly int id;
        private readonly string subject;
        private readonly DateTime timestamp;
        private readonly EventBody body;

        public Event(int id, string subject, DateTime timestamp, EventBody body)
        {
            this.id = id;
            this.subject = subject;
            this.timestamp = timestamp;
            this.body = body;
        }

        public int Id
        {
            get { return id; }
        }

        public string Subject
        {
            get { return subject; }
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
        }

        public EventBody Body
        {
            get { return body; }
        }
    }
}