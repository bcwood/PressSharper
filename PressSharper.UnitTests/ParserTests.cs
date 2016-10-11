using System.Linq;
using Xunit;

namespace PressSharper.UnitTests
{
    public class BlogTests
    {
        private const string WORDPRESS_XML =
            @"<?xml version=""1.0"" encoding=""UTF-8"" ?>
                <rss version=""2.0""
	                xmlns:excerpt=""http://wordpress.org/export/1.2/excerpt/""
	                xmlns:content=""http://purl.org/rss/1.0/modules/content/""
	                xmlns:wfw=""http://wellformedweb.org/CommentAPI/""
	                xmlns:dc=""http://purl.org/dc/elements/1.1/""
	                xmlns:wp=""http://wordpress.org/export/1.2/"">
                    <channel>
                        <title>foo title</title>
                        <description>foo description</description>
                        <wp:author>
                            <wp:author_id>1</wp:author_id>
                            <wp:author_login>johndoe</wp:author_login>
                            <wp:author_email>johndoe@gmail.com</wp:author_email>
                            <wp:author_display_name><![CDATA[John Doe]]></wp:author_display_name>
                        </wp:author>
                        <wp:author>
                            <wp:author_id>2</wp:author_id>
                            <wp:author_login>bobsmith</wp:author_login>
                            <wp:author_email>bobsmith@gmail.com</wp:author_email>
                            <wp:author_display_name><![CDATA[Bob Smith]]></wp:author_display_name>
                        </wp:author>
                        <wp:category>
                            <wp:term_id>1</wp:term_id>
                            <wp:category_nicename>category-one</wp:category_nicename>
                            <wp:cat_name>Category One</wp:cat_name>
                        </wp:category>
                        <wp:category>
                            <wp:term_id>2</wp:term_id>
                            <wp:category_nicename>category-two</wp:category_nicename>
                            <wp:cat_name>Category Two</wp:cat_name>
                        </wp:category>
                        <wp:tag>
                            <wp:term_id>1</wp:term_id>
                            <wp:tag_slug>tag-one</wp:tag_slug>
                        </wp:tag>
                        <wp:tag>
                            <wp:term_id>2</wp:term_id>
                            <wp:tag_slug>tag-two</wp:tag_slug>
                        </wp:tag>
                        <item>
		                    <title>test title 1</title>
		                    <dc:creator>johndoe</dc:creator>
		                    <content:encoded><![CDATA[test body 1]]></content:encoded>
		                    <wp:post_date>2010-04-05 06:12:10</wp:post_date>
		                    <wp:post_name>test-title-1</wp:post_name>
		                    <wp:status>publish</wp:status>
		                    <wp:post_type>post</wp:post_type>
		                    <category domain=""category"" nicename=""category-one""><![CDATA[]]></category>
                            <category domain=""category"" nicename=""category-two""><![CDATA[]]></category>
		                    <category domain=""post_tag"" nicename=""tag-one""><![CDATA[]]></category>
		                    <category domain=""post_tag"" nicename=""tag-two""><![CDATA[]]></category>
	                    </item>
                        <item>
		                    <title>test title 2</title>
		                    <dc:creator>bobsmith</dc:creator>
		                    <content:encoded><![CDATA[test body 2]]></content:encoded>
		                    <wp:post_date>2011-04-08 09:58:10</wp:post_date>
		                    <wp:post_name>test-title-2</wp:post_name>
		                    <wp:status>publish</wp:status>
		                    <wp:post_type>post</wp:post_type>
		                    <category domain=""category"" nicename=""category-one""><![CDATA[]]></category>
                            <category domain=""category"" nicename=""category-two""><![CDATA[]]></category>
		                    <category domain=""post_tag"" nicename=""tag-one""><![CDATA[]]></category>
		                    <category domain=""post_tag"" nicename=""tag-two""><![CDATA[]]></category>
	                    </item>
                        <item>
		                    <title>About</title>
		                    <dc:creator>bobsmith</dc:creator>
		                    <content:encoded><![CDATA[This is the about page]]></content:encoded>
		                    <wp:post_id>1</wp:post_id>
		                    <wp:post_parent>0</wp:post_parent>
		                    <wp:post_date>2012-05-09 09:58:10</wp:post_date>
		                    <wp:post_name>about</wp:post_name>
		                    <wp:status>publish</wp:status>
		                    <wp:post_type>page</wp:post_type>
	                    </item>
                        <item>
		                    <title>Contact Us</title>
		                    <dc:creator>bobsmith</dc:creator>
		                    <content:encoded><![CDATA[This is the contact page]]></content:encoded>
		                    <wp:post_id>2</wp:post_id>
		                    <wp:post_parent>1</wp:post_parent>
		                    <wp:post_date>2012-05-09 09:58:10</wp:post_date>
		                    <wp:post_name>contact-us</wp:post_name>
		                    <wp:status>publish</wp:status>
		                    <wp:post_type>page</wp:post_type>
	                    </item>
                    </channel>
                </rss>";

