using System;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;
using System.Data.Entity;

namespace MCCTomskTask
{
    public class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<MyDbContext>());
            using (var context = new MyDbContext())
            {
                InitializeData(context);

                Console.WriteLine("All posts:");
                var data = context.BlogPosts.ToList<BlogPost>();
                foreach ( var blogPost in data)
                {
                    Console.WriteLine($"Id - {blogPost.Id} Title - {blogPost.Title} Text - {blogPost.Text}");
                }
                Console.WriteLine();

                Console.WriteLine("All comments:");
                var blogComments = context.BlogComments.ToList<BlogComment>();
                foreach (var blogComment in blogComments)
                {
                    Console.WriteLine($"Id - {blogComment.Id}, UserName - {blogComment.UserName}, Text - {blogComment.Text}, " +
                        $"CreatedDate - {blogComment.CreatedDate}, blogPostId - {blogComment.BlogPostId}");
                }
                Console.WriteLine();

                Console.WriteLine("How many comments each user left:");
                var commentsLeftByUserDictionary = context.BlogComments.GroupBy(x => x.UserName).ToDictionary(x => x.Key, x => x.Count());
                foreach ( var blogComment in commentsLeftByUserDictionary)
                {
                    Console.WriteLine($"{blogComment.Key}: {blogComment.Value}"); 
                }

                Console.WriteLine("Posts ordered by date of last comment:");
                var posts = context.BlogComments.GroupBy(x => x.BlogPost.Title, x => x.CreatedDate)
                                                .ToDictionary(x => x.Key, x => x.Select(z => z).Max())
                                                .OrderByDescending(x => x.Value);
                foreach (var post in posts)
                {
                    Console.WriteLine($"{post.Key}: {post.Value.ToShortDateString()}");
                }

                Console.WriteLine("How many last comments each user left:");
                var lastCommentCountByUserName = 
                    context.BlogComments.GroupBy(x => x.BlogPost.Title)
                                        .ToDictionary(
                                            x => x.Key, 
                                            x => x.Select(y => y).Aggregate((z1, z2) => z1.CreatedDate > z2.CreatedDate ? z1 : z2))
                                        .GroupBy(x => x.Value.UserName)
                                        .ToDictionary(x => x.Key, x => x.Count());

                foreach (var comment in lastCommentCountByUserName)
                {
                    Console.WriteLine($"{comment.Key}: {comment.Value}");
                }
            }
            Console.ReadKey();
        }

        private static void InitializeData(MyDbContext context)
        {
            context.BlogPosts.Add(new BlogPost("Post1")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("1", new DateTime(2020, 3, 2), "Petr"),
                    new BlogComment("2", new DateTime(2020, 3, 4), "Elena"),
                    new BlogComment("8", new DateTime(2020, 3, 5), "Ivan"),
                }
            });
            context.BlogPosts.Add(new BlogPost("Post2")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("3", new DateTime(2020, 3, 5), "Elena"),
                    new BlogComment("4", new DateTime(2020, 3, 6), "Ivan"),
                }
            });
            context.BlogPosts.Add(new BlogPost("Post3")
            {
                Comments = new List<BlogComment>()
                {
                    new BlogComment("5", new DateTime(2020, 2, 7), "Ivan"),
                    new BlogComment("6", new DateTime(2020, 2, 9), "Elena"),
                    new BlogComment("7", new DateTime(2020, 2, 10), "Ivan"),
                    new BlogComment("9", new DateTime(2020, 2, 14), "Petr"),
                }
            });
            context.SaveChanges();
        }
    }
}
