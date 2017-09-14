<%@ WebHandler Language="VB" Class="apidownloadworksheet" %>

Imports System
Imports System.Data
Imports System.Web
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Public Class apidownloadworksheet : Implements IHttpHandler
    'Use Spire component to create spreadsheet dynamically
    Private WithEvents cellExport As Spire.DataExport.XLS.CellExport
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.Clear()
        'If context.Request.UrlReferrer.ToString().ToLower().Contains("/projects/detail.aspx") Then
            If Not context.Request.QueryString("id") Is Nothing Then
                Dim objCryption As Cryption = New Cryption()
            Dim lngScopeId As Long = context.Request.QueryString("id")
            
            'If Long.TryParse(objCryption.Decrypt(context.Request.QueryString("id"), objCryption.cryptionKey), lngScopeId) Then
                    If lngScopeId > 0 Then
                        GenerateSpreadsheetforGroupedWorksheet(lngScopeId, context)
                        'Dim strFileName As String = GenerateSpreadsheetforGroupedWorksheet(lngScopeId)
                        'If strFileName <> String.Empty Then
                        '    Dim strExtension As String
                        '    strExtension = Split(strFileName, ".")(1).ToLower()
                        '    Dim strContentType As String = "application/ms-excel"
                
                        '    Dim cVoucherCode As New VoucherCodeFunctions
                        '    Dim sToken As String = cVoucherCode.GenerateVoucherCodeGuid(16)
            
                        '    context.Response.ContentType = strContentType
                        '    context.Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}{1}.{2}", Now.ToString("yyyyMMddHHmmss"), sToken, strExtension))
                        '    context.Response.TransmitFile(strFileName)
              
                        '    context.Response.End()
                        'End If
                    End If
                'End If
            End If
       
    End Sub
 
    Private Function GenerateSpreadsheetforGroupedWorksheet(ByVal ScopeId As Long, ByVal context As HttpContext) As String
        Dim result As String = String.Empty
        Dim objScopeService As ScopeServices = New ScopeServices()
        objScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim objManagementService As ManagementService = New ManagementService()
        objManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim objScope As Scope = objScopeService.GetScopeByScopeId(ScopeId)
        Dim objProject As Project = objManagementService.GetProjectByProjectId(objScope.ProjectId)
        
        Dim dsQuery As DataSet
        Dim intFootTotalStartRow As Integer
        Dim intFootTotalStartColum As Integer
        Dim intCompanyInfoStartRow As Integer
        Dim intCompanyInfoStartColum As Integer
        Dim strExcelFile As String
        Dim strNewExcelFileName As String
        Dim dsProjectGroup As DataSet = New DataSet()

        Dim strClaimNo As String = ""
        Dim strClaimant As String = ""
        Dim strSiteLocation As String = ""
        Dim strEQRSupervisor As String = ""
        Dim strContractorEmail As String = ""
        Dim strScopeDate As String = ""
        Dim strContractor As String = ""
        Dim strAddress As String = ""
        Dim strAccreditationNo As String = ""
        Dim strContractorPhone As String = ""
        Dim strGSTNo As String = ""
        Dim strTotalPrice As String = ""
        Dim dsProject As DataSet
        Dim ProjectOwner As ProjectOwner
        Dim strLogo As String = ""
                
        dsProjectGroup = objScopeService.GetProjectGroupsByProjectOwnerId(objProject.ProjectOwnerId)
        dsProject = objManagementService.GetProjectInfoByProjectId(objScope.ProjectId)

        'get project group email
        If dsProjectGroup.Tables.Count > 0 Then
            If dsProjectGroup.Tables(0).Rows.Count > 0 Then
                If Not Convert.IsDBNull(dsProjectGroup.Tables(0).Rows(0).Item("Email")) Then
                    strContractorEmail = dsProjectGroup.Tables(0).Rows(0).Item("Email")
                End If
            End If
        End If

        'get claimant detail
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")) Then
            strClaimNo = dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")
        End If
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
            strScopeDate = dsProject.Tables(0).Rows(0).Item("ScopeDate").ToString()
        End If
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
            strScopeDate = CType(dsProject.Tables(0).Rows(0).Item("ScopeDate"), DateTime).ToString("dd/MM/yyyy")
        End If

        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Address")) Then
            strSiteLocation = dsProject.Tables(0).Rows(0).Item("Address")
        End If

        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ContactName")) Then
            strClaimant = dsProject.Tables(0).Rows(0).Item("ContactName")
        End If

        'get project owner detail
        ProjectOwner = objManagementService.GetProjectOwnerByProjectOwnerId(objProject.ProjectOwnerId)
        strEQRSupervisor = ProjectOwner.EQRSupervisor
        strContractor = ProjectOwner.Name
        strContractorPhone = ProjectOwner.Contact1
        strAddress = ProjectOwner.Address
        strGSTNo = ProjectOwner.GSTNumber
        strAccreditationNo = ProjectOwner.AccreditationNumber
        
        Dim ApprovedexcludeGstCost As String = objScope.Cost.ToString("c")
        Dim ApprovedGSTCost As String = (objScope.Total - objScope.Cost).ToString("c")
        Dim ApprovedInGSTCost As String = objScope.Total.ToString("c")

        Dim GrandExGSTCost As String = (objScope.Cost1 + objScope.Cost).ToString("c")
        Dim GrandInGSTCost As String = (objScope.Total1 + objScope.Total).ToString("c")

        strTotalPrice = ApprovedInGSTCost

        'Company Logo
        strLogo = String.Format("{0}\Images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), ProjectOwner.Identifier, ProjectOwner.Logo)

        strNewExcelFileName = String.Empty
        Dim strExcelFileNameTemp As String = StrConv(strSiteLocation, vbProperCase)
        Dim ascChar As Integer
        For index As Integer = 0 To strExcelFileNameTemp.Length - 1
            ascChar = Asc(strExcelFileNameTemp(index))
            If (ascChar >= 65 And ascChar <= 90) Or (ascChar >= 97 And ascChar <= 122) Or (ascChar >= 48 And ascChar <= 57) Then
                strNewExcelFileName = strNewExcelFileName + strExcelFileNameTemp(index)
            Else
                strNewExcelFileName = strNewExcelFileName + "-"
            End If
        Next
        Do While strNewExcelFileName.Contains("--")
            strNewExcelFileName = Replace(strNewExcelFileName, "--", "-")
        Loop
        If strNewExcelFileName.Substring(0, 1) = "-" Then
            strNewExcelFileName = strNewExcelFileName.Substring(1)
        End If
        If strNewExcelFileName.Substring(strNewExcelFileName.Length - 1) = "-" Then
            strNewExcelFileName = strNewExcelFileName.Substring(0, strNewExcelFileName.Length - 1)
        End If
        If strNewExcelFileName.Length > 20 Then
            strNewExcelFileName = strNewExcelFileName.Substring(0, 20)
        End If
        strExcelFile = String.Format("{0}\Downloads\{1}\{2}.xls", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport", strNewExcelFileName)

        If (Not System.IO.Directory.Exists(String.Format("{0}\Downloads\{1}", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport"))) Then
            System.IO.Directory.CreateDirectory(String.Format("{0}\Downloads\{1}", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport"))
        End If

        Dim workSheet As New Spire.DataExport.XLS.WorkSheet()

        'initialzing the cellexport tool
        cellExport = New Spire.DataExport.XLS.CellExport
        cellExport.ActionAfterExport = Spire.DataExport.Common.ActionType.OpenView

        'culture format setting
        cellExport.DataFormats.CultureName = "en-NZ"
        cellExport.DataFormats.Currency = "#,###,##0.00"
        cellExport.DataFormats.DateTime = "dd/MM/yyyy H:mm"
        cellExport.DataFormats.Float = "#,###,##0.00"
        cellExport.DataFormats.[Integer] = "#,###,##0"
        cellExport.DataFormats.Time = "H:mm"

        'set up file name and location
        cellExport.FileName = strExcelFile

        'outlook formating
        cellExport.SheetOptions.AggregateFormat.Font.Name = "Arial"
        cellExport.SheetOptions.CustomDataFormat.Font.Name = "Arial"
        cellExport.SheetOptions.DefaultFont.Name = "Arial"
        cellExport.SheetOptions.FooterFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HeaderFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue
        cellExport.SheetOptions.HyperlinkFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.[Single]
        cellExport.SheetOptions.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left
        cellExport.SheetOptions.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top
        cellExport.SheetOptions.NoteFormat.Font.Bold = True
        cellExport.SheetOptions.NoteFormat.Font.Name = "Tahoma"
        cellExport.SheetOptions.NoteFormat.Font.Size = 8.0F
        cellExport.SheetOptions.TitlesFormat.FillStyle.Background = Spire.DataExport.XLS.CellColor.Gray40Percent
        cellExport.SheetOptions.TitlesFormat.Font.Bold = True
        cellExport.SheetOptions.TitlesFormat.Font.Name = "Arial"



        'worksheet culture format
        workSheet.FormatsExport.CultureName = "en-NZ"
        workSheet.FormatsExport.Currency = "#,###,##0.00"
        workSheet.FormatsExport.DateTime = "dd/MM/yyyy H:mm"
        workSheet.FormatsExport.Float = "#,###,##0.00"
        workSheet.FormatsExport.[Integer] = "#,###,##0"
        workSheet.FormatsExport.Time = "H:mm"

        'worksheet outlook format
        workSheet.Options.AggregateFormat.Font.Name = "Arial"
        workSheet.Options.CustomDataFormat.Font.Name = "Arial"
        workSheet.Options.DefaultFont.Name = "Arial"
        workSheet.Options.FooterFormat.Font.Name = "Arial"
        workSheet.Options.HeaderFormat.Font.Name = "Arial"
        workSheet.Options.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.HyperlinkFormat.Font.Name = "Arial"
        workSheet.Options.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.[Single]
        workSheet.Options.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left
        workSheet.Options.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top
        workSheet.Options.NoteFormat.Font.Bold = True
        workSheet.Options.NoteFormat.Font.Name = "Tahoma"
        workSheet.Options.NoteFormat.Font.Size = 8.0F
        'workSheet.Options.CustomDataFormat.Font.Size = 40.0F

        workSheet.Options.TitlesFormat.Font.Bold = True
        workSheet.Options.TitlesFormat.Font.Name = "Arial"

        workSheet.Options.CustomDataFormat.Borders.Bottom.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Left.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Right.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Top.Color = Spire.DataExport.XLS.CellColor.Blue

        workSheet.SheetName = "Worksheet"
        workSheet.AutoFitColWidth = True

        Dim stripStyle1 As Spire.DataExport.XLS.StripStyle = New Spire.DataExport.XLS.StripStyle()

        Dim mcell As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        'mcell.Column = 2
        mcell.Column = 1
        mcell.Row = 2
        If dsProjectGroup.Tables.Count > 0 Then
            If dsProjectGroup.Tables(0).Rows.Count > 0 Then
                mcell.Value = String.Format("{0} - Contractor's Quote", dsProjectGroup.Tables(0).Rows(0)("Name"))
            Else
                mcell.Value = String.Format("Contractor's Quote")
            End If
        Else
            mcell.Value = String.Format("Contractor's Quote")
        End If
        mcell.CellType = Spire.DataExport.XLS.CellType.String
        'mcell.Format = workSheet.Options.TitlesFormat
        'mcell.Format.Font.Bold = 1
        'mcell.Format.Font.Size = 34

        workSheet.HeaderRows = 15
        workSheet.Cells.Add(mcell)


        intCompanyInfoStartRow = 8
        intCompanyInfoStartColum = 1
        'workSheet.Cells.Add(acell)


        'add image
        Dim pic1 As Spire.DataExport.XLS.CellPicture = New Spire.DataExport.XLS.CellPicture()
        pic1.FileName = strLogo 'need to find out where the logo is
        pic1.Name = "Logo"

        cellExport.Pictures.Add(pic1)

        Dim img1 As Spire.DataExport.XLS.CellImage = New Spire.DataExport.XLS.CellImage
        img1.Column = 3
        img1.PictureName = "Logo"
        img1.Row = 1
        workSheet.Images.Add(img1)



        'add contact detail head field
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1.Column = intCompanyInfoStartColum
        mcell1.Row = intCompanyInfoStartRow
        mcell1.Value = "Claim No:"
        mcell1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1)


        Dim mcell2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2.Column = intCompanyInfoStartColum
        mcell2.Row = intCompanyInfoStartRow + 1
        mcell2.Value = "Claimant:"
        mcell2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2)


        Dim mcell3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3.Column = intCompanyInfoStartColum
        mcell3.Row = intCompanyInfoStartRow + 2
        mcell3.Value = "Site Location:"
        mcell3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3)


        Dim mcell4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4.Column = intCompanyInfoStartColum
        mcell4.Row = intCompanyInfoStartRow + 3
        mcell4.Value = "EQR Supervisor:"
        mcell4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4)


        Dim mcell5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5.Column = intCompanyInfoStartColum
        mcell5.Row = intCompanyInfoStartRow + 4
        mcell5.Value = "Contractor E-mail:"
        mcell5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5)


        Dim mcell6 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6.Column = intCompanyInfoStartColum
        mcell6.Row = intCompanyInfoStartRow + 5
        mcell6.Value = "Date:"
        mcell6.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6)

        'add contact detail head field contet
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_2.Column = intCompanyInfoStartColum + 1
        mcell1_2.Row = intCompanyInfoStartRow
        mcell1_2.Value = strClaimNo
        mcell1_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_2)


        Dim mcell2_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_2.Column = intCompanyInfoStartColum + 1
        mcell2_2.Row = intCompanyInfoStartRow + 1
        mcell2_2.Value = strClaimant
        mcell2_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_2)


        Dim mcell3_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_2.Column = intCompanyInfoStartColum + 1
        mcell3_2.Row = intCompanyInfoStartRow + 2
        mcell3_2.Value = strSiteLocation
        mcell3_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_2)


        Dim mcell4_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_2.Column = intCompanyInfoStartColum + 1
        mcell4_2.Row = intCompanyInfoStartRow + 3
        mcell4_2.Value = strEQRSupervisor
        mcell4_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_2)


        Dim mcell5_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_2.Column = intCompanyInfoStartColum + 1
        mcell5_2.Row = intCompanyInfoStartRow + 4
        mcell5_2.Value = strContractorEmail
        mcell5_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_2)


        Dim mcell6_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_2.Column = intCompanyInfoStartColum + 1
        mcell6_2.Row = intCompanyInfoStartRow + 5
        'mcell6_2.Value = IIf(strScopeDate.Trim() = String.Empty, Today.ToString("dd/MM/yyyy"), strScopeDate)
        mcell6_2.Value = Today.ToString("dd/MM/yyyy")
        mcell6_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_2)

        'second part of heading
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_3.Column = intCompanyInfoStartColum + 2
        mcell1_3.Row = intCompanyInfoStartRow
        mcell1_3.Value = "Contractor:"
        mcell1_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_3)


        Dim mcell2_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_3.Column = intCompanyInfoStartColum + 2
        mcell2_3.Row = intCompanyInfoStartRow + 1
        mcell2_3.Value = "Address:"
        mcell2_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_3)


        Dim mcell3_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_3.Column = intCompanyInfoStartColum + 2
        mcell3_3.Row = intCompanyInfoStartRow + 2
        mcell3_3.Value = "Accreditation No:"
        mcell3_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_3)


        Dim mcell4_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_3.Column = intCompanyInfoStartColum + 2
        mcell4_3.Row = intCompanyInfoStartRow + 3
        mcell4_3.Value = "Contractor Phone:"
        mcell4_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_3)


        Dim mcell5_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_3.Column = intCompanyInfoStartColum + 2
        mcell5_3.Row = intCompanyInfoStartRow + 4
        mcell5_3.Value = "GST No:"
        mcell5_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_3)


        Dim mcell6_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_3.Column = intCompanyInfoStartColum + 2
        mcell6_3.Row = intCompanyInfoStartRow + 5
        mcell6_3.Value = "Total Price (incl. GST):"
        mcell6_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_3)

        'second part of heading
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_4.Column = intCompanyInfoStartColum + 3
        mcell1_4.Row = intCompanyInfoStartRow
        mcell1_4.Value = strContractor
        mcell1_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_4)


        Dim mcell2_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_4.Column = intCompanyInfoStartColum + 3
        mcell2_4.Row = intCompanyInfoStartRow + 1
        mcell2_4.Value = strAddress
        mcell2_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_4)


        Dim mcell3_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_4.Column = intCompanyInfoStartColum + 3
        mcell3_4.Row = intCompanyInfoStartRow + 2
        mcell3_4.Value = strAccreditationNo
        mcell3_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_4)


        Dim mcell4_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_4.Column = intCompanyInfoStartColum + 3
        mcell4_4.Row = intCompanyInfoStartRow + 3
        mcell4_4.Value = strContractorPhone
        mcell4_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_4)


        Dim mcell5_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_4.Column = intCompanyInfoStartColum + 3
        mcell5_4.Row = intCompanyInfoStartRow + 4
        mcell5_4.Value = strGSTNo
        mcell5_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_4)


        Dim mcell6_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_4.Column = intCompanyInfoStartColum + 3
        mcell6_4.Row = intCompanyInfoStartRow + 5
        mcell6_4.Value = strTotalPrice
        mcell6_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_4)

        'get data rows
        'build query       
        dsQuery = objScopeService.GetScopeItemsByScopeIdScopeItemStatus(ScopeId, 2)
        dsQuery.Tables(0).DefaultView.Sort = "ScopeGroup"
        Dim rowView As DataRowView
        Dim row As DataRow
        Dim intRowIndex As Integer
        Dim intColumIndex As Integer
        intRowIndex = 15
        intColumIndex = 0
        
        Dim strPrevScopeGroup As String = "****"
        Dim strScopeGroup As String = String.Empty
        Dim strPrevArea As String = "*****"
        Dim strArea As String = String.Empty
        Dim strAreaMeasurement As String = String.Empty
        Dim blnAreaMeasurementFilled As Boolean = False
        
        Dim dynamiccell_1 As Spire.DataExport.XLS.Cell

        intRowIndex = intRowIndex + 1
        Dim Range1 As Integer = intRowIndex
        
        For Each rowView In dsQuery.Tables(0).DefaultView
            row = rowView.Row
            strScopeGroup = String.Format("{0}", row("ScopeGroup"))
            If strScopeGroup <> strPrevScopeGroup Then
                intRowIndex = intRowIndex + 1

                'Area
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 1
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = String.Format("{0}", row("ScopeGroup"))
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Note
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 2
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Description of Works"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'QTY
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 3
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Dimension"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Rate
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 4
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "$ Rate"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Cost
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 5
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Contractors Quote"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)
                
                intRowIndex = intRowIndex + 1
            End If
            strPrevScopeGroup = strScopeGroup
            
            strArea = String.Format("{0}", row("Area"))
            strAreaMeasurement = String.Format("{0}", row("AreaMeasurement"))
            'Area
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 1
            dynamiccell_1.Row = intRowIndex
            If strArea <> strPrevArea Then
                dynamiccell_1.Value = strArea
                blnAreaMeasurementFilled = False
            Else
                If blnAreaMeasurementFilled Then
                    dynamiccell_1.Value = "-"
                Else
                    If strAreaMeasurement.Trim() = String.Empty Then
                        dynamiccell_1.Value = "-"
                    Else
                        dynamiccell_1.Value = strAreaMeasurement
                    End If
                    blnAreaMeasurementFilled = True
                End If
            End If
            strPrevArea = strArea
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'Note
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 2
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Description")) Then
                dynamiccell_1.Value = row("Description")
            Else
                dynamiccell_1.Value = ""
            End If

            If Not IsDBNull(row("Item")) Then
                If String.Format("{0}", row("Item")).Trim() <> String.Empty Then
                    dynamiccell_1.Value = String.Format("{0}: {1}", row("Item"), dynamiccell_1.Value)
                End If
            End If
            
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'QTY
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 3
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Quantity")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Quantity")).ToString()
            Else
                dynamiccell_1.Value = 0
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            'Rate
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 4
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Rate")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Rate")).ToString()
            Else
                dynamiccell_1.Value = 0
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            'Cost
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 5
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Cost")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Cost")).ToString("c")
            Else
                dynamiccell_1.Value = Convert.ToDouble(0).ToString("c")
            End If
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            intRowIndex = intRowIndex + 1
        Next
        Dim Range2 As Integer = intRowIndex - 1

        intFootTotalStartRow = intRowIndex
        intFootTotalStartColum = 4

        'heading
        Dim mcell1_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_5.Column = intFootTotalStartColum
        mcell1_5.Row = intFootTotalStartRow + 2
        mcell1_5.Value = "Subtotal (excl. GST)"
        mcell1_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_5)

        Dim mcell2_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_5.Column = intFootTotalStartColum
        mcell2_5.Row = intFootTotalStartRow + 3
        mcell2_5.Value = "Add 15% GST"
        mcell2_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_5)

        Dim mcell3_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_5.Column = intFootTotalStartColum
        mcell3_5.Row = intFootTotalStartRow + 4
        mcell3_5.Value = "Total Incl. GST"
        mcell3_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_5)

        '' ''Dim mcell4_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        '' ''mcell4_5.Column = intFootTotalStartColum
        '' ''mcell4_5.Row = intFootTotalStartRow + 6
        '' ''mcell4_5.Value = "Grand Total with Scope Changes Excl. GST"
        '' ''mcell4_5.CellType = Spire.DataExport.XLS.CellType.String
        '' ''workSheet.Cells.Add(mcell4_5)

        '' ''Dim mcell5_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        '' ''mcell5_5.Column = intFootTotalStartColum
        '' ''mcell5_5.Row = intFootTotalStartRow + 7
        '' ''mcell5_5.Value = "Grand Total with Scope Changes Incl. GST"
        '' ''mcell5_5.CellType = Spire.DataExport.XLS.CellType.String
        '' ''workSheet.Cells.Add(mcell5_5)

        'value 1
        'Dim CellFomat1_6 As New Spire.DataExport.XLS.CellFormat
        'CellFomat1_6.Font.Bold = True

        'value 2
        Dim mcell1_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_7.Column = intFootTotalStartColum + 1
        mcell1_7.Row = intFootTotalStartRow + 2
        mcell1_7.Value = ApprovedexcludeGstCost
        mcell1_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_7)

        Dim mcell2_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_7.Column = intFootTotalStartColum + 1
        mcell2_7.Row = intFootTotalStartRow + 3
        mcell2_7.Value = ApprovedGSTCost
        mcell2_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_7)

        Dim mcell3_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_7.Column = intFootTotalStartColum + 1
        mcell3_7.Row = intFootTotalStartRow + 4
        mcell3_7.Value = ApprovedInGSTCost
        mcell3_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_7)


        '''''''''''''''''
        'oleDbConnection.Open()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Dim InitialDataTable As New DataTable
        'connect to dataset
        workSheet.DataSource = Spire.DataExport.Common.ExportSource.DataTable
        'workSheet.DataTable = dsQuery.Tables(0)
        workSheet.DataTable = InitialDataTable
        'workSheet.SQLCommand = oleDbCommand
        workSheet.StartDataCol = CByte(1)
        cellExport.Sheets.Add(workSheet)
        Try
            'cellExport.SaveToFile()
            cellExport.SaveToHttpResponse(String.Format("{0}.xls", strNewExcelFileName), context.Response)
            result = strExcelFile
        Finally
            'oleDbConnection.Close()
        End Try
        
        Return result
    End Function
    
    Private Function GenerateSpreadsheetforGroupedWorksheet_ItemInDescription(ByVal ScopeId As Long, ByVal context As HttpContext) As String
        Dim result As String = String.Empty
        Dim objScopeService As ScopeServices = New ScopeServices()
        objScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim objManagementService As ManagementService = New ManagementService()
        objManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim objScope As Scope = objScopeService.GetScopeByScopeId(ScopeId)
        Dim objProject As Project = objManagementService.GetProjectByProjectId(objScope.ProjectId)
        
        Dim dsQuery As DataSet
        Dim intFootTotalStartRow As Integer
        Dim intFootTotalStartColum As Integer
        Dim intCompanyInfoStartRow As Integer
        Dim intCompanyInfoStartColum As Integer
        Dim strExcelFile As String
        Dim strNewExcelFileName As String
        Dim dsProjectGroup As DataSet = New DataSet()

        Dim strClaimNo As String = ""
        Dim strClaimant As String = ""
        Dim strSiteLocation As String = ""
        Dim strEQRSupervisor As String = ""
        Dim strContractorEmail As String = ""
        Dim strScopeDate As String = ""
        Dim strContractor As String = ""
        Dim strAddress As String = ""
        Dim strAccreditationNo As String = ""
        Dim strContractorPhone As String = ""
        Dim strGSTNo As String = ""
        Dim strTotalPrice As String = ""
        Dim dsProject As DataSet
        Dim ProjectOwner As ProjectOwner
        Dim strLogo As String = ""
                
        dsProjectGroup = objScopeService.GetProjectGroupsByProjectOwnerId(objProject.ProjectOwnerId)
        dsProject = objManagementService.GetProjectInfoByProjectId(objScope.ProjectId)

        'get project group email
        If dsProjectGroup.Tables.Count > 0 Then
            If dsProjectGroup.Tables(0).Rows.Count > 0 Then
                If Not Convert.IsDBNull(dsProjectGroup.Tables(0).Rows(0).Item("Email")) Then
                    strContractorEmail = dsProjectGroup.Tables(0).Rows(0).Item("Email")
                End If
            End If
        End If
        
        'get claimant detail
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")) Then
            strClaimNo = dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")
        End If
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
            strScopeDate = dsProject.Tables(0).Rows(0).Item("ScopeDate").ToString()
        End If
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
            strScopeDate = CType(dsProject.Tables(0).Rows(0).Item("ScopeDate"), DateTime).ToString("dd/MM/yyyy")
        End If

        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Address")) Then
            strSiteLocation = dsProject.Tables(0).Rows(0).Item("Address")
        End If

        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ContactName")) Then
            strClaimant = dsProject.Tables(0).Rows(0).Item("ContactName")
        End If

        'get project owner detail
        ProjectOwner = objManagementService.GetProjectOwnerByProjectOwnerId(objProject.ProjectOwnerId)
        strEQRSupervisor = ProjectOwner.EQRSupervisor
        strContractor = ProjectOwner.Name
        strContractorPhone = ProjectOwner.Contact1
        strAddress = ProjectOwner.Address
        strGSTNo = ProjectOwner.GSTNumber
        strAccreditationNo = ProjectOwner.AccreditationNumber
        
        Dim ApprovedexcludeGstCost As String = objScope.Cost.ToString("c")
        Dim ApprovedGSTCost As String = (objScope.Total - objScope.Cost).ToString("c")
        Dim ApprovedInGSTCost As String = objScope.Total.ToString("c")

        Dim GrandExGSTCost As String = (objScope.Cost1 + objScope.Cost).ToString("c")
        Dim GrandInGSTCost As String = (objScope.Total1 + objScope.Total).ToString("c")

        strTotalPrice = ApprovedInGSTCost

        'Company Logo
        strLogo = String.Format("{0}\Images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), ProjectOwner.Identifier, ProjectOwner.Logo)

        strNewExcelFileName = String.Format("ExcelExport_{0}_{1}", ProjectOwner.ContactId, ScopeId)
        strExcelFile = String.Format("{0}\Downloads\{1}\{2}.xls", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport", strNewExcelFileName)

        If (Not System.IO.Directory.Exists(String.Format("{0}\Downloads\{1}", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport"))) Then
            System.IO.Directory.CreateDirectory(String.Format("{0}\Downloads\{1}", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport"))
        End If

        Dim workSheet As New Spire.DataExport.XLS.WorkSheet()

        'initialzing the cellexport tool
        cellExport = New Spire.DataExport.XLS.CellExport
        cellExport.ActionAfterExport = Spire.DataExport.Common.ActionType.OpenView

        'culture format setting
        cellExport.DataFormats.CultureName = "en-NZ"
        cellExport.DataFormats.Currency = "#,###,##0.00"
        cellExport.DataFormats.DateTime = "dd/MM/yyyy H:mm"
        cellExport.DataFormats.Float = "#,###,##0.00"
        cellExport.DataFormats.[Integer] = "#,###,##0"
        cellExport.DataFormats.Time = "H:mm"

        'set up file name and location
        cellExport.FileName = strExcelFile

        'outlook formating
        cellExport.SheetOptions.AggregateFormat.Font.Name = "Arial"
        cellExport.SheetOptions.CustomDataFormat.Font.Name = "Arial"
        cellExport.SheetOptions.DefaultFont.Name = "Arial"
        cellExport.SheetOptions.FooterFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HeaderFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue
        cellExport.SheetOptions.HyperlinkFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.[Single]
        cellExport.SheetOptions.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left
        cellExport.SheetOptions.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top
        cellExport.SheetOptions.NoteFormat.Font.Bold = True
        cellExport.SheetOptions.NoteFormat.Font.Name = "Tahoma"
        cellExport.SheetOptions.NoteFormat.Font.Size = 8.0F
        cellExport.SheetOptions.TitlesFormat.FillStyle.Background = Spire.DataExport.XLS.CellColor.Gray40Percent
        cellExport.SheetOptions.TitlesFormat.Font.Bold = True
        cellExport.SheetOptions.TitlesFormat.Font.Name = "Arial"



        'worksheet culture format
        workSheet.FormatsExport.CultureName = "en-NZ"
        workSheet.FormatsExport.Currency = "#,###,##0.00"
        workSheet.FormatsExport.DateTime = "dd/MM/yyyy H:mm"
        workSheet.FormatsExport.Float = "#,###,##0.00"
        workSheet.FormatsExport.[Integer] = "#,###,##0"
        workSheet.FormatsExport.Time = "H:mm"

        'worksheet outlook format
        workSheet.Options.AggregateFormat.Font.Name = "Arial"
        workSheet.Options.CustomDataFormat.Font.Name = "Arial"
        workSheet.Options.DefaultFont.Name = "Arial"
        workSheet.Options.FooterFormat.Font.Name = "Arial"
        workSheet.Options.HeaderFormat.Font.Name = "Arial"
        workSheet.Options.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.HyperlinkFormat.Font.Name = "Arial"
        workSheet.Options.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.[Single]
        workSheet.Options.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left
        workSheet.Options.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top
        workSheet.Options.NoteFormat.Font.Bold = True
        workSheet.Options.NoteFormat.Font.Name = "Tahoma"
        workSheet.Options.NoteFormat.Font.Size = 8.0F
        'workSheet.Options.CustomDataFormat.Font.Size = 40.0F

        workSheet.Options.TitlesFormat.Font.Bold = True
        workSheet.Options.TitlesFormat.Font.Name = "Arial"

        workSheet.Options.CustomDataFormat.Borders.Bottom.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Left.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Right.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Top.Color = Spire.DataExport.XLS.CellColor.Blue

        workSheet.SheetName = "Worksheet"
        workSheet.AutoFitColWidth = True

        Dim stripStyle1 As Spire.DataExport.XLS.StripStyle = New Spire.DataExport.XLS.StripStyle()

        Dim mcell As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell.Column = 2
        mcell.Row = 2
        If dsProjectGroup.Tables.Count > 0 Then
            If dsProjectGroup.Tables(0).Rows.Count > 0 Then
                mcell.Value = String.Format("{0} - Contractor's Quote", dsProjectGroup.Tables(0).Rows(0)("Name"))
            Else
                mcell.Value = String.Format("Contractor's Quote")
            End If
        Else
            mcell.Value = String.Format("Contractor's Quote")
        End If
        mcell.CellType = Spire.DataExport.XLS.CellType.String
        'mcell.Format = workSheet.Options.TitlesFormat
        'mcell.Format.Font.Bold = 1
        'mcell.Format.Font.Size = 34

        workSheet.HeaderRows = 15
        workSheet.Cells.Add(mcell)


        intCompanyInfoStartRow = 8
        intCompanyInfoStartColum = 2
        'workSheet.Cells.Add(acell)


        'add image
        Dim pic1 As Spire.DataExport.XLS.CellPicture = New Spire.DataExport.XLS.CellPicture()
        pic1.FileName = strLogo 'need to find out where the logo is
        pic1.Name = "Logo"

        cellExport.Pictures.Add(pic1)

        Dim img1 As Spire.DataExport.XLS.CellImage = New Spire.DataExport.XLS.CellImage
        img1.Column = 4
        img1.PictureName = "Logo"
        img1.Row = 1
        workSheet.Images.Add(img1)



        'add contact detail head field
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1.Column = intCompanyInfoStartColum
        mcell1.Row = intCompanyInfoStartRow
        mcell1.Value = "Claim No:"
        mcell1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1)


        Dim mcell2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2.Column = intCompanyInfoStartColum
        mcell2.Row = intCompanyInfoStartRow + 1
        mcell2.Value = "Claimant:"
        mcell2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2)


        Dim mcell3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3.Column = intCompanyInfoStartColum
        mcell3.Row = intCompanyInfoStartRow + 2
        mcell3.Value = "Site Location:"
        mcell3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3)


        Dim mcell4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4.Column = intCompanyInfoStartColum
        mcell4.Row = intCompanyInfoStartRow + 3
        mcell4.Value = "EQR Supervisor:"
        mcell4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4)


        Dim mcell5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5.Column = intCompanyInfoStartColum
        mcell5.Row = intCompanyInfoStartRow + 4
        mcell5.Value = "Contractor E-mail:"
        mcell5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5)


        Dim mcell6 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6.Column = intCompanyInfoStartColum
        mcell6.Row = intCompanyInfoStartRow + 5
        mcell6.Value = "Date:"
        mcell6.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6)

        'add contact detail head field contet
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_2.Column = intCompanyInfoStartColum + 1
        mcell1_2.Row = intCompanyInfoStartRow
        mcell1_2.Value = strClaimNo
        mcell1_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_2)


        Dim mcell2_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_2.Column = intCompanyInfoStartColum + 1
        mcell2_2.Row = intCompanyInfoStartRow + 1
        mcell2_2.Value = strClaimant
        mcell2_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_2)


        Dim mcell3_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_2.Column = intCompanyInfoStartColum + 1
        mcell3_2.Row = intCompanyInfoStartRow + 2
        mcell3_2.Value = strSiteLocation
        mcell3_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_2)


        Dim mcell4_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_2.Column = intCompanyInfoStartColum + 1
        mcell4_2.Row = intCompanyInfoStartRow + 3
        mcell4_2.Value = strEQRSupervisor
        mcell4_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_2)


        Dim mcell5_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_2.Column = intCompanyInfoStartColum + 1
        mcell5_2.Row = intCompanyInfoStartRow + 4
        mcell5_2.Value = strContractorEmail
        mcell5_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_2)


        Dim mcell6_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_2.Column = intCompanyInfoStartColum + 1
        mcell6_2.Row = intCompanyInfoStartRow + 5
        mcell6_2.Value = strScopeDate
        mcell6_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_2)

        'second part of heading
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_3.Column = intCompanyInfoStartColum + 2
        mcell1_3.Row = intCompanyInfoStartRow
        mcell1_3.Value = "Contractor:"
        mcell1_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_3)


        Dim mcell2_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_3.Column = intCompanyInfoStartColum + 2
        mcell2_3.Row = intCompanyInfoStartRow + 1
        mcell2_3.Value = "Address:"
        mcell2_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_3)


        Dim mcell3_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_3.Column = intCompanyInfoStartColum + 2
        mcell3_3.Row = intCompanyInfoStartRow + 2
        mcell3_3.Value = "Accreditation No:"
        mcell3_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_3)


        Dim mcell4_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_3.Column = intCompanyInfoStartColum + 2
        mcell4_3.Row = intCompanyInfoStartRow + 3
        mcell4_3.Value = "Contractor Phone:"
        mcell4_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_3)


        Dim mcell5_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_3.Column = intCompanyInfoStartColum + 2
        mcell5_3.Row = intCompanyInfoStartRow + 4
        mcell5_3.Value = "GST No:"
        mcell5_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_3)


        Dim mcell6_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_3.Column = intCompanyInfoStartColum + 2
        mcell6_3.Row = intCompanyInfoStartRow + 5
        mcell6_3.Value = "Total Price (incl. GST):"
        mcell6_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_3)

        'second part of heading
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_4.Column = intCompanyInfoStartColum + 3
        mcell1_4.Row = intCompanyInfoStartRow
        mcell1_4.Value = strContractor
        mcell1_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_4)


        Dim mcell2_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_4.Column = intCompanyInfoStartColum + 3
        mcell2_4.Row = intCompanyInfoStartRow + 1
        mcell2_4.Value = strAddress
        mcell2_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_4)


        Dim mcell3_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_4.Column = intCompanyInfoStartColum + 3
        mcell3_4.Row = intCompanyInfoStartRow + 2
        mcell3_4.Value = strAccreditationNo
        mcell3_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_4)


        Dim mcell4_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_4.Column = intCompanyInfoStartColum + 3
        mcell4_4.Row = intCompanyInfoStartRow + 3
        mcell4_4.Value = strContractorPhone
        mcell4_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_4)


        Dim mcell5_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_4.Column = intCompanyInfoStartColum + 3
        mcell5_4.Row = intCompanyInfoStartRow + 4
        mcell5_4.Value = strGSTNo
        mcell5_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_4)


        Dim mcell6_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_4.Column = intCompanyInfoStartColum + 3
        mcell6_4.Row = intCompanyInfoStartRow + 5
        mcell6_4.Value = strTotalPrice
        mcell6_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_4)

        'get data rows
        'build query       
        dsQuery = objScopeService.GetScopeItemsByScopeIdScopeItemStatus(ScopeId, 2)
        dsQuery.Tables(0).DefaultView.Sort = "ScopeGroup"
        Dim rowView As DataRowView
        Dim row As DataRow
        Dim intRowIndex As Integer
        Dim intColumIndex As Integer
        intRowIndex = 15
        intColumIndex = 1

        Dim strPrevScopeGroup As String = "****"
        Dim strScopeGroup As String = String.Empty
        Dim strPrevArea As String = "*****"
        Dim strArea As String = String.Empty
        Dim dynamiccell_1 As Spire.DataExport.XLS.Cell

        intRowIndex = intRowIndex + 1
        Dim Range1 As Integer = intRowIndex
        
        For Each rowView In dsQuery.Tables(0).DefaultView
            row = rowView.Row
            strScopeGroup = String.Format("{0}", row("ScopeGroup"))
            If strScopeGroup <> strPrevScopeGroup Then
                intRowIndex = intRowIndex + 1
                'Area
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 2
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = String.Format("{0}", row("ScopeGroup"))
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Note
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 3
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Description of Works"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'QTY
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 4
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Dimension"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Rate
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 5
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "$ Rate"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Cost
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 6
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Contractors Quote"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)
                
                'Room Total
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 7
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Room Total"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)
                
                'Scope Change
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 8
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Scope Change"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)
                
                intRowIndex = intRowIndex + 1
            End If
            strPrevScopeGroup = strScopeGroup
            
            strArea = String.Format("{0}", row("Area"))
            'Area
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 2
            dynamiccell_1.Row = intRowIndex
            If strArea <> strPrevArea Then
                dynamiccell_1.Value = strArea
            Else
                dynamiccell_1.Value = "-"
            End If
            strPrevArea = strArea
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'Note
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 3
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Description")) Then
                dynamiccell_1.Value = row("Description")
            Else
                dynamiccell_1.Value = ""
            End If

            If Not IsDBNull(row("Item")) Then
                If String.Format("{0}", row("Item")).Trim() <> String.Empty Then
                    dynamiccell_1.Value = String.Format("{0}: {1}", row("Item"), dynamiccell_1.Value)
                End If
            End If
            
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'QTY
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 4
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Quantity")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Quantity")).ToString()
            Else
                dynamiccell_1.Value = 0
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            'Rate
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 5
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Rate")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Rate")).ToString()
            Else
                dynamiccell_1.Value = 0
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            'Cost
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 6
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Cost")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Cost")).ToString("c")
            Else
                dynamiccell_1.Value = Convert.ToDouble(0).ToString("c")
            End If
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            intRowIndex = intRowIndex + 1
        Next
        Dim Range2 As Integer = intRowIndex - 1

        intFootTotalStartRow = intRowIndex
        intFootTotalStartColum = 5

        'heading
        Dim mcell1_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_5.Column = intFootTotalStartColum
        mcell1_5.Row = intFootTotalStartRow + 2
        mcell1_5.Value = "Subtotal (excl. GST)"
        mcell1_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_5)

        Dim mcell2_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_5.Column = intFootTotalStartColum
        mcell2_5.Row = intFootTotalStartRow + 3
        mcell2_5.Value = "Add 15% GST"
        mcell2_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_5)

        Dim mcell3_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_5.Column = intFootTotalStartColum
        mcell3_5.Row = intFootTotalStartRow + 4
        mcell3_5.Value = "Total Incl. GST"
        mcell3_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_5)

        Dim mcell4_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_5.Column = intFootTotalStartColum
        mcell4_5.Row = intFootTotalStartRow + 6
        mcell4_5.Value = "Grand Total with Scope Changes Excl. GST"
        mcell4_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_5)

        Dim mcell5_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_5.Column = intFootTotalStartColum
        mcell5_5.Row = intFootTotalStartRow + 7
        mcell5_5.Value = "Grand Total with Scope Changes Incl. GST"
        mcell5_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_5)

        'value 1
        'Dim CellFomat1_6 As New Spire.DataExport.XLS.CellFormat
        'CellFomat1_6.Font.Bold = True

        'value 2
        Dim mcell1_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_7.Column = intFootTotalStartColum + 1
        mcell1_7.Row = intFootTotalStartRow + 2
        mcell1_7.Value = ApprovedexcludeGstCost
        mcell1_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_7)

        Dim mcell2_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_7.Column = intFootTotalStartColum + 1
        mcell2_7.Row = intFootTotalStartRow + 3
        mcell2_7.Value = ApprovedGSTCost
        mcell2_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_7)

        Dim mcell3_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_7.Column = intFootTotalStartColum + 1
        mcell3_7.Row = intFootTotalStartRow + 4
        mcell3_7.Value = ApprovedInGSTCost
        mcell3_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_7)

        Dim mcell4_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_7.Column = intFootTotalStartColum + 1
        mcell4_7.Row = intFootTotalStartRow + 6
        mcell4_7.Value = GrandExGSTCost
        mcell4_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_7)

        Dim mcell5_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_7.Column = intFootTotalStartColum + 1
        mcell5_7.Row = intFootTotalStartRow + 7
        mcell5_7.Value = GrandInGSTCost
        mcell5_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_7)

        '''''''''''''''''
        'oleDbConnection.Open()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Dim InitialDataTable As New DataTable
        'connect to dataset
        workSheet.DataSource = Spire.DataExport.Common.ExportSource.DataTable
        'workSheet.DataTable = dsQuery.Tables(0)
        workSheet.DataTable = InitialDataTable
        'workSheet.SQLCommand = oleDbCommand
        workSheet.StartDataCol = CByte(1)
        cellExport.Sheets.Add(workSheet)
        Try
            'cellExport.SaveToFile()
            cellExport.SaveToHttpResponse(String.Format("{0}.xls", strNewExcelFileName), context.Response)
            result = strExcelFile
        Finally
            'oleDbConnection.Close()
        End Try
        
        Return result
    End Function
    
    Private Function GenerateSpreadsheetforGroupedWorksheet_SeparatedItem(ByVal ScopeId As Long, ByVal context As HttpContext) As String
        Dim result As String = String.Empty
        Dim objScopeService As ScopeServices = New ScopeServices()
        objScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim objManagementService As ManagementService = New ManagementService()
        objManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim objScope As Scope = objScopeService.GetScopeByScopeId(ScopeId)
        Dim objProject As Project = objManagementService.GetProjectByProjectId(objScope.ProjectId)
        
        Dim dsQuery As DataSet
        Dim intFootTotalStartRow As Integer
        Dim intFootTotalStartColum As Integer
        Dim intCompanyInfoStartRow As Integer
        Dim intCompanyInfoStartColum As Integer
        Dim strExcelFile As String
        Dim strNewExcelFileName As String
        Dim dsProjectGroup As DataSet = New DataSet()

        Dim strClaimNo As String = ""
        Dim strClaimant As String = ""
        Dim strSiteLocation As String = ""
        Dim strEQRSupervisor As String = ""
        Dim strContractorEmail As String = ""
        Dim strScopeDate As String = ""
        Dim strContractor As String = ""
        Dim strAddress As String = ""
        Dim strAccreditationNo As String = ""
        Dim strContractorPhone As String = ""
        Dim strGSTNo As String = ""
        Dim strTotalPrice As String = ""
        Dim dsProject As DataSet
        Dim ProjectOwner As ProjectOwner
        Dim strLogo As String = ""
                
        dsProjectGroup = objScopeService.GetProjectGroupsByProjectOwnerId(objProject.ProjectOwnerId)
        dsProject = objManagementService.GetProjectInfoByProjectId(objScope.ProjectId)

        'get project group email
        If dsProjectGroup.Tables.Count > 0 Then
            If dsProjectGroup.Tables(0).Rows.Count > 0 Then
                If Not Convert.IsDBNull(dsProjectGroup.Tables(0).Rows(0).Item("Email")) Then
                    strContractorEmail = dsProjectGroup.Tables(0).Rows(0).Item("Email")
                End If
            End If
        End If

        'get claimant detail
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")) Then
            strClaimNo = dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")
        End If
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
            strScopeDate = dsProject.Tables(0).Rows(0).Item("ScopeDate").ToString()
        End If
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
            strScopeDate = CType(dsProject.Tables(0).Rows(0).Item("ScopeDate"), DateTime).ToString("dd/MM/yyyy")
        End If

        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Address")) Then
            strSiteLocation = dsProject.Tables(0).Rows(0).Item("Address")
        End If

        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ContactName")) Then
            strClaimant = dsProject.Tables(0).Rows(0).Item("ContactName")
        End If

        'get project owner detail
        ProjectOwner = objManagementService.GetProjectOwnerByProjectOwnerId(objProject.ProjectOwnerId)
        strEQRSupervisor = ProjectOwner.EQRSupervisor
        strContractor = ProjectOwner.Name
        strContractorPhone = ProjectOwner.Contact1
        strAddress = ProjectOwner.Address
        strGSTNo = ProjectOwner.GSTNumber
        strAccreditationNo = ProjectOwner.AccreditationNumber
        
        Dim ApprovedexcludeGstCost As String = objScope.Cost.ToString("c")
        Dim ApprovedGSTCost As String = (objScope.Total - objScope.Cost).ToString("c")
        Dim ApprovedInGSTCost As String = objScope.Total.ToString("c")

        Dim GrandExGSTCost As String = (objScope.Cost1 + objScope.Cost).ToString("c")
        Dim GrandInGSTCost As String = (objScope.Total1 + objScope.Total).ToString("c")

        strTotalPrice = ApprovedInGSTCost

        strLogo = String.Format("{0}\Images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), ProjectOwner.Identifier, ProjectOwner.Logo)

        strNewExcelFileName = String.Format("ExcelExport_{0}_{1}", ProjectOwner.ContactId, ScopeId)
        strExcelFile = String.Format("{0}\Downloads\{1}\{2}.xls", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport", strNewExcelFileName)

        If (Not System.IO.Directory.Exists(String.Format("{0}\Downloads\{1}", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport"))) Then
            System.IO.Directory.CreateDirectory(String.Format("{0}\Downloads\{1}", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport"))
        End If

        Dim workSheet As New Spire.DataExport.XLS.WorkSheet()

        'initialzing the cellexport tool
        cellExport = New Spire.DataExport.XLS.CellExport
        cellExport.ActionAfterExport = Spire.DataExport.Common.ActionType.OpenView

        'culture format setting
        cellExport.DataFormats.CultureName = "en-NZ"
        cellExport.DataFormats.Currency = "#,###,##0.00"
        cellExport.DataFormats.DateTime = "dd/MM/yyyy H:mm"
        cellExport.DataFormats.Float = "#,###,##0.00"
        cellExport.DataFormats.[Integer] = "#,###,##0"
        cellExport.DataFormats.Time = "H:mm"

        'set up file name and location
        cellExport.FileName = strExcelFile

        'outlook formating
        cellExport.SheetOptions.AggregateFormat.Font.Name = "Arial"
        cellExport.SheetOptions.CustomDataFormat.Font.Name = "Arial"
        cellExport.SheetOptions.DefaultFont.Name = "Arial"
        cellExport.SheetOptions.FooterFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HeaderFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue
        cellExport.SheetOptions.HyperlinkFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.[Single]
        cellExport.SheetOptions.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left
        cellExport.SheetOptions.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top
        cellExport.SheetOptions.NoteFormat.Font.Bold = True
        cellExport.SheetOptions.NoteFormat.Font.Name = "Tahoma"
        cellExport.SheetOptions.NoteFormat.Font.Size = 8.0F
        cellExport.SheetOptions.TitlesFormat.FillStyle.Background = Spire.DataExport.XLS.CellColor.Gray40Percent
        cellExport.SheetOptions.TitlesFormat.Font.Bold = True
        cellExport.SheetOptions.TitlesFormat.Font.Name = "Arial"



        'worksheet culture format
        workSheet.FormatsExport.CultureName = "en-NZ"
        workSheet.FormatsExport.Currency = "#,###,##0.00"
        workSheet.FormatsExport.DateTime = "dd/MM/yyyy H:mm"
        workSheet.FormatsExport.Float = "#,###,##0.00"
        workSheet.FormatsExport.[Integer] = "#,###,##0"
        workSheet.FormatsExport.Time = "H:mm"

        'worksheet outlook format
        workSheet.Options.AggregateFormat.Font.Name = "Arial"
        workSheet.Options.CustomDataFormat.Font.Name = "Arial"
        workSheet.Options.DefaultFont.Name = "Arial"
        workSheet.Options.FooterFormat.Font.Name = "Arial"
        workSheet.Options.HeaderFormat.Font.Name = "Arial"
        workSheet.Options.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.HyperlinkFormat.Font.Name = "Arial"
        workSheet.Options.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.[Single]
        workSheet.Options.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left
        workSheet.Options.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top
        workSheet.Options.NoteFormat.Font.Bold = True
        workSheet.Options.NoteFormat.Font.Name = "Tahoma"
        workSheet.Options.NoteFormat.Font.Size = 8.0F
        'workSheet.Options.CustomDataFormat.Font.Size = 40.0F

        workSheet.Options.TitlesFormat.Font.Bold = True
        workSheet.Options.TitlesFormat.Font.Name = "Arial"

        workSheet.Options.CustomDataFormat.Borders.Bottom.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Left.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Right.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Top.Color = Spire.DataExport.XLS.CellColor.Blue

        workSheet.SheetName = "Sheet 1"
        workSheet.AutoFitColWidth = True

        Dim stripStyle1 As Spire.DataExport.XLS.StripStyle = New Spire.DataExport.XLS.StripStyle()

        Dim mcell As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell.Column = 2
        mcell.Row = 2
        mcell.Value = "Kaiapoi Hub - Contractor's Quote"
        mcell.CellType = Spire.DataExport.XLS.CellType.String
        'mcell.Format = workSheet.Options.TitlesFormat
        'mcell.Format.Font.Bold = 1
        'mcell.Format.Font.Size = 34

        workSheet.HeaderRows = 15
        workSheet.Cells.Add(mcell)


        intCompanyInfoStartRow = 8
        intCompanyInfoStartColum = 2
        'workSheet.Cells.Add(acell)


        'add image
        Dim pic1 As Spire.DataExport.XLS.CellPicture = New Spire.DataExport.XLS.CellPicture()
        pic1.FileName = strLogo 'need to find out where the logo is
        pic1.Name = "Logo"

        cellExport.Pictures.Add(pic1)

        Dim img1 As Spire.DataExport.XLS.CellImage = New Spire.DataExport.XLS.CellImage
        img1.Column = 4
        img1.PictureName = "Logo"
        img1.Row = 1
        workSheet.Images.Add(img1)



        'add contact detail head field
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1.Column = intCompanyInfoStartColum
        mcell1.Row = intCompanyInfoStartRow
        mcell1.Value = "Claim No:"
        mcell1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1)


        Dim mcell2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2.Column = intCompanyInfoStartColum
        mcell2.Row = intCompanyInfoStartRow + 1
        mcell2.Value = "Claimant:"
        mcell2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2)


        Dim mcell3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3.Column = intCompanyInfoStartColum
        mcell3.Row = intCompanyInfoStartRow + 2
        mcell3.Value = "Site Location:"
        mcell3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3)


        Dim mcell4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4.Column = intCompanyInfoStartColum
        mcell4.Row = intCompanyInfoStartRow + 3
        mcell4.Value = "EQR Supervisor:"
        mcell4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4)


        Dim mcell5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5.Column = intCompanyInfoStartColum
        mcell5.Row = intCompanyInfoStartRow + 4
        mcell5.Value = "Contractor E-mail:"
        mcell5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5)


        Dim mcell6 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6.Column = intCompanyInfoStartColum
        mcell6.Row = intCompanyInfoStartRow + 5
        mcell6.Value = "Date:"
        mcell6.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6)

        'add contact detail head field contet
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_2.Column = intCompanyInfoStartColum + 1
        mcell1_2.Row = intCompanyInfoStartRow
        mcell1_2.Value = strClaimNo
        mcell1_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_2)


        Dim mcell2_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_2.Column = intCompanyInfoStartColum + 1
        mcell2_2.Row = intCompanyInfoStartRow + 1
        mcell2_2.Value = strClaimant
        mcell2_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_2)


        Dim mcell3_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_2.Column = intCompanyInfoStartColum + 1
        mcell3_2.Row = intCompanyInfoStartRow + 2
        mcell3_2.Value = strSiteLocation
        mcell3_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_2)


        Dim mcell4_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_2.Column = intCompanyInfoStartColum + 1
        mcell4_2.Row = intCompanyInfoStartRow + 3
        mcell4_2.Value = strEQRSupervisor
        mcell4_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_2)


        Dim mcell5_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_2.Column = intCompanyInfoStartColum + 1
        mcell5_2.Row = intCompanyInfoStartRow + 4
        mcell5_2.Value = strContractorEmail
        mcell5_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_2)


        Dim mcell6_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_2.Column = intCompanyInfoStartColum + 1
        mcell6_2.Row = intCompanyInfoStartRow + 5
        mcell6_2.Value = strScopeDate
        mcell6_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_2)

        'second part of heading
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_3.Column = intCompanyInfoStartColum + 2
        mcell1_3.Row = intCompanyInfoStartRow
        mcell1_3.Value = "Contractor:"
        mcell1_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_3)


        Dim mcell2_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_3.Column = intCompanyInfoStartColum + 2
        mcell2_3.Row = intCompanyInfoStartRow + 1
        mcell2_3.Value = "Address:"
        mcell2_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_3)


        Dim mcell3_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_3.Column = intCompanyInfoStartColum + 2
        mcell3_3.Row = intCompanyInfoStartRow + 2
        mcell3_3.Value = "Accreditation No:"
        mcell3_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_3)


        Dim mcell4_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_3.Column = intCompanyInfoStartColum + 2
        mcell4_3.Row = intCompanyInfoStartRow + 3
        mcell4_3.Value = "Contractor Phone:"
        mcell4_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_3)


        Dim mcell5_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_3.Column = intCompanyInfoStartColum + 2
        mcell5_3.Row = intCompanyInfoStartRow + 4
        mcell5_3.Value = "GST No:"
        mcell5_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_3)


        Dim mcell6_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_3.Column = intCompanyInfoStartColum + 2
        mcell6_3.Row = intCompanyInfoStartRow + 5
        mcell6_3.Value = "Total Price (incl. GST):"
        mcell6_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_3)

        'second part of heading
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_4.Column = intCompanyInfoStartColum + 3
        mcell1_4.Row = intCompanyInfoStartRow
        mcell1_4.Value = strContractor
        mcell1_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_4)


        Dim mcell2_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_4.Column = intCompanyInfoStartColum + 3
        mcell2_4.Row = intCompanyInfoStartRow + 1
        mcell2_4.Value = strAddress
        mcell2_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_4)


        Dim mcell3_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_4.Column = intCompanyInfoStartColum + 3
        mcell3_4.Row = intCompanyInfoStartRow + 2
        mcell3_4.Value = strAccreditationNo
        mcell3_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_4)


        Dim mcell4_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_4.Column = intCompanyInfoStartColum + 3
        mcell4_4.Row = intCompanyInfoStartRow + 3
        mcell4_4.Value = strContractorPhone
        mcell4_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_4)


        Dim mcell5_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_4.Column = intCompanyInfoStartColum + 3
        mcell5_4.Row = intCompanyInfoStartRow + 4
        mcell5_4.Value = strGSTNo
        mcell5_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_4)


        Dim mcell6_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_4.Column = intCompanyInfoStartColum + 3
        mcell6_4.Row = intCompanyInfoStartRow + 5
        mcell6_4.Value = strTotalPrice
        mcell6_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_4)

        'get data rows
        'build query       
        dsQuery = objScopeService.GetScopeItemsByScopeIdScopeItemStatus(ScopeId, 2)
        dsQuery.Tables(0).DefaultView.Sort = "ScopeGroup"
        Dim rowView As DataRowView
        Dim row As DataRow
        Dim intRowIndex As Integer
        Dim intColumIndex As Integer
        intRowIndex = 15
        intColumIndex = 1

        Dim strPrevScopeGroup As String = "****"
        Dim strScopeGroup As String = String.Empty
        Dim dynamiccell_1 As Spire.DataExport.XLS.Cell

        intRowIndex = intRowIndex + 1
        Dim Range1 As Integer = intRowIndex
        
        For Each rowView In dsQuery.Tables(0).DefaultView
            row = rowView.Row
            strScopeGroup = String.Format("{0}", row("ScopeGroup"))
            If strScopeGroup <> strPrevScopeGroup Then
                intRowIndex = intRowIndex + 1
                'Area
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 2
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = String.Format("{0}", row("ScopeGroup"))
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Item
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 3
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = String.Empty
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Note
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 4
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Description of Works"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'QTY
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 5
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Dimension"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Rate
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 6
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "$ Rate"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)

                'Cost
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 7
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Contractors Quote"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)
                
                'Room Total
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 8
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Contractors Quote"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)
                
                'Scope Change
                dynamiccell_1 = New Spire.DataExport.XLS.Cell
                dynamiccell_1.Column = 9
                dynamiccell_1.Row = intRowIndex
                dynamiccell_1.Value = "Scope Change"
                dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
                workSheet.Cells.Add(dynamiccell_1)
                
                intRowIndex = intRowIndex + 1
            End If
            strPrevScopeGroup = strScopeGroup
            'Area
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 2
            dynamiccell_1.Row = intRowIndex
            If Not Convert.IsDBNull(row("Area")) Then
                dynamiccell_1.Value = row("Area")
            Else
                dynamiccell_1.Value = ""
            End If
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'Item
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 3
            dynamiccell_1.Row = intRowIndex
            If Not Convert.IsDBNull(row("Item")) Then
                dynamiccell_1.Value = row("Item")
            Else
                dynamiccell_1.Value = ""
            End If
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'Note
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 4
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Description")) Then
                dynamiccell_1.Value = row("Description")
            Else
                dynamiccell_1.Value = ""
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'QTY
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 5
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Quantity")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Quantity")).ToString()
            Else
                dynamiccell_1.Value = 0
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            'Rate
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 6
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Rate")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Rate")).ToString()
            Else
                dynamiccell_1.Value = 0
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            'Cost
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 7
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Cost")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Cost")).ToString("c")
            Else
                dynamiccell_1.Value = Convert.ToDouble(0).ToString("c")
            End If
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            intRowIndex = intRowIndex + 1
        Next
        Dim Range2 As Integer = intRowIndex - 1

        intFootTotalStartRow = intRowIndex
        intFootTotalStartColum = 6

        'heading
        Dim mcell1_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_5.Column = intFootTotalStartColum
        mcell1_5.Row = intFootTotalStartRow + 2
        mcell1_5.Value = "Subtotal (excl. GST)"
        mcell1_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_5)

        Dim mcell2_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_5.Column = intFootTotalStartColum
        mcell2_5.Row = intFootTotalStartRow + 3
        mcell2_5.Value = "Add 15% GST"
        mcell2_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_5)

        Dim mcell3_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_5.Column = intFootTotalStartColum
        mcell3_5.Row = intFootTotalStartRow + 4
        mcell3_5.Value = "Total Incl. GST"
        mcell3_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_5)

        Dim mcell4_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_5.Column = intFootTotalStartColum
        mcell4_5.Row = intFootTotalStartRow + 6
        mcell4_5.Value = "Grand Total with Scope Changes Excl. GST"
        mcell4_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_5)

        Dim mcell5_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_5.Column = intFootTotalStartColum
        mcell5_5.Row = intFootTotalStartRow + 7
        mcell5_5.Value = "Grand Total with Scope Changes Incl. GST"
        mcell5_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_5)

        'value 1
        'Dim CellFomat1_6 As New Spire.DataExport.XLS.CellFormat
        'CellFomat1_6.Font.Bold = True

        'value 2
        Dim mcell1_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_7.Column = intFootTotalStartColum + 1
        mcell1_7.Row = intFootTotalStartRow + 2
        mcell1_7.Value = ApprovedexcludeGstCost
        mcell1_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_7)

        Dim mcell2_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_7.Column = intFootTotalStartColum + 1
        mcell2_7.Row = intFootTotalStartRow + 3
        mcell2_7.Value = ApprovedGSTCost
        mcell2_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_7)

        Dim mcell3_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_7.Column = intFootTotalStartColum + 1
        mcell3_7.Row = intFootTotalStartRow + 4
        mcell3_7.Value = ApprovedInGSTCost
        mcell3_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_7)

        Dim mcell4_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_7.Column = intFootTotalStartColum + 1
        mcell4_7.Row = intFootTotalStartRow + 6
        mcell4_7.Value = GrandExGSTCost
        mcell4_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_7)

        Dim mcell5_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_7.Column = intFootTotalStartColum + 1
        mcell5_7.Row = intFootTotalStartRow + 7
        mcell5_7.Value = GrandInGSTCost
        mcell5_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_7)

        '''''''''''''''''
        'oleDbConnection.Open()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Dim InitialDataTable As New DataTable
        'connect to dataset
        workSheet.DataSource = Spire.DataExport.Common.ExportSource.DataTable
        'workSheet.DataTable = dsQuery.Tables(0)
        workSheet.DataTable = InitialDataTable
        'workSheet.SQLCommand = oleDbCommand
        workSheet.StartDataCol = CByte(1)
        cellExport.Sheets.Add(workSheet)
        Try
            'cellExport.SaveToFile()
            cellExport.SaveToHttpResponse(String.Format("{0}.xls", strNewExcelFileName), context.Response)
            result = strExcelFile
        Finally
            'oleDbConnection.Close()
        End Try
        
        Return result
    End Function
    
    Private Function GenerateSpreadsheetforWorksheet(ByVal ScopeId As Long) As String
        Dim result As String = String.Empty
        Dim objScopeService As ScopeServices = New ScopeServices()
        objScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim objManagementService As ManagementService = New ManagementService()
        objManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim objScope As Scope = objScopeService.GetScopeByScopeId(ScopeId)
        Dim objProject As Project = objManagementService.GetProjectByProjectId(objScope.ProjectId)
        
        Dim dsQuery As DataSet
        Dim intFootTotalStartRow As Integer
        Dim intFootTotalStartColum As Integer
        Dim intCompanyInfoStartRow As Integer
        Dim intCompanyInfoStartColum As Integer
        Dim strExcelFile As String
        Dim strNewExcelFileName As String
        Dim dsProjectGroup As DataSet = New DataSet()

        Dim strClaimNo As String = ""
        Dim strClaimant As String = ""
        Dim strSiteLocation As String = ""
        Dim strEQRSupervisor As String = ""
        Dim strContractorEmail As String = ""
        Dim strScopeDate As String = ""
        Dim strContractor As String = ""
        Dim strAddress As String = ""
        Dim strAccreditationNo As String = ""
        Dim strContractorPhone As String = ""
        Dim strGSTNo As String = ""
        Dim strTotalPrice As String = ""
        Dim dsProject As DataSet
        Dim ProjectOwner As ProjectOwner
        Dim strLogo As String = ""

        dsProjectGroup = objScopeService.GetProjectGroupsByProjectOwnerId(objProject.ProjectOwnerId)
        dsProject = objManagementService.GetProjectInfoByProjectId(objScope.ProjectId)

        'get project group email
        If dsProjectGroup.Tables.Count > 0 Then
            If dsProjectGroup.Tables(0).Rows.Count > 0 Then
                If Not Convert.IsDBNull(dsProjectGroup.Tables(0).Rows(0).Item("Email")) Then
                    strContractorEmail = dsProjectGroup.Tables(0).Rows(0).Item("Email")
                End If
            End If
        End If
        
        'get claimant detail
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")) Then
            strClaimNo = dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")
        End If
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
            strScopeDate = dsProject.Tables(0).Rows(0).Item("ScopeDate").ToString()
        End If
        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
            strScopeDate = CType(dsProject.Tables(0).Rows(0).Item("ScopeDate"), DateTime).ToString("dd/MM/yyyy")
        End If

        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Address")) Then
            strSiteLocation = dsProject.Tables(0).Rows(0).Item("Address")
        End If

        If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ContactName")) Then
            strClaimant = dsProject.Tables(0).Rows(0).Item("ContactName")
        End If

        'get project owner detail
        ProjectOwner = objManagementService.GetProjectOwnerByProjectOwnerId(objProject.ProjectOwnerId)
        strEQRSupervisor = ProjectOwner.EQRSupervisor
        strContractor = ProjectOwner.Name
        strContractorPhone = ProjectOwner.Contact1
        strAddress = ProjectOwner.Address
        strGSTNo = ProjectOwner.GSTNumber
        strAccreditationNo = ProjectOwner.AccreditationNumber
        
        Dim ApprovedexcludeGstCost As String = objScope.Cost.ToString("c")
        Dim ApprovedGSTCost As String = (objScope.Total - objScope.Cost).ToString("c")
        Dim ApprovedInGSTCost As String = objScope.Total.ToString("c")

        Dim GrandExGSTCost As String = (objScope.Cost1 + objScope.Cost).ToString("c")
        Dim GrandInGSTCost As String = (objScope.Total1 + objScope.Total).ToString("c")

        strTotalPrice = ApprovedInGSTCost

        strLogo = String.Format("{0}\Images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), ProjectOwner.Identifier, ProjectOwner.Logo)

        strNewExcelFileName = String.Format("ExcelExport_{0}_{1}", ProjectOwner.ContactId, ScopeId)
        strExcelFile = String.Format("{0}\Downloads\{1}\{2}.xls", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport", strNewExcelFileName)

        If (Not System.IO.Directory.Exists(String.Format("{0}\Downloads\{1}", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport"))) Then
            System.IO.Directory.CreateDirectory(String.Format("{0}\Downloads\{1}", ConfigurationManager.AppSettings("ProjectPath"), "ExcelExport"))
        End If

        Dim workSheet As New Spire.DataExport.XLS.WorkSheet()

        'initialzing the cellexport tool
        cellExport = New Spire.DataExport.XLS.CellExport
        cellExport.ActionAfterExport = Spire.DataExport.Common.ActionType.OpenView

        'culture format setting
        cellExport.DataFormats.CultureName = "en-NZ"
        cellExport.DataFormats.Currency = "#,###,##0.00"
        cellExport.DataFormats.DateTime = "dd/MM/yyyy H:mm"
        cellExport.DataFormats.Float = "#,###,##0.00"
        cellExport.DataFormats.[Integer] = "#,###,##0"
        cellExport.DataFormats.Time = "H:mm"

        'set up file name and location
        cellExport.FileName = strExcelFile

        'outlook formating
        cellExport.SheetOptions.AggregateFormat.Font.Name = "Arial"
        cellExport.SheetOptions.CustomDataFormat.Font.Name = "Arial"
        cellExport.SheetOptions.DefaultFont.Name = "Arial"
        cellExport.SheetOptions.FooterFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HeaderFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue
        cellExport.SheetOptions.HyperlinkFormat.Font.Name = "Arial"
        cellExport.SheetOptions.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.[Single]
        cellExport.SheetOptions.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left
        cellExport.SheetOptions.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top
        cellExport.SheetOptions.NoteFormat.Font.Bold = True
        cellExport.SheetOptions.NoteFormat.Font.Name = "Tahoma"
        cellExport.SheetOptions.NoteFormat.Font.Size = 8.0F
        cellExport.SheetOptions.TitlesFormat.FillStyle.Background = Spire.DataExport.XLS.CellColor.Gray40Percent
        cellExport.SheetOptions.TitlesFormat.Font.Bold = True
        cellExport.SheetOptions.TitlesFormat.Font.Name = "Arial"

        'worksheet culture format
        workSheet.FormatsExport.CultureName = "en-NZ"
        workSheet.FormatsExport.Currency = "#,###,##0.00"
        workSheet.FormatsExport.DateTime = "dd/MM/yyyy H:mm"
        workSheet.FormatsExport.Float = "#,###,##0.00"
        workSheet.FormatsExport.[Integer] = "#,###,##0"
        workSheet.FormatsExport.Time = "H:mm"

        'worksheet outlook format
        workSheet.Options.AggregateFormat.Font.Name = "Arial"
        workSheet.Options.CustomDataFormat.Font.Name = "Arial"
        workSheet.Options.DefaultFont.Name = "Arial"
        workSheet.Options.FooterFormat.Font.Name = "Arial"
        workSheet.Options.HeaderFormat.Font.Name = "Arial"
        workSheet.Options.HyperlinkFormat.Font.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.HyperlinkFormat.Font.Name = "Arial"
        workSheet.Options.HyperlinkFormat.Font.Underline = Spire.DataExport.XLS.XlsFontUnderline.[Single]
        workSheet.Options.NoteFormat.Alignment.Horizontal = Spire.DataExport.XLS.HorizontalAlignment.Left
        workSheet.Options.NoteFormat.Alignment.Vertical = Spire.DataExport.XLS.VerticalAlignment.Top
        workSheet.Options.NoteFormat.Font.Bold = True
        workSheet.Options.NoteFormat.Font.Name = "Tahoma"
        workSheet.Options.NoteFormat.Font.Size = 8.0F
        'workSheet.Options.CustomDataFormat.Font.Size = 40.0F

        workSheet.Options.TitlesFormat.Font.Bold = True
        workSheet.Options.TitlesFormat.Font.Name = "Arial"

        workSheet.Options.CustomDataFormat.Borders.Bottom.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Left.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Right.Color = Spire.DataExport.XLS.CellColor.Blue
        workSheet.Options.CustomDataFormat.Borders.Top.Color = Spire.DataExport.XLS.CellColor.Blue

        workSheet.SheetName = "Sheet 1"
        workSheet.AutoFitColWidth = True

        Dim stripStyle1 As Spire.DataExport.XLS.StripStyle = New Spire.DataExport.XLS.StripStyle()

        Dim mcell As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell.Column = 2
        mcell.Row = 2
        mcell.Value = "Kaiapoi Hub - Contractor's Quote"
        mcell.CellType = Spire.DataExport.XLS.CellType.String
        'mcell.Format = workSheet.Options.TitlesFormat
        'mcell.Format.Font.Bold = 1
        'mcell.Format.Font.Size = 34

        workSheet.HeaderRows = 15
        workSheet.Cells.Add(mcell)


        intCompanyInfoStartRow = 8
        intCompanyInfoStartColum = 2
        'workSheet.Cells.Add(acell)

        'add image
        Dim pic1 As Spire.DataExport.XLS.CellPicture = New Spire.DataExport.XLS.CellPicture()
        pic1.FileName = strLogo 'need to find out where the logo is
        pic1.Name = "Logo"

        cellExport.Pictures.Add(pic1)

        Dim img1 As Spire.DataExport.XLS.CellImage = New Spire.DataExport.XLS.CellImage
        img1.Column = 4
        img1.PictureName = "Logo"
        img1.Row = 1
        workSheet.Images.Add(img1)

        'add contact detail head field
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1.Column = intCompanyInfoStartColum
        mcell1.Row = intCompanyInfoStartRow
        mcell1.Value = "Claim No:"
        mcell1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1)


        Dim mcell2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2.Column = intCompanyInfoStartColum
        mcell2.Row = intCompanyInfoStartRow + 1
        mcell2.Value = "Claimant:"
        mcell2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2)


        Dim mcell3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3.Column = intCompanyInfoStartColum
        mcell3.Row = intCompanyInfoStartRow + 2
        mcell3.Value = "Site Location:"
        mcell3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3)


        Dim mcell4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4.Column = intCompanyInfoStartColum
        mcell4.Row = intCompanyInfoStartRow + 3
        mcell4.Value = "EQR Supervisor:"
        mcell4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4)


        Dim mcell5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5.Column = intCompanyInfoStartColum
        mcell5.Row = intCompanyInfoStartRow + 4
        mcell5.Value = "Contractor E-mail:"
        mcell5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5)


        Dim mcell6 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6.Column = intCompanyInfoStartColum
        mcell6.Row = intCompanyInfoStartRow + 5
        mcell6.Value = "Date:"
        mcell6.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6)

        'add contact detail head field contet
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_2.Column = intCompanyInfoStartColum + 1
        mcell1_2.Row = intCompanyInfoStartRow
        mcell1_2.Value = strClaimNo
        mcell1_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_2)


        Dim mcell2_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_2.Column = intCompanyInfoStartColum + 1
        mcell2_2.Row = intCompanyInfoStartRow + 1
        mcell2_2.Value = strClaimant
        mcell2_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_2)


        Dim mcell3_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_2.Column = intCompanyInfoStartColum + 1
        mcell3_2.Row = intCompanyInfoStartRow + 2
        mcell3_2.Value = strSiteLocation
        mcell3_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_2)


        Dim mcell4_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_2.Column = intCompanyInfoStartColum + 1
        mcell4_2.Row = intCompanyInfoStartRow + 3
        mcell4_2.Value = strEQRSupervisor
        mcell4_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_2)


        Dim mcell5_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_2.Column = intCompanyInfoStartColum + 1
        mcell5_2.Row = intCompanyInfoStartRow + 4
        mcell5_2.Value = strContractorEmail
        mcell5_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_2)


        Dim mcell6_2 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_2.Column = intCompanyInfoStartColum + 1
        mcell6_2.Row = intCompanyInfoStartRow + 5
        mcell6_2.Value = strScopeDate
        mcell6_2.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_2)

        'second part of heading
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_3.Column = intCompanyInfoStartColum + 2
        mcell1_3.Row = intCompanyInfoStartRow
        mcell1_3.Value = "Contractor:"
        mcell1_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_3)


        Dim mcell2_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_3.Column = intCompanyInfoStartColum + 2
        mcell2_3.Row = intCompanyInfoStartRow + 1
        mcell2_3.Value = "Address:"
        mcell2_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_3)


        Dim mcell3_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_3.Column = intCompanyInfoStartColum + 2
        mcell3_3.Row = intCompanyInfoStartRow + 2
        mcell3_3.Value = "Accreditation No:"
        mcell3_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_3)


        Dim mcell4_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_3.Column = intCompanyInfoStartColum + 2
        mcell4_3.Row = intCompanyInfoStartRow + 3
        mcell4_3.Value = "Contractor Phone:"
        mcell4_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_3)


        Dim mcell5_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_3.Column = intCompanyInfoStartColum + 2
        mcell5_3.Row = intCompanyInfoStartRow + 4
        mcell5_3.Value = "GST No:"
        mcell5_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_3)


        Dim mcell6_3 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_3.Column = intCompanyInfoStartColum + 2
        mcell6_3.Row = intCompanyInfoStartRow + 5
        mcell6_3.Value = "Total Price (incl. GST):"
        mcell6_3.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_3)

        'second part of heading
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim mcell1_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_4.Column = intCompanyInfoStartColum + 3
        mcell1_4.Row = intCompanyInfoStartRow
        mcell1_4.Value = strContractor
        mcell1_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_4)


        Dim mcell2_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_4.Column = intCompanyInfoStartColum + 3
        mcell2_4.Row = intCompanyInfoStartRow + 1
        mcell2_4.Value = strAddress
        mcell2_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_4)


        Dim mcell3_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_4.Column = intCompanyInfoStartColum + 3
        mcell3_4.Row = intCompanyInfoStartRow + 2
        mcell3_4.Value = strAccreditationNo
        mcell3_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_4)


        Dim mcell4_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_4.Column = intCompanyInfoStartColum + 3
        mcell4_4.Row = intCompanyInfoStartRow + 3
        mcell4_4.Value = strContractorPhone
        mcell4_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_4)


        Dim mcell5_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_4.Column = intCompanyInfoStartColum + 3
        mcell5_4.Row = intCompanyInfoStartRow + 4
        mcell5_4.Value = strGSTNo
        mcell5_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_4)


        Dim mcell6_4 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell6_4.Column = intCompanyInfoStartColum + 3
        mcell6_4.Row = intCompanyInfoStartRow + 5
        mcell6_4.Value = strTotalPrice
        mcell6_4.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell6_4)

        'get data rows
        'build query        
        dsQuery = objScopeService.GetScopeItemsByScopeIdScopeItemStatus(ScopeId, 2)
        dsQuery.Tables(0).DefaultView.Sort = "ScopeGroup"
        Dim row As DataRow
        Dim intRowIndex As Integer
        Dim intColumIndex As Integer
        intRowIndex = 15
        intColumIndex = 1

        'create Head
        'Area
        Dim dynamiccell_1 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        dynamiccell_1.Column = 2
        dynamiccell_1.Row = intRowIndex
        dynamiccell_1.Value = "AREA"
        dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(dynamiccell_1)

        'Item
        dynamiccell_1 = New Spire.DataExport.XLS.Cell
        dynamiccell_1.Column = 3
        dynamiccell_1.Row = intRowIndex
        dynamiccell_1.Value = "ITEM"
        dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(dynamiccell_1)

        'Note
        dynamiccell_1 = New Spire.DataExport.XLS.Cell
        dynamiccell_1.Column = 4
        dynamiccell_1.Row = intRowIndex
        dynamiccell_1.Value = "NOTE"
        dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(dynamiccell_1)

        'QTY
        dynamiccell_1 = New Spire.DataExport.XLS.Cell
        dynamiccell_1.Column = 5
        dynamiccell_1.Row = intRowIndex
        dynamiccell_1.Value = "QTY"
        dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(dynamiccell_1)

        'Rate
        dynamiccell_1 = New Spire.DataExport.XLS.Cell
        dynamiccell_1.Column = 6
        dynamiccell_1.Row = intRowIndex
        dynamiccell_1.Value = "RATE"
        dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(dynamiccell_1)

        'Cost
        dynamiccell_1 = New Spire.DataExport.XLS.Cell
        dynamiccell_1.Column = 7
        dynamiccell_1.Row = intRowIndex
        dynamiccell_1.Value = "COST"
        dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(dynamiccell_1)

        intRowIndex = intRowIndex + 1
        Dim Range1 As Integer = intRowIndex
        For Each row In dsQuery.Tables(0).Rows
            'Area
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 2
            dynamiccell_1.Row = intRowIndex
            If Not Convert.IsDBNull(row("Area")) Then
                dynamiccell_1.Value = row("Area")
            Else
                dynamiccell_1.Value = ""
            End If
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'Item
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 3
            dynamiccell_1.Row = intRowIndex
            If Not Convert.IsDBNull(row("Item")) Then
                dynamiccell_1.Value = row("Item")
            Else
                dynamiccell_1.Value = ""
            End If
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'Note
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 4
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Description")) Then
                dynamiccell_1.Value = row("Description")
            Else
                dynamiccell_1.Value = ""
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            workSheet.Cells.Add(dynamiccell_1)

            'QTY
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 5
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Quantity")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Quantity")).ToString()
            Else
                dynamiccell_1.Value = 0
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            'Rate
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 6
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Rate")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Rate")).ToString()
            Else
                dynamiccell_1.Value = 0
            End If

            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            'Cost
            dynamiccell_1 = New Spire.DataExport.XLS.Cell
            dynamiccell_1.Column = 7
            dynamiccell_1.Row = intRowIndex

            If Not Convert.IsDBNull(row("Cost")) Then
                dynamiccell_1.Value = Convert.ToDouble(row("Cost")).ToString("c")
            Else
                dynamiccell_1.Value = Convert.ToDouble(0).ToString("c")
            End If
            dynamiccell_1.CellType = Spire.DataExport.XLS.CellType.String
            dynamiccell_1.NumericFormat = "#,###,##0.00"
            workSheet.Cells.Add(dynamiccell_1)

            intRowIndex = intRowIndex + 1
        Next
        Dim Range2 As Integer = intRowIndex - 1

        intFootTotalStartRow = intRowIndex
        intFootTotalStartColum = 6

        'heading
        Dim mcell1_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_5.Column = intFootTotalStartColum
        mcell1_5.Row = intFootTotalStartRow + 2
        mcell1_5.Value = "Subtotal (excl. GST)"
        mcell1_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_5)

        Dim mcell2_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_5.Column = intFootTotalStartColum
        mcell2_5.Row = intFootTotalStartRow + 3
        mcell2_5.Value = "Add 15% GST"
        mcell2_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_5)

        Dim mcell3_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_5.Column = intFootTotalStartColum
        mcell3_5.Row = intFootTotalStartRow + 4
        mcell3_5.Value = "Total Incl. GST"
        mcell3_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_5)

        Dim mcell4_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_5.Column = intFootTotalStartColum
        mcell4_5.Row = intFootTotalStartRow + 6
        mcell4_5.Value = "Grand Total with Scope Changes Excl. GST"
        mcell4_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_5)

        Dim mcell5_5 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_5.Column = intFootTotalStartColum
        mcell5_5.Row = intFootTotalStartRow + 7
        mcell5_5.Value = "Grand Total with Scope Changes Incl. GST"
        mcell5_5.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_5)

        'value 1
        'Dim CellFomat1_6 As New Spire.DataExport.XLS.CellFormat
        'CellFomat1_6.Font.Bold = True

        'value 2
        Dim mcell1_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell1_7.Column = intFootTotalStartColum + 1
        mcell1_7.Row = intFootTotalStartRow + 2
        mcell1_7.Value = ApprovedexcludeGstCost
        mcell1_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell1_7)

        Dim mcell2_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell2_7.Column = intFootTotalStartColum + 1
        mcell2_7.Row = intFootTotalStartRow + 3
        mcell2_7.Value = ApprovedGSTCost
        mcell2_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell2_7)

        Dim mcell3_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell3_7.Column = intFootTotalStartColum + 1
        mcell3_7.Row = intFootTotalStartRow + 4
        mcell3_7.Value = ApprovedInGSTCost
        mcell3_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell3_7)

        Dim mcell4_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell4_7.Column = intFootTotalStartColum + 1
        mcell4_7.Row = intFootTotalStartRow + 6
        mcell4_7.Value = GrandExGSTCost
        mcell4_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell4_7)

        Dim mcell5_7 As Spire.DataExport.XLS.Cell = New Spire.DataExport.XLS.Cell
        mcell5_7.Column = intFootTotalStartColum + 1
        mcell5_7.Row = intFootTotalStartRow + 7
        mcell5_7.Value = GrandInGSTCost
        mcell5_7.CellType = Spire.DataExport.XLS.CellType.String
        workSheet.Cells.Add(mcell5_7)

        '''''''''''''''''
        'oleDbConnection.Open()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Dim InitialDataTable As New DataTable
        'connect to dataset
        workSheet.DataSource = Spire.DataExport.Common.ExportSource.DataTable
        'workSheet.DataTable = dsQuery.Tables(0)
        workSheet.DataTable = InitialDataTable
        'workSheet.SQLCommand = oleDbCommand
        workSheet.StartDataCol = CByte(1)
        cellExport.Sheets.Add(workSheet)
        Try
            cellExport.SaveToFile()
            result = strExcelFile
        Finally
            'oleDbConnection.Close()
        End Try
        
        Return result
    End Function
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class