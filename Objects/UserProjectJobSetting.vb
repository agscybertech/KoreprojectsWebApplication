Namespace Warpfusion.A4PP.Objects
    Public Class UserProjectJobSetting
        Private m_UserProjectJobSettingId As Long
        Private m_UserId As Long
        Private m_ProjectId As Long
        Private m_Name As String
        Private m_Description As String
        Private m_JobValue As Integer
        Private m_DisplayOrder As Integer

        Public Property UserProjectJobSettingId() As Long
            Get
                Return m_UserProjectJobSettingId
            End Get
            Set(ByVal value As Long)
                m_UserProjectJobSettingId = value
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
        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
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
        Public Property DisplayOrder() As Integer
            Get
                Return m_DisplayOrder
            End Get
            Set(ByVal value As Integer)
                m_DisplayOrder = value
            End Set
        End Property
    End Class
End Namespace