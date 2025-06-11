using CrowdRelief;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_PeopleSearch : System.Web.UI.UserControl
{
    public void BindData(IEnumerable<object> data)
    {
        rptPeopleSearch.DataSource = data;
        rptPeopleSearch.DataBind();
    }
}