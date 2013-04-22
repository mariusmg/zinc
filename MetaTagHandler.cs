using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace voidsoft.Zinc
{
    /// <summary>
    /// Helper class for meta tags 
    /// </summary>
    public class MetaTagHandler
    {

        private Page page;

        private const string TAG_DESCRIPTION = "description";
        private const string TAG_ROBOTS = "robots";
        private const string TAG_COPYRIGHT = "copyright";
        private const string TAG_KEYWORDS = "keywords";


        /// <summary>
        /// Initializes a new instance of the <see cref="MetaTagHandler"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public MetaTagHandler(Page page)
        {
            this.page = page;
        }



        /// <summary>
        /// Adds the meta tag.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="content">The content.</param>
        public void AddMetaTag(string tagName, string content)
        {
            this.AddTag(tagName, content);
        }


        /// <summary>
        /// Adds the description meta tag.
        /// </summary>
        /// <param name="content">The content.</param>
        public void AddDescriptionMetaTag(string content)
        {
            this.AddTag(TAG_DESCRIPTION, content);
        }

        /// <summary>
        /// Adds the copyright meta tag.
        /// </summary>
        /// <param name="content">The content.</param>
        public void AddCopyrightMetaTag(string content)
        {
            this.AddTag(TAG_COPYRIGHT, content);
        }

        /// <summary>
        /// Adds the robots meta tag.
        /// </summary>
        public void AddRobotsMetaTag()
        {
            this.AddMetaTag(TAG_ROBOTS, "all");
            
            
        }

        /// <summary>
        /// Adds the keywords meta tag.
        /// </summary>
        /// <param name="content">The content.</param>
        public void AddKeywordsMetaTag(string content)
        {
            this.AddTag(TAG_KEYWORDS, content);
        }


        /// <summary>
        /// Adds the new tag
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="content">The content.</param>
        private void AddTag(string tagName, string content)
        {
            HtmlMeta hm = new HtmlMeta();
            HtmlHead head = page.Header;
            hm.Name = tagName;
            hm.Content = content;
            head.Controls.Add(hm);
        }

    }
}