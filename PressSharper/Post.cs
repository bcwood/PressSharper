using System;
using System.Collections.Generic;
using System.Linq;

namespace PressSharper
{
    public class Post
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public Author Author { get; set; }
        public string Body { get; set; }
        public string Excerpt { get; set; }
        public string Slug { get; set; }
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public Attachment FeaturedImage { get; set; }
        
        public Post()
        {
            this.Categories = Enumerable.Empty<Category>().ToList();
            this.Tags = Enumerable.Empty<Tag>().ToList();
        }
    }
}
