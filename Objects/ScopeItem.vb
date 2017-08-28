Namespace Warpfusion.A4PP.Objects
    Public Class ScopeItem
        Private m_ScopeItemId As Long
        Private m_ScopeId As Long
        Private m_ScopeGroupId As Long
        Private m_ScopeGroup As String
        Private m_WorksheetService As String
        Private m_Area As String
        Private m_Item As String
        Private m_Service As String
        Private m_Description As String
        Private m_ScopeItemStatusId As Integer
        Private m_AssignTo As String
        Private m_AssignToId As Long
        Private m_Rate As Decimal
        Private m_Unit As String
        Private m_Quantity As Decimal
        Private m_Cost As Decimal
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_ServiceGroupId As Integer
        Private m_DisplayOrder As Integer
        Private m_ProjectId As Long
        Private m_AreaMeasurement As String

        Public Property ScopeItemId() As Long
            Get
                Return m_ScopeItemId
            End Get
            Set(ByVal value As Long)
                m_ScopeItemId = value
            End Set
        End Property
        Public Property ScopeId() As Long
            Get
                Return m_ScopeId
            End Get
            Set(ByVal value As Long)
                m_ScopeId = value
            End Set
        End Property
        Public Property Area() As String
            Get
                Return m_Area
            End Get
            Set(ByVal value As String)
                m_Area = value
            End Set
        End Property
        Public Property Item() As String
            Get
                Return m_Item
            End Get
            Set(ByVal value As String)
                m_Item = value
            End Set
        End Property
        Public Property Service() As String
            Get
                Return m_Service
            End Get
            Set(ByVal value As String)
                m_Service = value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
            End Set
        End Property
        Public Property ScopeItemStatusId() As Integer
            Get
                Return m_ScopeItemStatusId
            End Get
            Set(ByVal value As Integer)
                m_ScopeItemStatusId = value
            End Set
        End Property

        Public Property AssignTo() As String
            Get
                Return m_AssignTo
            End Get
            Set(ByVal value As String)
                m_AssignTo = value
            End Set
        End Property
        Public Property AssignToId() As Integer
            Get
                Return m_AssignToId
            End Get
            Set(ByVal value As Integer)
                m_AssignToId = value
            End Set
        End Property
        Public Property Rate() As Decimal
            Get
                Return m_Rate
            End Get
            Set(ByVal value As Decimal)
                m_Rate = value
            End Set
        End Property
        Public Property Unit() As String
            Get
                Return m_Unit
            End Get
            Set(ByVal value As String)
                m_Unit = value
            End Set
        End Property
        Public Property Quantity() As Decimal
            Get
                Return m_Quantity
            End Get
            Set(ByVal value As Decimal)
                m_Quantity = value
            End Set
        End Property
        Public Property Cost() As Decimal
            Get
                Return m_Cost
            End Get
            Set(ByVal value As Decimal)
                m_Cost = value
            End Set
        End Property
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal value As DateTime)
                m_CreatedDate = value
            End Set
        End Property
        Public Property ModifiedDate() As DateTime
            Get
                Return m_ModifiedDate
            End Get
            Set(ByVal value As DateTime)
                m_ModifiedDate = value
            End Set
        End Property

        Public Property ProjectId() As Long
            Get
                Return m_ProjectId
            End Get
            Set(ByVal value As Long)
                m_ProjectId = value
            End Set
        End Property
        Public Property ServiceGroupId() As Long
            Get
                Return m_ServiceGroupId
            End Get
            Set(ByVal value As Long)
                m_ServiceGroupId = value
            End Set
        End Property
        Public Property DisplayOrder() As Long
            Get
                Return m_DisplayOrder
            End Get
            Set(ByVal value As Long)
                m_DisplayOrder = value
            End Set
        End Property
        Public Property ScopeGroupId() As Long
            Get
                Return m_ScopeGroupId
            End Get
            Set(ByVal value As Long)
                m_ScopeGroupId = value
            End Set
        End Property
        Public Property ScopeGroup() As String
            Get
                Return m_ScopeGroup
            End Get
            Set(ByVal value As String)
                m_ScopeGroup = value
            End Set
        End Property
        Public Property WorksheetService() As String
            Get
                Return m_WorksheetService
            End Get
            Set(ByVal value As String)
                m_WorksheetService = value
            End Set
        End Property
        Public Property AreaMeasurement() As String
            Get
                Return m_AreaMeasurement
            End Get
            Set(ByVal value As String)
                m_AreaMeasurement = value
            End Set
        End Property
    End Class
End Namespace
