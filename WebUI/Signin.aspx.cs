using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Signin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                string qmessage = Request.QueryString["msg"].ToString();
                lblErrorMessage.Text = "Thank you !" + " " + qmessage;
            }
        }
    }
}