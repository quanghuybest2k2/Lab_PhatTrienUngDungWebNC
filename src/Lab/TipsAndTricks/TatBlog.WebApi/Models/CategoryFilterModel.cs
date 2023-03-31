﻿using System.ComponentModel;

namespace TatBlog.WebApi.Models
{
    public class CategoryFilterModel: PagingModel
    {
        public string Name { get; set; }
        public bool ShowOnMenu { get; set; }
    }
}
