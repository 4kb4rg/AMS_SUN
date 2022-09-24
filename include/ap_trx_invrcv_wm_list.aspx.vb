Imports System
Imports System.Data
Imports System.Collections 
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction


Public Class ap_trx_invrcv_wm_list : Inherits Page

    Protected WithEvents dgList As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrDate As Label
    Protected WithEvents lblErrDateMsg As Label
    Protected WithEvents lblErrDateTo As Label
    Protected WithEvents lblErrDateToMsg As Label

    Protected WithEvents srchTrxID As TextBox
    Protected WithEvents srchRefNo As TextBox
    Protected WithEvents srchDate As TextBox
    Protected WithEvents srchDateTo As TextBox
    Protected WithEvents srchSupplier As TextBox
    Protected WithEvents srchStatusList As DropDownList
    Protected WithEvents srchUpdateBy As TextBox
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents GenInvBtn As ImageButton

    Protected WithEvents dgListSUM As DataGrid
    Protected WithEvents cbExcel As CheckBox
    Protected WithEvents PrintPrev As ImageButton
    Protected WithEvents btnGenerate As ImageButton

    Protected WithEvents dgListPay As DataGrid
    Protected WithEvents cbExcelPay As CheckBox
    Protected WithEvents PrintPrevPay As ImageButton

    Protected WithEvents ddlTBSPemilik As DropDownList
    Protected WithEvents ddlTBSAgen As DropDownList
    Protected WithEvents ddlPPN As DropDownList
    Protected WithEvents ddlPPH As DropDownList
    Protected WithEvents ddlOB As DropDownList
    Protected WithEvents ddlOL As DropDownList

    Protected WithEvents lblErrGenerate As Label
    Protected WithEvents lblErrGenInv As Label

    Protected WithEvents cbDetailByInvoice As CheckBox
    Protected WithEvents cbExcelListRekap As CheckBox
    Protected WithEvents cbExcelList As CheckBox
    Protected WithEvents hidSearch As HtmlInputHidden

    Protected objGLTrx As New agri.GL.ClsTrx()
    Protected objAPTrx As New agri.AP.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objGLSetup As New agri.GL.clsSetup()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer
    Dim strDateFormat As String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer
    Dim strParamName As String
    Dim strParamValue As String

    Dim BtnConfirm As Button
    Dim BtnCancel As Button

    Dim objTicketDs As New DataSet()

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_PMACCMONTH")
        strAccYear = Session("SS_PMACCYEAR")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intAPAR = Session("SS_APAR")
        strDateFormat = Session("SS_DATEFMT")
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APInvoiceReceive), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrDateMsg.Visible = False
            lblErrDate.Visible = False
            lblErrDateToMsg.Visible = False
            lblErrDateTo.Visible = False
            lblErrGenerate.Visible = False
            lblErrGenInv.Visible = False

            If SortExpression.Text = "" Then
                SortExpression.Text = "A.TrxID"
            End If

            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            GenInvBtn.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(GenInvBtn).ToString())
            btnGenerate.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(btnGenerate).ToString())


            If Not Page.IsPostBack Then
                srchDate.Text = "1/" & strSelAccMonth & "/" & strSelAccYear
                srchDateTo.Text = DateAdd(DateInterval.Month, 1, CDate(strSelAccMonth & "/1/" & strSelAccYear))
                srchDateTo.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), DateAdd(DateInterval.Day, -1, CDate(srchDateTo.Text)))
                BindSearchStatusList()
                BindGrid()
                BindGridSUM()
                BindPageList()
                BindAccount("", "", "", "", "", "")
                LoadCOASetting()

            End If
        End If
    End Sub

    Sub BindSearchStatusList()

        srchStatusList.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.All), objAPTrx.EnumInvoiceRcvStatus.All))
        srchStatusList.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Active), objAPTrx.EnumInvoiceRcvStatus.Active))
        srchStatusList.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Confirmed), objAPTrx.EnumInvoiceRcvStatus.Confirmed))
        srchStatusList.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Deleted), objAPTrx.EnumInvoiceRcvStatus.Deleted))
        srchStatusList.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Cancelled), objAPTrx.EnumInvoiceRcvStatus.Cancelled))
        srchStatusList.SelectedIndex = 1

    End Sub

   
    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgList.CurrentPageIndex = 0
        dgList.EditItemIndex = -1
        BindGrid()
        BindGridSUM()
        BindPageList()
    End Sub

    Sub BindGrid()
        Dim lbButton As LinkButton
        Dim intCnt As Integer
        Dim Status As Label
        Dim strStatus As String

        Dim PageNo As Integer 
        Dim PageCount As Integer
        Dim dsData As DataSet

        Dim UpdButton As LinkButton
        Dim DelButton As LinkButton
        Dim EdtButton As LinkButton
        Dim CanButton As LinkButton
        
        dsData = LoadData
        PageCount = objGlobal.mtdGetPageCount(dsData.Tables(0).Rows.Count, dgList.PageSize)
        
        dgList.DataSource = dsData
        If dgList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgList.CurrentPageIndex = 0
            Else
                dgList.CurrentPageIndex = PageCount - 1
            End If
        End If
        
        dgList.DataBind()
        BindPageList()
        PageNo = dgList.CurrentPageIndex + 1
        lblTracker.Text="Page " & pageno & " of " & dgList.PageCount


        For intCnt = 0 To dgList.Items.Count - 1
            Status = dgList.Items.Item(intCnt).FindControl("lblStatus")
            strStatus = Status.Text

            BtnConfirm = dgList.Items.Item(intCnt).FindControl("BtnConfirm")
            BtnCancel = dgList.Items.Item(intCnt).FindControl("BtnCancel")
            BtnConfirm.Attributes("onclick") = "javascript:return ConfirmAction('confirm');"
            BtnCancel.Attributes("onclick") = "javascript:return ConfirmAction('cancel');"

            EdtButton = dgList.Items.Item(intCnt).FindControl("Edit")
            DelButton = dgList.Items.Item(intCnt).FindControl("Delete")
            UpdButton = dgList.Items.Item(intCnt).FindControl("Update")
            CanButton = dgList.Items.Item(intCnt).FindControl("Cancel")

            Select Case strStatus
                Case objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Active)
                    BtnConfirm.Visible = True
                    BtnCancel.Visible = False

                    EdtButton.Visible = False
                    DelButton.Visible = False
                    UpdButton.Visible = False
                    CanButton.Visible = False

                Case objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Deleted)
                    BtnConfirm.Visible = False
                    BtnCancel.Visible = False

                    EdtButton.Visible = False
                    DelButton.Visible = False
                    UpdButton.Visible = False
                    CanButton.Visible = False

                Case objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Confirmed)
                    BtnConfirm.Visible = False
                    BtnCancel.Visible = True

                    EdtButton.Visible = False
                    DelButton.Visible = False
                    UpdButton.Visible = False
                    CanButton.Visible = False

                Case Else
                    BtnConfirm.Visible = False
                    BtnCancel.Visible = False

                    EdtButton.Visible = False
                    DelButton.Visible = False
                    UpdButton.Visible = False
                    CanButton.Visible = False
            End Select
        Next

    End Sub

    Sub BindGridSUM()
        Dim lbButton As LinkButton
        Dim intCnt As Integer
        Dim Status As Label
        Dim strStatus As String

        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim dsData As DataSet
        Dim lbl As Label

        dsData = LoadDataSUM()

        dgListSUM.DataBind()
        dgListPay.DataBind()

        For intCnt = 0 To dgListSUM.Items.Count - 1
            lbl = dgListSUM.Items.Item(intCnt).FindControl("lblNoUrut")
            If Trim(lbl.Text) = "999" Then
                lbl.Visible = False
            End If
        Next

        For intCnt = 0 To dgListPay.Items.Count - 1
            lbl = dgListPay.Items.Item(intCnt).FindControl("lblNoUrut")
            If Trim(lbl.Text) = "999" Then
                lbl.Visible = False
            End If
        Next
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgList.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While

        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgList.CurrentPageIndex

    End Sub

    Protected Function LoadData() As DataSet

        Dim strOpCd As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_SEARCH"

        Dim dsResult As New Object

        Dim strSrchTrxID As String
        Dim strSrchRefNo As String
        Dim strSrchDate As String = ""
        Dim strSrchDateTo As String = ""
        Dim strSrchSupplier As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim objFormatDate As String
        Dim objActualDate As String

       

        strSrchTrxID = IIf(Trim(srchTrxID.Text) = "", "", " AND  A.TRXID LIKE '%" & srchTrxID.Text & "%'")
        strSrchRefNo = IIf(Trim(srchRefNo.Text) = "", "", " AND  RefNo LIKE '" & srchRefNo.Text & "'")

       
        If Not srchDate.Text = "" Then
            strSrchDate = IIf(Trim(srchDate.Text) = "", "", " AND  RefDate = '" & Date_Validation(srchDate.Text, False)) & "'"
        End If

        If Not srchDateTo.Text = "" Then
            strSrchDate = IIf(Trim(srchDateTo.Text) = "", "", " AND  RefDate BETWEEN '" & Date_Validation(srchDate.Text, False) & "' AND '" & Date_Validation(srchDateTo.Text, False)) & "'"
        End If


        strSrchSupplier = IIf(srchSupplier.Text = "", "", " AND (A.SupplierCode LIKE '" & srchSupplier.Text & "' OR E.Name LIKE '%" & srchSupplier.Text & "%') ")
        strSrchStatus = IIf(srchStatusList.SelectedItem.Value = objAPTrx.EnumInvoiceRcvStatus.All, " AND  A.Status NOT IN ('3','4') ", " AND  A.Status = '" & srchStatusList.SelectedItem.Value & "'")
        strSrchLastUpdate = IIf(srchUpdateBy.Text = "", "", " AND  A.UpdateID LIKE '" & srchUpdateBy.Text & "'")


        strSearch = strSrchTrxID & strSrchRefNo & strSrchDate & strSrchStatus & strSrchSupplier & strSrchLastUpdate & _
                    " AND A.LOCCODE = '" & strLocation & "'"

        strSearch = " WHERE " & MID(Trim(strSearch), 6)

        strSearch = strSearch
        hidSearch.Value = strSearch

        strParamName = "STRSEARCH"
        strParamValue = strSearch

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                 strParamName, _
                                                 strParamValue, _
                                                 dsResult)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATEINV_LOAD&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try



        'For intCnt = 0 To dsResult.Tables(0).Rows.Count - 1
        '    dsResult.Tables(0).Rows(intCnt).Item("TrxID") = Trim(dsResult.Tables(0).Rows(intCnt).Item("TrxID"))
        '    dsResult.Tables(0).Rows(intCnt).Item("RefNo") = Trim(dsResult.Tables(0).Rows(intCnt).Item("RefNo"))
        '    dsResult.Tables(0).Rows(intCnt).Item("RefDate") = Trim(dsResult.Tables(0).Rows(intCnt).Item("RefDate"))
        '    dsResult.Tables(0).Rows(intCnt).Item("SupplierCode") = Trim(dsResult.Tables(0).Rows(intCnt).Item("SupplierCode"))
        '    dsResult.Tables(0).Rows(intCnt).Item("AccCode") = Trim(dsResult.Tables(0).Rows(intCnt).Item("AccCode"))
        '    dsResult.Tables(0).Rows(intCnt).Item("Status") = Trim(dsResult.Tables(0).Rows(intCnt).Item("Status"))
        '    dsResult.Tables(0).Rows(intCnt).Item("UserName") = Trim(dsResult.Tables(0).Rows(intCnt).Item("UserName"))
        'Next

        Return dsResult

    End Function

    Protected Function LoadDataSUM() As DataSet
        Dim strOpCd As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICESUM_SEARCH"
        Dim dsResult As New Object

        Dim strSrchTrxID As String
        Dim strSrchRefNo As String
        Dim strSrchDate As String = ""
        Dim strSrchDateTo As String = ""
        Dim strSrchSupplier As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim objFormatDate As String
        Dim objActualDate As String
        Dim BegDate As String
        Dim EndDate As String



        If Not srchDate.Text = "" Then
            strSrchDate = IIf(Trim(srchDate.Text) = "", "", " AND  RefDate = '" & Date_Validation(srchDate.Text, False)) & "'"
        End If

        If Not srchDateTo.Text = "" Then
            strSrchDate = IIf(Trim(srchDateTo.Text) = "", "", " AND  RefDate BETWEEN '" & Date_Validation(srchDate.Text, False) & "' AND '" & Date_Validation(srchDateTo.Text, False)) & "'"
        End If

        strSrchSupplier = IIf(srchSupplier.Text = "", "", " AND (A.SupplierCode LIKE '" & srchSupplier.Text & "' OR C.Name LIKE '%" & srchSupplier.Text & "%') ")
        strSrchStatus = IIf(srchStatusList.SelectedItem.Value = objAPTrx.EnumInvoiceRcvStatus.All, "", " AND  A.Status = '" & srchStatusList.SelectedItem.Value & "'")
        strSrchLastUpdate = IIf(srchUpdateBy.Text = "", "", " AND  A.UpdateID LIKE '" & srchUpdateBy.Text & "'")

        strSearch = strSrchDate & strSrchStatus & strSrchSupplier & strSrchLastUpdate & _
                    " AND A.LOCCODE = '" & strLocation & "'"

        strSearch = " WHERE " & Mid(Trim(strSearch), 6)

        strSearch = strSearch

        strParamName = "STRSEARCH"
        strParamValue = strSearch

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                 strParamName, _
                                                 strParamValue, _
                                                 dsResult)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATEINV_LOAD&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list.aspx")
        End Try

        dgListSUM.DataSource = dsResult
        dgListSUM.DataBind()

        dgListPay.DataSource = dsResult
        dgListPay.DataBind()

        If dsResult.Tables(0).Rows.Count > 0 Then
            cbExcel.Visible = True
            PrintPrev.Visible = True
            cbExcelPay.Visible = True
            PrintPrevPay.Visible = True
            btnGenerate.Visible = True 'generate berdasarkan data tiket, bukan invoice --> regenerate to accomodate adjustment (revisi/bonus)

            BegDate = "1/" & Month(Date_Validation(srchDateTo.Text, False)) & "/" & Year(Date_Validation(srchDateTo.Text, False))
            EndDate = DateAdd(DateInterval.Month, 1, CDate(Month(Date_Validation(srchDateTo.Text, False)) & "/1/" & Year(Date_Validation(srchDateTo.Text, False))))
            EndDate = objGlobal.GetShortDate(Session("SS_DATEFMT"), DateAdd(DateInterval.Day, -1, CDate(EndDate)))

            If Month(Date_Validation(srchDate.Text, False)) <> Month(Date_Validation(srchDateTo.Text, False)) Then
                btnGenerate.Visible = False
            ElseIf Day(Date_Validation(srchDate.Text, False)) <> 1 Then
                btnGenerate.Visible = False
            ElseIf Day(Date_Validation(srchDateTo.Text, False)) <> Day(Date_Validation(EndDate, False)) Then
                btnGenerate.Visible = False
            End If
        End If

        Return dsResult

    End Function

    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgList.CurrentPageIndex = 0
            Case "prev"
                dgList.CurrentPageIndex = _
                Math.Max(0, dgList.CurrentPageIndex - 1)
            Case "next"
                dgList.CurrentPageIndex = _
                Math.Min(dgList.PageCount - 1, dgList.CurrentPageIndex + 1)
            Case "last"
                dgList.CurrentPageIndex = dgList.PageCount - 1
        End Select

        lstDropList.SelectedIndex = dgList.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgList.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgList.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgList.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCd As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_DEL"

        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        Dim intErrNo As Integer
        Dim strTrxID As String
        Dim lblTrxID As Label

        dgList.EditItemIndex = CInt(E.Item.ItemIndex)
        lblTrxID = dgList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("lblTrxID")
        strTrxID = lblTrxID.Text

        strParamName = "TRXID|STATUS|USERID"
        strParamValue = strTrxID & "|" & objAPTrx.EnumInvoiceRcvStatus.Deleted & "|" & strUserId
        
        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATEINV_DELETED&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try  

        dgList.EditItemIndex = -1
        BindGrid()
        BindGridSUM()
    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim UpdButton As LinkButton
        Dim HrgDispText As Label
        Dim EditHrgText As Label
        Dim EditHrg As TextBox

        HrgDispText = E.Item.FindControl("lblHargaFinalDisplay")
        HrgDispText.Visible = False
        EditHrgText = E.Item.FindControl("lblHargaFinal")
        EditHrg = E.Item.FindControl("lstHargaFinal")
        EditHrg.Text = EditHrgText.Text
        EditHrg.Visible = True
        EditHrg.Focus()

        UpdButton = dgList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Edit")
        UpdButton.Visible = False
        UpdButton = dgList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Update")
        UpdButton.Visible = True
        UpdButton = dgList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Cancel")
        UpdButton.Visible = True
        UpdButton = dgList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("Delete")
        UpdButton.Visible = False
    End Sub

    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCd As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_UPD_PRICE"
        Dim objItemDs As New Object()
        Dim intCnt As Integer = 0
        Dim intErrNo As Integer
        Dim intSelectedIndex As Integer = 0

        Dim EditLabel As Label
        Dim EditText As TextBox
        Dim strTrxID As String
        Dim strHargaFinal As String

        EditLabel = E.Item.FindControl("lblTrxID")
        strTrxID = EditLabel.Text
        EditText = E.Item.FindControl("lstHargaFinal")
        strHargaFinal = EditText.Text
        
        strParamName = "TRXID|HARGA|USERID"
        strParamValue = strTrxID & "|" & strHargaFinal & "|" & strUserId

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=ADD_DETAIL&errmesg=" & lblErrMessage.Text & "&redirect=IN/trx/IN_PurReq.aspx")
        End Try

        BindGrid()
        BindGridSUM()
    End Sub

    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        BindGrid()
        BindGridSUM()
    End Sub

    Sub BtnGenInv_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_GET_TICKET"
        Dim strOpCdPPN As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_GET_TICKET_PPNAMOUNT"

        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        Dim intErrNo As Integer
        Dim strAccMonth As String
        Dim strAccYear As String

        Dim strDateFrom As String = Date_Validation(srchDate.Text, False)
        Dim strDateTo As String = Date_Validation(srchDateTo.Text, False)

        strAccMonth = strSelAccMonth
        strAccYear = strSelAccYear

        strParamName = "LOCCODE|ACCMONTH|ACCYEAR|USERID|DATEFROM|DATETO"
        strParamValue = strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strUserId & "|" & strDateFrom & "|" & strDateTo

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATEINV_DELETED&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        'generate ppn pembelian tbs
        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCdPPN, strParamName, strParamValue)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_MthEnd_PPH21_Estate&errmesg=" & Exp.Message & "&redirect=")
        End Try

        BindGrid()
        BindGridSUM()
        CheckTBSPriceNotFound()

        'langsung generate tbs journal
        btnGenerate_Click(Sender, E)
    End Sub

    Protected Sub CheckTBSPriceNotFound()
        Dim strOpCd As String = "AP_CLSTRX_WEIGHBRIDGE_FFBPRICE_NOTFOUND"

        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        Dim intErrNo As Integer
        Dim strAccMonth As String
        Dim strAccYear As String
        Dim strDateFrom As String = Date_Validation(srchDate.Text, False)
        Dim strDateTo As String = Date_Validation(srchDateTo.Text, False)
        Dim dsResult As New Object

        strAccMonth = strSelAccMonth
        strAccYear = strSelAccYear

        strParamName = "LOCCODE|ACCMONTH|ACCYEAR|USERID|DATEFROM|DATETO"
        strParamValue = strLocation & "|" & strAccMonth & "|" & strAccYear & "|" & strUserId & "|" & strDateFrom & "|" & strDateTo

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                dsResult)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATEINV_DELETED&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        If dsResult.Tables(0).Rows.Count > 0 Then
            lblErrGenInv.Visible = True
            lblErrGenInv.Text = dsResult.Tables(0).Rows(0).Item("Msg")
        End If
    End Sub

    Sub NewBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_invrcv_wm_det.aspx")
    End Sub

    Sub BtnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strOpCd As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_CONFIRM"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        Dim intErrNo As Integer
        Dim strAccMonth As String
        Dim strAccYear As String
        Dim strTrxID As String
        Dim cblHrgFinal As Double = 0

        Dim btn As Button = CType(sender, Button)
        Dim dgList As DataGridItem = CType(btn.NamingContainer, DataGridItem)

        strTrxID = CType(dgList.Cells(10).FindControl("lblTrxID"), Label).Text
        'cblHrgFinal = CType(dgList.Cells(5).FindControl("lblHargaFinal"), Label).Text

        strAccMonth = strSelAccMonth
        strAccYear = strSelAccYear

        'If cblHrgFinal = 0 Then
        '    UserMsgBox(Me, "Confirm failed, Harga akhir have to greater than 0...!!!")
        '    Exit Sub
        'End If

        strParamName = "TRXID|STATUS|USERID|LOCCODE|ACCMONTH|ACCYEAR"
        strParamValue = strTrxID & "|" & objAPTrx.EnumInvoiceRcvStatus.Confirmed & _
                        "|" & strUserId & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATEINV_DELETED&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        BindGrid()
        BindGridSUM()
    End Sub

    Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim strOpCd As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_CONFIRM"
        Dim strParamName As String = ""
        Dim strParamValue As String = ""

        Dim intErrNo As Integer
        Dim strAccMonth As String
        Dim strAccYear As String
        Dim strTrxID As String

        Dim btn As Button = CType(sender, Button)
        Dim dgList As DataGridItem = CType(btn.NamingContainer, DataGridItem)

        strTrxID = CType(dgList.Cells(10).FindControl("lblTrxID"), Label).Text

        strAccMonth = strSelAccMonth
        strAccYear = strSelAccYear

        strParamName = "TRXID|STATUS|USERID|LOCCODE|ACCMONTH|ACCYEAR"
        strParamValue = strTrxID & "|" & objAPTrx.EnumInvoiceRcvStatus.Cancelled & _
                        "|" & strUserId & "|" & strLocation & "|" & strAccMonth & "|" & strAccYear

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GENERATEINV_DELETED&errmesg=" & lblErrMessage.Text & "&redirect=ap/trx/ap_trx_invrcv_wm_list")
        End Try

        BindGrid()
    End Sub

    Sub UserMsgBox(ByVal F As Object, ByVal sMsg As String)
        Dim sb As New StringBuilder()
        Dim oFormObject As System.Web.UI.Control = Nothing
        Try
            sMsg = sMsg.Replace("'", "\'")
            sMsg = sMsg.Replace(Chr(34), "\" & Chr(34))
            sMsg = sMsg.Replace(vbCrLf, "\n")
            sMsg = "<script language='javascript'>alert('" & sMsg & "');</script>"
            sb = New StringBuilder()
            sb.Append(sMsg)
            For Each oFormObject In F.Controls
                If TypeOf oFormObject Is HtmlForm Then
                    Exit For
                End If
            Next
            oFormObject.Controls.AddAt(oFormObject.Controls.Count, New LiteralControl(sb.ToString()))
        Catch ex As Exception

        End Try
    End Sub

    Function Date_Validation(ByVal pv_strInputDate As String, ByVal pv_blnIsShortDate As Boolean) As String
        Dim objSysCfgDs As New Object
        Dim objActualDate As New Object
        Dim strDateFormat As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strAcceptFormat As String

        strParam = "PWSYSTEM_CLSCONFIG_CONFIG_DATEFMT_GET"

        Try
            intErrNo = objSysCfg.mtdGetConfigInfo(strParam, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  objSysCfgDs)
        Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_DEPOSIT_GET_CONFIG&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_DepositList.aspx")
        End Try

        strDateFormat = objSysCfg.mtdGetDateFormat(objSysCfgDs.Tables(0).Rows(0).Item("Datefmt").Trim())

        If pv_blnIsShortDate Then
            Date_Validation = objGlobal.GetShortDate(strDateFormat, pv_strInputDate)
        Else
            If objGlobal.mtdValidInputDate(strDateFormat, _
                                           pv_strInputDate, _
                                           strAcceptFormat, _
                                           objActualDate) = True Then
                Date_Validation = objActualDate
            Else
                Date_Validation = ""
            End If
        End If
    End Function

    Sub btnPreview_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strTRXID As String

        strTRXID = Trim(hidSearch.Value)
        If strTRXID = "" Then
            Exit Sub
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/AP_Rpt_InvRcv_WM_List.aspx?Type=Print&CompName=" & strCompany & _
                        "&Location=" & strLocation & _
                        "&strSearch=" & strTRXID & _
                        "&strSearchDateFrom=" & srchDate.Text & _
                        "&strSearchDateTo=" & srchDateTo.Text & _
                        "&strRptInvoiceType=" & IIf(cbDetailByInvoice.Checked, "1", "0") & _
                        "&strRptType=" & IIf(cbExcelListRekap.Checked, "R", "D") & _
                        "&strExportToExcel=" & IIf(cbExcelList.Checked, "1", "0") & _
                        """,null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub btnPrintPrev_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Clear()
        Response.AddHeader("content-disposition", "attachment;filename=INVREKAP-" & Trim(strLocation) & ".xls")
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.xls"

        Dim stringWrite = New System.IO.StringWriter()
        Dim htmlWrite = New HtmlTextWriter(stringWrite)

        dgListSUM.RenderControl(htmlWrite)
        Response.Write(stringWrite.ToString())
        Response.End()
    End Sub

    Sub btnPrintPrevPay_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Clear()
        Response.AddHeader("content-disposition", "attachment;filename=INVREKAPPAY-" & Trim(strLocation) & ".xls")
        Response.Charset = ""
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.ContentType = "application/vnd.xls"

        Dim stringWrite = New System.IO.StringWriter()
        Dim htmlWrite = New HtmlTextWriter(stringWrite)

        dgListPay.RenderControl(htmlWrite)
        Response.Write(stringWrite.ToString())
        Response.End()
    End Sub

    Sub btnSaveSetting_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_DKtr As String = "WM_CLSTRX_TICKET_COASETTING_BUY_UPDATE"
        Dim objDataSet As New Object()
        Dim intErrNo As Integer
        Dim strMn As String
        Dim strYr As String

        'strMn = ddlMonth.SelectedItem.Value.Trim
        'strYr = ddlyear.SelectedItem.Value.Trim

        strParamName = "COATBSPEMILIK|COATBSAGEN|COAPPN|COAONGKOSBONGKAR|COAONGKOSLAPANGAN|COAPPH"
        strParamValue = Trim(ddlTBSPemilik.SelectedItem.Value) & "|" & Trim(ddlTBSAgen.SelectedItem.Value) & "|" & Trim(ddlPPN.SelectedItem.Value) & "|" & Trim(ddlOB.SelectedItem.Value) & "|" & Trim(ddlOL.SelectedItem.Value) & "|" & Trim(ddlPPH.SelectedItem.Value)

        Try
            intErrNo = objGLTrx.mtdInsertDataCommon(strOpCd_DKtr, strParamName, strParamValue)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_MthEnd_PPH21_Estate&errmesg=" & Exp.Message & "&redirect=")
        End Try

        LoadCOASetting()
    End Sub

    Protected Function LoadCOASetting() As DataSet
        Dim strOpCd_DKtr As String = "WM_CLSTRX_TICKET_COASETTING_BUY_GET"
        Dim objDataSet As New Object()
        Dim intErrNo As Integer
        Dim strMn As String
        Dim strYr As String

        'strMn = ddlMonth.SelectedItem.Value.Trim
        'strYr = ddlyear.SelectedItem.Value.Trim

        strParamName = "STRSEARCH"
        strParamValue = ""

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd_DKtr, strParamName, strParamValue, objTicketDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_MthEnd_PPH21_Estate&errmesg=" & Exp.Message & "&redirect=")
        End Try

        If objTicketDs.Tables(0).Rows.Count > 0 Then
            BindAccount(objTicketDs.Tables(0).Rows(0).Item("COATBSPemilik"), objTicketDs.Tables(0).Rows(0).Item("COATBSAgen"), objTicketDs.Tables(0).Rows(0).Item("COAPPN"), objTicketDs.Tables(0).Rows(0).Item("COAOngkosBongkar"), objTicketDs.Tables(0).Rows(0).Item("COAOngkosLapangan"), objTicketDs.Tables(0).Rows(0).Item("COAPPH"))
        Else
            BindAccount("", "", "", "", "", "")
        End If
    End Function

    Sub BindAccount(ByVal pv_strCOATBSPemilik As String, ByVal pv_strCOATBSAgen As String, ByVal pv_strCOAPPN As String, ByVal pv_strCOAOB As String, ByVal pv_strCOAOL As String, ByVal pv_strCOAPPH As String)
        Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim strParam As String = "Order By ACC.AccCode|And ACC.Status = '" & objGLSetup.EnumAccountCodeStatus.Active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim intSelectedIndexCOATBSPemilik As Integer = 0
        Dim intSelectedIndexCOATBSAgen As Integer = 0
        Dim intSelectedIndexCOAPPN As Integer = 0
        Dim intSelectedIndexCOAOB As Integer = 0
        Dim intSelectedIndexCOAOL As Integer = 0
        Dim intSelectedIndexCOAPPH As Integer = 0
        Dim objAccDs As New Object

        strParam = strParam & IIf(Session("SS_COACENTRALIZED") = "1", "", " AND LocCode = '" & Trim(strLocation) & "' ")

        Try
            intErrNo = objGLSetup.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLSetup.EnumGLMasterType.AccountCode, _
                                                   objAccDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_TRX_GR_ACCCODE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strCOATBSPemilik) Then
                intSelectedIndexCOATBSPemilik = intCnt + 1
                Exit For
            End If
        Next

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strCOATBSAgen) Then
                intSelectedIndexCOATBSAgen = intCnt + 1
                Exit For
            End If
        Next

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strCOAPPN) Then
                intSelectedIndexCOAPPN = intCnt + 1
                Exit For
            End If
        Next

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strCOAOB) Then
                intSelectedIndexCOAOB = intCnt + 1
                Exit For
            End If
        Next

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strCOAOL) Then
                intSelectedIndexCOAOL = intCnt + 1
                Exit For
            End If
        Next

        For intCnt = 0 To objAccDs.Tables(0).Rows.Count - 1
            If objAccDs.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strCOAPPH) Then
                intSelectedIndexCOAPPH = intCnt + 1
                Exit For
            End If
        Next

        dr = objAccDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("_Description") = "Please select account code"
        objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlTBSPemilik.DataSource = objAccDs.Tables(0)
        ddlTBSPemilik.DataValueField = "AccCode"
        ddlTBSPemilik.DataTextField = "_Description"
        ddlTBSPemilik.DataBind()
        ddlTBSPemilik.SelectedIndex = intSelectedIndexCOATBSPemilik

        ddlTBSAgen.DataSource = objAccDs.Tables(0)
        ddlTBSAgen.DataValueField = "AccCode"
        ddlTBSAgen.DataTextField = "_Description"
        ddlTBSAgen.DataBind()
        ddlTBSAgen.SelectedIndex = intSelectedIndexCOATBSAgen

        ddlPPN.DataSource = objAccDs.Tables(0)
        ddlPPN.DataValueField = "AccCode"
        ddlPPN.DataTextField = "_Description"
        ddlPPN.DataBind()
        ddlPPN.SelectedIndex = intSelectedIndexCOAPPN

        ddlOB.DataSource = objAccDs.Tables(0)
        ddlOB.DataValueField = "AccCode"
        ddlOB.DataTextField = "_Description"
        ddlOB.DataBind()
        ddlOB.SelectedIndex = intSelectedIndexCOAOB

        ddlOL.DataSource = objAccDs.Tables(0)
        ddlOL.DataValueField = "AccCode"
        ddlOL.DataTextField = "_Description"
        ddlOL.DataBind()
        ddlOL.SelectedIndex = intSelectedIndexCOAOL

        ddlPPH.DataSource = objAccDs.Tables(0)
        ddlPPH.DataValueField = "AccCode"
        ddlPPH.DataTextField = "_Description"
        ddlPPH.DataBind()
        ddlPPH.SelectedIndex = intSelectedIndexCOAPPH

    End Sub

    Sub btnGenerate_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_Jurnal As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_GENERATE_JOURNAL"
        Dim strOpCd_PPN As String = "AP_CLSTRX_WEIGHBRIDGE_INVOICE_GENERATE_PPNAMOUNT"
        Dim objDataSet As New Object()
        Dim intErrNo As Integer
        Dim strMn As String
        Dim strYr As String
        Dim BegDate As String
        Dim EndDate As String

        BegDate = "1/" & Month(Date_Validation(srchDateTo.Text, False)) & "/" & Year(Date_Validation(srchDateTo.Text, False))
        EndDate = DateAdd(DateInterval.Month, 1, CDate(Month(Date_Validation(srchDateTo.Text, False)) & "/1/" & Year(Date_Validation(srchDateTo.Text, False))))
        EndDate = objGlobal.GetShortDate(Session("SS_DATEFMT"), DateAdd(DateInterval.Day, -1, CDate(EndDate)))

        If Month(Date_Validation(srchDate.Text, False)) <> Month(Date_Validation(srchDateTo.Text, False)) Then
            lblErrGenerate.Visible = True
            lblErrGenerate.Text = "Only 1 period to generate."
            Exit Sub
        ElseIf Day(Date_Validation(srchDate.Text, False)) <> 1 Then
            lblErrGenerate.Visible = True
            lblErrGenerate.Text = "Please put day 1 as beginning."
            Exit Sub
        ElseIf Day(Date_Validation(srchDateTo.Text, False)) <> Day(Date_Validation(EndDate, False)) Then
            lblErrGenerate.Visible = True
            lblErrGenerate.Text = "Please put end of day of this period."
            Exit Sub
        End If

        strMn = Month(Date_Validation(srchDate.Text, False))
        strYr = Year(Date_Validation(srchDate.Text, False))

        strParamName = "LOCCODE|ACCMONTH|ACCYEAR|UPDATEID"
        strParamValue = strLocation & "|" & strMn & "|" & strYr & "|" & strUserId

        'generate jurnal pembelian tbs
        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCd_Jurnal, strParamName, strParamValue, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_MthEnd_PPH21_Estate&errmesg=" & Exp.Message & "&redirect=")
        End Try

        If objDataSet.Tables(0).Rows.Count > 0 Then
            lblErrGenerate.Visible = True
            lblErrGenerate.Text = objDataSet.Tables(0).Rows(0).Item("Msg")
        End If

        'generate ppn pembelian tbs
        'Try
        '    intErrNo = objGLTrx.mtdGetDataCommon(strOpCd_PPN, strParamName, strParamValue, objDataSet)
        'Catch Exp As System.Exception
        '    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PR_MthEnd_PPH21_Estate&errmesg=" & Exp.Message & "&redirect=")
        'End Try

        'If objDataSet.Tables(0).Rows.Count > 0 Then
        '    lblErrGenerate.Visible = True
        '    lblErrGenerate.Text = lblErrGenerate.Text & "<br>" & objDataSet.Tables(0).Rows(0).Item("Msg")
        'End If

        'LoadData()
        'LoadDataPPH()
    End Sub
End Class
