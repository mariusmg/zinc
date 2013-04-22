using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Compilation;

namespace voidsoft.Zinc
{

    /// <summary>
    /// PathRights module implementation. Is a alternative to forma based authentification.
    /// </summary>
    public class PathRights : IHttpModule
    {
        private static HttpApplication currentContext;

        private static List<string> listLockedPaths = null;
        private static List<string> listAllowedPages = null;

        private static string redirectPath;
        private static string typeName;
        private static string methodName;


        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context)
        {
            //save the currentContext
            currentContext = context;
            context.PostAcquireRequestState += new EventHandler(context_PostAuthorizeRequest);
        }



        /// <summary>
        /// Handles the PostAuthorizeRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        void context_PostAuthorizeRequest(object sender, EventArgs e)
        {
            CheckUserRights();
        }


        ///<summary>
        ///Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"></see>.
        ///</summary>
        ///
        public void Dispose()
        {
        }


        /// <summary>
        /// Loads the configured paths
        /// </summary>
        private void CheckUserRights()
        {
            //check if the configuration data has been loaded
            if (typeName == null || methodName == null)
            {
                this.LoadConfigurationInformation();
            }

            this.CheckRequestedUrl();
        }

        /// <summary>
        /// Redirects to login page.
        /// </summary>
        private void CheckRequestedUrl()
        {
            try
            {
                if (currentContext.Context == null)
                {
                    return;
                }

                string url = currentContext.Context.Request.RawUrl.ToLower();

                //check first if it's allowed
                for (int i = 0; i < listAllowedPages.Count; i++)
                {
                    int index = url.IndexOf(listAllowedPages[i].ToLower());

                    if (index != -1)
                    {
                        return;
                    }
                }

                for (int i = 0; i < listLockedPaths.Count; i++)
                {
                    string currentPath = listLockedPaths[i].ToLower();

                    int index = url.IndexOf(currentPath);

                    if (index != -1)
                    {
                        Type tp = BuildManager.GetType(typeName, true);

                        object[] args = new object[] { currentContext };

                        object result = tp.InvokeMember(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, args);

                        bool hasRightsToPath = false;

                        hasRightsToPath = Convert.ToBoolean(result);

                        if (hasRightsToPath == false)
                        {
                            currentContext.Context.Response.Redirect(redirectPath, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //   HttpContext.Current.Response.Write(ex.Message);

                //no risks....redirect on error
                currentContext.Context.Response.Redirect(redirectPath);
            }
        }


        /// <summary>
        /// Loads the configured paths
        /// </summary>
        private void LoadConfigurationInformation()
        {
            listLockedPaths = new List<string>();
            listAllowedPages = new List<string>();


            //parse the locked folders
            AppSettingsReader reader = new AppSettingsReader();

            string paths = (string) reader.GetValue("LockedPaths", typeof(string));

            string[] individualPaths = paths.Split(';');

            for (int i = 0; i < individualPaths.Length; i++)
            {
                listLockedPaths.Add(individualPaths[i]);
            }

            //parsed the allowed paths
            string allowedPaths = (string) reader.GetValue("AllowedPaths", typeof(string));

            if (allowedPaths.Trim() != string.Empty)
            {
                string[] individualPages = allowedPaths.Split(';');

                for (int i = 0; i < individualPages.Length; i++)
                {
                    listAllowedPages.Add(individualPages[i]);
                }
            }

            //get the redirect path
            redirectPath = (string) reader.GetValue("RedirectLoginPage", typeof(string));


            //get the type name
            typeName = (string) reader.GetValue("TypeName", typeof(string));

            //get the method name
            methodName = (string) reader.GetValue("MethodName", typeof(string));

        }
    }
}
