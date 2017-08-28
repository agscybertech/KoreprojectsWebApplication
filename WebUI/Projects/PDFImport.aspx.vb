Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports org.apache.pdfbox.pdmodel
Imports org.apache.pdfbox.util
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class Projects_PDFImport
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Private m_Cryption As New Cryption
    Private m_Project As New Project
    Private m_ProjectId As Long
    Private m_User As New User
    Private m_UserID As Integer
    Private m_UserProfile As New UserProfile
    Private m_UserProfileID As Integer
    Private m_Scope As New Scope
    Private m_ScopeID As Integer
    Private m_ScopeItem As New ScopeItem
    Private m_OwnerProfile As UserProfile
    Private m_EmailType As Integer

    Public Function ReadPdfFile() As String
        Dim sbText As New StringBuilder
        Dim strFileName As String = "C:\Users\ping\Desktop\Support\a4plasterandpaint\ProjectScope\A4PP_Phase2_Scope\WebUI\PDF\2011 017980 SOD.PDF"
        Dim pdfReader As New iTextSharp.text.pdf.PdfReader(strFileName)
        Dim sOut As String = String.Empty

        For i = 1 To pdfReader.NumberOfPages
            Dim its As New iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy
            'Dim its As New iTextSharp.text.pdf.parser.LocationTextExtractionStrategy

            'Out &= iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, i, its) & "<br><br>"
            'Dim s As String = PdfTextExtractor.GetTextFromPage(pdfReader, i, its)
            Dim sc As Array = PdfTextExtractor.GetTextFromPage(pdfReader, i, its).ToCharArray

            For j = 0 To UBound(sc) - 1
                'Response.Write("test=" & Asc(sc(j)) & "<br>")
                If Asc(sc(j)) <> 10 And Asc(sc(j)) <> 32 Then
                    'Response.Write(sc(j))
                    If Asc(sc(j)) < 32 Or Asc(sc(j)) > 126 Then
                        sOut = sOut & "[" & Asc(sc(j)) & "]"
                    Else
                        sOut = sOut & sc(j)
                    End If
                Else
                    If Asc(sc(j)) = 32 Then
                        sOut = sOut & "[" & Asc(sc(j)) & "]"
                    Else
                        sOut = sOut & "<br>" & sc(j)
                    End If

                    'Response.Write("<br>")
                End If
            Next
            'Exit For
            's = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)))
            'Replace(s, Chr(10), "<br>")
            'Replace(s, Chr(13) & Chr(10), "<br>")

            'Dim st As Array = s.Split("\n")
            'Response.Write("<br>test=" & UBound(st) & "<br>")
            'sOut = sOut & "<br><br>" & s
            sOut = sOut & "<br><br><br><br>"
        Next

        'Dim readerPDF As New iTextSharp.text.pdf.PdfReader("C:\Users\ping\Desktop\Support\a4plasterandpaint\ProjectScope\A4PP_Phase2_Scope\WebUI\PDF\2011 017980 SOD.PDF")
        'Dim PDFfld As Object
        'sOut = sOut & "test<br><br>" & readerPDF.AcroFields.Fields.Count
        'For Each PDFfld In readerPDF.AcroFields.Fields
        '    If sOut = "" Then
        '        sOut = PDFfld.key.ToString() & "<br>"
        '    Else
        '        sOut = sOut & Environment.NewLine & PDFfld.key.ToString() & "<br>"
        '    End If
        'Next

        'Dim reader As StreamReader = New StreamReader(pdfReader.OpenFile(strFileName))

        'Return reader.ReadToEnd()
        Return sOut
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        'm_LoginUser.UserId = 1
        'm_LoginUser.CompanyId = 6
        m_OwnerProfile = m_ManagementService.GetUserProfileByUserID(m_LoginUser.UserId)
        'Label1.Text = ReadPdfFile()
        'Label1.Text = pdf2txt()
        lblTitle.Text = "Import EQC PDF"
    End Sub

    Public Function pdf2txt(ByVal FilePath As String) As String
        Dim strFileName As String = FilePath
        Dim doc As PDDocument = PDDocument.load(strFileName)
        Dim pdfStripper As New PDFTextStripper
        'pdfStripper.setSortByPosition(True)
        'pdfStripper.setShouldSeparateByBeads(True)
        pdfStripper.setWordSeparator("#|#")
        pdfStripper.setLineSeparator("<br>")
        'pdfStripper.setPageSeparator("<br><br>")
        Dim sOut As String = stringSectionFilter(pdfStripper.getText(doc))
        'Dim swPdfChange As StreamWriter = New StreamWriter(strFileName, False)
        'swPdfChange.Write(sOut)
        'swPdfChange.Close()

        Return sOut
    End Function

    Public Function stringSectionFilter(ByVal txtOrg As String) As String
        Dim m_ConnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("AddressService").ConnectionString
        Dim m_SqlConn As New SqlConnection(m_ConnString)

        Dim strBlock As Array = Split(txtOrg, "<br>")
        Dim strSection As String = String.Empty
        Dim strList As New ArrayList
        Dim idxRoomElement As Integer
        Dim idxItem As Integer
        Dim idxAddress As Integer
        Dim idxContact As Integer
        Dim idxGroup As Integer
        Dim strGroup As String = String.Empty
        Dim intGroupID As Integer
        Dim idxArea As Integer
        Dim strArea As String = String.Empty
        Dim strAreaMeasurement As String = String.Empty
        Dim idxStructure As Integer
        Dim blnItemCreated As Boolean
        Dim idxDescription As Integer
        Dim idxSubTitle As Integer        

        If IsArray(strBlock) Then
            If UBound(strBlock) > 0 Then
                For i = 0 To UBound(strBlock)
                    If strBlock(i).ToString.IndexOf("Assessment") > 0 Or strBlock(i).ToString.IndexOf("v3.00c") > 0 Or strBlock(i).ToString.IndexOf("v2.00o") > 0 Then
                    ElseIf strBlock(i).ToString.IndexOf("Page ") >= 0 And strBlock(i).ToString.IndexOf(" of ") > 0 And strBlock(i).ToString.IndexOf(" of ") > strBlock(i).ToString.IndexOf("Page ") Then
                        If m_EmailType = 0 Then
                            m_EmailType = 1
                        End If
                    Else
                        'If i = 0 Then
                        '    strSection = String.Format("{0}", strBlock(i))
                        'Else
                        '    If strBlock(i).ToString.IndexOf("Services") > 0 Or strBlock(i).ToString.IndexOf("Site") > 0 Or strBlock(i).ToString.IndexOf("BUILDING") > 0 Then
                        '        strSection = String.Format("{0}<br><br>==========================================================================================<br><br>{1}", strSection, strBlock(i))
                        '    ElseIf strBlock(i).ToString.IndexOf("Element") >= 0 Then
                        '        strSection = String.Format("{0}<br><br>{1}", strSection, strBlock(i))
                        '        'ElseIf strBlock(i).ToString.IndexOf("Comments") >= 0 Then
                        '        'strSection = String.Format("{0}<br>{1}<br><br>", strSection, strBlock(i))
                        '    ElseIf strBlock(i).ToString.IndexOf("Additional") > 0 Then
                        '        strSection = String.Format("{0}<br>{1}<br><br>", strSection, strBlock(i))
                        '    Else
                        '        strSection = String.Format("{0}<br>{1}", strSection, strBlock(i))
                        '    End If
                        'End If

                        If strBlock(i).ToString.IndexOf("Address#|#") = 0 Then
                            Dim strAddress As Array
                            strAddress = Split(Replace(strBlock(i).ToString, "Address#|#", ""), ",")
                            If UBound(strAddress) > 0 Then
                                m_Project.Address = Trim(strAddress(0))
                                m_Project.Name = m_Project.Address
                                m_Project.Suburb = Trim(strAddress(1))
                            End If
                            strList.Add(String.Format("{0}", strBlock(i)))
                            idxAddress = 1
                        ElseIf idxAddress = 1 And strBlock(i).ToString.IndexOf("EQC Claim Number#|#") < 0 Then
                            Dim strAddress As Array
                            strAddress = Split(strBlock(i).ToString, ",")
                            If UBound(strAddress) > 1 Then
                                m_Project.Suburb = Trim(strAddress(0))
                                m_Project.City = Trim(strAddress(1))
                                m_Project.PostCode = Trim(strAddress(2))
                            ElseIf UBound(strAddress) > 0 Then
                                m_Project.City = Trim(strAddress(0))
                                m_Project.PostCode = Trim(strAddress(1))
                            End If
                            strList.Add(String.Format("{0}", strBlock(i)))
                        ElseIf strBlock(i).ToString.IndexOf("EQC Claim Number#|#") = 0 Then
                            m_Project.EQCClaimNumber = Trim(Replace(strBlock(i).ToString, "EQC Claim Number#|#", ""))
                            strList.Add(String.Format("{0}", strBlock(i)))
                            idxAddress = 0
                        ElseIf strBlock(i).ToString.IndexOf("Hazards#|#") = 0 Then
                            If Replace(strBlock(i).ToString, "Hazards#|#", "") = "Nil" Then
                                m_Project.Hazard = String.Empty
                            Else
                                m_Project.Hazard = Trim(Replace(strBlock(i).ToString, "Hazards#|#", ""))
                            End If
                            strList.Add(String.Format("{0}", strBlock(i)))
                        ElseIf strBlock(i).ToString.IndexOf("Inspection Date") = 0 Then
                            Dim dtScope As DateTime
                            If DateTime.TryParse(Trim(Replace(strBlock(i).ToString, "Inspection Date#|#", "")), dtScope) Then
                                'm_Project.ScopeDate = dtScope
                            End If
                            strList.Add(String.Format("{0}", strBlock(i)))
                        ElseIf strBlock(i).ToString.IndexOf("Work Phone#|#Mobile Phone") = 0 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            idxContact = 1
                        ElseIf idxContact = 1 Then
                            Dim strContact As Array
                            strContact = Split(strBlock(i).ToString, "#|#")
                            If UBound(strContact) > 0 Then
                                If strContact(0).lastindexof(" ") > 0 Then
                                    m_UserProfile.FirstName = Trim(strContact(0).substring(0, strContact(0).lastindexof(" ")))
                                    m_UserProfile.LastName = Trim(strContact(0).substring(strContact(0).lastindexof(" ") + 1, strContact(0).length - strContact(0).lastindexof(" ") - 1))
                                End If
                                If strContact(1).indexof(" 64") > 0 Then
                                    m_UserProfile.Contact1 = Trim(strContact(1).substring(0, strContact(1).indexof(" 64")))
                                    strContact(1) = Trim(strContact(1).substring(strContact(1).indexof(" 64") + 1, strContact(1).length - strContact(1).indexof(" 64") - 1))
                                    If strContact(1).indexof(" 64") > 0 Then
                                        m_UserProfile.Contact2 = Trim(strContact(1).substring(0, strContact(1).indexof(" 64")))
                                        strContact(1) = Trim(strContact(1).substring(strContact(1).indexof(" 64") + 1, strContact(1).length - strContact(1).indexof(" 64") - 1))
                                        If strContact(1).lastindexof(" ") > 0 Then
                                            m_UserProfile.Contact3 = Trim(strContact(1).substring(0, strContact(1).lastindexof(" ")))
                                            m_UserProfile.Email = Trim(strContact(1).substring(strContact(1).lastindexof(" ") + 1, strContact(1).length - strContact(1).lastindexof(" ") - 1))
                                            m_User.Email = m_UserProfile.Email

                                            m_User.Type = 0
                                            m_UserID = SaveUser(m_User)
                                            m_User.UserId = m_UserID
                                            m_UserProfile.UserId = m_UserID
                                            m_UserProfileID = SaveProfile(m_UserProfile)
                                            m_UserProfile.UserProfileId = m_UserProfileID

                                            Dim cVoucherCode As New VoucherCodeFunctions
                                            Dim strIdentifier As String = String.Format("{0}{1}", m_UserProfileID, cVoucherCode.GenerateVoucherCodeGuid(16))
                                            m_UserProfile.Identifier = strIdentifier
                                            m_ManagementService.UpdateUserProfileIdentifier(m_UserProfile)

                                            m_Project.ContactId = m_UserID
                                            m_Project.ProjectOwnerId = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId).ProjectOwnerId
                                            m_Project.ProjectStatusId = 0
                                            m_Project.GroupID = 0
                                            m_Project.GroupName = String.Empty
                                            m_Project.Region = "Canterbury"
                                            m_Project.RegionID = 14
                                            m_Project.Country = "NEW ZEALAND"
                                            m_Project.CountryID = 554
                                            If Not m_Project.City Is Nothing Then
                                                Dim dsCity As New DataSet
                                                Dim m_SqlRCommand As SqlCommand = New SqlCommand("Select DistrictCityId as DistrictId,DisplayName as District From tbl_DistrictCity Where RegionId='14' and DisplayName = '" & m_Project.City & "' Order By DisplayName", m_SqlConn)
                                                Dim m_SqlRDataAdapter As SqlDataAdapter = New SqlDataAdapter(m_SqlRCommand)
                                                m_SqlRDataAdapter.Fill(dsCity, "District")
                                                If dsCity.Tables.Count > 0 Then
                                                    If dsCity.Tables(0).Rows.Count > 0 Then
                                                        m_Project.CityID = dsCity.Tables(0).Rows(0)("DistrictId")
                                                    End If
                                                End If
                                            End If
                                            If Not m_Project.Suburb Is Nothing And m_Project.CityID <> 0 Then
                                                Dim dsSuburb As New DataSet
                                                Dim m_SqlRCommand As SqlCommand = New SqlCommand("Select TownSuburbId as TownId,DisplayName as Town From tbl_TownSuburb Where DistrictCityId='" & m_Project.CityID & "' and DisplayName = '" & m_Project.Suburb & "' Order By DisplayName", m_SqlConn)
                                                Dim m_SqlRDataAdapter As SqlDataAdapter = New SqlDataAdapter(m_SqlRCommand)
                                                m_SqlRDataAdapter.Fill(dsSuburb, "Town")
                                                If dsSuburb.Tables.Count > 0 Then
                                                    If dsSuburb.Tables(0).Rows.Count > 0 Then
                                                        m_Project.SuburbID = dsSuburb.Tables(0).Rows(0)("TownId")
                                                    End If
                                                End If
                                            End If

                                            m_ProjectId = SaveProject(m_Project)
                                            m_Project.ProjectId = m_ProjectId

                                            Dim UserProjectStatusValue As UserProjectStatusValue = New UserProjectStatusValue
                                            UserProjectStatusValue.ProjectId = m_Project.ProjectId
                                            UserProjectStatusValue.UserId = m_LoginUser.UserId
                                            UserProjectStatusValue.UserProjectStatusValue = -1000
                                            m_ManagementService.CreateUserProjectStatusValue(UserProjectStatusValue)

                                            Dim projectCredit As Integer = 0
                                            projectCredit = GetProjectCreditBalance()
                                            If projectCredit > 0 Then
                                                m_ManagementService.UpdateUserAccount(m_LoginUser.UserId, projectCredit - 1)
                                                m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, String.Format("Create Project", m_Project.Name), 0, 0, -1, projectCredit - 1)
                                            End If

                                            m_Scope.ProjectId = m_ProjectId
                                            m_Scope.GSTRate = Decimal.Parse(ConfigurationManager.AppSettings("GST"))
                                            m_ScopeID = SaveScope(m_Scope)
                                            m_Scope.ScopeId = m_ScopeID
                                        End If
                                    End If
                                End If
                                idxContact = 0
                            Else
                                idxContact = 1
                            End If
                            strList.Add(String.Format("{0}", strBlock(i)))
                        ElseIf strBlock(i).ToString.IndexOf("Services") > 0 Or strBlock(i).ToString.IndexOf("Site") > 0 Or strBlock(i).ToString.IndexOf("BUILDING") > 0 Then
                            strList.Add(String.Format("<br><br>==========================================================================================<br><br>"))
                            strList.Add(String.Format("{0}", strBlock(i)))
                        ElseIf strBlock(i).ToString.IndexOf("Building - ") > 0 Then
                            strList.Add(String.Format("<br><br>==========================================================================================<br><br>"))
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strGroup = "Structure"
                            Dim objWorksheetGroup As New WorksheetGroup
                            objWorksheetGroup.ProjectOwnerId = m_LoginUser.CompanyId
                            objWorksheetGroup.Name = strGroup
                            objWorksheetGroup.DisplayOrder = 0
                            intGroupID = m_ScopeService.CreateWorksheetGroup(objWorksheetGroup)
                            idxGroup = 1
                            idxArea = 0
                            strArea = Trim(String.Format("{0}", strBlock(i).ToString.Substring(strBlock(i).ToString.IndexOf("Building - ") + 11)))
                            idxStructure = 1
                        ElseIf strBlock(i).ToString.IndexOf("Structure") = 0 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strGroup = Trim(String.Format("{0}", strBlock(i)))
                            Dim objWorksheetGroup As New WorksheetGroup
                            objWorksheetGroup.ProjectOwnerId = m_LoginUser.CompanyId
                            objWorksheetGroup.Name = strGroup
                            objWorksheetGroup.DisplayOrder = 0
                            intGroupID = m_ScopeService.CreateWorksheetGroup(objWorksheetGroup)
                            idxGroup = 1
                            idxArea = 0
                            idxStructure = 1
                        ElseIf strBlock(i).ToString.IndexOf("Element#|#") = 0 Then
                            strList.Add(String.Format("<br><br>"))
                            strList.Add(String.Format("{0}", strBlock(i)))
                            idxArea = 1
                        ElseIf strBlock(i).ToString.IndexOf("Comments:#|#") >= 0 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            idxArea = 0
                            strArea = String.Empty
                            strAreaMeasurement = String.Empty
                        ElseIf strBlock(i).ToString.IndexOf("Room / Element") = 0 Then
                            If strList.Count > 0 Then
                                Dim strTmp As String = String.Format("{0}", strList(strList.Count - 1))
                                strList.RemoveAt(strList.Count - 1)
                                strList.Add(String.Format("<br><br>==========================================================================================<br><br>"))
                                strList.Add(strTmp)
                                strGroup = Trim(strTmp)
                                Dim objWorksheetGroup As New WorksheetGroup
                                objWorksheetGroup.ProjectOwnerId = m_LoginUser.CompanyId
                                objWorksheetGroup.Name = strGroup
                                objWorksheetGroup.DisplayOrder = 0
                                intGroupID = m_ScopeService.CreateWorksheetGroup(objWorksheetGroup)
                                strList.Add(String.Format("<br><br>==========================================================================================<br><br>"))
                            End If
                            idxRoomElement = 1
                            idxStructure = 0
                            'strList.Add(String.Format("{0}", strBlock(i)))
                        ElseIf strBlock(i).ToString.IndexOf("Additional") > 0 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br><br>"))
                            idxRoomElement = 1
                        ElseIf strBlock(i).ToString.LastIndexOf("No of)") > 0 And strBlock(i).ToString.LastIndexOf("No of)") = strBlock(i).ToString.Length - 6 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxGroup = 1 Then
                                If idxArea = 1 Then
                                    Dim strItem As Array = Split(strBlock(i).ToString, "#|#")
                                    If UBound(strItem) > 0 Then
                                        m_ScopeItem.ScopeGroup = strGroup
                                        m_ScopeItem.ScopeGroupId = intGroupID
                                        m_ScopeItem.Area = strArea
                                        m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.Item = Trim(strItem(0))
                                        m_ScopeItem.Description = Trim(strItem(1).substring(0, strItem(1).indexof(" (")))
                                        m_ScopeItem.Quantity = Trim(strItem(1).substring(strItem(1).indexof(" (") + 2, strItem(1).ToString.LastIndexOf("No of)") - strItem(1).indexof(" (") - 2))
                                        m_ScopeItem.Unit = "no of"
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    End If
                                End If
                            End If
                            idxItem = 1
                        ElseIf strBlock(i).ToString.LastIndexOf("Item)") > 0 And strBlock(i).ToString.LastIndexOf("Item)") = strBlock(i).ToString.Length - 5 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxGroup = 1 Then
                                If idxArea = 1 Then
                                    Dim strItem As Array = Split(strBlock(i).ToString, "#|#")
                                    If UBound(strItem) > 0 Then
                                        m_ScopeItem.ScopeGroup = strGroup
                                        m_ScopeItem.ScopeGroupId = intGroupID
                                        m_ScopeItem.Area = strArea
                                        m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.Item = Trim(strItem(0))
                                        m_ScopeItem.Description = Trim(strItem(1).substring(0, strItem(1).indexof(" (")))
                                        m_ScopeItem.Quantity = Trim(strItem(1).substring(strItem(1).indexof(" (") + 2, strItem(1).ToString.LastIndexOf("Item)") - strItem(1).indexof(" (") - 2))
                                        m_ScopeItem.Unit = "item"
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    Else
                                        m_ScopeItem.ScopeGroup = strGroup
                                        m_ScopeItem.ScopeGroupId = intGroupID
                                        m_ScopeItem.Area = strArea
                                        m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.Description = Trim(strItem(0).substring(0, strItem(0).indexof(" (")))
                                        m_ScopeItem.Quantity = Trim(strItem(0).substring(strItem(0).indexof(" (") + 2, strItem(0).ToString.LastIndexOf("Item)") - strItem(0).indexof(" (") - 2))
                                        m_ScopeItem.Unit = "item"
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    End If
                                End If
                            End If
                            idxItem = 1
                        ElseIf strBlock(i).ToString.LastIndexOf("m2)") > 0 And strBlock(i).ToString.LastIndexOf("m2)") = strBlock(i).ToString.Length - 3 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxGroup = 1 Then
                                If idxArea = 1 Then
                                    Dim strItem As Array = Split(strBlock(i).ToString, "#|#")
                                    If UBound(strItem) > 0 Then
                                        m_ScopeItem.ScopeGroup = strGroup
                                        m_ScopeItem.ScopeGroupId = intGroupID
                                        m_ScopeItem.Area = strArea
                                        m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.Item = Trim(strItem(0))
                                        m_ScopeItem.Description = Trim(strItem(1).substring(0, strItem(1).indexof(" (")))
                                        m_ScopeItem.Quantity = Trim(strItem(1).substring(strItem(1).indexof(" (") + 2, strItem(1).ToString.LastIndexOf("m2)") - strItem(1).indexof(" (") - 2))
                                        m_ScopeItem.Unit = "m2"
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    End If
                                End If
                            End If
                            idxItem = 1
                        ElseIf strBlock(i).ToString.LastIndexOf("m)") > 0 And strBlock(i).ToString.LastIndexOf("m)") = strBlock(i).ToString.Length - 2 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxGroup = 1 Then
                                If idxRoomElement = 1 Then
                                    If strBlock(i).ToString.IndexOf(" (") > 0 Then
                                        strArea = Trim(strBlock(i).substring(0, strBlock(i).ToString.IndexOf(" (")))
                                        strAreaMeasurement = Trim(strBlock(i).substring(strBlock(i).ToString.IndexOf(" (") + 2, strBlock(i).ToString.LastIndexOf("m)") - strBlock(i).indexof(" (") - 1))
                                    Else
                                        strArea = Trim(strBlock(i).ToString)
                                        strAreaMeasurement = String.Empty
                                    End If
                                    idxArea = 1
                                    idxRoomElement = 0
                                Else
                                    If idxArea = 1 Then
                                        Dim strItem As Array = Split(strBlock(i).ToString, "#|#")
                                        If UBound(strItem) > 0 Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.ScopeItemStatusId = 2
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1).substring(0, strItem(1).indexof(" (")))
                                            m_ScopeItem.Quantity = Trim(strItem(1).substring(strItem(1).indexof(" (") + 2, strItem(1).ToString.LastIndexOf("m)") - strItem(1).indexof(" (") - 2))
                                            m_ScopeItem.Unit = "m"
                                            m_ScopeItem.ScopeId = m_ScopeID
                                            SaveScopeItem(m_ScopeItem)
                                            blnItemCreated = True
                                        End If
                                    End If
                                End If
                            End If
                            idxItem = 1
                        ElseIf strBlock(i).ToString.LastIndexOf("No of") > 0 And strBlock(i).ToString.LastIndexOf("No of") = strBlock(i).ToString.Length - 5 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxGroup = 1 Then
                                If idxArea = 1 Then
                                    Dim strItem As Array = Split(strBlock(i).ToString, "#|#")
                                    If UBound(strItem) = 3 Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1))
                                            m_ScopeItem.Quantity = Trim(strItem(2))
                                            m_ScopeItem.Unit = "no of"
                                        Else
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1))
                                            m_ScopeItem.Quantity = Trim(strItem(2))
                                            m_ScopeItem.Unit = "no of"
                                        End If
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    ElseIf UBound(strItem) > 1 Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Description = Trim(strItem(0))
                                            m_ScopeItem.Quantity = Trim(strItem(1))
                                            m_ScopeItem.Unit = "no of"
                                        Else
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Quantity = Trim(strItem(1))
                                            m_ScopeItem.Unit = "no of"
                                        End If
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    End If
                                End If
                            End If
                            idxItem = 1
                        ElseIf strBlock(i).ToString.LastIndexOf("Item") > 0 And strBlock(i).ToString.LastIndexOf("Item") = strBlock(i).ToString.Length - 4 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxGroup = 1 Then
                                If idxArea = 1 Then
                                    Dim strItem As Array = Split(strBlock(i).ToString, "#|#")
                                    If UBound(strItem) = 3 Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1))
                                            m_ScopeItem.Quantity = Trim(strItem(2))
                                            m_ScopeItem.Unit = "Item"
                                        Else
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1))
                                            m_ScopeItem.Quantity = Trim(strItem(2))
                                            m_ScopeItem.Unit = "Item"
                                        End If
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    ElseIf UBound(strItem) > 1 Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Description = Trim(strItem(0))
                                            m_ScopeItem.Quantity = Trim(strItem(1))
                                            m_ScopeItem.Unit = "Item"
                                        Else
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Quantity = Trim(strItem(1))
                                            m_ScopeItem.Unit = "Item"
                                        End If
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    End If
                                End If
                            End If
                            idxItem = 1
                        ElseIf strBlock(i).ToString.LastIndexOf("mm") > 0 And strBlock(i).ToString.LastIndexOf("mm") = strBlock(i).ToString.Length - 2 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            If idxGroup = 1 Then
                                If idxArea = 1 Then
                                    If Not m_ScopeItem.Item Is Nothing Then
                                        If Not m_ScopeItem.Description Is Nothing Then
                                            m_ScopeItem.Description = Trim(String.Format("{0} {1}", m_ScopeItem.Description, strBlock(i).ToString))
                                        Else
                                            m_ScopeItem.Description = Trim(strBlock(i).ToString)
                                        End If
                                    Else
                                        m_ScopeItem.Item = Trim(strBlock(i).ToString)
                                    End If
                                End If
                            End If
                        ElseIf strBlock(i).ToString.LastIndexOf("m2") > 0 And strBlock(i).ToString.LastIndexOf("m2") = strBlock(i).ToString.Length - 2 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxGroup = 1 Then
                                If idxArea = 1 Then
                                    Dim strItem As Array = Split(strBlock(i).ToString, "#|#")
                                    If UBound(strItem) = 3 Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1))
                                            m_ScopeItem.Quantity = Trim(strItem(2))
                                            m_ScopeItem.Unit = "m2"
                                        Else
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1))
                                            m_ScopeItem.Quantity = Trim(strItem(2))
                                            m_ScopeItem.Unit = "m2"
                                        End If
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    ElseIf UBound(strItem) > 1 Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Description = Trim(strItem(0))
                                            m_ScopeItem.Quantity = Trim(strItem(1))
                                            m_ScopeItem.Unit = "m2"
                                        Else
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Quantity = Trim(strItem(1))
                                            m_ScopeItem.Unit = "m2"
                                        End If
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    ElseIf UBound(strItem) > 0 Then
                                        m_ScopeItem.ScopeGroup = strGroup
                                        m_ScopeItem.ScopeGroupId = intGroupID
                                        m_ScopeItem.Area = strArea
                                        m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        If Trim(strItem(0)).LastIndexOf(" ") > 0 Then
                                            m_ScopeItem.Quantity = Trim(strItem(0)).Substring(Trim(strItem(0)).LastIndexOf(" ") + 1)
                                        Else
                                            m_ScopeItem.Quantity = Trim(strItem(0))
                                        End If
                                        m_ScopeItem.Unit = "m2"
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    End If
                                End If
                            End If
                            idxItem = 1
                        ElseIf strBlock(i).ToString.LastIndexOf("#|#m") > 0 And strBlock(i).ToString.LastIndexOf("#|#m") = strBlock(i).ToString.Length - 4 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxGroup = 1 Then
                                If idxArea = 1 Then
                                    Dim strItem As Array = Split(strBlock(i).ToString, "#|#")
                                    If UBound(strItem) = 3 Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1))
                                            m_ScopeItem.Quantity = Trim(strItem(2))
                                            m_ScopeItem.Unit = "m"
                                        Else
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Description = Trim(strItem(1))
                                            m_ScopeItem.Quantity = Trim(strItem(2))
                                            m_ScopeItem.Unit = "m"
                                        End If
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    ElseIf UBound(strItem) > 1 Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Description = Trim(strItem(0))
                                            m_ScopeItem.Quantity = Trim(strItem(1))
                                            m_ScopeItem.Unit = "m"
                                        Else
                                            m_ScopeItem.ScopeGroup = strGroup
                                            m_ScopeItem.ScopeGroupId = intGroupID
                                            m_ScopeItem.Area = strArea
                                            m_ScopeItem.AreaMeasurement = strAreaMeasurement
                                            m_ScopeItem.Item = Trim(strItem(0))
                                            m_ScopeItem.Quantity = Trim(strItem(1))
                                            m_ScopeItem.Unit = "m"
                                        End If
                                        m_ScopeItem.ScopeItemStatusId = 2
                                        m_ScopeItem.ScopeId = m_ScopeID
                                        SaveScopeItem(m_ScopeItem)
                                        blnItemCreated = True
                                    End If
                                End If
                            End If
                            idxItem = 1
                        ElseIf strBlock(i).ToString.LastIndexOf(")") > 0 And strBlock(i).ToString.LastIndexOf(")") = strBlock(i).ToString.Length - 1 Then
                            strList.Add(String.Format("{0}", strBlock(i)))
                            strList.Add(String.Format("<br>"))
                            If idxStructure = 1 Then
                                'If idxDescription = 1 Then
                                '    m_ScopeItem.Description = Trim(String.Format("{0} {1}", m_ScopeItem.Description, strBlock(i).ToString))
                                '    idxDescription = 0
                                'ElseIf idxSubTitle = 1 Then
                                '    m_ScopeItem.Item = Trim(String.Format("{0} {1}", m_ScopeItem.Item, strBlock(i).ToString))
                                '    idxSubTitle = 0
                                'ElseIf idxArea = 1 Then
                                '    strArea = Trim(strBlock(i).ToString)
                                '    idxItem = 1
                                'End If
                                If idxArea = 1 Then
                                    If strBlock(i).ToString.IndexOf(" (") > 0 Then
                                        strArea = Trim(strBlock(i).substring(0, strBlock(i).ToString.IndexOf(" (")))
                                        strAreaMeasurement = Trim(strBlock(i).substring(strBlock(i).ToString.IndexOf(" (") + 2, strBlock(i).ToString.LastIndexOf(")") - strBlock(i).indexof(" (") - 2))
                                    Else
                                        If Not m_ScopeItem.Description Is Nothing Then
                                            If m_ScopeItem.Description.LastIndexOf(")") < 0 And m_ScopeItem.Description.IndexOf("(") >= 0 Then
                                                m_ScopeItem.Description = Trim(String.Format("{0} {1}", m_ScopeItem.Description, strBlock(i).ToString))
                                            Else
                                                strArea = Trim(strBlock(i).ToString)
                                                strAreaMeasurement = String.Empty
                                                idxItem = 1
                                            End If
                                        Else
                                            strArea = Trim(strBlock(i).ToString)
                                            strAreaMeasurement = String.Empty
                                            idxItem = 1
                                        End If
                                    End If
                                End If

                                'If idxArea = 1 Then
                                '    If strBlock(i).ToString.IndexOf(" (") > 0 Then
                                '        strArea = Trim(strBlock(i).substring(0, strBlock(i).ToString.IndexOf(" (")))
                                '        strAreaMeasurement = Trim(strBlock(i).substring(strBlock(i).ToString.IndexOf(" (") + 2, strBlock(i).ToString.LastIndexOf(")") - strBlock(i).indexof(" (") - 2))
                                '    Else
                                '        strArea = Trim(strBlock(i).ToString)
                                '        strAreaMeasurement = String.Empty
                                '    End If
                                'End If
                                'idxItem = 1
                            Else
                                'If idxDescription = 1 Then
                                '    m_ScopeItem.Description = Trim(String.Format("{0} {1}", m_ScopeItem.Description, strBlock(i).ToString))
                                '    idxDescription = 0
                                'ElseIf idxSubTitle = 1 Then
                                '    m_ScopeItem.Item = Trim(String.Format("{0} {1}", m_ScopeItem.Item, strBlock(i).ToString))
                                '    idxSubTitle = 0
                                'Else
                                '    m_ScopeItem.Item = Trim(strBlock(i).ToString)
                                'End If
                                If Not m_ScopeItem.Item Is Nothing Then
                                    m_ScopeItem.Item = Trim(String.Format("{0} {1}", m_ScopeItem.Item, strBlock(i).ToString))
                                Else
                                    m_ScopeItem.Item = Trim(strBlock(i).ToString)
                                End If
                            End If
                        Else
                            strList.Add(String.Format("{0}", strBlock(i)))
                            If strBlock(i).ToString.IndexOf("Comments:#|#") >= 0 Then
                                idxArea = 0
                                strArea = String.Empty
                                strAreaMeasurement = String.Empty
                            End If

                            'If strBlock(i).ToString.IndexOf("Room - Comments:#|#") >= 0 Then
                            '    idxRoomElement = 1
                            'End If

                            If idxRoomElement = 1 Then
                                strArea = strBlock(i).ToString
                                idxArea = 1
                                idxRoomElement = 0
                            Else
                                If idxArea = 1 Then
                                    If strArea <> "" Then
                                        If Not m_ScopeItem.Item Is Nothing Then
                                            If Not m_ScopeItem.Description Is Nothing Then
                                                m_ScopeItem.Description = Trim(String.Format("{0} {1}", m_ScopeItem.Description, strBlock(i).ToString))
                                            Else
                                                m_ScopeItem.Description = Trim(strBlock(i).ToString)
                                            End If
                                            idxDescription = 1
                                        Else
                                            m_ScopeItem.Item = Trim(strBlock(i).ToString)
                                            idxSubTitle = 1
                                        End If
                                    Else
                                        strArea = Trim(strBlock(i).ToString)
                                        strAreaMeasurement = String.Empty
                                        idxItem = 1
                                    End If
                                End If
                            End If

                            idxAddress = 0
                            idxContact = 0
                        End If

                        If idxRoomElement = 1 Then
                            'strList.Add(String.Format("Room / Element = "))
                        ElseIf idxItem = 1 Then
                            strList.Add(String.Format("Item = "))
                            idxItem = 0
                            If blnItemCreated Then
                                m_ScopeItem = New ScopeItem
                                blnItemCreated = False
                            End If
                        End If
                    End If
                Next
            End If
        End If

        For j = 0 To strList.Count - 1
            If j = 0 Then
                strSection = strList(j).ToString
            Else
                strSection = String.Format("{0}<br>{1}", strSection, strList(j).ToString)
            End If
        Next
        Return strSection
    End Function

    Private Function SaveProject(ByVal NewProject As Project) As Integer
        Return m_ManagementService.CreateProject(NewProject)
    End Function
    Private Function SaveUser(ByVal ContactUser As User) As Integer
        Return m_ManagementService.CreateUser(ContactUser, m_LoginUser.UserId)
    End Function

    Private Function SaveProfile(ByVal ContactProfile As UserProfile) As Integer
        Return m_ManagementService.CreateUserProfile(ContactProfile)
    End Function

    Private Function SaveScope(ByVal NewScope As Scope) As Integer
        Return m_ScopeService.CreateScope(NewScope)
    End Function

    Private Function SaveScopeItem(ByVal NewScopeItem As ScopeItem) As Integer
        Return m_ScopeService.CreateScopeItem(NewScopeItem)
    End Function

    Private Function GetProjectCreditBalance() As Integer
        Dim result As Integer = 0
        Dim dsUserAccount As DataSet = New DataSet()
        dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
        If dsUserAccount.Tables.Count > 0 Then
            If dsUserAccount.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("ProjectCredit")) Then
                    result = dsUserAccount.Tables(0).Rows(0)("ProjectCredit")
                End If
            End If
        End If
        Return result
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Txt_FileUpload.FileName <> "" And m_OwnerProfile.Identifier <> "" Then
            Dim fileName As String
            Dim FileType As String
            Dim namePosition As Int16

            namePosition = Txt_FileUpload.PostedFile.FileName.LastIndexOf("\") + 1
            fileName = Txt_FileUpload.PostedFile.FileName.Substring(namePosition)
            FileType = Txt_FileUpload.PostedFile.ContentType

            If FileType = "application/pdf" Then
                If (Not System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_OwnerProfile.Identifier))) Then
                    System.IO.Directory.CreateDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_OwnerProfile.Identifier))
                End If

                Txt_FileUpload.PostedFile.SaveAs(String.Format("{0}\images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), m_OwnerProfile.Identifier, fileName))

                Label1.Text = pdf2txt(String.Format("{0}\images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), m_OwnerProfile.Identifier, fileName))

                If m_ProjectId <> 0 And m_ScopeID <> 0 Then
                    If File.Exists(String.Format("{0}\images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), m_OwnerProfile.Identifier, fileName)) Then
                        File.Delete(String.Format("{0}\images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), m_OwnerProfile.Identifier, fileName))
                    End If

                    If Not ProcessDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_OwnerProfile.Identifier)) Then
                        System.IO.Directory.Delete(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_OwnerProfile.Identifier), True)
                    End If

                    Response.Redirect(String.Format("Detail.aspx?id={0}&sid={1}", m_Cryption.Encrypt(m_ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(m_ScopeID, m_Cryption.cryptionKey)))
                Else
                    Dim tosupport_address As String = System.Configuration.ConfigurationManager.AppSettings("SupportEmail")
                    Dim toadmin_address As String = System.Configuration.ConfigurationManager.AppSettings("AdminEmail")
                    Dim MailMessage As New System.Net.Mail.MailMessage()
                    Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
                    MailMessage.To.Add(New System.Net.Mail.MailAddress(tosupport_address))
                    'MailMessage.CC.Add(New System.Net.Mail.MailAddress(toadmin_address))
                    MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("ErrorEmailFromServer"))
                    If m_EmailType = 1 Then
                        MailMessage.Subject = String.Format("EQC FTP Import in different vision - {0}", Txt_FileUpload.FileName)
                        MailMessage.Body = String.Format("EQC FTP Import in different vision - {0}", Txt_FileUpload.FileName)
                    Else
                        MailMessage.Subject = String.Format("EQC FTP Import Error in {0}", Txt_FileUpload.FileName)
                        MailMessage.Body = String.Format("EQC FTP Import Error in {0}", Txt_FileUpload.FileName)
                    End If
                    MailMessage.IsBodyHtml = False
                    Try
                        emailClient.Send(MailMessage)
                        'm_Log.Log(LogCategory.ApplicationError, ipAddress, String.Format("Error Emailed"), pageUrl, userAgent)
                    Catch ex As Exception
                        Throw
                    End Try
                    Label1.Text = "This is not a valid PDF file, please contact the administrator."
                End If
            Else
                Label1.Text = "The selected file is not in PDF format."
            End If
        Else
            Label1.Text = "Please select a PDF file."
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Public Function ProcessDirectory(ByVal targetDirectory As String) As Boolean
        Dim fileEntries As String() = Directory.GetFiles(targetDirectory)
        Dim hasFiles As Boolean = False
        Dim fileName As String
        For Each fileName In fileEntries
            hasFiles = True
            Exit For
        Next fileName
        'Dim subdirectoryEntries As String() = Directory.GetDirectories(targetDirectory)
        ' Recurse into subdirectories of this directory. 
        'Dim subdirectory As String
        'For Each subdirectory In subdirectoryEntries
        'ProcessDirectory(subdirectory)
        'Next subdirectory
        Return hasFiles
    End Function
End Class
