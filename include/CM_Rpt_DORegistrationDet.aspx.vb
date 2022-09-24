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

Public Class CM_Rpt_DORegistrationDet : inherits page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents dgResult As DataGrid
    Protected WithEvents lblErrMesage As Label

    Dim objIn As New agri.IN.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objComp As New agri.Admin.clsComp()
    Dim objLoc As New agri.Admin.clsLoc()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objCM as New agri.CM.clsReport()
    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String

    Dim strStatus As String
    Dim strDONo As String
	Dim strContNo As String
    Dim strPrintDate As String
    Dim strSortLine As String
    Dim strExportToExcel As String

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        
        crvView.Visible = False  

        strStatus = Trim(Request.QueryString("strStatus"))
        strDONo = Trim(Request.QueryString("strDONo"))
		strContNo = Trim(Request.QueryString("strContractNo"))
        strPrintDate = Trim(Request.QueryString("strPrintDate"))
        strSortLine = Trim(Request.QueryString("strSortLine"))
        strExportToExcel = Trim(Request.QueryString("strExportToExcel"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim crExportOptions As ExportOptions
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim objRptDs As New DataSet()
        Dim objMapPath As New Object()
        Dim objCompDs As New DataSet()
        Dim objLocDs As New DataSet()
        Dim strOpCd_Get As String = "CM_STDRPT_DO_REGISTRATION" & "|" & "CM_DOREG"

        Dim strOpCodes As String
        Dim strSearch As String
        Dim strSortItem As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strCompName As String
        Dim strLocName As String
        Dim i As Integer
        Dim doqty As Double

        strSearch = "where  a.DONo = '" & strDONo & "' AND a.LocCode = '" & strLocation & "' AND a.ContractNo = '" & strContNo & "' "
        strSortItem = ""

        strParam = strSearch & "|"

        Try
            intErrNo = objCM.mtdGetDORegReport(strOpCd_Get, strParam, strCompany, _
                                                strLocation, strUserId, strAccMonth, _
                                                strAccYear, objRptDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        If objRptDs.Tables(0).Rows.Count > 0 Then
            For i = 0 To objRptDs.Tables(0).Rows.Count - 1
                doqty = Trim(CStr(objRptDs.Tables(0).Rows(i).Item("doqty")))
                objRptDs.Tables(0).Rows(i).Item("Terbilang") = LCase(objGlobal.TerbilangDesimal(Replace(Replace(doqty, ",", ""), ".00", ""), ""))
            Next i
        End If

        Try
            intErrNo = objAdmin.mtdGetBasePath(objMapPath)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        rdCrystalViewer.Load(objMapPath & "Web\EN\CM\Reports\Crystal\CM_Rpt_DORegistrationDet.rpt", OpenReportMethod.OpenReportByTempCopy)
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
            'Dim DiskOpts As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions()
            'rdCrystalViewer.ExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            'rdCrystalViewer.ExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
            'DiskOpts.DiskFileName = objMapPath & "web\ftp\CM_Rpt_DORegistrationDet.pdf"
            'rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
            'rdCrystalViewer.Export()

            'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_Rpt_DORegistrationDet.pdf"">")

            crDiskFileDestinationOptions = New DiskFileDestinationOptions()
            If strExportToExcel = "0" Then
                crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_Rpt_DORegistrationDet" & ".pdf"
            Else
                crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_Rpt_DORegistrationDet" & ".xls"
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
                Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_Rpt_DORegistrationDet.pdf"">")
            Else
                Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_Rpt_DORegistrationDet.xls"">")
            End If

            objRptDs.Dispose()
            If Not objRptDs Is Nothing Then
                objRptDs = Nothing
            End If
        End If

    End Sub

    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()
        Dim paramField3 As New ParameterField()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        ParamFields = crvView.ParameterFieldInfo
     
        ParamField1 = ParamFields.Item("strPrintDate")  
        ParamField2 = ParamFields.Item("strPrintedBy")
        ParamField3 = ParamFields.Item("strStatus")  

        crParameterValues1 = ParamField1.CurrentValues
        crParameterValues2 = ParamField2.CurrentValues
        crParameterValues3 = ParamField3.CurrentValues

        ParamDiscreteValue1.value = strPrintDate
        ParamDiscreteValue2.value = Session("SS_USERNAME")
        ParamDiscreteValue3.value = strStatus

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)

        crvView.ParameterFieldInfo = paramFields
    End Sub



End Class

