﻿namespace PostGresAppilcation.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
