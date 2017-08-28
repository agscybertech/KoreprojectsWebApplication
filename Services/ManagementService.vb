Imports System.Data.SqlClient
Imports System.Xml
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.Shared.Utilities
Namespace Warpfusion.A4PP.Services
    Public Enum UserType
        Customer = 0
        Member = 1
        Admin = 2
        Staff = 101
        Contractor = 102
        Supplier = 103
    End Enum
    Public Enum UserRelationshipStatus
        'NotApproved = 0
        Inactive = 1
        Active = 2
    End Enum
    Public Enum PayStatus
        PayLater = 0
        PaidStandard = 1
        PaidDiscount = 2
    End Enum
    Public Enum JobStatus
        Cancelled = -1
        Not_Started = 0
        In_Progress = 1
        Completed = 2
    End Enum
    Public Class ManagementService
        Private m_SQLConn As New SQLConn
        Private m_SqlConnection As SqlConnection = m_SQLConn.conn()
        Private m_SqlCommand As SqlCommand
        Private m_SqlDataAdapter As SqlDataAdapter
        Private m_SQL As String
        Private m_SQLConnectionString As String = String.Empty

        Public WriteOnly Property SetSQLConn() As SQLConn
            Set(ByVal value As SQLConn)
                m_SQLConn = value
            End Set
        End Property

        Public Property SQLConnection() As String
            Get
                Return m_SQLConnectionString
            End Get
            Set(ByVal value As String)
                m_SQLConnectionString = value
                m_SQLConn.Connection = m_SQLConnectionString
            End Set
        End Property

        Public Function Login(ByVal Email As String, ByVal Password As String) As User
            Dim result As User = Nothing
            Dim rs_User As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "Login"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@Email", Email)
            m_SqlCommand.Parameters.AddWithValue("@Password", Password)
            conn.Open()
            rs_User = m_SqlCommand.ExecuteReader
            If rs_User.HasRows Then
                result = New User()
                rs_User.Read()
                If rs_User("UserID") Is DBNull.Value Then
                    result.UserId = String.Empty
                Else
                    result.UserId = rs_User("UserID")
                End If
                If rs_User("Email") Is DBNull.Value Then
                    result.Email = String.Empty
                Else
                    result.Email = rs_User("Email")
                End If
                If rs_User("Type") Is DBNull.Value Then
                    result.Type = UserType.Customer
                Else
                    result.Type = rs_User("Type")
                End If
                If rs_User("MailBox") Is DBNull.Value Then
                    result.Mailbox = Nothing
                Else
                    result.Mailbox = rs_User("MailBox")
                End If
                If rs_User("BranchID") Is DBNull.Value Then
                    result.BranchId = 0
                Else
                    result.BranchId = rs_User("BranchID")
                End If
                If rs_User("CompanyID") Is DBNull.Value Then
                    result.CompanyId = 0
                Else
                    result.CompanyId = rs_User("CompanyID")
                End If
                If rs_User("AccessLevel") Is DBNull.Value Then
                    result.AccessLevel = 0
                Else
                    result.AccessLevel = rs_User("AccessLevel")
                End If
            End If
            rs_User.Close()
            rs_User = Nothing
            conn.Close()

            Return result
        End Function

        Public Function Linkin(ByVal LinkID As String) As User
            Dim result As User = Nothing
            Dim rs_User As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "LinkIn"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@LinkID", LinkID)
            conn.Open()
            rs_User = m_SqlCommand.ExecuteReader
            If rs_User.HasRows Then
                result = New User()
                rs_User.Read()
                If rs_User("UserID") Is DBNull.Value Then
                    result.UserId = String.Empty
                Else
                    result.UserId = rs_User("UserID")
                End If
                If rs_User("Email") Is DBNull.Value Then
                    result.Email = String.Empty
                Else
                    result.Email = rs_User("Email")
                End If
                If rs_User("Type") Is DBNull.Value Then
                    result.Type = UserType.Customer
                Else
                    result.Type = rs_User("Type")
                End If
                If rs_User("MailBox") Is DBNull.Value Then
                    result.Mailbox = Nothing
                Else
                    result.Mailbox = rs_User("MailBox")
                End If
                If rs_User("BranchID") Is DBNull.Value Then
                    result.BranchId = 0
                Else
                    result.BranchId = rs_User("BranchID")
                End If
                If rs_User("CompanyID") Is DBNull.Value Then
                    result.CompanyId = 0
                Else
                    result.CompanyId = rs_User("CompanyID")
                End If
                If rs_User("AccessLevel") Is DBNull.Value Then
                    result.AccessLevel = 0
                Else
                    result.AccessLevel = rs_User("AccessLevel")
                End If
            End If
            rs_User.Close()
            rs_User = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetUserByEmail(ByVal Email As String) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserByEmail"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@Email", Email)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "User")
            conn.Close()
            Return result
        End Function

        Public Function GetUserCountByEmail(ByVal Email As String) As Integer
            Dim result As Integer = 0
            Dim dsUserCount As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserCountByEmail"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@Email", Email)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserCount, "UserCount")
            conn.Close()
            If dsUserCount.Tables.Count > 0 Then
                If dsUserCount.Tables(0).Rows.Count > 0 Then
                    result = dsUserCount.Tables(0).Rows(0)("UserCount")
                End If
            End If
            Return result
        End Function

        Public Function GetActivatedUserCountByEmail(ByVal Email As String) As Integer
            Dim result As Integer = 0
            Dim dsUserCount As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetActivatedUserCountByEmail"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@Email", Email)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserCount, "UserCount")
            conn.Close()
            If dsUserCount.Tables.Count > 0 Then
                If dsUserCount.Tables(0).Rows.Count > 0 Then
                    result = dsUserCount.Tables(0).Rows(0)("UserCount")
                End If
            End If
            Return result
        End Function

        Public Function CreateUser(ByVal User As User, ByVal LoginUserId As Long) As Long
            Dim result As Long = 0
            Dim dsUser As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUser"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If User.Email = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Email", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Email", User.Email)
            End If
            If User.Password = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Password", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Password", User.Password)
            End If
            m_SqlCommand.Parameters.AddWithValue("@Type", User.Type)
            m_SqlCommand.Parameters.AddWithValue("@LoginUserId", LoginUserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUser, "User")
            conn.Close()
            If dsUser.Tables.Count > 0 Then
                If dsUser.Tables(0).Rows.Count > 0 Then
                    result = dsUser.Tables(0).Rows(0)("UserId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateUserType(ByVal User As User)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserType"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", User.UserId)
            m_SqlCommand.Parameters.AddWithValue("@Type", User.Type)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserDeactivated(ByVal User As User)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserDeactivatedDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", User.UserId)
            m_SqlCommand.Parameters.AddWithValue("@DeactivatedDate", User.DeactivatedDate)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserActivated(ByVal User As User)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserDeactivatedDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", User.UserId)
            m_SqlCommand.Parameters.AddWithValue("@DeactivatedDate", DBNull.Value)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function IsUserActivated(ByVal UserId As Long) As Boolean
            Dim result As Boolean = False
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "IsUserActivated"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            result = m_SqlCommand.ExecuteScalar()
            conn.Close()
            Return result
        End Function

        Public Function IsUserAccountOverDue(ByVal UserId As Long) As Boolean
            Dim result As Boolean = False
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "IsUserAccountOverDue"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            result = m_SqlCommand.ExecuteScalar()
            conn.Close()
            Return result
        End Function

        Public Sub UpdateUserDeleted(ByVal User As User)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserDeletedDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", User.UserId)
            m_SqlCommand.Parameters.AddWithValue("@DeletedDate", User.DeactivatedDate)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserEmail(ByVal User As User)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserEmail"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", User.UserId)
            If User.Email = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Email", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Email", User.Email)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserAccessLevel(ByVal User As User)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserAccessLevel"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", User.UserId)
            m_SqlCommand.Parameters.AddWithValue("@AccessLevel", User.AccessLevel)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserPassword(ByVal User As User)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserPassword"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", User.UserId)
            If User.Password = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Password", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Password", User.Password)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserPasswordEmailToken(ByVal User As User)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserPasswordEmailToken"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@Email", User.Email)
            m_SqlCommand.Parameters.AddWithValue("@ActionToken", User.ActionToken)
            If User.Password = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Password", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Password", User.Password)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserActionToken(ByVal Email As String, ByVal ActionToken As String)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserActionToken"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@Email", Email)
            m_SqlCommand.Parameters.AddWithValue("@actionToken", ActionToken)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function IsUserActionTokenValid(ByVal Email As String, ByVal ActionToken As String) As Boolean
            Dim result As Boolean = False
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "IsUserActionTokenValid"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@Email", Email)
            m_SqlCommand.Parameters.AddWithValue("@ActionToken", ActionToken)
            conn.Open()
            result = m_SqlCommand.ExecuteScalar()
            conn.Close()
            Return result
        End Function

        Public Function CreateUserProfile(ByVal UserProfile As UserProfile) As Long
            Dim result As Long = 0
            Dim dsUserProfile As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserProfile"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If UserProfile.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserID", UserProfile.UserId)
            End If
            If UserProfile.FirstName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FirstName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FirstName", UserProfile.FirstName)
            End If
            If UserProfile.LastName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@LastName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@LastName", UserProfile.LastName)
            End If
            m_SqlCommand.Parameters.AddWithValue("@Gender", UserProfile.Gender)
            If UserProfile.Email = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Email", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Email", UserProfile.Email)
            End If
            If UserProfile.Address = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Address", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Address", UserProfile.Address)
            End If
            If UserProfile.Postcode = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Postcode", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Postcode", UserProfile.Postcode)
            End If
            If UserProfile.Suburb = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Suburb", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Suburb", UserProfile.Suburb)
            End If
            If UserProfile.City = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@City", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@City", UserProfile.City)
            End If
            If UserProfile.Region = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Region", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Region", UserProfile.Region)
            End If
            If UserProfile.Country = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Country", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Country", UserProfile.Country)
            End If
            If UserProfile.Contact1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact1", UserProfile.Contact1)
            End If
            If UserProfile.Contact2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact2", UserProfile.Contact2)
            End If
            If UserProfile.PersonalPhoto = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@PersonalPhoto", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@PersonalPhoto", UserProfile.PersonalPhoto)
            End If
            If UserProfile.Extension1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension1", UserProfile.Extension1)
            End If
            If UserProfile.Extension2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension2", UserProfile.Extension2)
            End If
            If UserProfile.DOB = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DOB", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DOB", UserProfile.DOB)
            End If
            If UserProfile.Contact3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact3", UserProfile.Contact3)
            End If
            If UserProfile.Extension3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension3", UserProfile.Extension3)
            End If
            If UserProfile.Notes = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Notes", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Notes", UserProfile.Notes)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserProfile, "UserProfile")
            conn.Close()
            If dsUserProfile.Tables.Count > 0 Then
                If dsUserProfile.Tables(0).Rows.Count > 0 Then
                    result = dsUserProfile.Tables(0).Rows(0)("UserProfileId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateUserProfile(ByVal UserProfile As UserProfile)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserProfile"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserProfileID", UserProfile.UserProfileId)
            If UserProfile.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserID", UserProfile.UserId)
            End If
            If UserProfile.FirstName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FirstName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FirstName", UserProfile.FirstName)
            End If
            If UserProfile.LastName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@LastName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@LastName", UserProfile.LastName)
            End If
            m_SqlCommand.Parameters.AddWithValue("@Gender", UserProfile.Gender)
            If UserProfile.Email = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Email", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Email", UserProfile.Email)
            End If
            If UserProfile.Address = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Address", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Address", UserProfile.Address)
            End If
            If UserProfile.Postcode = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Postcode", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Postcode", UserProfile.Postcode)
            End If
            If UserProfile.Suburb = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Suburb", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Suburb", UserProfile.Suburb)
            End If
            If UserProfile.City = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@City", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@City", UserProfile.City)
            End If
            If UserProfile.Region = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Region", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Region", UserProfile.Region)
            End If
            If UserProfile.Country = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Country", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Country", UserProfile.Country)
            End If
            If UserProfile.Contact1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact1", UserProfile.Contact1)
            End If
            If UserProfile.Contact2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact2", UserProfile.Contact2)
            End If
            If UserProfile.PersonalPhoto = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@PersonalPhoto", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@PersonalPhoto", UserProfile.PersonalPhoto)
            End If
            If UserProfile.Extension1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension1", UserProfile.Extension1)
            End If
            If UserProfile.Extension2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension2", UserProfile.Extension2)
            End If
            If UserProfile.DOB = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DOB", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DOB", UserProfile.DOB)
            End If
            If UserProfile.Contact3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact3", UserProfile.Contact3)
            End If
            If UserProfile.Extension3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension3", UserProfile.Extension3)
            End If
            If UserProfile.Notes = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Notes", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Notes", UserProfile.Notes)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUserProfileByUserID(ByVal UserId As Long) As UserProfile
            Dim result As UserProfile = New UserProfile()
            Dim rs_UserProfile As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProfileByUserID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", UserId)
            conn.Open()
            rs_UserProfile = m_SqlCommand.ExecuteReader
            If rs_UserProfile.HasRows Then
                rs_UserProfile.Read()
                If rs_UserProfile("UserProfileID") Is DBNull.Value Then
                    result.UserProfileId = 0
                Else
                    result.UserProfileId = rs_UserProfile("UserProfileID")
                End If
                If rs_UserProfile("UserID") Is DBNull.Value Then
                    result.UserId = 0
                Else
                    result.UserId = rs_UserProfile("UserID")
                End If
                If rs_UserProfile("FirstName") Is DBNull.Value Then
                    result.FirstName = String.Empty
                Else
                    result.FirstName = rs_UserProfile("FirstName")
                End If
                If rs_UserProfile("LastName") Is DBNull.Value Then
                    result.LastName = String.Empty
                Else
                    result.LastName = rs_UserProfile("LastName")
                End If
                If rs_UserProfile("Gender") Is DBNull.Value Then
                    result.Gender = False
                Else
                    result.Gender = rs_UserProfile("Gender")
                End If
                If rs_UserProfile("Email") Is DBNull.Value Then
                    result.Email = String.Empty
                Else
                    result.Email = rs_UserProfile("Email")
                End If
                If rs_UserProfile("Address") Is DBNull.Value Then
                    result.Address = String.Empty
                Else
                    result.Address = rs_UserProfile("Address")
                End If
                If rs_UserProfile("Postcode") Is DBNull.Value Then
                    result.Postcode = String.Empty
                Else
                    result.Postcode = rs_UserProfile("Postcode")
                End If
                If rs_UserProfile("Suburb") Is DBNull.Value Then
                    result.Suburb = String.Empty
                Else
                    result.Suburb = rs_UserProfile("Suburb")
                End If
                If rs_UserProfile("City") Is DBNull.Value Then
                    result.City = String.Empty
                Else
                    result.City = rs_UserProfile("City")
                End If
                If rs_UserProfile("Region") Is DBNull.Value Then
                    result.Region = String.Empty
                Else
                    result.Region = rs_UserProfile("Region")
                End If
                If rs_UserProfile("Country") Is DBNull.Value Then
                    result.Country = String.Empty
                Else
                    result.Country = rs_UserProfile("Country")
                End If
                If rs_UserProfile("Contact1") Is DBNull.Value Then
                    result.Contact1 = String.Empty
                Else
                    result.Contact1 = rs_UserProfile("Contact1")
                End If
                If rs_UserProfile("Contact2") Is DBNull.Value Then
                    result.Contact2 = String.Empty
                Else
                    result.Contact2 = rs_UserProfile("Contact2")
                End If
                If rs_UserProfile("Extension1") Is DBNull.Value Then
                    result.Extension1 = String.Empty
                Else
                    result.Extension1 = rs_UserProfile("Extension1")
                End If
                If rs_UserProfile("Extension2") Is DBNull.Value Then
                    result.Extension2 = String.Empty
                Else
                    result.Extension2 = rs_UserProfile("Extension2")
                End If
                If rs_UserProfile("Identifier") Is DBNull.Value Then
                    result.Identifier = String.Empty
                Else
                    result.Identifier = rs_UserProfile("Identifier")
                End If
                If rs_UserProfile("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_UserProfile("CreatedDate")
                End If
                If rs_UserProfile("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_UserProfile("ModifiedDate")
                End If
                If rs_UserProfile("DOB") Is DBNull.Value Then
                    result.DOB = Nothing
                Else
                    result.DOB = rs_UserProfile("DOB")
                End If
                If rs_UserProfile("Contact3") Is DBNull.Value Then
                    result.Contact3 = String.Empty
                Else
                    result.Contact3 = rs_UserProfile("Contact3")
                End If
                If rs_UserProfile("Extension3") Is DBNull.Value Then
                    result.Extension3 = String.Empty
                Else
                    result.Extension3 = rs_UserProfile("Extension3")
                End If
                If rs_UserProfile("PersonalPhoto") Is DBNull.Value Then
                    result.PersonalPhoto = String.Empty
                Else
                    result.PersonalPhoto = rs_UserProfile("PersonalPhoto")
                End If
                If rs_UserProfile("Notes") Is DBNull.Value Then
                    result.Notes = String.Empty
                Else
                    result.Notes = rs_UserProfile("Notes")
                End If
            End If
            rs_UserProfile.Close()
            rs_UserProfile = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetUserProfilesRelatedByUserId(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProfilesRelatedByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserProfilesRelated")
            conn.Close()
            Return result
        End Function

        Public Function GetUserProfilesByPartyA(ByVal PartyA As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProfilesByPartyA"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserProfilesByPartyA")
            conn.Close()
            Return result
        End Function

        Public Function GetUserProfilesAssignableByPartyA(ByVal PartyA As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProfilesAssignableByPartyA"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserProfilesAssignable")
            conn.Close()
            Return result
        End Function

        Public Function GetUserProfilesByPartyAType(ByVal PartyA As Long, ByVal Type As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProfilesByPartyAType"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@Type", Type)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserProfilesByPartyAType")
            conn.Close()
            Return result
        End Function

        Public Sub CreateUserRelationship(ByVal PartyA As Long, ByVal PartyB As Long, ByVal Type As Integer, ByVal Status As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserRelationship"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@Type", Type)
            m_SqlCommand.Parameters.AddWithValue("@Status", Status)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteUserRelationshipByPartyAPartyB(ByVal PartyA As Long, ByVal PartyB As Long, ByVal IsPhysicalDelete As Boolean)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteUserRelationshipByPartyAPartyB"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@IsPhysicalDelete", IsPhysicalDelete)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserRelationshipStatus(ByVal PartyA As Long, ByVal PartyB As Long, ByVal Status As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserRelationshipStatus"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@Status", Status)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserRelationshipType(ByVal PartyA As Long, ByVal PartyB As Long, ByVal Type As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserRelationshipType"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@Type", Type)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserRelationshipInvitationSentDate(ByVal PartyA As Long, ByVal PartyB As Long, ByVal InvitationSentDate As DateTime)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserRelationshipInvitationSentDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            If InvitationSentDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@InvitationSentDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@InvitationSentDate", InvitationSentDate)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserRelationshipInvitationAcceptDate(ByVal PartyA As Long, ByVal PartyB As Long, ByVal InvitationAcceptDate As DateTime)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserRelationshipInvitationAcceptDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            If InvitationAcceptDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@InvitationAcceptDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@InvitationAcceptDate", InvitationAcceptDate)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUserRelationshipByPartyAPartyB(ByVal PartyA As Long, ByVal PartyB As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserRelationshipByPartyAPartyB"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserRelationshipByPartyAPartyB")
            conn.Close()
            Return result
        End Function

        Public Function CreateUserNote(ByVal UserNote As UserNote) As Long
            Dim result As Long = 0
            Dim dsUserNote As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserNote"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If UserNote.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", UserNote.Description)
            End If
            If UserNote.NoteContent = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", UserNote.NoteContent)
            End If
            If UserNote.Owner = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Owner", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Owner", UserNote.Owner)
            End If
            If UserNote.Author = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Author", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Author", UserNote.Author)
            End If
            'If UserNote.ProjectStatusId = Nothing Then
            '    m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", DBNull.Value)
            'Else
            '    m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", UserNote.ProjectStatusId)
            'End If
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", UserNote.ProjectStatusId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserNote, "UserNote")
            conn.Close()
            If dsUserNote.Tables.Count > 0 Then
                If dsUserNote.Tables(0).Rows.Count > 0 Then
                    result = dsUserNote.Tables(0).Rows(0)("UserNoteId")
                End If
            End If
            Return result
        End Function

        Public Function CreateUserFile(ByVal UserFile As UserFile) As Long
            Dim result As Long = 0
            Dim dsUserFile As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserFile"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If UserFile.FileName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FileName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FileName", UserFile.FileName)
            End If
            If UserFile.FileExtension = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FileExtension", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FileExtension", UserFile.FileExtension)
            End If
            If UserFile.FileName_System = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FileName_System", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FileName_System", UserFile.FileName_System)
            End If
            If UserFile.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", UserFile.Description)
            End If
            If UserFile.Size = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Size", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Size", UserFile.Size)
            End If
            If UserFile.Owner = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Owner", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Owner", UserFile.Owner)
            End If
            If UserFile.Author = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Author", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Author", UserFile.Author)
            End If
            If UserFile.UserPhotoUploadFolder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserPhotoUploadFolder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserPhotoUploadFolder", UserFile.UserPhotoUploadFolder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserFile, "UserFile")
            conn.Close()
            If dsUserFile.Tables.Count > 0 Then
                If dsUserFile.Tables(0).Rows.Count > 0 Then
                    result = dsUserFile.Tables(0).Rows(0)("UserFileId")
                End If
            End If
            Return result
        End Function

        Public Sub DeleteUserFileByUserFileIDOwner(ByVal UserFileID As Long, ByVal Owner As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteUserFileByUserFileIDOwner"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserFileID", UserFileID)
            m_SqlCommand.Parameters.AddWithValue("@Owner", Owner)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserNote(ByVal UserNote As UserNote)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserNote"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserNoteID", UserNote.UserNoteId)
            If UserNote.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", UserNote.Description)
            End If
            If UserNote.NoteContent = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", UserNote.NoteContent)
            End If
            If UserNote.Owner = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Owner", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Owner", UserNote.Owner)
            End If
            If UserNote.Author = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Author", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Author", UserNote.Author)
            End If
            'If UserNote.ProjectStatusId = -1 Then
            '    m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", DBNull.Value)
            'Else
            '    m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", UserNote.ProjectStatusId)
            'End If
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", UserNote.ProjectStatusId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteUserNoteByUserNoteIDOwner(ByVal UserNoteId As Long, ByVal Owner As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteUserNoteByUserNoteIDOwner"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserNoteID", UserNoteId)
            m_SqlCommand.Parameters.AddWithValue("@Owner", Owner)

            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserFile(ByVal UserFile As UserFile)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserFile"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserUserFileID", UserFile.UserFileId)
            If UserFile.FileName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FileName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FileName", UserFile.FileName)
            End If
            If UserFile.FileExtension = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FileExtension", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FileExtension", UserFile.FileExtension)
            End If
            If UserFile.FileName_System = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FileName_System", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FileName_System", UserFile.FileName_System)
            End If
            If UserFile.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", UserFile.Description)
            End If
            If UserFile.Size = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Size", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Size", UserFile.Size)
            End If
            If UserFile.Owner = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Owner", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Owner", UserFile.Owner)
            End If
            If UserFile.Author = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Author", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Author", UserFile.Author)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUserFileByUserID(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserFileByUserID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserFileByUserID")
            conn.Close()
            Return result
        End Function

        Public Function GetUserFileByUserFileID(ByVal UserFileId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserFileByUserFileID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserFileId", UserFileId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserFileByUserFileID")
            conn.Close()
            Return result
        End Function

        Public Function GetUserNotesByUserIDProjectStatusID(ByVal UserId As Long, ByVal ProjectStatusId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserNotesByUserIDProjectStatusID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", ProjectStatusId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserNotesByUserIDProjectStatusID")
            conn.Close()
            Return result
        End Function

        Public Function GetUserNotesByUserIDUserProjectStatusValue(ByVal UserId As Long, ByVal UserProjectStatusValue As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserNotesByUserIDUserProjectStatusValue"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", UserProjectStatusValue)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserNotesByUserIDUserProjectStatusValue")
            conn.Close()
            Return result
        End Function

        Public Function GetUserNoteByUserID(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserNoteByUserID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserNoteByUserID")
            conn.Close()
            Return result
        End Function

        Public Function GetUserNoteByUserNoteID(ByVal UserNoteId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserNoteByUserNoteID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserNoteId", UserNoteId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserNoteByUserNoteID")
            conn.Close()
            Return result
        End Function

        Public Function CreateProject(ByVal Project As Project) As Long
            Dim result As Long = 0
            Dim dsProject As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateProject"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Project.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Project.Name)
            End If
            If Project.Address = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Address", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Address", Project.Address)
            End If
            If Project.PostCode = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Postcode", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Postcode", Project.PostCode)
            End If
            If Project.Suburb = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Suburb", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Suburb", Project.Suburb)
            End If
            If Project.SuburbID = 0 Then
                m_SqlCommand.Parameters.AddWithValue("@SuburbID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@SuburbID", Project.SuburbID)
            End If
            If Project.City = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@City", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@City", Project.City)
            End If
            If Project.CityID = 0 Then
                m_SqlCommand.Parameters.AddWithValue("@CityID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@CityID", Project.CityID)
            End If
            If Project.Region = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Region", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Region", Project.Region)
            End If
            If Project.RegionID = 0 Then
                m_SqlCommand.Parameters.AddWithValue("@RegionID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@RegionID", Project.RegionID)
            End If
            If Project.Country = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Country", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Country", Project.Country)
            End If
            If Project.CountryID = 0 Then
                m_SqlCommand.Parameters.AddWithValue("@CountryID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@CountryID", Project.CountryID)
            End If
            If Project.GroupID = 0 Then
                m_SqlCommand.Parameters.AddWithValue("@GroupID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GroupID", Project.GroupID)
            End If
            If Project.GroupName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GroupName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GroupName", Project.GroupName)
            End If
            If Project.StartDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@StartDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@StartDate", Project.StartDate)
            End If
            If Project.FinishDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FinishDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FinishDate", Project.FinishDate)
            End If
            If Project.DueDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DueDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DueDate", Project.DueDate)
            End If
            If Project.AssessmentDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AssessmentDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AssessmentDate", Project.AssessmentDate)
            End If
            If Project.Priority = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Priority", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Priority", Project.Priority)
            End If
            If Project.Hazard = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Hazard", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Hazard", Project.Hazard)
            End If
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", Project.ProjectStatusId)
            If Project.ContactId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContactId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContactId", Project.ContactId)
            End If
            If Project.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Project.ProjectOwnerId)
            End If
            If Project.EQCClaimNumber = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EQCClaimNumber", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EQCClaimNumber", Project.EQCClaimNumber)
            End If
            If Project.QuotationDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@QuotationDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@QuotationDate", Project.QuotationDate)
            End If
            If Project.EstimatedTime = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EstimatedTime", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EstimatedTime", Project.EstimatedTime)
            End If
            If Project.ScopeDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ScopeDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ScopeDate", Project.ScopeDate)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsProject, "Project")
            conn.Close()
            If dsProject.Tables.Count > 0 Then
                If dsProject.Tables(0).Rows.Count > 0 Then
                    result = dsProject.Tables(0).Rows(0)("ProjectId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateProject(ByVal Project As Project)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProject"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectID", Project.ProjectId)
            If Project.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Project.Name)
            End If
            If Project.Address = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Address", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Address", Project.Address)
            End If
            If Project.PostCode = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Postcode", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Postcode", Project.PostCode)
            End If
            If Project.Suburb = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Suburb", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Suburb", Project.Suburb)
            End If
            If Project.SuburbID = 0 Then
                m_SqlCommand.Parameters.AddWithValue("@SuburbID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@SuburbID", Project.SuburbID)
            End If
            If Project.City = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@City", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@City", Project.City)
            End If
            If Project.CityID = 0 Then
                m_SqlCommand.Parameters.AddWithValue("@CityID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@CityID", Project.CityID)
            End If
            If Project.Region = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Region", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Region", Project.Region)
            End If
            If Project.RegionID = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@RegionID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@RegionID", Project.RegionID)
            End If
            If Project.Country = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Country", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Country", Project.Country)
            End If
            If Project.CountryID = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@CountryID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@CountryID", Project.CountryID)
            End If
            If Project.GroupID = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GroupID", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GroupID", Project.GroupID)
            End If
            If Project.GroupName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GroupName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GroupName", Project.GroupName)
            End If
            If Project.StartDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@StartDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@StartDate", Project.StartDate)
            End If
            If Project.FinishDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@FinishDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@FinishDate", Project.FinishDate)
            End If
            If Project.DueDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DueDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DueDate", Project.DueDate)
            End If
            If Project.AssessmentDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AssessmentDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AssessmentDate", Project.AssessmentDate)
            End If
            If Project.Priority = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Priority", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Priority", Project.Priority)
            End If
            If Project.Hazard = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Hazard", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Hazard", Project.Hazard)
            End If
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", Project.ProjectStatusId)
            If Project.ContactId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContactId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContactId", Project.ContactId)
            End If
            If Project.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Project.ProjectOwnerId)
            End If
            If Project.EQCClaimNumber = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EQCClaimNumber", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EQCClaimNumber", Project.EQCClaimNumber)
            End If
            If Project.QuotationDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@QuotationDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@QuotationDate", Project.QuotationDate)
            End If
            If Project.EstimatedTime = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EstimatedTime", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EstimatedTime", Project.EstimatedTime)
            End If
            If Project.ScopeDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ScopeDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ScopeDate", Project.ScopeDate)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateProjectPriority(ByVal ProjectId As Long, ByVal Priority As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectPriority"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectID", ProjectId)
            If Priority = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Priority", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Priority", Priority)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateProjectProjectStatusId(ByVal ProjectId As Long, ByVal ProjectStatusId As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectProjectStatusId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectID", ProjectId)
            'If ProjectStatusId = Nothing Then
            'm_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", DBNull.Value)
            'Else
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", ProjectStatusId)
            'End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetProjectByProjectId(ByVal ProjectId As Long) As Project
            Dim result As Project = New Project()
            Dim rs_Project As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            rs_Project = m_SqlCommand.ExecuteReader
            If rs_Project.HasRows Then
                rs_Project.Read()
                If rs_Project("ProjectID") Is DBNull.Value Then
                    result.ProjectId = 0
                Else
                    result.ProjectId = rs_Project("ProjectID")
                End If
                If rs_Project("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_Project("Name")
                End If
                If rs_Project("Address") Is DBNull.Value Then
                    result.Address = String.Empty
                Else
                    result.Address = rs_Project("Address")
                End If
                If rs_Project("Postcode") Is DBNull.Value Then
                    result.PostCode = String.Empty
                Else
                    result.PostCode = rs_Project("Postcode")
                End If
                If rs_Project("Suburb") Is DBNull.Value Then
                    result.Suburb = String.Empty
                Else
                    result.Suburb = rs_Project("Suburb")
                End If
                If rs_Project("City") Is DBNull.Value Then
                    result.City = String.Empty
                Else
                    result.City = rs_Project("City")
                End If
                If rs_Project("Region") Is DBNull.Value Then
                    result.Region = String.Empty
                Else
                    result.Region = rs_Project("Region")
                End If
                If rs_Project("Country") Is DBNull.Value Then
                    result.Country = String.Empty
                Else
                    result.Country = rs_Project("Country")
                End If
                If rs_Project("GroupId") Is DBNull.Value Then
                    result.GroupID = 0
                Else
                    result.GroupID = rs_Project("GroupId")
                End If
                If rs_Project("GroupName") Is DBNull.Value Then
                    result.GroupName = String.Empty
                Else
                    result.GroupName = rs_Project("GroupName")
                End If
                If rs_Project("StartDate") Is DBNull.Value Then
                    result.StartDate = Nothing
                Else
                    result.StartDate = rs_Project("StartDate")
                End If
                If rs_Project("FinishDate") Is DBNull.Value Then
                    result.FinishDate = Nothing
                Else
                    result.FinishDate = rs_Project("FinishDate")
                End If
                If rs_Project("DueDate") Is DBNull.Value Then
                    result.DueDate = Nothing
                Else
                    result.DueDate = rs_Project("DueDate")
                End If
                If rs_Project("AssessmentDate") Is DBNull.Value Then
                    result.AssessmentDate = Nothing
                Else
                    result.AssessmentDate = rs_Project("AssessmentDate")
                End If
                If rs_Project("Priority") Is DBNull.Value Then
                    result.Priority = 0
                Else
                    result.Priority = rs_Project("Priority")
                End If
                If rs_Project("Hazard") Is DBNull.Value Then
                    result.Hazard = String.Empty
                Else
                    result.Hazard = rs_Project("Hazard")
                End If
                If rs_Project("ProjectStatusId") Is DBNull.Value Then
                    result.ProjectStatusId = 0
                Else
                    result.ProjectStatusId = rs_Project("ProjectStatusId")
                End If
                If rs_Project("ContactId") Is DBNull.Value Then
                    result.ContactId = 0
                Else
                    result.ContactId = rs_Project("ContactId")
                End If
                If rs_Project("ProjectOwnerId") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_Project("ProjectOwnerId")
                End If
                If rs_Project("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_Project("CreatedDate")
                End If
                If rs_Project("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_Project("ModifiedDate")
                End If
                If rs_Project("EQCClaimNumber") Is DBNull.Value Then
                    result.EQCClaimNumber = String.Empty
                Else
                    result.EQCClaimNumber = rs_Project("EQCClaimNumber")
                End If
                If rs_Project("QuotationDate") Is DBNull.Value Then
                    result.QuotationDate = Nothing
                Else
                    result.QuotationDate = rs_Project("QuotationDate")
                End If
                If rs_Project("EstimatedTime") Is DBNull.Value Then
                    result.EstimatedTime = String.Empty
                Else
                    result.EstimatedTime = rs_Project("EstimatedTime")
                End If
                If rs_Project("ScopeDate") Is DBNull.Value Then
                    result.ScopeDate = Nothing
                Else
                    result.ScopeDate = rs_Project("ScopeDate")
                End If
                If rs_Project("ProjectStatusName") Is DBNull.Value Then
                    result.ProjectStatusName = String.Empty
                Else
                    result.ProjectStatusName = rs_Project("ProjectStatusName")
                End If
                If rs_Project("ContactName") Is DBNull.Value Then
                    result.ContactName = String.Empty
                Else
                    result.ContactName = rs_Project("ContactName")
                End If
                If rs_Project("ProjectOwnerName") Is DBNull.Value Then
                    result.ProjectOwnerName = String.Empty
                Else
                    result.ProjectOwnerName = rs_Project("ProjectOwnerName")
                End If
            End If
            rs_Project.Close()
            rs_Project = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetProjectsByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsNotArchivedByProjectOwnerId(ByVal ProjectOwnerId As Long, ByVal NumberOfDaysToArchive As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsNotArchivedByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@NumberOfDaysToArchive", NumberOfDaysToArchive)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsNotArchivedByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsByProjectOwnerIdProjectStatusId(ByVal ProjectOwnerId As Long, ByVal NumberOfDaysToArchive As Integer, ByVal ProjectStatusID As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsByProjectOwnerIdProjectStatusId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@NumberOfDaysToArchive", NumberOfDaysToArchive)
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", ProjectStatusID)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsByProjectOwnerIdProjectStatusId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsArchivedByProjectOwnerId(ByVal ProjectOwnerId As Long, ByVal NumberOfDaysToArchive As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsArchivedByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@NumberOfDaysToArchive", NumberOfDaysToArchive)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsArchivedByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsNotArchivedByUserId(ByVal UserId As Long, ByVal NumberOfDaysToArchive As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsNotArchivedByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@NumberOfDaysToArchive", NumberOfDaysToArchive)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsNotArchivedByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsNotUserArchivedByUserId(ByVal UserId As Long, ByVal Keywords As String) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsNotUserArchivedByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@Keyword", Keywords)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsNotUserArchivedByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectGroupsNotUserArchivedByUserId(ByVal UserId As Long, ByVal Keywords As String, ByVal SearchType As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectGroupsNotUserArchivedByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@Keyword", Keywords)
            m_SqlCommand.Parameters.AddWithValue("@SearchType", SearchType)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectGroupsNotUserArchivedByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetGroupProjectsByUserId(ByVal UserId As Long, ByVal Keywords As String, ByVal SearchType As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetGroupProjectsByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@Keyword", Keywords)
            m_SqlCommand.Parameters.AddWithValue("@SearchType", SearchType)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "GroupProjectsByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetGroupProjectsNotUserArchivedByUserId(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetGroupProjectsNotUserArchivedByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "GroupProjectsNotUserArchivedByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsNotUserArchivedByUserIdGroupId(ByVal UserId As Long, ByVal GroupId As Long, ByVal Keywords As String, ByVal SearchType As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsNotUserArchivedByUserIdGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@GroupId", GroupId)
            m_SqlCommand.Parameters.AddWithValue("@Keyword", Keywords)
            m_SqlCommand.Parameters.AddWithValue("@SearchType", SearchType)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsNotUserArchivedByUserIdGroupId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsByUserIdProjectStatusId(ByVal UserId As Long, ByVal NumberOfDaysToArchive As Integer, ByVal ProjectStatusID As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsByUserIdProjectStatusId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@NumberOfDaysToArchive", NumberOfDaysToArchive)
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", ProjectStatusID)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsByUserIdProjectStatusId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectGroupsByUserIdUserProjectStatusValue(ByVal UserId As Long, ByVal Keywords As String, ByVal UserProjectStatusValue As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectGroupsByUserIdUserProjectStatusValue"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@Keyword", Keywords)
            m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", UserProjectStatusValue)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectGroupsByUserIdUserProjectStatusValue")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsByUserIdUserProjectStatusValue(ByVal UserId As Long, ByVal Keywords As String, ByVal UserProjectStatusValue As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsByUserIdUserProjectStatusValue"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@Keyword", Keywords)
            m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", UserProjectStatusValue)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsByUserIdUserProjectStatusValue")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsByUserIdGroupIdUserProjectStatusValue(ByVal UserId As Long, ByVal GroupId As Long, ByVal UserProjectStatusValue As Integer, ByVal Keywords As String) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsByUserIdGroupIdUserProjectStatusValue"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@GroupId", GroupId)
            m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", UserProjectStatusValue)
            m_SqlCommand.Parameters.AddWithValue("@Keyword", Keywords)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsByUserIdGroupIdUserProjectStatusValue")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsArchivedByUserId(ByVal UserId As Long, ByVal NumberOfDaysToArchive As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsArchivedByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@NumberOfDaysToArchive", NumberOfDaysToArchive)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsArchivedByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsUserArchivedByUserId(ByVal UserId As Long, ByVal Keywords As String) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsUserArchivedByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@Keyword", Keywords)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsUserArchivedByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectGroupsUserArchivedByUserId(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectGroupsUserArchivedByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectGroupsUserArchivedByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectsUserArchivedByUserIdGroupId(ByVal UserId As Long, ByVal GroupId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectsUserArchivedByUserIdGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@GroupId", GroupId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsUserArchivedByUserIdGroupId")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectInfoByProjectId(ByVal ProjectId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectsByProjectId")
            conn.Close()
            Return result
        End Function

        Public Function CreateProjectOwner(ByVal ProjectOwner As ProjectOwner) As Long
            Dim result As Long = 0
            Dim dsProjectOwner As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateProjectOwner"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If ProjectOwner.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", ProjectOwner.Name)
            End If
            If ProjectOwner.Address = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Address", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Address", ProjectOwner.Address)
            End If
            If ProjectOwner.PostCode = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Postcode", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Postcode", ProjectOwner.PostCode)
            End If
            If ProjectOwner.Suburb = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Suburb", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Suburb", ProjectOwner.Suburb)
            End If
            If ProjectOwner.City = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@City", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@City", ProjectOwner.City)
            End If
            If ProjectOwner.Region = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Region", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Region", ProjectOwner.Region)
            End If
            If ProjectOwner.Country = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Country", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Country", ProjectOwner.Country)
            End If
            If ProjectOwner.ContactId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContactId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContactId", ProjectOwner.ContactId)
            End If
            If ProjectOwner.Contact1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact1", ProjectOwner.Contact1)
            End If
            If ProjectOwner.Contact2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact2", ProjectOwner.Contact2)
            End If
            If ProjectOwner.Contact3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact3", ProjectOwner.Contact1)
            End If
            If ProjectOwner.Extension1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension1", ProjectOwner.Extension1)
            End If
            If ProjectOwner.Extension2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension2", ProjectOwner.Extension2)
            End If
            If ProjectOwner.Extension3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension3", ProjectOwner.Extension1)
            End If
            If ProjectOwner.Accreditation = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Accreditation", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Accreditation", ProjectOwner.Accreditation)
            End If
            If ProjectOwner.AccreditationNumber = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AccreditationNumber", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AccreditationNumber", ProjectOwner.AccreditationNumber)
            End If
            If ProjectOwner.GSTNumber = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GSTNumber", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GSTNumber", ProjectOwner.GSTNumber)
            End If
            If ProjectOwner.EQRSupervisor = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EQRSupervisor", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EQRSupervisor", ProjectOwner.EQRSupervisor)
            End If
            If ProjectOwner.Logo = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Logo", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Logo", ProjectOwner.Logo)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsProjectOwner, "ProjectOwner")
            conn.Close()
            If dsProjectOwner.Tables.Count > 0 Then
                If dsProjectOwner.Tables(0).Rows.Count > 0 Then
                    result = dsProjectOwner.Tables(0).Rows(0)("ProjectOwnerId")
                End If
            End If
            Return result
        End Function

        Public Function CreateProjectGroup(ByVal ProjectGroup As ProjectGroup) As Long
            Dim result As Long = 0
            Dim dsProjectOwner As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateProjectGroup"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If ProjectGroup.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", ProjectGroup.Name)
            End If
            If ProjectGroup.Email = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@email", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@email", ProjectGroup.Email)
            End If
            If ProjectGroup.Address = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Address", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Address", ProjectGroup.Address)
            End If
            If ProjectGroup.PostCode = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Postcode", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Postcode", ProjectGroup.PostCode)
            End If
            If ProjectGroup.Suburb = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Suburb", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Suburb", ProjectGroup.Suburb)
            End If
            If ProjectGroup.City = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@City", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@City", ProjectGroup.City)
            End If
            If ProjectGroup.Region = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Region", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Region", ProjectGroup.Region)
            End If
            If ProjectGroup.Country = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Country", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Country", ProjectGroup.Country)
            End If
            If ProjectGroup.Contact1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact1", ProjectGroup.Contact1)
            End If
            If ProjectGroup.Contact2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact2", ProjectGroup.Contact2)
            End If
            If ProjectGroup.Contact3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact3", ProjectGroup.Contact1)
            End If
            If ProjectGroup.Extension1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension1", ProjectGroup.Extension1)
            End If
            If ProjectGroup.Extension2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension2", ProjectGroup.Extension2)
            End If
            If ProjectGroup.Extension3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension3", ProjectGroup.Extension1)
            End If
            If ProjectGroup.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", ProjectGroup.DisplayOrder)
            End If
            If ProjectGroup.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", ProjectGroup.Disabled)
            End If
            If ProjectGroup.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectGroup.ProjectOwnerId)
            End If
            If ProjectGroup.Logo = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Logo", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Logo", ProjectGroup.Logo)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsProjectOwner, "ProjectGroup")
            conn.Close()
            If dsProjectOwner.Tables.Count > 0 Then
                If dsProjectOwner.Tables(0).Rows.Count > 0 Then
                    result = dsProjectOwner.Tables(0).Rows(0)("ProjectGroupId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateProjectOwner(ByVal ProjectOwner As ProjectOwner)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectOwner"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerID", ProjectOwner.ProjectOwnerId)
            If ProjectOwner.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", ProjectOwner.Name)
            End If
            If ProjectOwner.Address = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Address", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Address", ProjectOwner.Address)
            End If
            If ProjectOwner.PostCode = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Postcode", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Postcode", ProjectOwner.PostCode)
            End If
            If ProjectOwner.Suburb = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Suburb", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Suburb", ProjectOwner.Suburb)
            End If
            If ProjectOwner.City = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@City", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@City", ProjectOwner.City)
            End If
            If ProjectOwner.Region = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Region", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Region", ProjectOwner.Region)
            End If
            If ProjectOwner.Country = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Country", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Country", ProjectOwner.Country)
            End If
            If ProjectOwner.ContactId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContactId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContactId", ProjectOwner.ContactId)
            End If
            If ProjectOwner.Contact1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact1", ProjectOwner.Contact1)
            End If
            If ProjectOwner.Contact2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact2", ProjectOwner.Contact2)
            End If
            If ProjectOwner.Contact3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact3", ProjectOwner.Contact1)
            End If
            If ProjectOwner.Extension1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension1", ProjectOwner.Extension1)
            End If
            If ProjectOwner.Extension2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension2", ProjectOwner.Extension2)
            End If
            If ProjectOwner.Extension3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension3", ProjectOwner.Extension1)
            End If
            If ProjectOwner.Accreditation = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Accreditation", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Accreditation", ProjectOwner.Accreditation)
            End If
            If ProjectOwner.AccreditationNumber = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@AccreditationNumber", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@AccreditationNumber", ProjectOwner.AccreditationNumber)
            End If
            If ProjectOwner.GSTNumber = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@GSTNumber", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@GSTNumber", ProjectOwner.GSTNumber)
            End If
            If ProjectOwner.EQRSupervisor = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EQRSupervisor", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EQRSupervisor", ProjectOwner.EQRSupervisor)
            End If
            If ProjectOwner.Logo = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Logo", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Logo", ProjectOwner.Logo)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateProjectGroup(ByVal ProjectGroup As ProjectGroup)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectGroup"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectGroupID", ProjectGroup.ProjectGroupId)
            If ProjectGroup.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", ProjectGroup.Name)
            End If
            If ProjectGroup.Email = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Email", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Email", ProjectGroup.Email)
            End If
            If ProjectGroup.Address = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Address", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Address", ProjectGroup.Address)
            End If
            If ProjectGroup.PostCode = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Postcode", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Postcode", ProjectGroup.PostCode)
            End If
            If ProjectGroup.Suburb = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Suburb", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Suburb", ProjectGroup.Suburb)
            End If
            If ProjectGroup.City = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@City", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@City", ProjectGroup.City)
            End If
            If ProjectGroup.Region = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Region", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Region", ProjectGroup.Region)
            End If
            If ProjectGroup.Country = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Country", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Country", ProjectGroup.Country)
            End If
            If ProjectGroup.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectGroup.ProjectOwnerId)
            End If
            If ProjectGroup.Contact1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact1", ProjectGroup.Contact1)
            End If
            If ProjectGroup.Contact2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact2", ProjectGroup.Contact2)
            End If
            If ProjectGroup.Contact3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Contact3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Contact3", ProjectGroup.Contact1)
            End If
            If ProjectGroup.Extension1 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension1", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension1", ProjectGroup.Extension1)
            End If
            If ProjectGroup.Extension2 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension2", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension2", ProjectGroup.Extension2)
            End If
            If ProjectGroup.Extension3 = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Extension3", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Extension3", ProjectGroup.Extension1)
            End If
            If ProjectGroup.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", ProjectGroup.DisplayOrder)
            End If
            If ProjectGroup.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", ProjectGroup.Disabled)
            End If
            If ProjectGroup.Logo = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Logo", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Logo", ProjectGroup.Logo)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetProjectOwnerByProjectOwnerId(ByVal ProjectOwnerId As Long) As ProjectOwner
            Dim result As ProjectOwner = New ProjectOwner()
            Dim rs_ProjectOwner As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectOwnerByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            rs_ProjectOwner = m_SqlCommand.ExecuteReader
            If rs_ProjectOwner.HasRows Then
                rs_ProjectOwner.Read()
                If rs_ProjectOwner("ProjectOwnerID") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_ProjectOwner("ProjectOwnerID")
                End If
                If rs_ProjectOwner("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_ProjectOwner("Name")
                End If
                If rs_ProjectOwner("Address") Is DBNull.Value Then
                    result.Address = String.Empty
                Else
                    result.Address = rs_ProjectOwner("Address")
                End If
                If rs_ProjectOwner("Postcode") Is DBNull.Value Then
                    result.PostCode = String.Empty
                Else
                    result.PostCode = rs_ProjectOwner("Postcode")
                End If
                If rs_ProjectOwner("Suburb") Is DBNull.Value Then
                    result.Suburb = String.Empty
                Else
                    result.Suburb = rs_ProjectOwner("Suburb")
                End If
                If rs_ProjectOwner("City") Is DBNull.Value Then
                    result.City = String.Empty
                Else
                    result.City = rs_ProjectOwner("City")
                End If
                If rs_ProjectOwner("Region") Is DBNull.Value Then
                    result.Region = String.Empty
                Else
                    result.Region = rs_ProjectOwner("Region")
                End If
                If rs_ProjectOwner("Country") Is DBNull.Value Then
                    result.Country = String.Empty
                Else
                    result.Country = rs_ProjectOwner("Country")
                End If
                If rs_ProjectOwner("ContactId") Is DBNull.Value Then
                    result.ContactId = 0
                Else
                    result.ContactId = rs_ProjectOwner("ContactId")
                End If
                If rs_ProjectOwner("Contact1") Is DBNull.Value Then
                    result.Contact1 = String.Empty
                Else
                    result.Contact1 = rs_ProjectOwner("Contact1")
                End If
                If rs_ProjectOwner("Contact2") Is DBNull.Value Then
                    result.Contact2 = String.Empty
                Else
                    result.Contact2 = rs_ProjectOwner("Contact2")
                End If
                If rs_ProjectOwner("Contact3") Is DBNull.Value Then
                    result.Contact3 = String.Empty
                Else
                    result.Contact3 = rs_ProjectOwner("Contact3")
                End If
                If rs_ProjectOwner("Extension1") Is DBNull.Value Then
                    result.Extension1 = String.Empty
                Else
                    result.Extension1 = rs_ProjectOwner("Extension1")
                End If
                If rs_ProjectOwner("Extension2") Is DBNull.Value Then
                    result.Extension2 = String.Empty
                Else
                    result.Extension2 = rs_ProjectOwner("Extension2")
                End If
                If rs_ProjectOwner("Extension3") Is DBNull.Value Then
                    result.Extension3 = String.Empty
                Else
                    result.Extension3 = rs_ProjectOwner("Extension3")
                End If
                If rs_ProjectOwner("Identifier") Is DBNull.Value Then
                    result.Identifier = String.Empty
                Else
                    result.Identifier = rs_ProjectOwner("Identifier")
                End If
                If rs_ProjectOwner("Accreditation") Is DBNull.Value Then
                    result.Accreditation = String.Empty
                Else
                    result.Accreditation = rs_ProjectOwner("Accreditation")
                End If
                If rs_ProjectOwner("AccreditationNumber") Is DBNull.Value Then
                    result.AccreditationNumber = String.Empty
                Else
                    result.AccreditationNumber = rs_ProjectOwner("AccreditationNumber")
                End If
                If rs_ProjectOwner("GSTNumber") Is DBNull.Value Then
                    result.GSTNumber = String.Empty
                Else
                    result.GSTNumber = rs_ProjectOwner("GSTNumber")
                End If
                If rs_ProjectOwner("EQRSupervisor") Is DBNull.Value Then
                    result.EQRSupervisor = String.Empty
                Else
                    result.EQRSupervisor = rs_ProjectOwner("EQRSupervisor")
                End If
                If rs_ProjectOwner("Logo") Is DBNull.Value Then
                    result.Logo = String.Empty
                Else
                    result.Logo = rs_ProjectOwner("Logo")
                End If
                If rs_ProjectOwner("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_ProjectOwner("CreatedDate")
                End If
                If rs_ProjectOwner("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_ProjectOwner("ModifiedDate")
                End If
                If rs_ProjectOwner("ContactName") Is DBNull.Value Then
                    result.ContactName = String.Empty
                Else
                    result.ContactName = rs_ProjectOwner("ContactName")
                End If
                If rs_ProjectOwner("Frequency") Is DBNull.Value Then
                    result.Frequency = String.Empty
                Else
                    result.Frequency = rs_ProjectOwner("Frequency")
                End If
                If rs_ProjectOwner("PaymentStartDate") Is DBNull.Value Then
                    result.PaymentStartDate = Nothing
                Else
                    result.PaymentStartDate = rs_ProjectOwner("PaymentStartDate")
                End If
            End If
            rs_ProjectOwner.Close()
            rs_ProjectOwner = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetProjectGroupByProjectGroupId(ByVal ProjectGroupId As Long) As ProjectGroup
            Dim result As ProjectGroup = New ProjectGroup()
            Dim rs_ProjectGroup As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectGroupByProjectGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectGroupId", ProjectGroupId)
            conn.Open()
            rs_ProjectGroup = m_SqlCommand.ExecuteReader
            If rs_ProjectGroup.HasRows Then
                rs_ProjectGroup.Read()
                If rs_ProjectGroup("ProjectGroupID") Is DBNull.Value Then
                    result.ProjectGroupId = 0
                Else
                    result.ProjectGroupId = rs_ProjectGroup("ProjectGroupID")
                End If
                If rs_ProjectGroup("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_ProjectGroup("Name")
                End If
                If rs_ProjectGroup("Email") Is DBNull.Value Then
                    result.Email = String.Empty
                Else
                    result.Email = rs_ProjectGroup("Email")
                End If
                If rs_ProjectGroup("Address") Is DBNull.Value Then
                    result.Address = String.Empty
                Else
                    result.Address = rs_ProjectGroup("Address")
                End If
                If rs_ProjectGroup("Postcode") Is DBNull.Value Then
                    result.PostCode = String.Empty
                Else
                    result.PostCode = rs_ProjectGroup("Postcode")
                End If
                If rs_ProjectGroup("Suburb") Is DBNull.Value Then
                    result.Suburb = String.Empty
                Else
                    result.Suburb = rs_ProjectGroup("Suburb")
                End If
                If rs_ProjectGroup("City") Is DBNull.Value Then
                    result.City = String.Empty
                Else
                    result.City = rs_ProjectGroup("City")
                End If
                If rs_ProjectGroup("Region") Is DBNull.Value Then
                    result.Region = String.Empty
                Else
                    result.Region = rs_ProjectGroup("Region")
                End If
                If rs_ProjectGroup("Country") Is DBNull.Value Then
                    result.Country = String.Empty
                Else
                    result.Country = rs_ProjectGroup("Country")
                End If
                If rs_ProjectGroup("Contact1") Is DBNull.Value Then
                    result.Contact1 = String.Empty
                Else
                    result.Contact1 = rs_ProjectGroup("Contact1")
                End If
                If rs_ProjectGroup("Contact2") Is DBNull.Value Then
                    result.Contact2 = String.Empty
                Else
                    result.Contact2 = rs_ProjectGroup("Contact2")
                End If
                If rs_ProjectGroup("Contact3") Is DBNull.Value Then
                    result.Contact3 = String.Empty
                Else
                    result.Contact3 = rs_ProjectGroup("Contact3")
                End If
                If rs_ProjectGroup("Extension1") Is DBNull.Value Then
                    result.Extension1 = String.Empty
                Else
                    result.Extension1 = rs_ProjectGroup("Extension1")
                End If
                If rs_ProjectGroup("Extension2") Is DBNull.Value Then
                    result.Extension2 = String.Empty
                Else
                    result.Extension2 = rs_ProjectGroup("Extension2")
                End If
                If rs_ProjectGroup("Extension3") Is DBNull.Value Then
                    result.Extension3 = String.Empty
                Else
                    result.Extension3 = rs_ProjectGroup("Extension3")
                End If
                If rs_ProjectGroup("Identifier") Is DBNull.Value Then
                    result.Identifier = String.Empty
                Else
                    result.Identifier = rs_ProjectGroup("Identifier")
                End If
                If rs_ProjectGroup("Logo") Is DBNull.Value Then
                    result.Logo = String.Empty
                Else
                    result.Logo = rs_ProjectGroup("Logo")
                End If
                If rs_ProjectGroup("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_ProjectGroup("CreatedDate")
                End If
                If rs_ProjectGroup("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_ProjectGroup("ModifiedDate")
                End If
                If rs_ProjectGroup("ProjectOwnerId") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_ProjectGroup("ProjectOwnerId")
                End If
                If rs_ProjectGroup("Disabled") Is DBNull.Value Then
                    result.Disabled = False
                Else
                    result.Disabled = rs_ProjectGroup("Disabled")
                End If
                If rs_ProjectGroup("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_ProjectGroup("DisplayOrder")
                End If
            End If
            rs_ProjectGroup.Close()
            rs_ProjectGroup = Nothing
            conn.Close()

            Return result
        End Function

        Public Sub DeleteProjectGroupByProjectGroupId(ByVal ProjectGroupId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteProjectGroupByProjectGroupId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectGroupId", ProjectGroupId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetProjectOwnerByContactId(ByVal ContactId As Long) As ProjectOwner
            Dim result As ProjectOwner = New ProjectOwner()
            Dim rs_ProjectOwner As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectOwnerByContactId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ContactId", ContactId)
            conn.Open()
            rs_ProjectOwner = m_SqlCommand.ExecuteReader
            If rs_ProjectOwner.HasRows Then
                rs_ProjectOwner.Read()
                If rs_ProjectOwner("ProjectOwnerID") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_ProjectOwner("ProjectOwnerID")
                End If
                If rs_ProjectOwner("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_ProjectOwner("Name")
                End If
                If rs_ProjectOwner("Address") Is DBNull.Value Then
                    result.Address = String.Empty
                Else
                    result.Address = rs_ProjectOwner("Address")
                End If
                If rs_ProjectOwner("Postcode") Is DBNull.Value Then
                    result.PostCode = String.Empty
                Else
                    result.PostCode = rs_ProjectOwner("Postcode")
                End If
                If rs_ProjectOwner("Suburb") Is DBNull.Value Then
                    result.Suburb = String.Empty
                Else
                    result.Suburb = rs_ProjectOwner("Suburb")
                End If
                If rs_ProjectOwner("City") Is DBNull.Value Then
                    result.City = String.Empty
                Else
                    result.City = rs_ProjectOwner("City")
                End If
                If rs_ProjectOwner("Region") Is DBNull.Value Then
                    result.Region = String.Empty
                Else
                    result.Region = rs_ProjectOwner("Region")
                End If
                If rs_ProjectOwner("Country") Is DBNull.Value Then
                    result.Country = String.Empty
                Else
                    result.Country = rs_ProjectOwner("Country")
                End If
                If rs_ProjectOwner("ContactId") Is DBNull.Value Then
                    result.ContactId = 0
                Else
                    result.ContactId = rs_ProjectOwner("ContactId")
                End If
                If rs_ProjectOwner("Contact1") Is DBNull.Value Then
                    result.Contact1 = String.Empty
                Else
                    result.Contact1 = rs_ProjectOwner("Contact1")
                End If
                If rs_ProjectOwner("Contact2") Is DBNull.Value Then
                    result.Contact2 = String.Empty
                Else
                    result.Contact2 = rs_ProjectOwner("Contact2")
                End If
                If rs_ProjectOwner("Contact3") Is DBNull.Value Then
                    result.Contact3 = String.Empty
                Else
                    result.Contact3 = rs_ProjectOwner("Contact3")
                End If
                If rs_ProjectOwner("Extension1") Is DBNull.Value Then
                    result.Extension1 = String.Empty
                Else
                    result.Extension1 = rs_ProjectOwner("Extension1")
                End If
                If rs_ProjectOwner("Extension2") Is DBNull.Value Then
                    result.Extension2 = String.Empty
                Else
                    result.Extension2 = rs_ProjectOwner("Extension2")
                End If
                If rs_ProjectOwner("Extension3") Is DBNull.Value Then
                    result.Extension3 = String.Empty
                Else
                    result.Extension3 = rs_ProjectOwner("Extension3")
                End If
                If rs_ProjectOwner("Identifier") Is DBNull.Value Then
                    result.Identifier = String.Empty
                Else
                    result.Identifier = rs_ProjectOwner("Identifier")
                End If
                If rs_ProjectOwner("Accreditation") Is DBNull.Value Then
                    result.Accreditation = String.Empty
                Else
                    result.Accreditation = rs_ProjectOwner("Accreditation")
                End If
                If rs_ProjectOwner("EQRSupervisor") Is DBNull.Value Then
                    result.EQRSupervisor = String.Empty
                Else
                    result.EQRSupervisor = rs_ProjectOwner("EQRSupervisor")
                End If
                If rs_ProjectOwner("Logo") Is DBNull.Value Then
                    result.Logo = String.Empty
                Else
                    result.Logo = rs_ProjectOwner("Logo")
                End If
                If rs_ProjectOwner("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_ProjectOwner("CreatedDate")
                End If
                If rs_ProjectOwner("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_ProjectOwner("ModifiedDate")
                End If
                If rs_ProjectOwner("ContactName") Is DBNull.Value Then
                    result.ContactName = String.Empty
                Else
                    result.ContactName = rs_ProjectOwner("ContactName")
                End If
                If rs_ProjectOwner("Frequency") Is DBNull.Value Then
                    result.Frequency = String.Empty
                Else
                    result.Frequency = rs_ProjectOwner("Frequency")
                End If
                If rs_ProjectOwner("PaymentStartDate") Is DBNull.Value Then
                    result.PaymentStartDate = Nothing
                Else
                    result.PaymentStartDate = rs_ProjectOwner("PaymentStartDate")
                End If
            End If
            rs_ProjectOwner.Close()
            rs_ProjectOwner = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetProjectStatuses() As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectStatuses"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectStatuses")
            conn.Close()
            Return result
        End Function

        Public Sub UpdateProjectArchived(ByVal Project As Project)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectArchivedDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", Project.ProjectId)
            If Project.ArchivedDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ArchivedDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ArchivedDate", Project.ArchivedDate)
            End If

            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateProjectArchivedDateByUserId(ByVal Project As Project, ByVal UserId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectArchivedDateByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", Project.ProjectId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            If Project.ArchivedDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ArchivedDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ArchivedDate", Project.ArchivedDate)
            End If

            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateProjectDeactivated(ByVal Project As Project)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectDeactivatedDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", Project.ProjectId)
            If Project.DeactivatedDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DeactivatedDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DeactivatedDate", Project.DeactivatedDate)
            End If

            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub
        Public Function UpdateUserRelationshipTypeStatusToken(ByVal Type As Integer, ByVal Status As Integer, ByVal ActionToken As String) As Integer
            Dim result As Integer
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserRelationshipTypeStatusToken"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@Type", Type)
            m_SqlCommand.Parameters.AddWithValue("@Status", Status)
            m_SqlCommand.Parameters.AddWithValue("@ActionToken", ActionToken)
            conn.Open()
            'm_SqlCommand.ExecuteNonQuery()
            result = m_SqlCommand.ExecuteScalar()
            conn.Close()

            Return result
        End Function

        Public Sub UpdateUserRelationshipActionToken(ByVal PartyA As Long, ByVal PartyB As Long, ByVal ActionToken As String)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserRelationshipActionToken"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@actionToken", ActionToken)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserLinkID(ByVal UserID As Long, ByVal LinkID As String)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserLinkID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", UserID)
            m_SqlCommand.Parameters.AddWithValue("@LinkID", LinkID)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserRelationshipLinkID(ByVal PartyA As Long, ByVal PartyB As Long, ByVal LinkID As String)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserRelationshipLinkID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@LinkID", LinkID)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserProfileIdentifier(ByVal UserProfile As UserProfile)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserProfileIdentifier"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserProfileID", UserProfile.UserProfileId)
            m_SqlCommand.Parameters.AddWithValue("@Identifier", UserProfile.Identifier)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateProjectOwnerIdentifier(ByVal ProjectOwner As ProjectOwner)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectOwnerIdentifier"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerID", ProjectOwner.ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@Identifier", ProjectOwner.Identifier)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateProjectOwnerPaymentSetting(ByVal ProjectOwner As ProjectOwner)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectOwnerPaymentSetting"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerID", ProjectOwner.ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@Frequency", ProjectOwner.Frequency)
            m_SqlCommand.Parameters.AddWithValue("@PaymentStartDate", ProjectOwner.PaymentStartDate)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateProjectGroupIdentifier(ByVal ProjectGroup As ProjectGroup)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateProjectGroupIdentifier"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectGroupID", ProjectGroup.ProjectGroupId)
            m_SqlCommand.Parameters.AddWithValue("@Identifier", ProjectGroup.Identifier)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetProjectStatusesByProjectIdUserId(ByVal ProjectId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectStatusesByProjectIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectStatusesByProjectIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetUserProjectStatusesByProjectIdUserId(ByVal ProjectId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProjectStatusesByProjectIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserProjectStatusesByProjectIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function CreateUserProjectStatusSetting(ByVal UserProjectStatusSetting As UserProjectStatusSetting) As Long
            Dim result As Long = 0
            Dim dsUserProjectStatusSetting As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserProjectStatusSetting"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If UserProjectStatusSetting.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", UserProjectStatusSetting.UserId)
            End If
            If UserProjectStatusSetting.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", UserProjectStatusSetting.ProjectId)
            End If
            If UserProjectStatusSetting.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", UserProjectStatusSetting.Name)
            End If
            If UserProjectStatusSetting.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", UserProjectStatusSetting.DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserProjectStatusSetting, "UserProjectStatusSetting")
            conn.Close()
            If dsUserProjectStatusSetting.Tables.Count > 0 Then
                If dsUserProjectStatusSetting.Tables(0).Rows.Count > 0 Then
                    result = dsUserProjectStatusSetting.Tables(0).Rows(0)("UserProjectStatusSettingId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateUserProjectStatusSetting(ByVal UserProjectStatusSetting As UserProjectStatusSetting)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserProjectStatusSetting"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusSettingId", UserProjectStatusSetting.UserProjectStatusSettingId)
            If UserProjectStatusSetting.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", UserProjectStatusSetting.UserId)
            End If
            If UserProjectStatusSetting.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", UserProjectStatusSetting.ProjectId)
            End If
            If UserProjectStatusSetting.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", UserProjectStatusSetting.Name)
            End If
            If UserProjectStatusSetting.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", UserProjectStatusSetting.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUserProjectStatusSettingByUserProjectStatusSettingId(ByVal UserProjectStatusSettingId As Long) As UserProjectStatusSetting
            Dim result As UserProjectStatusSetting = New UserProjectStatusSetting()
            Dim rs_UserProjectStatusSetting As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProjectStatusSettingByUserProjectStatusSettingId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusSettingId", UserProjectStatusSettingId)
            conn.Open()
            rs_UserProjectStatusSetting = m_SqlCommand.ExecuteReader
            If rs_UserProjectStatusSetting.HasRows Then
                rs_UserProjectStatusSetting.Read()
                If rs_UserProjectStatusSetting("UserProjectStatusSettingId") Is DBNull.Value Then
                    result.UserProjectStatusSettingId = 0
                Else
                    result.UserProjectStatusSettingId = rs_UserProjectStatusSetting("UserProjectStatusSettingId")
                End If
                If rs_UserProjectStatusSetting("UserId") Is DBNull.Value Then
                    result.UserId = 0
                Else
                    result.UserId = rs_UserProjectStatusSetting("UserId")
                End If
                If rs_UserProjectStatusSetting("ProjectId") Is DBNull.Value Then
                    result.ProjectId = 0
                Else
                    result.ProjectId = rs_UserProjectStatusSetting("ProjectId")
                End If
                If rs_UserProjectStatusSetting("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_UserProjectStatusSetting("Name")
                End If
                If rs_UserProjectStatusSetting("Description") Is DBNull.Value Then
                    result.Description = String.Empty
                Else
                    result.Description = rs_UserProjectStatusSetting("Description")
                End If
                If rs_UserProjectStatusSetting("StatusValue") Is DBNull.Value Then
                    result.StatusValue = 0
                Else
                    result.StatusValue = rs_UserProjectStatusSetting("StatusValue")
                End If
                If rs_UserProjectStatusSetting("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_UserProjectStatusSetting("DisplayOrder")
                End If
            End If
            rs_UserProjectStatusSetting.Close()
            rs_UserProjectStatusSetting = Nothing
            conn.Close()

            Return result
        End Function

        Public Sub DeleteUserProjectStatusSettingByUserProjectStatusSettingId(ByVal UserProjectStatusSettingId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteUserProjectStatusSettingByUserProjectStatusSettingId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusSettingId", UserProjectStatusSettingId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateUserProjectStatusValue(ByVal UserProjectStatusValue As UserProjectStatusValue) As Long
            Dim result As Long = 0
            Dim dsUserProjectStatusValue As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserProjectStatusValue"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If UserProjectStatusValue.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", UserProjectStatusValue.UserId)
            End If
            If UserProjectStatusValue.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", UserProjectStatusValue.ProjectId)
            End If
            If UserProjectStatusValue.UserProjectStatusValue = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", UserProjectStatusValue.UserProjectStatusValue)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserProjectStatusValue, "UserProjectStatusValue")
            conn.Close()
            If dsUserProjectStatusValue.Tables.Count > 0 Then
                If dsUserProjectStatusValue.Tables(0).Rows.Count > 0 Then
                    result = dsUserProjectStatusValue.Tables(0).Rows(0)("UserProjectStatusValueId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateUserProjectStatusValueByProjectIdUserId(ByVal UserProjectStatusValue As UserProjectStatusValue)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserProjectStatusValueByProjectIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure

            If UserProjectStatusValue.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", UserProjectStatusValue.UserId)
            End If
            If UserProjectStatusValue.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", UserProjectStatusValue.ProjectId)
            End If
            If UserProjectStatusValue.UserProjectStatusValue = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", UserProjectStatusValue.UserProjectStatusValue)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUserProjectStatusValueByProjectIdUserId(ByVal ProjectId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProjectStatusValueByProjectIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserProjectStatusValueByProjectIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function CreatePlan(ByVal Name As String, ByVal Description As String, ByVal NumberOfProjects As Integer, ByVal Price As Decimal, ByVal DisplayOrder As Integer) As Integer
            Dim result As Long = 0
            Dim dsPlan As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreatePlan"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Name)
            End If
            If Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Description)
            End If
            If NumberOfProjects = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@NumberOfProjects", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@NumberOfProjects", NumberOfProjects)
            End If
            If Price = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Price", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Price", Price)
            End If
            If DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", 0)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsPlan, "Plan")
            conn.Close()
            If dsPlan.Tables.Count > 0 Then
                If dsPlan.Tables(0).Rows.Count > 0 Then
                    result = dsPlan.Tables(0).Rows(0)("PlanId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdatePlan(ByVal PlanId As Integer, ByVal Name As String, ByVal Description As String, ByVal NumberOfProjects As Integer, ByVal Price As Decimal, ByVal DisplayOrder As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdatePlan"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PlanId", PlanId)
            If Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", Name)
            End If
            If Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Description)
            End If
            If NumberOfProjects = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@NumberOfProjects", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@NumberOfProjects", NumberOfProjects)
            End If
            If Price = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Price", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Price", Price)
            End If
            If DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", 0)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetPlans(ByVal IsFree As Boolean) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetPlans"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@IsFree", IsFree)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "Plans")
            conn.Close()
            Return result
        End Function

        Public Function GetMonthlyPlans(ByVal ExistedProjectsCount As Integer, ByVal CurrentPlanId As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetMonthlyPlans"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ExistedProjectsCount", ExistedProjectsCount)
            m_SqlCommand.Parameters.AddWithValue("@CurrentPlanId", CurrentPlanId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "MonthlyPlans")
            conn.Close()
            Return result
        End Function

        Public Function GetAllPlans() As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetAllPlans"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "AllPlans")
            conn.Close()
            Return result
        End Function

        Public Sub DeletePlanByPlanId(ByVal PlanId As Integer, ByVal IsPhysicalDelete As Boolean)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeletePlanByPlanId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PlanId", PlanId)
            m_SqlCommand.Parameters.AddWithValue("@IsPhysicalDelete", IsPhysicalDelete)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateUserAccount(ByVal UserId As Long, ByVal ProjectCredit As Integer) As Long
            Dim result As Long = 0
            Dim dsUserAccount As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserAccount"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", UserId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectCredit", ProjectCredit)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserAccount, "UserAccount")
            conn.Close()
            If dsUserAccount.Tables.Count > 0 Then
                If dsUserAccount.Tables(0).Rows.Count > 0 Then
                    result = dsUserAccount.Tables(0).Rows(0)("UserAccountId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateUserAccount(ByVal UserId As Long, ByVal ProjectCredit As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserAccount"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", UserId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectCredit", ProjectCredit)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateUserAccountMonthly(ByVal UserId As Long, ByVal PlanId As Integer, ByVal NumberOfProjects As Integer, ByVal StorageSize As Decimal, ByVal NextBillingDate As DateTime)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserAccountMonthly"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", UserId)
            m_SqlCommand.Parameters.AddWithValue("@PlanID", PlanId)
            m_SqlCommand.Parameters.AddWithValue("@NumberOfProjects", NumberOfProjects)
            m_SqlCommand.Parameters.AddWithValue("@StorageSize", StorageSize)
            m_SqlCommand.Parameters.AddWithValue("@NextBillingDate", NextBillingDate)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUserAccountByUserID(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserAccountByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserAccountByUserID")
            conn.Close()
            Return result
        End Function

        Public Sub CreateUserTransaction(ByVal UserId As Long, ByVal Description As String, ByVal CreditAmount As Decimal, ByVal DebitAmount As Decimal, ByVal NumberOfProjectCredits As Integer, ByVal ProjectCreditBalance As Integer)
            Dim result As Long = 0
            Dim dsUserProfile As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserTransaction"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", UserId)
            m_SqlCommand.Parameters.AddWithValue("@Description", Description)
            m_SqlCommand.Parameters.AddWithValue("@CreditAmount", CreditAmount)
            m_SqlCommand.Parameters.AddWithValue("@DebitAmount", DebitAmount)
            m_SqlCommand.Parameters.AddWithValue("@NumberOfProjectCredits", NumberOfProjectCredits)
            m_SqlCommand.Parameters.AddWithValue("@ProjectCreditBalance", ProjectCreditBalance)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUserTransactionsByUserId(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserTransactionsByUserID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserTransactionsByUserID")
            conn.Close()
            Return result
        End Function

        Public Function GetUserTransactionByUserTransactionId(ByVal UserTransactionId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserTransactionByUserTransactionId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserTransactionId", UserTransactionId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserTransactionByUserTransactionId")
            conn.Close()
            Return result
        End Function

        Public Sub UpdateUserAccountMonthlyPaymentInfo_ebizsecure(ByVal UserId As Long, ByVal doc As XmlDocument)
            If UserId > 0 Then
                Dim xpath_PaymentInfo As String = "PaymentInfo"
                Dim ns As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
                Dim nodes As XmlNodeList = doc.SelectNodes(xpath_PaymentInfo, ns)
                Dim node As XmlNode = nodes(0)
                If node.HasChildNodes Then
                    Dim strCreditCardNumber As String = node("CreditCardNumber").InnerText
                    Dim strCreditCardExpiry As String = node("CreditCardExpiry").InnerText
                    Dim strInitialBillingDate As String = node("InitialBillingDate").InnerText
                    Dim strCurrentBillingDate As String = node("CurrentBillingDate").InnerText
                    Dim strNextBillingDate As String = node("NextBillingDate").InnerText
                    Dim strReferenceId As String = node("ReferenceID").InnerText
                    Dim strRecurringReferenceId As String = node("RecurringReferenceID").InnerText

                    Dim conn As SqlConnection = m_SQLConn.conn()
                    m_SqlCommand = New SqlCommand(m_SQL, conn)
                    m_SqlCommand.CommandText = "UpdateUserAccountMonthlyPaymentInfo"
                    m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
                    m_SqlCommand.Parameters.AddWithValue("@UserID", UserId)
                    If strCreditCardNumber = String.Empty Then
                        m_SqlCommand.Parameters.AddWithValue("@PartialCreditCardNumber", DBNull.Value)
                    Else
                        m_SqlCommand.Parameters.AddWithValue("@PartialCreditCardNumber", strCreditCardNumber)
                    End If
                    If strCreditCardExpiry = String.Empty Then
                        m_SqlCommand.Parameters.AddWithValue("@ValidThru", DBNull.Value)
                    Else
                        m_SqlCommand.Parameters.AddWithValue("@ValidThru", strCreditCardExpiry)
                    End If
                    If strCurrentBillingDate = String.Empty Then
                        m_SqlCommand.Parameters.AddWithValue("@LastBillingDate", CDate(strInitialBillingDate))
                    Else
                        m_SqlCommand.Parameters.AddWithValue("@LastBillingDate", CDate(strCurrentBillingDate))
                    End If
                    If strNextBillingDate = String.Empty Then
                        m_SqlCommand.Parameters.AddWithValue("@NextBillingDate", DBNull.Value)
                    Else
                        m_SqlCommand.Parameters.AddWithValue("@NextBillingDate", CDate(strNextBillingDate))
                    End If
                    If strReferenceId = String.Empty Then
                        m_SqlCommand.Parameters.AddWithValue("@ReferenceId", DBNull.Value)
                    Else
                        m_SqlCommand.Parameters.AddWithValue("@ReferenceId", strReferenceId)
                    End If
                    If strRecurringReferenceId = String.Empty Then
                        m_SqlCommand.Parameters.AddWithValue("@RecurringReferenceId", DBNull.Value)
                    Else
                        m_SqlCommand.Parameters.AddWithValue("@RecurringReferenceId", strRecurringReferenceId)
                    End If
                    conn.Open()
                    m_SqlCommand.ExecuteNonQuery()
                    conn.Close()
                End If
            End If
        End Sub

        Public Sub UpdateUserAccountMonthlyCancelSubscription(ByVal UserId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserAccountMonthlyCancelSubscription"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserID", UserId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetUnarchivedProjectsCountByUserId(ByVal UserId As Long) As Integer
            Dim result As Integer = 0
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUnarchivedProjectsCountByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            result = m_SqlCommand.ExecuteScalar()
            conn.Close()
            Return result
        End Function

        Public Function GetUnarchivedProjectsByUserId(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUnarchivedProjectsByUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UnarchivedProjectsByUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetPlanByPromoCodeUserId(ByVal PromoCode As String, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetPlanByPromoCodeUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PromoCode", PromoCode)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "PromoPlan")
            conn.Close()
            Return result
        End Function

        Public Function GetPlanByPlanId(ByVal PlanId As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetPlanByPlanId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PlanId", PlanId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "Plan")
            conn.Close()
            Return result
        End Function

        Public Function CheckPromoCodeValidByPromoCodeUserId(ByVal PromoCode As String, ByVal UserId As Long) As Boolean
            Dim result As Boolean = False
            Dim dsUnarchivedProjectsCount As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CheckPromoCodeValidByPromoCodeUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PromoCode", PromoCode)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            result = m_SqlCommand.ExecuteScalar()
            conn.Close()
            Return result
        End Function

        Public Sub CreatePromotionRedeemed(ByVal PromoCode As String, ByVal UserId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreatePromotionRedeemed"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PromoCode", PromoCode)
            m_SqlCommand.Parameters.AddWithValue("@UserID", UserId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateTradeNote(ByVal TradeNote As UserNote) As Long
            Dim result As Long = 0
            Dim dsTradeNote As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateTradeNote"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If TradeNote.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", TradeNote.Description)
            End If
            If TradeNote.NoteContent = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", TradeNote.NoteContent)
            End If
            If TradeNote.Owner = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Owner", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Owner", TradeNote.Owner)
            End If
            If TradeNote.Author = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Author", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Author", TradeNote.Author)
            End If
            'If UserNote.ProjectStatusId = Nothing Then
            '    m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", DBNull.Value)
            'Else
            '    m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", UserNote.ProjectStatusId)
            'End If
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", TradeNote.ProjectStatusId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsTradeNote, "TradeNote")
            conn.Close()
            If dsTradeNote.Tables.Count > 0 Then
                If dsTradeNote.Tables(0).Rows.Count > 0 Then
                    result = dsTradeNote.Tables(0).Rows(0)("UserNoteId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateTradeNote(ByVal TradeNote As UserNote)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateTradeNote"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserNoteID", TradeNote.UserNoteId)
            If TradeNote.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", TradeNote.Description)
            End If
            If TradeNote.NoteContent = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", TradeNote.NoteContent)
            End If
            If TradeNote.Owner = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Owner", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Owner", TradeNote.Owner)
            End If
            If TradeNote.Author = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Author", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Author", TradeNote.Author)
            End If
            'If UserNote.ProjectStatusId = -1 Then
            '    m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", DBNull.Value)
            'Else
            '    m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", UserNote.ProjectStatusId)
            'End If
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", TradeNote.ProjectStatusId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteTradeNoteByUserNoteIDOwner(ByVal UserNoteId As Long, ByVal Owner As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteTradeNoteByUserNoteIDOwner"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserNoteID", UserNoteId)
            m_SqlCommand.Parameters.AddWithValue("@Owner", Owner)

            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetTradeNotesByUserIDProjectStatusID(ByVal UserId As Long, ByVal ProjectStatusId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTradeNotesByUserIDProjectStatusID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@ProjectStatusId", ProjectStatusId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "TradeNotesByUserIDProjectStatusID")
            conn.Close()
            Return result
        End Function

        Public Function GetTradeNotesByUserIDUserProjectStatusValue(ByVal UserId As Long, ByVal UserProjectStatusValue As Integer) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTradeNotesByUserIDUserProjectStatusValue"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@UserProjectStatusValue", UserProjectStatusValue)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "TradeNotesByUserIDUserProjectStatusValue")
            conn.Close()
            Return result
        End Function

        Public Function GetTradeNoteByUserID(ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTradeNoteByUserID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "TradeNoteByUserID")
            conn.Close()
            Return result
        End Function

        Public Function GetTradeNoteByUserNoteID(ByVal UserNoteId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTradeNoteByUserNoteID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserNoteId", UserNoteId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "TradeNoteByUserNoteID")
            conn.Close()
            Return result
        End Function

        Public Function CreateDynamicPageContent(ByVal DynamicPageContent As DynamicPageContent) As Long
            Dim result As Long = 0
            Dim dsDynamicPageContent As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateDynamicPageContent"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If DynamicPageContent.ContentTypeId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContentTypeId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContentTypeId", DynamicPageContent.ContentTypeId)
            End If
            If DynamicPageContent.DynamicPageId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DynamicPageId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DynamicPageId", DynamicPageContent.DynamicPageId)
            End If
            If DynamicPageContent.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DynamicPageContent.ProjectOwnerId)
            End If
            If DynamicPageContent.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DynamicPageContent.ProjectId)
            End If
            If DynamicPageContent.ContentTitle = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContentTitle", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContentTitle", DynamicPageContent.ContentTitle)
            End If
            If DynamicPageContent.ContentData = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContentData", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContentData", DynamicPageContent.ContentData)
            End If
            If DynamicPageContent.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DynamicPageContent.Disabled)
            End If
            If DynamicPageContent.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DynamicPageContent.DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsDynamicPageContent, "DynamicPageContent")
            conn.Close()
            If dsDynamicPageContent.Tables.Count > 0 Then
                If dsDynamicPageContent.Tables(0).Rows.Count > 0 Then
                    result = dsDynamicPageContent.Tables(0).Rows(0)("DynamicPageContentId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateDynamicPageContent(ByVal DynamicPageContent As DynamicPageContent)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateDynamicPageContent"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@DynamicPageContentId", DynamicPageContent.DynamicPageContentId)
            If DynamicPageContent.ContentTypeId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContentTypeId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContentTypeId", DynamicPageContent.ContentTypeId)
            End If
            If DynamicPageContent.DynamicPageId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DynamicPageId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DynamicPageId", DynamicPageContent.DynamicPageId)
            End If
            If DynamicPageContent.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DynamicPageContent.ProjectOwnerId)
            End If
            If DynamicPageContent.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DynamicPageContent.ProjectId)
            End If
            If DynamicPageContent.ContentTitle = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContentTitle", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContentTitle", DynamicPageContent.ContentTitle)
            End If
            If DynamicPageContent.ContentData = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ContentData", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ContentData", DynamicPageContent.ContentData)
            End If
            If DynamicPageContent.Disabled = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Disabled", DynamicPageContent.Disabled)
            End If
            If DynamicPageContent.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DynamicPageContent.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetDynamicPageContentByDynamicPageContentId(ByVal DynamicPageContentId As Long) As DynamicPageContent
            Dim result As DynamicPageContent = New DynamicPageContent()
            Dim rs_DynamicPageContent As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetDynamicPageContentByDynamicPageContentId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@DynamicPageContentId", DynamicPageContentId)
            conn.Open()
            rs_DynamicPageContent = m_SqlCommand.ExecuteReader
            If rs_DynamicPageContent.HasRows Then
                rs_DynamicPageContent.Read()
                If rs_DynamicPageContent("DynamicPageContentId") Is DBNull.Value Then
                    result.DynamicPageContentId = 0
                Else
                    result.DynamicPageContentId = rs_DynamicPageContent("DynamicPageContentId")
                End If
                If rs_DynamicPageContent("ContentTypeId") Is DBNull.Value Then
                    result.ContentTypeId = 0
                Else
                    result.ContentTypeId = rs_DynamicPageContent("ContentTypeId")
                End If
                If rs_DynamicPageContent("DynamicPageId") Is DBNull.Value Then
                    result.DynamicPageId = 0
                Else
                    result.DynamicPageId = rs_DynamicPageContent("DynamicPageId")
                End If
                If rs_DynamicPageContent("ProjectOwnerId") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_DynamicPageContent("ProjectOwnerId")
                End If
                If rs_DynamicPageContent("ProjectId") Is DBNull.Value Then
                    result.ProjectId = 0
                Else
                    result.ProjectId = rs_DynamicPageContent("ProjectId")
                End If
                If rs_DynamicPageContent("ContentTitle") Is DBNull.Value Then
                    result.ContentTitle = String.Empty
                Else
                    result.ContentTitle = rs_DynamicPageContent("ContentTitle")
                End If
                If rs_DynamicPageContent("ContentData") Is DBNull.Value Then
                    result.ContentData = String.Empty
                Else
                    result.ContentData = rs_DynamicPageContent("ContentData")
                End If
                If rs_DynamicPageContent("Disabled") Is DBNull.Value Then
                    result.Disabled = False
                Else
                    result.Disabled = rs_DynamicPageContent("Disabled")
                End If
                If rs_DynamicPageContent("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = Nothing
                Else
                    result.DisplayOrder = rs_DynamicPageContent("DisplayOrder")
                End If
                If rs_DynamicPageContent("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_DynamicPageContent("CreatedDate")
                End If
                If rs_DynamicPageContent("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_DynamicPageContent("ModifiedDate")
                End If
                If rs_DynamicPageContent("ContentTypeName") Is DBNull.Value Then
                    result.ContentTypeName = 0
                Else
                    result.ContentTypeName = rs_DynamicPageContent("ContentTypeName")
                End If
            End If
            rs_DynamicPageContent.Close()
            rs_DynamicPageContent = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetDynamicPageContentsByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetDynamicPageContentsByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "DynamicPageContentsByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetDynamicPageContentsByProjectId(ByVal ProjectId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetDynamicPageContentsByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "DynamicPageContentsByProjectId")
            conn.Close()
            Return result
        End Function

        Public Function GetDynamicPageContentsFileByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetDynamicPageContentsFileByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "DynamicPageContentsFileByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetDynamicPageContentsFileByProjectId(ByVal ProjectId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetDynamicPageContentsFileByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "DynamicPageContentsFileByProjectId")
            conn.Close()
            Return result
        End Function

        Public Function GetDynamicPageContentsNotFileByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetDynamicPageContentsNotFileByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "DynamicPageContentsNotFileByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetDynamicPageContentsNotFileByProjectId(ByVal ProjectId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetDynamicPageContentsNotFileByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "DynamicPageContentsNotFileByProjectId")
            conn.Close()
            Return result
        End Function

        Public Sub DeleteDynamicPageContentByDynamicPageContentId(ByVal DynamicPageContentId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteDynamicPageContentByDynamicPageContentId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@DynamicPageContentId", DynamicPageContentId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateReminder(ByVal Reminder As Reminder) As Long
            Dim result As Long = 0
            Dim dsReminder As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateReminder"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Reminder.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Reminder.ProjectOwnerId)
            End If
            If Reminder.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", Reminder.ProjectId)
            End If
            If Reminder.ReminderTitle = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ReminderTitle", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ReminderTitle", Reminder.ReminderTitle)
            End If
            If Reminder.ReminderContentData = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ReminderContentData", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ReminderContentData", Reminder.ReminderContentData)
            End If
            If Reminder.Status = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Status", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Status", Reminder.Status)
            End If
            If Reminder.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Reminder.DisplayOrder)
            End If
            If Reminder.StartDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@StartDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@StartDate", Reminder.StartDate)
            End If
            If Reminder.EndDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EndDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EndDate", Reminder.EndDate)
            End If
            m_SqlCommand.Parameters.AddWithValue("@EmailTimeSetting", Reminder.EmailTimeSetting)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsReminder, "Reminder")
            conn.Close()
            If dsReminder.Tables.Count > 0 Then
                If dsReminder.Tables(0).Rows.Count > 0 Then
                    result = dsReminder.Tables(0).Rows(0)("ReminderId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateReminder(ByVal Reminder As Reminder)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateReminder"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ReminderId", Reminder.ReminderId)
            If Reminder.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Reminder.ProjectOwnerId)
            End If
            If Reminder.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", Reminder.ProjectId)
            End If
            If Reminder.ReminderTitle = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ReminderTitle", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ReminderTitle", Reminder.ReminderTitle)
            End If
            If Reminder.ReminderContentData = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ReminderContentData", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ReminderContentData", Reminder.ReminderContentData)
            End If
            If Reminder.Status = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Status", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Status", Reminder.Status)
            End If
            If Reminder.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", Reminder.DisplayOrder)
            End If
            If Reminder.StartDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@StartDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@StartDate", Reminder.StartDate)
            End If
            If Reminder.EndDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EndDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EndDate", Reminder.EndDate)
            End If
            m_SqlCommand.Parameters.AddWithValue("@EmailTimeSetting", Reminder.EmailTimeSetting)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateReminderEmailReminderSentByReminderId(ByVal ReminderId As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateReminderEmailReminderSentByReminderId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ReminderId", ReminderId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetReminderByReminderId(ByVal ReminderId As Long) As Reminder
            Dim result As Reminder = New Reminder()
            Dim rs_Reminder As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetReminderByReminderId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ReminderId", ReminderId)
            conn.Open()
            rs_Reminder = m_SqlCommand.ExecuteReader
            If rs_Reminder.HasRows Then
                rs_Reminder.Read()
                If rs_Reminder("ReminderId") Is DBNull.Value Then
                    result.ReminderId = 0
                Else
                    result.ReminderId = rs_Reminder("ReminderId")
                End If
                If rs_Reminder("ProjectOwnerId") Is DBNull.Value Then
                    result.ProjectOwnerId = 0
                Else
                    result.ProjectOwnerId = rs_Reminder("ProjectOwnerId")
                End If
                If rs_Reminder("ProjectId") Is DBNull.Value Then
                    result.ProjectId = 0
                Else
                    result.ProjectId = rs_Reminder("ProjectId")
                End If
                If rs_Reminder("ReminderTitle") Is DBNull.Value Then
                    result.ReminderTitle = String.Empty
                Else
                    result.ReminderTitle = rs_Reminder("ReminderTitle")
                End If
                If rs_Reminder("ReminderContentData") Is DBNull.Value Then
                    result.ReminderContentData = String.Empty
                Else
                    result.ReminderContentData = rs_Reminder("ReminderContentData")
                End If
                If rs_Reminder("Status") Is DBNull.Value Then
                    result.Status = Nothing
                Else
                    result.Status = rs_Reminder("Status")
                End If
                If rs_Reminder("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = Nothing
                Else
                    result.DisplayOrder = rs_Reminder("DisplayOrder")
                End If
                If rs_Reminder("StartDate") Is DBNull.Value Then
                    result.StartDate = Nothing
                Else
                    result.StartDate = rs_Reminder("StartDate")
                End If
                If rs_Reminder("EndDate") Is DBNull.Value Then
                    result.EndDate = Nothing
                Else
                    result.EndDate = rs_Reminder("EndDate")
                End If
                If rs_Reminder("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_Reminder("CreatedDate")
                End If
                If rs_Reminder("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_Reminder("ModifiedDate")
                End If
                If rs_Reminder("EmailTimeSetting") Is DBNull.Value Then
                    result.EmailTimeSetting = 0
                Else
                    result.EmailTimeSetting = rs_Reminder("EmailTimeSetting")
                End If
            End If
            rs_Reminder.Close()
            rs_Reminder = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetRemindersByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetRemindersByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "RemindersByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetRemindersValidforCalendarFeedByProjectOwnerId(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetRemindersValidforCalendarFeedByProjectOwnerId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "RemindersByProjectOwnerId")
            conn.Close()
            Return result
        End Function

        Public Function GetRemindersByProjectId(ByVal ProjectId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetRemindersByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "RemindersByProjectId")
            conn.Close()
            Return result
        End Function

        Public Function GetRemindersToEmail() As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetRemindersToEmail"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "RemindersToEmail")
            conn.Close()
            Return result
        End Function

        Public Sub DeleteReminderByReminderId(ByVal ReminderId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteReminderByReminderId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ReminderId", ReminderId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateTimesheetEntry(ByVal TimesheetEntry As TimesheetEntry) As Long
            Dim result As Long = 0
            Dim dsTimesheetEntry As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateTimesheetEntry"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure

            If TimesheetEntry.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", TimesheetEntry.Description)
            End If

            If TimesheetEntry.NoteContent = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", TimesheetEntry.NoteContent)
            End If

            If TimesheetEntry.PartyA = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@PartyA", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@PartyA", TimesheetEntry.PartyA)
            End If
            If TimesheetEntry.PartyB = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@PartyB", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@PartyB", TimesheetEntry.PartyB)
            End If
            If TimesheetEntry.EntryDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EntryDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EntryDate", TimesheetEntry.EntryDate)
            End If
            If TimesheetEntry.WorkStart = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorkStart", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorkStart", TimesheetEntry.WorkStart)
            End If
            If TimesheetEntry.WorkEnd = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorkEnd", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorkEnd", TimesheetEntry.WorkEnd)
            End If
            If TimesheetEntry.LunchStart = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@LunchStart", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@LunchStart", TimesheetEntry.LunchStart)
            End If
            If TimesheetEntry.LunchEnd = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@LunchEnd", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@LunchEnd", TimesheetEntry.LunchEnd)
            End If
            If TimesheetEntry.CycleEndDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@CycleEndDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@CycleEndDate", TimesheetEntry.CycleEndDate)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsTimesheetEntry, "TimesheetEntry")
            conn.Close()
            If dsTimesheetEntry.Tables.Count > 0 Then
                If dsTimesheetEntry.Tables(0).Rows.Count > 0 Then
                    result = dsTimesheetEntry.Tables(0).Rows(0)("TimesheetEntryId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateTimesheetEntry(ByVal TimesheetEntry As TimesheetEntry)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateTimesheetEntry"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@TimesheetEntryId", TimesheetEntry.TimesheetEntryId)
            If TimesheetEntry.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", TimesheetEntry.Description)
            End If
            If TimesheetEntry.NoteContent = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@NoteContent", TimesheetEntry.NoteContent)
            End If
            If TimesheetEntry.PartyA = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@PartyA", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@PartyA", TimesheetEntry.PartyA)
            End If
            If TimesheetEntry.PartyB = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@PartyB", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@PartyB", TimesheetEntry.PartyB)
            End If
            If TimesheetEntry.EntryDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EntryDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EntryDate", TimesheetEntry.EntryDate)
            End If
            If TimesheetEntry.WorkStart = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorkStart", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorkStart", TimesheetEntry.WorkStart)
            End If
            If TimesheetEntry.WorkEnd = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorkEnd", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorkEnd", TimesheetEntry.WorkEnd)
            End If
            If TimesheetEntry.LunchStart = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@LunchStart", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@LunchStart", TimesheetEntry.LunchStart)
            End If
            If TimesheetEntry.LunchEnd = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@LunchEnd", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@LunchEnd", TimesheetEntry.LunchEnd)
            End If
            If TimesheetEntry.CycleEndDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@CycleEndDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@CycleEndDate", TimesheetEntry.CycleEndDate)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub SaveTimesheetEntry(ByVal TimesheetEntry As TimesheetEntry)
            Dim objTimesheetEntry As TimesheetEntry = GetTimesheetEntryByPartyAPartyBEntryDate(TimesheetEntry.PartyA, TimesheetEntry.PartyB, TimesheetEntry.EntryDate)
            If objTimesheetEntry Is Nothing Then
                CreateTimesheetEntry(TimesheetEntry)
            Else
                TimesheetEntry.TimesheetEntryId = objTimesheetEntry.TimesheetEntryId
                UpdateTimesheetEntry(TimesheetEntry)
            End If
        End Sub

        Public Sub ProcessTimesheetEntry(ByVal TimesheetEntryId As Integer)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "ProcessTimesheetEntry"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@TimesheetEntryId", TimesheetEntryId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub ProcessTimesheetEntryPartyAEntryDateRange(ByVal PartyA As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "ProcessTimesheetEntryPartyAEntryDateRange"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@StartDate", StartDate)
            m_SqlCommand.Parameters.AddWithValue("@EndDate", EndDate)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub OpenTimesheetEntryPartyAEntryDateRange(ByVal PartyA As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "OpenTimesheetEntryPartyAEntryDateRange"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@StartDate", StartDate)
            m_SqlCommand.Parameters.AddWithValue("@EndDate", EndDate)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub ProcessTimesheetEntryPartyAPartyBEntryDateRange(ByVal PartyA As Long, ByVal PartyB As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "ProcessTimesheetEntryPartyAPartyBEntryDateRange"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@StartDate", StartDate)
            m_SqlCommand.Parameters.AddWithValue("@EndDate", EndDate)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteTimesheetEntry(ByVal TimesheetEntryId As Integer, ByVal MarkAsDeleted As Boolean)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteTimesheetEntry"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@TimesheetEntryId", TimesheetEntryId)
            m_SqlCommand.Parameters.AddWithValue("@MarkAsDeleted", MarkAsDeleted)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetTimesheetEntryByTimesheetEntryId(ByVal TimesheetEntryId As Long) As TimesheetEntry
            Dim result As TimesheetEntry = New TimesheetEntry()
            Dim rs_TimesheetEntry As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTimesheetEntryByTimesheetEntryId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@TimesheetEntryId", TimesheetEntryId)
            conn.Open()
            rs_TimesheetEntry = m_SqlCommand.ExecuteReader
            If rs_TimesheetEntry.HasRows Then
                rs_TimesheetEntry.Read()
                result.TimesheetEntryId = rs_TimesheetEntry("TimesheetEntryId")
                If rs_TimesheetEntry("Description") Is DBNull.Value Then
                    result.Description = Nothing
                Else
                    result.Description = rs_TimesheetEntry("Description")
                End If
                If rs_TimesheetEntry("NoteContent") Is DBNull.Value Then
                    result.NoteContent = Nothing
                Else
                    result.NoteContent = rs_TimesheetEntry("NoteContent")
                End If
                If rs_TimesheetEntry("PartyA") Is DBNull.Value Then
                    result.PartyA = Nothing
                Else
                    result.PartyA = rs_TimesheetEntry("PartyA")
                End If
                If rs_TimesheetEntry("PartyB") Is DBNull.Value Then
                    result.PartyB = Nothing
                Else
                    result.PartyB = rs_TimesheetEntry("PartyB")
                End If
                If rs_TimesheetEntry("EntryDate") Is DBNull.Value Then
                    result.EntryDate = Nothing
                Else
                    result.EntryDate = rs_TimesheetEntry("EntryDate")
                End If
                If rs_TimesheetEntry("WorkStart") Is DBNull.Value Then
                    result.WorkStart = Nothing
                Else
                    result.WorkStart = rs_TimesheetEntry("WorkStart")
                End If
                If rs_TimesheetEntry("WorkEnd") Is DBNull.Value Then
                    result.WorkEnd = Nothing
                Else
                    result.WorkEnd = rs_TimesheetEntry("WorkEnd")
                End If
                If rs_TimesheetEntry("LunchStart") Is DBNull.Value Then
                    result.LunchStart = Nothing
                Else
                    result.LunchStart = rs_TimesheetEntry("LunchStart")
                End If
                If rs_TimesheetEntry("LunchEnd") Is DBNull.Value Then
                    result.LunchEnd = Nothing
                Else
                    result.LunchEnd = rs_TimesheetEntry("LunchEnd")
                End If
                If rs_TimesheetEntry("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_TimesheetEntry("CreatedDate")
                End If
                If rs_TimesheetEntry("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_TimesheetEntry("ModifiedDate")
                End If
                If rs_TimesheetEntry("ProcessDate") Is DBNull.Value Then
                    result.ProcessDate = Nothing
                Else
                    result.ProcessDate = rs_TimesheetEntry("ProcessDate")
                End If
                If rs_TimesheetEntry("CycleEndDate") Is DBNull.Value Then
                    result.CycleEndDate = Nothing
                Else
                    result.CycleEndDate = rs_TimesheetEntry("CycleEndDate")
                End If
                If rs_TimesheetEntry("DeletedDate") Is DBNull.Value Then
                    result.DeletedDate = Nothing
                Else
                    result.DeletedDate = rs_TimesheetEntry("DeletedDate")
                End If
            End If
            rs_TimesheetEntry.Close()
            rs_TimesheetEntry = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetTimesheetEntryByPartyAPartyBEntryDate(ByVal PartyA As Long, ByVal PartyB As Long, ByVal EntryDate As DateTime) As TimesheetEntry
            Dim result As TimesheetEntry = Nothing
            Dim rs_TimesheetEntry As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTimesheetEntryByPartyAPartyBEntryDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@EntryDate", EntryDate)
            conn.Open()
            rs_TimesheetEntry = m_SqlCommand.ExecuteReader
            If rs_TimesheetEntry.HasRows Then
                rs_TimesheetEntry.Read()
                result = New TimesheetEntry()
                result.TimesheetEntryId = rs_TimesheetEntry("TimesheetEntryId")
                If rs_TimesheetEntry("Description") Is DBNull.Value Then
                    result.Description = Nothing
                Else
                    result.Description = rs_TimesheetEntry("Description")
                End If
                If rs_TimesheetEntry("NoteContent") Is DBNull.Value Then
                    result.NoteContent = Nothing
                Else
                    result.NoteContent = rs_TimesheetEntry("NoteContent")
                End If
                If rs_TimesheetEntry("PartyA") Is DBNull.Value Then
                    result.PartyA = Nothing
                Else
                    result.PartyA = rs_TimesheetEntry("PartyA")
                End If
                If rs_TimesheetEntry("PartyB") Is DBNull.Value Then
                    result.PartyB = Nothing
                Else
                    result.PartyB = rs_TimesheetEntry("PartyB")
                End If
                If rs_TimesheetEntry("EntryDate") Is DBNull.Value Then
                    result.EntryDate = Nothing
                Else
                    result.EntryDate = rs_TimesheetEntry("EntryDate")
                End If
                If rs_TimesheetEntry("WorkStart") Is DBNull.Value Then
                    result.WorkStart = Nothing
                Else
                    result.WorkStart = rs_TimesheetEntry("WorkStart")
                End If
                If rs_TimesheetEntry("WorkEnd") Is DBNull.Value Then
                    result.WorkEnd = Nothing
                Else
                    result.WorkEnd = rs_TimesheetEntry("WorkEnd")
                End If
                If rs_TimesheetEntry("LunchStart") Is DBNull.Value Then
                    result.LunchStart = Nothing
                Else
                    result.LunchStart = rs_TimesheetEntry("LunchStart")
                End If
                If rs_TimesheetEntry("LunchEnd") Is DBNull.Value Then
                    result.LunchEnd = Nothing
                Else
                    result.LunchEnd = rs_TimesheetEntry("LunchEnd")
                End If
                If rs_TimesheetEntry("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_TimesheetEntry("CreatedDate")
                End If
                If rs_TimesheetEntry("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_TimesheetEntry("ModifiedDate")
                End If
                If rs_TimesheetEntry("ProcessDate") Is DBNull.Value Then
                    result.ProcessDate = Nothing
                Else
                    result.ProcessDate = rs_TimesheetEntry("ProcessDate")
                End If
                If rs_TimesheetEntry("CycleEndDate") Is DBNull.Value Then
                    result.CycleEndDate = Nothing
                Else
                    result.CycleEndDate = rs_TimesheetEntry("CycleEndDate")
                End If
                If rs_TimesheetEntry("DeletedDate") Is DBNull.Value Then
                    result.DeletedDate = Nothing
                Else
                    result.DeletedDate = rs_TimesheetEntry("DeletedDate")
                End If
            End If
            rs_TimesheetEntry.Close()
            rs_TimesheetEntry = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetTimesheetEntryByPartyAPartyBEntryDateRange(ByVal PartyA As Long, ByVal PartyB As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTimesheetEntryByPartyAPartyBEntryDateRange"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@StartDate", StartDate)
            m_SqlCommand.Parameters.AddWithValue("@EndDate", EndDate)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "TimesheetEntryByPartyAPartyBEntryDateRange")
            conn.Close()
            Return result
        End Function

        Public Function GetTimesheetEntryDetailsByPartyAPartyBEntryDateRange(ByVal PartyA As Long, ByVal PartyB As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTimesheetEntryDetailsByPartyAPartyBEntryDateRange"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            m_SqlCommand.Parameters.AddWithValue("@StartDate", StartDate)
            m_SqlCommand.Parameters.AddWithValue("@EndDate", EndDate)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "TimesheetEntryDetailsByPartyAPartyBEntryDateRange")
            conn.Close()
            Return result
        End Function

        Public Function GetTimesheetEntrySummaryByPartyAEntryDateRange(ByVal PartyA As Long, ByVal StartDate As DateTime, ByVal EndDate As DateTime) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTimesheetEntrySummaryByPartyAEntryDateRange"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@StartDate", StartDate)
            m_SqlCommand.Parameters.AddWithValue("@EndDate", EndDate)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "TimesheetEntrySummaryByPartyAEntryDateRange")
            conn.Close()
            Return result
        End Function

        Public Function GetTimesheetLastCycleEndDateByPartyAPartyB(ByVal PartyA As Long, ByVal PartyB As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetTimesheetLastCycleEndDateByPartyAPartyB"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyA", PartyA)
            m_SqlCommand.Parameters.AddWithValue("@PartyB", PartyB)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "TimesheetLastCycleEndDateByPartyAPartyB")
            conn.Close()
            Return result
        End Function

        Public Function CreateUserProjectJobSetting(ByVal UserProjectJobSetting As UserProjectJobSetting) As Long
            Dim result As Long = 0
            Dim dsUserProjectJobSetting As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateUserProjectJobSetting"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If UserProjectJobSetting.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", UserProjectJobSetting.UserId)
            End If
            If UserProjectJobSetting.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", UserProjectJobSetting.ProjectId)
            End If
            If UserProjectJobSetting.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", UserProjectJobSetting.Name)
            End If
            If UserProjectJobSetting.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", UserProjectJobSetting.DisplayOrder)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsUserProjectJobSetting, "UserProjectJobSetting")
            conn.Close()
            If dsUserProjectJobSetting.Tables.Count > 0 Then
                If dsUserProjectJobSetting.Tables(0).Rows.Count > 0 Then
                    result = dsUserProjectJobSetting.Tables(0).Rows(0)("UserProjectJobSettingId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateUserProjectJobSetting(ByVal UserProjectJobSetting As UserProjectJobSetting)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateUserProjectJobSetting"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserProjectJobSettingId", UserProjectJobSetting.UserProjectJobSettingId)
            If UserProjectJobSetting.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", UserProjectJobSetting.UserId)
            End If
            If UserProjectJobSetting.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", UserProjectJobSetting.ProjectId)
            End If
            If UserProjectJobSetting.Name = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Name", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Name", UserProjectJobSetting.Name)
            End If
            If UserProjectJobSetting.DisplayOrder = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DisplayOrder", UserProjectJobSetting.DisplayOrder)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetProjectJobs() As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectJobs"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectJobs")
            conn.Close()
            Return result
        End Function

        Public Function GetProjectJobsByProjectIdUserId(ByVal ProjectId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetProjectJobsByProjectIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ProjectJobsByProjectIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetUserProjectJobsByProjectIdUserId(ByVal ProjectId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProjectJobsByProjectIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "UserProjectJobsByProjectIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetUserProjectJobSettingByUserProjectJobSettingId(ByVal UserProjectJobSettingId As Long) As UserProjectJobSetting
            Dim result As UserProjectJobSetting = New UserProjectJobSetting()
            Dim rs_UserProjectJobSetting As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProjectJobSettingByUserProjectJobSettingId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserProjectJobSettingId", UserProjectJobSettingId)
            conn.Open()
            rs_UserProjectJobSetting = m_SqlCommand.ExecuteReader
            If rs_UserProjectJobSetting.HasRows Then
                rs_UserProjectJobSetting.Read()
                If rs_UserProjectJobSetting("UserProjectJobSettingId") Is DBNull.Value Then
                    result.UserProjectJobSettingId = 0
                Else
                    result.UserProjectJobSettingId = rs_UserProjectJobSetting("UserProjectJobSettingId")
                End If
                If rs_UserProjectJobSetting("UserId") Is DBNull.Value Then
                    result.UserId = 0
                Else
                    result.UserId = rs_UserProjectJobSetting("UserId")
                End If
                If rs_UserProjectJobSetting("ProjectId") Is DBNull.Value Then
                    result.ProjectId = 0
                Else
                    result.ProjectId = rs_UserProjectJobSetting("ProjectId")
                End If
                If rs_UserProjectJobSetting("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_UserProjectJobSetting("Name")
                End If
                If rs_UserProjectJobSetting("Description") Is DBNull.Value Then
                    result.Description = String.Empty
                Else
                    result.Description = rs_UserProjectJobSetting("Description")
                End If
                If rs_UserProjectJobSetting("JobValue") Is DBNull.Value Then
                    result.JobValue = 0
                Else
                    result.JobValue = rs_UserProjectJobSetting("JobValue")
                End If
                If rs_UserProjectJobSetting("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_UserProjectJobSetting("DisplayOrder")
                End If
            End If
            rs_UserProjectJobSetting.Close()
            rs_UserProjectJobSetting = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetUserProjectJobSettingByUserIdJobValue(ByVal UserId As Long, ByVal JobValue As Integer) As UserProjectJobSetting
            Dim result As UserProjectJobSetting = New UserProjectJobSetting()
            Dim rs_UserProjectJobSetting As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetUserProjectJobSettingByUserIdJobValue"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            m_SqlCommand.Parameters.AddWithValue("@JobValue", JobValue)
            conn.Open()
            rs_UserProjectJobSetting = m_SqlCommand.ExecuteReader
            If rs_UserProjectJobSetting.HasRows Then
                rs_UserProjectJobSetting.Read()
                If rs_UserProjectJobSetting("Name") Is DBNull.Value Then
                    result.Name = String.Empty
                Else
                    result.Name = rs_UserProjectJobSetting("Name")
                End If
                If rs_UserProjectJobSetting("Description") Is DBNull.Value Then
                    result.Description = String.Empty
                Else
                    result.Description = rs_UserProjectJobSetting("Description")
                End If
                If rs_UserProjectJobSetting("JobValue") Is DBNull.Value Then
                    result.JobValue = 0
                Else
                    result.JobValue = rs_UserProjectJobSetting("JobValue")
                End If
                If rs_UserProjectJobSetting("DisplayOrder") Is DBNull.Value Then
                    result.DisplayOrder = 0
                Else
                    result.DisplayOrder = rs_UserProjectJobSetting("DisplayOrder")
                End If
            End If
            rs_UserProjectJobSetting.Close()
            rs_UserProjectJobSetting = Nothing
            conn.Close()

            Return result
        End Function

        Public Sub DeleteUserProjectJobSettingByUserProjectJobSettingId(ByVal UserProjectJobSettingId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteUserProjectJobSettingByUserProjectJobSettingId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@UserProjectJobSettingId", UserProjectJobSettingId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateJob(ByVal Job As Job) As Long
            Dim result As Long = 0
            Dim dsJob As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateJob"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If Job.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Job.ProjectOwnerId)
            End If
            If Job.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", Job.ProjectId)
            End If
            If Job.JobName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobName", Job.JobName)
            End If
            If Job.JobValue = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobValue", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobValue", Job.JobValue)
            End If
            If Job.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Job.Description)
            End If
            If Job.DueDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DueDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DueDate", Job.DueDate)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsJob, "Job")
            conn.Close()
            If dsJob.Tables.Count > 0 Then
                If dsJob.Tables(0).Rows.Count > 0 Then
                    result = dsJob.Tables(0).Rows(0)("JobId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateJob(ByVal Job As Job)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateJob"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobId", Job.JobId)
            If Job.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", Job.ProjectOwnerId)
            End If
            If Job.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", Job.ProjectId)
            End If
            If Job.JobName = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobName", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobName", Job.JobName)
            End If
            If Job.JobValue = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobValue", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobValue", Job.JobValue)
            End If
            If Job.Description = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@Description", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@Description", Job.Description)
            End If
            If Job.DueDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@DueDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@DueDate", Job.DueDate)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateJobStatus(ByVal JobId As Long, ByVal Status As JobStatus)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateJobStatus"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobId", JobId)
            m_SqlCommand.Parameters.AddWithValue("@Status", Status)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateJobArchivedDate(ByVal JobId As Long, ByVal ArchivedDate As DateTime)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateJobArchivedDate"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobId", JobId)
            If ArchivedDate = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ArchivedDate", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ArchivedDate", ArchivedDate)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteJobByJobId(ByVal JobId As Integer, ByVal MarkAsDeleted As Boolean)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteJobByJobId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobId", JobId)
            m_SqlCommand.Parameters.AddWithValue("@MarkAsDeleted", MarkAsDeleted)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetJobsByProjectId(ByVal ProjectId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetJobsByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "JobsByProjectId")
            conn.Close()
            Return result
        End Function

        Public Function GetArchivedJobsByProjectId(ByVal ProjectId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetArchivedJobsByProjectId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectId", ProjectId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ArchivedJobsByProjectId")
            conn.Close()
            Return result
        End Function

        Public Function GetJobsByProjectOwnerIdUserId(ByVal ProjectOwnerId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetJobsByProjectOwnerIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "JobsByProjectOwnerIdUserId")
            conn.Close()
            Return result
        End Function

        Public Function GetJobByJobId(ByVal JobId As Long) As Job
            Dim result As Job = New Job()
            Dim dsJob As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetJobByJobId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobId", JobId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsJob, "JobByJobId")
            conn.Close()

            If dsJob.Tables.Count > 1 Then
                Dim row As DataRow
                If dsJob.Tables(0).Rows.Count > 0 Then
                    row = dsJob.Tables(0).Rows(0)
                    If Not IsDBNull(row("JobId")) Then
                        result.JobId = row("JobId")
                    End If
                    If Not IsDBNull(row("ProjectOwnerId")) Then
                        result.ProjectOwnerId = row("ProjectOwnerId")
                    End If
                    If Not IsDBNull(row("ProjectId")) Then
                        result.ProjectId = row("ProjectId")
                    End If
                    result.JobName = String.Format("{0}", row("JobName"))
                    If Not IsDBNull(row("JobValue")) Then
                        result.JobValue = row("JobValue")
                    End If
                    result.Description = String.Format("{0}", row("Description"))
                    If Not IsDBNull(row("Status")) Then
                        result.Status = row("Status")
                    End If
                    If Not IsDBNull(row("DueDate")) Then
                        result.DueDate = row("DueDate")
                    End If
                    If Not IsDBNull(row("StartedDate")) Then
                        result.StartedDate = row("StartedDate")
                    End If
                    If Not IsDBNull(row("CompletedDate")) Then
                        result.CompletedDate = row("CompletedDate")
                    End If
                    If Not IsDBNull(row("CreatedDate")) Then
                        result.CreatedDate = row("CreatedDate")
                    End If
                    If Not IsDBNull(row("ModifiedDate")) Then
                        result.ModifiedDate = row("ModifiedDate")
                    End If
                    result.StatusName = StrConv(Replace(CType(result.Status, JobStatus).ToString(), "_", " "), VbStrConv.Uppercase)
                End If
                If dsJob.Tables(1).Rows.Count > 0 Then
                    result.JobAssignments = New List(Of JobAssignment)
                    Dim objJobAssignment As JobAssignment
                    Dim objUserProfile As UserProfile
                    For Each row In dsJob.Tables(1).Rows
                        objJobAssignment = New JobAssignment()
                        objUserProfile = New UserProfile()
                        If Not IsDBNull(row("JobAssignmentId")) Then
                            objJobAssignment.JobAssignmentId = row("JobAssignmentId")
                        End If

                        If Not IsDBNull(row("ProjectOwnerId")) Then
                            objJobAssignment.ProjectOwnerId = row("ProjectOwnerId")
                        End If
                        If Not IsDBNull(row("ProjectId")) Then
                            objJobAssignment.ProjectId = row("ProjectId")
                        End If
                        If Not IsDBNull(row("JobId")) Then
                            objJobAssignment.JobId = row("JobId")
                        End If
                        If Not IsDBNull(row("UserId")) Then
                            objJobAssignment.UserId = row("UserId")
                        End If
                        If Not IsDBNull(row("Status")) Then
                            objJobAssignment.Status = row("Status")
                        End If
                        If Not IsDBNull(row("CreatedDate")) Then
                            objJobAssignment.CreatedDate = row("CreatedDate")
                        End If
                        If Not IsDBNull(row("ModifiedDate")) Then
                            objJobAssignment.ModifiedDate = row("ModifiedDate")
                        End If
                        If Not IsDBNull(row("UserId")) Then
                            objJobAssignment.UserProfile = GetUserProfileByUserID(row("UserId"))
                        End If
                        result.JobAssignments.Add(objJobAssignment)
                    Next
                End If
            End If

            Return result
        End Function

        Public Function CreateJobAssignment(ByVal JobAssignment As JobAssignment) As Long
            Dim result As Long = 0
            Dim dsJobAssignment As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateJobAssignment"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            If JobAssignment.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", JobAssignment.ProjectOwnerId)
            End If
            If JobAssignment.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", JobAssignment.ProjectId)
            End If
            If JobAssignment.JobId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobId", JobAssignment.JobId)
            End If
            If JobAssignment.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", JobAssignment.UserId)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsJobAssignment, "JobAssignment")
            conn.Close()
            If dsJobAssignment.Tables.Count > 0 Then
                If dsJobAssignment.Tables(0).Rows.Count > 0 Then
                    result = dsJobAssignment.Tables(0).Rows(0)("JobAssignmentId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateJobAssignment(ByVal JobAssignment As JobAssignment)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateJobAssignment"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobAssignmentId", JobAssignment.JobAssignmentId)
            If JobAssignment.ProjectOwnerId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", JobAssignment.ProjectOwnerId)
            End If
            If JobAssignment.ProjectId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@ProjectId", JobAssignment.ProjectId)
            End If
            If JobAssignment.JobId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobId", JobAssignment.JobId)
            End If
            If JobAssignment.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", JobAssignment.UserId)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub UpdateJobAssignmentStatus(ByVal JobAssignmentId As Long, ByVal Status As JobStatus)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateJobAssignmentStatus"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobAssignmentId", JobAssignmentId)
            m_SqlCommand.Parameters.AddWithValue("@Status", Status)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteJobAssignmentByJobAssignmentId(ByVal JobAssignmentId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteJobAssignmentByJobAssignmentId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobAssignmentId", JobAssignmentId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub DeleteJobAssignmentByJobIdUserId(ByVal JobId As Long, ByVal UserId As Long)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "DeleteJobAssignmentByJobIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobId", JobId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function CreateJobTimeEntry(ByVal JobTimeEntry As JobTimeEntry) As Long
            Dim result As Long = 0
            Dim dsJobTimeEntry As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "CreateJobTimeEntry"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure

            If JobTimeEntry.JobAssignmentId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobAssignmentId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobAssignmentId", JobTimeEntry.JobAssignmentId)
            End If
            If JobTimeEntry.JobId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobId", JobTimeEntry.JobId)
            End If
            If JobTimeEntry.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", JobTimeEntry.UserId)
            End If
            If JobTimeEntry.StartTime = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@StartTime", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@StartTime", JobTimeEntry.StartTime)
            End If
            If JobTimeEntry.EndTime = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EndTime", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EndTime", JobTimeEntry.EndTime)
            End If
            If JobTimeEntry.WorkingTimeInMinutes = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorkingTimeInMinutes", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorkingTimeInMinutes", JobTimeEntry.WorkingTimeInMinutes)
            End If
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(dsJobTimeEntry, "JobTimeEntry")
            conn.Close()
            If dsJobTimeEntry.Tables.Count > 0 Then
                If dsJobTimeEntry.Tables(0).Rows.Count > 0 Then
                    result = dsJobTimeEntry.Tables(0).Rows(0)("JobTimeEntryId")
                End If
            End If
            Return result
        End Function

        Public Sub UpdateJobTimeEntry(ByVal JobTimeEntry As JobTimeEntry)
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "UpdateJobTimeEntry"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobTimeEntryId", JobTimeEntry.JobTimeEntryId)
            If JobTimeEntry.JobAssignmentId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobAssignmentId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobAssignmentId", JobTimeEntry.JobAssignmentId)
            End If
            If JobTimeEntry.JobId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@JobId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@JobId", JobTimeEntry.JobId)
            End If
            If JobTimeEntry.UserId = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@UserId", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@UserId", JobTimeEntry.UserId)
            End If
            If JobTimeEntry.StartTime = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@StartTime", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@StartTime", JobTimeEntry.StartTime)
            End If
            If JobTimeEntry.EndTime = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@EndTime", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@EndTime", JobTimeEntry.EndTime)
            End If
            If JobTimeEntry.WorkingTimeInMinutes = Nothing Then
                m_SqlCommand.Parameters.AddWithValue("@WorkingTimeInMinutes", DBNull.Value)
            Else
                m_SqlCommand.Parameters.AddWithValue("@WorkingTimeInMinutes", JobTimeEntry.WorkingTimeInMinutes)
            End If
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Function GetJobTimeEntryByJobTimeEntryId(ByVal JobTimeEntryId As Long) As JobTimeEntry
            Dim result As JobTimeEntry = New JobTimeEntry()
            Dim rs_JobTimeEntry As SqlDataReader
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetJobTimeEntryByJobTimeEntryId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobTimeEntryId", JobTimeEntryId)
            conn.Open()
            rs_JobTimeEntry = m_SqlCommand.ExecuteReader
            If rs_JobTimeEntry.HasRows Then
                rs_JobTimeEntry.Read()
                result.JobTimeEntryId = rs_JobTimeEntry("JobTimeEntryId")
                If rs_JobTimeEntry("JobAssignmentId") Is DBNull.Value Then
                    result.JobAssignmentId = Nothing
                Else
                    result.JobAssignmentId = rs_JobTimeEntry("JobAssignmentId")
                End If
                If rs_JobTimeEntry("JobId") Is DBNull.Value Then
                    result.JobId = Nothing
                Else
                    result.JobId = rs_JobTimeEntry("JobId")
                End If
                If rs_JobTimeEntry("UserId") Is DBNull.Value Then
                    result.UserId = Nothing
                Else
                    result.UserId = rs_JobTimeEntry("UserId")
                End If
                If rs_JobTimeEntry("StartTime") Is DBNull.Value Then
                    result.StartTime = Nothing
                Else
                    result.StartTime = rs_JobTimeEntry("StartTime")
                End If
                If rs_JobTimeEntry("EndTime") Is DBNull.Value Then
                    result.EndTime = Nothing
                Else
                    result.EndTime = rs_JobTimeEntry("EndTime")
                End If
                If rs_JobTimeEntry("WorkingTimeInMinutes") Is DBNull.Value Then
                    result.WorkingTimeInMinutes = Nothing
                Else
                    result.WorkingTimeInMinutes = rs_JobTimeEntry("WorkingTimeInMinutes")
                End If
                If rs_JobTimeEntry("CreatedDate") Is DBNull.Value Then
                    result.CreatedDate = Nothing
                Else
                    result.CreatedDate = rs_JobTimeEntry("CreatedDate")
                End If
                If rs_JobTimeEntry("ModifiedDate") Is DBNull.Value Then
                    result.ModifiedDate = Nothing
                Else
                    result.ModifiedDate = rs_JobTimeEntry("ModifiedDate")
                End If
            End If
            rs_JobTimeEntry.Close()
            rs_JobTimeEntry = Nothing
            conn.Close()

            Return result
        End Function

        Public Function GetJobTimeEntrySummaryByJobId(ByVal JobId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetJobTimeEntrySummaryByJobId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobId", JobId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "JobTimeEntrySummaryByJobId")
            conn.Close()
            Return result
        End Function

        Public Function GetJobTimeEntriesByJobIdUserId(ByVal JobId As Long, ByVal UserId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetJobTimeEntriesByJobIdUserId"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@JobId", JobId)
            m_SqlCommand.Parameters.AddWithValue("@UserId", UserId)
            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "JobTimeEntriesByJobIdUserId")
            conn.Close()
            Return result
        End Function
        'GetReorder
        Public Function GetReorder(ByVal ProjectOwnerId As Long) As DataSet
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "GetReOderByProjectID"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ProjectOwnerId", ProjectOwnerId)

            conn.Open()
            m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
            m_SqlDataAdapter.Fill(result, "ReOrder")
            conn.Close()
            Return result
        End Function

        Public Function UpdateSubscription(ByVal UserID As Long, ByVal NoOfSubscribedUsers As Integer, ByVal NextBillingDate As Date) As DataSet
            Try
                Dim result As DataSet = New DataSet()
                Dim conn As SqlConnection = m_SQLConn.conn()
                m_SqlCommand = New SqlCommand(m_SQL, conn)
                m_SqlCommand.CommandText = "USP_UserAccounts_UpdateSubscription"
                m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
                m_SqlCommand.Parameters.AddWithValue("@UserID", UserID)
                m_SqlCommand.Parameters.AddWithValue("@NoOfSubscribedUsers", NoOfSubscribedUsers)
                m_SqlCommand.Parameters.AddWithValue("@NextBillingDate", NextBillingDate)

                conn.Open()
                m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
                m_SqlDataAdapter.Fill(result, "ReOrder")
                conn.Close()

                Return result
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Sub UpdatePaymentHistory(ByVal paymentID As String, ByVal isPaymentSuccessful As Boolean, ByVal State As String, ByVal NoOfUsers As Integer, ByVal Amount As Decimal, ByVal failureReason As String)
            Try
                Dim conn As SqlConnection = m_SQLConn.conn()
                m_SqlCommand = New SqlCommand(m_SQL, conn)
                m_SqlCommand.CommandText = "USP_PaymentHistory_Insert"
                m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
                m_SqlCommand.Parameters.AddWithValue("@PaypalPaymentID", paymentID)
                m_SqlCommand.Parameters.AddWithValue("@bitSuccess", isPaymentSuccessful)
                m_SqlCommand.Parameters.AddWithValue("@State", State)
                m_SqlCommand.Parameters.AddWithValue("@NoOfSubscribedUsers", NoOfUsers)
                m_SqlCommand.Parameters.AddWithValue("@Amount", Amount)
                m_SqlCommand.Parameters.AddWithValue("@FailureReason", failureReason)

                conn.Open()
                m_SqlCommand.ExecuteNonQuery()
                conn.Close()
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Function CanAddOrActiveUser(ByVal UserID As Long, ByVal ContactID As Long) As Boolean
            Try
                Dim canAddorActivateUser As Boolean = False
                Dim conn As SqlConnection = m_SQLConn.conn()
                m_SqlCommand = New SqlCommand(m_SQL, conn)
                m_SqlCommand.CommandText = "USP_UserRelationships_CanAddOrActivate"
                m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
                m_SqlCommand.Parameters.AddWithValue("@UserID", UserID)
                m_SqlCommand.Parameters.AddWithValue("@ContactID", ContactID)

                conn.Open()
                canAddorActivateUser = Convert.ToBoolean(m_SqlCommand.ExecuteScalar())
                conn.Close()

                Return canAddorActivateUser
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Sub SavePaypalBillingAgreement(ByVal UserID As Long, ByVal IsActive As Boolean, ByVal StartDate As DateTime, ByVal PaypalAgreementID As String, ByVal billingCycle As String, ByVal LastPaymentAmount As Decimal, ByVal NextPaymentDate As DateTime)
            Try
                Dim conn As SqlConnection = m_SQLConn.conn()
                m_SqlCommand = New SqlCommand(m_SQL, conn)
                m_SqlCommand.CommandText = "USP_PaypalBillingAgreement_Insert"
                m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
                m_SqlCommand.Parameters.AddWithValue("@UserID", UserID)
                m_SqlCommand.Parameters.AddWithValue("@PaypalAgreementID", PaypalAgreementID)
                m_SqlCommand.Parameters.AddWithValue("@IsActive", IsActive)
                m_SqlCommand.Parameters.AddWithValue("@StartDate", StartDate)
                m_SqlCommand.Parameters.AddWithValue("@BillingCycle", billingCycle)
                m_SqlCommand.Parameters.AddWithValue("@LastPaymentDate", StartDate)
                m_SqlCommand.Parameters.AddWithValue("@LastPaymentAmount", LastPaymentAmount)
                m_SqlCommand.Parameters.AddWithValue("@NextPaymentDate", NextPaymentDate)

                conn.Open()
                m_SqlCommand.ExecuteNonQuery()
                conn.Close()
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Function GetActiveBillingAgreementByUser(ByVal UserID As Long) As DataSet
            Try
                Dim ds As DataSet = New DataSet()
                Dim conn As SqlConnection = m_SQLConn.conn()
                m_SqlCommand = New SqlCommand(m_SQL, conn)
                m_SqlCommand.CommandText = "USP_PaypalBillingAgreement_GetActiveByUserID"
                m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
                m_SqlCommand.Parameters.AddWithValue("@UserID", UserID)

                conn.Open()
                m_SqlDataAdapter = New SqlDataAdapter(m_SqlCommand)
                m_SqlDataAdapter.Fill(ds, "BillingAgreement")
                conn.Close()

                Return ds
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Sub UpdateBillingSubscription(ByVal PaypalAgreementID As String, ByVal LastPaymentDate As DateTime, ByVal LastPaymentAmount As Decimal, ByVal NextPaymentDate As DateTime)
            Try
                Dim conn As SqlConnection = m_SQLConn.conn()
                m_SqlCommand = New SqlCommand(m_SQL, conn)
                m_SqlCommand.CommandText = "USP_PaypalBillingAgreement_Update"
                m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
                m_SqlCommand.Parameters.AddWithValue("@PaypalAgreementID", PaypalAgreementID)
                m_SqlCommand.Parameters.AddWithValue("@LastPaymentDate", LastPaymentDate)
                m_SqlCommand.Parameters.AddWithValue("@LastPaymentAmount", LastPaymentAmount)
                m_SqlCommand.Parameters.AddWithValue("@NextPaymentDate", NextPaymentDate)

                conn.Open()
                m_SqlCommand.ExecuteNonQuery()
                conn.Close()
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub CancelBillingSubscription(ByVal PaypalAgreementID As String)
            Try
                Dim conn As SqlConnection = m_SQLConn.conn()
                m_SqlCommand = New SqlCommand(m_SQL, conn)
                m_SqlCommand.CommandText = "USP_PaypalBillingAgreement_Cancel"
                m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
                m_SqlCommand.Parameters.AddWithValue("@PaypalAgreementID", PaypalAgreementID)

                conn.Open()
                m_SqlCommand.ExecuteNonQuery()
                conn.Close()
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub LogError(ByVal errorMessage As String, ByVal stackTrace As String)
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "USP_ErrorLog_Insert"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@ErrorMessage", errorMessage)
            m_SqlCommand.Parameters.AddWithValue("@StackTrace", stackTrace)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

        Public Sub ChangeContactStatus(ByVal partyB As Long, ByVal status As Short)
            Dim result As DataSet = New DataSet()
            Dim conn As SqlConnection = m_SQLConn.conn()
            m_SqlCommand = New SqlCommand(m_SQL, conn)
            m_SqlCommand.CommandText = "USP_UserRelationships_ChangeStauts"
            m_SqlCommand.CommandType = Data.CommandType.StoredProcedure
            m_SqlCommand.Parameters.AddWithValue("@PartyB", partyB)
            m_SqlCommand.Parameters.AddWithValue("@Status", status)
            conn.Open()
            m_SqlCommand.ExecuteNonQuery()
            conn.Close()
        End Sub

    End Class

End Namespace