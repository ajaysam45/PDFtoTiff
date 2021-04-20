Imports NUnit.Framework

Namespace MyNewTest

    Public Class Tests

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub LoadTest()
            Dim MyForm As New PDFtoTiff.Form1

            MyForm.ExtractPagesFromSource("E:\Downloads\DifferencesBetweenWindowsXPEmbeddedandWindowsXPProfessional.pdf", ".tiff", "E:\Downloads\TIFF")

            Dim MyFiles() As String = IO.Directory.GetFiles("E:\Downloads\TIff", "*.tiff")
            Assert.Pass("SuccessfullCreatedtheFiles", MyFiles.Count = 6)
        End Sub

    End Class

End Namespace