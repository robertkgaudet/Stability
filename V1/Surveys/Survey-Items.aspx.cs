using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Surveys_Survey_Items : System.Web.UI.Page
{
    public string numberRules = string.Empty;
    public int tableId = 0;
    public int textBoxId = 0;
    private bool surveyCreated = false;
    private Guid itemSubTypePersonSurveyId;
    private Guid createdById = new Guid(Membership.GetUser().ProviderUserKey.ToString());
    private string survivorFullName = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var itemTypesMultiple = from it in dc.ItemTypes
                                join ist in dc.ItemSubTypes on it.ItemTypeId equals ist.ItemTypeId
                                where ist.MaxItemPurchasesAllowed > 1
                                orderby it.Name
                                select it;

        rptMultiCountItemTypes.DataSource = itemTypesMultiple.Distinct();
        rptMultiCountItemTypes.DataBind();

        var itemTypesSingle = from it in dc.ItemTypes
                              join ist in dc.ItemSubTypes on it.ItemTypeId equals ist.ItemTypeId
                              where ist.MaxItemPurchasesAllowed == 1
                              orderby it.Name
                              select it;

        rptOneCountItemTypes.DataSource = itemTypesSingle.Distinct();
        rptOneCountItemTypes.DataBind();


        Guid userId = new Guid(Request.QueryString["userId"]);
        var profile = (from p in dc.Profiles
                      where p.UserId == userId
                      select p).SingleOrDefault();


        var disaster = (from ev in dc.UserEvents
                        where ev.UserId == userId
                        orderby ev.Event.BeginDate descending
                        select new { ev.Event.Name }).Take(1).SingleOrDefault();

        survivorFullName = profile.Firstname + " " + profile.Lastname;
        litName.Text = "Create a " + disaster.Name + " Stability Disaster Recovery Wishlist for <a href='/sp/" + profile.ProfileNumber + "/" + survivorFullName.Replace(" ","-") + "'>" + survivorFullName + "</a></b><br><i class=''>Note, a survivor can only create one Disaster Recovery Wishlist.</i>";
       
        txtSurveyTitle.Value = "Help " + survivorFullName + " recover from " + disaster.Name + " by replacing an item in their home.";
        txtSruveyDescription.Value = survivorFullName + " and their family lost their possessions during " + disaster.Name + ". You can help them by purchasing an item from the list below and sharing their Disaster Recovery Wishlist with your social networks.";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Guid userId = new Guid(Request.QueryString["userId"]);
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var itemSubTypePersonSurvey = (from istps in dc.ItemSubTypePersonSurveys
                                      join p in dc.Profiles on istps.UserId equals p.UserId
                                      where istps.UserId == userId
                                      select new { p.ProfileNumber, fullname = p.Firstname + ' ' + p.Lastname, istps }).SingleOrDefault();

        if(itemSubTypePersonSurvey != null)
        {
            Response.Redirect("/sp/" + itemSubTypePersonSurvey.ProfileNumber + "/" + itemSubTypePersonSurvey.fullname.Replace(" ","-"));
        }
    }

    protected void rptOneCountItemTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RepeaterItem dataItem = (RepeaterItem)e.Item;
            CheckBoxList cblSubItemType = (CheckBoxList)e.Item.FindControl("cblSubItemType");
            Literal litItemTypeHeader = (Literal)e.Item.FindControl("litItemTypeHeaderSingle"); 

            Guid itemTypeId = (Guid)DataBinder.Eval(dataItem.DataItem, "itemTypeId");
            string itemTypeName = (string)DataBinder.Eval(dataItem.DataItem, "name");

            litItemTypeHeader.Text = itemTypeName;

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            var itemSubTypes = from ist in dc.ItemSubTypes
                               where ist.ItemTypeId == itemTypeId
                               select ist;

            cblSubItemType.DataSource = itemSubTypes;
            cblSubItemType.DataBind();
        }
    }

    protected void rptMultiCountItemTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        RepeaterItem dataItem = (RepeaterItem)e.Item;
        Panel pnlTextBoxes = (Panel)e.Item.FindControl("pnlTextBoxes");
        Literal litItemTypeHeaderMultiple = (Literal)e.Item.FindControl("litItemTypeHeaderMultiple");

        Guid itemTypeId = (Guid)DataBinder.Eval(dataItem.DataItem, "itemTypeId");
        string itemTypeName = (string)DataBinder.Eval(dataItem.DataItem, "name");

        litItemTypeHeaderMultiple.Text = itemTypeName;

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var itemSubTypes = from ist in dc.ItemSubTypes
                           where ist.ItemTypeId == itemTypeId
                           select ist;

        Table table = new Table();
        table.ID = "table_" + (tableId + 1);

        foreach (var itemSubType in itemSubTypes) 
        {
            TableRow tableRow = new TableRow();
            int count = +1;
            string controlId = "txtBoxDynamic_" + itemSubType.ItemSubTypeId + "_" + count;
            CreateTextBox(controlId, itemSubType.Name, ref tableRow);
            table.Rows.Add(tableRow);

            numberRules += controlId + ":{number: true},";
        }
        pnlTextBoxes.Controls.Add(table);
    }

    private void CreateTextBox(string id, string itemSubTypeName, ref TableRow tableRow)
    {
        TableCell tableCell = new TableCell();

        Label label = new Label();
        label.Text = itemSubTypeName + " (Enter amount needed)";
        tableCell.Controls.Add(label);

        TextBox textBox = new TextBox();
        textBox.ID = id;
        textBox.MaxLength = 2;
        textBox.Width = 50;
        textBox.CssClass = "form-control";
        textBox.Attributes.Add("number", "");
        textBox.Attributes.Add("placeholder", "0");
        tableCell.Controls.Add(textBox);

        tableRow.Cells.Add(tableCell);
        ViewState["AddedControl"] = "true";
    }

    private void GetTextBoxValues(Guid userId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        TextBox textBox;

        foreach (RepeaterItem item in rptMultiCountItemTypes.Items)
        {
            Panel panel = item.FindControl("pnlTextBoxes") as Panel;

            foreach (Table table in panel.Controls.OfType<Table>())
            {
                foreach (TableRow tr in table.Rows)
                {
                    foreach (TableCell tc in tr.Cells)
                    {
                        foreach (Control control in tc.Controls)
                        {
                            if (control.GetType() == typeof(TextBox))
                            {
                                textBox = (TextBox)control;
                                int itemNeededCount = 0;

                                if (!String.IsNullOrEmpty(textBox.Text) && int.TryParse(textBox.Text, out itemNeededCount) && itemNeededCount > 0)
                                {
                                    if (!surveyCreated)
                                    {
                                        itemSubTypePersonSurveyId = Guid.NewGuid();
                                        //Only do this once.
                                        ItemSubTypePersonSurvey itemSubTypePersonSurvey = new ItemSubTypePersonSurvey();
                                        itemSubTypePersonSurvey.CreatedBy = createdById;
                                        itemSubTypePersonSurvey.CreatedOn = DateTime.Now;
                                        itemSubTypePersonSurvey.Hidden = false;
                                        itemSubTypePersonSurvey.ItemSubTypePersonSurveyId = itemSubTypePersonSurveyId;
                                        itemSubTypePersonSurvey.UserId = userId;
                                        itemSubTypePersonSurvey.Title = txtSurveyTitle.Value;
                                        itemSubTypePersonSurvey.Description = txtSruveyDescription.Value;
                                        dc.ItemSubTypePersonSurveys.InsertOnSubmit(itemSubTypePersonSurvey);
                                        dc.SubmitChanges();
                                    }

                                    string[] textBoxIdArray = textBox.ID.Split(Convert.ToChar("_"));
                                    Guid itemSubTypeId = new Guid(textBoxIdArray[1]);

                                    ItemSubTypePerson itemSubTypePerson = new ItemSubTypePerson();
                                    itemSubTypePerson.CreatedBy = createdById;
                                    itemSubTypePerson.CreatedOn = DateTime.Now;
                                    itemSubTypePerson.Hidden = false;
                                    itemSubTypePerson.ItemSubTypeId = itemSubTypeId;
                                    itemSubTypePerson.ItemSubTypePersonId = Guid.NewGuid();
                                    itemSubTypePerson.ItemSubTypePersonSurveyId = itemSubTypePersonSurveyId;
                                    itemSubTypePerson.UserId = userId;
                                    itemSubTypePerson.ItemNeededCount = itemNeededCount;
                                    dc.ItemSubTypePersons.InsertOnSubmit(itemSubTypePerson);
                                    dc.SubmitChanges();

                                    surveyCreated = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(Request.QueryString["userId"]);

        GetTextBoxValues(userId);
        foreach (RepeaterItem item in rptOneCountItemTypes.Items)
        {
            CheckBoxList checkBoxList = item.FindControl("cblSubItemType") as CheckBoxList;
            UpdatePersonSubItemTypes(checkBoxList, userId);
        }
    }

    protected void UpdatePersonSubItemTypes(CheckBoxList cbl, Guid userId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        //UPDATE OR INSERT
        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected)
            {
                if (!surveyCreated)
                {
                    itemSubTypePersonSurveyId = Guid.NewGuid();
                    //Only do this once.
                    ItemSubTypePersonSurvey itemSubTypePersonSurvey = new ItemSubTypePersonSurvey();
                    itemSubTypePersonSurvey.CreatedBy = createdById;
                    itemSubTypePersonSurvey.CreatedOn = DateTime.Now;
                    itemSubTypePersonSurvey.Hidden = false;
                    itemSubTypePersonSurvey.ItemSubTypePersonSurveyId = itemSubTypePersonSurveyId;
                    itemSubTypePersonSurvey.UserId = userId;
                    dc.ItemSubTypePersonSurveys.InsertOnSubmit(itemSubTypePersonSurvey);
                    dc.SubmitChanges();
                }

                Guid itemSubTypeId = new Guid(item.Value);

                ItemSubTypePerson itemSubTypePerson = new ItemSubTypePerson();
                itemSubTypePerson.CreatedBy = createdById;
                itemSubTypePerson.CreatedOn = DateTime.Now;
                itemSubTypePerson.Hidden = false;
                itemSubTypePerson.ItemSubTypeId = itemSubTypeId;
                itemSubTypePerson.ItemSubTypePersonId = Guid.NewGuid();
                itemSubTypePerson.ItemSubTypePersonSurveyId = itemSubTypePersonSurveyId;
                itemSubTypePerson.UserId = userId;
                itemSubTypePerson.ItemNeededCount = 1;
                dc.ItemSubTypePersons.InsertOnSubmit(itemSubTypePerson);
                dc.SubmitChanges();

                surveyCreated = true;
            }
        }
    }
}