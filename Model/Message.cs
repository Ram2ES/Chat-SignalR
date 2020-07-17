using System;

namespace Model
{
    [Serializable]
    public class Message
    {
        public string Text { get; set; }
        public string Name{ get; set; }
        public DateTime Time { get; set; }

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

        public Message() { }
    }
}