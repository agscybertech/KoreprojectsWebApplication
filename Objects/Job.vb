Namespace Warpfusion.A4PP.Objects
    Public Class Job
        Private m_JobId As Long
        Private m_ProjectOwnerId As Long
        Private m_ProjectId As Long
        Private m_JobName As String
        Private m_JobValue As Integer
        Private m_Description As String
        Private m_Status As Integer
        Private m_DueDate As DateTime
        Private m_StartedDate As DateTime
        Private m_CompletedDate As DateTime
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_StatusName As String
        Private m_JobAssignments As List(Of JobAssignment)

        Public Property JobId() As Long
            Get
                Return m_JobId
            End Get
            Set(ByVal value As Long)
                m_JobId = value
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

        Public Property JobName() As String
            Get
                Return m_JobName
            End Get
            Set(ByVal value As String)
                m_JobName = value
            End Set
        End Property

        Public Property JobValue() As Integer
            Get
                Return m_JobValue
            End Get
            Set(ByVal value As Integer)
                m_JobValue = value
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

        Public Property Status() As Integer
            Get
                Return m_Status
            End Get
            Set(ByVal value As Integer)
                m_Status = value
            End Set
        End Property

        Public Property DueDate() As DateTime
            Get
                Return m_Duedate
            End Get
            Set(ByVal value As DateTime)
                m_Duedate = value
            End Set
        End Property

        Public Property StartedDate() As DateTime
            Get
                Return m_StartedDate
            End Get
            Set(ByVal value As DateTime)
                m_StartedDate = value
            End Set
        End Property

        Public Property CompletedDate() As DateTime
            Get
                Return m_CompletedDate
            End Get
            Set(ByVal value As DateTime)
                m_CompletedDate = value
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

        Public Property StatusName() As String
            Get
                Return m_StatusName
            End Get
            Set(ByVal value As String)
                m_StatusName = value
            End Set
        End Property

        Public Property JobAssignments() As List(Of JobAssignment)
            Get
                Return m_JobAssignments
            End Get
            Set(ByVal value As List(Of JobAssignment))
                m_JobAssignments = value
            End Set
        End Property
    End Class
End Namespace