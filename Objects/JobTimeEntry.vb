Namespace Warpfusion.A4PP.Objects
    Public Class JobTimeEntry        
        Private m_JobTimeEntryId As Long
        Private m_JobAssignmentId As Long
        Private m_JobId As Long
        Private m_UserId As Long
        Private m_StartTime As DateTime
        Private m_EndTime As DateTime
        Private m_WorkingTimeInMinutes As Integer
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime

        Public Property JobTimeEntryId() As Long
            Get
                Return m_JobTimeEntryId
            End Get
            Set(ByVal value As Long)
                m_JobTimeEntryId = value
            End Set
        End Property

        Public Property JobAssignmentId() As Long
            Get
                Return m_JobAssignmentId
            End Get
            Set(ByVal value As Long)
                m_JobAssignmentId = value
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

        Public Property StartTime() As DateTime
            Get
                Return m_StartTime
            End Get
            Set(ByVal value As DateTime)
                m_StartTime = value
            End Set
        End Property

        Public Property EndTime() As DateTime
            Get
                Return m_EndTime
            End Get
            Set(ByVal value As DateTime)
                m_EndTime = value
            End Set
        End Property

        Public Property WorkingTimeInMinutes() As Integer
            Get
                Return m_WorkingTimeInMinutes
            End Get
            Set(ByVal value As Integer)
                m_WorkingTimeInMinutes = value
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