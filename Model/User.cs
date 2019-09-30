namespace Model
{
    public class User
    {
        public string Name { get; set; }
        public string Id { get;}

        public User(string connectionId)
        {
            Id = connectionId;
        }
    }

}