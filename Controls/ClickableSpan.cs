using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace voidsoft.Zinc
{
    /// <summary>
    /// 
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ClickableSpan runat=server></{0}:ClickableSpan>")]
    public class ClickableSpan : WebControl, IPostBackEventHandler
    {
        public event EventHandler Click;


        /// <summary>
        /// Raises the <see cref="E:Click"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        // Method of IPostBackEventHandler that raises change events.
        public void RaisePostBackEvent(string eventArgument)
        {
            OnClick(EventArgs.Empty);
        }


        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return (s ?? String.Empty);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write("<span id=\"" + this.UniqueID + "\" onclick=\"" + Page.GetPostBackEventReference(this) + "\">" + this.Text + "</span>  ");
        }
    }
}