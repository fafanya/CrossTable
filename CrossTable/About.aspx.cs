using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace CrossTable
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //loadDataTable();


            if (!this.IsPostBack)
            {
                this.BindRepeater();
            }
        }


        /*private void BindRepeater()
        {
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Customers_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            Repeater1.DataSource = dt;
                            Repeater1.DataBind();
                        }
                    }
                }
            }
        }*/

        protected void Insert(object sender, EventArgs e)
        {
            /*string name = txtName.Text;
            string country = txtCountry.Text;
            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Customers_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Country", country);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }*/
            this.BindRepeater();
        }

        protected void OnEdit(object sender, EventArgs e)
        {
            //Find the reference of the Repeater Item.
            RepeaterItem item = (sender as LinkButton).Parent as RepeaterItem;
            this.ToggleElements(item, true);
        }

        private void ToggleElements(RepeaterItem item, bool isEdit)
        {
            //Toggle Buttons.
            item.FindControl("lnkEdit").Visible = !isEdit;
            item.FindControl("lnkUpdate").Visible = isEdit;
            item.FindControl("lnkCancel").Visible = isEdit;
            item.FindControl("lnkDelete").Visible = !isEdit;

            //Toggle Labels.
            item.FindControl("lblContactName").Visible = !isEdit;
            item.FindControl("lblCountry").Visible = !isEdit;

            //Toggle TextBoxes.
            item.FindControl("txtContactName").Visible = isEdit;
            item.FindControl("txtCountry").Visible = isEdit;
        }

        protected void OnUpdate(object sender, EventArgs e)
        {
            //Find the reference of the Repeater Item.
            /*RepeaterItem item = (sender as LinkButton).Parent as RepeaterItem;
            int customerId = int.Parse((item.FindControl("lblCustomerId") as Label).Text);
            string name = (item.FindControl("txtContactName") as TextBox).Text.Trim();
            string country = (item.FindControl("txtCountry") as TextBox).Text.Trim();

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Customers_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Country", country);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }*/
            this.BindRepeater();
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            //Find the reference of the Repeater Item.
            RepeaterItem item = (sender as LinkButton).Parent as RepeaterItem;
            this.ToggleElements(item, false);
        }

        protected void OnDelete(object sender, EventArgs e)
        {
            //Find the reference of the Repeater Item.
            /*RepeaterItem item = (sender as LinkButton).Parent as RepeaterItem;
            int customerId = int.Parse((item.FindControl("lblCustomerId") as Label).Text);

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Customers_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }*/
            this.BindRepeater();
        }

        private void BindRepeater()
        {
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
            //GridView1.DataSource = ds.Tables[0];
            //GridView1.DataBind();


            Repeater1.DataSource = dt;
            Repeater1.DataBind();
        }
    }
}