Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace AmaknaCore.Common.IO
    Public Interface IDataWriter
        ReadOnly Property Data() As Byte()

        ''' <summary>
        '''   Write a Short into the buffer
        ''' </summary>
        Sub WriteShort([short] As Short)

        ''' <summary>
        '''   Write a int into the buffer
        ''' </summary>
        Sub WriteInt(int As Integer)

        ''' <summary>
        '''   Write a long into the buffer
        ''' </summary>
        Sub WriteLong([long] As Int64)

        ''' <summary>
        '''   Write a UShort into the buffer
        ''' </summary>
        Sub WriteUShort([ushort] As UShort)

        ''' <summary>
        '''   Write a int into the buffer
        ''' </summary>
        Sub WriteUInt(uint As UInt32)

        ''' <summary>
        '''   Write a long into the buffer
        ''' </summary>
        Sub WriteULong([ulong] As UInt64)

        ''' <summary>
        '''   Write a byte into the buffer
        ''' </summary>
        Sub WriteByte([byte] As Byte)
        Sub WriteSByte([byte] As SByte)

        ''' <summary>
        '''   Write a Float into the buffer
        ''' </summary>
        Sub WriteFloat(float As Single)

        ''' <summary>
        '''   Write a Boolean into the buffer
        ''' </summary>
        Sub WriteBoolean(bool As [Boolean])

        ''' <summary>
        '''   Write a Char into the buffer
        ''' </summary>
        Sub WriteChar([char] As [Char])

        ''' <summary>
        '''   Write a Double into the buffer
        ''' </summary>
        Sub WriteDouble([double] As [Double])

        ''' <summary>
        '''   Write a Single into the buffer
        ''' </summary>
        Sub WriteSingle([single] As [Single])

        ''' <summary>
        '''   Write a string into the buffer
        ''' </summary>
        Sub WriteUTF(str As String)

        ''' <summary>
        '''   Write a string into the buffer
        ''' </summary>
        Sub WriteUTFBytes(str As String)

        ''' <summary>
        '''   Write bytes array into the buffer
        ''' </summary>
        Sub WriteBytes(data As Byte())

        ''' <summary>
        '''   Write bytes array into the buffer
        ''' </summary>
        Sub WriteBytes(data As Byte(), offset As UInteger, length As UInteger)

        ''' <summary>
        '''   Write a int array into the buffer
        ''' </summary>
        Sub WriteVarInt(int As Integer)

        ''' <summary>
        '''   Write a int array into the buffer
        ''' </summary>
        Sub WriteVarShort(int As Integer)

        ''' <summary>
        '''   Write a double array into the buffer
        ''' </summary>
        Sub WriteVarLong([double] As Double)

        Sub Clear()
    End Interface
End Namespace
