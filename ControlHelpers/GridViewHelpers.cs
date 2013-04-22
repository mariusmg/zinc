using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace voidsoft.Zinc
{

    /// <summary>
    /// Helper class for the GridView control
    /// </summary>
    public static class GridViewHelpers
    {
        /// <summary>
        /// Exports to excel.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void ExportGridContentToExcel(GridView gridView, string fileName)
        {
            HttpContext context = HttpContext.Current;

            context.Response.Clear();

            context.Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

            context.Response.Charset = "";

            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            context.Response.ContentType = "application/vnd.xls";

            StringWriter stringWrite = new StringWriter();

            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            gridView.RenderControl(htmlWrite);

            byte[] converted = Encoding.GetEncoding("utf-8").GetBytes(stringWrite.ToString());
            context.Response.BinaryWrite(converted);

            context.Response.End();
        }
    }
}