using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Contracts
{
    public interface ICommentQuery
    {
        public string Keyword { get; set; }
        public int? PostedMonth { get; set; }
        public int? PostedYear { get; set; }
        public bool IsNotApproved { get; set; }
        public int? PostId { get; set; }
    }
}
