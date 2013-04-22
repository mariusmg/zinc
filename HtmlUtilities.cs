using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace voidsoft.Zinc
{
    /// <summary>
    /// HTML utilities
    /// </summary>
    public class HtmlUtilities
    {
        //vars used for sync
        private static object lockedText = new object();
        private static object lockedHtml = new object();


        /// <summary>
        /// Converts the text to HTML.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="allow">if set to <c>true</c> [allow].</param>
        /// <returns></returns>
        public static string ConvertTextToHtml(string text, bool allow)
        {
            lock (lockedText)
            {
                try
                {
                    StringBuilder sb = new StringBuilder(text);

                    //Replace all double white spaces with a single white space and &nbsp;
                    sb.Replace("  ", " &nbsp;");

                    //Check if HTML tags are not allowed
                    if (!allow)
                    {
                        //Convert the brackets into HTML equivalents
                        sb.Replace("<", "&lt;");
                        sb.Replace(">", "&gt;");
                        //Convert the double quote
                        sb.Replace("\"", "&quot;");
                    }

                    //Create a StringReader from the processed string of the StringBuilder
                    StringReader sr = new StringReader(sb.ToString());
                    StringWriter sw = new StringWriter();
                    //Loop while next character exists
                    while (sr.Peek() > -1)
                    {
                        //Read a line from the string and store it to a temp
                        //variable
                        string temp = sr.ReadLine();
                        //write the string with the HTML break tag
                        //Note here write method writes to a Internal StringBuilder
                        //object created automatically
                        sw.Write(temp + "<br>");
                    }
                    //Return the final processed text
                    return sw.GetStringBuilder().ToString();
                }
                catch
                {
                    return text;
                }
            }
        }


        /// <summary>
        /// Converts the HTML to text.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static string ConvertHtmlToText(string source)
        {
            lock (lockedHtml)
            {
                try
                {
                    string result;

                    // Remove HTML Development formatting
                    result = source.Replace("\r", " "); // Replace line breaks with space because browsers inserts space
                    result = result.Replace("\n", " "); // Replace line breaks with space because browsers inserts space
                    result = result.Replace("\t", string.Empty); // Remove step-formatting
                    result = Regex.Replace(result, @"( )+", " "); // Remove repeating speces becuase browsers ignore them

                    // Remove the header (prepare first by clearing attributes)
                    result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);

                    // remove all scripts (prepare first by clearing attributes)
                    result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase);
                    //result = System.Text.RegularExpressions.Regex.Replace(result, @"(<script>)([^(<script>\.</script>)])*(</script>)",string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);

                    // remove all styles (prepare first by clearing attributes)
                    result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);

                    // insert tabs in spaces of <td> tags
                    result = Regex.Replace(result, @"<( )*td([^>])*>", "\t", RegexOptions.IgnoreCase);

                    // insert line breaks in places of <BR> and <LI> tags
                    result = Regex.Replace(result, @"<( )*br( )*>", "\r", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"<( )*li( )*>", "\r", RegexOptions.IgnoreCase);

                    // insert line paragraphs (double line breaks) in place if <P>, <DIV> and <TR> tags
                    result = Regex.Replace(result, @"<( )*div([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"<( )*tr([^>])*>", "\r\r", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"<( )*p([^>])*>", "\r\r", RegexOptions.IgnoreCase);

                    // Remove remaining tags like <a>, links, images, comments etc - anything thats enclosed inside < >
                    result = Regex.Replace(result, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);

                    // replace special characters:
                    result = Regex.Replace(result, @"&nbsp;", " ", RegexOptions.IgnoreCase);

                    result = Regex.Replace(result, @"&bull;", " * ", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&lsaquo;", "<", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&rsaquo;", ">", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&trade;", "(tm)", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&frasl;", "/", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&lt;", "<", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&gt;", ">", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&copy;", "(c)", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&reg;", "(r)", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, @"&(.{2,6});", string.Empty, RegexOptions.IgnoreCase);

                    // for testng
                    //System.Text.RegularExpressions.Regex.Replace(result, this.txtRegex.Text,string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    // make line breaking consistent
                    result = result.Replace("\n", "\r");

                    // Remove extra line breaks and tabs: replace over 2 breaks with 2 and over 4 tabs with 4. 
                    // Prepare first to remove any whitespaces inbetween the escaped characters and remove redundant tabs inbetween linebreaks
                    result = Regex.Replace(result, "(\r)( )+(\r)", "\r\r", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, "(\t)( )+(\t)", "\t\t", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, "(\t)( )+(\r)", "\t\r", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, "(\r)( )+(\t)", "\r\t", RegexOptions.IgnoreCase);
                    result = Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", RegexOptions.IgnoreCase); // Remove redundant tabs
                    result = Regex.Replace(result, "(\r)(\t)+", "\r\t", RegexOptions.IgnoreCase); // Remove multible tabs followind a linebreak with just one tab
                    string breaks = "\r\r\r"; // Initial replacement target string for linebreaks
                    string tabs = "\t\t\t\t\t"; // Initial replacement target string for tabs
                    for (int index = 0; index < result.Length; index++)
                    {
                        result = result.Replace(breaks, "\r\r");
                        result = result.Replace(tabs, "\t\t\t\t");
                        breaks = breaks + "\r";
                        tabs = tabs + "\t";
                    }

                  
                    return result;
                }
                catch
                {
                    return source;
                }
            }
        }
    }
}