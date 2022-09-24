

Imports System
Imports System.Data
Imports System.Collections 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction


Public Class ap_trx_InvRcvList : Inherits Page

    Protected WithEvents dgInvList As DataGrid
    Protected WithEvents lblTracker As Label
    Protected WithEvents lstDropList As DropDownList
    Protected WithEvents ddlStatus As DropDownList
    Protected WithEvents txtInvoiceID As TextBox
    Protected WithEvents txtSupplier As TextBox
    Protected WithEvents txtLastUpdate As TextBox
    Protected WithEvents ddlInvoiceType As DropDownList
    Protected WithEvents SortExpression As Label
    Protected WithEvents SortCol As Label
    Protected WithEvents lblErrMesage As Label

    Protected WithEvents lblTitle As Label
    Protected WithEvents lblInvReceiveID As Label
    Protected WithEvents lblID as Label
    Protected WithEvents lstAccMonth As DropDownList
    Protected WithEvents lstAccYear As DropDownList
    Protected WithEvents ConfirmBtn As ImageButton
    Protected WithEvents txtPOID As TextBox

    Protected WithEvents NewInvRcv As ImageButton
    Protected WithEvents ibPrint As ImageButton

    Protected objAPTrx As New agri.AP.clsTrx()
    Protected objPUTrx As New agri.PU.clsTrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()   
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objGLtrx As New agri.GL.ClsTrx()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intAPAR As Integer
    Dim objInvRcvDs As New Object() 
    Dim objLangCapDs As New Object()
    Dim strLocType as String
    Dim strSelAccMonth As String
    Dim strSelAccYear As String
    Dim intLevel As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strAccMonth = Session("SS_APACCMONTH")
        strAccYear = Session("SS_APACCYEAR")
        strSelAccMonth = Session("SS_SELACCMONTH")
        strSelAccYear = Session("SS_SELACCYEAR")
        strLangCode = Session("SS_LANGCODE")
        intAPAR = Session("SS_APAR")
        strLocType = Session("SS_LOCTYPE")
        intLevel = Session("SS_USRLEVEL")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumAPAccessRights.APInvoiceReceive), intAPAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            'to avoid double click, on aspx add this : UseSubmitBehavior="false"
            NewInvRcv.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(NewInvRcv).ToString())
            ibPrint.Attributes.Add("onclick", "this.disabled=true;" + GetPostBackEventReference(ibPrint).ToString())

            If SortExpression.Text = "" Then
                SortExpression.Text = "INV.InvoiceRcvID"
            End If

            If Not Page.IsPostBack Then
                If Session("SS_FILTERPERIOD") = "0" Then
                    lstAccMonth.SelectedValue = strAccMonth
                    BindAccYear(strAccYear)
                Else
                    lstAccMonth.SelectedValue = strSelAccMonth
                    BindAccYear(strSelAccYear)
                End If

                onload_GetLangCap()
                BindInvoiceTypeList()
                BindStatusList()
                BindGrid()
                BindPageList()
            End If

            ConfirmBtn.Attributes("onclick") = "javascript:return ConfirmAction('proceed this period posting');"
        End If
    End Sub

    Sub srchBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        dgInvList.CurrentPageIndex = 0
        dgInvList.EditItemIndex = -1
        BindGrid()
        BindPageList()
    End Sub

    Sub BindInvoiceTypeList()
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.All), objAPTrx.EnumInvoiceType.All))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.SupplierPO), objAPTrx.EnumInvoiceType.SupplierPO))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.AdvancePayment), objAPTrx.EnumInvoiceType.AdvancePayment))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.Others), objAPTrx.EnumInvoiceType.Others))
        ddlInvoiceType.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceType(objAPTrx.EnumInvoiceType.FFBSupplier), objAPTrx.EnumInvoiceType.FFBSupplier))
        ddlStatus.SelectedIndex = 0
    End Sub

    Sub BindStatusList()
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.All), objAPTrx.EnumInvoiceRcvStatus.All))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Active), objAPTrx.EnumInvoiceRcvStatus.Active))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Cancelled), objAPTrx.EnumInvoiceRcvStatus.Cancelled))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Closed), objAPTrx.EnumInvoiceRcvStatus.Closed))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Confirmed), objAPTrx.EnumInvoiceRcvStatus.Confirmed))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Deleted), objAPTrx.EnumInvoiceRcvStatus.Deleted))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Paid), objAPTrx.EnumInvoiceRcvStatus.Paid))
        ddlStatus.Items.Add(New ListItem(objAPTrx.mtdGetInvoiceRcvStatus(objAPTrx.EnumInvoiceRcvStatus.Writeoff), objAPTrx.EnumInvoiceRcvStatus.Writeoff))
        If intLevel = 0 Then
            ddlStatus.SelectedIndex = 1
        Else
            ddlStatus.SelectedIndex = 0
        End If
    End Sub

    Sub BindGrid()
        Dim strOpCode_GetInvRcvList As String = "AP_CLSTRX_INVOICERECEIVE_LIST_GET" 
        Dim PageNo As Integer
        Dim PageCount As Integer
        Dim strSrchInvID As String
        Dim strSrchSuppCode As String
        Dim strSrchInvoiceType As String
        Dim strSrchStatus As String
        Dim strSrchLastUpdate As String
        Dim strSearch As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbButton As LinkButton
        Dim strPOID As String

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value
        strSrchInvID = IIf(txtInvoiceID.Text = "", "", txtInvoiceID.Text)
        strSrchSuppCode = IIf(txtSupplier.Text = "", "", txtSupplier.Text)
        strSrchInvoiceType = IIf(ddlInvoiceType.SelectedIndex = 0, "", ddlInvoiceType.SelectedItem.Value)
        strSrchStatus = IIf(ddlStatus.SelectedItem.Value = "0", "", ddlStatus.SelectedItem.Value)
        strSrchLastUpdate = IIf(txtLastUpdate.Text = "", "", txtLastUpdate.Text)
        strPOID = IIf(txtPOID.Text = "", "", txtPOID.Text)
        strParam = strSrchInvID & "|" & _
                   strSrchSuppCode & "|" & _
                   strSrchStatus & "|" & _
                   strSrchLastUpdate & "|" & _
                   SortExpression.Text & "|" & _
                   SortCol.Text & "|" & _
                   strSrchInvoiceType & "|" & _
                   strPOID

        Try
            intErrNo = objAPTrx.mtdGetInvoiceRcv(strOpCode_GetInvRcvList, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 strParam, _
                                                 objInvRcvDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=INVRCVLIST_GET_LIST&errmesg=" & lblErrMesage.Text & "&redirect=AP/trx/ap_trx_InvRcvList.aspx")
        End Try

        PageCount = objGlobal.mtdGetPageCount(objInvRcvDs.Tables(0).Rows.Count, dgInvList.PageSize)
        dgInvList.DataSource = objInvRcvDs
        If dgInvList.CurrentPageIndex >= PageCount Then
            If PageCount = 0 Then
                dgInvList.CurrentPageIndex = 0
            Else
                dgInvList.CurrentPageIndex = PageCount - 1
            End If
        End If
        dgInvList.DataBind()
        BindPageList()


        PageNo = dgInvList.CurrentPageIndex + 1
        lblTracker.Text = "Page " & PageNo & " of " & dgInvList.PageCount
    End Sub

    Sub BindPageList()
        Dim count As Integer = 1
        Dim arrDList As New ArrayList()

        While Not count = dgInvList.PageCount + 1
            arrDList.Add("Page " & count)
            count = count + 1
        End While
        lstDropList.DataSource = arrDList
        lstDropList.DataBind()
        lstDropList.SelectedIndex = dgInvList.CurrentPageIndex
    End Sub


    Sub Update_Status(ByVal pv_strInvoiceRcvId As String, _
                      ByVal pv_intInvoiceRcvSts As Integer, _
                      ByVal pv_strPOID As String, _
                      ByVal pv_intPOSts As Integer)

        Dim strOpCode_InvoiceRcv As String = "AP_CLSTRX_INVOICERECEIVE_STATUS_UPD"
        Dim objPOId As New Object()
        Dim strOpCd_AddPO As String = "PU_CLSTRX_PO_ADD"
        Dim strOpCd_UpdPo As String = "PU_CLSTRX_PO_UPD"
        Dim strOppCd As String = "PU_CLSTRX_PO_MOVEID"
        Dim strOppCd_Back As String = "PU_CLSTRX_PO_BACKID"
        Dim strPOID As String = pv_strPOID
        Dim strParam As String = pv_strInvoiceRcvId & "|" & pv_intInvoiceRcvSts
        Dim intCnt As Integer
        Dim intErrNo As Integer

        Try
            intErrNo = objAPTrx.mtdUpdInvoiceRcvStatus(strOpCode_InvoiceRcv, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=INVRCVLIST_UPD_STATUS&errmesg=" & lblErrMesage.Text & "&redirect=AP/trx/ap_trx_InvRcvList.aspx")
        End Try

        'Try

        '    strParam = pv_strPOID & "||||" & pv_intPOSts & "||" & strAccMonth & "|" & strAccYear & "||||||||||||"
        '    intErrNo = objPUTrx.mtdUpdPO(strOpCd_AddPO, _
        '                                 strOpCd_UpdPo, _
        '                                 strOppCd, _
        '                                 strOppCd_Back, _
        '                                 strCompany, _
        '                                 strLocation, _
        '                                 strUserId, _
        '                                 strParam, _
        '                                 objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.PurchaseOrder), _
        '                                 objPOId)
        'Catch Exp As System.Exception
        '    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=INVRCVLIST_UPD_POSTATUS&errmesg=" & lblErrMesage.Text & "&redirect=AP/trx/ap_trx_InvRcvList.aspx")
        'End Try

    End Sub



    Sub btnPrevNext_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim direction As String = CType(sender, ImageButton).CommandArgument
        Select Case direction
            Case "first"
                dgInvList.CurrentPageIndex = 0
            Case "prev"
                dgInvList.CurrentPageIndex = _
                Math.Max(0, dgInvList.CurrentPageIndex - 1)
            Case "next"
                dgInvList.CurrentPageIndex = _
                Math.Min(dgInvList.PageCount - 1, dgInvList.CurrentPageIndex + 1)
            Case "last"
                dgInvList.CurrentPageIndex = dgInvList.PageCount - 1
        End Select
        lstDropList.SelectedIndex = dgInvList.CurrentPageIndex
        BindGrid()
    End Sub

    Sub PagingIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsPostBack Then
            dgInvList.CurrentPageIndex = lstDropList.SelectedIndex
            BindGrid()
        End If
    End Sub

    Sub OnPageChanged(ByVal sender As Object, ByVal e As DataGridPageChangedEventArgs)
        dgInvList.CurrentPageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Sub Sort_Grid(ByVal sender As Object, ByVal e As DataGridSortCommandEventArgs)
        SortExpression.Text = e.SortExpression.ToString()
        SortCol.Text = IIf(SortCol.Text = "ASC", "DESC", "ASC")
        dgInvList.CurrentPageIndex = lstDropList.SelectedIndex
        BindGrid()
    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim lblDelText As Label
        Dim strInvoiceRcvId As String
        Dim strPOID As String

        dgInvList.EditItemIndex = CInt(E.Item.ItemIndex)
        lblDelText = dgInvList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idInvoiceRcvId")
        strInvoiceRcvId = lblDelText.Text
        lblDelText = dgInvList.Items.Item(CInt(E.Item.ItemIndex)).FindControl("idPOID")
        strPOID = lblDelText.Text

        Update_Status(strInvoiceRcvId, objAPTrx.EnumInvoiceRcvStatus.Deleted, strPOID, objPUTrx.EnumPOStatus.Confirmed)

        BindGrid()
        BindPageList()
    End Sub


    Sub NewInvRcv_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("ap_trx_InvRcvDet.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.Text = UCase(GetCaption(objLangCap.EnumLangCap.InvoiceReceive))
        lblInvReceiveID.Text = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & lblID.Text
        dgInvList.Columns(0).HeaderText = GetCaption(objLangCap.EnumLangCap.InvoiceReceive) & lblID.Text
    End Sub

    Sub GetEntireLangCap()
        Dim strOpCode_BussTerm As String = "PWSYSTEM_CLSLANGCAP_BUSSTERM_GET"
        Dim strParam As String
        Dim intErrNo As Integer

        strParam = strLangCode
        Try
            intErrNo = objLangCap.mtdGetBussTerm(strOpCode_BussTerm, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strAccMonth, _
                                                 strAccYear, _
                                                 objLangCapDs, _
                                                 strParam)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=AP_TRX_INVRCV_LANGCAP&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
            Dim count As Integer

            For count = 0 To objLangCapDs.Tables(0).Rows.Count - 1
                If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode")) Then
                    If strLocType = objAdminLoc.EnumLocType.Mill then
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                    else
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                    end if
                    Exit For
                End If
            Next
        End Function

    Sub BindAccYear(ByVal pv_strAccYear As String)
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer
        Dim intCnt As Integer = 0
        Dim intSelectedIndex As Integer
        Dim objAccYearDs As New Object
        Dim dr As DataRow
        Dim strOpCd As String = "ADMIN_CLSACCPERIOD_CONFIG_GET"

        strParamName = "LOCCODE|SEARCHSTR|SORTEXP"
        strParamValue = strLocation & "||Order By HD.AccYear"

        Try
            intErrNo = objGLtrx.mtdGetDataCommon(strOpCd, _
                                                strParamName, _
                                                strParamValue, _
                                                objAccYearDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCYEAR&errmesg=" & lblErrMesage.Text & "&redirect=")
        End Try

        For intCnt = 0 To objAccYearDs.Tables(0).Rows.Count - 1
            objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear") = Trim(objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear"))
            objAccYearDs.Tables(0).Rows(intCnt).Item("UserName") = Trim(objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear"))
            If objAccYearDs.Tables(0).Rows(intCnt).Item("AccYear") = pv_strAccYear Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        lstAccYear.DataSource = objAccYearDs.Tables(0)
        lstAccYear.DataValueField = "AccYear"
        lstAccYear.DataTextField = "UserName"
        lstAccYear.DataBind()
        lstAccYear.SelectedIndex = intSelectedIndex - 1
    End Sub

    Sub ConfirmBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd As String = "AP_CLSTRX_INVOICERECEIVE_PERIOD_POSTING"
        Dim strParamName As String
        Dim strParamValue As String
        Dim intErrNo As Integer

        If lstAccMonth.SelectedItem.Value = "0" Then
            strAccMonth = "1','2','3','4','5','6','7','8','9','10','11','12"
        Else
            strAccMonth = lstAccMonth.SelectedItem.Value
        End If

        strAccYear = lstAccYear.SelectedItem.Value

        strParamName = "ACCMONTH|ACCYEAR|LOCCODE"
        strParamValue = strAccMonth & "|" & strAccYear & "|" & strLocation

        Try
            intErrNo = objGLtrx.mtdInsertDataCommon(strOpCd, _
                                                    strParamName, _
                                                    strParamValue)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PERIOD POSTING&errmesg=" & Exp.ToString() & "&redirect=pu/trx/AP_trx_InvoiceRcvList")
        End Try

        BindGrid()
    End Sub
End Class
