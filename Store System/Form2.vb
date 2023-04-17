Imports MySql.Data.MySqlClient
Imports System.IO
Public Class Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim result As DialogResult = MessageBox.Show("Do you want to continue?", "LOGOUT?", MessageBoxButtons.YesNo)

        If result = DialogResult.Yes Then
            Me.Close()
        Else

        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim backup As New SaveFileDialog
        backup.InitialDirectory = "C:\"
        backup.Title = "Database Backup"
        backup.CheckFileExists = False
        backup.CheckFileExists = False
        backup.DefaultExt = "sql"
        backup.Filter = "sql files (*.sql) | *.ssql | All files (*.*) | *.*"
        backup.RestoreDirectory = True

        If backup.ShowDialog = Windows.Forms.DialogResult.OK Then
            Call Connect_to_DB()
            Dim cmd As MySqlCommand = New MySqlCommand
            cmd.Connection = myconn
            Dim mb As MysqlBackup = New MysqlBackup(cmd)
            mb.ExportToFile(backup.FileName)
            myconn.Close()
            MessageBox.Show("Database backup successful!", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)

        ElseIf backup.showDialog = Windows.Forms.DialogResult.Cancel Then
            Return
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim import As New OpenFileDialog
        import.InitialDirectory = "C:"
        import.Title = "Import CSV File"
        import.CheckFileExists = True
        import.DefaultExt = "csv"
        import.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
        import.RestoreDirectory = True

        If import.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim fieldNames As String() = {"product_name", "product_quantity", "product_price", "product_supplier"}

            Dim lines As String() = File.ReadAllLines(import.FileName)
            Dim header As String = lines(0)
            Dim headerFields As String() = header.Split(","c)

            If Not fieldNames.SequenceEqual(headerFields) Then
                MessageBox.Show("Invalid CSV file format. Please make sure the file has the correct header fields.", "Import CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Load the CSV file into a DataTable
            Dim dt As New DataTable()
            For Each columnName In headerFields
                dt.Columns.Add(columnName)
            Next

            For i As Integer = 1 To lines.Length - 1
                Dim fields As String() = lines(i).Split(","c)
                dt.Rows.Add(fields)
            Next

            ' Bind the DataTable to the DataGridView
            DataGridView1.DataSource = dt

            MessageBox.Show("CSV file loaded successfully!", "Load CSV", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If DataGridView1.DataSource Is Nothing Then
            MessageBox.Show("Please load a CSV file first.", "Import CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Call Connect_to_DB()
        Dim cmd As MySqlCommand = New MySqlCommand
        cmd.Connection = myconn

        For Each row As DataRow In CType(DataGridView1.DataSource, DataTable).Rows
            Dim query As String = String.Format("INSERT INTO products (product_name, product_quantity, product_price, product_supplier) VALUES ('{0}', '{1}', '{2}', '{3}')",row("product_name"), row("product_quantity"), row("product_price"), row("product_supplier"))
            cmd.CommandText = query
            cmd.ExecuteNonQuery()
        Next

        ' Clear the data grid
        DataGridView1.DataSource = Nothing
        myconn.Close()
        MessageBox.Show("CSV file imported successfully!", "Import CSV", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class