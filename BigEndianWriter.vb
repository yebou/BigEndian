Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.IO

Imports Arsonist.Protocol.Other.Log

Namespace AmaknaCore.Common.IO
    Public Class BigEndianWriter
        Implements IDataWriter
        Implements IDisposable

#Region "Properties"

        Private m_writer As BinaryWriter

        Public ReadOnly Property BaseStream() As Stream
            Get
                Return m_writer.BaseStream
            End Get
        End Property

        Public ReadOnly Property BytesAvailable() As Long
            Get
                Return m_writer.BaseStream.Length - m_writer.BaseStream.Position
            End Get
        End Property

        Public Property Position() As Long
            Get
                Return m_writer.BaseStream.Position
            End Get
            Set(ByVal Value As Long)
                m_writer.BaseStream.Position = Value
            End Set
        End Property

        Public ReadOnly Property Data() As Byte() Implements IDataWriter.Data
            Get
                Dim pos = m_writer.BaseStream.Position

                Dim datas = New Byte(m_writer.BaseStream.Length - 1) {}
                m_writer.BaseStream.Position = 0
                m_writer.BaseStream.Read(datas, 0, CInt(m_writer.BaseStream.Length))
                m_writer.BaseStream.Position = pos

                Return datas
            End Get
        End Property

        Private Function ComputeStaticHeader(ByVal PacketId As Integer, ByVal MessageLenghtType As Integer) As Short
            Return (PacketId << 2) Or MessageLenghtType
        End Function

        Private Function ComputeTypeLen(ByVal MessageLenght As Integer) As Short
            Select Case MessageLenght
                Case Is > UShort.MaxValue
                    Return 3
                Case Is > Byte.MaxValue
                    Return 2
                Case Is > 0
                    Return 1
                Case Else
                    Return 0
            End Select
        End Function

        Public Sub Send(ByVal PacketID As Integer, Message As BigEndianWriter)
            Dim test As New BigEndianWriter

            Dim _loc_5 As UInteger = 0
            Dim _loc_6 As UInteger = 0

            Dim MessageLenghtType As UInteger = ComputeTypeLen(Message.Data().Length)
            Dim Header As Short = ComputeStaticHeader(PacketID, MessageLenghtType)

            test.WriteShort(Header)

            Select Case MessageLenghtType

                Case 0
                    Exit Select
                Case 1
                    test.WriteByte(Message.Data().Length)
                    Exit Select
                Case 2
                    test.WriteUShort(Message.Data().Length)
                    Exit Select
                Case 3
                    _loc_5 = (Message.Data().Length >> 16) And 255
                    _loc_6 = Message.Data().Length and 65535
                    test.WriteByte(_loc_5)
                    test.WriteShort(_loc_6)
                    Exit Select
                Case Else
                    Exit Select
            End Select

            test.WriteBytes(Message.Data())
            WriteBytes(test.Data())

            Log.Write("[INFO] Message Lenght = " & test.Data().Length)

            test.Dispose()
            Message.Dispose()
        End Sub

#End Region

#Region "Initialisation"

        Public Sub New()
            m_writer = New BinaryWriter(New MemoryStream(), Encoding.UTF8)
        End Sub

        Public Sub New(ByVal stream As Stream)
            m_writer = New BinaryWriter(stream, Encoding.UTF8)
        End Sub

        Public Sub New(ByVal buffer() As Byte)
            m_writer = New BinaryWriter(New MemoryStream(buffer))
        End Sub
#End Region

#Region "Private Methods"

        Private Sub WriteBigEndianBytes(ByVal endianBytes() As Byte)
            Dim i As Integer
            For i = endianBytes.Length - 1 To 0 Step i - 1
                m_writer.Write(endianBytes(i))
            Next
        End Sub

#End Region

