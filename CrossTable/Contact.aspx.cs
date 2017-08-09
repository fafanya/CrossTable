using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CrossTable
{
    public class ColumnInfo
    {
        public string DataHeader { get; set; }
        public string DataSource { get; set; }
        public Type DataControlType { get; set; }
        public int WidthPX { get; set; }

        public ColumnInfo(string header, string source, Type controlType, int width = -1)
        {
            DataHeader = header;
            DataSource = source;
            DataControlType = controlType;
            WidthPX = width;
        }
    }

    public static class CrossTableData
    {
        public static ColumnInfo[] MainTableColumns = new ColumnInfo[]
            {
                new ColumnInfo("№", "Number", typeof(Label), 15),
                new ColumnInfo("Наименование ЗИ", "NomenclatureName", typeof(Label)),
                new ColumnInfo("Артикул ЗИ", "NomenclatureCode",  typeof(Label)),
                new ColumnInfo("Кол-во ЗИ", "TransferQuantityMO", typeof(Label), 70),
                new ColumnInfo("Наличие", "Availability", typeof(Label)),
                new ColumnInfo("Кол-во с учётом перемещения", "TotalQuantityMO" , typeof(Label))
            };
        public static ColumnInfo[] SecondaryTableColumns = new ColumnInfo[]
            {
                new ColumnInfo("Наименование КП", "NomenclatureNameAnalog", typeof(Label)),
                new ColumnInfo("Артикул КП", "NomenclatureCodeAnalog", typeof(Label)),
                new ColumnInfo("История", "History", typeof(Button)),
                new ColumnInfo("Выбор РИ", "AutorHeadSelect", typeof(CheckBox)),
                new ColumnInfo("Согл-но МЗ", "ManagerSelect", typeof(CheckBox)),
                new ColumnInfo("Кол-во", "TransferQuantity", typeof(TextBox), 70),
                new ColumnInfo("Цена р.", "CostInRub", typeof(Label)),
                new ColumnInfo("Сумма р.", "Total", typeof(Label)),
                new ColumnInfo("Срок", "Term", typeof(Label))
            };
    }

    public class CustomOffers
    {
        public Offers InitialOffer { get; set; }
        public int Number { get; set; }
        public List<Variants> Variants = new List<Variants>();
    }

    public class CustomRequests
    {
        public Requests InitialRequest { get; set; }
        public List<CustomOffers> InitialOffers = new List<CustomOffers>();
    }

    public class DynamicallyTemplatedGridViewHandler : ITemplate
    {
        ListItemType ItemType;
        string FieldName;
        string InfoType;
        int Width;

        public DynamicallyTemplatedGridViewHandler(ListItemType item_type, string field_name, string info_type,
            int width = -1)
        {
            ItemType = item_type;
            FieldName = field_name;
            InfoType = info_type;
            Width = width;
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
                            //edit_button.ImageUrl = "~/images/edit.gif";
                            edit_button.CommandName = "Edit";
                            //edit_button.Click += new ImageClickEventHandler(edit_button_Click);
                            edit_button.ToolTip = "Edit";
                            Container.Controls.Add(edit_button);
                            ImageButton insert_button = new ImageButton();
                            insert_button.ID = "insert_button";
                            //insert_button.ImageUrl = "~/images/insert.bmp";
                            insert_button.CommandName = "Edit";
                            insert_button.ToolTip = "Insert";
                            //insert_button.Click += new ImageClickEventHandler(insert_button_Click);
                            if (Width > 0)
                            {
                                insert_button.Style.Add("width", Width.ToString() + "px");
                            }
                            Container.Controls.Add(insert_button);
                            break;
                        default:
                            Label field_lbl = new Label();
                            field_lbl.ID = FieldName;
                            field_lbl.Text = String.Empty;
                            field_lbl.DataBinding += new EventHandler(OnDataBinding);
                            if (Width > 0)
                            {
                                field_lbl.Style.Add("width", Width.ToString() + "px");
                            }
                            Container.Controls.Add(field_lbl);
                            break;
                    }
                    break;
                case ListItemType.EditItem:
                    if (InfoType == "Button")
                    {
                        Button update_button = new Button();
                        update_button.ID = "update_button";
                        //update_button.CommandName = "Update";
                        //update_button.ImageUrl = "~/images/update.gif";
                        update_button.Text = "История";
                        update_button.ToolTip = "История";
                        update_button.OnClientClick =
                          "return confirm('История согласования выбранного варианта')";
                        if (Width > 0)
                        {
                            update_button.Style.Add("width", Width.ToString() + "px");
                        }
                        Container.Controls.Add(update_button);

                        // Similarly, add a button for Cancel

                    }
                    else if(InfoType == "CheckBox")
                    {
                        CheckBox cb = new CheckBox();
                        cb.ID = FieldName;
                        cb.Text = String.Empty;
                        cb.DataBinding += new EventHandler(OnDataBinding);
                        if (Width > 0)
                        {
                            cb.Style.Add("width", Width.ToString() + "px");
                        }
                        Container.Controls.Add(cb);
                        
                    }
                    else
                    // if other key and non key fields then bind textboxes with texts
                    {
                        TextBox field_txtbox = new TextBox();
                        field_txtbox.ID = FieldName;
                        field_txtbox.Text = String.Empty;
                        field_txtbox.TextChanged += Field_txtbox_TextChanged;
                        // if to update then bind the textboxes with coressponding field texts
                        //otherwise for insert no need to bind it with text

                        /*if ((int)new Page().Session["InsertFlag"] == 0)*/
                            field_txtbox.DataBinding += new EventHandler(OnDataBinding);
                        if (Width > 0)
                        {
                            field_txtbox.Style.Add("width", Width.ToString() + "px");
                        }
                        Container.Controls.Add(field_txtbox);
                    }
                    break;
            }
        }

        private void Field_txtbox_TextChanged(object sender, EventArgs e)
        {
            TextBox thisTextBox = (TextBox)sender;
            GridViewRow thisGridViewRow = (GridViewRow)thisTextBox.Parent.Parent;
            int row = thisGridViewRow.RowIndex;
            //rowChanged[row] = true;
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
                    {
                        if (sender is TextBox)
                        {
                            TextBox field_txtbox = (TextBox)sender;
                            field_txtbox.Text = bound_value_obj.ToString();
                        }
                        else if (sender is CheckBox)
                        {
                            CheckBox field_chbox = (CheckBox)sender;
                            if (bound_value_obj is bool)
                            {
                                field_chbox.Checked = (bool)bound_value_obj;
                            }
                        }
                        break;
                    }
            }
        }
    }

    public partial class Contact : System.Web.UI.Page
    {
        bool[] rowChanged;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
            //Page.DataBind();
        }

        private void BindGrid()
        {
            GridView1.ViewStateMode = ViewStateMode.Enabled;


            OffersData offers = CrossDataHelper.LoadTestOffers();
            RequestsData requests = CrossDataHelper.LoadTestPurchseRequest();

            List<CustomOffers> coList = new List<CustomOffers>();
            int t = 1;
            foreach (var a11 in offers.offers.GroupBy(x=>x.Title).OrderBy(x=>x.Key))
            {
                foreach (Offers o in a11)
                {
                    CustomOffers co = new CustomOffers();
                    co.InitialOffer = o;
                    co.Number = t;
                    coList.Add(co);
                }
                t++;
            }

            List<CustomRequests> crList = new List<CustomRequests>();
            foreach(Requests rd in requests.requests)
            {
                CustomRequests cr = new CustomRequests();
                var offersByRequest = coList.Where(x => x.InitialOffer.variants.Any(y => y.requestId.Equals(rd.ID)));

                List<CustomOffers> ncoList = new List<CustomOffers>();
                foreach(CustomOffers co in offersByRequest)
                {
                    CustomOffers nco = new CustomOffers();
                    nco.InitialOffer = co.InitialOffer;
                    nco.Number = co.Number;
                    nco.Variants = co.InitialOffer.variants.Where(y => y.requestId.Equals(rd.ID)).ToList();
                    ncoList.Add(nco);
                }
                cr.InitialOffers.AddRange(ncoList);
                cr.InitialRequest = rd;
                crList.Add(cr);
            }

            int secTabAmount = offers.offers.GroupBy(x => x.Title).Count();

            foreach(ColumnInfo ci in CrossTableData.MainTableColumns)
            {
                TemplateField tf = new TemplateField();
                tf.HeaderTemplate = new DynamicallyTemplatedGridViewHandler
                    (ListItemType.Header, ci.DataHeader, String.Empty);
                tf.ItemTemplate = new DynamicallyTemplatedGridViewHandler
                    (ListItemType.Item, ci.DataSource, String.Empty, ci.WidthPX);
                GridView1.Columns.Add(tf);
            }

            for(int i = 0; i < secTabAmount; i++)
            {
                foreach (ColumnInfo ci in CrossTableData.SecondaryTableColumns)
                {
                    string postfix = String.Empty;
                    if(i != 0)
                    {
                        postfix = "_" + (i + 1).ToString();
                    }

                    TemplateField tf = new TemplateField();
                    tf.HeaderTemplate = new DynamicallyTemplatedGridViewHandler
                        (ListItemType.Header, ci.DataHeader, String.Empty);
                    if (ci.DataControlType == typeof(TextBox))
                    {
                        tf.ItemTemplate = new DynamicallyTemplatedGridViewHandler
                            (ListItemType.EditItem, ci.DataSource + postfix, String.Empty, ci.WidthPX);
                    }
                    else if (ci.DataControlType == typeof(CheckBox))
                    {
                        tf.ItemTemplate = new DynamicallyTemplatedGridViewHandler
                            (ListItemType.EditItem, ci.DataSource + postfix, "CheckBox", ci.WidthPX);
                    }
                    else if (ci.DataControlType == typeof(Button))
                    {
                        tf.ItemTemplate = new DynamicallyTemplatedGridViewHandler
                            (ListItemType.EditItem, ci.DataSource + postfix, "Button", ci.WidthPX);
                    }
                    else
                    {
                        tf.ItemTemplate = new DynamicallyTemplatedGridViewHandler
                            (ListItemType.Item, ci.DataSource + postfix, String.Empty, ci.WidthPX);
                    }
                    GridView1.Columns.Add(tf);
                }

            }


            DataTable dt = new DataTable();
            dt.Columns.Add("Number", typeof(string));
            dt.Columns.Add("NomenclatureName", typeof(string));
            dt.Columns.Add("NomenclatureCode", typeof(string));
            dt.Columns.Add("TransferQuantityMO", typeof(string));
            dt.Columns.Add("Availability", typeof(string));
            dt.Columns.Add("TotalQuantityMO", typeof(string));

            for (int i = 0; i < secTabAmount; i++)
            {
                string postfix = String.Empty;
                if (i != 0)
                {
                    postfix = "_" + (i + 1).ToString();
                }

                dt.Columns.Add("NomenclatureNameAnalog" + postfix, typeof(string));
                dt.Columns.Add("NomenclatureCodeAnalog" + postfix, typeof(string));
                dt.Columns.Add("History" + postfix, typeof(string));
                dt.Columns.Add("AutorHeadSelect" + postfix, typeof(bool));
                dt.Columns.Add("ManagerSelect" + postfix, typeof(bool));
                dt.Columns.Add("TransferQuantity" + postfix, typeof(string));
                dt.Columns.Add("CostInRub" + postfix, typeof(string));
                dt.Columns.Add("Total" + postfix, typeof(string));
                dt.Columns.Add("Term" + postfix, typeof(string));
            }

            int j = 1;
            foreach(CustomRequests cr in crList.OrderBy(x=>x.InitialRequest.NomenclatureName))
            {
                DataRow dr1 = dt.NewRow();
                dr1["Number"] = j.ToString();
                dr1["NomenclatureName"] = cr.InitialRequest.NomenclatureName;
                dr1["NomenclatureCode"] = cr.InitialRequest.NomenclatureCode;
                dr1["TransferQuantityMO"] = cr.InitialRequest.TransferQuantityMO;
                dr1["Availability"] = cr.InitialRequest.Availability;
                dr1["TotalQuantityMO"] = cr.InitialRequest.TotalQuantityMO;

                for(int i = 0; i < secTabAmount; i++)
                {
                    string postfix = String.Empty;
                    if (i != 0)
                    {
                        postfix = "_" + (i + 1).ToString();
                    }
                    CustomOffers co = cr.InitialOffers.FirstOrDefault(x => x.Number == (i + 1));
                    if(co != null)
                    {
                        dr1["NomenclatureNameAnalog" + postfix] = co.Variants[0].NomeclatureNameAnalog;
                        dr1["NomenclatureCodeAnalog" + postfix] = co.Variants[0].NomeclatureCodeAnalog;
                        dr1["History" + postfix] = "History";
                        dr1["AutorHeadSelect" + postfix] = co.Variants[0].AuthorHeadSelect;
                        dr1["ManagerSelect" + postfix] = co.Variants[0].ManagerSelect;
                        dr1["TransferQuantity" + postfix] = co.Variants[0].transferQuantity;
                        dr1["CostInRub" + postfix] = co.Variants[0].CostInRub;
                        dr1["Total" + postfix] = co.Variants[0].Total;
                        dr1["Term" + postfix] = co.Variants[0].Term;
                    }
                }
                dt.Rows.Add(dr1);
                for (int l = 1; cr.InitialOffers.Any(x => x.Variants.Count > l); l++)
                {
                    dr1 = dt.NewRow();
                    dr1["Number"] = j.ToString();
                    dr1["NomenclatureName"] = cr.InitialRequest.NomenclatureName;
                    dr1["NomenclatureCode"] = cr.InitialRequest.NomenclatureCode;
                    dr1["TransferQuantityMO"] = cr.InitialRequest.TransferQuantityMO;
                    dr1["Availability"] = cr.InitialRequest.Availability;
                    dr1["TotalQuantityMO"] = cr.InitialRequest.TotalQuantityMO;
                    for (int i = 0; i < secTabAmount; i++)
                    {
                        string postfix = String.Empty;
                        if (i != 0)
                        {
                            postfix = "_" + (i + 1).ToString();
                        }
                        CustomOffers co = cr.InitialOffers.FirstOrDefault(x => (x.Number == (i + 1)
                        && x.Variants.Count > l));
                        if (co != null)
                        {
                            dr1["NomenclatureNameAnalog" + postfix] = co.Variants[l].NomeclatureNameAnalog;
                            dr1["NomenclatureCodeAnalog" + postfix] = co.Variants[l].NomeclatureCodeAnalog;
                            dr1["History" + postfix] = "History";
                            dr1["AutorHeadSelect" + postfix] = co.Variants[l].AuthorHeadSelect;
                            dr1["ManagerSelect" + postfix] = co.Variants[l].ManagerSelect;
                            dr1["TransferQuantity" + postfix] = co.Variants[l].transferQuantity;
                            dr1["CostInRub" + postfix] = co.Variants[l].CostInRub;
                            dr1["Total" + postfix] = co.Variants[l].Total;
                            dr1["Term" + postfix] = co.Variants[l].Term;
                        }
                    }
                    dt.Rows.Add(dr1);
                }

                j++;
            }

            /*DataRow dr = dt.NewRow();
            dr["Number"] = (1).ToString();
            dr["NomenclatureName"] = "Номенклатура 1";
            dr["NomenclatureCode"] = "N1";
            dr["TransferQuantityMO"] = "11";
            dr["Availability"] = "111";
            dr["TotalQuantityMO"] = "1111";
            dr["NomenclatureNameAnalog"] = "Номенклатура 1-А";
            dr["NomenclatureCodeAnalog"] = "N2-А";
            dr["History"] = "H";
            dr["AutorHeadSelect"] = false;
            dr["ManagerSelect"] = true;
            dr["TransferQuantity"] = "1";
            dr["CostInRub"] = "1700";
            dr["Total"] = "3400";
            dr["Term"] = "успеть к аудиту";
            dr["NomenclatureNameAnalog_2"] = "Номенклатура 1-А";
            dr["NomenclatureCodeAnalog_2"] = "N2-А";
            dr["History_2"] = "H";
            dr["AutorHeadSelect_2"] = false;
            dr["ManagerSelect_2"] = true;
            dr["TransferQuantity_2"] = "1";
            dr["CostInRub_2"] = "1700";
            dr["Total_2"] = "3400";
            dr["Term_2"] = "успеть к аудиту";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Number"] = (1).ToString();
            dr["NomenclatureName"] = "Номенклатура 1";
            dr["NomenclatureCode"] = "N1";
            dr["TransferQuantityMO"] = "11";
            dr["Availability"] = "111";
            dr["TotalQuantityMO"] = "1111";
            dr["NomenclatureNameAnalog"] = "Номенклатура 1-А";
            dr["NomenclatureCodeAnalog"] = "N2-А";
            dr["History"] = "H";
            dr["AutorHeadSelect"] = false;
            dr["ManagerSelect"] = true;
            dr["TransferQuantity"] = "1";
            dr["CostInRub"] = "1700";
            dr["Total"] = "3400";
            dr["Term"] = "успеть к аудиту";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Number"] = (2).ToString();
            dr["NomenclatureName"] = "Номенклатура 2";
            dr["NomenclatureCode"] = "N2";
            dr["TransferQuantityMO"] = "2";
            dr["Availability"] = "222";
            dr["TotalQuantityMO"] = "2222";
            dr["NomenclatureNameAnalog_2"] = "Номенклатура 1-А";
            dr["NomenclatureCodeAnalog_2"] = "N2-А";
            dr["History_2"] = "H";
            dr["AutorHeadSelect_2"] = false;
            dr["ManagerSelect_2"] = true;
            dr["TransferQuantity_2"] = "1";
            dr["CostInRub_2"] = "1700";
            dr["Total_2"] = "3400";
            dr["Term_2"] = "успеть к аудиту";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Number"] = (3).ToString();
            dr["NomenclatureName"] = "Номенклатура 3";
            dr["NomenclatureCode"] = "N3";
            dr["TransferQuantityMO"] = "33";
            dr["Availability"] = "333";
            dr["TotalQuantityMO"] = "3333";
            dt.Rows.Add(dr);*/

            GridView1.DataSource = dt;
            GridView1.DataBind();

            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Заказчик1";
            HeaderCell.ColumnSpan = 9;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Заказчик2";
            HeaderCell.ColumnSpan = 9;
            HeaderGridRow.Cells.Add(HeaderCell);

            GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);

            HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Заказчик1";
            HeaderCell.ColumnSpan = 9;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Заказчик2";
            HeaderCell.ColumnSpan = 9;
            HeaderGridRow.Cells.Add(HeaderCell);

            GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);

            SpanCellsRecursive(0, 0, GridView1.Rows.Count);

            for(int r = 0; r < GridView1.Rows.Count; r++)
            {
                for (int c = 0; c < GridView1.Columns.Count; c++)
                {
                    if ((c - 6) % 9 == 0)
                    {
                        TableCell currentCell = GridView1.Rows[r].Cells[c];
                        if (string.IsNullOrWhiteSpace(GetControlText(currentCell)))
                        {
                            for(int cc = 1; cc < 8; cc++)
                            {
                                TableCell delCell = GridView1.Rows[r].Cells[c+cc];
                                delCell.Visible = false;
                            }
                            currentCell.ColumnSpan = 9;
                        }
                    }
                }
            }
        }

        private void SpanCellsRecursive(int columnIndex, int startRowIndex, int endRowIndex)
        {
            if (columnIndex >= 6 || columnIndex >= GridView1.Columns.Count)
                return;

            TableCell groupStartCell = null;
            int groupStartRowIndex = startRowIndex;

            for (int i = startRowIndex; i < endRowIndex; i++)
            {
                TableCell currentCell = GridView1.Rows[i].Cells[columnIndex];

                bool isNewGroup = (null == groupStartCell) ||
                    (0 != String.CompareOrdinal(GetControlText(currentCell), GetControlText(groupStartCell)));

                if (isNewGroup)
                {
                    if (null != groupStartCell)
                    {
                        SpanCellsRecursive(columnIndex + 1, groupStartRowIndex, i);
                    }

                    groupStartCell = currentCell;
                    groupStartCell.RowSpan = 1;
                    groupStartRowIndex = i;
                }
                else
                {
                    currentCell.Visible = false;
                    groupStartCell.RowSpan += 1;
                }
            }

            SpanCellsRecursive(columnIndex + 1, groupStartRowIndex, endRowIndex);
        }

        private string GetControlText(Control control)
        {
            return (control.Controls[0] as Label).Text;
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
                        /*HiddenField hf1 = (HiddenField)thisGridViewRow.FindControl("HiddenField1");
                        string pk = hf1.Value;
                        TextBox tb1 = (TextBox)thisGridViewRow.FindControl("TextBox1");
                        string name = tb1.Text;*/
                        TextBox tb2 = (TextBox)thisGridViewRow.FindControl("TransferQuantity");
                        decimal price = Convert.ToDecimal(tb2.Text);
                    }
                }
                GridView1.DataBind();
                BindGrid();
            }
        }
    }
}