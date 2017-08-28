Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Collections
Imports System.Collections.Generic
Imports AjaxControlToolkit

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class AddressService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function GetCountries(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim m_ConnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("AddressService").ConnectionString
        Dim m_SqlConn As New SqlConnection(m_ConnString)
        Dim m_Result As New DataSet
        m_SqlConn.Open()
        Try
            Dim m_SqlRCommand As SqlCommand = New SqlCommand("Select CountryId, CountryName as Country From tbl_Countrys Order By CountryName", m_SqlConn)
            Dim m_SqlRDataAdapter As SqlDataAdapter = New SqlDataAdapter(m_SqlRCommand)
            m_SqlRDataAdapter.Fill(m_Result, "Country")
        Catch ex As Exception
            Throw
        Finally
            m_SqlConn.Close()
        End Try
        Dim values As New List(Of CascadingDropDownNameValue)
        If m_Result.Tables.Count > 0 And m_Result.Tables(0).Rows.Count > 0 Then
            For Each rr As DataRow In m_Result.Tables(0).Rows
                Dim rid As Integer = rr("CountryId")
                Dim rn As String = rr("Country")
                values.Add(New CascadingDropDownNameValue(rn, Convert.ToString(rid)))
            Next
        End If
        Return values.ToArray()
    End Function

    <WebMethod()> _
    Public Function GetSelectedCountries(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim m_ConnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("AddressService").ConnectionString
        Dim m_SqlConn As New SqlConnection(m_ConnString)
        Dim m_Result As New DataSet
        m_SqlConn.Open()
        Try
            Dim m_SqlRCommand As SqlCommand = New SqlCommand("Select CountryId, CountryName as Country From tbl_Countrys where CountryId in (36, 554) Order By CountryName", m_SqlConn)
            Dim m_SqlRDataAdapter As SqlDataAdapter = New SqlDataAdapter(m_SqlRCommand)
            m_SqlRDataAdapter.Fill(m_Result, "Country")
        Catch ex As Exception
            Throw
        Finally
            m_SqlConn.Close()
        End Try
        Dim values As New List(Of CascadingDropDownNameValue)
        If m_Result.Tables.Count > 0 And m_Result.Tables(0).Rows.Count > 0 Then
            For Each rr As DataRow In m_Result.Tables(0).Rows
                Dim rid As Integer = rr("CountryId")
                Dim rn As String = rr("Country")
                values.Add(New CascadingDropDownNameValue(rn, Convert.ToString(rid)))
            Next
        End If
        Return values.ToArray()
    End Function

    <WebMethod()> _
    Public Function GetRegions(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim kv As StringDictionary = New StringDictionary
        kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim cid As Integer
        If (Not kv.ContainsKey("Country") Or Not Int32.TryParse(kv("Country"), cid)) Then
            Return Nothing
        End If

        Dim m_ConnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("AddressService").ConnectionString
        Dim m_SqlConn As New SqlConnection(m_ConnString)
        Dim m_Result As New DataSet
        m_SqlConn.Open()
        Try
            Dim m_SqlRCommand As SqlCommand = New SqlCommand("Select RegionId,DisplayName as Region From tbl_Region Where CountryID='" & cid & "' Order By DisplayName", m_SqlConn)
            Dim m_SqlRDataAdapter As SqlDataAdapter = New SqlDataAdapter(m_SqlRCommand)
            m_SqlRDataAdapter.Fill(m_Result, "Region")
        Catch ex As Exception
            Throw
        Finally
            m_SqlConn.Close()
        End Try
        Dim values As New List(Of CascadingDropDownNameValue)
        If m_Result.Tables.Count > 0 And m_Result.Tables(0).Rows.Count > 0 Then
            For Each rr As DataRow In m_Result.Tables(0).Rows
                Dim rid As Integer = rr("RegionId")
                Dim rn As String = rr("Region")
                values.Add(New CascadingDropDownNameValue(rn, Convert.ToString(rid)))
            Next
        End If
        Return values.ToArray()
    End Function

    <WebMethod()> _
    Public Function GetDistricts(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim kv As StringDictionary = New StringDictionary
        kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim rid As Integer
        If (Not kv.ContainsKey("Region") Or Not Int32.TryParse(kv("Region"), rid)) Then
            Return Nothing
        End If
        'Dim rid As String = kv("Region").ToString
        Dim m_ConnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("AddressService").ConnectionString
        Dim m_SqlConn As New SqlConnection(m_ConnString)
        Dim m_Result As New DataSet
        m_SqlConn.Open()
        Try
            Dim m_SqlRCommand As SqlCommand = New SqlCommand("Select DistrictCityId as DistrictId,DisplayName as District From tbl_DistrictCity Where RegionId='" & Convert.ToString(rid) & "' Order By DisplayName", m_SqlConn)
            Dim m_SqlRDataAdapter As SqlDataAdapter = New SqlDataAdapter(m_SqlRCommand)
            m_SqlRDataAdapter.Fill(m_Result, "District")
        Catch ex As Exception
            Throw
        Finally
            m_SqlConn.Close()
        End Try
        Dim values As New List(Of CascadingDropDownNameValue)
        If m_Result.Tables.Count > 0 And m_Result.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In m_Result.Tables(0).Rows
                Dim did As Integer = dr("DistrictId")
                Dim dn As String = dr("District")
                values.Add(New CascadingDropDownNameValue(dn, Convert.ToString(did)))
            Next
        End If
        Return values.ToArray()
    End Function
    <WebMethod()> _
    Public Function GetTowns(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim kv As StringDictionary = New StringDictionary
        kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
        Dim rid As Integer
        If (Not kv.ContainsKey("District") Or Not Int32.TryParse(kv("District"), rid)) Then
            Return Nothing
        End If
        'Dim rid As String = kv("Region").ToString
        Dim m_ConnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("AddressService").ConnectionString
        Dim m_SqlConn As New SqlConnection(m_ConnString)
        Dim m_Result As New DataSet
        m_SqlConn.Open()
        Try
            Dim m_SqlRCommand As SqlCommand = New SqlCommand("Select TownSuburbId as TownId,DisplayName as Town From tbl_TownSuburb Where DistrictCityId='" & Convert.ToString(rid) & "' Order By DisplayName", m_SqlConn)
            Dim m_SqlRDataAdapter As SqlDataAdapter = New SqlDataAdapter(m_SqlRCommand)
            m_SqlRDataAdapter.Fill(m_Result, "Town")
        Catch ex As Exception
            Throw
        Finally
            m_SqlConn.Close()
        End Try
        Dim values As New List(Of CascadingDropDownNameValue)
        If m_Result.Tables.Count > 0 And m_Result.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In m_Result.Tables(0).Rows
                Dim did As Integer = dr("TownId")
                Dim dn As String = dr("Town")
                values.Add(New CascadingDropDownNameValue(dn, Convert.ToString(did)))
            Next
        End If
        Return values.ToArray()
    End Function

End Class