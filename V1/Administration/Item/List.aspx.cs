using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_Item_List : System.Web.UI.Page
{
    Guid itemId = String.IsNullOrEmpty(HttpContext.Current.Request.QueryString["itemId"]) ? Guid.Empty : new Guid(HttpContext.Current.Request.QueryString["itemId"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Master.PageTitle = "Stability - Items";
        this.Master.PageDescription = "";
        this.Master.FbDescription = "";
        this.Master.FbImage = "Images/HurricaneMichael.jpg";
        this.Master.FbImageType = "image/jpg";
        this.Master.FbSite_name = "Stability - Items";
        this.Master.FbURL = Request.Url.AbsoluteUri;

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        if(itemId != null)
        {
            var item = (from i in dc.Items
                       where i.ItemId == itemId
                       select i).SingleOrDefault();

            if(item.IsDeleted != null && (bool)!item.IsDeleted)
            {
                item.IsDeleted = true;
                dc.SubmitChanges();
            }
            else
            {
                item.IsDeleted = false;
                dc.SubmitChanges();
            }
        }

        var items = from i in dc.Items
                    orderby i.UpdatedOn descending
                    select new { i.Name, i.Price, i.SKU, i.ItemId, i.IsDeleted, i.PhotoId, i.URLFriendlyName };

        rpItems.DataSource = items;
        rpItems.DataBind();
    }

    protected void rpItemTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RepeaterItem dataItem = (RepeaterItem)e.Item;
            Guid itemId = (Guid)DataBinder.Eval(dataItem.DataItem, "ItemId");
            Guid photoId = (DataBinder.Eval(dataItem.DataItem, "PhotoId") != null) ? (Guid)DataBinder.Eval(dataItem.DataItem, "PhotoId") : Guid.Empty;
            Decimal price = DataBinder.Eval(dataItem.DataItem, "Price") != null ? (Decimal)DataBinder.Eval(dataItem.DataItem, "Price") : 00;
            String SKU = (String)DataBinder.Eval(dataItem.DataItem, "SKU");
            String URLFriendlyName = (String)DataBinder.Eval(dataItem.DataItem, "URLFriendlyName");
            String name = (String)DataBinder.Eval(dataItem.DataItem, "Name"); 
            Boolean isDeleted = DataBinder.Eval(dataItem.DataItem, "IsDeleted") != null ? (Boolean)DataBinder.Eval(dataItem.DataItem, "IsDeleted") : false;

            Literal litName = (Literal)e.Item.FindControl("litName");
            Literal litPrice = (Literal)e.Item.FindControl("litPrice");
            Literal litSKU = (Literal)e.Item.FindControl("litSKU");
            Literal litDeleted = (Literal)e.Item.FindControl("litDeleted");
            Literal litPhoto = (Literal)e.Item.FindControl("litPhoto"); 
            HyperLink hypDelete = (HyperLink)e.Item.FindControl("hypDelete");
            HyperLink hypItem = (HyperLink)e.Item.FindControl("hypItem");

            hypItem.NavigateUrl = "/V1/DisasterRegistry/Default.aspx?itemId=" + itemId;
            hypItem.Text = name;
            if (!String.IsNullOrEmpty(URLFriendlyName))
            {
                hypItem.NavigateUrl = "/DisasterRegistryItem/" + URLFriendlyName;
            }

            if (photoId != Guid.Empty)
            {
                litPhoto.Text = "Has Photo";
            }

            if(isDeleted)
            {
                hypDelete.Text = "Undelete";
                litDeleted.Text = "(Deleted) ";
            }
            else
            {
                hypDelete.Text = "Delete";
                litDeleted.Text = "(Active) ";
            }

            hypDelete.NavigateUrl = "~/V1/Administration/Item/List.aspx?itemId=" + itemId;

            litPrice.Text = "$" + price;
            litSKU.Text = SKU;
        }
    }
}