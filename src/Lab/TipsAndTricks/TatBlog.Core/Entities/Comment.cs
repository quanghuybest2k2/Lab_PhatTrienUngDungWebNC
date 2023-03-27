using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public bool Censored { get; set; }
        public int PostID { get; set; }
        public Post Post { get; set; }
    }
}
