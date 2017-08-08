using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CrossTable
{
    public class DynamicallyTemplatedGridViewHandler : ITemplate
    {
        ListItemType ItemType;
        string FieldName;
        string InfoType;

        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string info_type)
        {
            ItemType = item_type;
            FieldName = field_name;
            InfoType = info_type;
        }

        public void InstantiateIn(System.Web.UI.Control Container)
        {
            switch (ItemType)
            {
                case ListItemType.Header:
                    Literal header_ltrl = new Literal();
                    header_ltrl.Text = "<b>" + FieldName + "</b>";
                    Container.Controls.Add(header_ltrl);
                    break;
                case ListItemType.Item:
                    switch (InfoType)
                    {
                        case "Button":
                            ImageButton edit_button = new ImageButton();
                            edit_button.ID = "edit_button";
                            edit_button.ImageUrl = "~/images/edit.gif";
                            edit_button.CommandName = "Edit";
                            //edit_button.Click += new ImageClickEventHandler(edit_button_Click);
                            edit_button.ToolTip = "Edit";
                            Container.Controls.Add(edit_button);
                            ImageButton insert_button = new ImageButton();
                            insert_button.ID = "insert_button";
                            insert_button.ImageUrl = "~/images/insert.bmp";
                            insert_button.CommandName = "Edit";
                            insert_button.ToolTip = "Insert";
                            //insert_button.Click += new ImageClickEventHandler(insert_button_Click);
                            Container.Controls.Add(insert_button);
                            break;
                        default:
                            Label field_lbl = new Label();
                            field_lbl.ID = FieldName;
                            field_lbl.Text = String.Empty;
                            field_lbl.DataBinding += new EventHandler(OnDataBinding);
                            Container.Controls.Add(field_lbl);
                            break;
                    }
                    break;
                case ListItemType.EditItem:
                    if (InfoType == "Button")
                    {
                        ImageButton update_button = new ImageButton();
                        update_button.ID = "update_button";
                        update_button.CommandName = "Update";
                        update_button.ImageUrl = "~/images/update.gif";
                        update_button.ToolTip = "Update";
                        update_button.OnClientClick =
                          "return confirm('Are you sure to update the record?')";
                        Container.Controls.Add(update_button);

                        // Similarly, add a button for Cancel

                    }
                    else
                    // if other key and non key fields then bind textboxes with texts
                    {
                        TextBox field_txtbox = new TextBox();
                        field_txtbox.ID = FieldName;
                        field_txtbox.Text = String.Empty;
                        // if to update then bind the textboxes with coressponding field texts
                        //otherwise for insert no need to bind it with text

                        /*if ((int)new Page().Session["InsertFlag"] == 0)*/
                            field_txtbox.DataBinding += new EventHandler(OnDataBinding);
                        Container.Controls.Add(field_txtbox);

                    }
                    break;
            }
        }

        private void OnDataBinding(object sender, EventArgs e)
        {
            object bound_value_obj = null;
            Control ctrl = (Control)sender;
            IDataItemContainer data_item_container =
            (IDataItemContainer)ctrl.NamingContainer;
            bound_value_obj = DataBinder.Eval(data_item_container.DataItem, FieldName);
            switch (ItemType)
            {
                case ListItemType.Item:
                    Label field_ltrl = (Label)sender;
                    field_ltrl.Text = bound_value_obj.ToString();
                    break;
                case ListItemType.EditItem:
                    TextBox field_txtbox = (TextBox)sender;
                    field_txtbox.Text = bound_value_obj.ToString();
                    break;
            }
        }
    }

    public partial class Contact : System.Web.UI.Page
    {

        SqlConnection con;
        bool[] rowChanged;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }

        public void BindGrid_()
        {
            SqlDataAdapter adap = new SqlDataAdapter("select * from items", con);
            DataTable dt = new DataTable();
            adap.Fill(dt);


            TemplateField tf = new TemplateField();
            TextBox tb = new TextBox();
            tb.Text = "<%# Bind(\"Product_Name\") %>";
            tf.ItemTemplate.InstantiateIn(new TextBox());
            GridView1.Columns.Add(tf);


            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private void BindGrid()
        {
            TemplateField tf = new TemplateField();
            TextBox tb = new TextBox();
            tf.ItemTemplate = new DynamicallyTemplatedGridViewHandler(ListItemType.EditItem, "Product_Name", "Product_Name");
            //tf.ItemTemplate.InstantiateIn(new TextBox());
            GridView1.Columns.Add(tf);

            DataSet ds = new DataSet();
            DataTable dt;
            DataRow dr;
            DataColumn pName;
            DataColumn pQty;
            DataColumn pPrice;
            int i = 0;
            dt = new DataTable();
            pName = new DataColumn("Product_Name", Type.GetType("System.String"));
            pQty = new DataColumn("Quantity", Type.GetType("System.Int32"));
            pPrice = new DataColumn("Price", Type.GetType("System.Int32"));
            dt.Columns.Add(pName);
            dt.Columns.Add(pQty);
            dt.Columns.Add(pPrice);
            dr = dt.NewRow();
            dr["Product_Name"] = "Product 1";
            dr["Quantity"] = 2;
            dr["Price"] = 200;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Product_Name"] = "Product 2";
            dr["Quantity"] = 5;
            dr["Price"] = 480;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Product_Name"] = "Product 3";
            dr["Quantity"] = 8;
            dr["Price"] = 100;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            dr["Product_Name"] = "Product 4";
            dr["Quantity"] = 2;
            dr["Price"] = 500;
            dt.Rows.Add(dr);
            ds.Tables.Add(dt);
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();


            //Repeater1.DataSource = dt;
            //Repeater1.DataBind();
        }

        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            rowChanged[row] = true;
        }



        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                int totalRows = GridView1.Rows.Count;
                for (int r = 0; r < totalRows; r++)
                {
                    if (rowChanged[r])
                    {
                        GridViewRow thisGridViewRow = GridView1.Rows[r];
                        HiddenField hf1 = (HiddenField)thisGridViewRow.FindControl("HiddenField1");
                        string pk = hf1.Value;
                        TextBox tb1 = (TextBox)thisGridViewRow.FindControl("TextBox1");
                        string name = tb1.Text;
                        TextBox tb2 = (TextBox)thisGridViewRow.FindControl("TextBox2");
                        decimal price = Convert.ToDecimal(tb2.Text);

                        SqlCommand cmd = new SqlCommand("update items set name='" + name + "' , price='" + price + "' where INTERM_NO=' " + pk + "'", con);
                    }
                }
                GridView1.DataBind();
                BindGrid();


            }
        }
    }
}