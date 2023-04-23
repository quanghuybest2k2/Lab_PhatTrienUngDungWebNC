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
                },
                new()
                {
                    FullName = "Justin Bieber",
                    UrlSlug = "justin-bieber",
                    Email = "justin@gmail.com",
                    JoinedDate = new DateTime(2023, 4, 19)
                },
                new()
                {
                    FullName = "Nguyễn Văn Tràm",
                    UrlSlug = "nguyen van tram",
                    Email = "tram@gmail.com",
                    JoinedDate = new DateTime(2023, 5, 19)
                },
                    new()
                {
                    FullName = "Nguyễn Trung Phan",
                    UrlSlug = "nguyen-trung-Phan",
                    Email = "trungphan@gmail.com",
                    JoinedDate = new DateTime(2021, 4, 20)
                },
                new()
                {
                    FullName = "Nguyễn Du",
                    UrlSlug = "nguyen-du",
                    Email = "nguyendu@gmail.com",
                    JoinedDate = new DateTime(2023, 9, 19)
                },
                new()
                {
                    FullName = "Phan Thạch Mỹ",
                    UrlSlug = "phan-thach-my",
                    Email = "thachmy@gmail.com",
                    JoinedDate = new DateTime(2023, 9, 30)
                },
                new()
                {
                    FullName = "Trần Ngọc Hân",
                    UrlSlug = "tran-ngoc-han",
                    Email = "ngochan@gmail.com",
                    JoinedDate = new DateTime(2022, 8, 23)
                },
                new()
                {
                    FullName = "Công Hoàng",
                    UrlSlug = "cong-hoang",
                    Email = "conghoang@gmail.com",
                    JoinedDate = new DateTime(2021, 1, 19)
                },
                 new()
                {
                    FullName = "Công Lý",
                    UrlSlug = "cong-ly",
                    Email = "congly@gmail.com",
                    JoinedDate = new DateTime(2022, 7, 6)
                }
            };
            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;
        }
        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new() {Name = ".NET Core", Description = ".NET Core", UrlSlug="asp-dot-net-core", ShowOnMenu=true},
                new() {Name = "Architecture", Description="Architecture", UrlSlug="architecture", ShowOnMenu=true},
                new() {Name = "Messaging", Description="Messaging", UrlSlug="mess-info", ShowOnMenu=true},
                new() {Name = "OOP", Description="Object-Oriented Programing", UrlSlug="object-oriented-programming", ShowOnMenu=true},
                new() {Name = "Design Patterns", Description="Design Patterns", UrlSlug="design-patterns", ShowOnMenu=true},
                 new() {Name = "Social", Description="Tám chuyện", UrlSlug="social", ShowOnMenu=true},
                  new() {Name = "test", Description="test0", UrlSlug="test", ShowOnMenu=true},
                   new() {Name = "test2", Description="test2", UrlSlug="test2", ShowOnMenu=true},
                    new() {Name = "test3", Description="test3", UrlSlug="test3", ShowOnMenu=true},
                     new() {Name = "test4", Description = "test4", UrlSlug = "test4", ShowOnMenu=true},
                      new() {Name = "test5", Description = "test5", UrlSlug = "test5", ShowOnMenu=true}
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
                new() {Name = "Neural Network", Description="Neural Network", UrlSlug="neural-network"},
                   new() {Name = "test1", Description="test1", UrlSlug="test1"},
                      new() {Name = "test2", Description="test2", UrlSlug="test2"},
                         new() {Name = "test3", Description="test3", UrlSlug="test3"},
                            new() {Name = "test4", Description="test4", UrlSlug="test4"},
                               new() {Name = "test5", Description="test5", UrlSlug="test5"}
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
                },
                 new()
                {
                    Title = "Thầy Huấn là ai?",
                    ShortDescription = "Một người thích nói đạo lý",
                    Description = "Chỉ có làm thì mới có ăn",
                    Meta = "Thầy Huấn",
                    UrlSlug = "thay-huan",
                    Published = true,
                    PostedDate = new DateTime(2022, 4, 20, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 20,
                    Author = authors[4],
                    Category = categories[3],
                    Tags = new List<Tag>()
                    {
                        tags[5]
                    }
                },
                  new()
                {
                    Title = "Cách để chém gió giỏi",
                    ShortDescription = "Chả có cách nào đâu",
                    Description = "Bớt ảo tưởng lại nha",
                    Meta = "Chém gió",
                    UrlSlug = "chem-gio",
                    Published = true,
                    PostedDate = new DateTime(2021, 5, 15, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 15,
                    Author = authors[2],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                        tags[3]
                    }
                },
                   new()
                {
                    Title = "test1",
                    ShortDescription = "test1",
                    Description = "test1",
                    Meta = "test1",
                    UrlSlug = "test1",
                    Published = true,
                    PostedDate = new DateTime(2023, 6, 19, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[4],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                        tags[4]
                    }
                },
                    new()
                {
                    Title = "Test2",
                    ShortDescription = "Test2",
                    Description = "Test2",
                    Meta = "Test2",
                    UrlSlug = "test2",
                    Published = true,
                    PostedDate = new DateTime(2021, 7, 15, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[6],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[2]
                    }
                },
                     new()
                {
                    Title = "Test3",
                    ShortDescription = "Test3",
                    Description = "Test3",
                    Meta = "Test3",
                    UrlSlug = "test3",
                    Published = true,
                    PostedDate = new DateTime(2021, 3, 20, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 20,
                    Author = authors[6],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[4]
                    }
                },
                      new()
                {
                    Title = "test4",
                    ShortDescription = "test4",
                    Description = "test4",
                    Meta = "test4",
                    UrlSlug = "test4",
                    Published = true,
                    PostedDate = new DateTime(2022, 4, 20, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 20,
                    Author = authors[6],
                    Category = categories[2],
                    Tags = new List<Tag>()
                    {
                        tags[4]
                    }
                },
                       new()
                {
                    Title = "test5",
                    ShortDescription = "test5",
                    Description = "test5",
                    Meta = "test5",
                    UrlSlug = "test5",
                    Published = true,
                    PostedDate = new DateTime(2023, 6, 10, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 40,
                    Author = authors[2],
                    Category = categories[2],
                    Tags = new List<Tag>()
                    {
                        tags[3]
                    }
                },
                        new()
                {
                    Title = "test6",
                    ShortDescription = "test6",
                    Description = "test6",
                    Meta = "test6",
                    UrlSlug = "test6",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 20, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[6],
                    Category = categories[2],
                    Tags = new List<Tag>()
                    {
                        tags[4]
                    }
                }
            };
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;
        }
    }
}
