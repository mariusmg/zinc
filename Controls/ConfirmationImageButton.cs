using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using System.Web.UI.Design;

namespace voidsoft.Zinc
{
    /// <summary>
    /// ConfirmationButton
    /// </summary>
    public class ConfirmationImageButton : System.Web.UI.WebControls.ImageButton
    {
        /// <summary>
        /// Gets or sets the mouse over image URL.
        /// </summary>
        /// <value>The mouse over image URL.</value>
        [Themeable(true), UrlProperty, DefaultValue(""), Bindable(true), Category("Appearance"),
        Editor(typeof (ImageUrlEditor), typeof (UITypeEditor))]
        public string MouseOverImageUrl
        {
            get
            {
                return (string) (ViewState["MouseOverImageUrl"] ?? string.Empty);
            }
            set
            {
                ViewState["MouseOverImageUrl"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the confirmation message.
        /// </summary>
        /// <value>The confirmation message.</value>
        [Themeable(true), DefaultValue(""), Localizable(true), Category("Appearance")]
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
        /// Adds the attributes of an <see cref="T:System.Web.UI.WebControls.ImageButton"></see> to the output stream for rendering on the client.
        /// </summary>
        /// <param name="writer">The output stream to render on the client.</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if ((MouseOverImageUrl.Length > 0) && (Enabled))
            {
                writer.AddAttribute("onmouseover", "src='" + ResolveClientUrl(MouseOverImageUrl) + "'");
                writer.AddAttribute("onmouseout", "src='" + ResolveClientUrl(ImageUrl) + "'");
            }
            if (RequiresConfirmation)
            {
                writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Onclick, "return confirm('" + ConfirmationMessage + "');");
            }
            base.AddAttributesToRender(writer);
        }
    }
}