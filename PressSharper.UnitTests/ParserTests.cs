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
                        <item>
		                    <title>test title 1</title>
		                    <dc:creator>johndoe</dc:creator>
		                    <content:encoded><![CDATA[test body 1]]></content:encoded>
		                    <wp:post_date>2010-03-05 06:12:10</wp:post_date>
		                    <wp:post_name>test-title-1</wp:post_name>
		                    <wp:status>publish</wp:status>
		                    <wp:post_type>post</wp:post_type>
		                    <category domain=""category"" nicename=""category-one""><![CDATA[Category One]]></category>
                            <category domain=""category"" nicename=""category-two""><![CDATA[Category Two]]></category>
		                    <category domain=""post_tag"" nicename=""tag-one""><![CDATA[Tag One]]></category>
                            <wp:postmeta>
			                    <wp:meta_key><![CDATA[_thumbnail_id]]></wp:meta_key>
			                    <wp:meta_value><![CDATA[3]]></wp:meta_value>
		                    </wp:postmeta>
	                    </item>
                        <item>
		                    <title>test title 2</title>
		                    <dc:creator>bobsmith</dc:creator>
		                    <content:encoded><![CDATA[test body 2]]></content:encoded>
		                    <wp:post_date>2011-04-08 09:58:10</wp:post_date>
		                    <wp:post_name>test-title-2</wp:post_name>
		                    <wp:status>publish</wp:status>
		                    <wp:post_type>post</wp:post_type>
		                    <category domain=""category"" nicename=""category-three""><![CDATA[Category Three]]></category>
		                    <category domain=""post_tag"" nicename=""tag-two""><![CDATA[Tag Two]]></category>
		                    <category domain=""post_tag"" nicename=""tag-three""><![CDATA[Tag Three]]></category>
	                    </item>
                        <item>
		                    <title>About</title>
		                    <dc:creator>johndoe</dc:creator>
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
		                    <wp:post_date>2013-06-13 09:58:10</wp:post_date>
		                    <wp:post_name>contact-us</wp:post_name>
		                    <wp:status>publish</wp:status>
		                    <wp:post_type>page</wp:post_type>
	                    </item>
                        <item>
		                    <title>Featured Image</title>
		                    <wp:post_id>3</wp:post_id>
                            <wp:attachment_url><![CDATA[http://www.example.com/featured.jpg]]></wp:attachment_url>
                            <wp:post_type>attachment</wp:post_type>
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
            var authors = blog.Authors.ToList();

            Assert.Equal(2, authors.Count);

            Assert.Equal(1, authors[0].Id);
            Assert.Equal("johndoe", authors[0].Username);
            Assert.Equal("johndoe@gmail.com", authors[0].Email);

            Assert.Equal(2, authors[1].Id);
            Assert.Equal("bobsmith", authors[1].Username);
            Assert.Equal("bobsmith@gmail.com", authors[1].Email);
        }

        [Fact]
        public void Can_Parse_Attachments()
        {
            var blog = new Blog(WORDPRESS_XML);
            var attachments = blog.Attachments.ToList();

            Assert.Equal(1, attachments.Count);

            Assert.Equal(3, attachments[0].Id);
            Assert.Equal("Featured Image", attachments[0].Title);
            Assert.Equal("http://www.example.com/featured.jpg", attachments[0].Url);
        }

        [Fact]
        public void Can_Parse_Posts()
        {
            var blog = new Blog(WORDPRESS_XML);
            var posts = blog.GetPosts().ToList();

            Assert.Equal(2, posts.Count);

            // post 1
            Assert.Equal("test title 1", posts[0].Title);
            Assert.Equal("johndoe", posts[0].Author.Username);
            Assert.Equal("test body 1", posts[0].Body);
            Assert.Equal("3/5/2010", posts[0].PublishDate.ToShortDateString());
            Assert.Equal("test-title-1", posts[0].Slug);
            Assert.Equal(2, posts[0].Categories.Count);
            Assert.Equal("category-one", posts[0].Categories[0].Slug);
            Assert.Equal("Category One", posts[0].Categories[0].Name);
            Assert.Equal("category-two", posts[0].Categories[1].Slug);
            Assert.Equal("Category Two", posts[0].Categories[1].Name);
            Assert.Equal(1, posts[0].Tags.Count);
            Assert.Equal("tag-one", posts[0].Tags[0].Slug);
            Assert.Equal("Tag One", posts[0].Tags[0].Name);

            // post 1 featured image
            Assert.NotNull(posts[0].FeaturedImage);
            Assert.Equal(3, posts[0].FeaturedImage.Id);
            Assert.Equal("Featured Image", posts[0].FeaturedImage.Title);
            Assert.Equal("http://www.example.com/featured.jpg", posts[0].FeaturedImage.Url);

            // post 2
            Assert.Equal("test title 2", posts[1].Title);
            Assert.Equal("bobsmith", posts[1].Author.Username);
            Assert.Equal("test body 2", posts[1].Body);
            Assert.Equal("4/8/2011", posts[1].PublishDate.ToShortDateString());
            Assert.Equal("test-title-2", posts[1].Slug);
            Assert.Equal(1, posts[1].Categories.Count);
            Assert.Equal("category-three", posts[1].Categories[0].Slug);
            Assert.Equal("Category Three", posts[1].Categories[0].Name);
            Assert.Equal(2, posts[1].Tags.Count);
            Assert.Equal("tag-two", posts[1].Tags[0].Slug);
            Assert.Equal("Tag Two", posts[1].Tags[0].Name);
            Assert.Equal("tag-three", posts[1].Tags[1].Slug);
            Assert.Equal("Tag Three", posts[1].Tags[1].Name);

            // post 2 featured image
            Assert.Null(posts[1].FeaturedImage);
        }

        [Fact]
        public void Can_Parse_Pages()
        {
            var blog = new Blog(WORDPRESS_XML);
            var pages = blog.GetPages().ToList();

            Assert.Equal(2, pages.Count);

            // page 1
            Assert.Equal(1, pages[0].Id);
            Assert.Null(pages[0].ParentId);
            Assert.Equal("About", pages[0].Title);
            Assert.Equal("johndoe", pages[0].Author.Username);
            Assert.Equal("This is the about page", pages[0].Body);
            Assert.Equal("5/9/2012", pages[0].PublishDate.ToShortDateString());
            Assert.Equal("about", pages[0].Slug);

            // page 2
            Assert.Equal(2, pages[1].Id);
            Assert.Equal(1, pages[1].ParentId);
            Assert.Equal("Contact Us", pages[1].Title);
            Assert.Equal("bobsmith", pages[1].Author.Username);
            Assert.Equal("This is the contact page", pages[1].Body);
            Assert.Equal("6/13/2013", pages[1].PublishDate.ToShortDateString());
            Assert.Equal("contact-us", pages[1].Slug);
        }
    }
}
