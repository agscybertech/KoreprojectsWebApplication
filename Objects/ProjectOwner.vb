Namespace Warpfusion.A4PP.Objects
    Public Class ProjectOwner
        Private m_ProjectOwnerId As Long
        Private m_Name As String
        Private m_Address As String
        Private m_PostCode As String
        Private m_Suburb As String
        Private m_City As String
        Private m_Region As String
        Private m_Country As String
        Private m_ContactId As Long
        Private m_Contact1 As String
        Private m_Contact2 As String
        Private m_Contact3 As String
        Private m_Extension1 As String
        Private m_Extension2 As String
        Private m_Extension3 As String
        Private m_Identifier As String
        Private m_Accreditation As String
        Private m_AccreditationNumber As String
        Private m_GSTNumber As String
        Private m_EQRSupervisor As String
        Private m_Logo As String
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_Frequency As String
        Private m_PaymentStartDate As DateTime
        Private m_NextPaymentDate As DateTime

        Private m_ContactName As String

        Public Property ProjectOwnerId() As Long
            Get
                Return m_ProjectOwnerId
            End Get
            Set(ByVal value As Long)
                m_ProjectOwnerId = value
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
        Public Property City() As String
            Get
                Return m_City
            End Get
            Set(ByVal value As String)
                m_City = value
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
        Public Property Country() As String
            Get
                Return m_Country
            End Get
            Set(ByVal value As String)
                m_Country = value
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
        Public Property Contact1() As String
            Get
                Return m_Contact1
            End Get
            Set(ByVal value As String)
                m_Contact1 = value
            End Set
        End Property
        Public Property Contact2() As String
            Get
                Return m_Contact2
            End Get
            Set(ByVal value As String)
                m_Contact2 = value
            End Set
        End Property
        Public Property Contact3() As String
            Get
                Return m_Contact3
            End Get
            Set(ByVal value As String)
                m_Contact3 = value
            End Set
        End Property
        Public Property Extension1() As String
            Get
                Return m_Extension1
            End Get
            Set(ByVal value As String)
                m_Extension1 = value
            End Set
        End Property
        Public Property Extension2() As String
            Get
                Return m_Extension2
            End Get
            Set(ByVal value As String)
                m_Extension2 = value
            End Set
        End Property
        Public Property Identifier() As String
            Get
                Return m_Identifier
            End Get
            Set(ByVal value As String)
                m_Identifier = value
            End Set
        End Property
        Public Property Extension3() As String
            Get
                Return m_Extension3
            End Get
            Set(ByVal value As String)
                m_Extension3 = value
            End Set
        End Property
        Public Property Accreditation() As String
            Get
                Return m_Accreditation
            End Get
            Set(ByVal value As String)
                m_Accreditation = value
            End Set
        End Property
        Public Property AccreditationNumber() As String
            Get
                Return m_AccreditationNumber
            End Get
            Set(ByVal value As String)
                m_AccreditationNumber = value
            End Set
        End Property
        Public Property GSTNumber() As String
            Get
                Return m_GSTNumber
            End Get
            Set(ByVal value As String)
                m_GSTNumber = value
            End Set
        End Property
        Public Property EQRSupervisor() As String
            Get
                Return m_EQRSupervisor
            End Get
            Set(ByVal value As String)
                m_EQRSupervisor = value
            End Set
        End Property
        Public Property Logo() As String
            Get
                Return m_Logo
            End Get
            Set(ByVal value As String)
                m_Logo = value
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
        Public Property ContactName() As String
            Get
                Return m_ContactName
            End Get
            Set(ByVal value As String)
                m_ContactName = value
            End Set
        End Property
        Public Property Frequency() As String
            Get
                Return m_Frequency
            End Get
            Set(ByVal value As String)
                m_Frequency = value
            End Set
        End Property
        Public Property PaymentStartDate() As DateTime
            Get
                Return m_PaymentStartDate
            End Get
            Set(ByVal value As DateTime)
                m_PaymentStartDate = value
            End Set
        End Property
    End Class
End Namespace