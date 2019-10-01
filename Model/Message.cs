using System;

namespace Model
{
    public class Message
    {
        public string Text { get; }
        public string Name{ get; }
        public DateTime Time { get; }

        public override string ToString()
        {
            return $"[{Time.ToShortTimeString()}] {Name}: {Text}";
        }

        public Message(string name, string text)
        {
            Text = text;
            Name = name;
            Time = DateTime.Now;
        }
    }
}