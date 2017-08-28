Namespace Warpfusion.A4PP.Objects
    Public Class JobAssignment
        Private m_JobAssignmentId As Long
        Private m_ProjectOwnerId As Long
        Private m_ProjectId As Long
        Private m_JobId As Long
        Private m_UserId As Long
        Private m_Status As Integer
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_UserProfile As UserProfile

        Public Property JobAssignmentId() As Long
            Get
                Return m_JobAssignmentId
            End Get
            Set(ByVal value As Long)
                m_JobAssignmentId = value
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

        Public Property ProjectId() As Long
            Get
                Return m_ProjectId
            End Get
            Set(ByVal value As Long)
                m_ProjectId = value
            End Set
        End Property

        Public Property JobId() As Long
            Get
                Return m_JobId
            End Get
            Set(ByVal value As Long)
                m_JobId = value
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

        Public Property Status() As Long
            Get
                Return m_Status
            End Get
            Set(ByVal value As Long)
                m_Status = value
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

        Public Property UserProfile() As UserProfile
            Get
                Return m_UserProfile
            End Get
            Set(ByVal value As UserProfile)
                m_UserProfile = value
            End Set
        End Property
    End Class
End Namespace