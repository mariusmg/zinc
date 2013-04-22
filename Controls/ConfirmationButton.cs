using System.ComponentModel;

namespace voidsoft.Zinc
{
    /// <summary>
    /// Confirmation Button
    /// </summary>
    public class ConfirmationButton : System.Web.UI.WebControls.Button
    {
        /// <summary>
        /// Gets or sets the confirmation message.
        /// </summary>
        /// <value>The confirmation message.</value>
        [DefaultValue(""), Localizable(true), Category("Appearance")]
        public string ConfirmationMessage
        {
            get
            {
                return (string) (ViewState["ConfirmationMessage"] ?? string.Empty);
            }
            set
            {
                ViewState["ConfirmationMessage"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [requires confirmation].
        /// </summary>
        /// <value><c>true</c> if [requires confirmation]; otherwise, <c>false</c>.</value>
        [DefaultValue(false), Category("Behavior")]
        public bool RequiresConfirmation
        {
            get
            {
                return (bool) (ViewState["RequiresConfirmation"] ?? false);
            }
            set
            {
                ViewState["RequiresConfirmation"] = value;
            }
        }

        /// <summary>
        /// Adds the attributes of the <see cref="T:System.Web.UI.WebControls.Button"></see> control to the output stream for rendering on the client.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter"></see> that contains the output stream to render on the client.</param>
        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            if (RequiresConfirmation)
            {
                writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Onclick, "return confirm('" + ConfirmationMessage + "');");
            }
            base.AddAttributesToRender(writer);
        }
    }
}