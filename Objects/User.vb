Namespace Warpfusion.A4PP.Objects
    Public Class User
        Private m_UserId As Long
        Private m_Email As String
        Private m_Password As String
        Private m_Type As Integer
        Private m_Mailbox As String
        Private m_BranchId As Long
        Private m_CompanyId As Long
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_DeactivatedDate As DateTime
        Private m_ActionToken As String
        Private m_AccessLevel As Integer

        Public Property UserId() As Long
            Get
                Return m_UserId
            End Get
            Set(ByVal value As Long)
                m_UserId = value
            End Set
        End Property
        Public Property Email() As String
            Get
                Return m_Email
            End Get
            Set(ByVal value As String)
                m_Email = value
            End Set
        End Property
        Public Property Password() As String
            Get
                Return m_Password
            End Get
            Set(ByVal value As String)
                m_Password = value
            End Set
        End Property
        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal value As Integer)
                m_Type = value
            End Set
        End Property
        Public Property Mailbox() As String
            Get
                Return m_Mailbox
            End Get
            Set(ByVal value As String)
                m_Mailbox = value
            End Set
        End Property
        Public Property BranchId() As Long
            Get
                Return m_BranchId
            End Get
            Set(ByVal value As Long)
                m_BranchId = value
            End Set
        End Property
        Public Property CompanyId() As Long
            Get
                Return m_CompanyId
            End Get
            Set(ByVal value As Long)
                m_CompanyId = value
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
        Public Property DeactivatedDate() As DateTime
            Get
                Return m_DeactivatedDate
            End Get
            Set(ByVal value As DateTime)
                m_DeactivatedDate = value
            End Set
        End Property
        Public Property ActionToken() As String
            Get
                Return m_ActionToken
            End Get
            Set(ByVal value As String)
                m_ActionToken = value
            End Set
        End Property
        Public Property AccessLevel() As Integer
            Get
                Return m_AccessLevel
            End Get
            Set(ByVal value As Integer)
                m_AccessLevel = value
            End Set
        End Property
    End Class
End Namespace