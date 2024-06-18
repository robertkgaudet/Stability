using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;

public partial class S1_MasterPages_HomerNoNavigation : System.Web.UI.MasterPage
{
	public string _pageTitle = string.Empty;
	public string _pageDescription = string.Empty;
	public string _fbImage = string.Empty;
	public string _fbURL = string.Empty;
	public string _fbImageType = string.Empty;
	public string _fbSite_name = string.Empty;
	public string _fbDescription = string.Empty;
	public bool _hideCategoryList = false;
	public bool _hideHeader = false;
	public bool _hideFooter = false;
	public bool _hideMenu = false;
	public bool _boxedBody = false;
	public string bodyTag = string.Empty;
	public string boxedWrapperOpen = string.Empty;
	public string boxedWrapperClosed = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{

		//title.Text = PageTitle;
		//description.Attributes.Add("content", PageTitle);

		//fbTitle.Attributes.Add("content", PageTitle);
		//fbImage.Attributes.Add("content", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + FbImage);
		//fbURL.Attributes.Add("content", FbURL);
		//fbImageType.Attributes.Add("content", FbImageType);// content="image/jpeg" content="image/png"
		//fbSite_name.Attributes.Add("content", FbSite_name);
		//fbDescription.Attributes.Add("content", FbDescription);
	}

	public bool BoxedBody
	{
		get
		{
			return _boxedBody;
		}
		set
		{
			_boxedBody = value;
		}
	}
	public bool HideCategoryList
	{
		get
		{
			return _hideCategoryList;
		}
		set
		{
			_hideCategoryList = value;
		}
	}
	public bool HideMenu
	{
		get
		{
			return _hideMenu;
		}
		set
		{
			_hideMenu = value;
		}
	}
	public bool HideFooter
	{
		get
		{
			return _hideFooter;
		}
		set
		{
			_hideFooter = value;
		}
	}
	public bool HideHeader
	{
		get
		{
			return _hideHeader;
		}
		set
		{
			_hideHeader = value;
		}
	}

	public string PageTitle
	{
		get
		{
			return _pageTitle;
		}
		set
		{
			_pageTitle = value;
		}
	}
	public string PageDescription
	{
		get
		{
			return _pageDescription;
		}
		set
		{
			_pageDescription = value;
		}
	}
	public string FbImage
	{
		get
		{
			return _fbImage;
		}
		set
		{
			_fbImage = value;
		}
	}
	public string FbURL
	{
		get
		{
			return _fbURL;
		}
		set
		{
			_fbURL = value;
		}
	}
	public string FbImageType
	{
		get
		{
			return _fbImageType;
		}
		set
		{
			_fbImageType = value;
		}
	}
	public string FbSite_name
	{
		get
		{
			return _fbSite_name;
		}
		set
		{
			_fbSite_name = value;
		}
	}
	public string FbDescription
	{
		get
		{
			return _fbDescription;
		}
		set
		{
			_fbDescription = value;
		}
	}
}