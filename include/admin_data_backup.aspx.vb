
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.VisualBasic.FileSystem


Public Class admin_data_backup
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnBackup As System.Web.UI.WebControls.Button
    Protected WithEvents btnRestore As System.Web.UI.WebControls.Button
    Protected lblFolderNotExist As Label
    Protected lblUnexpectedBackupErr As Label
    Protected lblNoFileGeneratedErr As Label
    Protected filUpload As HtmlInputFile
    Protected lblUploaded As Label
    Protected hidRestorePath As HtmlInputHidden
    Protected lblRestoreSuccess As Label
    Protected lblUnexpectedRestoreErr As Label

    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub

#End Region

    Private strServer As String
    Private strDatabase As String
    Private strUserID As String
    Private strPassword As String
    Private strSqlConn As String
    Private strFtpPath As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.lblFolderNotExist.Visible = False
        Me.lblUnexpectedBackupErr.Visible = False
        Me.lblRestoreSuccess.Visible = False
        Me.lblUploaded.Visible = False
        Me.lblUnexpectedRestoreErr.Visible = False
        Me.lblNoFileGeneratedErr.Visible = False
    End Sub

#Region " -- Backup -- "
    Private Sub btnBackup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackup.Click
        If Directory.Exists(GetBackupPath()) Then
            Dim objSqlConn As SqlConnection
            
            GetDBStringProp()
            objSqlConn = New SqlConnection(strSqlConn)
            
            Try
                RunSP(objSqlConn, "AD_DATA_BACKUP", _
                      "@DBName", strDatabase, _
                      "@BackupPath", GetBackupPath() & GenBackupFileName())
                Dim fiBackup As New FileInfo(GetBackupPath() & GenBackupFileName())
                If fiBackup.Exists Then
                    PopUpSaveDialog()
                Else
                    Me.lblNoFileGeneratedErr.Visible = True
                End If
            Catch ex As System.Exception
                Me.lblUnexpectedBackupErr.Visible = True
            Finally
                objSqlConn = Nothing
            End Try
        Else
            Me.lblFolderNotExist.Visible = True
        End If
    End Sub

    Private Sub PopUpSaveDialog()
        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment;filename=" & Chr(34) & GenBackupFileName() & Chr(34))
        Response.ContentType = "application/x-unknown"
        Response.BinaryWrite(GetBinaryFile(GetBackupPath() & GenBackupFileName()))
    End Sub

    Private Function GetBinaryFile(ByVal strFileName As String) As Object
        Dim objStream As Object = CreateObject("ADODB.Stream")
        objStream.Open()
        objStream.Type = 1
        objStream.LoadFromFile(strFileName)
        GetBinaryFile = objStream.Read
        objStream = Nothing
    End Function

    Private Sub GetDBStringProp()
        Dim objConn As New agri.Admin.clsBackupRestore
        objConn.GetDBStringProp(strServer, strDatabase, strUserID, strPassword)
        objConn = Nothing

        Me.strSqlConn = "server=" & Me.strServer & ";User ID=" & Me.strUserID & ";Password=" & _
                        Me.strPassword & ";database=" & Me.strDatabase & ";Connection Reset=FALSE"
    End Sub

    Private Function GenBackupFileName() As String
        Return "Plantware_" & GetReverseDate() & ".bak"
    End Function

    Private Function GetBackupPath() As String
        If Me.strFtpPath = "" Then
            Dim objSysCfg As New agri.PWSystem.clsConfig
            Dim intErrNum As Integer

            intErrNum = objSysCfg.mtdGetFtpPath(strFtpPath)
            objSysCfg = Nothing
            Return strFtpPath
        Else
            Return strFtpPath
        End If
    End Function

    Private Function GetReverseDate() As String
        Dim strYear As String = Year(Now).ToString
        Dim strMonth As String = Month(Now).ToString
        If Len(strMonth) = 1 Then strMonth = "0" & strMonth
        Dim strDay As String = Day(Now).ToString
        If Len(strDay) = 1 Then strDay = "0" & strDay
        Return strYear & strMonth & strDay
    End Function
