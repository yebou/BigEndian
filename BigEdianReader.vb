Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.IO

Namespace AmaknaCore.Common.IO
    Public Class BigEndianReader
        Implements IDataReader
        Implements IDisposable
#Region "Properties"

        Private m_reader As BinaryReader

        ''' <summary>
        '''   Gets availiable bytes number in the buffer
        ''' </summary>
        Public ReadOnly Property BytesAvailable() As Long Implements IDataReader.BytesAvailable
            Get
                Return m_reader.BaseStream.Length - m_reader.BaseStream.Position
            End Get
        End Property

        Public ReadOnly Property Position As Long Implements IDataReader.Position
            Get
                Return m_reader.BaseStream.Position
            End Get
        End Property


        Public ReadOnly Property BaseStream() As Stream
            Get
                Return m_reader.BaseStream
            End Get
        End Property

#End Region

#Region "Initialisation"

        ''' <summary>
        '''   Initializes a new instance of the <see cref = "BigEndianReader" /> class.
        ''' </summary>
        Public Sub New()
            m_reader = New BinaryReader(New MemoryStream(), Encoding.UTF8)
        End Sub

        ''' <summary>
        '''   Initializes a new instance of the <see cref = "BigEndianReader" /> class.
        ''' </summary>
        ''' <param name = "stream">The stream.</param>
        Public Sub New(stream As Stream)
            m_reader = New BinaryReader(stream, Encoding.UTF8)
        End Sub

        ''' <summary>
        '''   Initializes a new instance of the <see cref = "BigEndianReader" /> class.
        ''' </summary>
        ''' <param name = "tab">Memory buffer.</param>
        Public Sub New(tab As Byte())
            m_reader = New BinaryReader(New MemoryStream(tab), Encoding.UTF8)
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        '''   Read bytes in big endian format
        ''' </summary>
        ''' <param name = "count"></param>
        ''' <returns></returns>
        Private Function ReadBigEndianBytes(count As Integer) As Byte()
            Dim bytes(count) As Byte
            For i As Integer = 0 To count - 1
                bytes(i) = BaseStream.ReadByte()
            Next
            Return bytes
        End Function

#End Region

