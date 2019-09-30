using System;

namespace Model
{
    public class Message
    {
        public string Text { get; }
        public User User { get; }
        public DateTime Time { get; }

        public override string ToString()
        {
            return $"[{Time.ToShortTimeString()}] {User.Name}: {Text}";
        }

        public Message(User user, string text)
        {
            Text = text;
            User = user;
            Time = DateTime.Now;
        }
    }
}