Imports System
Imports System.Collections
Imports System.Linq
Imports System.Configuration
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports System.Data
Imports System.Data.SqlClient

Partial Class Contents_HealthSafetyManager
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner
    Private m_ProjectID As Integer = 0
    Private m_ProjectOwnerId As Long = 0
    Private m_DynamicPageId As Integer = 0
    Public m_Cryption As New Cryption()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("msg") <> String.Empty Then
            lblMsg.Text = Request.QueryString("msg")
        End If

        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        m_ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_LoginUser.CompanyId)
        m_ProjectOwnerId = m_LoginUser.CompanyId
        m_DynamicPageId = 1
        LoadContents()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Private Sub LoadContents()
        Dim dsDynamicPageContents As DataSet
        dsDynamicPageContents = m_ManagementService.GetDynamicPageContentsByProjectOwnerId(m_ProjectOwnerId)
        If dsDynamicPageContents.Tables.Count > 0 Then
            rptDynamicPageContents.DataSource = dsDynamicPageContents.Tables(0)
            rptDynamicPageContents.DataBind()

            Dim intContentTypeId As Integer
            Dim objHtmlGeneric As HtmlGenericControl
            Dim strContent As String
            Dim objRegex As Regex = New Regex("(\r\n|\r|\n)+")

            For Each repeatDataItem As RepeaterItem In rptDynamicPageContents.Items
                objHtmlGeneric = New HtmlGenericControl("h2")              
                objHtmlGeneric.InnerHtml = String.Format("{0}.", dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("DisplayOrder"))
                repeatDataItem.Controls.Add(objHtmlGeneric)
                intContentTypeId = dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTypeId")
                Select Case intContentTypeId
                    Case 1
                        If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle")) Then
                            objHtmlGeneric = New HtmlGenericControl("h2")
                            objHtmlGeneric.InnerHtml = dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle")
                            repeatDataItem.Controls.Add(objHtmlGeneric)
                        End If

                        If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData")) Then
                            objHtmlGeneric = New HtmlGenericControl("div")
                            strContent = String.Format("{0}", dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData"))
                            strContent = objRegex.Replace(strContent, "<br /><br />")
                            objHtmlGeneric.InnerHtml = strContent
                            repeatDataItem.Controls.Add(objHtmlGeneric)
                        End If
                    Case 2
                        If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData")) Then
                            objHtmlGeneric = New HtmlGenericControl("img")
                            objHtmlGeneric.Attributes.Add("src", String.Format("../images/{0}/{1}", m_ProjectOwner.Identifier, dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData")))
                            objHtmlGeneric.Attributes.Add("style", "width:auto; max-width:265px; height:auto; max-height:300px")
                            repeatDataItem.Controls.Add(objHtmlGeneric)
                            objHtmlGeneric = New HtmlGenericControl("br")
                            repeatDataItem.Controls.Add(objHtmlGeneric)
                        End If

                        If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle")) Then
                            objHtmlGeneric = New HtmlGenericControl("span")
                            objHtmlGeneric.InnerHtml = dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle")
                            'objHtmlGeneric.Attributes.Add("style", "width:auto; max-width:265px; text-align:center")
                            repeatDataItem.Controls.Add(objHtmlGeneric)
                        End If
                    Case 3
                        If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle")) Then
                            If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData")) Then
                                'objHtmlGeneric = New HtmlGenericControl("a")
                                'objHtmlGeneric.InnerHtml = dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle")
                                'objHtmlGeneric.Attributes.Add("href", String.Format("../images/{0}/{1}", m_ProjectOwner.Identifier, dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData")))
                                'objHtmlGeneric.Attributes.Add("target", "_blank")
                                'repeatDataItem.Controls.Add(objHtmlGeneric)

                                objHtmlGeneric = New HtmlGenericControl()
                                If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle")) Then
                                    If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData")) Then
                                        objHtmlGeneric.InnerHtml = String.Format("<table><tr><td align='center'><b>{0}</b><br /><a target = '_blank' href='../images/{1}/{2}'><img src='../images/pdf_icon2.gif' border='0' /></a></td></tr></table>", dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle"), m_ProjectOwner.Identifier, dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData"))
                                    Else
                                        objHtmlGeneric.InnerHtml = String.Format("<table><tr><td align='center'><b>{0}</b></td></tr></table>", dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle"))
                                    End If
                                Else
                                    If Not IsDBNull(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData")) Then
                                        objHtmlGeneric.InnerHtml = String.Format("<table><tr><td align='center'><a target = '_blank' href='../images/{0}/{1}'><img src='../images/pdf_icon2.gif' border='0' /></a></td></tr></table>", m_ProjectOwner.Identifier, dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData"))
                                    End If
                                End If
                                repeatDataItem.Controls.Add(objHtmlGeneric)

                                'objHtmlGeneric = New HtmlGenericControl("a")
                                'objHtmlGeneric.Attributes.Add("href", String.Format("../downloadfile.ashx?filename={0}", m_Cryption.Encrypt(String.Format("/images/{0}/{1}", m_ProjectOwner.Identifier, dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentData")), m_Cryption.cryptionKey)))
                                'objHtmlGeneric.Attributes.Add("style", "float:right;")
                                'objHtmlGeneric.Attributes.Add("id", "MiddleContent_button")
                                'objHtmlGeneric.InnerText = "Download"
                                'repeatDataItem.Controls.Add(objHtmlGeneric)
                                'objHtmlGeneric = New HtmlGenericControl("br")
                                'repeatDataItem.Controls.Add(objHtmlGeneric)
                            Else
                                objHtmlGeneric = New HtmlGenericControl("span")
                                objHtmlGeneric.InnerHtml = dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("ContentTitle")
                                repeatDataItem.Controls.Add(objHtmlGeneric)
                            End If
                        End If
                End Select

                objHtmlGeneric = New HtmlGenericControl("a")
                objHtmlGeneric.Attributes.Add("href", String.Format("HealthSafetyEditor.aspx?id={0}", m_Cryption.Encrypt(dsDynamicPageContents.Tables(0).Rows(repeatDataItem.ItemIndex)("DynamicPageContentId").ToString(), m_Cryption.cryptionKey)))
                objHtmlGeneric.Attributes.Add("class", "form_popup")
                objHtmlGeneric.Attributes.Add("style", "float:right;")
                objHtmlGeneric.Attributes.Add("id", "MiddleContent_button")
                objHtmlGeneric.InnerText = "Edit"
                repeatDataItem.Controls.Add(objHtmlGeneric)
                objHtmlGeneric = New HtmlGenericControl("br")
                repeatDataItem.Controls.Add(objHtmlGeneric)
                objHtmlGeneric = New HtmlGenericControl("br")
                repeatDataItem.Controls.Add(objHtmlGeneric)
            Next
        End If
    End Sub
End Class