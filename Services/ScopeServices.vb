Imports System.Data.SqlClient
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.Shared.Utilities
Namespace Warpfusion.A4PP.Services
    Public Class ScopeServices
        Private m_SQLConn As New SQLConn
        Private m_SqlConnection As SqlConnection = m_SQLConn.conn()
        Private m_SqlCommand As SqlCommand
        Private m_SqlDataAdapter As SqlDataAdapter
        Private m_SQL As String
        Private m_SQLConnectionString As String = String.Empty

        Public WriteOnly Property SetSQLConn() As SQLConn
            Set(ByVal value As SQLConn)
                m_SQLConn = value
            End Set
        End Property

        Public Property SQLConnection() As String
            Get
                Return m_SQLConnectionString
            End Get
            Set(ByVal value As String)
                m_SQLConnectionString = value
                m_SQLConn.Connection = m_SQLConnectionString
            End Set
        End Property

        Public Function CreateScope(ByVal Scope As Scope) As Long
            Dim result As Long = 0
            Dim dsScope As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateScope"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Scope.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", Scope.ProjectId)
            End If
            If Scope.GSTRate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GSTRate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GSTRate", Scope.GSTRate)
            End If
            If Scope.Cost = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Cost", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Cost", Scope.Cost)
            End If
            If Scope.GST = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GST", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GST", Scope.GST)
            End If
            If Scope.Total = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Total", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Total", Scope.Total)
            End If
            If Scope.Cost1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Cost1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Cost1", Scope.Cost1)
            End If
            If Scope.GST1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GST1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GST1", Scope.GST1)
            End If
            If Scope.Total1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Total1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Total1", Scope.Total1)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsScope, "Scope")
            conn.Close()
            If dsScope.Tables.Count > 0 Then
                If dsScope.Tables(0).Rows.Count > 0 Then
                    result = dsScope.Tables(0).Rows(0)("ScopeId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateScope(ByVal Scope As Scope)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateScope"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", Scope.ScopeId)
            If Scope.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", Scope.ProjectId)
            End If
            If Scope.GSTRate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GSTRate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GSTRate", Scope.GSTRate)
            End If
            If Scope.Cost = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Cost", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Cost", Scope.Cost)
            End If
            If Scope.GST = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GST", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GST", Scope.GST)
            End If
            If Scope.Total = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Total", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Total", Scope.Total)
            End If
            If Scope.Cost1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Cost1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Cost1", Scope.Cost1)
            End If
            If Scope.GST1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GST1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GST1", Scope.GST1)
            End If
            If Scope.Total1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Total1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Total1", Scope.Total1)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetScopeByProjectId(ByVal ProjectId As Long) As Scope
            Dim result As Scope = New Scope()
            Dim rs_Scope As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            rs_Scope = m_SqlCommand.ExecuteReader
            If rs_Scope.HasRows Then
                rs_Scope.Read()
                If rs_Scope("ScopeId") Is DBNull.Value Then
                    result.ScopeId = 0
                Else
                    result.ScopeId = rs_Scope("ScopeId")
                End If
                If rs_Scope("ProjectId") Is DBNull.Value Then
                    result.ProjectId = 0
                Else
                    result.ProjectId = rs_Scope("ProjectId")
                End If
                If rs_Scope("GSTRate") Is DBNull.Value Then
                    result.GSTRate = 0
                Else
                    result.GSTRate = rs_Scope("GSTRate")
                End If
                If rs_Scope("Cost") Is DBNull.Value Then
                    result.Cost = 0
                Else
                    result.Cost = rs_Scope("Cost")
                End If
                If rs_Scope("GST") Is DBNull.Value Then
                    result.GST = 0
                Else
                    result.GST = rs_Scope("GST")
                End If
                If rs_Scope("Total") Is DBNull.Value Then
                    result.Total = 0
                Else
                    result.Total = rs_Scope("Total")
                End If
                If rs_Scope("Cost1") Is DBNull.Value Then
                    result.Cost1 = 0
                Else
                    result.Cost1 = rs_Scope("Cost1")
                End If
                If rs_Scope("GST1") Is DBNull.Value Then
                    result.GST1 = 0
                Else
                    result.GST1 = rs_Scope("GST1")
                End If
                If rs_Scope("Total1") Is DBNull.Value Then
                    result.Total1 = 0
                Else
                    result.Total1 = rs_Scope("Total1")
                End If
                If rs_Scope("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_Scope("CreatedDate")
                End If
                If rs_Scope("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_Scope("ModifiedDate")
                End If
            End If
            rs_Scope.Close()
            rs_Scope = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetScopeByScopeId(ByVal ScopeId As Long) As Scope
            Dim result As Scope = New Scope()
            Dim rs_Scope As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeByScopeId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)
            conn.Open()
            rs_Scope = m_SqlCommand.ExecuteReader
            If rs_Scope.HasRows Then
                rs_Scope.Read()
                If rs_Scope("ScopeId") Is DBNull.Value Then
                    result.ScopeId = 0
                Else
                    result.ScopeId = rs_Scope("ScopeId")
                End If
                If rs_Scope("ProjectId") Is DBNull.Value Then
                    result.ProjectId = 0
                Else
                    result.ProjectId = rs_Scope("ProjectId")
                End If
                If rs_Scope("GSTRate") Is DBNull.Value Then
                    result.GSTRate = 0
                Else
                    result.GSTRate = rs_Scope("GSTRate")
                End If
                If rs_Scope("Cost") Is DBNull.Value Then
                    result.Cost = 0
                Else
                    result.Cost = rs_Scope("Cost")
                End If
                If rs_Scope("GST") Is DBNull.Value Then
                    result.GST = 0
                Else
                    result.GST = rs_Scope("GST")
                End If
                If rs_Scope("Total") Is DBNull.Value Then
                    result.Total = 0
                Else
                    result.Total = rs_Scope("Total")
                End If
                If rs_Scope("Cost1") Is DBNull.Value Then
                    result.Cost1 = 0
                Else
                    result.Cost1 = rs_Scope("Cost1")
                End If
                If rs_Scope("GST1") Is DBNull.Value Then
                    result.GST1 = 0
                Else
                    result.GST1 = rs_Scope("GST1")
                End If
                If rs_Scope("Total1") Is DBNull.Value Then
                    result.Total1 = 0
                Else
                    result.Total1 = rs_Scope("Total1")
                End If
                If rs_Scope("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_Scope("CreatedDate")
                End If
                If rs_Scope("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_Scope("ModifiedDate")
                End If
            End If
            rs_Scope.Close()
            rs_Scope = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetScopeItemStatuses() As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeItemStatuses"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ScopeItemStatuses")
            conn.Close()
            Return result
        End Function

        Public Function CreateScopeItem(ByVal ScopeItem As ScopeItem) As Long
            Dim result As Long = 0
            Dim dsScopeItem As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateScopeItem"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If ScopeItem.ScopeId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ScopeId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeItem.ScopeId)
            End If
            If ScopeItem.ScopeGroup = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ScopeGroup", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ScopeGroup", ScopeItem.ScopeGroup)
            End If
            If ScopeItem.ScopeGroupId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ScopeGroupId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ScopeGroupId", ScopeItem.ScopeGroupId)
            End If
            If ScopeItem.WorksheetService = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorksheetService", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorksheetService", ScopeItem.WorksheetService)
            End If
            If ScopeItem.Area = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Area", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Area", ScopeItem.Area)
            End If
            If ScopeItem.Item = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Item", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Item", ScopeItem.Item)
            End If
            If ScopeItem.Service = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Service", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Service", ScopeItem.Service)
            End If
            If ScopeItem.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", ScopeItem.Description)
            End If
            'If ScopeItem.ScopeItemStatusId = Nothing Then
            '    m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatusId", DBNull.Value)
            'Else
            '    m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatusId", ScopeItem.ScopeItemStatusId)
            'End If
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatusId", ScopeItem.ScopeItemStatusId)
            If ScopeItem.AssignTo = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AssignTo", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AssignTo", ScopeItem.AssignTo)
            End If
            If ScopeItem.AssignToId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AssignToId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AssignToId", ScopeItem.AssignToId)
            End If
            If ScopeItem.Rate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Rate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Rate", ScopeItem.Rate)
            End If
            If ScopeItem.Unit = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Unit", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Unit", ScopeItem.Unit)
            End If
            If ScopeItem.Quantity = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Quantity", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Quantity", ScopeItem.Quantity)
            End If
            If ScopeItem.Cost = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Cost", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Cost", ScopeItem.Cost)
            End If
            If ScopeItem.ServiceGroupId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ServiceGroupID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ServiceGroupID", ScopeItem.ServiceGroupId)
            End If
            m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", ScopeItem.DisplayOrder)
            If ScopeItem.AreaMeasurement = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AreaMeasurement", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AreaMeasurement", ScopeItem.AreaMeasurement)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsScopeItem, "ScopeItem")
            conn.Close()
            If dsScopeItem.Tables.Count > 0 Then
                If dsScopeItem.Tables(0).Rows.Count > 0 Then
                    result = dsScopeItem.Tables(0).Rows(0)("ScopeItemId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateScopeItem(ByVal ScopeItem As ScopeItem)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateScopeItem"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemId", ScopeItem.ScopeItemId)
            If ScopeItem.ScopeId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ScopeId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeItem.ScopeId)
            End If
            If ScopeItem.ScopeGroup = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ScopeGroup", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ScopeGroup", ScopeItem.ScopeGroup)
            End If
            If ScopeItem.ScopeGroupId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ScopeGroupId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ScopeGroupId", ScopeItem.ScopeGroupId)
            End If
            If ScopeItem.WorksheetService = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorksheetService", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorksheetService", ScopeItem.WorksheetService)
            End If
            If ScopeItem.Area = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Area", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Area", ScopeItem.Area)
            End If
            If ScopeItem.Item = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Item", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Item", ScopeItem.Item)
            End If
            If ScopeItem.Service = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Service", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Service", ScopeItem.Service)
            End If
            If ScopeItem.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", ScopeItem.Description)
            End If
            'If ScopeItem.ScopeItemStatusId = Nothing Then
            '    m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatusId", DBNull.Value)
            'Else
            '    m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatusId", ScopeItem.ScopeItemStatusId)
            'End If
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatusId", ScopeItem.ScopeItemStatusId)
            If ScopeItem.AssignTo = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AssignTo", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AssignTo", ScopeItem.AssignTo)
            End If
            If ScopeItem.AssignToId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AssignToId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AssignToId", ScopeItem.AssignToId)
            End If
            If ScopeItem.Rate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Rate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Rate", ScopeItem.Rate)
            End If
            If ScopeItem.Unit = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Unit", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Unit", ScopeItem.Unit)
            End If
            If ScopeItem.Quantity = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Quantity", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Quantity", ScopeItem.Quantity)
            End If
            If ScopeItem.Cost = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Cost", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Cost", ScopeItem.Cost)
            End If
            If ScopeItem.ServiceGroupId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ServiceGroupID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ServiceGroupID", ScopeItem.ServiceGroupId)
            End If
            m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", ScopeItem.DisplayOrder)
            If ScopeItem.AreaMeasurement = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AreaMeasurement", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AreaMeasurement", ScopeItem.AreaMeasurement)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetScopeItemByScopeItemId(ByVal ScopeItemId As Long) As ScopeItem
            Dim result As ScopeItem = New ScopeItem()
            Dim rs_ScopeItem As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeItemByScopeItemId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemId", ScopeItemId)
            conn.Open()
            rs_ScopeItem = m_SqlCommand.ExecuteReader
            If rs_ScopeItem.HasRows Then
                rs_ScopeItem.Read()
                If rs_ScopeItem("ScopeItemId") Is DBNull.Value Then
                    result.ScopeItemId = 0
                Else
                    result.ScopeItemId = rs_ScopeItem("ScopeItemId")
                End If
                If rs_ScopeItem("ScopeId") Is DBNull.Value Then
                    result.ScopeId = 0
                Else
                    result.ScopeId = rs_ScopeItem("ScopeId")
                End If
                If rs_ScopeItem("Area") Is DBNull.Value Then
                    result.Area = Nothing
                Else
                    result.Area = rs_ScopeItem("Area")
                End If
                If rs_ScopeItem("AreaMeasurement") Is DBNull.Value Then
                    result.AreaMeasurement = Nothing
                Else
                    result.AreaMeasurement = rs_ScopeItem("AreaMeasurement")
                End If
                If rs_ScopeItem("Item") Is DBNull.Value Then
                    result.Item = Nothing
                Else
                    result.Item = rs_ScopeItem("Item")
                End If
                If rs_ScopeItem("Service") Is DBNull.Value Then
                    result.Service = Nothing
                Else
                    result.Service = rs_ScopeItem("Service")
                End If
                If rs_ScopeItem("Description") Is DBNull.Value Then
                    result.Description = Nothing
                Else
                    result.Description = rs_ScopeItem("Description")
                End If
                If rs_ScopeItem("ScopeItemStatusId") Is DBNull.Value Then
                    result.ScopeItemStatusId = 0
                Else
                    result.ScopeItemStatusId = rs_ScopeItem("ScopeItemStatusId")
                End If
                If rs_ScopeItem("AssignTo") Is DBNull.Value Then
                    result.AssignTo = Nothing
                Else
                    result.AssignTo = rs_ScopeItem("AssignTo")
                End If
                If rs_ScopeItem("AssignToId") Is DBNull.Value Then
                    result.AssignToId = 0
                Else
                    result.AssignToId = rs_ScopeItem("AssignToId")
                End If
                If rs_ScopeItem("Rate") Is DBNull.Value Then
                    result.Rate = 0
                Else
                    result.Rate = rs_ScopeItem("Rate")
                End If
                If rs_ScopeItem("Unit") Is DBNull.Value Then
                    result.Unit = Nothing
                Else
                    result.Unit = rs_ScopeItem("Unit")
                End If
                If rs_ScopeItem("Quantity") Is DBNull.Value Then
                    result.Quantity = 0
                Else
                    result.Quantity = rs_ScopeItem("Quantity")
                End If
                If rs_ScopeItem("Cost") Is DBNull.Value Then
                    result.Cost = 0
                Else
                    result.Cost = rs_ScopeItem("Cost")
                End If
                If rs_ScopeItem("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_ScopeItem("CreatedDate")
                End If
                If rs_ScopeItem("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_ScopeItem("ModifiedDate")
                End If
                If rs_ScopeItem("ProjectId") Is DBNull.Value Then
                    result.ProjectId = 0
                Else
                    result.ProjectId = rs_ScopeItem("ProjectId")
                End If
                If rs_ScopeItem("ServiceGroupId") Is DBNull.Value Then
                    result.ServiceGroupId = 0
                Else
                    result.ServiceGroupId = rs_ScopeItem("ServiceGroupId")
                End If
                If rs_ScopeItem("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_ScopeItem("DisplayOrder")
                End If
                If rs_ScopeItem("ScopeGroupId") Is DBNull.Value Then
                    result.ScopeGroupId = 0
                Else
                    result.ScopeGroupId = rs_ScopeItem("ScopeGroupId")
                End If
                If rs_ScopeItem("ScopeGroup") Is DBNull.Value Then
                    result.ScopeGroup = Nothing
                Else
                    result.ScopeGroup = rs_ScopeItem("ScopeGroup")
                End If
                If rs_ScopeItem("WorksheetService") Is DBNull.Value Then
                    result.WorksheetService = Nothing
                Else
                    result.WorksheetService = rs_ScopeItem("WorksheetService")
                End If
            End If
            rs_ScopeItem.Close()
            rs_ScopeItem = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetScopeItemsByScopeId(ByVal ScopeId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeItemsByScopeId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ScopeItemsByScopeId")
            conn.Close()
            Return result
        End Function

        Public Function GetScopeItemsByScopeIdScopeItemStatus(ByVal ScopeId As Long, ByVal ScopeItemStatus As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeItemsByScopeIdScopeItemStatus"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatus", ScopeItemStatus)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ScopeItemsByScopeIdScopeItemStatus")
            conn.Close()
            Return result
        End Function

        Public Function GetScopeItemsByScopeIdScopeItemStatusUserId(ByVal ScopeId As Long, ByVal ScopeItemStatus As Integer, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeItemsByScopeIdScopeItemStatusUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatus", ScopeItemStatus)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ScopeItemsByScopeIdScopeItemStatusUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetWorksheetGroupsByScopeIdScopeItemStatusUserId(ByVal ScopeId As Long, ByVal ScopeItemStatus As Integer, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetWorksheetGroupsByScopeIdScopeItemStatusUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatus", ScopeItemStatus)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "WorksheetGroupsByScopeIdScopeItemStatusUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetScopeItemsByScopeIdScopeItemStatusUserIdGroupId(ByVal ScopeId As Long, ByVal ScopeItemStatus As Integer, ByVal UserId As Long, ByVal GroupId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeItemsByScopeIdScopeItemStatusUserIdGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatus", ScopeItemStatus)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@GroupId", GroupId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ScopeItemsByScopeIdScopeItemStatusUserIdGroupId")
            conn.Close()
            Return result
        End Function

        Public Function GetScopeItemsHasDescriptionByScopeIdScopeItemStatusUserId(ByVal ScopeId As Long, ByVal ScopeItemStatus As Integer, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetScopeItemsHasDescriptionByScopeIdScopeItemStatusUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemStatus", ScopeItemStatus)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ScopeItemsHasDescriptionByScopeIdScopeItemStatusUserId")
            conn.Close()
            Return result
        End Function

        Public Sub DeleteScopeItemByScopeItemId(ByVal ScopeItemId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteScopeItemByScopeItemId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemId", ScopeItemId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateArea(ByVal Area As Area) As Long
            Dim result As Long = 0
            Dim dsArea As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateArea"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Area.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Area.ProjectOwnerId)
            End If
            If Area.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Area.Name)
            End If
            If Area.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Area.Description)
            End If
            If Area.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", Area.Disabled)
            End If
            If Area.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Area.DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsArea, "Area")
            conn.Close()
            If dsArea.Tables.Count > 0 Then
                If dsArea.Tables(0).Rows.Count > 0 Then
                    result = dsArea.Tables(0).Rows(0)("AreaId")
                End If
            End If
            Return result
        End Function

        Public Function CreateWorksheetService(ByVal Service As WorksheetService) As Long
            Dim result As Long = 0
            Dim dsService As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateWorksheetService"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Service.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Service.ProjectOwnerId)
            End If
            If Service.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Service.Name)
            End If
            If Service.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Service.Description)
            End If
            If Service.Unit = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Unit", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Unit", Service.Unit)
            End If
            If Service.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", Service.Disabled)
            End If
            If Service.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Service.DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsService, "WorksheetService")
            conn.Close()
            If dsService.Tables.Count > 0 Then
                If dsService.Tables(0).Rows.Count > 0 Then
                    result = dsService.Tables(0).Rows(0)("WorksheetServiceId")
                End If
            End If
            Return result
        End Function

        Public Function CreateWorksheetGroup(ByVal WorksheetGroup As WorksheetGroup) As Long
            Dim result As Long = 0
            Dim dsWorksheetGroup As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateWorksheetGroup"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If WorksheetGroup.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", WorksheetGroup.ProjectOwnerId)
            End If
            If WorksheetGroup.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", WorksheetGroup.Name)
            End If
            If WorksheetGroup.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", WorksheetGroup.Description)
            End If
            If WorksheetGroup.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", WorksheetGroup.Disabled)
            End If
            If WorksheetGroup.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", WorksheetGroup.DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsWorksheetGroup, "WorksheetGroup")
            conn.Close()
            If dsWorksheetGroup.Tables.Count > 0 Then
                If dsWorksheetGroup.Tables(0).Rows.Count > 0 Then
                    result = dsWorksheetGroup.Tables(0).Rows(0)("WorksheetGroupId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateArea(ByVal Area As Area)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateArea"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Area.AreaId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AreaId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AreaId", Area.AreaId)
            End If
            If Area.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Area.ProjectOwnerId)
            End If
            If Area.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Area.Name)
            End If
            If Area.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Area.Description)
            End If
            If Area.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", Area.Disabled)
            End If
            If Area.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Area.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateWorksheetService(ByVal Service As WorksheetService)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateWorksheetService"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Service.WorksheetServiceId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorksheetServiceId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorksheetServiceId", Service.WorksheetServiceId)
            End If
            If Service.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Service.ProjectOwnerId)
            End If
            If Service.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Service.Name)
            End If
            If Service.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Service.Description)
            End If
            If Service.Unit = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Unit", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Unit", Service.Unit)
            End If
            If Service.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", Service.Disabled)
            End If
            If Service.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Service.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateWorksheetGroup(ByVal WorksheetGroup As WorksheetGroup)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateWorksheetGroup"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If WorksheetGroup.WorksheetGroupId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorksheetGroupId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorksheetGroupId", WorksheetGroup.WorksheetGroupId)
            End If
            If WorksheetGroup.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", WorksheetGroup.ProjectOwnerId)
            End If
            If WorksheetGroup.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", WorksheetGroup.Name)
            End If
            If WorksheetGroup.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", WorksheetGroup.Description)
            End If
            If WorksheetGroup.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", WorksheetGroup.Disabled)
            End If
            If WorksheetGroup.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", WorksheetGroup.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetAreasByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetAreasByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "AreasByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetWorksheetServicesByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetWorksheetServicesByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "WorksheetServicesByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetWorksheetGroupsByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetWorksheetGroupsByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "WorksheetGroupsByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectGroupsByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectGroupsByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectGroupsByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetAreaByAreaId(ByVal AreaId As Long) As Area
            Dim result As Area = New Area()
            Dim rs_Area As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetAreaByAreaId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@AreaId", AreaId)
            conn.Open()
            rs_Area = m_SqlCommand.ExecuteReader
            If rs_Area.HasRows Then
                rs_Area.Read()
                If rs_Area("AreaId") Is DBNull.Value Then
                    result.AreaId = 0
                Else
                    result.AreaId = rs_Area("AreaId")
                End If
                If rs_Area("ProjectOwnerId") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_Area("ProjectOwnerId")
                End If
                If rs_Area("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_Area("Name")
                End If
                If rs_Area("Description") Is DBNull.Value Then
                    result.Description = String.Empty
                Else
                    result.Description = rs_Area("Description")
                End If
                If rs_Area("Disabled") Is DBNull.Value Then
                    result.Disabled = False
                Else
                    result.Disabled = rs_Area("Disabled")
                End If
                If rs_Area("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_Area("DisplayOrder")
                End If
            End If
            rs_Area.Close()
            rs_Area = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetWorksheetServiceByWorksheetServiceId(ByVal WorksheetServiceId As Long) As WorksheetService
            Dim result As WorksheetService = New WorksheetService()
            Dim rs_WorksheetService As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetWorksheetServiceByWorksheetServiceId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@WorksheetServiceId", WorksheetServiceId)
            conn.Open()
            rs_WorksheetService = m_SqlCommand.ExecuteReader
            If rs_WorksheetService.HasRows Then
                rs_WorksheetService.Read()
                If rs_WorksheetService("WorksheetServiceId") Is DBNull.Value Then
                    result.WorksheetServiceId = 0
                Else
                    result.WorksheetServiceId = rs_WorksheetService("WorksheetServiceId")
                End If
                If rs_WorksheetService("ProjectOwnerId") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_WorksheetService("ProjectOwnerId")
                End If
                If rs_WorksheetService("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_WorksheetService("Name")
                End If
                If rs_WorksheetService("Description") Is DBNull.Value Then
                    result.Description = String.Empty
                Else
                    result.Description = rs_WorksheetService("Description")
                End If
                If rs_WorksheetService("Unit") Is DBNull.Value Then
                    result.Unit = Nothing
                Else
                    result.Unit = rs_WorksheetService("Unit")
                End If
                If rs_WorksheetService("Disabled") Is DBNull.Value Then
                    result.Disabled = False
                Else
                    result.Disabled = rs_WorksheetService("Disabled")
                End If
                If rs_WorksheetService("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_WorksheetService("DisplayOrder")
                End If
            End If
            rs_WorksheetService.Close()
            rs_WorksheetService = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetWorksheetGroupByWorksheetGroupId(ByVal WorksheetGroupId As Long) As WorksheetGroup
            Dim result As WorksheetGroup = New WorksheetGroup()
            Dim rs_WorksheetGroup As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetWorksheetGroupByWorksheetGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@WorksheetGroupId", WorksheetGroupId)
            conn.Open()
            rs_WorksheetGroup = m_SqlCommand.ExecuteReader
            If rs_WorksheetGroup.HasRows Then
                rs_WorksheetGroup.Read()
                If rs_WorksheetGroup("WorksheetGroupId") Is DBNull.Value Then
                    result.WorksheetGroupId = 0
                Else
                    result.WorksheetGroupId = rs_WorksheetGroup("WorksheetGroupId")
                End If
                If rs_WorksheetGroup("ProjectOwnerId") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_WorksheetGroup("ProjectOwnerId")
                End If
                If rs_WorksheetGroup("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_WorksheetGroup("Name")
                End If
                If rs_WorksheetGroup("Description") Is DBNull.Value Then
                    result.Description = String.Empty
                Else
                    result.Description = rs_WorksheetGroup("Description")
                End If
                If rs_WorksheetGroup("Disabled") Is DBNull.Value Then
                    result.Disabled = False
                Else
                    result.Disabled = rs_WorksheetGroup("Disabled")
                End If
                If rs_WorksheetGroup("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_WorksheetGroup("DisplayOrder")
                End If
            End If
            rs_WorksheetGroup.Close()
            rs_WorksheetGroup = Nothing
            conn.Close()

            Return result
        End Function

        Public Sub DeleteAreaByAreaId(ByVal AreaId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteAreaByAreaId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@AreaId", AreaId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteWorksheetServiceByWorksheetServiceId(ByVal WorksheetServiceId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteWorksheetServiceByWorksheetServiceId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@WorksheetServiceId", WorksheetServiceId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteWorksheetGroupByWorksheetGroupId(ByVal WorksheetGroupId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteWorksheetGroupByWorksheetGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@WorksheetGroupId", WorksheetGroupId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateItem(ByVal Item As Item) As Long
            Dim result As Long = 0
            Dim dsItem As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateItem"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Item.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Item.ProjectOwnerId)
            End If
            If Item.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Item.Name)
            End If
            If Item.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Item.Description)
            End If
            If Item.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", Item.Disabled)
            End If
            If Item.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Item.DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsItem, "Item")
            conn.Close()
            If dsItem.Tables.Count > 0 Then
                If dsItem.Tables(0).Rows.Count > 0 Then
                    result = dsItem.Tables(0).Rows(0)("ItemId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateItem(ByVal Item As Item)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateItem"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Item.ItemId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ItemId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ItemId", Item.ItemId)
            End If
            If Item.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Item.ProjectOwnerId)
            End If
            If Item.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Item.Name)
            End If
            If Item.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Item.Description)
            End If
            If Item.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", Item.Disabled)
            End If
            If Item.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Item.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetItemsByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetItemsByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ItemsByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetItemByItemId(ByVal ItemId As Long) As Item
            Dim result As Item = New Item()
            Dim rs_Item As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetItemByItemId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ItemId", ItemId)
            conn.Open()
            rs_Item = m_SqlCommand.ExecuteReader
            If rs_Item.HasRows Then
                rs_Item.Read()
                If rs_Item("ItemId") Is DBNull.Value Then
                    result.ItemId = 0
                Else
                    result.ItemId = rs_Item("ItemId")
                End If
                If rs_Item("ProjectOwnerId") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_Item("ProjectOwnerId")
                End If
                If rs_Item("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_Item("Name")
                End If
                If rs_Item("Description") Is DBNull.Value Then
                    result.Description = String.Empty
                Else
                    result.Description = rs_Item("Description")
                End If
                If rs_Item("Disabled") Is DBNull.Value Then
                    result.Disabled = False
                Else
                    result.Disabled = rs_Item("Disabled")
                End If
                If rs_Item("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_Item("DisplayOrder")
                End If
            End If
            rs_Item.Close()
            rs_Item = Nothing
            conn.Close()

            Return result
        End Function

        Public Sub DeleteItemByItemId(ByVal ItemId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteItemByItemId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ItemId", ItemId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateService(ByVal Service As Service) As Long
            Dim result As Long = 0
            Dim dsService As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateService"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Service.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Service.ProjectOwnerId)
            End If
            If Service.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", Service.UserId)
            End If
            If Service.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Service.Name)
            End If
            If Service.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Service.Description)
            End If
            If Service.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", Service.Disabled)
            End If
            If Service.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Service.DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsService, "Service")
            conn.Close()
            If dsService.Tables.Count > 0 Then
                If dsService.Tables(0).Rows.Count > 0 Then
                    result = dsService.Tables(0).Rows(0)("ServiceId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateService(ByVal Service As Service)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateService"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceId", Service.ServiceId)
            If Service.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Service.ProjectOwnerId)
            End If
            If Service.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", Service.UserId)
            End If
            If Service.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Service.Name)
            End If
            If Service.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Service.Description)
            End If
            If Service.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", Service.Disabled)
            End If
            If Service.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Service.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetServicesByProjectOwnerIdUserId(ByVal ProjectOwnerId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServicesByProjectOwnerIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServicesByProjectOwnerIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetServiceByServiceId(ByVal ServiceId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceByServiceId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceByServiceId")
            conn.Close()
            Return result
        End Function

        Public Function GetAllServicesByProjectOwnerIdUserId(ByVal ProjectOwnerId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetAllServicesByProjectOwnerIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "AllServicesByProjectOwnerIdUserId")
            conn.Close()
            Return result
        End Function

        Public Sub DeleteServiceByServiceId(ByVal ServiceId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteServiceByServiceId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateServiceRate(ByVal ServiceRate As ServiceRate) As Long
            Dim result As Long = 0
            Dim dsServiceRate As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateServiceRate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If ServiceRate.ServiceId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ServiceId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceRate.ServiceId)
            End If
            If ServiceRate.CostRate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@CostRate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@CostRate", ServiceRate.CostRate)
            End If
            If ServiceRate.ChargeRate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ChargeRate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ChargeRate", ServiceRate.ChargeRate)
            End If
            If ServiceRate.Unit = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Unit", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Unit", ServiceRate.Unit)
            End If
            If ServiceRate.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", ServiceRate.Disabled)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsServiceRate, "ServiceRate")
            conn.Close()
            If dsServiceRate.Tables.Count > 0 Then
                If dsServiceRate.Tables(0).Rows.Count > 0 Then
                    result = dsServiceRate.Tables(0).Rows(0)("ServiceRateId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateServiceRate(ByVal ServiceRate As ServiceRate)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateServiceRate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceRateId", ServiceRate.ServiceRateId)
            If ServiceRate.ServiceId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ServiceId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceRate.ServiceId)
            End If
            If ServiceRate.CostRate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@CostRate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@CostRate", ServiceRate.CostRate)
            End If
            If ServiceRate.ChargeRate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ChargeRate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ChargeRate", ServiceRate.ChargeRate)
            End If
            If ServiceRate.Unit = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Unit", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Unit", ServiceRate.Unit)
            End If
            If ServiceRate.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", ServiceRate.Disabled)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetServiceRatesByProjectOwnerIdUserId(ByVal ProjectOwnerId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceRatesByProjectOwnerIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceRatesByProjectOwnerIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetAllServiceRatesByProjectOwnerIdUserId(ByVal ProjectOwnerId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetAllServiceRatesByProjectOwnerIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "AllServiceRatesByProjectOwnerIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetAllServiceRatesByServiceGroupId(ByVal ServiceGroupId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetAllServiceRatesByServiceGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "AllServiceRatesByServiceGroupId")
            conn.Close()
            Return result
        End Function

        Public Function GetServiceRatesByServiceId(ByVal ServiceId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceRatesByServiceId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceRatesByServiceId")
            conn.Close()
            Return result
        End Function

        Public Function GetAllServiceRatesByServiceId(ByVal ServiceId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetAllServiceRatesByServiceId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "AllServiceRatesByServiceId")
            conn.Close()
            Return result
        End Function

        Public Function GetServiceRateByServiceRateId(ByVal ServiceRateId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceRateByServicerateId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceRateId", ServiceRateId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceRateByServiceRateId")
            conn.Close()
            Return result
        End Function

        Public Sub DeleteServiceRateByServiceRateId(ByVal ServiceRateId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteServiceRateByServiceRateId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceRateId", ServiceRateId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUnits() As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUnits"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "Units")
            conn.Close()
            Return result
        End Function

        Public Sub UpdateScopeItemApproveByScopeId(ByVal ScopeId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateScopeItemApproveByScopeId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)            
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateScopeItemDisapproveByScopeId(ByVal ScopeId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateScopeItemDisapproveByScopeId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeId", ScopeId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateScopeItemApproveByScopeItemId(ByVal ScopeItemId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateScopeItemApproveByScopeItemId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemId", ScopeItemId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateScopeItemDisapproveByScopeItemId(ByVal ScopeItemId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateScopeItemDisapproveByScopeItemId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ScopeItemId", ScopeItemId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateTotal(ByVal ScopeId As Long)
            Dim Scope As Scope = New Scope()
            Scope = GetScopeByScopeId(ScopeId)
            Dim dsScopeItem As DataSet
            Dim index As Integer
            'Pending
            Dim CostPending As Decimal = 0
            dsScopeItem = GetScopeItemsByScopeIdScopeItemStatus(ScopeId, 1)
            If dsScopeItem.Tables.Count > 0 Then
                If dsScopeItem.Tables(0).Rows.Count > 0 Then
                    For index = 0 To dsScopeItem.Tables(0).Rows.Count - 1
                        If Not IsDBNull(dsScopeItem.Tables(0).Rows(index)("Cost")) Then
                            CostPending = CostPending + dsScopeItem.Tables(0).Rows(index)("Cost")
                        End If
                    Next
                End If
            End If
            Scope.Cost1 = CostPending
            Scope.GST1 = Scope.Cost1 * Scope.GSTRate
            If Scope.GSTRate > 1 Then
                Scope.GST1 = Scope.GST1 / 100
            End If
            Scope.Total1 = Scope.Cost1 + Scope.GST1

            'Approved
            Dim CostApproved As Decimal = 0
            dsScopeItem = GetScopeItemsByScopeIdScopeItemStatus(ScopeId, 2)
            If dsScopeItem.Tables.Count > 0 Then
                If dsScopeItem.Tables(0).Rows.Count > 0 Then
                    For index = 0 To dsScopeItem.Tables(0).Rows.Count - 1
                        If Not IsDBNull(dsScopeItem.Tables(0).Rows(index)("Cost")) Then
                            CostApproved = CostApproved + dsScopeItem.Tables(0).Rows(index)("Cost")
                        End If
                    Next
                End If
            End If
            Scope.Cost = CostApproved
            Scope.GST = Scope.Cost * Scope.GSTRate
            If Scope.GSTRate > 1 Then
                Scope.GST = Scope.GST / 100
            End If
            Scope.Total = Scope.Cost + Scope.GST

            UpdateScope(Scope)
        End Sub

        Public Function CreateServiceGroup(ByVal ServiceGroup As ServiceGroup) As Long
            Dim result As Long = 0
            Dim dsServiceGroup As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateServiceGroup"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If ServiceGroup.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", ServiceGroup.UserId)
            End If
            If ServiceGroup.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", ServiceGroup.Name)
            End If
            If ServiceGroup.IsPrivate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@IsPrivate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@IsPrivate", ServiceGroup.IsPrivate)
            End If
            If ServiceGroup.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", 0)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", ServiceGroup.DisplayOrder)
            End If

            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsServiceGroup, "ServiceGroup")
            conn.Close()
            If dsServiceGroup.Tables.Count > 0 Then
                If dsServiceGroup.Tables(0).Rows.Count > 0 Then
                    result = dsServiceGroup.Tables(0).Rows(0)("ServiceGroupId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateServiceGroup(ByVal ServiceGroup As ServiceGroup)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateServiceGroup"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroup.ServiceGroupId)
            If ServiceGroup.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", ServiceGroup.UserId)
            End If
            If ServiceGroup.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", ServiceGroup.Name)
            End If
            If ServiceGroup.IsPrivate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@IsPrivate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@IsPrivate", ServiceGroup.IsPrivate)
            End If
            If ServiceGroup.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", 0)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", ServiceGroup.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetServiceGroupByServiceGroupId(ByVal ServiceGroupId As Long) As ServiceGroup
            Dim result As ServiceGroup = New ServiceGroup()
            Dim rs_ServiceGroup As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceGroupByServiceGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            conn.Open()
            rs_ServiceGroup = m_SqlCommand.ExecuteReader
            If rs_ServiceGroup.HasRows Then
                rs_ServiceGroup.Read()
                If rs_ServiceGroup("ServiceGroupId") Is DBNull.Value Then
                    result.ServiceGroupId = 0
                Else
                    result.ServiceGroupId = rs_ServiceGroup("ServiceGroupId")
                End If
                If rs_ServiceGroup("UserId") Is DBNull.Value Then
                    result.UserId = 0
                Else
                    result.UserId = rs_ServiceGroup("UserId")
                End If
                If rs_ServiceGroup("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_ServiceGroup("Name")
                End If
                If rs_ServiceGroup("IsPrivate") Is DBNull.Value Then
                    result.IsPrivate = 0
                Else
                    result.IsPrivate = rs_ServiceGroup("IsPrivate")
                End If
                If rs_ServiceGroup("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_ServiceGroup("DisplayOrder")
                End If
            End If
            rs_ServiceGroup.Close()
            rs_ServiceGroup = Nothing
            conn.Close()

            Return result
        End Function

        Public Sub DeleteServiceGroupByServiceGroupId(ByVal ServiceGroupId As Long, ByVal IsPhysicalDelete As Boolean)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteServiceGroupByServiceGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            m_SqlCommand.Parameters.AddWithValue("@IsPhysicalDelete", IsPhysicalDelete)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateServiceGroupRelationship(ByVal ServiceGroupId As Long, ByVal ProjectOwnerId As Long) As Long
            Dim result As Long = 0
            Dim dsServiceGroup As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateServiceGroupRelationship"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)

            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsServiceGroup, "ServiceGroupRelationship")
            conn.Close()
            If dsServiceGroup.Tables.Count > 0 Then
                If dsServiceGroup.Tables(0).Rows.Count > 0 Then
                    result = dsServiceGroup.Tables(0).Rows(0)("ServiceGroupRelationshipId")
                End If
            End If
            Return result
        End Function

        Public Sub DeleteServiceGroupRelationshipByServiceGroupIdProjectOwnerId(ByVal ServiceGroupId As Long, ByVal ProjectOwnerId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteServiceGroupRelationshipByServiceGroupIdProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateServiceGroupItem(ByVal ServiceGroupId As Long, ByVal ServiceId As Long) As Long
            Dim result As Long = 0
            Dim dsServiceGroupItem As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateServiceGroupItem"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceId)

            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsServiceGroupItem, "ServiceGroupItem")
            conn.Close()
            If dsServiceGroupItem.Tables.Count > 0 Then
                If dsServiceGroupItem.Tables(0).Rows.Count > 0 Then
                    result = dsServiceGroupItem.Tables(0).Rows(0)("ServiceGroupItemId")
                End If
            End If
            Return result
        End Function

        Public Sub DeleteServiceGroupItemByServiceGroupIdServiceId(ByVal ServiceGroupId As Long, ByVal ServiceId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteServiceGroupItemByServiceGroupIdServiceId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetServiceGroupsByUserId(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceGroupsByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)            
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceGroupsByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetServiceGroupsByUserIdProjectOwnerId(ByVal UserId As Long, ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceGroupsByUserIdProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceGroupsByUserIdProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(ByVal UserId As Long, ByVal ProjectOwnerId As Long, ByVal LoginUserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceGroupsByUserIdProjectOwnerIdLoginUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@LoginUserId", LoginUserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceGroupsByUserIdProjectOwnerIdLoginUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetServicesByServiceGroupId(ByVal ServiceGroupId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServicesByServiceGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServicesByServiceGroupId")
            conn.Close()
            Return result
        End Function

        Public Function GetServiceGroupItemByServiceGroupIdServiceId(ByVal ServiceGroupId As Long, ByVal ServiceId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceGroupItemByServiceGroupIdServiceId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            m_SqlCommand.Parameters.AddWithValue("@ServiceId", ServiceId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceGroupItemByServiceGroupIdServiceId")
            conn.Close()
            Return result
        End Function

        Public Function GetServiceGroupRelationshipByServiceGroupIdProjectOwnerId(ByVal ServiceGroupId As Long, ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetServiceGroupRelationshipByServiceGroupIdProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ServiceGroupId", ServiceGroupId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ServiceGroupRelationshipByServiceGroupIdProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectOwnersAssignableByUserId(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectOwnersAssignableByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectOwnersAssignableByUserId")
            conn.Close()
            Return result
        End Function
    End Class
End Namespace
