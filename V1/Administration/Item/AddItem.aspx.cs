using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public partial class V1_Administration_Item_AddItem : BaseWebForm
{
    Guid itemId = String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["itemId"]) ? Guid.NewGuid() : new Guid(HttpContext.Current.Request.QueryString["itemId"]);
    public string   toggleLocationTypeVisibility    = "hide";
    public string   togglePersonVisibility          = "hide";
    public bool     locationEntityTypeChecked       = false;
    public bool     personEntityTypeChecked         = false;

    protected void Page_Load(object sender, EventArgs e)
	{
        if(!IsPostBack)
        {
            string brandId          = string.Empty;
            string itemTypeId       = string.Empty;
            string itemSubTypeId    = string.Empty;
            if (!String.IsNullOrEmpty(Request.QueryString["itemId"]))
            {
                divUploadImage.Visible = true;
                CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

                hypPhotoUpload.NavigateUrl = "ItemPhotoUpload.aspx?itemId=" + itemId;

                //Load the fields.
                var item = (from i in dc.Items
                            where i.ItemId == itemId
                            select i).SingleOrDefault();

                txtCost.Value = !item.Cost.HasValue ? string.Empty : item.Cost.ToString();
                txtItemDescription.Value = item.Description;
                txtShippingCharge.Value = !item.ShippingCharge.HasValue ? string.Empty : item.ShippingCharge.ToString();
                txtItemName.Value = item.Name;
                txtURLFriendlyItemName.Value = item.URLFriendlyName; 
                txtMarkupPercent.Value = !item.MarkupPercent.HasValue ? string.Empty : item.MarkupPercent.ToString();
                txtPrice.Value = !item.Price.HasValue ? string.Empty : item.Price.ToString();
                txtSku.Value = item.SKU;
                txtSourceURL.Value = item.URLToSourceItem;
                txtTaxAmount.Value = !item.TaxAmount.HasValue ? string.Empty : item.TaxAmount.ToString();
                txtVendorPrice.Value = !item.VendorPrice.HasValue ? string.Empty : item.VendorPrice.ToString();
                brandId = item.BrandId.ToString();
                itemTypeId = item.ItemTypeId.ToString();
                itemSubTypeId = item.ItemSubTypeId.ToString();

                //Get the personId
                var personEntityType    = (from et in dc.EntityTypes
                                        join iet in dc.ItemEntityTypes on et.EntityTypeId equals iet.EntityTypeId
                                        where et.Name.ToLower() == "person"
                                        && iet.ItemId == itemId
                                        select new { et.EntityTypeId }).SingleOrDefault();

                //Get the personId
                var locationEntityType  = (from et in dc.EntityTypes
                                        join iet in dc.ItemEntityTypes on et.EntityTypeId equals iet.EntityTypeId
                                        where et.Name.ToLower() == "location"
                                        && iet.ItemId == itemId
                                        select new { et.EntityTypeId }).SingleOrDefault();

                if(personEntityType != null)
                {
                    chkPerson.Checked =  true;
                    togglePersonVisibility = "show";
                    personEntityTypeChecked = true;
                }

                if (locationEntityType != null)
                {
                    chkLocation.Checked = true;
                    toggleLocationTypeVisibility = "show";
                    locationEntityTypeChecked = true;
                }
            }

            LoadBrands(brandId);
            LoadItemTypes(itemTypeId);
            LoadItemSubTypes(itemSubTypeId);
            LoadLocationTypes();
            LoadItemRoles();
            LoadEvents();
            LoadRecoveryStages();
        }
	}

    protected void btnSubmit_Cancel(object sender, EventArgs e)
    {
        Response.Redirect("~/V1/Administration/Item/List.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        //Get the existing itemId or create a new one.
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var item = (from i in dc.Items
                    where i.ItemId == itemId
                    select i).SingleOrDefault();

        //----------------------------
        int? markupPercent = !String.IsNullOrEmpty(txtMarkupPercent.Value) ? Convert.ToInt32(txtMarkupPercent.Value) : 0;
        decimal cost = !String.IsNullOrEmpty(txtCost.Value) ? Convert.ToDecimal(txtCost.Value) : 0;
        decimal price = !String.IsNullOrEmpty(txtPrice.Value) ? Convert.ToDecimal(txtPrice.Value) : 0;
        decimal taxAmount = !String.IsNullOrEmpty(txtTaxAmount.Value) ? Convert.ToDecimal(txtTaxAmount.Value) : 0;
        decimal vendorPrice = !String.IsNullOrEmpty(txtVendorPrice.Value) ? Convert.ToDecimal(txtVendorPrice.Value) : 0;
        decimal shippingCharge = !String.IsNullOrEmpty(txtShippingCharge.Value) ? Convert.ToDecimal(txtShippingCharge.Value) : 0;
        //----------------------------
        string description = txtItemDescription.Value;
        string itemName = txtItemName.Value;
        string URLFriendlyItemName = txtURLFriendlyItemName.Value;
        string sku = txtSku.Value;
        string sourceURL = txtSourceURL.Value;
        //----------------------------
        bool personEntityTypeChecked = chkPerson.Checked;
        bool locationEntityTypeChecked = chkLocation.Checked;

        if (item == null)
        {
            //INSERT
            Item newItem = new Item();
            newItem.ItemId = itemId;
            if (!String.IsNullOrEmpty(ddlBrand.SelectedItem.Value) && ddlBrand.SelectedItem.Value.ToLower() != "none")
            {
                newItem.BrandId = new Guid(ddlBrand.SelectedItem.Value);
            }
            if (!String.IsNullOrEmpty(ddlItemType.SelectedItem.Value))
            {
                newItem.ItemTypeId = new Guid(ddlItemType.SelectedItem.Value);
            }
            if (!String.IsNullOrEmpty(ddlItemSubType.SelectedItem.Value))
            {
                newItem.ItemSubTypeId = new Guid(ddlItemSubType.SelectedItem.Value);
            }

            if (markupPercent > 0)
                newItem.MarkupPercent = markupPercent;
            if (cost > 0)
                newItem.Cost = cost;
            if (price > 0)
                newItem.Price = price;
            if (taxAmount > 0)
                newItem.TaxAmount = taxAmount;
            if (vendorPrice > 0)
                newItem.VendorPrice = vendorPrice;
            if (shippingCharge > 0)
                newItem.ShippingCharge = shippingCharge;

            newItem.Description = description;
            newItem.Name = itemName;
            newItem.URLFriendlyName = URLFriendlyItemName;
            newItem.SKU = sku;
            newItem.URLToSourceItem = sourceURL;
            newItem.CreatedBy = userId;
            newItem.CreatedOn = DateTime.Now;
            newItem.UpdatedBy = userId;
            newItem.IsDeleted = false;
            newItem.UpdatedOn = DateTime.Now;
            dc.Items.InsertOnSubmit(newItem);
            dc.SubmitChanges();

            if (personEntityTypeChecked)
            {
                //Get the person entity type id
                var entityType = (from et in dc.EntityTypes
                                    where et.Name.ToLower() == "person"
                                    select new { et.EntityTypeId }).SingleOrDefault();
                
                ItemEntityType itemEntity = new ItemEntityType();
                itemEntity.EntityTypeId = entityType.EntityTypeId;
                itemEntity.ItemId = itemId;
                itemEntity.ItemEntityTypeId = Guid.NewGuid();
                dc.ItemEntityTypes.InsertOnSubmit(itemEntity);
                dc.SubmitChanges();
            }

            if (locationEntityTypeChecked)
            {
                //Get the location entity type id
                var entityType = (from et in dc.EntityTypes
                                  where et.Name.ToLower() == "location"
                                  select new { et.EntityTypeId }).SingleOrDefault();

                ItemEntityType itemEntity = new ItemEntityType();
                itemEntity.EntityTypeId = entityType.EntityTypeId;
                itemEntity.ItemId = itemId;
                itemEntity.ItemEntityTypeId = Guid.NewGuid();
                dc.ItemEntityTypes.InsertOnSubmit(itemEntity);
                dc.SubmitChanges();
            }
        }
        else
        {
            //UPDATE
            if (String.IsNullOrEmpty(ddlBrand.SelectedItem.Value) || ddlBrand.SelectedItem.Value.ToLower() == "none")
            {
                item.BrandId = null;
            }
            else
            {
                item.BrandId = new Guid(ddlBrand.SelectedItem.Value);
            }
            if (String.IsNullOrEmpty(ddlItemType.SelectedItem.Value) || ddlItemType.SelectedItem.Value.ToLower() == "none")
            {
                item.ItemTypeId = Guid.Empty;
            }
            else
            {
                item.ItemTypeId = new Guid(ddlItemType.SelectedItem.Value);
            }
            if (String.IsNullOrEmpty(ddlItemSubType.SelectedItem.Value) || ddlItemSubType.SelectedItem.Value.ToLower() == "none")
            {
                item.ItemSubTypeId = null;
            }
            else
            {
                item.ItemSubTypeId = new Guid(ddlItemSubType.SelectedItem.Value);
            }

            if (markupPercent > 0)
                item.MarkupPercent = markupPercent;
            if (cost > 0)
                item.Cost = cost;
            if (price > 0)
                item.Price = price;
            if (taxAmount > 0)
                item.TaxAmount = taxAmount;
            if (vendorPrice > 0)
                item.VendorPrice = vendorPrice;
            if (shippingCharge > 0)
                item.ShippingCharge = shippingCharge;

            item.Description = description;
            item.Name = itemName;
            item.URLFriendlyName = URLFriendlyItemName;
            item.SKU = sku;
            item.IsDeleted = false;
            item.URLToSourceItem = sourceURL;
            item.UpdatedBy = userId;
            item.UpdatedOn = DateTime.Now;
            dc.SubmitChanges();

            //Get the personId
            var personEntityTypeId = (from et in dc.EntityTypes
                                  where et.Name.ToLower() == "person"
                                  select new { et.EntityTypeId }).SingleOrDefault();

            if (personEntityTypeChecked)
            {
                //See if it already exists...
                var personEntityType = (from et in dc.EntityTypes
                                        join iet in dc.ItemEntityTypes on et.EntityTypeId equals iet.EntityTypeId
                                        where et.Name.ToLower() == "person"
                                        && iet.ItemId == itemId
                                        select new { et.EntityTypeId }).SingleOrDefault();

                if(personEntityType == null)
                { 
                    //Add new entry.
                    ItemEntityType itemEntity = new ItemEntityType();
                    itemEntity.EntityTypeId = personEntityTypeId.EntityTypeId;
                    itemEntity.ItemId = itemId;
                    itemEntity.ItemEntityTypeId = Guid.NewGuid();
                    dc.ItemEntityTypes.InsertOnSubmit(itemEntity);
                    dc.SubmitChanges();
                }
            }
            else
            {
                //Delete any existing entries and add a new one.
                var itemEntityType = (from et in dc.ItemEntityTypes
                                      where et.EntityTypeId == personEntityTypeId.EntityTypeId && et.ItemId == item.ItemId
                                      select et).SingleOrDefault();

                if (itemEntityType != null)
                {
                    dc.ItemEntityTypes.DeleteOnSubmit(itemEntityType);
                    dc.SubmitChanges();
                }
            }

            //Get the locationId
            var locationEntityTypeId = (from et in dc.EntityTypes
                                        where et.Name.ToLower() == "location"
                                        select new { et.EntityTypeId }).SingleOrDefault();

            if (locationEntityTypeChecked)
            {
                var locationEntityType = (from et in dc.EntityTypes
                                          join iet in dc.ItemEntityTypes on et.EntityTypeId equals iet.EntityTypeId
                                          where et.Name.ToLower() == "location"
                                          && iet.ItemId == itemId
                                          select new { et.EntityTypeId }).SingleOrDefault();

                if (locationEntityType == null)
                { 
                    ItemEntityType itemEntity = new ItemEntityType();
                    itemEntity.EntityTypeId = locationEntityTypeId.EntityTypeId;
                    itemEntity.ItemId = itemId;
                    itemEntity.ItemEntityTypeId = Guid.NewGuid();
                    dc.ItemEntityTypes.InsertOnSubmit(itemEntity);
                    dc.SubmitChanges();
                }
            }
            else
            {
                //Delete any existing entries and add a new one.
                var itemEntityType = (from et in dc.ItemEntityTypes
                                      where et.EntityTypeId == locationEntityTypeId.EntityTypeId && et.ItemId == item.ItemId
                                      select et).SingleOrDefault();

                if (itemEntityType != null)
                { 
                    dc.ItemEntityTypes.DeleteOnSubmit(itemEntityType);
                    dc.SubmitChanges();
                }
            }
        }

        UpdateLocationTypes(cblLocationTypes, itemId);
        UpdateItemRoles(cblRolesTypes, itemId);
        UpdateItemEvents(chkCblEvent, itemId);
        UpdateRecoveryStages(cblRecoveryStages, itemId);
        divUploadImage.Visible = true;
    }

    protected void UpdateLocationTypes(CheckBoxList cbl, Guid itemId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        //UPDATE OR INSERT
        foreach (ListItem item in cbl.Items)
        {
            //GET ANY EXISTING RECORDS
            var itemLocationTypes = from ilt in dc.ItemLocationTypes
                            where ilt.ItemId == itemId && ilt.LocationTypeId == new Guid(item.Value)
                            select ilt;

            if (item.Selected)
            {
                if (itemLocationTypes.Count() == 0)
                {
                    //THIS ITEM IS SELECTED BUT NOT IN THE DATABASE
                    //ADD IT
                    ItemLocationType itemLocationType = new ItemLocationType();
                    itemLocationType.ItemId = itemId;
                    itemLocationType.ItemLocationTypeId = Guid.NewGuid();
                    itemLocationType.LocationTypeId = new Guid(item.Value);
                    dc.ItemLocationTypes.InsertOnSubmit(itemLocationType);
                    dc.SubmitChanges();
                }
            }
            else
            {
                //Delete any checked records
                if (itemLocationTypes.Count() > 0)
                {
                    foreach (var itemLocationType in itemLocationTypes)
                    {
                        //Item is selected.
                        dc.ItemLocationTypes.DeleteOnSubmit(itemLocationType);
                        dc.SubmitChanges();
                    }
                }
            }
        }
    }

    protected void UpdateItemRoles(CheckBoxList cbl, Guid itemId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        //UPDATE OR INSERT
        foreach (ListItem item in cbl.Items)
        {
            //GET ANY EXISTING RECORDS
            var itemRoleTypes = from irt in dc.ItemRoleTypes
                            where irt.ItemId == itemId && irt.RoleTypeId == new Guid(item.Value)
                            select irt;

            if (item.Selected)
            {
                if (itemRoleTypes.Count() == 0)
                {
                    //THIS ITEM IS SELECTED BUT NOT IN THE DATABASE
                    //ADD IT
                    ItemRoleType itemRoleType = new ItemRoleType();
                    itemRoleType.ItemId = itemId;
                    itemRoleType.ItemRoleTypeId = Guid.NewGuid();
                    itemRoleType.RoleTypeId = new Guid(item.Value);
                    dc.ItemRoleTypes.InsertOnSubmit(itemRoleType);
                    dc.SubmitChanges();
                }
            }
            else
            {
                //Delete any checked records
                if (itemRoleTypes.Count() > 0)
                {
                    foreach (var itemRoleType in itemRoleTypes)
                    {
                        //Item is selected.
                        dc.ItemRoleTypes.DeleteOnSubmit(itemRoleType);
                        dc.SubmitChanges();
                    }
                }
            }
        }
    }

    protected void UpdateItemEvents(CheckBoxList cbl, Guid itemId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        //UPDATE OR INSERT
        foreach (ListItem item in cbl.Items)
        {
            //GET ANY EXISTING RECORDS
            var itemEvents = from ie in dc.ItemEvents
                                where ie.ItemId == itemId && ie.EventId == new Guid(item.Value)
                                select ie;

            if (item.Selected)
            {
                if (itemEvents.Count() == 0)
                {
                    //THIS ITEM IS SELECTED BUT NOT IN THE DATABASE
                    //ADD IT
                    ItemEvent itemEvent = new ItemEvent();
                    itemEvent.ItemId = itemId;
                    itemEvent.ItemEventId = Guid.NewGuid();
                    itemEvent.EventId = new Guid(item.Value);
                    dc.ItemEvents.InsertOnSubmit(itemEvent);
                    dc.SubmitChanges();
                }
            }
            else
            {
                //Delete any checked records
                if (itemEvents.Count() > 0)
                {
                    foreach (var itemEvent in itemEvents)
                    {
                        //Item is selected.
                        dc.ItemEvents.DeleteOnSubmit(itemEvent);
                        dc.SubmitChanges();
                    }
                }
            }
        }
    }

    protected void UpdateRecoveryStages(CheckBoxList cbl, Guid itemId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        //UPDATE OR INSERT
        foreach (ListItem item in cbl.Items)
        {
            //GET ANY EXISTING RECORDS
            var itemRecoveryStages = from irs in dc.ItemRecoveryStages
                             where irs.ItemId == itemId 
                             && irs.RecoveryStageId == new Guid(item.Value)
                             select irs;

            if (item.Selected)
            {
                if (itemRecoveryStages.Count() == 0)
                {
                    //THIS ITEM IS SELECTED BUT NOT IN THE DATABASE
                    ItemRecoveryStage itemRecoveryStage = new ItemRecoveryStage();
                    itemRecoveryStage.ItemId = itemId;
                    itemRecoveryStage.ItemRecoveryStageId = Guid.NewGuid();
                    itemRecoveryStage.RecoveryStageId = new Guid(item.Value);
                    dc.ItemRecoveryStages.InsertOnSubmit(itemRecoveryStage);
                    dc.SubmitChanges();
                }
            }
            else
            {
                //Delete any checked records
                if (itemRecoveryStages.Count() > 0)
                {
                    foreach (var itemRecoveryStage in itemRecoveryStages)
                    {
                        //Item is selected.
                        dc.ItemRecoveryStages.DeleteOnSubmit(itemRecoveryStage);
                        dc.SubmitChanges();
                    }
                }
            }
        }
    }

    protected void LoadRecoveryStages()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var sliderTicks = from st in dc.SliderTicks
                          where st.SliderId == new Guid("817CFA06-2233-4144-92D6-E3DAB3E0301A") //Recovery Stages
                          orderby st.Tick
                          select st;

        cblRecoveryStages.DataSource = sliderTicks;
        cblRecoveryStages.DataBind();

        var itemRecoveryStages = from irs in dc.ItemRecoveryStages
                                    where irs.ItemId == itemId
                                    select irs;

        //Preselect the orgs for this user
        if (itemRecoveryStages.Count() > 0)
        {
            foreach (var itemRecoveryStage in itemRecoveryStages)
            {
                for (int i = 0; i < cblRecoveryStages.Items.Count; i++)
                {
                    if (itemRecoveryStage.RecoveryStageId.ToString() == cblRecoveryStages.Items[i].Value)
                    {
                        cblRecoveryStages.Items[i].Selected = true;
                    }
                }
            }
        }
    }

    protected void LoadEvents()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var events = from ev in dc.Events
                     orderby ev.BeginDate
                     select ev;

        chkCblEvent.DataSource = events;
        chkCblEvent.DataBind();

        var itemEvents = from ie in dc.ItemEvents
                            where ie.ItemId == itemId
                            select ie;

        //Preselect the orgs for this user
        if (itemEvents.Count() > 0)
        {
            foreach (var itemEvent in itemEvents)
            {
                for (int i = 0; i < chkCblEvent.Items.Count; i++)
                {
                    if (itemEvent.EventId.ToString() == chkCblEvent.Items[i].Value)
                    {
                        chkCblEvent.Items[i].Selected = true;
                    }
                }
            }
        }
    }

    protected void LoadItemRoles()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var roleTypes = from rt in dc.RoleTypes
                        orderby rt.RoleTypeName
                        select rt;

        cblRolesTypes.DataSource = roleTypes;
        cblRolesTypes.DataBind();

        var itemRoleTypes = from irt in dc.ItemRoleTypes
                         where irt.ItemId == itemId
                         select irt;

        //Preselect the orgs for this user
        if (itemRoleTypes.Count() > 0)
        {
            foreach (var itemRoleType in itemRoleTypes)
            {
                for (int i = 0; i < cblRolesTypes.Items.Count; i++)
                {
                    if (itemRoleType.RoleTypeId.ToString() == cblRolesTypes.Items[i].Value)
                    {
                        cblRolesTypes.Items[i].Selected = true;
                    }
                }
            }
        }
    }

    protected void LoadLocationTypes()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var locationParents = from lt in dc.LocationTypes
                              orderby lt.Name
                              select lt;

        cblLocationTypes.DataSource = locationParents;
        cblLocationTypes.DataBind();

        var itemLocationTypes = from ilt in dc.ItemLocationTypes
                            where ilt.ItemId == itemId
                            select ilt;

        //Preselect the orgs for this user
        if (itemLocationTypes.Count() > 0)
        {
            foreach (var itemLocationType in itemLocationTypes)
            {
                for (int i = 0; i < cblLocationTypes.Items.Count; i++)
                {
                    if (itemLocationType.LocationTypeId.ToString() == cblLocationTypes.Items[i].Value)
                    {
                        cblLocationTypes.Items[i].Selected = true;
                    }
                }
            }
        }
    }

    protected void LoadItemTypes(string selectedItemType)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var itemTypes = from it in dc.ItemTypes
                        orderby it.Name
                        select it;

        ddlItemType.DataSource = itemTypes;
        ddlItemType.DataBind();

        ddlItemType.SelectedIndex = ddlItemType.Items.IndexOf(ddlItemType.Items.FindByValue(selectedItemType));
    }

    protected void LoadItemSubTypes(string selectedItemSubType)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var itemSubTypes = from ist in dc.ItemSubTypes
                           orderby ist.Name
                           select ist;

        ddlItemSubType.DataSource = itemSubTypes;
        ddlItemSubType.DataBind();

        ddlItemSubType.SelectedIndex = ddlItemSubType.Items.IndexOf(ddlItemSubType.Items.FindByValue(selectedItemSubType));
    }

    protected void LoadBrands(string selectedBrand)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var brands = from b in dc.Brands
                     orderby b.BrandName
                     select b;

        ddlBrand.DataSource = brands;
        ddlBrand.DataBind();

        ddlBrand.Items.Insert(0, "None");
        ddlBrand.SelectedIndex = ddlBrand.Items.IndexOf(ddlBrand.Items.FindByValue(selectedBrand));
    }
}