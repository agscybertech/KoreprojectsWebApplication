Namespace Warpfusion.A4PP.Objects
    Public Class Project
        Private m_ProjectId As Long
        Private m_Name As String
        Private m_Address As String
        Private m_PostCode As String
        Private m_Suburb As String
        Private m_SuburbID As Integer
        Private m_City As String
        Private m_CityID As Integer
        Private m_Region As String
        Private m_RegionID As Integer
        Private m_Country As String
        Private m_CountryID As Integer
        Private m_GroupID As Integer
        Private m_GroupName As String
        Private m_StartDate As DateTime
        Private m_FinishDate As DateTime
        Private m_DueDate As DateTime
        Private m_AssessmentDate As DateTime
        Private m_Priority As Integer
        Private m_Hazard As String
        Private m_ProjectStatusId As Integer
        Private m_ContactId As Long
        Private m_ProjectOwnerId As Long
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_DeactivatedDate As DateTime
        Private m_EQCClaimNumber As String
        Private m_QuotationDate As DateTime
        Private m_EstimatedTime As String        
        Private m_ScopeDate As DateTime
        Private m_ArchivedDate As DateTime

        Private m_ProjectStatusName As String
        Private m_ContactName As String
        Private m_ProjectOwnerName As String

        Public Property ProjectId() As Long
            Get
                Return m_ProjectId
            End Get
            Set(ByVal value As Long)
                m_ProjectId = value
            End Set
        End Property
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(ByVal value As String)
                m_Name = value
            End Set
        End Property
        Public Property Address() As String
            Get
                Return m_Address
            End Get
            Set(ByVal value As String)
                m_Address = value
            End Set
        End Property
        Public Property PostCode() As String
            Get
                Return m_PostCode
            End Get
            Set(ByVal value As String)
                m_PostCode = value
            End Set
        End Property
        Public Property Suburb() As String
            Get
                Return m_Suburb
            End Get
            Set(ByVal value As String)
                m_Suburb = value
            End Set
        End Property
        Public Property SuburbID() As Integer
            Get
                Return m_SuburbID
            End Get
            Set(ByVal value As Integer)
                m_SuburbID = value
            End Set
        End Property
        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(ByVal value As String)
                m_City = value
            End Set
        End Property
        Public Property CityID() As Integer
            Get
                Return m_CityID
            End Get
            Set(ByVal value As Integer)
                m_CityID = value
            End Set
        End Property
        Public Property Region() As String
            Get
                Return m_Region
            End Get
            Set(ByVal value As String)
                m_Region = value
            End Set
        End Property
        Public Property RegionID() As Integer
            Get
                Return m_RegionID
            End Get
            Set(ByVal value As Integer)
                m_RegionID = value
            End Set
        End Property
        Public Property Country() As String
            Get
                Return m_Country
            End Get
            Set(ByVal value As String)
                m_Country = value
            End Set
        End Property
        Public Property CountryID() As Integer
            Get
                Return m_CountryID
            End Get
            Set(ByVal value As Integer)
                m_CountryID = value
            End Set
        End Property
        Public Property GroupID() As Integer
            Get
                Return m_GroupID
            End Get
            Set(ByVal value As Integer)
                m_GroupID = value
            End Set
        End Property
        Public Property GroupName() As String
            Get
                Return m_GroupName
            End Get
            Set(ByVal value As String)
                m_GroupName = value
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
        Public Property FinishDate() As DateTime
            Get
                Return m_FinishDate
            End Get
            Set(ByVal value As DateTime)
                m_FinishDate = value
            End Set
        End Property
        Public Property DueDate() As DateTime
            Get
                Return m_DueDate
            End Get
            Set(ByVal value As DateTime)
                m_DueDate = value
            End Set
        End Property
        Public Property AssessmentDate() As DateTime
            Get
                Return m_AssessmentDate
            End Get
            Set(ByVal value As DateTime)
                m_AssessmentDate = value
            End Set
        End Property
        Public Property Priority() As Integer
            Get
                Return m_Priority
            End Get
            Set(ByVal value As Integer)
                m_Priority = value
            End Set
        End Property
        Public Property Hazard() As String
            Get
                Return m_Hazard
            End Get
            Set(ByVal value As String)
                m_Hazard = value
            End Set
        End Property
        Public Property ProjectStatusId() As Integer
            Get
                Return m_ProjectStatusId
            End Get
            Set(ByVal value As Integer)
                m_ProjectStatusId = value
            End Set
        End Property
        Public Property ContactId() As Long
            Get
                Return m_ContactId
            End Get
            Set(ByVal value As Long)
                m_ContactId = value
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
        Public Property EQCClaimNumber() As String
            Get
                Return m_EQCClaimNumber
            End Get
            Set(ByVal value As String)
                m_EQCClaimNumber = value
            End Set
        End Property
        Public Property QuotationDate() As DateTime
            Get
                Return m_QuotationDate
            End Get
            Set(ByVal value As DateTime)
                m_QuotationDate = value
            End Set
        End Property
        Public Property EstimatedTime() As String
            Get
                Return m_EstimatedTime
            End Get
            Set(ByVal value As String)
                m_EstimatedTime = value
            End Set
        End Property
        Public Property ScopeDate() As DateTime
            Get
                Return m_ScopeDate
            End Get
            Set(ByVal value As DateTime)
                m_ScopeDate = value
            End Set
        End Property
        Public Property ArchivedDate() As DateTime
            Get
                Return m_ArchivedDate
            End Get
            Set(ByVal value As DateTime)
                m_ArchivedDate = value
            End Set
        End Property
        Public Property ProjectStatusName() As String
            Get
                Return m_ProjectStatusName
            End Get
            Set(ByVal value As String)
                m_ProjectStatusName = value
            End Set
        End Property
        Public Property ContactName() As String
            Get
                Return m_ContactName
            End Get
            Set(ByVal value As String)
                m_ContactName = value
            End Set
        End Property
        Public Property ProjectOwnerName() As String
            Get
                Return m_ProjectOwnerName
            End Get
            Set(ByVal value As String)
                m_ProjectOwnerName = value
            End Set
        End Property
    End Class
End Namespace