
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


Public Class CM_Rpt_ContractRegDet: inherits page

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

    Dim strTitle As String
    Dim strContractNo As String
    Dim strTerbilangQty As String
    Dim strTerbilangPrice As String
    Dim strTerbilangTotal As String   
    Dim strCurrencyDesc As String

    Dim strProduct As String
    Dim strBillParty As String
    Dim strBuyer As String
    Dim strOwner1 As String
    Dim strOwner2 As String
    Dim strPPN As String
    Dim strPPNInclude As String
    Dim strPengiriman As String
    Dim strAsalBarang As String
    Dim strExportToExcel As String

    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        
        crvView.Visible = False  

        strContractNo = Server.UrlDecode(Request.QueryString("ContractNo").Trim())
        strProduct = Server.UrlDecode(Request.QueryString("Product").Trim())
        strBillParty = Server.UrlDecode(Request.QueryString("BillParty").Trim())
        strBuyer = Server.UrlDecode(Request.QueryString("Buyer").Trim())
        strOwner1 = Server.UrlDecode(Request.QueryString("Owner1").Trim())
        strOwner2 = Server.UrlDecode(Request.QueryString("Owner2").Trim())
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
        Dim strOpCd As String = "CM_STDRPT_CONTRACT_REG_GET_NEW"
        Dim strSearch As String
        Dim strParameter As String
        Dim strNoContract As String
        Dim strProd As String
        Dim strQty As String
        Dim strPrice As String
        Dim strTotal As String

        strSearch = "and A.ContractType = '2' "
        If strContractNo <> "" Then
            strNoContract = Trim(strContractNo)
            strContractNo = "and A.ContractNo = '" & strContractNo & "' "
        End If
        If strLocation <> "" Then
            strLocation = "and A.LocCode in ('" & strLocation & "') "
        End If
        If strProduct <> "" Then
            strProduct = "and A.ProductCode = '" & strProduct & "' "
        End If
        If strBillParty <> "" Then
            strBillParty = "and A.BuyerCode = '" & strBillParty & "' "
        End If

        strSearch &= strContractNo & strLocation & strProduct & strBillParty
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
                    Case objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah
                        strProd = "MINYAK LIMBAH"
                    Case objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang
                        strProd = "ABU JANJANG"
                    Case objWMTrx.EnumWeighBridgeTicketProduct.Others
                        strProd = "OTHERS"
                    Case objWMTrx.EnumWeighBridgeTicketProduct.EFBOil
                        strProd = "POME"
                    Case objWMTrx.EnumWeighBridgeTicketProduct.EFB
                        strProd = "JJK"
                End Select

                strQty = Trim(CStr(FormatNumber(.Rows(intCount).Item("ContractQty"), CInt(Session("SS_ROUNDNO")))))
                strPrice = Trim(CStr(FormatNumber(.Rows(intCount).Item("Price"), CInt(Session("SS_ROUNDNO")))))
                strTotal = Trim(CStr(FormatNumber(.Rows(intCount).Item("ContractQty") * (.Rows(intCount).Item("Price")+FormatNumber(.Rows(intCount).Item("Price")*.Rows(intCount).Item("PPNInit"), 2)), CInt(Session("SS_ROUNDNO")))))

                'Select Case Convert.ToInt16(.Rows(intCount).Item("ContCategory"))
                '    Case objCMTrx.EnumContCategory.LTCBiasa, objCMTrx.EnumContCategory.LTCForward
                '        .Rows(intCount).Item("Title") = "KONTRAK PENJUALAN " & strProd
                '    Case Else
                '        .Rows(intCount).Item("Title") = "KONFIRMASI PENJUALAN " & strProd
                'End Select
                .Rows(intCount).Item("Title") = "KONTRAK PENJUALAN"
                '.Rows(intCount).Item("Title") = "SURAT PERJANJIAN JUAL/BELI"
                .Rows(intCount).Item("ContractNo") = Convert.ToString(.Rows(intCount).Item("ContractNo")).Trim()
                .Rows(intCount).Item("CurrencyCode") = Convert.ToString(.Rows(intCount).Item("CurrencyCode")).Trim()
                .Rows(intCount).Item("TermOfDelivery") = lGetNameTermOfDeliv(Convert.ToInt16(.Rows(intCount).Item("TermOfDelivery"))) ''objCMTrx.mtdGetTermOfDelivery(Convert.ToInt16(.Rows(intCount).Item("TermOfDelivery")))
                .Rows(intCount).Item("Remarks") = Convert.ToString(.Rows(intCount).Item("Remarks")).Trim()
                .Rows(intCount).Item("ContCategory") = objCMTrx.mtdGetContCategory(Convert.ToInt16(.Rows(intCount).Item("ContCategory")))
                .Rows(intCount).Item("ProductCode") = UCase(objWMTrx.mtdGetWeighBridgeTicketProduct(Convert.ToInt16(.Rows(intCount).Item("ProductCode"))))
                .Rows(intCount).Item("BankCode") = Convert.ToString(.Rows(intCount).Item("BankCode")).Trim()
                .Rows(intCount).Item("BankCode2") = Convert.ToString(.Rows(intCount).Item("BankCode2")).Trim()
                .Rows(intCount).Item("BankAccNo") = Convert.ToString(.Rows(intCount).Item("BankAccNo")).Trim()
                .Rows(intCount).Item("BankAccNo2") = Convert.ToString(.Rows(intCount).Item("BankAccNo2")).Trim()
                .Rows(intCount).Item("BillPartyName") = Convert.ToString(.Rows(intCount).Item("BillPartyName")).Trim()
                .Rows(intCount).Item("BillPartyAddress") = Convert.ToString(.Rows(intCount).Item("BillPartyAddress")).Trim()
                .Rows(intCount).Item("BankDesc1") = Convert.ToString(.Rows(intCount).Item("BankDesc1")).Trim()
                .Rows(intCount).Item("BankDesc2") = Convert.ToString(.Rows(intCount).Item("BankDesc2")).Trim()
                .Rows(intCount).Item("CurrencyDesc") = Convert.ToString(.Rows(intCount).Item("CurrencyDesc")).Trim()
                .Rows(intCount).Item("IDRContractQty") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("ContractQty"), CInt(Session("SS_ROUNDNO")))
                .Rows(intCount).Item("IDRPrice") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("Price"), CInt(Session("SS_ROUNDNO")))
                .Rows(intCount).Item("IDRFFA") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("FFA"), 2)
                .Rows(intCount).Item("IDRMI") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("MI"), 2)
                .Rows(intCount).Item("IDRTotal") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Rows(intCount).Item("Total"), CInt(Session("SS_ROUNDNO")))
                .Rows(intCount).Item("TermOfPayment") = Convert.ToString(.Rows(intCount).Item("TermOfPayment")).Trim()
                .Rows(intCount).Item("ProductQuality") = Convert.ToString(.Rows(intCount).Item("ProductQuality")).Trim()
                .Rows(intCount).Item("CompAddress") = Convert.ToString(.Rows(intCount).Item("CompAddress")).Trim()
                .Rows(intCount).Item("CompTel") = Convert.ToString(.Rows(intCount).Item("CompTel")).Trim()
                .Rows(intCount).Item("CompFax") = Convert.ToString(.Rows(intCount).Item("CompFax")).Trim()
                .Rows(intCount).Item("CompNPWP") = Convert.ToString(.Rows(intCount).Item("CompNPWP")).Trim()
                .Rows(intCount).Item("TerbilangQty") = Replace(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(Replace(strQty, ",", ""), ".00", ""), ""))), 1), Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(Replace(strQty, ",", ""), ".00", ""), ""))), 1), UCase(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(Replace(strQty, ",", ""), ".00", ""), ""))), 1))) + Mid(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(Replace(strQty, ",", ""), ".00", ""), ""))), 2, Len(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(Replace(strQty, ",", ""), ".00", ""), "")))) - 1)
                .Rows(intCount).Item("TerbilangPrice") = LCase(objGlobal.TerbilangDesimal(Replace(strPrice, ",", ""), "Rupiah"))
                .Rows(intCount).Item("TerbilangTotal") = Replace(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah"))), 1), Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah"))), 1), UCase(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah"))), 1))) + Mid(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah"))), 2, Len(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah")))) - 1)

                'replace(lcase(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah")),left(ltrim(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah")),1), uCase(left(ltrim(objGlobal.TerbilangDesimal(Replace(strTotal, ",", ""), "Rupiah")),1)))
            End With
        Next


        rdCrystalViewer.Load(objMapPath & "Web\EN\CM\Reports\Crystal\CM_StdRpt_PrintContractDoc.rpt", OpenReportMethod.OpenReportByTempCopy)
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
            'Dim DiskOpts As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
            'rdCrystalViewer.ExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
            'rdCrystalViewer.ExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat

            'DiskOpts.DiskFileName = objMapPath & "web\ftp\CM_StdRpt_PrintContractDoc.pdf"
            'rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
            'rdCrystalViewer.Export()

            'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_StdRpt_PrintContractDoc.pdf"">")

            crDiskFileDestinationOptions = New DiskFileDestinationOptions()
            If strExportToExcel = "0" Then
                crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_StdRpt_PrintContractDoc" & ".pdf"
            Else
                crDiskFileDestinationOptions.DiskFileName = objMapPath & "web\ftp\CM_StdRpt_PrintContractDoc" & ".xls"
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
                Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_StdRpt_PrintContractDoc.pdf"">")
            Else
                Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_StdRpt_PrintContractDoc.xls"">")
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
        Dim paramField3 As New ParameterField
        Dim paramField4 As New ParameterField
        Dim paramField5 As New ParameterField
        Dim paramField6 As New ParameterField
        Dim paramField7 As New ParameterField
        Dim paramField8 As New ParameterField

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue
        Dim ParamDiscreteValue6 As New ParameterDiscreteValue
        Dim ParamDiscreteValue7 As New ParameterDiscreteValue
        Dim ParamDiscreteValue8 As New ParameterDiscreteValue

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues
        Dim crParameterValues5 As ParameterValues
        Dim crParameterValues6 As ParameterValues
        Dim crParameterValues7 As ParameterValues
        Dim crParameterValues8 As ParameterValues

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

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues
        crParameterValues5 = paramField5.CurrentValues
        crParameterValues6 = paramField6.CurrentValues
        crParameterValues7 = paramField7.CurrentValues
        crParameterValues8 = paramField8.CurrentValues

        ParamDiscreteValue1.Value = Session("SS_COMPANYNAME")
        ParamDiscreteValue2.Value = strBuyer
        ParamDiscreteValue3.Value = Session("SS_LOCATIONNAME")
        ParamDiscreteValue4.Value = strOwner2
        ParamDiscreteValue5.Value = strPPN
        ParamDiscreteValue6.Value = strPPNInclude
        ParamDiscreteValue7.Value = strPengiriman
        ParamDiscreteValue8.Value = strAsalBarang

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)
        crParameterValues5.Add(ParamDiscreteValue5)
        crParameterValues6.Add(ParamDiscreteValue6)
        crParameterValues7.Add(ParamDiscreteValue7)
        crParameterValues8.Add(ParamDiscreteValue8)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)
        PFDefs(4).ApplyCurrentValues(crParameterValues5)
        PFDefs(5).ApplyCurrentValues(crParameterValues6)
        PFDefs(6).ApplyCurrentValues(crParameterValues7)
        PFDefs(7).ApplyCurrentValues(crParameterValues8)

        crvView.ParameterFieldInfo = paramFields
    End Sub


    Function lGetNameTermOfDeliv(ByVal pKode As String) As String
        Dim nName As String

        Select Case pKode
            Case "1"
                nName = "FRANCO Pabrik pembeli"
            Case "2"
                nName = "LOCO Pabrik penjual"
            Case "3"
                nName = "CIF Pelabuhan pembeli "
            Case "4"
                nName = "FOB Pangkal Balam"
            Case Else
                nName = ""
        End Select

        Return nName

    End Function

    'Sub Bind_ITEM_oLD(ByVal blnIsPDFFormat As Boolean)
    '    Dim objRptDs As New DataSet
    '    Dim objMapPath As New Object()
    '    Dim strOpCd As String = "CM_CLSRPT_CONTRACT_REG_GET"
    '    Dim strSearch As String
    '    Dim strParameter As String

    '    strSearch = "and A.LocCode = '" & strLocation & "' and A.ContractNo = '" & strContractNo & "' "
    '    strParameter = strSearch & "|" & ""

    '    Try
    '        If objCMTrx.mtdGetContract(strOpCd, strParameter, 0, objRptDs) = 0 Then
    '            objRptDs.Tables(0).Columns.Add("Total", Type.GetType("System.Decimal"), "ContractQty * Price")
    '            objRptDs.Tables(0).Columns.Add("IDRContractQty", Type.GetType("System.String"))
    '            objRptDs.Tables(0).Columns.Add("IDRPrice", Type.GetType("System.String"))
    '            strTerbilangQty = Trim(CStr(objRptDs.Tables(0).Rows(0).Item("ContractQty")))
    '            strTerbilangPrice = Trim(CStr(objRptDs.Tables(0).Rows(0).Item("Price")))
    '            strTerbilangTotal = Trim(CStr(objRptDs.Tables(0).Rows(0).Item("Total")))
    '            strCurrencyDesc = Trim(objRptDs.Tables(0).Rows(0).Item("CurrencyDesc"))
    '            objRptDs.Tables(0).Columns.Add("IDRTotal", Type.GetType("System.String"))
    '        Else
    '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
    '        End If
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
    '    End Try

    '    Try
    '        If objAdmin.mtdGetBasePath(objMapPath) <> 0 Then
    '            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
    '        End If
    '    Catch Exp As System.Exception
    '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
    '    End Try

    '    With objRptDs.Tables(0).Rows(0)
    '        Select Case Convert.ToInt16(.Item("ContCategory"))
    '            Case objCMTrx.EnumContCategory.LTCBiasa, objCMTrx.EnumContCategory.LTCForward
    '                strTitle = "Kontrak Penjualan"
    '            Case Else
    '                strTitle = "Konfirmasi Penjualan"
    '        End Select

    '        .Item("ContractNo") = Convert.ToString(.Item("ContractNo")).Trim()
    '        .Item("CurrencyCode") = Convert.ToString(.Item("CurrencyCode")).Trim()
    '        .Item("TermOfDelivery") = objCMTrx.mtdGetTermOfDelivery(Convert.ToInt16(.Item("TermOfDelivery")))
    '        .Item("Remarks") = Convert.ToString(.Item("Remarks")).Trim()
    '        .Item("ContCategory") = objCMTrx.mtdGetContCategory(Convert.ToInt16(.Item("ContCategory")))
    '        .Item("ProductCode") = objWMTrx.mtdGetWeighBridgeTicketProduct(Convert.ToInt16(.Item("ProductCode")))
    '        .Item("BankCode") = Convert.ToString(.Item("BankCode")).Trim()
    '        .Item("BankCode2") = Convert.ToString(.Item("BankCode2")).Trim()
    '        .Item("BankAccNo") = Convert.ToString(.Item("BankAccNo")).Trim()
    '        .Item("BankAccNo2") = Convert.ToString(.Item("BankAccNo2")).Trim()
    '        .Item("BillPartyName") = Convert.ToString(.Item("BillPartyName")).Trim()
    '        .Item("BillPartyAddress") = Convert.ToString(.Item("BillPartyAddress")).Trim()
    '        .Item("BankDesc1") = Convert.ToString(.Item("BankDesc1")).Trim()
    '        .Item("BankDesc2") = Convert.ToString(.Item("BankDesc2")).Trim()
    '        .Item("CurrencyDesc") = Convert.ToString(.Item("CurrencyDesc")).Trim()
    '        .Item("IDRContractQty") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Item("ContractQty"), 5)
    '        .Item("IDRPrice") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Item("Price"), 5)
    '        .Item("ProductQuality") = Convert.ToString(.Item("ProductQuality")).Trim()
    '        .Item("TermOfPayment") = Convert.ToString(.Item("TermOfPayment")).Trim()
    '        .Item("IDRTotal") = objGlobal.GetIDDecimalSeparator_FreeDigit(.Item("Total"), 5)
    '    End With

    '    rdCrystalViewer.Load(objMapPath & "Web\EN\CM\Reports\Crystal\CM_Rpt_ContractRegDet.rpt", OpenReportMethod.OpenReportByTempCopy)
    '    rdCrystalViewer.SetDataSource(objRptDs.Tables(0))

    '    If Not blnIsPDFFormat Then
    '        crvView.Visible = True
    '        crvView.ReportSource = rdCrystalViewer
    '        crvView.DataBind()
    '        PassParam()
    '    Else
    '        crvView.Visible = False
    '        crvView.ReportSource = rdCrystalViewer
    '        crvView.DataBind()
    '        PassParam()
    '        Dim DiskOpts As CrystalDecisions.Shared.DiskFileDestinationOptions = New CrystalDecisions.Shared.DiskFileDestinationOptions
    '        rdCrystalViewer.ExportOptions.ExportDestinationType = CrystalDecisions.[Shared].ExportDestinationType.DiskFile
    '        rdCrystalViewer.ExportOptions.ExportFormatType = CrystalDecisions.[Shared].ExportFormatType.PortableDocFormat
    '        DiskOpts.DiskFileName = objMapPath & "web\ftp\CM_Rpt_ContractRegDet.pdf"
    '        rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts
    '        rdCrystalViewer.Export()

    '        Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CM_Rpt_ContractRegDet.pdf"">")
    '    End If

    'End Sub

    'Sub PassParam()
    '    Dim paramFields As New ParameterFields()
    '    Dim paramField1 As New ParameterField()
    '    Dim paramField2 As New ParameterField()
    '    Dim paramField3 As New ParameterField
    '    Dim paramField4 As New ParameterField
    '    Dim paramField5 As New ParameterField
    '    Dim paramField6 As New ParameterField
    '    Dim paramField7 As New ParameterField
    '    Dim paramField8 As New ParameterField

    '    Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
    '    Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
    '    Dim ParamDiscreteValue3 As New ParameterDiscreteValue
    '    Dim ParamDiscreteValue4 As New ParameterDiscreteValue
    '    Dim ParamDiscreteValue5 As New ParameterDiscreteValue
    '    Dim ParamDiscreteValue6 As New ParameterDiscreteValue
    '    Dim ParamDiscreteValue7 As New ParameterDiscreteValue
    '    Dim ParamDiscreteValue8 As New ParameterDiscreteValue

    '    Dim crParameterValues1 As ParameterValues
    '    Dim crParameterValues2 As ParameterValues
    '    Dim crParameterValues3 As ParameterValues
    '    Dim crParameterValues4 As ParameterValues
    '    Dim crParameterValues5 As ParameterValues
    '    Dim crParameterValues6 As New ParameterValues
    '    Dim crParameterValues7 As New ParameterValues
    '    Dim crParameterValues8 As New ParameterValues

    '    Dim crDataDef As DataDefinition
    '    Dim PFDefs As ParameterFieldDefinitions

    '    crDataDef = rdCrystalViewer.DataDefinition
    '    PFDefs = crDataDef.ParameterFields
    '    ParamFields = crvView.ParameterFieldInfo

    '    paramField1 = paramFields.Item("CompanyName")
    '    paramField2 = paramFields.Item("Title")
    '    paramField3 = paramFields.Item("Buyer")
    '    paramField4 = paramFields.Item("Owner1")
    '    paramField5 = paramFields.Item("Owner2")
    '    paramField6 = paramFields.Item("TerbilangQty")
    '    paramField7 = paramFields.Item("TerbilangPrice")
    '    paramField8 = paramFields.Item("TerbilangTotal")

    '    crParameterValues1 = paramField1.CurrentValues
    '    crParameterValues2 = paramField2.CurrentValues
    '    crParameterValues3 = paramField3.CurrentValues
    '    crParameterValues4 = paramField4.CurrentValues
    '    crParameterValues5 = paramField5.CurrentValues
    '    crParameterValues6 = paramField6.CurrentValues
    '    crParameterValues7 = paramField7.CurrentValues
    '    crParameterValues8 = paramField8.CurrentValues

    '    ParamDiscreteValue1.Value = Session("SS_COMPANYNAME")
    '    ParamDiscreteValue2.Value = strTitle
    '    ParamDiscreteValue3.Value = ""
    '    ParamDiscreteValue4.Value = ""
    '    ParamDiscreteValue5.Value = ""
    '    ParamDiscreteValue6.Value = objGlobal.terbilangdesimal(strTerbilangQty, "Kilogram")
    '    ParamDiscreteValue7.Value = objGlobal.terbilangdesimal(strTerbilangPrice, "Rupiah per Kilogram")
    '    ParamDiscreteValue8.Value = objGlobal.terbilangdesimal(strTerbilangTotal, "Rupiah")

    '    crParameterValues1.Add(ParamDiscreteValue1)
    '    crParameterValues2.Add(ParamDiscreteValue2)
    '    crParameterValues3.Add(ParamDiscreteValue3)
    '    crParameterValues4.Add(ParamDiscreteValue4)
    '    crParameterValues5.Add(ParamDiscreteValue5)
    '    crParameterValues6.Add(ParamDiscreteValue6)
    '    crParameterValues7.Add(ParamDiscreteValue7)
    '    crParameterValues8.Add(ParamDiscreteValue8)

    '    PFDefs(0).ApplyCurrentValues(crParameterValues1)
    '    PFDefs(1).ApplyCurrentValues(crParameterValues2)
    '    PFDefs(2).ApplyCurrentValues(crParameterValues3)
    '    PFDefs(3).ApplyCurrentValues(crParameterValues4)
    '    PFDefs(4).ApplyCurrentValues(crParameterValues5)
    '    PFDefs(5).ApplyCurrentValues(crParameterValues6)
    '    PFDefs(6).ApplyCurrentValues(crParameterValues7)
    '    PFDefs(7).ApplyCurrentValues(crParameterValues8)

    '    crvView.ParameterFieldInfo = paramFields
    'End Sub



End Class

