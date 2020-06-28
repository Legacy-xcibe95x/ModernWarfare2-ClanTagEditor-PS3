Imports FileCheetah
Imports System.IO
Imports DevComponents.DotNetBar
Imports System.Text.RegularExpressions

Public Class Form1
    Public OFD As New OpenFileDialog
    Public offset As Integer       ' This Is going to end up being the Offset of the clan tag
    Public offset2 As Integer       ' This Is going to end up being the Offset of the motd
    Public Finder As ASCIISearch   'This will be used to Find the Dvar "set clanName"
    Public Filepath As String      ' This string will hold the Path of the file
    Public open As Boolean  ' Will use to make sure the user has a file open
    Private Sub button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        OFD.Filter = "All Files (*.*)|*.*"
        If OFD.ShowDialog = DialogResult.OK Then 'Ensures the user Selected a File

            TAGCLAN()
            MOTD()

        End If


    End Sub


    Public Sub log(ByVal log As String) ' so i dont have to keep typing ToolStripStatusLabel1.Text :0
        ToolStripStatusLabel1.Text = log
    End Sub
    Public Sub mb(ByVal mb As String)  ' creates a nice looking messagebox for us
        Dim d As New MessageBoxEx()

        MessageBoxEx.Show(mb, _
            "Modded", _
            MessageBoxButtons.OK, _
            MessageBoxIcon.Information
            )
    End Sub
    Private Sub button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If open = True Then
            Dim reg As New Regex("\s*")
            clantag.Text = reg.Replace(clantag.Text, "") ' my attempt to replace all white space from the textbox
            If clantag.Text.Length < 4 Then
                mb("Enter 4 Characters First!!")
            Else
                Try
                    log("Writing Clan Tag...")
                    Dim stream As New FileStream(Filepath, FileMode.Open)
                    Dim writer As New BinaryWriter(stream)
                    writer.BaseStream.Position = offset
                    For Each character As Char In clantag.Text
                        writer.Write(CChar(character))            ' Write each character in textbox1
                    Next
                    writer.Close()
                    writer.Dispose()
                    stream.Close()
                    stream.Dispose()
                    log("Idle")
                    mb("Clan Tag Modded to  " + clantag.Text)
                Catch
                    mb("Error Writing Clan Tag!")
                    log("Idle")
                    Return
                End Try
            End If

        Else
            mb("Open a file First!")
            log("Idle")
        End If


        If open = True Then
            Dim FixRemainingText As String = RichTextBox1.Text

            RichTextBox1.Text = FixRemainingText.PadRight(73, " ")

            Try
                log("Writing Motd...")
                Dim stream As New FileStream(Filepath, FileMode.Open)
                Dim writer2 As New BinaryWriter(stream)
                writer2.BaseStream.Position = offset2
                For Each character As Char In RichTextBox1.Text
                    writer2.Write(CChar(character))            ' Write each character in textbox1
                Next
                writer2.Close()
                writer2.Dispose()
                stream.Close()
                stream.Dispose()
                log("Idle")
                mb("Motd Modded to  " + RichTextBox1.Text)
            Catch
                mb("Error Writing Motd!")
                log("Idle")
                Return
            End Try

        Else
            mb("Open a file First!")
            log("Idle")
        End If


    End Sub
  
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Button4.Enabled = False
    End Sub

    Private Sub RibbonControl1_Click(sender As System.Object, e As System.EventArgs)

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Form3.Show()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Form4.Show()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Form2.Show()
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs)

    End Sub


    Private Sub TAGCLAN()
        Try
            Filepath = Me.OFD.FileName
            Finder = New ASCIISearch(Me.Filepath, "set clanName", 1)
            log("Searching For Clan Tag...")
            If (Finder.Search = 0) Then
                Button4.Enabled = False
                mb("No Clan Tag Found")
                log("Idle")
                open = False

            Else ' if no clan tag is found then
                log("Calculating Offset...")
                offset = Me.Finder.Search
                offset = (Me.offset + &H9F)
                log("Fetching Clan Tag...")
                Dim input As New FileStream(Me.Filepath, FileMode.Open)
                Dim reader As New BinaryReader(input)
                reader.BaseStream.Position = Me.offset
                clantag.Text = New String(reader.ReadChars(4))
                reader.Close()
                reader.Dispose()
                input.Close()
                input.Dispose()
                Button4.Enabled = True
                log("Idle")
                open = True

            End If


        Catch ' If their is an error reading the characters
            Button4.Enabled = False
            mb("Error Reading Clan tag")
            log("Idle")
            open = False
            Filepath = Nothing
            Return
        End Try


    End Sub

    Private Sub MOTD()
        Try
            Filepath = Me.OFD.FileName
            Finder = New ASCIISearch(Me.Filepath, "motd", 1)
            log("Searching For MOTD...")
            If (Finder.Search = 0) Then
                Button4.Enabled = False
                mb("No MOTD Found")
                log("Idle")
                open = False
            Else
                log("Calculating Offset...")
                offset2 = Finder.Search
                offset2 = (offset2 + &H4F)
                log("Fetching MOTD...")
                Dim input As New FileStream(Filepath, FileMode.Open)
                Dim reader As New BinaryReader(input)
                reader.BaseStream.Position = offset2
                RichTextBox1.Text = New String(reader.ReadChars(&H49))
                reader.Close()
                reader.Dispose()
                input.Close()
                input.Dispose()
                Button4.Enabled = True
                log("Idle")
                open = True
            End If



        Catch ' If their is an error reading the characters
            Button4.Enabled = False
            mb("Error Reading Motd")
            log("Idle")
            open = False
            Filepath = Nothing
            Return
        End Try
    End Sub

    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles Button6.Click
        clantag.Text = "{CB}"
    End Sub
End Class