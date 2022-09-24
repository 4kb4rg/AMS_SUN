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

Imports agri.GL
Imports agri.Admin.clsShare
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap


Public Class AP_Rpt_InvRcvDet : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents dgResult As DataGrid
    Protected WithEvents lblErrMesage As Label

    Dim objGLRpt As New agri.GL.clsReport()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLangCapDs As New Object()
    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String

    Dim strSelLoc As String
    Dim strSelComp As String
    Dim strSelTrxID As String
    Dim strSelTrxType As String
    Dim strSelSuppCode As String
    Dim strTotalAmount As String = ""
    Dim strTotalAmountSentence As String = ""
    Dim strBeforeDec As String = ""
    Dim strAfterDec As String = ""
    Dim strCompName As String
    Dim strLocName As String


    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strCompName = Session("SS_COMPANYNAME")
        strLocName = Session("SS_LOCATIONNAME")

        crvView.Visible = False

        strSelLoc = Trim(Request.QueryString("Location"))
        strSelComp = Trim(Request.QueryString("CompName"))
        strSelTrxID = Trim(Request.QueryString("TrxID"))
        strSelTrxType = Trim(Request.QueryString("TrxType"))
        strSelSuppCode = Trim(Request.QueryString("SupplierCode"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)

        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim crExportOptions As ExportOptions
        Dim objRptDs As New DataSet()
        Dim objRptDs1 As New DataSet
        Dim objMapPath As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strReportName As String
        Dim strPDFName As String
        Dim strOpCd As String = ""
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim totAmount As Double = 0
        Dim pv_TrxID As String = ""
        Dim objFTPFolder As String


        strOpCd = "AP_CLSTRX_INVOICERECEIVENOTE_GET_FOR_DOCRPT"

        'If strSelTrxType = "1" Then
        '    strParamName = "STRSEARCH"
        '    strParamValue = "AP.InvoiceRcvID = '" & Trim(strSelTrxID) & "' AND AP.Status IN ('1','2') AND AP.OutstandingAmount > 0"
        'Else
        '    strParamName = "STRSEARCH"
        '    strParamValue = "NT.TrxID = '" & Trim(strSelTrxID) & "' OR AP.SupplierCode = '" & Trim(strSelSuppCode) & "' AND AP.Status IN ('1','2') AND AP.OutstandingAmount > 0"
        'End If

        strParamName = "INVOICERCVID"
        strParamValue = Trim(strSelTrxID)
        strReportName = "AP_Rpt_InvRcvDet"

        Try
            intErrNo = objGLRpt.mtdGetReport_KaryawanStaff(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objRptDs, _
                                                objMapPath, objFTPFolder)

            objRptDs.Tables(0).TableName = "INVOICE_HEADER"
            objRptDs.Tables(1).TableName = "INVOICE_DETAIL"

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_SPK_INFO&errmesg=" & "" & "&redirect=../AP/trx/ap_trx_InvRcvDet.aspx")
        End Try

        If objRptDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objRptDs.Tables(0).Rows.Count - 1
                'If intCnt > 0 Then
                '    pv_TrxID = objRptDs.Tables(0).Rows(intCnt - 1).Item("TrxID")
                'End If

                'If objRptDs.Tables(0).Rows(intCnt).Item("TrxID") = pv_TrxID Then
                '    totAmount += FormatNumber(objRptDs.Tables(0).Rows(intCnt).Item("TotalAmount"), 2)
                'Else
                '    totAmount = FormatNumber(objRptDs.Tables(0).Rows(intCnt).Item("TotalAmount"), 2)
                'End If
                totAmount = FormatNumber(objRptDs.Tables(0).Rows(intCnt).Item("TotalAmount"), 2)
                strTotalAmount = CStr(totAmount)

                If UCase(Trim(objRptDs.Tables(0).Rows(intCnt).Item("CurrencyCode"))) = "IDR" Then
                    strBeforeDec = objGlobal.TerbilangDesimal(totAmount, "Rupiah")
                Else
                    strBeforeDec = objGlobal.ConvertNo2WordsDecimal(objRptDs.Tables(0).Rows(intCnt).Item("TotalAmount"))
                End If

                strTotalAmountSentence = Replace(strBeforeDec, "Zero", "")
                'objRptDs.Tables(0).Rows(intCnt).Item("Terbilang") = strTotalAmountSentence
            Next
            

            objRptDs1.Tables.Add(objRptDs.Tables(0).Copy())
            objRptDs1.Tables(0).TableName = "INVOICE_HEADER"
            objRptDs1.Tables.Add(objRptDs.Tables(1).Copy())
            objRptDs1.Tables(1).TableName = "INVOICE_DETAIL"

            rdCrystalViewer.Load(objMapPath & "Web\EN\AP\Reports\Crystal\" & strReportName & ".rpt", OpenReportMethod.OpenReportByTempCopy)

            rdCrystalViewer.SetDataSource(objRptDs1)

            crvView.Visible = True
            crvView.ReportSource = rdCrystalViewer
            crvView.DataBind()

            PassParam()

            crDiskFileDestinationOptions = New DiskFileDestinationOptions()
            'crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\AP_Rpt_InvRcvDet.pdf"
            crDiskFileDestinationOptions.DiskFileName = objFTPFolder & strReportName & ".pdf"

            crExportOptions = rdCrystalViewer.ExportOptions
            With crExportOptions
                .DestinationOptions = crDiskFileDestinationOptions
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
            End With

            rdCrystalViewer.Export()
            'rdCrystalViewer.Close()
            rdCrystalViewer.Dispose()

            Dim strUrl As String
            strUrl = objFTPFolder.Substring(InStr(objFTPFolder, "ftp") - 1, Len(objFTPFolder) - InStr(objFTPFolder, "ftp") + 1)
            strUrl = Replace(strUrl, "\", "/")
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../" & strUrl & strReportName & ".pdf"">")
            'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/AP_Rpt_InvRcvDet.pdf"">")
        End If
    End Sub

    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()
        Dim paramField3 As New ParameterField()
        Dim paramField4 As New ParameterField()
        Dim paramField5 As New ParameterField()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues
        Dim crParameterValues5 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        paramFields = crvView.ParameterFieldInfo

        paramField1 = paramFields.Item("strCompName")
        paramField2 = paramFields.Item("TotalAmountSentence")
        paramField3 = paramFields.Item("TotalAmount")
        paramField4 = paramFields.Item("ParamLocation")
        paramField5 = paramFields.Item("ParamCompanyName")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues
        crParameterValues5 = paramField5.CurrentValues

        ParamDiscreteValue1.Value = strCompName
        ParamDiscreteValue2.Value = strTotalAmountSentence
        ParamDiscreteValue3.Value = strTotalAmount
        ParamDiscreteValue4.Value = strLocName
        ParamDiscreteValue5.Value = strCompName

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)
        crParameterValues5.Add(ParamDiscreteValue5)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)
        PFDefs(4).ApplyCurrentValues(crParameterValues5)

        crvView.ParameterFieldInfo = paramFields
    End Sub

End Class

