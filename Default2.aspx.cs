using System;
using System.Web.UI;

public partial class Default2 : Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			// You can initialize the Select2 options here if needed
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		// Retrieve the selected tags
		string selectedTags = Request.Form[txtTags.ClientID];

		// Process the tags as needed
		// For example, display them
		Response.Write("Selected Emails: " + selectedTags);
	}
}
