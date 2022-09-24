
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.XML
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.CrystalReports
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Shared
Imports CrystalDecisions.Web

Imports agri.PU.clsTrx
Imports agri.Admin.clsShare
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap
Imports Microsoft.VisualBasic

Public Class CB_Rpt_ChequeGiroMultiPrint : Inherits Page

    Protected WithEvents crvView As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents dgResult As DataGrid
    Protected WithEvents lblErrMesage As Label

    Dim objCB As New agri.CB.clsTrx()
    Dim objAdmin As New agri.Admin.clsShare()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLangCapDs As New Object()
    Dim rdCrystalViewer As New ReportDocument()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String

    Dim strProgramPath As String
    Dim strPayId As String
    Dim strSortLine As String
    Dim strCBType As String
    Dim TRXType As String
    Dim strCurrencyCode As String
    Dim strExchangeRate As String
    Dim strBiaya As String
    Dim strDeduct As String

    Dim strCompName As String
    Dim strLocName As String
    Dim strUserName As String
    Dim strAccountTag As String

    Dim strCompAddress As String
    Dim strCompCity As String
    Dim strCompTelp As String
    Dim strCompCountry As String

    Dim strBasicTotalAmount As String = ""
    Dim strTotalAmount As String = ""
    Dim arrTotalAmount(15) As String
    Dim strBeforeDec As String = ""
    Dim strAfterDec As String = ""
    Dim strAmtToPrint As String = ""
    Dim strChequeNoToPrint As String = ""


    Public Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        strCompName = Session("SS_COMPANYNAME")
        strLocName = Session("SS_LOCATIONNAME")
        strUserName = Session("SS_USERNAME")

        crvView.Visible = False

        strProgramPath = Trim(Request.QueryString("strProgramPath"))
        strPayId = Trim(Request.QueryString("strId"))
        strSortLine = Trim(Request.QueryString("strSortLine"))
        strCBType = Trim(Request.QueryString("CBType"))
        TRXType = Trim(Request.QueryString("TRXType"))
        strCurrencyCode = Trim(Request.QueryString("strCurrencyCode"))
        strExchangeRate = Trim(Request.QueryString("strExchangeRate"))
        strBiaya = Trim(Request.QueryString("strBiaya"))
        strDeduct = Trim(Request.QueryString("strDeduct"))
        strAmtToPrint = Trim(Request.QueryString("strAmtToPrint"))
        strChequeNoToPrint = Trim(Request.QueryString("strChequeNoToPrint"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim objRptDs As New DataSet()
        Dim objMapPath As New Object()
        Dim strOpCd_Get As String = ""
        Dim strOpCd_GetLine As String = ""
        Dim strOpCodes As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strReportName As String
        Dim strPDFName As String
        Dim objFTPFolder As String
        Dim dblTotal As Double

        strReportName = strProgramPath '& ".rpt"

        strOpCd_Get = "CB_CLSTRX_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX" & "|" & "CashBank"
        strOpCd_GetLine = "CB_CLSTRX_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX_LINE" & "|" & "CashBankLn"

        strOpCodes = strOpCd_Get & Chr(9) & strOpCd_GetLine
        strParam = strPayId & "|" & strSortLine

        Try
            intErrNo = objCB.mtdGetCashBankDocRpt(strOpCodes, _
                                                 strParam, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 objRptDs, objMapPath, objFTPFolder)

        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try




        'If TRXType = "CASHBANK" Then
        '    strOpCd_Get = "CB_CLSTRX_CASHBANK_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX" & "|" & "CashBank"
        '    strOpCd_GetLine = "CB_CLSTRX_CASHBANK_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX_LINE" & "|" & "CashBankLn"

        '    strOpCodes = strOpCd_Get & Chr(9) & strOpCd_GetLine
        '    strParam = strPayId & "|" & strSortLine

        '    Try
        '        intErrNo = objCB.mtdGetCashBankDocRpt(strOpCodes, _
        '                                             strParam, _
        '                                             strCompany, _
        '                                             strLocation, _
        '                                             strUserId, _
        '                                             strAccMonth, _
        '                                             strAccYear, _
        '                                             objRptDs)

        '    Catch Exp As Exception
        '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        '    End Try

        'ElseIf TRXType = "PAYMENT" Then
        '    strOpCd_Get = "CB_CLSTRX_PAYMENT_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX" & "|" & "CashBank"
        '    strOpCd_GetLine = "CB_CLSTRX_PAYMENT_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX_LINE" & "|" & "CashBankLn"

        '    strOpCodes = strOpCd_Get & Chr(9) & strOpCd_GetLine
        '    strParam = strPayId & "|" & strSortLine

        '    Try
        '        intErrNo = objCB.mtdGetPaymentDocRpt(strOpCodes, _
        '                                             strParam, _
        '                                             strCompany, _
        '                                             strLocation, _
        '                                             strUserId, _
        '                                             strAccMonth, _
        '                                             strAccYear, _
        '                                             objRptDs)

        '    Catch Exp As Exception
        '        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        '    End Try
        'End If


        

        Try
            intErrNo = objAdmin.mtdGetBasePath(objMapPath)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        If objRptDs.Tables(0).Rows.Count > 0 Then
            If objRptDs.Tables(0).Rows.Count > 1 And objRptDs.Tables(0).Rows(0).Item("SourceType") = "3" Then
                lblErrMesage.Text = "There are different supplier code between transactions."
                lblErrMesage.Visible = True
                Exit Sub
            End If

            'strTotalAmount = CStr(objRptDs.Tables(0).Rows(0).Item("TotalAmount")) remark coz can print multi giro in 1 BP
            strBasicTotalAmount = objRptDs.Tables(0).Rows(0).Item("TotalAmount")
            'objRptDs.Tables("CashBank").Rows(0).Item("ChequeNo") = strChequeNoToPrint
            objRptDs.Tables("CashBank").Rows(0).Item("GiroNo") = strChequeNoToPrint
            strTotalAmount = strAmtToPrint
            arrTotalAmount = Split(strTotalAmount, "")

            strBeforeDec = objGlobal.TerbilangDesimal((CDbl(arrTotalAmount(0))), "Rupiah")

            'strCBType 1=print cheque, 2=print setoran, 3=print transfers
            objRptDs.Tables("CashBank").Rows(0).Item("TotalAmount") = CDbl(arrTotalAmount(0))

            If strCBType = "3" Then
                If objRptDs.Tables("CashBankLn").Rows.Count > 0 Then
                    For intCnt = 0 To objRptDs.Tables("CashBankLn").Rows.Count - 1
                        arrTotalAmount(0) = CDbl(arrTotalAmount(0)) - objRptDs.Tables("CashBankLn").Rows(intCnt).Item("Total")
                    Next
                End If
            End If

            If strCBType = "2" Or strCBType = "3" Then
                If strDeduct = "1" Then
                    If UCase(Trim(strCurrencyCode)) = "IDR" Then
                        If objRptDs.Tables("CashBankln").Rows.Count > 0 Then
                            objRptDs.Tables("CashBankln").Rows(0).Item("Total") = CDbl(objRptDs.Tables("CashBankln").Rows(0).Item("Total")) + CDbl(Str(strBiaya))
                        End If
                        arrTotalAmount(0) = CDbl(arrTotalAmount(0)) - CDbl(Str(strBiaya))
                    End If
                Else
                    If UCase(Trim(strCurrencyCode)) = "IDR" Then
                        If Val(strBiaya) <> 0 Or strBiaya <> "0" Then
                            If objRptDs.Tables("CashBank").Rows.Count > 0 Then
                                'objRptDs.Tables("CashBank").Rows(0).Item("TotalAmount") = CDbl(objRptDs.Tables("CashBank").Rows(0).Item("TotalAmount")) + CDbl(Str(strBiaya))
                                objRptDs.Tables("CashBank").Rows(0).Item("TotalAmount") = CDbl(arrTotalAmount(0)) + CDbl(Str(strBiaya))
                            End If
                            If objRptDs.Tables("CashBankln").Rows.Count > 0 Then
                                objRptDs.Tables("CashBankln").Rows(0).Item("Total") = CDbl(objRptDs.Tables("CashBankln").Rows(0).Item("Total")) + CDbl(Str(strBiaya))
                            End If
                            arrTotalAmount(0) = CDbl(arrTotalAmount(0))
                        End If
                    End If
                End If
            End If

            Dim strOpCode As String
            Dim dsMaster As Object
            Dim strParamName As String = ""
            Dim strParamValue As String = ""
            Dim totalrp As String

            'If TRXType = "CASHBANK" Then
            '    strParamName = "CASHBANKID"
            '    strOpCode = "CB_CLSTRX_CASHBANK_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX_LINE"
            'Else
            '    strParamName = "PAYMENTID"
            '    strOpCode = "CB_CLSTRX_PAYMENT_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX_LINE"
            'End If

            strParamName = "CASHBANKID"
            strOpCode = "CB_CLSTRX_GET_FOR_DOCRPT_CHEQUEGIRO_MULTITRX_LINE"

            strParamValue = Trim(strPayId)

            Try
                intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                    strParamName, _
                                                    strParamValue, _
                                                    dsMaster)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
            End Try

            If dsMaster.Tables(0).Rows.Count > 0 Then
                If UCase(Trim(dsMaster.Tables(0).Rows(0).Item("CurrencyCode"))) = "IDR" Then
                    If UCase(Trim(strCurrencyCode)) = "IDR" Then
                        totalrp = Trim(CStr(FormatNumber(arrTotalAmount(0), 2)))
                        strBeforeDec = objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah")
                    Else
                        arrTotalAmount(0) = CDbl(arrTotalAmount(0)) / CDbl(IIf(strExchangeRate = "0", "1", strExchangeRate))
                        If objRptDs.Tables("CashBank").Rows.Count > 0 Then
                            objRptDs.Tables("CashBank").Rows(0).Item("TotalAmount") = arrTotalAmount(0)
                        End If
                        arrTotalAmount(0) = FormatNumber(arrTotalAmount(0), 2)
                        'strBeforeDec = objGlobal.ConvertNo2WordsDecimal((arrTotalAmount(0)))
                        strBeforeDec = ConvertCurrencyToEnglish(arrTotalAmount(0))
                    End If
                Else
                    strBeforeDec = objGlobal.ConvertNo2WordsDecimal((arrTotalAmount(0)))
                End If
            End If
            strTotalAmount = UCase(Replace(strBeforeDec, "Zero", ""))

            Dim strOpCd As String = "CB_CLSTRX_CBPAY_INSERT_PRINTDOCUMENT"
            strParamName = ""
            strParamValue = ""

            strParamName = "CHEQUENO|CHEQUENOREF|PRINTDATE|TRXTYPE|BANKFROM|BANKTO|BANKACCNO|CURRENCYCODE|EXCHANGERATE|TOTALAMOUNT|TOTALAMOUNTTOPRINT|ADMAMOUNT|DEDUCT"

            strParamValue = Trim(strChequeNoToPrint) & _
                            "|" & strPayId & _
                            "|" & Now() & _
                            "|" & TRXType & _
                            "|" & Trim(objRptDs.Tables("CashBank").Rows(0).Item("BankCode")) & _
                            "|" & Trim(objRptDs.Tables("CashBank").Rows(0).Item("SuppBankName")) & _
                            "|" & Trim(objRptDs.Tables("CashBank").Rows(0).Item("BankAccNo")) & _
                            "|" & Trim(strCurrencyCode) & _
                            "|" & Trim(strExchangeRate) & _
                            "|" & Trim(strBasicTotalAmount) & _
                            "|" & Trim(strAmtToPrint) & _
                            "|" & Trim(strBiaya) & _
                            "|" & Trim(strDeduct)

            Try
                intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                        strParamName, _
                                                        strParamValue)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_CASHBANK_PREVIEWCHEQUE_UPD&errmesg=" & Exp.ToString() & "&redirect=CB/trx/CB_trx_CashBankDet.aspx")
            End Try
        End If


        rdCrystalViewer.Load(objMapPath & "Web\EN\CB\Reports\Crystal\" & strReportName & ".rpt", OpenReportMethod.OpenReportByTempCopy)
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
            'DiskOpts.DiskFileName = objMapPath & "web\ftp\" & strReportName & ".pdf"
            DiskOpts.DiskFileName = objFTPFolder & strReportName & ".pdf"
            rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts

            rdCrystalViewer.Export()
            rdCrystalViewer.Close()
            rdCrystalViewer.Dispose()

            Dim strUrl As String
            strUrl = objFTPFolder.Substring(InStr(objFTPFolder, "ftp") - 1, Len(objFTPFolder) - InStr(objFTPFolder, "ftp") + 1)
            strUrl = Replace(strUrl, "\", "/")
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../" & strUrl & strReportName & ".pdf"">")
            'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/" & strReportName & ".pdf"">")
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
        Dim paramField7 As New ParameterField()
        Dim paramField8 As New ParameterField()
        Dim paramField9 As New ParameterField()
        Dim paramField10 As New ParameterField()
        Dim paramField11 As New ParameterField()

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue6 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue7 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue8 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue9 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue10 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue11 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues
        Dim crParameterValues5 As ParameterValues
        Dim crParameterValues6 As ParameterValues
        Dim crParameterValues7 As ParameterValues
        Dim crParameterValues8 As ParameterValues
        Dim crParameterValues9 As ParameterValues
        Dim crParameterValues10 As ParameterValues
        Dim crParameterValues11 As ParameterValues

        Dim crDataDef As DataDefinition
        Dim PFDefs As ParameterFieldDefinitions

        crDataDef = rdCrystalViewer.DataDefinition
        PFDefs = crDataDef.ParameterFields
        paramFields = crvView.ParameterFieldInfo

        Dim strOpCode As String = "ADMIN_CLSLOC_LOCATION_DETAILS_GET"
        Dim dsMaster As Object
        Dim strParamName As String = ""
        Dim strParamValue As String = ""
        Dim intErrNo As Integer

        strParamName = "LOCCODE"
        strParamValue = Trim(strLocation)

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
                                                strParamName, _
                                                strParamValue, _
                                                dsMaster)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try

        If dsMaster.Tables(0).Rows.Count > 0 Then
            strCompAddress = Trim(dsMaster.Tables(0).Rows(0).Item("Address"))
            strCompCity = Trim(dsMaster.Tables(0).Rows(0).Item("City"))
            strCompTelp = Trim(dsMaster.Tables(0).Rows(0).Item("TelNo"))
            strCompCountry = Trim(dsMaster.Tables(0).Rows(0).Item("Country"))

            'strOpCode = "ADMIN_CLSCOUNTRY_COUNTRY_DETAILS_GET"
            'strParamName = "COUNTRYCODE|SEARCHSTR"
            'strParamValue = Trim(strCompCountry) & "|" & ""

            'Try
            '    intErrNo = objGLtrx.mtdGetDataCommon(strOpCode, _
            '                                        strParamName, _
            '                                        strParamValue, _
            '                                        dsMaster)
            'Catch Exp As System.Exception
            '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATE_INVOICE_WM_GET_HEADER&errmesg=" & Exp.ToString() & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
            'End Try

            'If dsMaster.Tables(0).Rows.Count > 0 Then
            '    strCompCountry = Trim(dsMaster.Tables(0).Rows(0).Item("CountryDesc"))
            'End If
        End If

        paramField1 = paramFields.Item("strPrintedBy")
        paramField2 = paramFields.Item("strCompName")
        paramField3 = paramFields.Item("strLocName")
        paramField4 = paramFields.Item("TotalAmountSentence")
        paramField5 = paramFields.Item("strCompAddress")
        paramField6 = paramFields.Item("strCompCity")
        paramField7 = paramFields.Item("strCompTelp")
        paramField8 = paramFields.Item("strCompCountry")
        paramField9 = paramFields.Item("strCurrencyCode")
        paramField10 = paramFields.Item("strExchangeRate")
        paramField11 = paramFields.Item("strBiaya")

        crParameterValues1 = paramField1.CurrentValues
        crParameterValues2 = paramField2.CurrentValues
        crParameterValues3 = paramField3.CurrentValues
        crParameterValues4 = paramField4.CurrentValues
        crParameterValues5 = paramField5.CurrentValues
        crParameterValues6 = paramField6.CurrentValues
        crParameterValues7 = paramField7.CurrentValues
        crParameterValues8 = paramField8.CurrentValues
        crParameterValues9 = paramField9.CurrentValues
        crParameterValues10 = paramField10.CurrentValues
        crParameterValues11 = paramField11.CurrentValues

        ParamDiscreteValue1.Value = strUserName
        ParamDiscreteValue2.Value = UCase(strCompName)
        ParamDiscreteValue3.Value = UCase(strLocName)
        ParamDiscreteValue4.Value = strTotalAmount
        ParamDiscreteValue5.Value = UCase(strCompAddress)
        ParamDiscreteValue6.Value = UCase(strCompCity)
        ParamDiscreteValue7.Value = strCompTelp
        ParamDiscreteValue8.Value = UCase(strCompCountry)
        ParamDiscreteValue9.Value = UCase(strCurrencyCode)
        ParamDiscreteValue10.Value = strExchangeRate
        ParamDiscreteValue11.Value = strBiaya

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)
        crParameterValues5.Add(ParamDiscreteValue5)
        crParameterValues6.Add(ParamDiscreteValue6)
        crParameterValues7.Add(ParamDiscreteValue7)
        crParameterValues8.Add(ParamDiscreteValue8)
        crParameterValues9.Add(ParamDiscreteValue9)
        crParameterValues10.Add(ParamDiscreteValue10)
        crParameterValues11.Add(ParamDiscreteValue11)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)
        PFDefs(4).ApplyCurrentValues(crParameterValues5)
        PFDefs(5).ApplyCurrentValues(crParameterValues6)
        PFDefs(6).ApplyCurrentValues(crParameterValues7)
        PFDefs(7).ApplyCurrentValues(crParameterValues8)
        PFDefs(8).ApplyCurrentValues(crParameterValues9)
        PFDefs(9).ApplyCurrentValues(crParameterValues10)
        PFDefs(10).ApplyCurrentValues(crParameterValues11)

        crvView.ParameterFieldInfo = paramFields
    End Sub

    Public Function ConvertCurrencyToEnglish(ByVal MyNumber As Double) As String
        Dim Temp As String
        Dim Dollars, Cents As String
        Dim DecimalPlace, Count As Integer
        Dim Place(9) As String
        Dim Numb As String
        Place(2) = " Thousand " : Place(3) = " Million " : Place(4) = " Billion " : Place(5) = " Trillion "
        ' Convert Numb to a string, trimming extra spaces.
        Numb = Trim(Str(MyNumber))
        ' Find decimal place.
        DecimalPlace = InStr(Numb, ".")
        ' If we find decimal place...
        If DecimalPlace > 0 Then
            ' Convert cents
            Temp = Left(Mid(Numb, DecimalPlace + 1) & "00", 2)
            Cents = ConvertTens(Temp)
            ' Strip off cents from remainder to convert.
            Numb = Trim(Left(Numb, DecimalPlace - 1))
        End If
        Count = 1
        Do While Numb <> ""
            ' Convert last 3 digits of Numb to English dollars.
            Temp = ConvertHundreds(Right(Numb, 3))
            If Temp <> "" Then Dollars = Temp & Place(Count) & Dollars
            If Len(Numb) > 3 Then
                ' Remove last 3 converted digits from Numb.
                Numb = Left(Numb, Len(Numb) - 3)
            Else
                Numb = ""
            End If
            Count = Count + 1
        Loop

        ' Clean up dollars.
        Select Case Dollars
            Case "" : Dollars = "No "
            Case "One" : Dollars = "One"
            Case Else : Dollars = Dollars & ""
        End Select

        ' Clean up cents.
        Select Case Cents
            Case "" : Cents = ""
            Case "One" : Cents = " And One Cent"
            Case Else : Cents = " And Cents " & Cents & ""
        End Select
        ConvertCurrencyToEnglish = Dollars & Cents
    End Function

    Private Function ConvertHundreds(ByVal MyNumber As String) As String
        Dim Result As String
        ' Exit if there is nothing to convert.
        If Val(MyNumber) = 0 Then Exit Function
        ' Append leading zeros to number.
        MyNumber = Right("000" & MyNumber, 3)
        ' Do we have a hundreds place digit to convert?
        If Left(MyNumber, 1) <> "0" Then
            Result = ConvertDigit(Left(MyNumber, 1)) & " Hundred "
        End If
        ' Do we have a tens place digit to convert?
        If Mid(MyNumber, 2, 1) <> "0" Then
            Result = Result & ConvertTens(Mid(MyNumber, 2))
        Else
            ' If not, then convert the ones place digit.
            Result = Result & ConvertDigit(Mid(MyNumber, 3))
        End If
        ConvertHundreds = Trim(Result)
    End Function

    Private Function ConvertTens(ByVal MyTens As String) As String
        Dim Result As String
        ' Is value between 10 and 19?
        If Val(Left(MyTens, 1)) = 1 Then
            Select Case Val(MyTens)
                Case 10 : Result = "Ten"
                Case 11 : Result = "Eleven"
                Case 12 : Result = "Twelve"
                Case 13 : Result = "Thirteen"
                Case 14 : Result = "Fourteen"
                Case 15 : Result = "Fifteen"
                Case 16 : Result = "Sixteen"
                Case 17 : Result = "Seventeen"
                Case 18 : Result = "Eighteen"
                Case 19 : Result = "Nineteen"
                Case Else
            End Select
        Else
            ' .. otherwise it's between 20 and 99.
            Select Case Val(Left(MyTens, 1))
                Case 2 : Result = "Twenty "
                Case 3 : Result = "Thirty "
                Case 4 : Result = "Forty "
                Case 5 : Result = "Fifty "
                Case 6 : Result = "Sixty "
                Case 7 : Result = "Seventy "
                Case 8 : Result = "Eighty "
                Case 9 : Result = "Ninety "
                Case Else
            End Select
            ' Convert ones place digit.
            Result = Result & ConvertDigit(Right(MyTens, 1))
        End If
        ConvertTens = Result
    End Function

    Private Function ConvertDigit(ByVal MyDigit As String) As String
        Select Case Val(MyDigit)
            Case 1 : ConvertDigit = "One"
            Case 2 : ConvertDigit = "Two"
            Case 3 : ConvertDigit = "Three"
            Case 4 : ConvertDigit = "Four"
            Case 5 : ConvertDigit = "Five"
            Case 6 : ConvertDigit = "Six"
            Case 7 : ConvertDigit = "Seven"
            Case 8 : ConvertDigit = "Eight"
            Case 9 : ConvertDigit = "Nine"
            Case Else : ConvertDigit = ""
        End Select
    End Function

End Class

