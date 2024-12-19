using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_MasterPages_1_Column_Child : System.Web.UI.MasterPage
{
	public string _coverImage = string.Empty;
	public string _pageName = string.Empty;
	public string _coverImageFile = "businesscoverimage.png";
	protected void Page_Load(object sender, EventArgs e)
	{
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "businesscoverimage.png";
		Master.PageTitle = _pageName;
		Master.FbImage = _coverImage;
	}
	public string CoverImageFile
	{
		get
		{
			return _coverImageFile;
		}
		set
		{
			_coverImageFile = value;
		}
	}
	public string PageName
	{
		get
		{
			return _pageName;
		}
		set
		{
			_pageName = value;
		}
	}
}
