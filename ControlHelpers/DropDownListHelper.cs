using System.Web.UI.WebControls;

namespace voidsoft.Zinc
{
    /// <summary>
    /// Helper class for the DropDownList control
    /// </summary>
    public class DropDownListHelper
    {
        /// <summary>
        /// Selects the specified text in the DropDownList control
        /// </summary>
        /// <param name="dropDown">The drop down.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public bool SelectItemByText(DropDownList dropDown, string text)
        {
            for (int i = 0; i < dropDown.Items.Count; i++)
            {
                if (dropDown.Items[i].Text == text)
                {
                    dropDown.SelectedIndex = i;
                    return true;
                }
            }

            return false;
        }



        /// <summary>
        /// Selects the value.
        /// </summary>
        /// <param name="dropDown">The drop down.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool SelectItemByValue(DropDownList dropDown, string value)
        {
            for (int i = 0; i < dropDown.Items.Count; i++)
            {
                if (dropDown.Items[i].Value == value)
                {
                    dropDown.SelectedIndex = i;
                    return true;
                }
            }

            return false;
        }
    }
}