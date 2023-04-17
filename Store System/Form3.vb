Imports MySql.Data.MySqlClient

Public Class Form3
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Connect_to_DB()
    End Sub

    Private Sub registerButton_Click(sender As Object, e As EventArgs) Handles registerButton.Click
        Try
            If String.IsNullOrEmpty(userName.Text) OrElse String.IsNullOrEmpty(firstName.Text) OrElse
               String.IsNullOrEmpty(lastName.Text) OrElse String.IsNullOrEmpty(email.Text) OrElse
               String.IsNullOrEmpty(userPasswordRegister1.Text) OrElse String.IsNullOrEmpty(userPasswordRegister2.Text) Then
                MsgBox("please fill up the form!")
                Exit Sub
                ' Check if the passwords match
            ElseIf userPasswordRegister1.Text = userPasswordRegister2.Text Then

                ' Check if the username already exists in the database
                Using cmd As New MySqlCommand("SELECT COUNT(*) FROM users WHERE userName = ?", myconn)
                    cmd.Parameters.AddWithValue("@userName", userName.Text)
                    Dim count As Integer = CInt(cmd.ExecuteScalar())
                    If count > 0 Then
                        MsgBox("Username already exists!")
                        Exit Sub
                    End If
                End Using
                ' Insert the user information into the database using parameterized queries
                Using cmd As New MySqlCommand("INSERT INTO users (userName, firstName, lastName, userEmail, userPassword) VALUES (?,?,?,?,MD5(?))", myconn)
                    cmd.Parameters.AddWithValue("@userName", userName.Text)
                    cmd.Parameters.AddWithValue("@firstName", firstName.Text)
                    cmd.Parameters.AddWithValue("@lastName", lastName.Text)
                    cmd.Parameters.AddWithValue("@userEmail", email.Text)
                    cmd.Parameters.AddWithValue("@userPassword", userPasswordRegister1.Text)
                    cmd.ExecuteNonQuery()
                End Using
                MsgBox("User registered successfully!")
                Call Disconnect_to_DB()
                Form1.Show()
                Me.Close()
            Else
                MsgBox("Passwords do not match!")
            End If
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub loginButton_Click(sender As Object, e As EventArgs) Handles loginButton.Click
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub
End Class