#Region "Public Method"

        ''' <summary>
        '''   Read a Short from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadShort() As Short Implements IDataReader.ReadShort
            Dim Value As UShort = ReadUInt16()
            If Value > Short.MaxValue Then
                Dim Value2 As Short = -(UShort.MaxValue - Value) - 1
                Return Value2
            End If
            Return Value
        End Function

        ''' <summary>
        '''   Read a int from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadInt() As Integer Implements IDataReader.ReadInt
            Dim Value As UInteger = ReadUInt32()
            If Value > Integer.MaxValue Then
                Dim Value2 As Integer = -(UInteger.MaxValue - Value) - 1
                Return Value2
            End If
            Return Value
        End Function

        ''' <summary>
        '''   Read a long from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadLong() As Int64 Implements IDataReader.ReadLong
            Return BitConverter.ToInt64(ReadBigEndianBytes(8), 0)
        End Function

        ''' <summary>
        '''   Read a Float from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadFloat() As Single Implements IDataReader.ReadFloat
            Return BitConverter.ToSingle(ReadBigEndianBytes(4), 0)
        End Function

        ''' <summary>
        '''   Read a UShort from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadUShort() As UShort Implements IDataReader.ReadUShort
            Return (CUShort(ReadByte()) << 8) + ReadByte()
        End Function

        ''' <summary>
        '''   Read a int from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadUInt() As UInt32 Implements IDataReader.ReadUInt
            Return (CUInt(ReadByte()) << 24) + (CUInt(ReadByte()) << 16) + (CUInt(ReadByte()) << 8) + ReadByte()
        End Function

        ''' <summary>
        '''   Read a long from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadULong() As UInt64 Implements IDataReader.ReadULong
            Return BitConverter.ToUInt64(ReadBigEndianBytes(8), 0)
        End Function

        ''' <summary>
        '''   Read a byte from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadByte() As Byte Implements IDataReader.ReadByte
            Return m_reader.ReadByte()
        End Function

        Public Function ReadSByte() As SByte Implements IDataReader.ReadSByte
            Return m_reader.ReadSByte()
        End Function

        Public ReadOnly Property Data() As Byte()
            Get
                Dim pos As Long = BaseStream.Position

                Dim data__1 As Byte() = New Byte(BaseStream.Length - 1) {}
                BaseStream.Position = 0
                BaseStream.Read(data__1, 0, CInt(BaseStream.Length))

                BaseStream.Position = pos

                Return data__1
            End Get
        End Property
        ''' <summary>
        '''   Returns N bytes from the buffer
        ''' </summary>
        ''' <param name = "n">Number of read bytes.</param>
        ''' <returns></returns>
        Public Function ReadBytes(n As Integer) As Byte() Implements IDataReader.ReadBytes
            Return m_reader.ReadBytes(n)
        End Function

        ''' <summary>
        ''' Returns N bytes from the buffer
        ''' </summary>
        ''' <param name = "n">Number of read bytes.</param>
        ''' <returns></returns>
        Public Function ReadBytesInNewBigEndianReader(n As Integer) As BigEndianReader
            Return New BigEndianReader(m_reader.ReadBytes(n))
        End Function

        ''' <summary>
        '''   Read a Boolean from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadBoolean() As [Boolean] Implements IDataReader.ReadBoolean
            Return m_reader.ReadByte() = 1
        End Function

        ''' <summary>
        '''   Read a Char from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadChar() As [Char] Implements IDataReader.ReadChar
            Return ChrW(ReadUShort())
        End Function

        ''' <summary>
        '''   Read a Double from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadDouble() As [Double] Implements IDataReader.ReadDouble
            Dim Bytes() As Byte = ReadBytes(8)
            Array.Reverse(Bytes)
            Return BitConverter.ToDouble(Bytes, 0)
        End Function

        ''' <summary>
        '''   Read a Single from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadSingle() As [Single]
            Return BitConverter.ToSingle(ReadBigEndianBytes(4), 0)
        End Function

        ''' <summary>
        '''   Read a string from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadUTF() As String Implements IDataReader.ReadUTF
            Dim length As UShort = ReadUShort()

            Dim bytes As Byte() = ReadBytes(length)
            Return Encoding.UTF8.GetString(bytes)
        End Function

        ''' <summary>
        '''   Read a string from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadUTF7BitLength() As String
            Dim length As Integer = ReadInt()

            Dim bytes As Byte() = ReadBytes(length)
            Return Encoding.UTF8.GetString(bytes)
        End Function

        ''' <summary>
        '''   Read a string from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Public Function ReadUTFBytes(len As UShort) As String Implements IDataReader.ReadUTFBytes
            Dim bytes As Byte() = ReadBytes(len)

            Return Encoding.UTF8.GetString(bytes)
        End Function

        ''' <summary>
        '''   Skip bytes
        ''' </summary>
        ''' <param name = "n"></param>
        Public Sub SkipBytes(n As Integer) Implements IDataReader.SkipBytes
            Dim i As Integer
            For i = 0 To n - 1
                m_reader.ReadByte()
            Next
        End Sub

        Public Sub SetPosition(Position As Integer)
            Seek(Position, SeekOrigin.Begin)
        End Sub

        Public Sub Seek(offset As Integer, seekOrigin As SeekOrigin) Implements IDataReader.Seek
            m_reader.BaseStream.Seek(offset, seekOrigin)
        End Sub

        ''' <summary>
        '''   Add a bytes array to the end of the buffer
        ''' </summary>
        Public Sub Add(data As Byte(), offset As Integer, count As Integer)
            Dim pos As Long = m_reader.BaseStream.Position

            m_reader.BaseStream.Position = m_reader.BaseStream.Length
            m_reader.BaseStream.Write(data, offset, count)
            m_reader.BaseStream.Position = pos
        End Sub

        Public Sub Close()
            BaseStream.Close()
        End Sub

#End Region

#Region "Alternatives Methods"
        Public Function ReadInt16() As Short
            Return ReadShort()
        End Function

        Public Function ReadInt32() As Integer
            Return ReadInt()
        End Function

        Public Function ReadInt64() As Long
            Return ReadLong()
        End Function

        Public Function ReadUInt16() As UShort
            Return ReadUShort()
        End Function

        Public Function ReadUInt32() As UInteger
            Return ReadUInt()
        End Function

        Public Function ReadUInt64() As ULong
            Return ReadULong()
        End Function

        Public Function ReadString() As String
            Return ReadUTF()
        End Function
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


        Public Function ReadVarInt() As Integer Implements IDataReader.ReadVarInt
            Dim local_4 = 0
            Dim local_1 = 0
            Dim local_2 = 0
            Dim local_3 = False

            While local_2 < INT_SIZE
                local_4 = ReadByte()
                local_3 = (local_4 And MASK_1) = MASK_1

                If local_2 > 0 Then
                    local_1 += ((local_4 And MASK_0) << local_2)
                Else
                    local_1 += (local_4 And MASK_0)
                End If

                local_2 += CHUNCK_BIT_SIZE

                If Not local_3 Then
                    Return local_1
                End If
            End While

            Throw New System.Exception("Too much data")
        End Function

        Public Function ReadVarUhInt() As UInteger Implements IDataReader.ReadVarUhInt
            Return CUInt(ReadVarInt())
        End Function

        Public Function ReadVarShort() As Integer Implements IDataReader.ReadVarShort
            Dim local_4 = 0
            Dim local_1 = 0
            Dim local_2 = 0
            Dim local_3 = False

            While local_2 < SHORT_SIZE
                local_4 = ReadByte()
                local_3 = (local_4 And MASK_1) = MASK_1

                If local_2 > 0 Then
                    local_1 += ((local_4 And MASK_0) << local_2)
                Else
                    local_1 += (local_4 And MASK_0)
                End If

                local_2 += CHUNCK_BIT_SIZE

                If Not local_3 Then
                    If local_1 > SHORT_MAX_VALUE Then
                        local_1 -= UNSIGNED_SHORT_MAX_VALUE
                    End If
                    Return local_1
                End If
            End While

            Throw New System.Exception("Too much data")
        End Function

        Public Function ReadVarUhShort() As UInteger Implements IDataReader.ReadVarUhShort
            Return CUInt(ReadVarShort())
        End Function

        Public Function ReadVarLong() As Double Implements IDataReader.ReadVarLong
            Return ReadVarInt64(Me).ToNumber()
        End Function

        Public Function ReadVarUhLong() As Double Implements IDataReader.ReadVarUhLong
            Return ReadVarUInt64(Me).ToNumber()
        End Function

        Public Function ReadVarInt64(reader As BigEndianReader) As FinalInt64
            Dim local_3 As Integer = 0
            Dim local_2 As New FinalInt64()
            Dim local_4 As Integer = 0

            While True
                local_3 = reader.ReadByte()
                If local_4 = 28 Then
                    Exit While
                End If
                If local_3 >= 128 Then
                    local_2.Low = local_2.Low Or CUInt((local_3 And 127) << local_4)
                    local_4 += 7
                    Continue While
                End If
                local_2.Low = local_2.Low Or CUInt(local_3 << local_4)
                Return local_2
            End While

            If local_3 >= 128 Then
                local_3 = local_3 And 127
                local_2.Low = local_2.Low Or CUInt(local_3 << local_4)
                local_2.High = CUInt(local_3 >> 4)
                local_4 = 3

                While True
                    local_3 = reader.ReadByte()
                    If local_4 < 32 Then
                        If local_3 >= 128 Then
                            local_2.High = local_2.High Or CUInt((local_3 And 127) << local_4)
                        Else
                            Exit While
                        End If
                    End If
                    local_4 += 7
                End While
                local_2.High = local_2.High Or CUInt(local_3 << local_4)
                Return local_2
            End If
            local_2.Low = local_2.Low Or CUInt(local_3 << local_4)
            local_2.High = CUInt(local_3 >> 4)
            Return local_2
        End Function

        Public Function ReadVarUInt64(reader As BigEndianReader) As FinalUInt64
            Dim local_3 As Integer = 0
            Dim local_2 As New FinalUInt64()
            Dim local_4 As Integer = 0

            While True
                local_3 = reader.ReadByte()
                If local_4 = 28 Then
                    Exit While
                End If
                If local_3 >= 128 Then
                    local_2.Low = local_2.Low Or CUInt((local_3 And 127) << local_4)
                    local_4 += 7
                    Continue While
                End If
                local_2.Low = local_2.Low Or CUInt(local_3 << local_4)
                Return local_2
            End While

            If local_3 >= 128 Then
                local_3 = local_3 And 127
                local_2.Low = local_2.Low Or CUInt(local_3 << local_4)
                local_2.High = CUInt(local_3 >> 4)
                local_4 = 3

                While True
                    local_3 = reader.ReadByte()
                    If local_4 < 32 Then
                        If local_3 >= 128 Then
                            local_2.High = local_2.High Or CUInt((local_3 And 127) << local_4)
                        Else
                            Exit While
                        End If
                    End If
                    local_4 += 7
                End While
                local_2.High = local_2.High Or CUInt(local_3 << local_4)
                Return local_2
            End If
            local_2.Low = local_2.Low Or CUInt(local_3 << local_4)
            local_2.High = CUInt(local_3 >> 4)
            Return local_2
        End Function

#End Region

#Region "Credentials"

        Public CredentialsReader As BinaryReader

        Public Sub InitCredentials(ByVal ByteData() As Byte)
            CredentialsReader = New BinaryReader(New MemoryStream(ByteData), Encoding.UTF8)
        End Sub

        Public Function ReadUtfCredentials()
            Dim length As UShort = ReadUShortCredentials(CredentialsReader)
            Dim bytes() As Byte = CredentialsReader.ReadBytes(length)
            Return Encoding.UTF8.GetString(bytes)
        End Function

        Public Function ReadByteCredentials(ByVal Reader As BinaryReader) As Byte
            Return Reader.ReadByte()
        End Function

        Public Function ReadUShortCredentials(ByVal reader As BinaryReader) As UShort
            Return (CUShort(ReadByteCredentials(reader)) << 8) + ReadByteCredentials(reader)
        End Function

#End Region

#Region "Dispose"

        ''' <summary>
        '''   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            m_reader.Dispose()
            m_reader = Nothing
        End Sub

#End Region
    End Class
End Namespace
