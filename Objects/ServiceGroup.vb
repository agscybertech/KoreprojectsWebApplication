Namespace Warpfusion.A4PP.Objects
    Public Class ServiceGroup
        Private m_ServiceGroupId As Long
        Private m_UserId As Long
        Private m_Name As String    
        Private m_IsPrivate As Integer
        Private m_DisplayOrder As Integer

        Public Property ServiceGroupId() As Long
            Get
                Return m_ServiceGroupId
            End Get
            Set(ByVal value As Long)
                m_ServiceGroupId = value
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
        Public Property IsPrivate() As Integer
            Get
                Return m_IsPrivate
            End Get
            Set(ByVal value As Integer)
                m_IsPrivate = value
            End Set
        End Property
        Public Property DisplayOrder() As Integer
            Get
                Return m_DisplayOrder
            End Get
            Set(ByVal value As Integer)
                m_DisplayOrder = value
            End Set
        End Property
    End Class
End Namespace