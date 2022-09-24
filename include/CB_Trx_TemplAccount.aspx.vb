
Imports System
Imports System.Data

Imports agri.PWSystem.clsLangCap

Public Class cb_trx_TemplAccount : Inherits Page

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblErrDup As Label
    Protected WithEvents dgLineDet As DataGrid
    Protected WithEvents lbDelete As LinkButton
    Protected WithEvents validateCode As RequiredFieldValidator
    Protected WithEvents validateDesc As RequiredFieldValidator

    Protected WithEvents lblTitle As Label
    Protected WithEvents lblRowId As Label
    Protected WithEvents lblRowDesc As Label
    Protected WithEvents lblStmtType As Label
    Protected WithEvents ddlAccGrp As Dropdownlist
    Protected WithEvents ddlAccount As Dropdownlist
    Protected WithEvents tblSelection As HtmlTable
    Protected WithEvents lblAccGrp As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblPlsSelectEither As Label
    Protected WithEvents lblPlsSelect As Label
    Protected WithEvents lblOr As Label
    Protected WithEvents lblOnly As Label
    Protected WithEvents lblErrSelectBoth As Label
    Protected WithEvents lblErrNotSelect As Label
    Protected WithEvents lblCode As Label

    Dim objAccGrpDs As New Object()
    Dim objAccDs As New Object()
    Dim objLineDs As New Object()
    Dim objCBTrx As New agri.CB.clsTrx()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objLangCapDs As New Object()
    
    Dim strLocType as String
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Dim strOpCdGetAcc As String = "CB_CLSTRX_TEMPLACC_GETACCLIST"
    Dim strOpCdGetAccGrp As String = "CB_CLSTRX_CHARTOFACCGRP_LIST_SEARCH"
    Dim strOpCdGetLine As String = "CB_CLSTRX_TEMPLACC_GET"
    Dim strOpCdDelLine As String = "CB_CLSTRX_TEMPLACC_DEL"
    Dim strOpCdAddLine As String = "CB_CLSTRX_TEMPLACC_ADD"
    Dim strOpCdUpdTempl As String = "CB_CLSTRX_TEMPLATE_UPDATETRACK"
    Dim strOpCdGetAccByGrp As String = "CB_CLSSETUP_CHARTOFACCOUNT_LIST_GET"

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCBAR As Integer

    Dim strRowId As String
    Dim strRowDesc As String
    Dim strStmtType As String

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        intCBAR = Session("SS_CBAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashFlow), intCBAR) = False  Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            lblErrSelectBoth.visible = false
            lblErrNotSelect.visible = false
            onload_GetLangCap()

            strRowId = IIf(Trim(Request.QueryString("rowid")) <> "", Trim(Request.QueryString("rowid")), Trim(Request.Form("lblRowId")))
            strRowDesc = IIf(Trim(Request.QueryString("strDesc")) <> "", Trim(Request.QueryString("strDesc")), Trim(Request.Form("lblRowDesc")))
            strStmtType = IIf(Trim(Request.QueryString("stmttype")) <> "", Trim(Request.QueryString("stmttype")), Trim(Request.Form("lblStmtType")))
            If Not IsPostBack Then
                BindAccount()
                BindAccGrpCode()
                onLoad_LineDisplay()
            End If
        End If
    End Sub

    Sub onLoad_LineDisplay()
        Dim strParam As String
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim lbButton As LinkButton

        lblRowId.text = strRowId
        lblRowDesc.text = strRowDesc
        lblStmtType.text = strStmtType

        strParam = "order by tpl.AccCode|" & _
                   "and tpl.RowId = '" & strRowId & _
                   "' and acc.Status = '" & objGLSetup.EnumAccStatus.Active & "' |"

        Try
            intErrNo = objCBTrx.mtdGetTemplAcc(strOpCdGetLine, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strParam, _
                                                 objLineDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_TEMPLACC_GETLINE&errmesg=" & lblErrMessage.Text & "&redirect=CB/TRX/cb_trx_TemplAccount.aspx")
        End Try

        dgLineDet.DataSource = objLineDs.Tables(0)
        dgLineDet.DataBind()
        
        For intCnt = 0 To dgLineDet.Items.Count - 1
            lbButton = dgLineDet.Items.Item(intCnt).FindControl("lbDelete")
            lbButton.Visible = True
            lbButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        Next
    End Sub


    Sub BindAccount()
        Dim strParam As String      
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim dr As DataRow
        Dim strAccType As String
        
        strAccType = Convert.ToString(objGLSetup.EnumAccountType.BalanceSheet)

        strParam = "order by acc.AccCode|" & _
                    "and acc.status = '" & objGLSetup.EnumAccStatus.Active & "'  and acc.AccType = '" & strAccType & "' |" & _
                    "and tpl.RowId = '" & strRowId & "' "

        Try
            intErrNo = objCBTrx.mtdGetTemplAcc(strOpCdGetAcc, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strParam, _
                                                 objAccDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_TEMPLACC_GETACCLIST&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_TemplAccount.aspx")
        End Try
        
        dr = objAccDs.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("_Description") = "Select One"
        objAccDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccount.DataSource = objAccDs.Tables(0)
        ddlAccount.DataValueField = "AccCode"
        ddlAccount.DataTextField = "_Description"
        ddlAccount.DataBind()
    End Sub

    Sub BindAccGrpCode()
        Dim strParam As String 
        Dim intErrNo As Integer
        Dim dr As DataRow
        Dim intCnt As Integer

        strParam = "order by acc.AccGrpCode|" & _
                        "and acc.status = '" & objGLSetup.EnumAccGrpStatus.Active & "' " & _
                        "and accgrpcode in (select distinct accgrpcode from gl_account where status = '" & objGLSetup.EnumAccStatus.Active & "' and acctype = '" & objGLSetup.EnumAccountType.BalanceSheet & "')"
      

        Try
            intErrNo = objCBTrx.mtdGetMasterList(strOpCdGetAccGrp, _
                                                    strParam, 0, _
                                                    objAccGrpDs)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_TEMPLACC_GETACCLIST&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try
        
        For intCnt = 0 To objAccGrpDs.Tables(0).Rows.Count - 1
            objAccGrpDs.Tables(0).Rows(intCnt).Item("AccGrpCode") = Trim(objAccGrpDs.Tables(0).Rows(intCnt).Item("AccGrpCode"))
            objAccGrpDs.Tables(0).Rows(intCnt).Item("Description") = objAccGrpDs.Tables(0).Rows(intCnt).Item("AccGrpCode") & " (" & Trim(objAccGrpDs.Tables(0).Rows(intCnt).Item("Description")) & ")"
        Next

        dr = objAccGrpDs.Tables(0).NewRow()
        dr("AccGrpCode") = ""
        dr("Description") = "Select One"
        objAccGrpDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlAccGrp.DataSource = objAccGrpDs.Tables(0)
        ddlAccGrp.DataValueField = "AccGrpCode"
        ddlAccGrp.DataTextField = "Description"
        ddlAccGrp.DataBind()
    End Sub

    Sub btnAdd_Click(Sender As Object, E As ImageClickEventArgs)
        Dim strParam As String
        Dim strAccCode As String
        Dim strAccGrpCode As String
        Dim intErrNo As Integer

        Try
            strAccCode = ddlAccount.SelectedItem.Value
        Catch Exp As System.Exception
            Exit Sub
        End Try

        Try
            strAccGrpCode = ddlAccGrp.SelectedItem.Value
        Catch Exp As System.Exception
            Exit Sub
        End Try

        If strAccGrpCode = "" And strAccCode = "" Then
            lblErrNotSelect.Visible = True
            Exit Sub
        End If

        If strAccGrpCode <> "" And strAccCode <> "" Then
            lblErrSelectBoth.Visible = True
            Exit Sub
        End If
        
        strParam = lblRowId.text & Chr(9) & strAccCode & Chr(9) & strAccGrpCode

        Try
            intErrNo = objCBTrx.mtdUpdTemplAcc(strOpCdUpdTempl, _
                                                 strOpCdAddLine, _
                                                 strOpCdGetAccByGrp, _
                                                 strOpCdDelLine, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strParam, False) 
        Catch Exp As System.Exception
        Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_TEMPLACC_ADD&errmesg=" & lblErrMessage.Text & "&redirect=CB/TRX/cb_trx_TemplAccount.aspx")
        End Try
       
        onLoad_LineDisplay()
        BindAccGrpCode()
        BindAccount()

    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strParam As String
        Dim strAccCode As String
        Dim strAccGrpCode As String = ""
        Dim intErrNo As Integer
        Dim lbl As Label

        lbl = E.Item.FindControl("lblAccCode")
        strAccCode = lbl.text.trim


        strParam = lblRowId.text & Chr(9) & strAccCode  & Chr(9) & strAccGrpCode

        Try
            intErrNo = objCBTrx.mtdUpdTemplAcc(strOpCdUpdTempl, _
                                                 strOpCdAddLine, _
                                                 strOpCdGetAccByGrp, _
                                                 strOpCdDelLine, _
                                                 strCompany, _
                                                 strLocation, _
                                                 strUserId, _
                                                 strParam, True) 
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_TEMPLACC_DEL&errmesg=" & lblErrMessage.Text & "&redirect=gl/setup/cb_trx_TemplAccount.aspx")
        End Try

        onLoad_LineDisplay()
        BindAccGrpCode()
        BindAccount()
    End Sub

    Sub CloseWindow_Click(Sender As Object, E As ImageClickEventArgs)
    End Sub


    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblTitle.text = UCase(GetCaption(objLangCap.EnumLangCap.Account))
        lblAccGrp.text = GetCaption(objLangCap.EnumLangCap.AccGrp)
        lblAccount.text = GetCaption(objLangCap.EnumLangCap.Account)

        lblErrSelectBoth.text = lblPlsSelectEither.text & lblAccGrp.text & lblOr.text & lblAccount.text & lblOnly.text
        lblErrNotSelect.text = lblPlsSelect.text & lblAccGrp.text & lblOr.text & lblAccount.text & "."

        dgLineDet.Columns(0).HeaderText = lblAccount.text & lblCode.text
        dgLineDet.Columns(1).HeaderText = GetCaption(objLangCap.EnumLangCap.AccDesc)

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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=PU_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=gl/setup/GL_setup_TemplAccount.aspx")
        End Try
    End Sub

    Function GetCaption(ByVal pv_TermCode) As String
        Dim count As Integer
        
        For count=0 To objLangCapDs.Tables(0).Rows.Count - 1
            If Trim(pv_TermCode) = Trim(objLangCapDs.Tables(0).Rows(count).Item("TermCode"))
                 If strLocType = objAdminLoc.EnumLocType.Mill then
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                 else
                        Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                 end if

                Exit For
            End If
        Next
    End Function



End Class
