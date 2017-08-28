Namespace Warpfusion.A4PP.Objects
    Public Class UserProjectStatusValue
        Private m_UserProjectStatusValueId As Long
        Private m_ProjectId As Long
        Private m_UserId As Long
        Private m_UserProjectStatusValue As Integer

        Public Property UserProjectStatusValueId() As Long
            Get
                Return m_UserProjectStatusValueId
            End Get
            Set(ByVal value As Long)
                m_UserProjectStatusValueId = value
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
        Public Property UserId() As Long
            Get
                Return m_UserId
            End Get
            Set(ByVal value As Long)
                m_UserId = value
            End Set
        End Property
        Public Property UserProjectStatusValue() As Integer
            Get
                Return m_UserProjectStatusValue
            End Get
            Set(ByVal value As Integer)
                m_UserProjectStatusValue = value
            End Set
        End Property
    End Class
End Namespace