#End Region

#Region " -- Restore -- "
    Private Sub btnRestore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRestore.Click
        If IsUploadSuccess() Then
            Dim objSqlConn As SqlConnection
            GetDBStringProp()
            BuildMasterConnStr()

            objSqlConn = New SqlConnection(strSqlConn)

            Try
                RunSP(objSqlConn, "AD_DATA_RESTORE", _
                      "@DBName", strDatabase, _
                      "@RestorePath", Me.hidRestorePath.Value)
                Me.lblRestoreSuccess.Visible = True

            Catch ex As System.Exception
                Me.lblUnexpectedRestoreErr.Visible = True
            Finally
                objSqlConn = Nothing
            End Try
        End If
    End Sub

    Private Function IsUploadSuccess() As Boolean
        If Me.filUpload.PostedFile.FileName <> "" Then
            If Directory.Exists(GetRestorePath) Then
                Dim objFileInfo As New FileInfo(Me.filUpload.PostedFile.FileName.ToString)
                Me.filUpload.PostedFile.SaveAs(GetRestorePath() & objFileInfo.Name)
                Me.hidRestorePath.Value = GetRestorePath() & objFileInfo.Name
                objFileInfo = Nothing
                Return True
            Else
                Me.lblUploaded.Text = "Upload folder does not exist"
                Me.lblUploaded.Visible = True
                Return False
            End If
        Else
            Me.lblUploaded.Text = "No file selected"
            Me.lblUploaded.Visible = True
            Return False
        End If
    End Function

    Private Function GetRestorePath() As String
        If Me.strFtpPath = "" Then
            Dim objSysCfg As New agri.PWSystem.clsConfig
            Dim intErrNum As Integer

            intErrNum = objSysCfg.mtdGetFtpPath(strFtpPath)
            objSysCfg = Nothing
            Return strFtpPath
        Else
            Return strFtpPath
        End If
    End Function

    Private Sub BuildMasterConnStr()
        Me.strSqlConn = "server=" & Me.strServer & ";User ID=" & Me.strUserID & ";Password=" & _
                        Me.strPassword & ";database=master;Connection Reset=FALSE"
    End Sub
#End Region

#Region " -- DBHelper -- "
    Private Function RunSP(ByRef Connection As SqlConnection, ByVal sProcName As String, _
                           ByVal ParamArray args As Object()) As Integer
        Dim cnConn As SqlConnection
        Dim cmdSql As SqlCommand
        Dim intRows As Integer

        cnConn = Connection
        cmdSql = GetCommandSP(cnConn, sProcName, args)
        cnConn.Open()
        Return cmdSql.ExecuteScalar()
        cnConn.Close()
    End Function

    Private Function GetCommandSP(ByRef cnConn As SqlConnection, _
                                  ByVal sProcName As String, ByVal ParamArray args As Object()) As SqlCommand

        Dim cmdSql As SqlCommand = New SqlCommand(sProcName, cnConn)
        cmdSql.CommandType = CommandType.StoredProcedure

        Dim i As Integer = 0
        While (i < args.Length)
            If ((i + 1) >= args.Length) Then Exit While
            Dim sParamName As String = CType(args(i), String)
            Dim oVal As Object = args(i + 1)
            If IsDBNull(oVal) Then
                cmdSql.Parameters.Add(New SqlParameter(sParamName, oVal))
            Else
                If Len(oVal) >= 4000 Then
                    Dim NewParameter As New SqlParameter
                    With NewParameter
                        .ParameterName = sParamName
                        .SqlDbType = SqlDbType.Text
                        .Direction = ParameterDirection.Input
                        .Value = oVal
                    End With
                    cmdSql.Parameters.Add(NewParameter)
                Else
                    cmdSql.Parameters.Add(New SqlParameter(sParamName, oVal))
                End If
            End If
            i += 2
        End While
        Return cmdSql
    End Function
#End Region

#Region " -- Junk -- "



#End Region


End Class
