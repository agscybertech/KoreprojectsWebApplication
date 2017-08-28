Namespace Warpfusion.A4PP.Objects
    Public Class UserProjectStatusSetting
        Private m_UserProjectStatusSettingId As Long
        Private m_UserId As Long
        Private m_ProjectId As Long
        Private m_Name As String
        Private m_Description As String
        Private m_StatusValue As Integer
        Private m_DisplayOrder As Integer

        Public Property UserProjectStatusSettingId() As Long
            Get
                Return m_UserProjectStatusSettingId
            End Get
            Set(ByVal value As Long)
                m_UserProjectStatusSettingId = value
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
        Public Property StatusValue() As Integer
            Get
                Return m_StatusValue
            End Get
            Set(ByVal value As Integer)
                m_StatusValue = value
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