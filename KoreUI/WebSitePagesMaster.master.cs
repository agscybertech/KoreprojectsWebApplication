using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Warpfusion.A4PP.Objects;
using Warpfusion.A4PP.Services;
using Warpfusion.Shared.Utilities;

public partial class WebSitePagesMaster : System.Web.UI.MasterPage
{
    ManagementService m_ManagementService = new ManagementService();
    ScopeServices m_ScopeService = new ScopeServices();
    User m_LoginUser;
    string m_jsString;

    protected void Page_Load(object sender, EventArgs e)
    {
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
    }

    protected void btnSignup_Click(object sender, System.EventArgs e)
    {
        if (m_ManagementService.GetUserCountByEmail(txtUser.Text) == 0)
        {
            User signUser = new User();
            VoucherCodeFunctions cVoucherCode = new VoucherCodeFunctions();
            string strIdentifier = null;
            signUser.Email = txtUser.Text;
            signUser.Password = txtPassword.Text;
            signUser.Type = 2;
            long signUserID = m_ManagementService.CreateUser(signUser, 0);
            UserProfile signUserProfile = new UserProfile();
            signUserProfile.UserId = signUserID;
            signUserProfile.Email = txtUser.Text;
            long userProfileId = m_ManagementService.CreateUserProfile(signUserProfile);
            signUserProfile.UserProfileId = userProfileId;
            strIdentifier = string.Format("{0}{1}", userProfileId, cVoucherCode.GenerateVoucherCodeGuid(16));
            signUserProfile.Identifier = strIdentifier;
            m_ManagementService.UpdateUserProfileIdentifier(signUserProfile);
            ProjectOwner signUserOwner = new ProjectOwner();
            signUserOwner.ContactId = signUserID;
            long intCompanyID = m_ManagementService.CreateProjectOwner(signUserOwner);
            signUserOwner.ProjectOwnerId = intCompanyID;
            strIdentifier = string.Format("{0}{1}", cVoucherCode.GenerateVoucherCodeGuid(16), intCompanyID);
            signUserOwner.Identifier = strIdentifier;
            m_ManagementService.UpdateProjectOwnerIdentifier(signUserOwner);
            m_LoginUser = m_ManagementService.Login(txtUser.Text.Trim(), txtPassword.Text.Trim());
            if ((m_LoginUser != null))
            {
                int numberOfProjects = 0;
                string strPromoCode = ConfigurationManager.AppSettings["PromoCode_SignUp"].ToString();
                bool PromoCodeValid = false;
                PromoCodeValid = m_ManagementService.CheckPromoCodeValidByPromoCodeUserId(strPromoCode, m_LoginUser.UserId);
                if (PromoCodeValid)
                {
                    DataSet dsPlan = default(DataSet);
                    dsPlan = m_ManagementService.GetPlanByPromoCodeUserId(strPromoCode, m_LoginUser.UserId);
                    if (dsPlan.Tables.Count > 0)
                    {
                        if (dsPlan.Tables[0].Rows.Count > 0)
                        {
                            int PlanId = 0;
                            if (dsPlan.Tables[0].Rows[0]["PlanId"] != DBNull.Value)
                            {
                                PlanId = Convert.ToInt32(dsPlan.Tables[0].Rows[0]["PlanId"]);
                            }
                            if (dsPlan.Tables[0].Rows[0]["NumberOfProjects"] != DBNull.Value)
                            {
                                numberOfProjects = Convert.ToInt32(dsPlan.Tables[0].Rows[0]["NumberOfProjects"]);
                            }
                            decimal storageSize = 0;
                            if (dsPlan.Tables[0].Rows[0]["StorageSize"] != DBNull.Value)
                            {
                                storageSize = Convert.ToDecimal(dsPlan.Tables[0].Rows[0]["StorageSize"]);
                            }
                            int term = 0;
                            if (dsPlan.Tables[0].Rows[0]["Term"] != DBNull.Value)
                            {
                                term = Convert.ToInt32(dsPlan.Tables[0].Rows[0]["Term"]);
                            }
                            DateTime NextBillingDate = default(DateTime);
                            NextBillingDate = DateTime.Now.AddMonths(term);
                            m_ManagementService.UpdateUserAccountMonthly(m_LoginUser.UserId, PlanId, numberOfProjects, storageSize, NextBillingDate);
                            m_ManagementService.CreatePromotionRedeemed(strPromoCode, m_LoginUser.UserId);
                            m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, string.Format("Promotion - Redeem {0}", strPromoCode), 0, 0, numberOfProjects, numberOfProjects);
                        }
                    }
                }

                Session["CurrentLogin"] = m_LoginUser;

                System.Net.Mail.MailMessage MailMessage = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient emailClient = new System.Net.Mail.SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
                //MailMessage.To.Add(New System.Net.Mail.MailAddress(ContactProfile.Email))
                MailMessage.To.Add(new System.Net.Mail.MailAddress(m_LoginUser.Email));
                MailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["AdminEmail"].ToString());

                MailMessage.Subject = string.Format("Welcome to {0}", Request.Url.Host.Replace("www.", string.Empty));

                if (numberOfProjects < 1)
                {
                    MailMessage.Body = string.Format("Hi there,<br><br>Welcome to join Kore Projects.<br><br>Your account has been activated.<br><br>The Kore Projects Team");
                }
                else
                {
                    if (numberOfProjects == 1)
                    {
                        MailMessage.Body = string.Format("Hi there,<br><br>Welcome to Kore Projects.<br><br>Your account has been activated with {0} project.<br>To visit Kore Projects please use the link below:<br><br>https://{1}<br><br>The Kore Projects Team", numberOfProjects, Request.Url.Authority);
                    }
                    else
                    {
                        MailMessage.Body = string.Format("Hi there,<br><br>Welcome to Kore Projects.<br><br>Your account has been activated with {0} projects.<br>To visit Kore Projects please use the link below:<br><br>https://{1}<br><br>The Kore Projects Team", numberOfProjects, Request.Url.Authority);
                    }
                }

                MailMessage.IsBodyHtml = true;
                try
                {
                    emailClient.Send(MailMessage);
                }
                catch (Exception ex)
                {
                    //Response.Redirect(String.Format("View.aspx?&msg=Invitation hasn't been sent - { 0}", ex.Message))
                    //throw;
                }

                ServiceGroup objServiceGroup = new ServiceGroup();
                objServiceGroup.UserId = m_LoginUser.UserId;
                objServiceGroup.Name = "Rate Sheet 1";
                int intDisplayOrder = 0;
                objServiceGroup.DisplayOrder = intDisplayOrder;
                objServiceGroup.IsPrivate = 2;
                m_ScopeService.CreateServiceGroup(objServiceGroup);
                objServiceGroup = new ServiceGroup();
                objServiceGroup.UserId = m_LoginUser.UserId;
                objServiceGroup.Name = "Rate Sheet 2";
                objServiceGroup.DisplayOrder = intDisplayOrder;
                objServiceGroup.IsPrivate = 2;
                m_ScopeService.CreateServiceGroup(objServiceGroup);

                if (Session["AcceptInvitation"] == null)
                {
                    //m_jsString = "parent.location.href='Projects/View.aspx';"
                    m_jsString = "parent.location.href='Contacts/ProjectOwnerDetail.aspx?user=new';";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ProjectsView", m_jsString, true);
                }
                else
                {
                    Session["AcceptInvitation"] = null;
                    m_jsString = "parent.location.href='Contacts/ProjectOwnerDetail.aspx?user=new';";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ProjectOwnerDetail", m_jsString, true);
                }
            }
            else
            {
                lblErrorMessage.Text = "The user email already exists. Please use a different email address.<br>Already a member? Click <a class='loginlink' onclick=\"parent.location.href='http://www.koreprojects.com/login.asp';\" href='#'>here</a> to login.";
                lblErrorMessage.Focus();
            }
        }
        else
        {
            lblErrorMessage.Text = "The user email already exists. Please use a different email address.<br>Already a member? Click <a class='loginlink' onclick=\"parent.location.href='http://www.koreprojects.com/login.asp';\" href='#'>here</a> to login.";
            lblErrorMessage.Focus();
        }
    }
}