#Region "Public Methods"

        Public Sub WriteShort(ByVal shorts As Short) Implements IDataWriter.WriteShort
            Dim arr As Byte()
            arr = BitConverter.GetBytes(shorts)
            WriteByte(arr(1))
            WriteByte(arr(0))
        End Sub

        Public Sub WriteInt(ByVal int As Integer) Implements IDataWriter.WriteInt
            Dim arr As Byte()
            arr = BitConverter.GetBytes(int)
            WriteByte(arr(3))
            WriteByte(arr(2))
            WriteByte(arr(1))
            WriteByte(arr(0))
        End Sub

        Public Sub WriteLong(ByVal longs As Int64) Implements IDataWriter.WriteLong
            WriteBigEndianBytes(BitConverter.GetBytes(longs))
        End Sub

        Public Sub WriteUShort(ByVal ushorts As System.UInt16) Implements IDataWriter.WriteUShort
            WriteBigEndianBytes(BitConverter.GetBytes(ushorts))
        End Sub

        Public Sub WriteUInt(ByVal uint As UInt32) Implements IDataWriter.WriteUInt
            WriteBigEndianBytes(BitConverter.GetBytes(uint))
        End Sub

        Public Sub WriteULong(ByVal ulongs As UInt64) Implements IDataWriter.WriteULong
            WriteBigEndianBytes(BitConverter.GetBytes(ulongs))
        End Sub

        Public Sub WriteByte(ByVal bytes As Byte) Implements IDataWriter.WriteByte
            m_writer.Write(bytes)
        End Sub

        Public Sub WriteSByte(ByVal bytes As System.SByte) Implements IDataWriter.WriteSByte
            m_writer.Write(bytes)
        End Sub

        Public Sub WriteFloat(ByVal floats As Single) Implements IDataWriter.WriteFloat
            m_writer.Write(floats)
        End Sub

        Public Sub WriteBoolean(ByVal bool As Boolean) Implements IDataWriter.WriteBoolean
            If bool Then
                m_writer.Write(CByte(1))
            Else
                m_writer.Write(CByte(0))
            End If
        End Sub

        Public Sub WriteChar(ByVal chars As Char) Implements IDataWriter.WriteChar
            WriteBigEndianBytes(BitConverter.GetBytes(chars))
        End Sub

        Public Sub WriteDouble(ByVal Value As Double) Implements IDataWriter.WriteDouble
            WriteByte(CByte(Value >> 56))
            Value -= (Value >> 56) << 56
            WriteByte(CByte(Value >> 48))
            Value -= (Value >> 48) << 48
            WriteByte(CByte(Value >> 40))
            Value -= (Value >> 40) << 40
            WriteByte(CByte(Value >> 32))
            Value -= (Value >> 32) << 32
            WriteByte(CByte(Value >> 24))
            Value -= (Value >> 24) << 24
            WriteByte(CByte(Value >> 16))
            Value -= (Value >> 16) << 16
            WriteByte(CByte(Value >> 8))
            Value -= (Value >> 8) << 8
            WriteByte(CByte(Value))
        End Sub

        Public Sub WriteSingle(ByVal singles As Single) Implements IDataWriter.WriteSingle
            WriteBigEndianBytes(BitConverter.GetBytes(singles))
        End Sub

        Public Sub WriteUTF(ByVal str As String) Implements IDataWriter.WriteUTF
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(str)
            Dim len = CUInt(bytes.Count)
            WriteUShort(len)

            Dim i As Integer
            For i = 0 To len - 1 Step i + 1
                m_writer.Write(bytes(i))
            Next
        End Sub

        Public Sub WriteUTFBytes(ByVal str As String) Implements IDataWriter.WriteUTFBytes
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(str)
            Dim len = bytes.Count
            Dim i As Integer
            For i = 0 To len - 1 Step i + 1
                m_writer.Write(bytes(i))
            Next
        End Sub

        Public Sub WriteBytes(ByVal data() As Byte) Implements IDataWriter.WriteBytes
            m_writer.Write(data)
        End Sub

        Public Sub WriteBytes(ByVal data() As Byte, ByVal offset As System.UInt32, ByVal length As System.UInt32) Implements IDataWriter.WriteBytes
            Dim array() As Byte = New Byte(length) {}
            System.Array.Copy(data, offset, array, 0, length)
            m_writer.Write(array)
        End Sub


        Public Sub Seek(ByVal offset As Integer, ByVal seekOrigin As SeekOrigin)
            m_writer.BaseStream.Seek(offset, seekOrigin)
        End Sub


        Public Sub Clear() Implements IDataWriter.Clear
            m_writer = New BinaryWriter(New MemoryStream(), Encoding.UTF8)
        End Sub

#End Region

