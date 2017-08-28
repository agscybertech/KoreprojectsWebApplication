Namespace Warpfusion.A4PP.Objects
    Public Class DynamicPageContent
        Private m_DynamicPageContentId As Long
        Private m_ContentTypeId As Integer
        Private m_DynamicPageId As Integer
        Private m_ProjectOwnerId As Long
        Private m_ProjectId As Long
        Private m_ContentTitle As String
        Private m_ContentData As String
        Private m_Disabled As Boolean
        Private m_DisplayOrder As Integer
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime

        Private m_ContentTypeName As String

        Public Property DynamicPageContentId() As Long
            Get
                Return m_DynamicPageContentId
            End Get
            Set(ByVal value As Long)
                m_DynamicPageContentId = value
            End Set
        End Property
        Public Property ContentTypeId() As Integer
            Get
                Return m_ContentTypeId
            End Get
            Set(ByVal value As Integer)
                m_ContentTypeId = value
            End Set
        End Property
        Public Property DynamicPageId() As Integer
            Get
                Return m_DynamicPageId
            End Get
            Set(ByVal value As Integer)
                m_DynamicPageId = value
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
        Public Property ContentTitle() As String
            Get
                Return m_ContentTitle
            End Get
            Set(ByVal value As String)
                m_ContentTitle = value
            End Set
        End Property
        Public Property ContentData() As String
            Get
                Return m_ContentData
            End Get
            Set(ByVal value As String)
                m_ContentData = value
            End Set
        End Property
        Public Property Disabled() As Boolean
            Get
                Return m_Disabled
            End Get
            Set(ByVal value As Boolean)
                m_Disabled = value
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

        Public Property ContentTypeName() As String
            Get
                Return m_ContentTypeName
            End Get
            Set(ByVal value As String)
                m_ContentTypeName = value
            End Set
        End Property
    End Class
End Namespace