namespace DateApp.API.Models
{
    public class Like
    {
        public int LikerId { get; set; }    // user who likes
        public int LikeeId { get; set; }    // user who is liked

        public User Liker { get; set; }
        public User Likee { get; set; }
    }
}