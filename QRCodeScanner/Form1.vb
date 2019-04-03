Imports Emgu.CV
Imports ZXing

Public Class Form1

    Dim cap As New VideoCapture
    Dim barcodeReader As New BarcodeReader
    Dim nameList As New Dictionary(Of String, String)
    Dim priceList As New Dictionary(Of String, String)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim streamReader As IO.StreamReader = My.Computer.FileSystem.OpenTextFileReader(".\\products.csv")
        Dim line As String
        Do
            line = streamReader.ReadLine()
            If line Is Nothing Then Exit Do

            Dim splitLine As String() = line.Split(",")
            Dim id As String = splitLine(0)
            Dim name As String = splitLine(1)
            Dim price As String = splitLine(2)
            nameList.Add(id, name)
            priceList.Add(id, price)
        Loop
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            PictureBox1.Image = cap.QueryFrame.Bitmap()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Dim result As Result = barcodeReader.Decode(PictureBox1.Image)
        If result IsNot Nothing Then
            Timer1.Enabled = False
            Dim decoded As String = result.ToString().Trim()
            If nameList.ContainsKey(decoded) Then
                Try
                    TextBox1.Text = "商品: " + nameList.Item(decoded) + vbCrLf + "價格: $" + priceList.Item(decoded)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            Else
                TextBox1.Text = "查無此商品"
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox1.Text = ""
        Timer1.Enabled = True
    End Sub
End Class
