using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace voidsoft.Zinc
{
    /// <summary>
    /// Control finder
    /// </summary>
    public class ControlFinder
    {
        private Page page;

        private ContentPlaceHolder holder;


        private Control lastControlFound = null;

        public ControlFinder(Page page)
        {
            this.page = page;
        }


        public ControlFinder(ContentPlaceHolder holder)
        {
            this.holder = holder;
        }

        public Label GetLabel(string id)
        {
            return (Label)FindControl(id);
        }

        public HyperLink GetHyperLink(string id)
        {
            return (HyperLink)FindControl(id);
        }

        public ImageButton GetImageButton(string id)
        {
            return (ImageButton)FindControl(id);
        }

        public LinkButton GetLinkButton(string id)
        {
            return (LinkButton)FindControl(id);
        }


        public Calendar GetCalendar(string id)
        {
            return (Calendar)FindControl(id);
        }

        public RadioButton GetRadioButton(string id)
        {
            return (RadioButton)FindControl(id);
        }


        public RadioButtonList GetRadioButtonList(string id)
        {
            return (RadioButtonList)FindControl(id);
        }

        public Image GetImage(string id)
        {
            return (Image)FindControl(id);
        }


        public Literal GetLiteral(string id)
        {
            return (Literal)FindControl(id);
        }


        public ImageMap GetImageMap(string id)
        {
            return (ImageMap)FindControl(id);
        }


        public TextBox GetTextBox(string id)
        {
            return (TextBox)FindControl(id);
        }

        public CheckBox GetCheckBox(string id)
        {
            return (CheckBox)FindControl(id);
        }

        public TreeView GetTreeView(string id)
        {
            return (TreeView)FindControl(id);
        }


        public ListBox GetListBox(string id)
        {
            return (ListBox)FindControl(id);
        }

        public GridView GetGridView(string id)
        {
            return (GridView)FindControl(id);
        }


        public DetailsView GetDetailsView(string id)
        {
            return (DetailsView)FindControl(id);
        }

        public FormView GetFormView(string id)
        {
            return (FormView)FindControl(id);
        }

        public Repeater GetRepeater(string id)
        {
            return (Repeater)FindControl(id);
        }

        public DataList GetDataList(string id)
        {
            return (DataList)FindControl(id);
        }


        public FileUpload GetFileUploader(string id)
        {
            return (FileUpload)FindControl(id);
        }


        public Control GetControl(string id)
        {
            return FindControl(id);
        }



        private Control FindControl(string id)
        {
            Control control; ;


            control = page != null ? page.FindControl(id) : this.holder.FindControl(id);
                       
            if (control != null)
            {
                return control;
            }

            control = this.page != null ? FindControlRecursive(this.page, id) : this.FindControlRecursive(this.holder, id);

            if (control != null)
            {
                return control;
            }

            throw new ArgumentException("Invalid control with id " + id);
        }


        private Control FindControlRecursive(Control root, string id)
        {
            if (root.ID == id)
            {
                return root;
            }

            foreach (Control c in root.Controls)
            {
                Control t = FindControlRecursive(c, id);

                if (t != null)
                {
                    return t;
                }
            }

            return null;
        }
    }
}