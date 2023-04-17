Imports MySql.Data.MySqlClient
Imports System.Windows

Public Class Form1
    Private Sub loginButton_Click(sender As Object, e As EventArgs) Handles loginButton.Click
        With Me
            Call Connect_to_DB()
            Dim mycmd As New MySqlCommand
            strSQL = "SELECT COUNT(*) FROM users WHERE userName = '" & .userName.Text & "' AND userPassword = MD5('" & .userPassword.Text & "')"
            mycmd.CommandText = strSQL
            mycmd.Connection = myconn

            Dim count As Integer = CInt(mycmd.ExecuteScalar())

            If count > 0 Then
                MsgBox("Login successful!")
                Form2.Show()
                Me.Hide()
            Else
                MsgBox("Invalid username or password")
            End If
            Call Disconnect_to_DB()
        End With
    End Sub

    Private Sub registerButton_Click(sender As Object, e As EventArgs) Handles registerButton.Click
        Me.Hide()
        Form3.Show()
    End Sub
End Class
