using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace voidsoft.Zinc
{
    /// <summary>
    /// Displays multiple ads
    /// </summary>
    public class MultipleAds : WebControl
    {
        private string filePath = string.Empty;
     
        private List<string> listPaths = null;
        private List<string> listUrls = null;


        private const string LIST_URLS = "list_urls_";
        private const string LIST_PATHS = "list_paths_";


        private const int IMAGE_URL_LENGTH = 10;
        private const int NAVIGATE_URL_LENGTH = 13;


        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleAds"/> class.
        /// </summary>
        public MultipleAds()
        {
            listPaths = new List<string>();
            listUrls = new List<string>();

            // this.SetWatcher();
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;

                this.listUrls.Clear();
                this.listPaths.Clear();
            }
        }


        /// <summary>
        /// Builds the HTML code for ads data
        /// </summary>
        /// <returns></returns>
        private string BuildString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<table width='100%' >");

            for (int i = 0; i < this.listPaths.Count; i++)
            {
                builder.Append("<tr>");
                builder.Append("<td style='height:10px' >");
                builder.Append("</td>");
                builder.Append("</tr>");
                builder.Append("<tr>");
                builder.Append("<td align='center'>");
                builder.Append("<a href='" + listUrls[i] + "'>" + "<img alt='' style='border:0px' src='" + this.listPaths[i] + "'/></a> ");
                builder.Append("</td>");
                builder.Append("</tr>");
                builder.Append("<tr>");
                builder.Append("<td style='height:10px' >");
                builder.Append("</td>");
                builder.Append("</tr>");
            }

            builder.Append("</table>");

            return builder.ToString();
        }


        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.GetAdsData();

            if (this.listPaths.Count == 0)
            {
                return;
            }
            else
            {
                writer.Write(this.BuildString());
            }
        }


        private void SetWatcher()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(filePath);
            watcher.Changed += delegate(object sender, FileSystemEventArgs e)
                                   {
                                       HttpContext.Current.Cache[LIST_PATHS + this.ID] = null;
                                       HttpContext.Current.Cache[LIST_URLS + this.ID] = null;
                                   };
        }


        /// <summary>
        /// Gets the ads data.
        /// </summary>
        private void GetAdsData()
        {
            object result = HttpContext.Current.Cache[LIST_URLS + this.ID];

            if (result != null)
            {
                this.listUrls = (List<string>) result;
                this.listPaths = (List<string>) HttpContext.Current.Cache[LIST_PATHS + this.ID];
                return;
            }

            if (this.FilePath != string.Empty)
            {
                string path = HttpContext.Current.Request.MapPath(this.FilePath);

                string[] lines = File.ReadAllLines(path);

                string current;

                for (int i = 0; i < lines.Length; i++)
                {
                    current = lines[i].Trim();


                    if (current.StartsWith("<ImageUrl>"))
                    {
                        int index = current.IndexOf("<", IMAGE_URL_LENGTH);

                        listPaths.Add(current.Substring(IMAGE_URL_LENGTH, index - IMAGE_URL_LENGTH));
                    }
                    else if (current.StartsWith("<NavigateUrl>"))
                    {
                        int index = current.IndexOf("<", NAVIGATE_URL_LENGTH);
                        listUrls.Add(current.Substring(NAVIGATE_URL_LENGTH, index - NAVIGATE_URL_LENGTH));
                    }
                }

                //store them in the cache

                HttpContext.Current.Cache.Add(LIST_URLS + this.ID, this.listUrls, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);

                HttpContext.Current.Cache.Add(LIST_PATHS + this.ID, this.listPaths, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
        }
    }
}