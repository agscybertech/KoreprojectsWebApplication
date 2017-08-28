Namespace Warpfusion.A4PP.Objects
    Public Class UserProfile
        Private m_UserProfileId As Long
        Private m_UserId As Long
        Private m_FirstName As String
        Private m_LastName As String
        Private m_Gender As Boolean
        Private m_Email As String
        Private m_Address As String
        Private m_Postcode As String
        Private m_Suburb As String
        Private m_City As String
        Private m_Region As String
        Private m_Country As String
        Private m_Contact1 As String
        Private m_Contact2 As String
        Private m_PersonalPhoto As String
        Private m_Extension1 As String
        Private m_Extension2 As String
        Private m_Identifier As String
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime
        Private m_DOB As DateTime
        Private m_Contact3 As String
        Private m_Extension3 As String
        Private m_Notes As String

        Public Property UserProfileId() As Long
            Get
                Return m_UserProfileId
            End Get
            Set(ByVal value As Long)
                m_UserProfileId = value
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
        Public Property FirstName() As String
            Get
                Return m_FirstName
            End Get
            Set(ByVal value As String)
                m_FirstName = value
            End Set
        End Property
        Public Property LastName() As String
            Get
                Return m_LastName
            End Get
            Set(ByVal value As String)
                m_LastName = value
            End Set
        End Property
        Public Property Gender() As Boolean
            Get
                Return m_Gender
            End Get
            Set(ByVal value As Boolean)
                m_Gender = value
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
        Public Property Address() As String
            Get
                Return m_Address
            End Get
            Set(ByVal value As String)
                m_Address = value
            End Set
        End Property
        Public Property Postcode() As String
            Get
                Return m_Postcode
            End Get
            Set(ByVal value As String)
                m_Postcode = value
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
        Public Property PersonalPhoto() As String
            Get
                Return m_PersonalPhoto
            End Get
            Set(ByVal value As String)
                m_PersonalPhoto = value
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
        Public Property Identifier() As String
            Get
                Return m_Identifier
            End Get
            Set(ByVal value As String)
                m_Identifier = value
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
        Public Property DOB() As DateTime
            Get
                Return m_DOB
            End Get
            Set(ByVal value As DateTime)
                m_DOB = value
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
        Public Property Extension3() As String
            Get
                Return m_Extension3
            End Get
            Set(ByVal value As String)
                m_Extension3 = value
            End Set
        End Property
        Public Property Notes() As String
            Get
                Return m_Notes
            End Get
            Set(ByVal value As String)
                m_Notes = value
            End Set
        End Property
    End Class
End Namespace