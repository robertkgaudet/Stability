using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_Item_Item : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
        if(!String.IsNullOrEmpty(Request.QueryString["itemId"]))
        {
            Guid itemId = new Guid(Request.QueryString["itemId"]);

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            var item = (from i in dc.Items
                       where i.ItemId == itemId
                       select i).SingleOrDefault();

            //Get the brand.
            //Get ItemEntityType
            //Get ItemEvent
            //Get ItemLocationType
            //Get ItemRecoveryStage
            //Get ItemRoleType
            //Get ItemType
            //Get ItemSubType
        }
    }
}