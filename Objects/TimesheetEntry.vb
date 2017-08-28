Namespace Warpfusion.A4PP.Objects
    Public Class TimesheetEntry
        Private m_TimesheetEntryId As Long
        Private m_Description As String
        Private m_NoteContent As String
        Private m_PartyA As Long
        Private m_PartyB As Long
        Private m_EntryDate As DateTime
        Private m_WorkStart As DateTime
        Private m_WorkEnd As DateTime
        Private m_LunchStart As DateTime
        Private m_LunchEnd As DateTime
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_ProcessDate As DateTime
        Private m_CycleEndDate As DateTime
        Private m_DeletedDate As DateTime

        Public Property TimesheetEntryId() As Long
            Get
                Return m_TimesheetEntryId
            End Get
            Set(ByVal value As Long)
                m_TimesheetEntryId = value
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
        Public Property PartyA() As Long
            Get
                Return m_PartyA
            End Get
            Set(ByVal value As Long)
                m_PartyA = value
            End Set
        End Property
        Public Property PartyB() As Long
            Get
                Return m_PartyB
            End Get
            Set(ByVal value As Long)
                m_PartyB = value
            End Set
        End Property
        Public Property EntryDate() As DateTime
            Get
                Return m_EntryDate
            End Get
            Set(ByVal value As DateTime)
                m_EntryDate = value
            End Set
        End Property
        Public Property WorkStart() As DateTime
            Get
                Return m_WorkStart
            End Get
            Set(ByVal value As DateTime)
                m_WorkStart = value
            End Set
        End Property
        Public Property WorkEnd() As DateTime
            Get
                Return m_WorkEnd
            End Get
            Set(ByVal value As DateTime)
                m_WorkEnd = value
            End Set
        End Property
        Public Property LunchStart() As DateTime
            Get
                Return m_LunchStart
            End Get
            Set(ByVal value As DateTime)
                m_LunchStart = value
            End Set
        End Property
        Public Property LunchEnd() As DateTime
            Get
                Return m_LunchEnd
            End Get
            Set(ByVal value As DateTime)
                m_LunchEnd = value
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
        Public Property ProcessDate() As DateTime
            Get
                Return m_ProcessDate
            End Get
            Set(ByVal value As DateTime)
                m_ProcessDate = value
            End Set
        End Property
        Public Property CycleEndDate() As DateTime
            Get
                Return m_CycleEndDate
            End Get
            Set(ByVal value As DateTime)
                m_CycleEndDate = value
            End Set
        End Property
        Public Property DeletedDate() As DateTime
            Get
                Return m_DeletedDate
            End Get
            Set(ByVal value As DateTime)
                m_DeletedDate = value
            End Set
        End Property
    End Class
End Namespace