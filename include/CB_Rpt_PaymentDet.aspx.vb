
Imports System
Imports System.Data
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


Public Class CB_Rpt_PaymentDet : inherits page

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

    Dim strStatus As String
    Dim strPayId As String
    Dim strPrintDate As String
    Dim strSortLine As String

    Dim strCompName As String
    Dim strLocName As String
    Dim strUserName As String
    Dim strAccountTag As String
    Dim strTotalAmount As String = ""
    Dim arrTotalAmount(15) As String
    Dim strBeforeDec As String = ""
    Dim strAfterDec As String = ""

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

        strStatus = Trim(Request.QueryString("strStatus"))
        strPayId = Trim(Request.QueryString("strPayId"))
        strPrintDate = Trim(Request.QueryString("strPrintDate"))
        strSortLine = Trim(Request.QueryString("strSortLine"))
        strAccountTag = Trim(Request.QueryString("strAccountTag"))

        Bind_ITEM(True)

    End Sub

    Sub Bind_ITEM(ByVal blnIsPDFFormat As Boolean)
        Dim crLogonInfo As CrystalDecisions.Shared.TableLogOnInfo
        Dim objRptDs As New Dataset()
        Dim objMapPath As New Object()
        Dim strOpCd_Get As String = ""
        Dim strOpCd_GetLine As String = ""
        Dim strOpCodes As String
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim strReportName As String
        Dim strPDFName As String
        Dim totalrp As String
        Dim objFTPFolder As String

        strOpCd_Get = "CB_CLSTRX_PAYMENT_GET_FOR_DOCRPT" & "|" & "Payment"
        strOpCd_GetLine = "CB_CLSTRX_PAYMENT_LINE_GET_FOR_DOCRPT" & "|" & "PaymentLn"
        strReportName = "CB_RPT_PaymentDet"

        strOpCodes = strOpCd_Get & chr(9) & strOpCd_GetLine
        strParam = strPayId & "|" & strSortLine

        Try
            intErrNo = objCB.mtdGetPaymentDocRpt(strOpCodes, _
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

        'Try
        '    intErrNo = objAdmin.mtdGetBasePath(objMapPath)
        'Catch Exp As Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=&errmesg=" & lblErrMesage.Text & "&redirect=")
        'End Try

        If objRptDs.Tables("Payment").Rows.Count > 0 Then
            'For intCnt=0 To objRptDs.Tables("Payment").Rows.Count - 1
            '    objRptDs.Tables("Payment").Rows(intCnt).Item("PaymentType") = objCB.mtdGetPaymentType(CInt(objRptDs.Tables("Payment").Rows(intCnt).Item("PaymentType")))
            'Next
            If objRptDs.Tables("Payment").Rows(0).Item("TotalAmount") < 0 Then
                strTotalAmount = objRptDs.Tables("Payment").Rows(0).Item("TotalAmount") * -1
            Else
                strTotalAmount = objRptDs.Tables("Payment").Rows(0).Item("TotalAmount")
            End If
            strTotalAmount = CStr(strTotalAmount)
            arrTotalAmount = Split(strTotalAmount, "")

            totalrp = Trim(CStr(FormatNumber(arrTotalAmount(0), 2)))
            strBeforeDec = objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah")
            'strBeforeDec = objGlobal.TerbilangDesimal((CDbl(arrTotalAmount(0))), "Rupiah")
            strTotalAmount = strBeforeDec

            Dim strOpCode As String
            Dim dsMaster As Object
            Dim strParamName As String = ""
            Dim strParamValue As String = ""

            strParamName = "PAYMENTID"
            strOpCode = "CB_CLSTRX_PAYMENT_LINE_GET_FOR_DOCRPT_CHEQUEGIRO"

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
                    totalrp = Trim(CStr(FormatNumber(arrTotalAmount(0), 2)))
                    strBeforeDec = Replace(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1), Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1), UCase(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1))) + Mid(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 2, Len(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah")))) - 1)
                    'objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah")
                    'strBeforeDec = objGlobal.TerbilangDesimal((CDbl(arrTotalAmount(0))), "Rupiah")
                Else
                    If Left(Trim(objRptDs.Tables("Payment").Rows(0).Item("PaymentID")), 2) = "KK" Then
                        totalrp = objRptDs.Tables("Payment").Rows(0).Item("TotalAmountIDR")
                        strBeforeDec = Replace(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1), Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1), UCase(Left(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 1))) + Mid(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah"))), 2, Len(LTrim(LCase(objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah")))) - 1)
                        'objGlobal.TerbilangDesimal(Replace(totalrp, ",", ""), "Rupiah")
                    Else
                        'strBeforeDec = objGlobal.ConvertNo2WordsDecimal((arrTotalAmount(0)))
                        strBeforeDec = ConvertCurrencyToEnglish(FormatNumber(arrTotalAmount(0), 2))
                    End If
                End If
            End If
            strTotalAmount = Replace(strBeforeDec, "Zero", "")
        End If

        If objRptDs.Tables("PaymentLn").Rows.Count > 0 Then
            For intCnt = 0 To objRptDs.Tables("PaymentLn").Rows.Count - 1
                objRptDs.Tables("PaymentLn").Rows(intCnt).Item("DocType") = objCB.mtdGetPaymentDocType(CInt(objRptDs.Tables("PaymentLn").Rows(intCnt).Item("DocType")))
            Next
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
            'DiskOpts.DiskFileName = objMapPath & "web\ftp\CB_RPT_PaymentDet.pdf"
            DiskOpts.DiskFileName = objFTPFolder & strReportName & ".pdf"
            rdCrystalViewer.ExportOptions.DestinationOptions = DiskOpts

            rdCrystalViewer.Export()
            rdCrystalViewer.Close()
            rdCrystalViewer.Dispose()

            Dim strUrl As String
            strUrl = objFTPFolder.Substring(InStr(objFTPFolder, "ftp") - 1, Len(objFTPFolder) - InStr(objFTPFolder, "ftp") + 1)
            strUrl = Replace(strUrl, "\", "/")
            Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../" & strUrl & strReportName & ".pdf"">")
            'Response.Write("<META HTTP-EQUIV=""refresh"" CONTENT=""0; URL=../../../ftp/CB_RPT_PaymentDet.pdf"">")
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

        Dim ParamDiscreteValue1 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue2 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue3 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue4 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue5 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue6 As New ParameterDiscreteValue()
        Dim ParamDiscreteValue7 As New ParameterDiscreteValue()

        Dim crParameterValues1 As ParameterValues
        Dim crParameterValues2 As ParameterValues
        Dim crParameterValues3 As ParameterValues
        Dim crParameterValues4 As ParameterValues
        Dim crParameterValues5 As ParameterValues
        Dim crParameterValues6 As ParameterValues
        Dim crParameterValues7 As ParameterValues

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
        ParamField6 = ParamFields.Item("lblAccount")
        ParamField7 = ParamFields.Item("TotalAmountSentence")

        crParameterValues1 = ParamField1.CurrentValues
        crParameterValues2 = ParamField2.CurrentValues
        crParameterValues3 = ParamField3.CurrentValues
        crParameterValues4 = ParamField4.CurrentValues
        crParameterValues5 = ParamField5.CurrentValues
        crParameterValues6 = ParamField6.CurrentValues
        crParameterValues7 = ParamField7.CurrentValues

        ParamDiscreteValue1.value = strPrintDate
        ParamDiscreteValue2.value = strUserName
        ParamDiscreteValue3.value = strCompName
        ParamDiscreteValue4.value = strLocName
        ParamDiscreteValue5.value = strStatus
        ParamDiscreteValue6.value = strAccountTag
        ParamDiscreteValue7.value = strTotalAmount

        crParameterValues1.Add(ParamDiscreteValue1)
        crParameterValues2.Add(ParamDiscreteValue2)
        crParameterValues3.Add(ParamDiscreteValue3)
        crParameterValues4.Add(ParamDiscreteValue4)
        crParameterValues5.Add(ParamDiscreteValue5)
        crParameterValues6.Add(ParamDiscreteValue6)
        crParameterValues7.Add(ParamDiscreteValue7)

        PFDefs(0).ApplyCurrentValues(crParameterValues1)
        PFDefs(1).ApplyCurrentValues(crParameterValues2)
        PFDefs(2).ApplyCurrentValues(crParameterValues3)
        PFDefs(3).ApplyCurrentValues(crParameterValues4)
        PFDefs(4).ApplyCurrentValues(crParameterValues5)
        PFDefs(5).ApplyCurrentValues(crParameterValues6)
        PFDefs(6).ApplyCurrentValues(crParameterValues7)

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

