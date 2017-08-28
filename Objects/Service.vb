Namespace Warpfusion.A4PP.Objects
    Public Class Service
        Private m_ServiceId As Long
        Private m_ProjectOwnerId As Long
        Private m_UserId As Long
        Private m_Name As String
        Private m_Description As String
        Private m_Disabled As Boolean
        Private m_DisplayOrder As Long

        Public Property ServiceId() As Long
            Get
                Return m_ServiceId
            End Get
            Set(ByVal value As Long)
                m_ServiceId = value
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
        Public Property UserId() As Long
            Get
                Return m_UserId
            End Get
            Set(ByVal value As Long)
                m_UserId = value
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