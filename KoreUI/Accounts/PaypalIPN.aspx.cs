using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Warpfusion.A4PP.Services;

public partial class Accounts_PaypalIPN : System.Web.UI.Page
{
    ManagementService m_ManagementService = new ManagementService();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Post back to either sandbox or live
            m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            string url = "";;
            if (config["mode"].ToLower() == "sandbox")
            {
                url = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                url = "https://www.paypal.com/cgi-bin/webscr";
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] Param = Request.BinaryRead(HttpContext.Current.Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(Param);
            strRequest = strRequest + "&cmd=_notify-validate";
            req.ContentLength = strRequest.Length;

            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), Encoding.ASCII);
            streamOut.Write(strRequest);
            streamOut.Close();

            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            if (strResponse == "VERIFIED")
            {
                NameValueCollection qscoll = HttpUtility.ParseQueryString(strRequest);
                string agreementID = Convert.ToString(qscoll["recurring_payment_id"]);
                DateTime lastPaymentDate = default(DateTime);
                decimal lastPaymentAmount = default(decimal);
                DateTime nextBillingDate = default(DateTime);

                Agreement agreement = PayPal.Api.Agreement.Get(apiContext, agreementID);

                if ((agreement != null))
                {
                    if ((agreement.agreement_details != null) && (agreement.agreement_details.last_payment_date != null) && (agreement.agreement_details.next_billing_date != null) && (agreement.agreement_details.last_payment_amount != null))
                    {
                        lastPaymentDate = Convert.ToDateTime(agreement.agreement_details.last_payment_date);
                        nextBillingDate = Convert.ToDateTime(agreement.agreement_details.next_billing_date);
                        lastPaymentAmount = Convert.ToDecimal(agreement.agreement_details.last_payment_amount.value);
                        m_ManagementService.UpdateBillingSubscription(agreementID, lastPaymentDate, lastPaymentAmount, nextBillingDate);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            m_ManagementService.LogError(ex.Message, ex.StackTrace);
        }
        
    }
}