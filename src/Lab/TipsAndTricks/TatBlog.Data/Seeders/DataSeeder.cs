using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;
        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();
            if (_dbContext.Posts.Any()) return;
            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }
        private IList<Author> AddAuthors()
        {

            var authors = new List<Author>()
            {
                new()
                {
                    FullName = "Jason Mouth",
                    UrlSlug = "jason-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@motip.com",
                    JoinedDate = new DateTime(2020, 4, 19)
                },
                 new()
                {
                    FullName = "Doan Quang Huy",
                    UrlSlug = "huy",
                    Email = "quanghuybest@gmail.com",
                    JoinedDate = new DateTime(2021, 4, 19)
                }
            };

            //foreach (var item in authors)
            //{
            //    if (!_dbContext.Authors.Any(a => a.UrlSlug == item.UrlSlug))
            //    {
            //        _dbContext.Authors.Add(item);
            //    }
            //}
            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;
        }
        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new() {Name = ".NET Core", Description = ".NET Core", UrlSlug="asp-dot-net-core", ShowOnMenu=true},
                new() {Name = "Architecture", Description="Architecture", UrlSlug="architecture", ShowOnMenu=false},
                new() {Name = "Messaging", Description="Messaging", UrlSlug="mess-info", ShowOnMenu=false},
                new() {Name = "OOP", Description="Object-Oriented Programing", UrlSlug="object-oriented-programming", ShowOnMenu=true},
                new() {Name = "Design Patterns", Description="Design Patterns", UrlSlug="design-patterns", ShowOnMenu=true}
            };
            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;
        }
        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
            {
                new() {Name = "Google", Description = "Google applications", UrlSlug="google-clc"},
                new() {Name = "ASP.NET MVC", Description="ASP.NET MVC", UrlSlug="asp-dot-net"},
                new() {Name = "Razor Page", Description="Razor Page", UrlSlug="razor-page"},
                new() {Name = "Blazor", Description="Blazor", UrlSlug="Blazor.png"},
                new() {Name = "Deep Learning", Description="Deep Learning", UrlSlug="deep-learning"},
                new() {Name = "Neural Network", Description="Neural Network", UrlSlug="neural-network"}
            };
            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();
            return tags;
        }
        private IList<Post> AddPosts(
            IList<Author> authors,
            IList<Category> categories,
            IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "ASP.NET Core...",
                    ShortDescription = "David and friends has a great...",
                    Description = "Here's a few great...",
                    Meta = "David and friends has a great repository...",
                    UrlSlug = "asp.net-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
                new()
                {
                    Title = "Huy Cận",
                    ShortDescription = "Nhà thơ hiện đại",
                    Description = "Không chỉ biết làm thơ mà còn biết nói đạo lý",
                    Meta = "Huy Cận",
                    UrlSlug = "huy-can",
                    Published = true,
                    PostedDate = new DateTime(2022, 10, 20, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[1],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
                 new()
                {
                    Title = "Trần Ngọc Huyền",
                    ShortDescription = "Một cô gái xinh đẹp",
                    Description = "Bề ngoài mang kính cận, dáng cao, nói chuyện nhẹ nhàng",
                    Meta = "Ngọc Huyền",
                    UrlSlug = "ngoc-huyen",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 20, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 20,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                }
            };
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;
        }
    }
}
