using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public class AuthorRepository : IAuthorRepository
    {
        public Task<Author> GetAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetAuthorBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Author>> GetAuthorsMostPost(int numAuthor, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IPagedList<AuthorItem>> GetPageAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Author> UpdateAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
