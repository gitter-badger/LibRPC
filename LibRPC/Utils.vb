﻿Imports System.IO
Imports System.Threading

Public NotInheritable Class Utils

	Public Shared Property DeserializerOverride As Func(Of BinaryReader, Type, Object)
	Public Shared Property SerializerOverride As Action(Of BinaryWriter, Object)

	Public Shared Sub WaitFor(condition As Func(Of Boolean))
		Dim sw As New SpinWait
		While Not condition()
			sw.SpinOnce()
		End While
	End Sub

	Public Shared Sub Locked(Of T As Class)(target As T, action As Action)
		SyncLock target
			action()
		End SyncLock
	End Sub

	Public Shared Function Async(target As Action) As Task
		Return Task.Run(target)
	End Function

	Public Shared Function DeserializeBase(input As BinaryReader, type As Type) As Object
		If DeserializerOverride IsNot Nothing Then Return DeserializerOverride(input, type)
		If type Is GetType(Boolean) Then
			Return input.ReadBoolean()
		ElseIf type Is GetType(Byte) Then
			Return input.ReadByte()
		ElseIf type Is GetType(Char) Then
			Return input.ReadChar()
		ElseIf type Is GetType(Short) Then
			Return input.ReadInt16()
		ElseIf type Is GetType(Integer) Then
			Return input.ReadInt32()
		ElseIf type Is GetType(Long) Then
			Return input.ReadInt64()
		ElseIf type Is GetType(SByte) Then
			Return input.ReadSByte()
		ElseIf type Is GetType(UShort) Then
			Return input.ReadUInt16()
		ElseIf type Is GetType(UInteger) Then
			Return input.ReadUInt32()
		ElseIf type Is GetType(ULong) Then
			Return input.ReadUInt64()
		ElseIf type Is GetType(Single) Then
			Return input.ReadSingle()
		ElseIf type Is GetType(Double) Then
			Return input.ReadDouble()
		ElseIf type Is GetType(Decimal) Then
			Return input.ReadDecimal()
		ElseIf type Is GetType(String) Then
			Return input.ReadString()
		ElseIf type Is GetType(Guid) Then
			Return New Guid(input.ReadBytes(16))
		Else
			Throw New SerializationException("Unable to serialize: " + type.FullName)
		End If
	End Function

	Public Shared Sub SerializeBase(output As BinaryWriter, obj As Object)
		If SerializerOverride IsNot Nothing Then SerializerOverride.Invoke(output, obj)
		Dim type = obj.GetType()
		If type Is GetType(Boolean) Then
			output.Write(DirectCast(obj, Boolean))
		ElseIf type Is GetType(Byte) Then
			output.Write(DirectCast(obj, Byte))
		ElseIf type Is GetType(Char) Then
			output.Write(DirectCast(obj, Char))
		ElseIf type Is GetType(Short) Then
			output.Write(DirectCast(obj, Short))
		ElseIf type Is GetType(Integer) Then
			output.Write(DirectCast(obj, Integer))
		ElseIf type Is GetType(Long) Then
			output.Write(DirectCast(obj, Long))
		ElseIf type Is GetType(SByte) Then
			output.Write(DirectCast(obj, SByte))
		ElseIf type Is GetType(UShort) Then
			output.Write(DirectCast(obj, UShort))
		ElseIf type Is GetType(UInteger) Then
			output.Write(DirectCast(obj, UInteger))
		ElseIf type Is GetType(ULong) Then
			output.Write(DirectCast(obj, ULong))
		ElseIf type Is GetType(Single) Then
			output.Write(DirectCast(obj, Single))
		ElseIf type Is GetType(Double) Then
			output.Write(DirectCast(obj, Double))
		ElseIf type Is GetType(Decimal) Then
			output.Write(DirectCast(obj, Decimal))
		ElseIf type Is GetType(String) Then
			output.Write(DirectCast(obj, String))
		ElseIf type Is GetType(Guid) Then
			output.Write(DirectCast(obj, Guid).ToByteArray())
		Else
			Throw New SerializationException("Unable to deserialize: " + type.FullName)
		End If
	End Sub

End Class