Imports Sympraxis

Public Class Form1

    Public Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim arguments() = Environment.GetCommandLineArgs()
            If Not IsNothing(arguments) AndAlso arguments.Count >= 2 Then
                TextBox1.Text = arguments(1)
                TextBox2.Text = arguments(2)
                If TextBox1.Text <> "" AndAlso TextBox2.Text <> "" Then
                    If IO.Directory.Exists(TextBox1.Text) AndAlso IO.Directory.Exists(TextBox2.Text) Then
                        Dim pdfdiles() = IO.Directory.GetFiles(TextBox1.Text)
                        If Not IsNothing(pdfdiles) AndAlso pdfdiles.Count > 0 Then
                            For pdfcount = 0 To pdfdiles.Count - 1
                                Dim pdfFileName As String = ""
                                pdfFileName = pdfdiles(pdfcount).ToString
                                If IO.Path.GetExtension(pdfFileName).Trim.ToUpper = ".PDF" Then ExtractPagesFromSource(pdfFileName, "PDF", TextBox2.Text)
                            Next
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub


    Public Function ExtractPagesFromSource(sPDFileName As String, ByVal sInputFileType As String, ByVal saveFilePath As String) As Int32 '(ByRef clsInpFileRec As Sympraxis.MR.Upload.Filter.FilterBase.FilterBase.InputFileRec, _
        'myDocFile As Sympraxis.MR.Upload.Filter.CD.Meta.DocumentFile, _
        'ByRef myMetaFile As Sympraxis.MR.Upload.Filter.CD.Meta.CDMetaFile, _
        'ByRef clsAddlInput As Sympraxis.MR.Upload.Filter.FilterBase.FilterBase.AddlInputData, _
        'ByRef sErrMsg As String) As Int32
        Dim rc As Int32 = 0
        Dim clsFileIO As New Sympraxis.Utilities.ImageControl.FileIO.FileIO
        Dim y As Int32 = 0
        Dim z As Int32 = 0
        Dim aImageInfo As New List(Of Sympraxis.Utilities.ImageControl.FileIO.ImageInfo)
        Dim iImg As Int32 = 0
        Dim sTmp As String = ""
        Dim x As Int32 = 0
        Dim iMax As Int32 = 0



        Dim irc As Int32 = 0
        Dim ix As Int32 = 0
        Dim sSaveFile As String = ""
        Dim sSaveName As String = ""
        Dim oShortGuid As String = ""
        Dim oFullGuid As String = ""
        Dim bBurnAnno As Boolean = False
        Dim aTiffHdr As New List(Of Sympraxis.Utilities.ImageControl.FileIO.TiffHeader)
        Dim aAnnotText As New List(Of Sympraxis.Utilities.ImageControl.FileIO.AnnotationBase)
        Dim stResizeImage As Sympraxis.Utilities.ImageControl.FileIO.FileIO.ResizeImage

        Try
            If (sInputFileType = "PDF") Then
                If IO.Directory.Exists("c:\work\" & IO.Path.GetFileNameWithoutExtension(sPDFileName) & "\") = False Then
                    IO.Directory.CreateDirectory("c:\work\" & IO.Path.GetFileNameWithoutExtension(sPDFileName) & "\")
                End If
                Call clsFileIO.InitControl()

                aImageInfo.Add(New Sympraxis.Utilities.ImageControl.FileIO.ImageInfo)
                y = aImageInfo.Count - 1
                aImageInfo(y).Archived = False
                aImageInfo(y).FileName = sPDFileName
                aImageInfo(y).Height = 0
                aImageInfo(y).Hidden = False
                aImageInfo(y).PageIndex = y
                aImageInfo(y).PageType = ""
                aImageInfo(y).ReadOnly = False
                aImageInfo(y).StackIndex = y
                aImageInfo(y).StackName = "INPUT"
                aImageInfo(y).StackSuffix = ""

                clsFileIO.MaxImageCount = 10
                clsFileIO.CacheMethod = Sympraxis.Utilities.ImageControl.FileIO.CacheMethods.LoadPartial
                clsFileIO.TempFilepath = "c:\work\" & IO.Path.GetFileNameWithoutExtension(sPDFileName) & "\"

                If (IO.Directory.Exists(clsFileIO.TempFilepath) = True) Then
                Else
                    IO.Directory.CreateDirectory(clsFileIO.TempFilepath)
                End If


                rc = clsFileIO.LoadAndCacheImgs(aImageInfo)
                If (rc = 0) Then
                    iMax = clsFileIO.Images.Count

                    iImg = 0

                    'If (myDocFile.clsPage Is Nothing) Then

                    'Else
                    '    iMax = myDocFile.clsPage.aPageBlockItems.Count
                    'End If

                    'sTmp = "Extract FilePointer (" & myDocFile.sFilePointer & "), (" & iMax.ToString & " Pages)"
                    ''    myShowMsg(sTmp)
                    ''

                    Do While (iImg < clsFileIO.Images.Count) And (rc = 0)

                        sTmp = (iImg + 1).ToString
                        'If (myDocFile.IsPage(sTmp) = True) Then


                        irc = 0
                        ix = 0
                        sSaveFile = ""
                        sSaveName = ""
                        oShortGuid = ""
                        oFullGuid = ""
                        bBurnAnno = False
                        aTiffHdr = New List(Of Sympraxis.Utilities.ImageControl.FileIO.TiffHeader)
                        aAnnotText = New List(Of Sympraxis.Utilities.ImageControl.FileIO.AnnotationBase)
                        stResizeImage = New Sympraxis.Utilities.ImageControl.FileIO.FileIO.ResizeImage


                        sSaveName = IO.Path.GetFileNameWithoutExtension(sPDFileName) & "_" & iImg & ".tif"


                        sSaveFile = saveFilePath & "\" & sSaveName

                        rc = clsFileIO.SavePage(iImg, sSaveFile, aTiffHdr, aAnnotText, stResizeImage, False, bBurnAnno)




                        ' End If

                        iImg = iImg + 1

                    Loop
                    clsFileIO.Deletetempfiles(clsFileIO.TempFilepath)
                Else
                    '' sErrMsg = clsFileIO.sErrMsg
                End If
            End If

        Catch ex As Exception
            'rc = 1
            ''  sErrMsg = SetErrMsg("ExtractPagesFromSource", "")
        Finally
            If Not (clsFileIO Is Nothing) Then
                clsFileIO.ReleaseControl()
                'clsFileIO.RemoveAllCache()
                'clsFileIO.RemoveAllImages()
                clsFileIO = Nothing
            End If

            If Not (aImageInfo Is Nothing) Then
                aImageInfo.Clear()
                aImageInfo = Nothing
            End If

            ExtractPagesFromSource = rc
        End Try
    End Function


End Class
