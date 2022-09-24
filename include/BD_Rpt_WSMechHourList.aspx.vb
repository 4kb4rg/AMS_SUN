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

Imports agri.IN.clsSetup
Imports agri.Admin.clsShare
Imports agri.Admin.clsComp
Imports agri.Admin.clsLoc
Imports agri.GlobalHdl.clsGlobalHdl


Public Class BD_Rpt_WSMechHourList : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents tblCriteria As HtmlTable
    Protected WithEvents tblCrystal As HtmlTable
    Protected WithEvents txtInvoiceRcvID As TextBox
    Protected WithEvents lblErrMesage As Label
    Protected WithEvents dgResult As DataGrid

    Dim objBD As New agri.BD.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objComp As New agri.Admin.clsComp()
    Dim objLoc As New agri.Admin.clsLoc()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim rdCrystalViewer As New ReportDocument()

    Dim strCompName As String
    Dim strLocName As String
    Dim strUserId As String
    Dim strLangCode As String

    Dim strEmpCode As String
    Dim strWorkCode As String
    Dim strStatus As String
    Dim strUpdateBy As String
    Dim strPeriodID As String
    Dim strTitle As String
    
    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompName = Session("SS_COMPANY")
        strLocName = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")

        crvView.Visible = False

        strEmpCode = Trim(Request.QueryString("EmpCode"))
        strWorkCode = Trim(Request.QueryString("WorkCode"))
        strPeriodID = Trim(Request.QueryString("PeriodID"))
        strTitle = Trim(Request.QueryString("DocTitleTag"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim objRptDs As New DataSet()
        Dim objMapPath As New Object()
        Dim objCompDs As New DataSet()
        Dim objLocDs As New DataSet()
        Dim strOpCd As String = "BD_CLSTRX_WSMECHHOUR_LIST_GET"
        Dim strOpCd_Comp As String = "ADMIN_CLSCOMP_COMPANY_DETAILS_GET"
        Dim strOpCod_Loc As String = "ADMIN_CLSLOC_LOCATION_DETAILS_GET"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow

        strParam = "|" & " AND BD.LocCode LIKE '" & Trim(strLocName) & "'" & _
                    " AND BD.EmpCode LIKE '" & strEmpCode & "'" & _
                    " AND BD.WorkCode LIKE '" & strWorkCode & "'" & _
                    " AND BD.PeriodID = " & CInt(strPeriodID)

        Try
            intErrNo = objBD.mtdGetMechanicHour(strOpCd, strParam, objRptDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        Try
            intErrNo = objAdmin.mtdGetBasePath(objMapPath)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        Try
            intErrNo = objComp.mtdGetComp(strOpCd_Comp, strCompName, objCompDs, True)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try
        strCompName = Trim(objCompDs.Tables(0).Rows(0).Item("CompName"))

        Try
            intErrNo = objLoc.mtdGetLocDetail(strOpCod_Loc, "", "", "", objLocDs, strLocName)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try
        strLocName = Trim(objLocDs.Tables(0).Rows(0).Item("Description"))

        For intCnt = 0 To objRptDs.Tables(0).Rows.Count - 1
            With objRptDs.Tables(0).Rows(intCnt)
                .Item("EmpCode") = Trim(.Item("EmpCode"))
                .Item("WorkCode") = Trim(.Item("WorkCode"))
                .Item("MechHour") = CDec(Trim(.Item("MechHour")))
                .Item("AddVote") = CDec(Trim(.Item("AddVote")))
                .Item("CreateDate") = Trim(.Item("CreateDate"))
                .Item("UpdateDate") = Trim(.Item("UpdateDate"))
                .Item("UserName") = Trim(.Item("UserName"))
            End With
        Next

        For intCnt = 0 To objRptDs.Tables(0).Rows.Count - 1
            With objRptDs.Tables(0).Rows(intCnt)
                .Item("CreateDate") = objGlobal.GetLongDate(.Item("CreateDate"))
                .Item("UpdateDate") = objGlobal.GetLongDate(.Item("UpdateDate"))
            End With
        Next

        rdCrystalViewer.Load(objMapPath & "Web\EN\BD\Reports\Crystal\BD_Rpt_WSMechHourList.rpt", OpenReportMethod.OpenReportByTempCopy)
        rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

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
            DiskOpts.DiskFileName = objMapPath & "web\ftp\BD_Rpt_WSMechHourList.pdf"
            rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
            rdCrystalViewer.Export()

            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/BD_Rpt_WSMechHourList.pdf"">")
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
        paramFields = crvView.ParameterFieldInfo

        paramField1 = paramFields.Item("DocTitleTag")
        paramField2 = paramFields.Item("strCompName")
        paramField3 = paramFields.Item("strLocName")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues

        ParamDiscreteValue1.Value = strTitle
        ParamDiscreteValue2.Value = Session("SS_COMPANYNAME")
        ParamDiscreteValue3.Value = Session("SS_LOCATIONNAME")

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)

        crvView.ParameterFieldInfo = paramFields

    End Sub


End Class

