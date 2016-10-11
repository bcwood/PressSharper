using System;

namespace PressSharper
{
    public class Page
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public Author Author { get; set; }
        public string Body { get; set; }
        public string Slug { get; set; }
    }
}
