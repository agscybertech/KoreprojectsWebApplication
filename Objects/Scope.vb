Namespace Warpfusion.A4PP.Objects
    Public Class Scope
        Private m_ScopeId As Long
        Private m_ProjectId As Long
        Private m_GSTRate As Decimal
        Private m_Cost As Decimal
        Private m_GST As Decimal
        Private m_Total As Decimal
        Private m_Cost1 As Decimal
        Private m_GST1 As Decimal
        Private m_Total1 As Decimal
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime

        Public Property ScopeId() As Long
            Get
                Return m_ScopeId
            End Get
            Set(ByVal value As Long)
                m_ScopeId = value
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
        Public Property GSTRate() As Decimal
            Get
                Return m_GSTRate
            End Get
            Set(ByVal value As Decimal)
                m_GSTRate = value
            End Set
        End Property
        Public Property Cost() As Decimal
            Get
                Return m_Cost
            End Get
            Set(ByVal value As Decimal)
                m_Cost = value
            End Set
        End Property
        Public Property GST() As Decimal
            Get
                Return m_GST
            End Get
            Set(ByVal value As Decimal)
                m_GST = value
            End Set
        End Property
        Public Property Total() As Decimal
            Get
                Return m_Total
            End Get
            Set(ByVal value As Decimal)
                m_Total = value
            End Set
        End Property
        Public Property Cost1() As Decimal
            Get
                Return m_Cost1
            End Get
            Set(ByVal value As Decimal)
                m_Cost1 = value
            End Set
        End Property
        Public Property GST1() As Decimal
            Get
                Return m_GST1
            End Get
            Set(ByVal value As Decimal)
                m_GST1 = value
            End Set
        End Property
        Public Property Total1() As Decimal
            Get
                Return m_Total1
            End Get
            Set(ByVal value As Decimal)
                m_Total1 = value
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
    End Class
End Namespace