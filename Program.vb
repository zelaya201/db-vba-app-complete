Imports System
Imports MySql.Data.MySqlClient

Module Program
    Dim connection As MySqlConnection

    Sub Main(args As String())
        Dim op As Integer
        Dim nombre As String
        Dim nie As Integer

        Connect()

        Do
            Console.Clear()
            Console.WriteLine("-- MENU --")
            Console.WriteLine("1. Mostrar")
            Console.WriteLine("2. Insertar")
            Console.WriteLine("3. Actualizar")
            Console.WriteLine("4. Eliminar")
            Console.WriteLine("5. Salir")
            Console.Write("Seleccione una opción: ")
            op = Console.ReadLine()

            Select Case op
                Case 1
                    Console.Clear()
                    Console.WriteLine("Mostrar")
                    Console.WriteLine(Show())
                Case 2
                    Console.Clear()
                    Console.WriteLine("Insertar")
                    Console.Write("Nombre: ")
                    nombre = Console.ReadLine()

                    Console.Write("NIE: ")
                    nie = Integer.Parse(Console.ReadLine())

                    Console.WriteLine(Insert(nombre, nie))
                Case 3
                    Console.Clear()
                    Console.WriteLine("Actualizar")
                    Console.WriteLine(Show()) ' Mostrar la lista de registros
                    Console.Write("Ingrese el NIE del alumno a actualizar: ")
                    nie = Integer.Parse(Console.ReadLine())
                    Console.Write("Nuevo Nombre: ")
                    nombre = Console.ReadLine()
                    Console.WriteLine(Update(nie, nombre))
                Case 4
                    Console.Clear()
                    Console.WriteLine("Eliminar")
                    Console.WriteLine(Show()) ' Mostrar la lista de registros
                    Console.Write("Ingrese el NIE del alumno a eliminar: ")
                    nie = Integer.Parse(Console.ReadLine())
                    Console.WriteLine(Delete(nie))
                Case 5
                    Console.WriteLine("Hasta pronto :)")
                Case Else
                    Console.WriteLine("Opción no válida")
            End Select

            Console.ReadKey()
        Loop While op <> 5

        connection.Close() ' Cerrar conexión al final
    End Sub

    Sub Connect()
        connection = New MySqlConnection("server=localhost;user=root;password=;database=db_escuela")
        connection.Open()
    End Sub

    Function Show() As String
        Dim result As String = ""
        Dim q As String = "SELECT * FROM alumno"

        Using cmd As New MySqlCommand(q, connection)
            Using reader As MySqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    result &= reader("id_alumno") & " - " & reader("nie_alumno") & " - " & reader("nombre_alumno") & vbCrLf
                End While
            End Using
        End Using

        Return result
    End Function

    Function Insert(nombre As String, nie As Integer) As String
        Dim sql As String = "INSERT INTO alumno(nombre_alumno, nie_alumno) VALUES (@nombre, @nie)"
        Using cmd As New MySqlCommand(sql, connection)
            cmd.Parameters.AddWithValue("@nombre", nombre)
            cmd.Parameters.AddWithValue("@nie", nie)
            cmd.ExecuteNonQuery()
        End Using

        Return "Registro insertado"
    End Function

    Function Update(nie As Integer, nuevoNombre As String) As String
        Dim sql As String = "UPDATE alumno SET nombre_alumno = @nuevoNombre WHERE nie_alumno = @nie"
        Using cmd As New MySqlCommand(sql, connection)
            cmd.Parameters.AddWithValue("@nuevoNombre", nuevoNombre)
            cmd.Parameters.AddWithValue("@nie", nie)

            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            If rowsAffected > 0 Then
                Return "Registro actualizado"
            Else
                Return "NIE no encontrado"
            End If
        End Using
    End Function

    Function Delete(nie As Integer) As String
        Dim sql As String = "DELETE FROM alumno WHERE nie_alumno = @nie"
        Using cmd As New MySqlCommand(sql, connection)
            cmd.Parameters.AddWithValue("@nie", nie)

            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            If rowsAffected > 0 Then
                Return "Registro eliminado"
            Else
                Return "NIE no encontrado"
            End If
        End Using
    End Function
End Module