Namespace Warpfusion.A4PP.Objects
    Public Class UserFile
        Private m_UserFileId As Long
        Private m_FileName As String
        Private m_FileExtension As String
        Private m_FileName_System As String
        Private m_Description As String
        Private m_Size As Long
        Private m_Owner As Long
        Private m_Author As Long
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_UserPhotoUploadFolder As String

        Public Property UserFileId() As Long
            Get
                Return m_UserFileId
            End Get
            Set(ByVal value As Long)
                m_UserFileId = value
            End Set
        End Property
        Public Property FileName() As String
            Get
                Return m_FileName
            End Get
            Set(ByVal value As String)
                m_FileName = value
            End Set
        End Property
        Public Property FileExtension() As String
            Get
                Return m_FileExtension
            End Get
            Set(ByVal value As String)
                m_FileExtension = value
            End Set
        End Property
        Public Property FileName_System() As String
            Get
                Return m_FileName_System
            End Get
            Set(ByVal value As String)
                m_FileName_System = value
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
        Public Property Size() As Long
            Get
                Return m_Size
            End Get
            Set(ByVal value As Long)
                m_Size = value
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
        Public Property UserPhotoUploadFolder() As String
            Get
                Return m_UserPhotoUploadFolder
            End Get
            Set(ByVal value As String)
                m_UserPhotoUploadFolder = value
            End Set
        End Property
    End Class
End Namespace
