using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace voidsoft.Zinc
{
    public class Exporter
    {
        /// <summary>
        /// Exports a DataTable to CSV and writes it to the reponse
        /// </summary>
        /// <param name="d">The datatable to be exported</param>
        /// <param name="separator">The CSV separator.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="columnIndexToSkip">The column indexes to skip from export</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ExportToCsv(DataTable d, string separator, string fileName, params int[] columnIndexToSkip)
        {
            StringBuilder builder = new StringBuilder();


            for (int i = 0; i < d.Columns.Count; i++)
            {
                if (columnIndexToSkip != null && columnIndexToSkip.Length > 0)
                {
                    if (Array.IndexOf(columnIndexToSkip, i) > -1)
                    {
                        continue;
                    }
                }

                if (i != d.Columns.Count - 1)
                {
                    builder.Append(d.Columns[i].ColumnName + separator);
                }
                else
                {
                    builder.Append(d.Columns[i].ColumnName);
                }
            }

            builder.Append(Environment.NewLine);


            for (int i = 0; i < d.Rows.Count; i++)
            {
                for (int j = 0; j < d.Columns.Count; j++)
                {
                    if (columnIndexToSkip != null && columnIndexToSkip.Length > 0)
                    {
                        if (Array.IndexOf(columnIndexToSkip, j) > -1)
                        {
                            continue;
                        }
                    }


                    if (d.Rows[i][j] == null || d.Rows[i][j] == DBNull.Value)
                    {
                        builder.Append("");
                    }
                    else
                    {
                        builder.Append(d.Rows[i][j].ToString());
                    }

                    if (j != d.Columns.Count - 1)
                    {
                        builder.Append(separator);
                    }
                }

                builder.Append(Environment.NewLine);
            }


            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", string.Format("inline; filename={0}", fileName));
            HttpContext.Current.Response.ContentType = "application/ms-excel";

            HttpContext.Current.Response.Write(builder.ToString());

            HttpContext.Current.Response.End();
        }
    }
}