#Region "Custom Methods"

        Private Const INT_SIZE As Integer = 32
        Private Const SHORT_SIZE As Integer = 16
        Private Const SHORT_MIN_VALUE As Integer = -32768
        Private Const SHORT_MAX_VALUE As Integer = 32767
        Private Const UNSIGNED_SHORT_MAX_VALUE As Integer = 65536
        Private Const CHUNCK_BIT_SIZE As Integer = 7

        Private Const MASK_1 As Integer = 128
        Private Const MASK_0 As Integer = 127

        Public Sub WriteVarInt(ByVal int As Integer) Implements IDataWriter.WriteVarInt
            Dim local_5 = 0
            Dim local_2 As BigEndianWriter = New BigEndianWriter()

            If int >= 0 And int <= MASK_0 Then
                local_2.WriteByte(CByte(int))
                WriteBytes(local_2.Data())
                Return
            End If

            Dim local_3 As Integer = int
            Dim local_4 As BigEndianWriter = New BigEndianWriter()

            While local_3 <> 0
                local_4.WriteByte(CByte(local_3 And MASK_0))
                local_4.Position = local_4.Data().Length - 1

                Dim local_4_reader As BigEndianReader = New BigEndianReader(local_4.Data())
                local_5 = local_4_reader.ReadByte()
                local_4 = New BigEndianWriter(local_4_reader.Data)

                local_3 = CInt(CUInt(local_3 >> CHUNCK_BIT_SIZE))
                If local_3 > 0 Then
                    local_5 = local_5 Or MASK_1
                End If
                local_2.WriteByte(CByte(local_5))
            End While

            WriteBytes(local_2.Data())
        End Sub

        Public Sub WriteVarShort(ByVal int As Integer) Implements IDataWriter.WriteVarShort
            Dim local_5 = 0
            If int > SHORT_MAX_VALUE Or int < SHORT_MIN_VALUE Then
                Throw New System.Exception("Forbidden value")
            Else
                Dim local_2 As BigEndianWriter = New BigEndianWriter()
                If int >= 0 And int <= MASK_0 Then
                    local_2.WriteByte(CByte(int))
                    WriteBytes(local_2.Data)
                    Return
                End If

                Dim local_3 = int And 65535
                Dim local_4 As BigEndianWriter = New BigEndianWriter()

                While local_3 <> 0
                    local_4.WriteByte(CByte(local_3 And MASK_0))
                    local_4.Position = local_4.Data.Length - 1

                    Dim local_4_reader As BigEndianReader = New BigEndianReader(local_4.Data)
                    local_5 = local_4_reader.ReadByte()
                    local_4 = New BigEndianWriter(local_4_reader.Data)

                    local_3 = CInt(CUInt(local_3 >> CHUNCK_BIT_SIZE))
                    If local_3 > 0 Then
                        local_5 = local_5 Or MASK_1
                    End If
                    local_2.WriteByte(CByte(local_5))
                End While
                WriteBytes(local_2.Data)
                Return
            End If
        End Sub

        Public Sub WriteVarLong(ByVal doubles As Double) Implements IDataWriter.WriteVarLong
            Dim i As UInt32 = 0
            Dim value As FinalInt64 = FinalInt64.FromNumber(doubles)
            If value.High = 0 Then
                WriteInt32(value.Low)
            Else
                i = 0
                While i < 4
                    WriteByte(value.Low And 127 Or 128)
                    value.Low = value.Low >> 7
                    i += 1
                End While
                If (value.High And 268435455 << 3) = 0 Then
                    WriteByte(value.High << 4 Or value.Low)
                Else
                    WriteByte((value.High << 4 Or value.Low) And 127 Or 128)
                    WriteInt32(value.High >> 3)
                End If
            End If
        End Sub

        Public Sub WriteInt32(ByVal Value As UInteger)
            While (True)
                If (Value < 128) Then
                    Exit While
                End If
                WriteByte(Value And 127 Or 128)
                Value = Value >> 7
            End While
            WriteByte(Value)
        End Sub


#End Region

#Region "Dispose"

        Public Sub Dispose()
            m_writer.Dispose()
            m_writer = Nothing
        End Sub

#End Region

        Public Sub Dispose1() Implements IDisposable.Dispose
            m_writer.Dispose()
            m_writer = Nothing
        End Sub
    End Class
End Namespace