namespace Blog.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
