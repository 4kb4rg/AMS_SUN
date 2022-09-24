
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


Public Class CM_StdRpt_SalesListPreview : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents lblErrMesage As Label

    Dim objGLRpt As New agri.GL.clsReport()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminComp As New agri.Admin.clsComp()
    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String

    Dim strSelLocation As String
    Dim intDecimal As Int16
    Dim intErrNo As Int16

    Dim strSrchProduct As String
    Dim strSrchBuyer As String
    Dim strSrchRptType As String
    Dim strSrchPeriode1 As String
    Dim strSrchPeriode2 As String
    Dim strExportToExcel As String

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")

        crvView.Visible = False

        strSelLocation = Server.UrlDecode(Request.QueryString("Location").Trim())
        intDecimal = Convert.ToInt16(Server.UrlDecode(Request.QueryString("Decimal").Trim()))

        strSrchProduct = Trim(Request.QueryString("SrchProduct"))
        strSrchBuyer = Trim(Request.QueryString("SrchBuyer"))
        strSrchRptType = Trim(Request.QueryString("SrchRptType"))
        strSrchPeriode1 = Trim(Request.QueryString("SrchPeriod1"))
        strSrchPeriode2 = Trim(Request.QueryString("SrchPeriod2"))
        strExportToExcel = Trim(Request.QueryString("ExportToExcel"))

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        Else
            BindReport()
        End If
    End Sub

    Sub BindReport()

        Dim objRptDs As New DataSet()
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim objMapPath As String
        Dim objFTPFolder As String

        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions

        Dim strOpCode As String = "CM_STDRPT_SALESLIST"
        Dim strFileName As String = "CM_StdRpt_SalesList"

        Dim strParam As String
        Dim strSearch As String

        If strSrchRptType = "0" Then
            strOpCode = "CM_STDRPT_SALESLIST"
            strFileName = "CM_StdRpt_SalesList"
        ElseIf strSrchRptType = "1" Then
            strOpCode = "CM_STDRPT_SALESSUM"
            strFileName = "CM_StdRpt_SalesSum"
        ElseIf strSrchRptType = "2" Then
            strOpCode = "CM_STDRPT_SALESSUMWIDE"
            strFileName = "CM_StdRpt_SalesSumWide"
        ElseIf strSrchRptType = "3" Then
            strOpCode = "CM_STDRPT_SALESSUMWIDE"
            strFileName = "CM_StdRpt_SalesSumWidePPN"
        ElseIf strSrchRptType = "4" Then
            strOpCode = "CM_STDRPT_SALESBYDISPATCH"
            strFileName = "CM_StdRpt_SalesByDispatch"
        End If

        If strSrchRptType <> "4" Then
            strSearch = "WHERE (YEAR(A.ContractDate)*100)+MONTH(A.ContractDate) BETWEEN '" & strSrchPeriode1 & "' AND '" & strSrchPeriode2 & "'"
            If strSrchBuyer <> "" Then
                strSearch = strSearch & "and (A.BuyerCode LIKE '%" & strSrchBuyer & "%' OR B.Name LIKE '%" & strSrchBuyer & "%') "
            End If
            If strSrchProduct <> "" Then
                strSearch = strSearch & "and A.ProductCode = '" & strSrchProduct & "' "
            End If

            strParamName = "STRSEARCH"
            strParamValue = strSearch
        Else
            strSearch = "WHERE (YEAR(A.Indate)*100)+MONTH(A.Indate) BETWEEN '" & strSrchPeriode1 & "' AND '" & strSrchPeriode2 & "'"
            If strSrchBuyer <> "" Then
                strSearch = strSearch & "and (C.BuyerCode LIKE '%" & strSrchBuyer & "%' OR D.Name LIKE '%" & strSrchBuyer & "%') "
            End If
            If strSrchProduct <> "" Then
                strSearch = strSearch & "and A.ProductCode = '" & strSrchProduct & "' "
            End If

            strParamName = "STRSEARCH|PERIODAWAL"
            strParamValue = strSearch & "|" & strSrchPeriode1
        End If
        
        

        Try
            intErrNo = objGLRpt.mtdGetReport_BiayaProduksi(strOpCode, strParamName, strParamValue, objRptDs, objMapPath, objFTPFolder)

        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=CM_STDRPT_SALESLIST_RPT&errmesg=" & "&redirect=")
        End Try

        rdCrystalViewer.Load(objMapPath & "web\" & strLangCode & "\Reports\Crystal\" & strFileName & ".rpt", OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

        If strSrchRptType = "0" Or strSrchRptType = "4" Then
            rdCrystalViewer.PrintOptions.PaperOrientation = PaperOrientation.Landscape
            rdCrystalViewer.PrintOptions.PaperSize = PaperSize.PaperA3
        ElseIf strSrchRptType = "1" Then
            rdCrystalViewer.PrintOptions.PaperOrientation = PaperOrientation.Portrait
            rdCrystalViewer.PrintOptions.PaperSize = PaperSize.PaperLegal
        Else
            rdCrystalViewer.PrintOptions.PaperOrientation = PaperOrientation.Landscape
            rdCrystalViewer.PrintOptions.PaperSize = PaperSize.PaperLegal
        End If


        crvView.Visible = False
        crvView.ReportSource = rdCrystalViewer
        crvView.DataBind()

        PassParam()

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        If strExportToExcel = "0" Then
            'crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\" & strFileName & ".pdf"
            crDiskFileDestinationOptions.DiskFileName = objFTPFolder & strFileName & ".pdf"
        Else
            'crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\" & strFileName & ".xls"
            crDiskFileDestinationOptions.DiskFileName = objFTPFolder & strFileName & ".xls"
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

        Dim strUrl As String
        strUrl = objFTPFolder.Substring(InStr(objFTPFolder, "ftp") - 1, Len(objFTPFolder) - InStr(objFTPFolder, "ftp") + 1)
        strUrl = Replace(strUrl, "\", "/")

        If strExportToExcel = "0" Then
            'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../ftp/" & strFileName & ".pdf"">")
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../" & strUrl & strFileName & ".pdf"">")
        Else
            'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../ftp/" & strFileName & ".xls"">")
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../" & strUrl & strFileName & ".xls"">")
        End If


    End Sub

    Sub PassParam()
        Dim ParamFieldDefs As ParameterFieldDefinitions
        Dim ParamFieldDef1 As ParameterFieldDefinition
        Dim ParamFieldDef2 As ParameterFieldDefinition
        Dim ParamFieldDef3 As ParameterFieldDefinition
        Dim ParamFieldDef4 As ParameterFieldDefinition
        Dim ParamFieldDef5 As ParameterFieldDefinition
        Dim ParamFieldDef6 As ParameterFieldDefinition

        Dim ParameterValues1 As New ParameterValues()
        Dim ParameterValues2 As New ParameterValues()
        Dim ParameterValues3 As New ParameterValues()
        Dim ParameterValues4 As New ParameterValues()
        Dim ParameterValues5 As New ParameterValues()
        Dim ParameterValues6 As New ParameterValues()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue6 As New ParameterDiscreteValue()

        ParamDiscreteValue1.Value = strCompany
        ParamDiscreteValue2.Value = strLocation
        ParamDiscreteValue3.Value = strUserId
        ParamDiscreteValue4.Value = intDecimal
        ParamDiscreteValue5.Value = strSrchPeriode1
        ParamDiscreteValue6.Value = strSrchPeriode2

        ParamFieldDefs = rdCrystalViewer.DataDefinition.ParameterFields
        ParamFieldDef1 = ParamFieldDefs.Item("ParamCompanyName")
        ParamFieldDef2 = ParamFieldDefs.Item("ParamLocation")
        ParamFieldDef3 = ParamFieldDefs.Item("ParamUserName")
        ParamFieldDef4 = ParamFieldDefs.Item("ParamDecimal")
        ParamFieldDef5 = ParamFieldDefs.Item("strSrchPeriode1")
        ParamFieldDef6 = ParamFieldDefs.Item("strSrchPeriode2")
      
        ParameterValues1 = ParamFieldDef1.CurrentValues
        ParameterValues2 = ParamFieldDef2.CurrentValues
        ParameterValues3 = ParamFieldDef3.CurrentValues
        ParameterValues4 = ParamFieldDef4.CurrentValues
        ParameterValues5 = ParamFieldDef5.CurrentValues
        ParameterValues6 = ParamFieldDef6.CurrentValues

        ParameterValues1.Add(ParamDiscreteValue1)
        ParameterValues2.Add(ParamDiscreteValue2)
        ParameterValues3.Add(ParamDiscreteValue3)
        ParameterValues4.Add(ParamDiscreteValue4)
        ParameterValues5.Add(ParamDiscreteValue5)
        ParameterValues6.Add(ParamDiscreteValue6)

        ParamFieldDef1.ApplyCurrentValues(ParameterValues1)
        ParamFieldDef2.ApplyCurrentValues(ParameterValues2)
        ParamFieldDef3.ApplyCurrentValues(ParameterValues3)
        ParamFieldDef4.ApplyCurrentValues(ParameterValues4)
        ParamFieldDef5.ApplyCurrentValues(ParameterValues5)
        ParamFieldDef6.ApplyCurrentValues(ParameterValues6)
    End Sub


End Class

