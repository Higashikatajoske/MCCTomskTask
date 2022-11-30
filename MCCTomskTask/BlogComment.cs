using System;

namespace MCCTomskTask
{
    public class BlogComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        public BlogComment()
        {

        }

        public BlogComment(string text, DateTime createdDate, string userName)
        {
            Text = text;
            CreatedDate = createdDate;
            UserName = userName;
        }
    }
}
