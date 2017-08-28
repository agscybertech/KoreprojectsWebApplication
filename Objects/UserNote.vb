Namespace Warpfusion.A4PP.Objects
    Public Class UserNote
        Private m_UserNoteId As Long
        Private m_Description As String
        Private m_NoteContent As String
        Private m_Owner As Long
        Private m_Author As Long
        Private m_ProjectStatusId As Integer
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime

        Public Property UserNoteId() As Long
            Get
                Return m_UserNoteId
            End Get
            Set(ByVal value As Long)
                m_UserNoteId = value
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
        Public Property NoteContent() As String
            Get
                Return m_NoteContent
            End Get
            Set(ByVal value As String)
                m_NoteContent = value
            End Set
        End Property
        Public Property Owner() As Long
            Get
                Return m_Owner
            End Get
            Set(ByVal value As Long)
                m_Owner = value
            End Set
        End Property
        Public Property Author() As Long
            Get
                Return m_Author
            End Get
            Set(ByVal value As Long)
                m_Author = value
            End Set
        End Property
        Public Property ProjectStatusId() As Long
            Get
                Return m_ProjectStatusId
            End Get
            Set(ByVal value As Long)
                m_ProjectStatusId = value
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
    End Class
End Namespace
