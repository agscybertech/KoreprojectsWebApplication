Namespace Warpfusion.A4PP.Objects
    Public Class WorksheetGroup
        Private m_WorksheetGroupId As Long
        Private m_ProjectOwnerId As Long
        Private m_Name As String
        Private m_Description As String
        Private m_Disabled As Boolean
        Private m_DisplayOrder As Long

        Public Property WorksheetGroupId() As Long
            Get
                Return m_WorksheetGroupId
            End Get
            Set(ByVal value As Long)
                m_WorksheetGroupId = value
            End Set
        End Property
        Public Property ProjectOwnerId() As Long
            Get
                Return m_ProjectOwnerId
            End Get
            Set(ByVal value As Long)
                m_ProjectOwnerId = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
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
        Public Property Disabled() As Boolean
            Get
                Return m_Disabled
            End Get
            Set(ByVal value As Boolean)
                m_Disabled = value
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
    End Class
End Namespace
