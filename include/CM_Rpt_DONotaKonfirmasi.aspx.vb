
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic
Imports System.XML
Imports System.Web.Services
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Imports agri.CM.clsTrx
Imports agri.Admin.clsShare
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GL.clsTrx

Public Class CM_Rpt_DONotaKonfirmasi : inherits page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents lblErrMesage As Label

    Dim objCMRpt As New agri.CM.clsReport
    Dim objCMTrx As New agri.CM.clsTrx
    Dim objWMTrx As New agri.WM.clsTrx
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
	Dim objGL As New agri.GL.clsTrx()

    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String

    Dim strTitle As String
    Dim strContractNo As String
    Dim strTerbilangQty As String
    Dim strTerbilangPrice As String
    Dim strTerbilangTotal As String   
    Dim strCurrencyDesc As String

    Dim strProduct As String
	Dim strDO As String
    Dim strBillParty As String
    Dim strBuyer As String
    Dim strOwner1 As String
    Dim strOwner2 As String
    Dim strPPN As String
    Dim strPPNInclude As String
    Dim strPengiriman As String
    Dim strAsalBarang As String
    Dim strExportToExcel As String
	Dim strDraf As String

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        
        crvView.Visible = False  

        strContractNo = Server.UrlDecode(Request.QueryString("strContractNo").Trim())
        strDO = Server.UrlDecode(Request.QueryString("strDONo").Trim())
        strPPN = "" 'Request.QueryString("PPN")
        strPPNInclude = "" 'Request.QueryString("PPNInclude")
        strPengiriman = "" 'Request.QueryString("Pengiriman")
        strAsalBarang = "" 'Request.QueryString("AsalBarang")
        strExportToExcel = Trim(Request.QueryString("strExportToExcel"))
        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim crExportOptions As ExportOptions
        Dim objRptDs As New DataSet
        Dim objMapPath As New Object()
        Dim strOpCd As String = "CM_STDRPT_CONTRACT_NOTAKONFIRMASI"
        Dim strSearch As String
        Dim strParameter As String
        Dim strNoContract As String
        Dim strNoDO As String
		Dim strTimeProd As String
        Dim strQty As String
        Dim strPrice As String
        Dim strTotal As String
		Dim intErrNo As Integer

        strNoContract = Trim(strContractNo)
        strNoDO = Trim(strDO)
	
        strParameter = "CONTRACT|DO"
		strSearch = strNoContract & "|" &  strNoDO

        Try
			intErrNo = objGL.mtdGetDataCommon(strOpCd, strParameter, strSearch,objRptDs)
	    Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        Try
            If objAdmin.mtdGetBasePath(objMapPath) <> 0 Then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        rdCrystalViewer.Load(objMapPath & "Web\EN\CM\Reports\Crystal\CM_Rpt_NotaKonfirmasi.rpt", OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

        If Not blnIsPDFFormat Then
            crvView.Visible = True
            crvView.ReportSource = rdCrystalViewer
            crvView.DataBind()
            
        Else
            crvView.Visible = False
            crvView.ReportSource = rdCrystalViewer
            crvView.DataBind()
               

            crDiskFileDestinationOptions = New DiskFileDestinationOptions()
            If strExportToExcel = "0" Then
                crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_Rpt_NotaKonfirmasi" & ".pdf"
            Else
                crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_Rpt_NotaKonfirmasi" & ".xls"
            End If

            crExportOptions = rdCrystalViewer.ExportOptions
            With crExportOptions
                .DestinationOptions = crDiskFileDestinationOptions
                .ExportDestinationType = ExportDestinationType.DiskFile
                If strExportToExcel = "0" Then
                    .ExportFormatType = ExportFormatType.PortableDocFormat
                Else
                    .ExportFormatType = ExportFormatType.Excel
                End If
            End With

            rdCrystalViewer.Export()
            rdCrystalViewer.Close()
            rdCrystalViewer.Dispose()

            If strExportToExcel = "0" Then
                Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_Rpt_NotaKonfirmasi.pdf"">")
            Else
                Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_Rpt_NotaKonfirmasi.xls"">")
            End If

            objRptDs.Dispose()
            If Not objRptDs Is Nothing Then
                objRptDs = Nothing
            End If
        End If

    End Sub
   



End Class

