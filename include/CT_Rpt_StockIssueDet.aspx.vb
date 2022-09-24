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
Imports agri.PWSystem.clsLangCap


Public Class CT_Rpt_StockIssueDet : inherits page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents dgResult As DataGrid
    Protected WithEvents lblErrMesage As Label

    Dim objCT As New agri.CT.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLangCapDs As New Object()
    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String

    Dim strStatus As String
    Dim strStockIssueId As String
    Dim strPrintDate As String
    Dim strSortLine As String
    Dim strIssueType As String
    Dim strDisplayCost As String

    Dim strCompName As String
    Dim strLocName As String
    Dim strUserName As String

    Dim strAccountTag As String
    Dim strBlockTag As String
    Dim strVehicleTag As String
    Dim strVehExpTag As String
    Dim batchPrint As String
    Dim strReprintedID As String
    Dim arrReprintedID As Array

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")
        strCompName = Session("SS_COMPANYNAME")
        strLocName = Session("SS_LOCATIONNAME")
        strUserName = Session("SS_USERNAME")
        
        crvView.Visible = False  

        strStatus = Trim(Request.QueryString("strStatus"))
        strStockIssueId = Trim(Request.QueryString("strStockIssueId"))
        strPrintDate = Trim(Request.QueryString("strPrintDate"))
        strSortLine = Trim(Request.QueryString("strSortLine"))
        strIssueType = Trim(Request.QueryString("strIssueType"))
        strDisplayCost = Trim(Request.QueryString("strDisplayCost"))
        strAccountTag = Trim(Request.QueryString("AccountTag"))
        strBlockTag = Trim(Request.QueryString("BlockTag"))
        strVehicleTag = Trim(Request.QueryString("VehicleTag"))
        strVehExpTag = Trim(Request.QueryString("VehExpenseTag"))
        batchPrint = Trim(Request.QueryString("batchPrint"))
        strReprintedID = Trim(Request.QueryString("reprintId"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim objRptDs As New Dataset()
        Dim objMapPath As New Object()
        Dim strOpCd_Get As String = ""
        Dim strOpCd_GetLine As String = ""
        Dim strOpCodes As String
        Dim strSortItem As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intCnt2 As Integer
        Dim strReportName As String
        Dim strPDFName As String

        Select Case CInt(strIssueType)
            Case objCT.EnumStockIssueType.OwnUse
                strOpCd_Get = "CT_CLSTRX_STOCKISSUE_DETAILS_GET_REPORT_OWNUSE" & "|" & "StockIssue"
                strOpCd_GetLine = "CT_CLSTRX_STOCKISSUE_LINE_GET_REPORT" & "|" & "StockIssueLn"
                strReportName = "CT_Rpt_StockIssueDet_OwnUse.rpt"
            Case objCT.EnumStockIssueType.StaffPayroll, objCT.EnumStockIssueType.StaffDN
                strOpCd_Get = "CT_CLSTRX_STOCKISSUE_DETAILS_GET_REPORT_STAFF" & "|" & "StockIssue"
                strOpCd_GetLine = "CT_CLSTRX_STOCKISSUE_LINE_GET_REPORT_STAFF_EXTERNAL" & "|" & "StockIssueLn"
                strReportName = "CT_Rpt_StockIssueDet_Staff.rpt"
            Case objCT.EnumStockIssueType.External
                strOpCd_Get = "CT_CLSTRX_STOCKISSUE_DETAILS_GET_REPORT_EXTERNAL" & "|" & "StockIssue"
                strOpCd_GetLine = "CT_CLSTRX_STOCKISSUE_LINE_GET_REPORT_STAFF_EXTERNAL" & "|" & "StockIssueLn"
                strReportName = "CT_Rpt_StockIssueDet_External.rpt"
        End Select
        
        strOpCodes = strOpCd_Get & chr(9) & strOpCd_GetLine
        strParam = strStockIssueId & "|" & strSortLine & "||"

        Try
            intErrNo = objCT.mtdGetStockIssueReport(strOpCodes, _
                                                    strParam, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    objRptDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_RPT_CANTEENISSUE_GET&errmesg=" & lblErrMesage.Text & "&redirect=")        
        End Try

        If objRptDs.Tables(0).Rows.Count > 0 Then
            For intCnt=0 To objRptDs.Tables(0).Rows.Count - 1
                If batchPrint = "yes" Then
                    objRptDs.Tables(0).Rows(intCnt).Item("PrintStatus") = objCT.mtdGetStockIssueStatus(CInt(objRptDs.Tables(0).Rows(intCnt).Item("Status")))
                    
                Else
                    objRptDs.Tables(0).Rows(intCnt).Item("PrintStatus") = strStatus
                End If
            Next
        End If

        If objRptDs.Tables(0).Rows.Count > 0 Then
            If batchPrint = "yes" Then
                If InStr(strReprintedID, "|") <> 0 Then
                    arrReprintedID = Split(strReprintedID, "|")
                    For intCnt=0 To objRptDs.Tables(0).Rows.Count - 1
                        For intCnt2=0 To UBound(arrReprintedID)
                            If Trim(objRptDs.Tables(0).Rows(intCnt).Item("StockIssueId")) = Trim(arrReprintedID(intCnt2)) Then
                                objRptDs.Tables(0).Rows(intCnt).Item("PrintStatus") = objCT.mtdGetStockIssueStatus(CInt(objRptDs.Tables(0).Rows(intCnt).Item("Status"))) & " (re-printed)"                                
                            End If
                        Next
                    Next
                End If
            End If
        End If

        Try
            intErrNo = objAdmin.mtdGetBasePath(objMapPath)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_RPT_CANTEENISSUE_GET_MAPPATH&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        rdCrystalViewer.Load(objMapPath & "Web\EN\CT\Reports\Crystal\" & strReportName, OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs)

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
            Dim DiskOpts As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions()
            rdCrystalViewer.ExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            rdCrystalViewer.ExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
            DiskOpts.DiskFileName = objMapPath & "web\ftp\CT_Rpt_StockIssueDet.pdf"
            rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
            rdCrystalViewer.Export()

            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CT_Rpt_StockIssueDet.pdf"">")
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
        ParamField3 = ParamFields.Item("strCompName")
        ParamField4 = ParamFields.Item("strLocName")
        ParamField5 = ParamFields.Item("strStatus")
        ParamField6 = ParamFields.Item("strDisplayCost")

        crParameterValues1 = ParamField1.CurrentValues
        crParameterValues2 = ParamField2.CurrentValues
        crParameterValues3 = ParamField3.CurrentValues
        crParameterValues4 = ParamField4.CurrentValues
        crParameterValues5 = ParamField5.CurrentValues
        crParameterValues6 = ParamField6.CurrentValues

        ParamDiscreteValue1.value = strPrintDate
        ParamDiscreteValue2.value = strUserName
        ParamDiscreteValue3.value = strCompName
        ParamDiscreteValue4.value = strLocName
        ParamDiscreteValue5.value = strStatus
        ParamDiscreteValue6.value = strDisplayCost

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

        If Trim(strIssueType) = objCT.EnumStockIssueType.OwnUse Then
            Dim paramField7 As New ParameterField()
            Dim paramField8 As New ParameterField()
            Dim paramField9 As New ParameterField()
            Dim paramField10 As New ParameterField()

            Dim ParamDiscreteValue7 As New ParameterDiscreteValue()
            Dim ParamDiscreteValue8 As New ParameterDiscreteValue()
            Dim ParamDiscreteValue9 As New ParameterDiscreteValue()
            Dim ParamDiscreteValue10 As New ParameterDiscreteValue()

            Dim crParameterValues7 As ParameterValues
            Dim crParameterValues8 As ParameterValues
            Dim crParameterValues9 As ParameterValues
            Dim crParameterValues10 As ParameterValues
         
            ParamField7 = ParamFields.Item("lblAccount")  
            ParamField8 = ParamFields.Item("lblBlock")
            ParamField9 = ParamFields.Item("lblVehicle")
            ParamField10 = ParamFields.Item("lblVehExpense")

            crParameterValues7 = ParamField7.CurrentValues
            crParameterValues8 = ParamField8.CurrentValues
            crParameterValues9 = ParamField9.CurrentValues
            crParameterValues10 = ParamField10.CurrentValues

            ParamDiscreteValue7.value = strAccountTag
            ParamDiscreteValue8.value = strBlockTag
            ParamDiscreteValue9.value = strVehicleTag
            ParamDiscreteValue10.value = strVehExpTag

            crParameterValues7.Add(ParamDiscreteValue7)
            crParameterValues8.Add(ParamDiscreteValue8)
            crParameterValues9.Add(ParamDiscreteValue9)
            crParameterValues10.Add(ParamDiscreteValue10)

            PFDefs(6).ApplyCurrentValues(crParameterValues7)
            PFDefs(7).ApplyCurrentValues(crParameterValues8)
            PFDefs(8).ApplyCurrentValues(crParameterValues9)
            PFDefs(9).ApplyCurrentValues(crParameterValues10)
        End If

        crvView.ParameterFieldInfo = paramFields
    End Sub



End Class

