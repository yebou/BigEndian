Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace AmaknaCore.Common.IO
    Public Class FinalUInt64

#Region "Variables"

        Public Low As UInteger
        Public High As UInteger

#End Region

#Region "Builder"

        Public Sub New(Optional param1 As UInteger = 0, Optional param2 As Integer = 0)
            Low = param1
            High = CUInt(param2)
        End Sub

#End Region

#Region "Methods"

        Public Shared Function FromNumber(param1 As [Double]) As FinalInt64
            Return New FinalInt64(CUInt(Math.Truncate(param1)), CInt(Math.Truncate(Math.Floor(CDbl(param1 / 4294967296.0)))))
        End Function

        Public Function ToNumber() As [Double]
            Return High * 4294967296.0 + Low
        End Function

#End Region

    End Class
End Namespace
