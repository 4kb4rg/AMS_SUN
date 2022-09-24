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

Imports agri.IN.clsTrx
Imports agri.Admin.clsShare
Imports agri.Admin.clsComp
Imports agri.Admin.clsLoc
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.GL

Public Class BI_Rpt_InvoiceDet : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents dgResult As DataGrid
    Protected WithEvents lblErrMesage As Label

    Dim objBI As New agri.BI.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim rdCrystalViewer As New ReportDocument()
    Dim objGLRpt As New agri.GL.clsReport()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String

    Dim strInvoiceId As String
    Dim strContractNo As String
    Dim strPrintDate As String
    Dim strProduct As String
    Dim strPPN As String
    Dim strOptionNo As String
    Dim strOptionDesc As String
    Dim strFakturPajakNo As String
    Dim strFakturPajakDate As String

    Dim strCompName As String
    Dim strLocName As String
    Dim strUserName As String
    Dim strExportToExcel As String


    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        strCompName = Session("SS_COMPANYNAME")
        strLocName = Session("SS_LOCATIONNAME")
        strUserName = Session("SS_USERNAME")

        crvView.Visible = False

        
        strInvoiceId = Trim(Request.QueryString("InvoiceId"))
        strContractNo = Trim(Request.QueryString("ContractNo"))
        strProduct = Trim(Request.QueryString("Product"))
        strPPN = Trim(Request.QueryString("PPN"))
        strPrintDate = Now()
        strOptionNo = Trim(Request.QueryString("OptionNo"))
        strOptionDesc = Trim(Request.QueryString("OptionDesc"))
        strFakturPajakNo = Trim(Request.QueryString("FakturPajakNo"))
        strFakturPajakDate = Trim(Request.QueryString("FakturPajakDate"))
        strExportToExcel = Trim(Request.QueryString("strExportToExcel"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim crExportOptions As ExportOptions
        Dim objRptDs As New DataSet()
        Dim objMapPath As String
        Dim strOpCd As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim ReportFileName As String
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim objFTPFolder As String
        Dim dbTotal As Double = 0
        Dim strTotal As String

        strOpCd = "BI_CLSTRX_INVOICE_GET_REPORT"
        ReportFileName = "BI_Rpt_InvoiceDet.rpt"
        strParamName = "INVOICEID|LOCCODE|FAKTURPAJAKNO|FAKTURPAJAKDATE"
        strParamValue = strInvoiceId & "|" & strLocation & "|" & strFakturPajakNo & "|" & strFakturPajakDate
        
        Try
            intErrNo = objGLRpt.mtdGetReportDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objRptDs, _
                                                objMapPath, objFTPFolder)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        For intCount As Integer = 0 To objRptDs.Tables(0).Rows.Count - 1
            With objRptDs.Tables(0)
                dbTotal = dbTotal + FormatNumber(.Rows(intCount).Item("TotalAmount"), CInt(Session("SS_ROUNDNO")))
                'strTotal = Trim(CStr(FormatNumber(.Rows(intCount).Item("TotalAmount"), CInt(Session("SS_ROUNDNO")))))

                strTotal = Trim(CStr(dbTotal))
                .Rows(intCount).Item("Terbilang") = LCase(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah"))
            End With
        Next


        rdCrystalViewer.Load(objMapPath & "Web\EN\BI\Reports\Crystal\" & ReportFileName, OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

        crvView.Visible = False
        crvView.ReportSource = rdCrystalViewer
        crvView.DataBind()
        PassParam()

        'crDiskFileDestinationOptions = New DiskFileDestinationOptions()

        'crDiskFileDestinationOptions.DiskFileName = objFTPFolder & ReportFileName & ".pdf"
        ''crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\GL_Rpt_Journal_Detail.pdf"

        'crExportOptions = rdCrystalViewer.ExportOptions
        'With crExportOptions
        '    .DestinationOptions = crDiskFileDestinationOptions
        '    .ExportDestinationType = ExportDestinationType.DiskFile
        '    .ExportFormatType = ExportFormatType.PortableDocFormat
        'End With

        'rdCrystalViewer.Export()

        'Dim strUrl As String
        'strUrl = objFTPFolder.Substring(InStr(objFTPFolder, "ftp") - 1, Len(objFTPFolder) - InStr(objFTPFolder, "ftp") + 1)
        'strUrl = Replace(strUrl, "\", "/")
        'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../" & strUrl & ReportFileName & ".pdf"">")

        crDiskFileDestinationOptions = New DiskFileDestinationOptions()
        If strExportToExcel = "0" Then
            crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\" & ReportFileName & ".pdf"
        Else
            crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\" & ReportFileName & ".xls"
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
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/" & ReportFileName & ".pdf"">")
        Else
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/" & ReportFileName & ".xls"">")
        End If

        objRptDs.Dispose()
        If Not objRptDs Is Nothing Then
            objRptDs = Nothing
        End If
    End Sub

    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()
        Dim paramField3 As New ParameterField()
        Dim paramField4 As New ParameterField()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        paramFields = crvView.ParameterFieldInfo

        paramField1 = paramFields.Item("CompName")
        paramField2 = paramFields.Item("PrintDate")
        paramField3 = paramFields.Item("PPN")
        paramField4 = paramFields.Item("Product")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues

        ParamDiscreteValue1.Value = strCompName
        ParamDiscreteValue2.Value = strPrintDate
        ParamDiscreteValue3.Value = strPPN
        ParamDiscreteValue4.Value = strProduct

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)

        crvView.ParameterFieldInfo = paramFields
    End Sub


End Class

