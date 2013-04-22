using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace voidsoft.Zinc
{
    /// <summary>
    /// 
    /// </summary>
    public class VirtualPathRedirector : IHttpModule
    {
        private static List<string> listVirtualExtensions = null;

        private static List<string> listRedirectPages = null;

        private static HttpApplication currentContext;


        ///<summary>
        ///Initializes a module and prepares it to handle requests.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpApplication"></see> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            currentContext = context;
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        /// <summary>
        /// Handles the PostAuthorizeRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void context_BeginRequest(object sender, EventArgs e)
        {
            CheckVirtualPath();
        }


        /// <summary>
        /// Checks the virtual path.
        /// </summary>
        protected void CheckVirtualPath()
        {
            LoadSettings();

            int index = -1;

            foreach (string extension in listVirtualExtensions)
            {
                ++index;

                if (currentContext.Request.Url.ToString().EndsWith("." + extension))
                {
                    //extract the file name
                    string fileName = currentContext.Request.Url.ToString().Substring(currentContext.Request.Url.ToString().IndexOf("/"));

                    string redirectPath = listRedirectPages[index];

                    currentContext.Server.Transfer(redirectPath + "?content=" + fileName, false);
                }
            }
        }


        /// <summary>
        /// Loads the settings.
        /// </summary>
        protected void LoadSettings()
        {
            if (listVirtualExtensions != null && listRedirectPages != null)
            {
                return;
            }


            listVirtualExtensions = new List<string>();
            listRedirectPages = new List<string>();


            //parse the locked folders
            AppSettingsReader reader = new AppSettingsReader();

            string paths = (string) reader.GetValue("VirtualExtensions", typeof (string));

            string[] individualPaths = paths.Split(';');

            for (int i = 0; i < individualPaths.Length; i++)
            {
                listVirtualExtensions.Add(individualPaths[i]);
            }

            //parsed the allowed paths
            string pages = (string) reader.GetValue("VirtualPageViewers", typeof (string));

            if (pages.Trim() != string.Empty)
            {
                string[] individualPages = pages.Split(';');

                for (int i = 0; i < individualPages.Length; i++)
                {
                    listRedirectPages.Add(individualPages[i]);
                }
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