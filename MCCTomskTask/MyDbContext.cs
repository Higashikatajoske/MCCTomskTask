using System;
using System.Data.Entity;
using System.Linq;

namespace MCCTomskTask
{
    public class MyDbContext : DbContext
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }

        public MyDbContext()
            : base("name=MyDbContext")
        {
        }
    }
}