        [Fact]
        public void Can_Parse_Blog_Title()
        {
            var blog = new Blog(WORDPRESS_XML);

            Assert.Equal("foo title", blog.Title);
        }

        [Fact]
        public void Can_Parse_Blog_Description()
        {
            var blog = new Blog(WORDPRESS_XML);

            Assert.Equal("foo description", blog.Description);
        }

        [Fact]
        public void Can_Parse_Authors()
        {
            var blog = new Blog(WORDPRESS_XML);

            Assert.Equal(2, blog.Authors.Count());

            Assert.True(
                blog.Authors.Any(a => 
                    a.Id == "1" && 
                    a.Username == "johndoe" && 
                    a.Email == "johndoe@gmail.com"));

            Assert.True(
                blog.Authors.Any(a =>
                    a.Id == "2" &&
                    a.Username == "bobsmith" &&
                    a.Email == "bobsmith@gmail.com"));
        }

        [Fact]
        public void Can_Parse_Categories()
        {
            var blog = new Blog(WORDPRESS_XML);

            Assert.Equal(2, blog.Categories.Count());

            Assert.True(
                blog.Categories.Any(a =>
                    a.Id == "1" && 
                    a.Slug == "category-one" &&
                    a.Name == "Category One"));

            Assert.True(
                blog.Categories.Any(a =>
                    a.Id == "2" &&
                    a.Slug == "category-two" &&
                    a.Name == "Category Two"));
        }

        [Fact]
        public void Can_Parse_Tags()
        {
            var blog = new Blog(WORDPRESS_XML);

            Assert.Equal(2, blog.Tags.Count());

            Assert.True(
                blog.Tags.Any(a =>
                    a.Id == "1" &&
                    a.Slug == "tag-one"));

            Assert.True(
                blog.Tags.Any(a =>
                    a.Id == "2" &&
                    a.Slug == "tag-two"));
        }

        [Fact]
        public void Can_Parse_Posts()
        {
            var blog = new Blog(WORDPRESS_XML);

            var posts = blog.GetPosts();

            Assert.Equal(2, posts.Count());

            Assert.True(
                posts.Any(p => 
                    p.Title == "test title 1" && 
                    p.Author.Username == "johndoe" && 
                    p.Body == "test body 1" && 
                    p.PublishDate.Month == 4 && 
                    p.PublishDate.Day == 5 && 
                    p.PublishDate.Year == 2010 && 
                    p.Slug == "test-title-1" && 
                    p.Categories.Count() == 2 && 
                    p.Tags.Count() == 2));

            Assert.True(
                posts.Any(p =>
                    p.Title == "test title 2" &&
                    p.Author.Username == "bobsmith" &&
                    p.Body == "test body 2" &&
                    p.PublishDate.Month == 4 &&
                    p.PublishDate.Day == 8 &&
                    p.PublishDate.Year == 2011 &&
                    p.Slug == "test-title-2" &&
                    p.Categories.Count() == 2 &&
                    p.Tags.Count() == 2));
        }

        [Fact]
        public void Can_Parse_Pages()
        {
            var blog = new Blog(WORDPRESS_XML);

            var pages = blog.GetPages();

            Assert.Equal(2, pages.Count());

            Assert.True(
                pages.Any(p =>
                    p.Id == 1 &&
                    p.ParentId == null &&
                    p.Title == "About" &&
                    p.Author.Username == "bobsmith" &&
                    p.Body == "This is the about page" &&
                    p.PublishDate.Month == 5 &&
                    p.PublishDate.Day == 9 &&
                    p.PublishDate.Year == 2012 &&
                    p.Slug == "about"));

            Assert.True(
                pages.Any(p =>
                    p.Id == 2 &&
                    p.ParentId == 1 &&
                    p.Title == "Contact Us" &&
                    p.Author.Username == "bobsmith" &&
                    p.Body == "This is the contact page" &&
                    p.PublishDate.Month == 5 &&
                    p.PublishDate.Day == 9 &&
                    p.PublishDate.Year == 2012 &&
                    p.Slug == "contact-us"));
        }
    }
}
