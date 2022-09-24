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


Public Class CM_StdRpt_PrintDORegDocPreview: inherits page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents lblErrMesage As Label

    Dim objCMRpt As New agri.CM.clsReport
    Dim objCMTrx As New agri.CM.clsTrx
    Dim objWMTrx As New agri.WM.clsTrx
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String

    Dim strSelLocation As String
    Dim intDecimal As Int16
    Dim strJabatan As String
    Dim strTTD As String
    Dim strDONo1 As String
    Dim strDONo2 As String
    Dim strPrintDate As String = ""
    Dim strPrintedBy As String = ""
    Dim strStatus As String = ""
    Dim strAccMonth as string = ""
    Dim strAccYear As String = ""
    Dim strPacking As String
    Dim strPenyerahan As String
    Dim strMonth As String
    Dim strPPN As String


    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        
        crvView.Visible = False  

        strSelLocation = Server.UrlDecode(Request.QueryString("Location").Trim())
        intDecimal = Convert.ToInt16(Server.UrlDecode(Request.QueryString("Decimal").Trim()))
        strTTD = Server.UrlDecode(Request.QueryString("TTD").Trim())
        strDONo1 = Server.UrlDecode(Request.QueryString("DONo1").Trim())
        strDONo2 = Server.UrlDecode(Request.QueryString("DONo2").Trim())
        strJabatan = Server.UrlDecode(Request.QueryString("Jabatan").Trim())
        strPacking = Server.UrlDecode(Request.QueryString("Packing").Trim())
        strPenyerahan = Server.UrlDecode(Request.QueryString("Penyerahan").Trim())
        strPPN = Request.QueryString("PPN")

        

        Bind_ITEM(True)
    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim objRptDs As New DataSet
        Dim objMapPath As New Object()
        Dim strOpCd As String = "CM_STDRPT_DO_REGISTRATION_PRINT" & "|" & "CM_DOREG"
        Dim strSearch As String
        Dim strParameter As String
        Dim intDONo1 as integer
        Dim intDONo2 as integer
        Dim strDONoAll as String = ""
        Dim strDO as string = ""
        Dim strUpdate as string = ""
        Dim strParam as string = ""
        Dim i as integer
        Dim intErrNo as integer
        Dim doqty as double

    

        strUpdate = " Update CM_DOREG set TTD = '" & trim(strTTD) & "', Jabatan = '" & trim(strJabatan) & "'"
        strUpdate = strUpdate & " where dono = '" & trim(strDONo1) & "' "
           


        strSearch = " where a.dono = '" & trim(strDONo1) & "' and a.LocCode = '" & trim(strSelLocation) & "'"
        strSearch = strSearch & " and a.status IN ('1', '4') "

        strParam = strSearch & "|" & strUpdate

        Try
            intErrNo = objCMRpt.mtdGetDORegReport(strOpCd, strParam, strCompany, _
                                                strLocation, strUserId, strAccMonth, _
                                                strAccYear, objRptDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")        
        End Try

        If objRptDs.Tables(0).Rows.Count > 0 then 
        For i = 0 To objRptDs.Tables(0).Rows.Count - 1
            doqty = Trim(CStr(objRptDs.Tables(0).Rows(i).Item("doqty")))
            objRptDs.Tables(0).rows(i).Item("Terbilang") = objGlobal.terbilangdesimal(doqty, "kilogram")
        Next i
        end if 

        Try
            If objAdmin.mtdGetBasePath(objMapPath) <> 0 Then
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        Select Case Month(objRptDs.Tables(0).Rows(0).Item("DODate"))
            Case 1
                strMonth = "Januari"
            Case 2
                strMonth = "Februari"
            Case 3
                strMonth = "Maret"
            Case 4
                strMonth = "April"
            Case 5
                strMonth = "Mei"
            Case 6
                strMonth = "Juni"
            Case 7
                strMonth = "Juli"
            Case 8
                strMonth = "Agustus"
            Case 9
                strMonth = "September"
            Case 10
                strMonth = "Oktober"
            Case 11
                strMonth = "November"
            Case 12
                strMonth = "Desember"
        End Select
        strPrintDate = Day(objRptDs.Tables(0).Rows(0).Item("DODate")) & " " & strMonth & " " & Year(objRptDs.Tables(0).Rows(0).Item("DODate"))

        If UCase(strPPN) = "NO" Or Convert.ToInt16(objRptDs.Tables(0).Rows(0).Item("DODestination")) = objCMTrx.EnumDODestination.Bulking Then
            rdCrystalViewer.Load(objMapPath & "Web\EN\Reports\Crystal\CM_StdRpt_PrintDORegDoc.rpt", OpenReportMethod.OpenReportByTempCopy)
            rdCrystalViewer.SetDataSource(objRptDs.Tables(0))
        Else
            rdCrystalViewer.Load(objMapPath & "Web\EN\Reports\Crystal\CM_StdRpt_PrintDORegDocPPN.rpt", OpenReportMethod.OpenReportByTempCopy)
            rdCrystalViewer.SetDataSource(objRptDs.Tables(0))
        End If

        If Not blnIsPDFFormat Then
            crvView.Visible = True
            crvView.ReportSource = rdCrystalViewer
            crvView.DataBind()
            PassParam()
        Else
            crvView.Visible = False
            crvView.ReportSource = rdCrystalViewer
            crvView.DataBind()
            PassParam()
            Dim DiskOpts As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
            rdCrystalViewer.ExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            rdCrystalViewer.ExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
            DiskOpts.DiskFileName = objMapPath & "web\ftp\CM_StdRpt_PrintDORegDoc.pdf"
            rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
            rdCrystalViewer.Export()
            rdCrystalViewer.Close()
            rdCrystalViewer.Dispose()

            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_StdRpt_PrintDORegDoc.pdf"">")
        End If

    End Sub

    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()
        Dim paramField3 As New ParameterField()
        Dim paramField4 As New ParameterField()
        Dim paramField5 As New ParameterField()
        Dim paramField6 As New ParameterField()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue6 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues
        Dim crParameterValues5 As ParameterValues
        Dim crParameterValues6 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        ParamFields = crvView.ParameterFieldInfo
     
        ParamField1 = ParamFields.Item("strPrintDate")  
        ParamField2 = ParamFields.Item("strPrintedBy")
        paramField3 = paramFields.Item("strStatus")
        paramField4 = paramFields.Item("Packing")
        paramField5 = paramFields.Item("Penyerahan")
        paramField6 = paramFields.Item("PPN")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues
        crParameterValues5 = paramField5.CurrentValues
        crParameterValues6 = paramField6.CurrentValues

        ParamDiscreteValue1.value = strPrintDate
        ParamDiscreteValue2.value = Session("SS_USERNAME")
        ParamDiscreteValue3.Value = strStatus
        ParamDiscreteValue4.Value = strPacking
        ParamDiscreteValue5.Value = strPenyerahan
        ParamDiscreteValue6.Value = strPPN

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)
        crParameterValues5.Add(ParamDiscreteValue5)
        crParameterValues6.Add(ParamDiscreteValue6)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)
        PFDefs(4).ApplyCurrentValues(crParameterValues5)
        PFDefs(5).ApplyCurrentValues(crParameterValues6)

        crvView.ParameterFieldInfo = paramFields
    End Sub


End Class

