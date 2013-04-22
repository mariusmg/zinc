using System;
using System.Web;

namespace voidsoft.Zinc
{
    /// <summary>
    /// Disables browser caching on response
    /// </summary>
    public class CacheDisabler : IHttpModule
    {
        private static HttpApplication currentContext;


        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            //save the currentContext
            currentContext = context;
            context.UpdateRequestCache += new EventHandler(context_UpdateRequestCache);
        }


        /// <summary>
        /// Handles the PostAuthorizeRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        void context_UpdateRequestCache(object sender, EventArgs e)
        {
            try
            {
                //disable cache       
                Random rd = new Random();

                currentContext.Response.AddHeader("ETag", rd.Next(1111111, 9999999).ToString());
                currentContext.Response.AddHeader("Pragma", "no-cache");
                currentContext.Response.CacheControl = "no-cache";
                currentContext.Response.Cache.SetNoStore();
                currentContext.Response.Expires = -1;
            }
            catch
            {
                //ignore the exception here   
            }
        }

        ///<summary>
        ///Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"></see>.
        ///</summary>
        ///
        public void Dispose()
        {
        }
    }
}