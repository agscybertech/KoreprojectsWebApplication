Namespace Warpfusion.A4PP.Objects
    Public Class Reminder
        Private m_ReminderId As Long
        Private m_ProjectOwnerId As Long
        Private m_ProjectId As Long
        Private m_ReminderTitle As String
        Private m_ReminderContentData As String
        Private m_Status As Integer
        Private m_DisplayOrder As Integer
        Private m_StartDate As DateTime
        Private m_EndDate As DateTime
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_EmailTimeSetting As Integer

        Public Property ReminderId() As Long
            Get
                Return m_ReminderId
            End Get
            Set(ByVal value As Long)
                m_ReminderId = value
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
        Public Property ReminderTitle() As String
            Get
                Return m_ReminderTitle
            End Get
            Set(ByVal value As String)
                m_ReminderTitle = value
            End Set
        End Property
        Public Property ReminderContentData() As String
            Get
                Return m_ReminderContentData
            End Get
            Set(ByVal value As String)
                m_ReminderContentData = value
            End Set
        End Property
        Public Property Status() As Integer
            Get
                Return m_Status
            End Get
            Set(ByVal value As Integer)
                m_Status = value
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
        Public Property StartDate() As DateTime
            Get
                Return m_StartDate
            End Get
            Set(ByVal value As DateTime)
                m_StartDate = value
            End Set
        End Property
        Public Property EndDate() As DateTime
            Get
                Return m_EndDate
            End Get
            Set(ByVal value As DateTime)
                m_EndDate = value
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
        Public Property EmailTimeSetting() As Integer
            Get
                Return m_EmailTimeSetting
            End Get
            Set(ByVal value As Integer)
                m_EmailTimeSetting = value
            End Set
        End Property
    End Class
End Namespace