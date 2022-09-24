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


Public Class CM_StdRpt_PrintContractDocPreview: inherits page

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

    Dim strContractNo As String
    Dim strSelLocation As String
    Dim intDecimal As Int16
    Dim strProduct As String
    Dim strBillParty As String
    Dim strBuyer As String
    Dim strOwner1 As String
    Dim strOwner2 As String
    Dim strPPN As String
    Dim strPPNInclude As String
    Dim strPengiriman As String
    Dim strAsalBarang As String
    Dim strPrintDate As String
    Dim strMonth As String

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        
        crvView.Visible = False  

        strContractNo = Server.UrlDecode(Request.QueryString("ContractNo").Trim())
        strSelLocation = Server.UrlDecode(Request.QueryString("Location").Trim())
        intDecimal = Convert.ToInt16(Server.UrlDecode(Request.QueryString("Decimal").Trim()))
        strProduct = Server.UrlDecode(Request.QueryString("Product").Trim())
        strBillParty = Server.UrlDecode(Request.QueryString("BillParty").Trim())
        strBuyer = Server.UrlDecode(Request.QueryString("Buyer").Trim())
        strOwner1 = Server.UrlDecode(Request.QueryString("Owner1").Trim())
        strOwner2 = Server.UrlDecode(Request.QueryString("Owner2").Trim())
        strPPN = Request.QueryString("PPN")
        strPPNInclude = Request.QueryString("PPNInclude")
        strPengiriman = Request.QueryString("Pengiriman")
        strAsalBarang = Request.QueryString("AsalBarang")

        

        Bind_ITEM(True)
    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim objRptDs As New DataSet
        Dim objMapPath As New Object()
        Dim strOpCd As String = "CM_CLSRPT_CONTRACT_REG_GET1"
        Dim strSearch As String
        Dim strParameter As String
        Dim strNoContract As String
        Dim strProd As String

        strSearch = "and A.ContractType = '2' "
        If strContractNo <> "" Then
            strNoContract = Trim(strContractNo)
            strContractNo = "and A.ContractNo = '" & strContractNo & "' "
        End If
        If strSelLocation <> "" Then
            strSelLocation = "and A.LocCode in ('" & strSelLocation & "') "
        End If
        If strProduct <> "" Then
            strProduct = "and A.ProductCode = '" & strProduct & "' "
        End If
        If strBillParty <> "" Then
            strBillParty = "and A.BillPartyCode = '" & strBillParty & "' "
        End If

        strSearch &= strContractNo & strSelLocation & strProduct & strBillParty
        strParameter = strSearch & "||" & strBuyer & "|" & strOwner1 & "|" & strOwner2 & "|" & strNoContract

        Try
            If objCMRpt.mtdGetContract(strOpCd, strParameter, 0, objRptDs) = 0 Then
                objRptDs.Tables(0).Columns.Add("Total", Type.GetType("System.Decimal"), "ContractQty * Price")
                objRptDs.Tables(0).Columns.Add("IDRContractQty", Type.GetType("System.String"))
                objRptDs.Tables(0).Columns.Add("IDRPrice", Type.GetType("System.String"))
                objRptDs.Tables(0).Columns.Add("IDRFFA", Type.GetType("System.String"))
                objRptDs.Tables(0).Columns.Add("IDRMI", Type.GetType("System.String"))
                objRptDs.Tables(0).Columns.Add("IDRTotal", Type.GetType("System.String"))
                objRptDs.Tables(0).Columns.Add("Title", Type.GetType("System.String"))
            Else
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
            End If
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

        For intCount As Integer = 0 To objRptDs.Tables(0).Rows.Count - 1
            With objRptDs.Tables(0)
                Select Case Convert.ToInt16(.Rows(intCount).Item("ProductCode"))
                    Case objWMTrx.EnumWeighBridgeTicketProduct.CPO
                        strProd = "CPO"
                    Case objWMTrx.EnumWeighBridgeTicketProduct.PK
                        strProd = "PK"
                    Case objWMTrx.EnumWeighBridgeTicketProduct.Shell
                        strProd = "CANGKANG"
                End Select

                'Select Case Convert.ToInt16(.Rows(intCount).Item("ContCategory"))
                '    Case objCMTrx.EnumContCategory.LTCBiasa, objCMTrx.EnumContCategory.LTCForward
                '        .Rows(intCount).Item("Title") = "KONTRAK PENJUALAN " & strProd
                '    Case Else
                '        .Rows(intCount).Item("Title") = "KONFIRMASI PENJUALAN " & strProd
                'End Select
                .Rows(intCount).Item("Title") = "KONTRAK PENJUALAN " & strProd
                .Rows(intCount).Item("ContractNo") = Convert.ToString(.Rows(intCount).Item("ContractNo")).Trim()
                .Rows(intCount).Item("CurrencyCode") = Convert.ToString(.Rows(intCount).Item("CurrencyCode")).Trim()
                .Rows(intCount).Item("TermOfDelivery") = objCMTrx.mtdGetTermOfDelivery(Convert.ToInt16(.Rows(intCount).Item("TermOfDelivery")))
                .Rows(intCount).Item("Remarks") = Convert.ToString(.Rows(intCount).Item("Remarks")).Trim()
                .Rows(intCount).Item("ContCategory") = objCMTrx.mtdGetContCategory(Convert.ToInt16(.Rows(intCount).Item("ContCategory")))
                .Rows(intCount).Item("ProductCode") = objWMTrx.mtdGetWeighBridgeTicketProduct(Convert.ToInt16(.Rows(intCount).Item("ProductCode")))
                .Rows(intCount).Item("BankCode") = Convert.ToString(.Rows(intCount).Item("BankCode")).Trim()
                .Rows(intCount).Item("BankCode2") = Convert.ToString(.Rows(intCount).Item("BankCode2")).Trim()
                .Rows(intCount).Item("BankAccNo") = Convert.ToString(.Rows(intCount).Item("BankAccNo")).Trim()
                .Rows(intCount).Item("BankAccNo2") = Convert.ToString(.Rows(intCount).Item("BankAccNo2")).Trim()
                .Rows(intCount).Item("BillPartyName") = Convert.ToString(.Rows(intCount).Item("BillPartyName")).Trim()
                .Rows(intCount).Item("BillPartyAddress") = Convert.ToString(.Rows(intCount).Item("BillPartyAddress")).Trim()
                .Rows(intCount).Item("BankDesc1") = Convert.ToString(.Rows(intCount).Item("BankDesc1")).Trim()
                .Rows(intCount).Item("BankDesc2") = Convert.ToString(.Rows(intCount).Item("BankDesc2")).Trim()
                .Rows(intCount).Item("CurrencyDesc") = Convert.ToString(.Rows(intCount).Item("CurrencyDesc")).Trim()
                .Rows(intCount).Item("IDRContractQty") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("ContractQty"), intDecimal)
                .Rows(intCount).Item("IDRPrice") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("Price"), intDecimal)
                .Rows(intCount).Item("IDRFFA") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("FFA"), 2)
                .Rows(intCount).Item("IDRMI") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("MI"), 2)
                .Rows(intCount).Item("IDRTotal") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("Total"), intDecimal)
                .Rows(intCount).Item("TermOfPayment") = Convert.ToString(.Rows(intCount).Item("TermOfPayment")).Trim()
                .Rows(intCount).Item("ProductQuality") = Convert.ToString(.Rows(intCount).Item("ProductQuality")).Trim()
                .Rows(intCount).Item("CompAddress") = Convert.ToString(.Rows(intCount).Item("CompAddress")).Trim()
                .Rows(intCount).Item("CompTel") = Convert.ToString(.Rows(intCount).Item("CompTel")).Trim()
                .Rows(intCount).Item("CompFax") = Convert.ToString(.Rows(intCount).Item("CompFax")).Trim()
                .Rows(intCount).Item("CompNPWP") = Convert.ToString(.Rows(intCount).Item("CompNPWP")).Trim()
            End With
        Next

        Select Case Month(objRptDs.Tables(0).Rows(0).Item("ContractDate"))
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
        strPrintDate = Day(objRptDs.Tables(0).Rows(0).Item("ContractDate")) & " " & strMonth & " " & Year(objRptDs.Tables(0).Rows(0).Item("ContractDate"))

        If UCase(strPPN) = "YES" Then
            rdCrystalViewer.Load(objMapPath & "Web\EN\Reports\Crystal\CM_StdRpt_PrintContractDocPPN.rpt", OpenReportMethod.OpenReportByTempCopy)
            rdCrystalViewer.SetDataSource(objRptDs.Tables(0))
        Else
            rdCrystalViewer.Load(objMapPath & "Web\EN\Reports\Crystal\CM_StdRpt_PrintContractDoc.rpt", OpenReportMethod.OpenReportByTempCopy)
            rdCrystalViewer.SetDataSource(objRptDs.Tables(0))
        End If


        'rdCrystalViewer.PrintOptions.PaperOrientation = PaperOrientation.Portrait
        'rdCrystalViewer.PrintOptions.PaperSize = PaperSize.PaperLegal

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
            If UCase(strPPN) = "YES" Then
                DiskOpts.DiskFileName = objMapPath & "web\ftp\CM_StdRpt_PrintContractDocPPN.pdf"
                rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
                rdCrystalViewer.Export()
                rdCrystalViewer.Close()
                rdCrystalViewer.Dispose()

                Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_StdRpt_PrintContractDocPPN.pdf"">")
            Else
                DiskOpts.DiskFileName = objMapPath & "web\ftp\CM_StdRpt_PrintContractDoc.pdf"
                rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
                rdCrystalViewer.Export()
                rdCrystalViewer.Close()
                rdCrystalViewer.Dispose()

                Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_StdRpt_PrintContractDoc.pdf"">")
            End If
            
        End If



    End Sub

    Sub PassParam()
        Dim paramFields As New ParameterFields()
        Dim paramField1 As New ParameterField()
        Dim paramField2 As New ParameterField()
        Dim paramField3 As New ParameterField
        Dim paramField4 As New ParameterField
        Dim paramField5 As New ParameterField
        Dim paramField6 As New ParameterField
        Dim paramField7 As New ParameterField
        Dim paramField8 As New ParameterField
        Dim paramField9 As New ParameterField

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue
        Dim ParamDiscreteValue6 As New ParameterDiscreteValue
        Dim ParamDiscreteValue7 As New ParameterDiscreteValue
        Dim ParamDiscreteValue8 As New ParameterDiscreteValue
        Dim ParamDiscreteValue9 As New ParameterDiscreteValue

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues
        Dim crParameterValues5 As ParameterValues
        Dim crParameterValues6 As ParameterValues
        Dim crParameterValues7 As ParameterValues
        Dim crParameterValues8 As ParameterValues
        Dim crParameterValues9 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        ParamFields = crvView.ParameterFieldInfo
     
        paramField1 = paramFields.Item("CompanyName")
        paramField2 = paramFields.Item("Buyer")
        paramField3 = paramFields.Item("Owner1")
        paramField4 = paramFields.Item("Owner2")
        paramField5 = paramFields.Item("PPN")
        paramField6 = paramFields.Item("PPNInclude")
        paramField7 = paramFields.Item("Pengiriman")
        paramField8 = paramFields.Item("AsalBarang")
        paramField9 = paramFields.Item("strPrintDate")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues
        crParameterValues5 = paramField5.CurrentValues
        crParameterValues6 = paramField6.CurrentValues
        crParameterValues7 = paramField7.CurrentValues
        crParameterValues8 = paramField8.CurrentValues
        crParameterValues9 = paramField9.CurrentValues

        ParamDiscreteValue1.Value = Session("SS_LOCATIONNAME")
        ParamDiscreteValue2.Value = strBuyer
        ParamDiscreteValue3.Value = strOwner1
        ParamDiscreteValue4.Value = strOwner2
        ParamDiscreteValue5.Value = strPPN
        ParamDiscreteValue6.Value = strPPNInclude
        ParamDiscreteValue7.Value = strPengiriman
        ParamDiscreteValue8.Value = strAsalBarang
        ParamDiscreteValue9.Value = strPrintDate

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)
        crParameterValues5.Add(ParamDiscreteValue5)
        crParameterValues6.Add(ParamDiscreteValue6)
        crParameterValues7.Add(ParamDiscreteValue7)
        crParameterValues8.Add(ParamDiscreteValue8)
        crParameterValues9.Add(ParamDiscreteValue9)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)
        PFDefs(4).ApplyCurrentValues(crParameterValues5)
        PFDefs(5).ApplyCurrentValues(crParameterValues6)
        PFDefs(6).ApplyCurrentValues(crParameterValues7)
        PFDefs(7).ApplyCurrentValues(crParameterValues8)
        PFDefs(8).ApplyCurrentValues(crParameterValues9)

        crvView.ParameterFieldInfo = paramFields
    End Sub

End Class

