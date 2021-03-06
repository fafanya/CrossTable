﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace CrossTable
{
    public partial class CrossTablePage : Page
    {
        private OffersData m_Offers;
        private RequestsData m_Requests;
        private List<CustomRequests> m_CustomRequests;
        private List<OfferHeader> m_OfferHeaders;

        private int MainVisibleColNum
        {
            get
            {
                return m_MainColVisibility.Count(x => x == true);
            }
        }
        private int SecVisibleColNum
        {
            get
            {
                return m_SecColVisibility.Count(x => x == true);
            }
        }
        private bool[] m_MainColVisibility;
        private bool[] m_SecColVisibility;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetData();
            BindGrid();
        }

        public List<string> LoadVisibleHeaders()
        {
            List<string> headers = new List<string>();

            headers.Add("Number");
            headers.Add("NomenclatureName");
            headers.Add("NomenclatureCode");
            headers.Add("TransferQuantityMO");
            headers.Add("Availability");
            headers.Add("TotalQuantityMO");
            headers.Add("NomenclatureNameAnalog");
            headers.Add("NomenclatureCodeAnalog");
            /*headers.Add("History");
            headers.Add("AutorHeadSelect");
            headers.Add("ManagerSelect");*/
            headers.Add("TransferQuantity");
            headers.Add("CostInRub");
            headers.Add("Total");
            headers.Add("Term");

            return headers;
        }

        private void GetData()
        {
            m_Offers = CrossDataHelper.LoadTestOffers();
            m_Requests = CrossDataHelper.LoadTestPurchseRequest();
            GetColumsVisibility();

            m_OfferHeaders = new List<OfferHeader>();
            List<CustomOffers> coList = new List<CustomOffers>();
            int offerID = 1;
            foreach (var grByTitle in m_Offers.offers.GroupBy(x => x.Title).OrderBy(x => x.Key))
            {
                Offers offer = grByTitle.First();
                OfferHeader oh = new OfferHeader();
                oh.HeaderNumber = offerID;
                oh.Row1 = offer.Title;
                oh.Row2 = offer.PaymentType + " " + offer.DelayDays + " дн.";
                oh.Row3 = offer.DeliveryType + " " + offer.Area;
                m_OfferHeaders.Add(oh);

                foreach (Offers o in grByTitle)
                {
                    CustomOffers co = new CustomOffers();
                    co.InitialOffer = o;
                    co.Number = offerID;
                    coList.Add(co);
                }
                offerID++;
            }

            m_CustomRequests = new List<CustomRequests>();
            foreach (Requests rd in m_Requests.requests)
            {
                CustomRequests cr = new CustomRequests();
                var offersByRequest = coList.Where(x => x.InitialOffer.variants.Any(y => y.requestId.Equals(rd.ID)));

                List<CustomOffers> ncoList = new List<CustomOffers>();
                foreach (CustomOffers co in offersByRequest)
                {
                    CustomOffers nco = new CustomOffers();
                    nco.InitialOffer = co.InitialOffer;
                    nco.Number = co.Number;
                    nco.Variants = co.InitialOffer.variants.Where(y => y.requestId.Equals(rd.ID)).ToList();
                    ncoList.Add(nco);
                }
                cr.InitialOffers.AddRange(ncoList);
                cr.InitialRequest = rd;
                m_CustomRequests.Add(cr);
            }
        }

        /// <summary>
        /// Настройка отображения/скрытия колонок
        /// </summary>
        private void GetColumsVisibility()
        {
            m_MainColVisibility = new bool[] { false, false, false, false, false, false };
            m_SecColVisibility = new bool[] { false, false, false, false, false, false, false, false, false };

            List<string> headers = LoadVisibleHeaders();

            for(int i = 0; i < CrossTableData.MainTableColumns.Length; i++)
            {
                m_MainColVisibility[i] = headers.Any(x => x.Equals(CrossTableData.MainTableColumns[i].DataSource));
            }

            for (int i = 0; i < CrossTableData.SecondaryTableColumns.Length; i++)
            {
                m_SecColVisibility[i] = headers.Any(x => x.Equals(CrossTableData.SecondaryTableColumns[i].DataSource));
            }
        }

        /// <summary>
        /// Обновление информации в источнике данных
        /// </summary>
        /// <param name="offers">Отредактированные Offers</param>
        /// <param name="requests">Отредактированные Requests</param>
        private void SetData(OffersData offers, RequestsData requests)
        {

        }

        private void BindGrid()
        {
            int secTabAmount = m_Offers.offers.GroupBy(x => x.Title).Count();

            for(int i = 0; i < CrossTableData.MainTableColumns.Length; i++)
            {
                if (m_MainColVisibility[i])
                {
                    ColumnInfo ci = CrossTableData.MainTableColumns[i];
                    TemplateField tf = new TemplateField();
                    tf.HeaderTemplate = new DynamicallyTemplatedGridViewHandler
                        (ListItemType.Header, ci.DataHeader, String.Empty);
                    tf.ItemTemplate = new DynamicallyTemplatedGridViewHandler
                        (ListItemType.Item, ci.DataSource, String.Empty, ci.WidthPX);

                    tf.HeaderStyle.CssClass = ci.DataSource;
                    tf.ItemStyle.CssClass = ci.DataSource;

                    GridView1.Columns.Add(tf);
                }
            }

            for (int i = 0; i < secTabAmount; i++)
            {
                for(int j = 0; j < CrossTableData.SecondaryTableColumns.Length; j++)
                {
                    if (m_SecColVisibility[j])
                    {
                        ColumnInfo ci = CrossTableData.SecondaryTableColumns[j];
                        string postfix = "_" + i.ToString();
                        /*string postfix = String.Empty;
                        if (i != 0)
                        {
                            postfix = "_" + (i + 1).ToString();
                        }*/

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

                        tf.HeaderStyle.CssClass = ci.DataSource;
                        tf.ItemStyle.CssClass = ci.DataSource;

                        GridView1.Columns.Add(tf);
                    }
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
                string postfix = "_" + i.ToString();
                /*string postfix = String.Empty;
                if (i != 0)
                {
                    postfix = "_" + (i + 1).ToString();
                }*/

                dt.Columns.Add("NomenclatureNameAnalog" + postfix, typeof(string));
                dt.Columns.Add("NomenclatureCodeAnalog" + postfix, typeof(string));
                dt.Columns.Add("History" + postfix, typeof(string));
                dt.Columns.Add("AutorHeadSelect" + postfix, typeof(bool));
                dt.Columns.Add("ManagerSelect" + postfix, typeof(bool));
                dt.Columns.Add("TransferQuantity" + postfix, typeof(string));
                dt.Columns.Add("CostInRub" + postfix, typeof(string));
                dt.Columns.Add("Total" + postfix, typeof(string));
                dt.Columns.Add("Term" + postfix, typeof(string));
                dt.Columns.Add("VariantID" + postfix, typeof(string));
            }

            decimal[] totals = new decimal[secTabAmount];

            int rowNum = 1;
            foreach (CustomRequests cr in m_CustomRequests.OrderBy(x => x.InitialRequest.NomenclatureName))
            {
                for (int l = 0; cr.InitialOffers.Any(x => x.Variants.Count > l); l++)
                {
                    DataRow dr1 = dt.NewRow();
                    dr1["Number"] = rowNum.ToString();
                    dr1["NomenclatureName"] = cr.InitialRequest.NomenclatureName;
                    dr1["NomenclatureCode"] = cr.InitialRequest.NomenclatureCode;
                    dr1["TransferQuantityMO"] = cr.InitialRequest.TransferQuantityMO;
                    dr1["Availability"] = cr.InitialRequest.Availability;
                    dr1["TotalQuantityMO"] = cr.InitialRequest.TotalQuantityMO;
                    for (int i = 0; i < secTabAmount; i++)
                    {
                        string postfix = "_" + i.ToString();
                        /*string postfix = String.Empty;
                        if (i != 0)
                        {
                            postfix = "_" + (i + 1).ToString();
                        }*/

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
                            dr1["VariantID" + postfix] = co.Variants[l].ID;

                            var strTotal = co.Variants[l].Total;
                            strTotal = strTotal.Replace(" ","");
                            strTotal = strTotal.Replace(",", ".");
                            decimal currentTotal;
                            if(decimal.TryParse(strTotal, out currentTotal))
                            {
                                totals[i] += currentTotal;
                            }
                        }
                    }
                    dt.Rows.Add(dr1);
                }
                rowNum++;
            }

            ViewState["Data"] = dt;
            GridView1.DataSource = ViewState["Data"];
            GridView1.DataBind();

            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            HeaderGridRow.ID = "1_HEADER_ROW";
            TableCell HeaderCell = new TableCell();
            HeaderCell.BackColor = System.Drawing.Color.White;
            HeaderCell.BorderColor = System.Drawing.Color.White;
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = MainVisibleColNum;
            HeaderCell.ID = "1_HEADER_CELL";
            HeaderGridRow.Cells.Add(HeaderCell);
            for (int i = 0; i < secTabAmount; i++)
            {
                HeaderCell = new TableCell();
                HeaderCell.BackColor = AllowedColors.Colors[i % 4];
                HeaderCell.BorderColor = AllowedColors.Colors[i % 4];
                HeaderCell.Text = m_OfferHeaders[i].Row1;
                HeaderCell.ColumnSpan = SecVisibleColNum;
                HeaderCell.ID = "1_HEADER_CELL" + i.ToString();
                HeaderGridRow.Cells.Add(HeaderCell);
            }
            GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);

            HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            HeaderGridRow.ID = "2_HEADER_ROW";
            HeaderCell = new TableCell();
            HeaderCell.BackColor = System.Drawing.Color.White;
            HeaderCell.BorderColor = System.Drawing.Color.White;
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = MainVisibleColNum;
            HeaderCell.ID = "2_HEADER_CELL";
            HeaderGridRow.Cells.Add(HeaderCell);
            for (int i = 0; i < secTabAmount; i++)
            {
                HeaderCell = new TableCell();
                HeaderCell.BackColor = AllowedColors.Colors[i % 4];
                HeaderCell.BorderColor = AllowedColors.Colors[i % 4];
                HeaderCell.Text = m_OfferHeaders[i].Row2;
                HeaderCell.ColumnSpan = SecVisibleColNum;
                HeaderCell.ID = "2_HEADER_CELL" + i.ToString();
                HeaderGridRow.Cells.Add(HeaderCell);
            }
            GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);

            HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            HeaderGridRow.ID = "3_HEADER_ROW";
            HeaderCell = new TableCell();
            HeaderCell.BackColor = System.Drawing.Color.White;
            HeaderCell.BorderColor = System.Drawing.Color.White;
            HeaderCell.Text = "";
            HeaderCell.ColumnSpan = MainVisibleColNum;
            HeaderCell.ID = "3_HEADER_CELL";
            HeaderGridRow.Cells.Add(HeaderCell);
            for (int i = 0; i < secTabAmount; i++)
            {
                HeaderCell = new TableCell();
                HeaderCell.BackColor = AllowedColors.Colors[i % 4];
                HeaderCell.BorderColor = AllowedColors.Colors[i % 4];
                HeaderCell.Text = m_OfferHeaders[i].Row3;
                HeaderCell.ColumnSpan = SecVisibleColNum;
                HeaderCell.ID = "3_HEADER_CELL" + i.ToString();
                HeaderGridRow.Cells.Add(HeaderCell);
            }
            GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);

            HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);
            HeaderGridRow.ID = "1_FOOTER_ROW";
            HeaderCell = new TableCell();
            HeaderCell.BackColor = System.Drawing.Color.White;
            HeaderCell.BorderColor = System.Drawing.Color.White;
            HeaderCell.ColumnSpan = MainVisibleColNum;
            HeaderCell.ID = "1_FOOTER_CELL";
            HeaderGridRow.Cells.Add(HeaderCell);
            for (int i = 0; i < secTabAmount; i++)
            {
                string postfix = "_" + i.ToString();
                /*string postfix = String.Empty;
                if (i != 0)
                {
                    postfix = "_" + (i + 1).ToString();
                }*/

                HeaderCell = new TableCell();
                HeaderCell.BackColor = System.Drawing.Color.White;
                HeaderCell.BorderColor = System.Drawing.Color.White;
                HeaderCell.Text = "Итого";
                HeaderCell.Font.Bold = true;
                HeaderCell.HorizontalAlign = HorizontalAlign.Right;
                HeaderCell.ColumnSpan = SecVisibleColNum - 2;
                HeaderCell.ID = "1_FOOTER_CELL_1" + postfix;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.BackColor = System.Drawing.Color.White;
                HeaderCell.BorderColor = System.Drawing.Color.White;

                var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                nfi.NumberGroupSeparator = " ";
                var total = totals[i].ToString("#,0.00", nfi);
                total = total.Replace('.', ',');
                HeaderCell.Text = total;

                HeaderCell.Font.Bold = true;
                HeaderCell.ColumnSpan = 2;
                HeaderCell.ID = "1_FOOTER_CELL_2" + postfix;
                HeaderGridRow.Cells.Add(HeaderCell);
            }
            GridView1.Controls[0].Controls.AddAt(GridView1.Controls[0].Controls.Count - 1, HeaderGridRow);

            SpanRowsRecursive(0, 0, GridView1.Rows.Count);
            SpanColumns();
        }


        private void SpanRowsRecursive(int columnIndex, int startRowIndex, int endRowIndex)
        {
            if (columnIndex >= MainVisibleColNum || columnIndex >= GridView1.Columns.Count)
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
                        SpanRowsRecursive(columnIndex + 1, groupStartRowIndex, i);
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

            SpanRowsRecursive(columnIndex + 1, groupStartRowIndex, endRowIndex);
        }
        private void SpanColumns()
        {
            for (int r = 0; r < GridView1.Rows.Count; r++)
            {
                for (int c = 0; c < GridView1.Columns.Count; c++)
                {
                    if ((c - MainVisibleColNum) % SecVisibleColNum == 0)
                    {
                        TableCell currentCell = GridView1.Rows[r].Cells[c];
                        if (string.IsNullOrWhiteSpace(GetControlText(currentCell)))
                        {
                            for (int cc = 1; cc < SecVisibleColNum; cc++)
                            {
                                TableCell delCell = GridView1.Rows[r].Cells[c + cc];
                                delCell.Visible = false;
                            }
                            currentCell.ColumnSpan = SecVisibleColNum;
                        }
                    }
                }
            }
        }

        private string GetControlText(Control control)
        {
            if (control.Controls.Count > 0)
            {
                if (control.Controls[0] is Label)
                {
                    return (control.Controls[0] as Label).Text;
                }
            }
            return String.Empty;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                DataTable dt = (DataTable)ViewState["Data"];
                for (int r = 0; r < GridView1.Rows.Count; r++)
                {
                    for (int i = 0; ; i++)
                    {
                        string postfix = "_" + i.ToString();
                        /*string postfix = String.Empty;
                        if (i != 0)
                        {
                            postfix = "_" + (i + 1).ToString();
                        }*/

                        GridViewRow thisGridViewRow = GridView1.Rows[r];
                        TextBox tb = (TextBox)thisGridViewRow.FindControl("TransferQuantity" + postfix);
                        if (tb == null)
                        {
                            break;
                        }

                        var variantID = dt.Rows[r]["VariantID" + postfix];
                        foreach (CustomRequests cr in m_CustomRequests)
                        {
                            foreach (CustomOffers o in cr.InitialOffers.Where(x => x.Number == i + 1))
                            {
                                Variants v = o.InitialOffer.variants.FirstOrDefault(x => x.ID.Equals(variantID));
                                if (v != null)
                                {
                                    v.transferQuantity = tb.Text;
                                }
                            }
                        }
                    }
                }
                SetData(m_Offers, m_Requests);
                Response.Redirect(Request.RawUrl);
            }
        }
    }

    public class DynamicallyTemplatedGridViewHandler : ITemplate
    {
        private ListItemType m_ItemType;
        private string m_FieldName;
        private string m_InfoType;
        private int m_Width;

        public DynamicallyTemplatedGridViewHandler(ListItemType itemType, string fieldName, string infoType, int width = 0)
        {
            m_ItemType = itemType;
            m_FieldName = fieldName;
            m_InfoType = infoType;
            m_Width = width;
        }

        public void InstantiateIn(Control Container)
        {
            switch (m_ItemType)
            {
                case ListItemType.Header:
                    {
                        Literal lt = new Literal();
                        lt.Text = "<b>" + m_FieldName + "</b>";
                        Container.Controls.Add(lt);
                        break;
                    }
                case ListItemType.Item:
                    {
                        switch (m_InfoType)
                        {
                            default:
                                {
                                    Label lb = new Label();
                                    lb.ID = m_FieldName;
                                    lb.Text = String.Empty;
                                    lb.DataBinding += new EventHandler(OnDataBinding);
                                    lb.Style.Add("width", m_Width.ToString() + "px");
                                    Container.Controls.Add(lb);
                                    break;
                                }
                        }
                        break;
                    }
                case ListItemType.EditItem:
                    {
                        if (m_InfoType == "Button")
                        {
                            Button btn = new Button();
                            btn.Text = "История";
                            btn.ToolTip = "История";
                            btn.OnClientClick =
                              "return confirm('История согласования выбранного варианта')";
                            btn.Style.Add("width", m_Width.ToString() + "px");
                            Container.Controls.Add(btn);
                        }
                        else if (m_InfoType == "CheckBox")
                        {
                            CheckBox cb = new CheckBox();
                            cb.ID = m_FieldName;
                            cb.DataBinding += new EventHandler(OnDataBinding);
                            cb.Style.Add("width", m_Width.ToString() + "px");
                            Container.Controls.Add(cb);
                        }
                        else
                        {
                            TextBox tb = new TextBox();
                            tb.ID = m_FieldName;
                            tb.DataBinding += new EventHandler(OnDataBinding);
                            tb.Style.Add("width", m_Width.ToString() + "px");
                            Container.Controls.Add(tb);
                        }
                        break;
                    }
            }
        }

        private void OnDataBinding(object sender, EventArgs e)
        {
            object boundValueObj = null;
            Control ctrl = (Control)sender;
            IDataItemContainer dataItemContainer = (IDataItemContainer)ctrl.NamingContainer;
            boundValueObj = DataBinder.Eval(dataItemContainer.DataItem, m_FieldName);
            switch (m_ItemType)
            {
                case ListItemType.Item:
                    {
                        Label lb = (Label)sender;
                        lb.Text = boundValueObj.ToString();
                        break;
                    }
                case ListItemType.EditItem:
                    {
                        if (sender is TextBox)
                        {
                            TextBox tb = (TextBox)sender;
                            string data = boundValueObj.ToString();
                            tb.Text = boundValueObj.ToString();
                        }
                        else if (sender is CheckBox)
                        {
                            CheckBox cb = (CheckBox)sender;
                            if (boundValueObj is bool)
                            {
                                cb.Checked = (bool)boundValueObj;
                            }
                        }
                        break;
                    }
            }
        }
    }

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

    public class OfferHeader
    {
        public int HeaderNumber { get; set; }
        public string Row1 { get; set; }
        public string Row2 { get; set; }
        public string Row3 { get; set; }
    }

    public class AllowedColors
    {
        public static System.Drawing.Color[] Colors = new System.Drawing.Color[]
        {
            System.Drawing.Color.LightYellow,
            System.Drawing.Color.LightGreen,
            System.Drawing.Color.LightBlue,
            System.Drawing.Color.LightSlateGray
        };
    }
}