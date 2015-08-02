Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace AmaknaCore.Common.IO
    Public Interface IDataReader
        Inherits IDisposable
        ReadOnly Property Position As Long

        ReadOnly Property BytesAvailable As Long

        ''' <summary>
        '''   Read a Short from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadShort() As Short

        ''' <summary>
        '''   Read a int from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadInt() As Integer

        ''' <summary>
        '''   Read a long from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadLong() As Int64


        ''' <summary>
        '''   Read a UShort from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadUShort() As UShort

        ''' <summary>
        '''   Read a int from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadUInt() As UInt32

        ''' <summary>
        '''   Read a long from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadULong() As UInt64

        ''' <summary>
        '''   Read a byte from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadByte() As Byte
        Function ReadSByte() As SByte

        ''' <summary>
        '''   Returns N bytes from the buffer
        ''' </summary>
        ''' <param name = "n">Number of read bytes.</param>
        ''' <returns></returns>
        Function ReadBytes(n As Integer) As Byte()

        ''' <summary>
        '''   Read a Boolean from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadBoolean() As [Boolean]

        ''' <summary>
        '''   Read a Char from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadChar() As [Char]

        ''' <summary>
        '''   Read a Double from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadDouble() As [Double]

        ''' <summary>
        '''   Read a Float from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadFloat() As Single

        ''' <summary>
        '''   Read a string from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadUTF() As String

        ''' <summary>
        '''   Read a string from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadUTFBytes(len As UShort) As String

        ''' <summary>
        '''   Read a int from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadVarInt() As Integer

        ''' <summary>
        '''   Read a uint from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadVarUhInt() As UInteger

        ''' <summary>
        '''   Read a int from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadVarShort() As Integer

        ''' <summary>
        '''   Read a uint from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadVarUhShort() As UInteger

        ''' <summary>
        '''   Read a double from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadVarLong() As Double

        ''' <summary>
        '''   Read a double from the Buffer
        ''' </summary>
        ''' <returns></returns>
        Function ReadVarUhLong() As Double

        Sub Seek(offset As Integer, seekOrigin As SeekOrigin)
        Sub SkipBytes(n As Integer)
    End Interface
End Namespace
