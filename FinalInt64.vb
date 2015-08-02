Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Imports Arsonist.Protocol.Other.Log

Namespace AmaknaCore.Common.IO
    Public Class FinalInt64

#Region "Variables"

        Public Low As UInt64
        Public High As Integer

#End Region

#Region "Builder"

        Public Sub New(Optional param1 As UInt64 = 0, Optional param2 As Integer = 0)
            Low = param1
            High = param2
        End Sub

#End Region

#Region "Methods"

        Public Shared Function FromNumber(param1 As Double) As FinalInt64
            Return New FinalInt64(Convert.ToUInt64(param1), CInt(Math.Floor(param1 / 4294967296.0)))
        End Function

        Public Function ToNumber() As Double
            Return High * 4294967296.0 + Low
        End Function

#End Region

    End Class
End Namespace
