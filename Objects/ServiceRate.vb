Namespace Warpfusion.A4PP.Objects
    Public Class ServiceRate
        Private m_ServiceRateId As Long
        Private m_ServiceId As Long
        Private m_CostRate As Decimal
        Private m_ChargeRate As Decimal
        Private m_Unit As String
        Private m_Disabled As Boolean

        Public Property ServiceRateId() As Long
            Get
                Return m_ServiceRateId
            End Get
            Set(ByVal value As Long)
                m_ServiceRateId = value
            End Set
        End Property
        Public Property ServiceId() As Long
            Get
                Return m_ServiceId
            End Get
            Set(ByVal value As Long)
                m_ServiceId = value
            End Set
        End Property
        Public Property CostRate() As Long
            Get
                Return m_CostRate
            End Get
            Set(ByVal value As Long)
                m_CostRate = value
            End Set
        End Property
        Public Property ChargeRate() As String
            Get
                Return m_ChargeRate
            End Get
            Set(ByVal value As String)
                m_ChargeRate = value
            End Set
        End Property
        Public Property Unit() As String
            Get
                Return m_Unit
            End Get
            Set(ByVal value As String)
                m_Unit = value
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
    End Class
End Namespace
