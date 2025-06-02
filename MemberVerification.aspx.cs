using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MemberVerification : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{

	}

	protected void btnFindProfile_Click(object sender, EventArgs e)
	{
		lblMessage.Text = ""; // Clear previous messages

		int profileId;
		if (!int.TryParse(txtProfileId.Text.Trim(), out profileId))
		{
			lblMessage.Text = "Invalid Profile ID format. Please enter a number.";
			return;
		}

		using (var dc = new CrowdReliefDBDataContext()) // Replace with your actual LINQ to SQL DataContext
		{
			var profile = dc.Profiles.FirstOrDefault(p => p.ProfileNumber == profileId);

			if (profile != null)
			{
				// Assuming profile.UserId is a Guid used in the member page URL
				Response.Redirect("/V1/Member/Default.aspx?userid=" + profile.UserId);
			}
			else
			{
				lblMessage.Text = "No member found with that Profile ID.";
			}
		}
	}
}