
Imports System
Imports System.Data
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic

Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.PWSystem.clsLangCap
Imports agri.PWSystem.clsConfig

Public Class cb_trx_TreasuryTemplate : Inherits Page

    Protected WithEvents TmplList As DataGrid
    Protected WithEvents ErrorMessage As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblOper As Label
    Protected WithEvents lblDupDispSeq As Label
    Protected WithEvents lblAccount As Label
    Protected WithEvents lblErrCalc As Label
    Protected WithEvents txtDescription As TextBox
    Protected WithEvents txtCode As TextBox
    Protected WithEvents txtName As TextBox
    Protected WithEvents chkBegBalance As CheckBox

    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DeleteBtn As ImageButton
    Protected WithEvents BackBtn As ImageButton
    Protected WithEvents ibNew As ImageButton

    Dim strLocType as String
    Dim objAdminLoc As New agri.Admin.clsLoc()

    Protected objCBTrx As New agri.CB.clsTrx()

    Dim objSysConfig As New agri.PWSystem.clsConfig()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap() 

    Dim strOpCd_Get As String = "CB_TRX_TREASURYTEMPLDTL_GET"
    Dim strOpCd_Add As String = "CB_TRX_TREASURYTEMPLDTL_ADD"
    Dim strOpCd_Upd As String = "CB_TRX_TREASURYTEMPLDTL_UPD"
    Dim strOpCd_Del As String = "CB_TRX_TREASURYTEMPLDTL_DEL"
    Dim strOpCdDelAcc As String = "CB_TRX_TREASURYTEMPL_ACC_DEL"
    
    Dim strOpCd_Config As String = "PWSYSTEM_CLSCONFIG_CONFIG_GET" 
 
    Dim objLangCapDs As New Object
    Dim objDataSet As New DataSet()
    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCBAR As Integer

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_CBACCMONTH")
        strAccYear = Session("SS_CBACCYEAR")
        intCBAR = Session("SS_CBAR")
        
        strLocType = Session("SS_LOCTYPE")

        lblErrCalc.visible = false
        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCBAccessRights.CBCashFlow), intCBAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
    
             If Not Page.IsPostBack Then
                If Not Request.QueryString("ReportCode") = "" Then
                    txtCode.Text = Request.QueryString("ReportCode")
                    ViewState.Item("ReportCode") = Request.QueryString("ReportCode")
                Else
                    ibNew.Visible = False
                End If

                If Not txtCode.Text = "" Then
                    DisplayData()
                    BindGrid()
                End If
            End If

        End IF

    End Sub

    Sub BindGrid()
        TmplList.DataSource = LoadDtlData()
        TmplList.DataBind()
    End Sub

    Sub DataGrid_ItemCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Dim btn As Button
       
        Select Case e.Item.ItemType
           Case ListItemType.Item, ListItemType.AlternatingItem
                lbl = e.Item.FindControl("lblNo")
                lbl.text = e.Item.ItemIndex.ToString + 1

                lbl = e.Item.FindControl("lblRowType")
                If lbl.text.trim = objCBTrx.EnumRowType.Entry then
                    btn = e.item.FindControl("btnAccount")
                    btn.visible = True
                End If
        End Select
    End Sub

    Sub BindRowTypeList(ByRef DropList As DropDownList, Optional ByVal itemtype As String = "")
        DropList.Items.Add(New ListItem(objCBTrx.mtdGetRowType(objCBTrx.EnumRowType.Entry), objCBTrx.EnumRowType.Entry))
        DropList.Items.Add(New ListItem(objCBTrx.mtdGetRowType(objCBTrx.EnumRowType.Header), objCBTrx.EnumRowType.Header))
        DropList.Items.Add(New ListItem(objCBTrx.mtdGetRowType(objCBTrx.EnumRowType.Formula), objCBTrx.EnumRowType.Formula))
        Select Case itemtype.Trim
            Case objCBTrx.EnumRowType.Entry
                DropList.SelectedIndex = 0
            Case objCBTrx.EnumRowType.Header
                DropList.SelectedIndex = 1
            Case objCBTrx.EnumRowType.Formula
                DropList.SelectedIndex = 2
        End Select
    End Sub

    Protected Function LoadHdrData() As DataSet
        Dim strOpCode As String = "CB_CLSTRX_TREASURYTEMPLATE_LIST_GET"
        strParam = Replace(txtCode.Text, "'", "''")   & "|" 
        Try
             intErrNo = objCBTrx.mtdGetTreasuryTemplateList(strOpCode, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              objDataSet)
            
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=CB_TRX_TREASURYTMPL_GET&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_TreasuryTemplate.aspx")
        End Try
        Return objDataSet
    End Function

    Protected Function LoadDtlData() As DataSet
        Dim strOpCode As String = "CB_TRX_TREASURYTEMPLDTL_GET"
        strParam = txtCode.Text & "|ORDER BY DISPSEQ"    
        Try
             intErrNo = objCBTrx.mtdGetTreasuryTemplateDtl(strOpCode, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam, _
                                              objDataSet)
            
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=CB_TRX_TREASURYTEMPLDTL_GET&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_TreasuryTemplate.aspx")
        End Try
        Return objDataSet
    End Function

    Sub DisplayData()
        Dim dsTx As DataSet = LoadHdrData()
        If dsTx.Tables(0).Rows.Count > 0 Then
            txtCode.Enabled = False
            txtName.Text = Trim(dsTx.Tables(0).Rows(0).Item("Name"))
            txtDescription.Text = Trim(dsTx.Tables(0).Rows(0).Item("Description"))
            ibNew.Visible = True
        End If
    End Sub


    Sub ddlCheckType(ByVal Sender As Object, ByVal E As EventArgs)
        Dim Droplist As DropDownList
        Dim txt As TextBox
        Dim btn As Button
        Dim lbl As Label
        Dim chk As CheckBox
        Dim rfv As RequiredFieldValidator

        Droplist = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("ddlRowType")
        If Droplist.SelectedItem.Value.Trim = objCBTrx.EnumRowType.Entry Then
            btn = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("btnEditAccount")
            btn.visible = False
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtRefNo")
            txt.visible = True
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtFormula")
            txt.visible = False
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvRefNo")
            rfv.Visible = True
            chk = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("chkBegBalance")
            chk.Visible = True
        ElseIf Droplist.SelectedItem.Value.Trim = objCBTrx.EnumRowType.Header Then
            btn = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("btnEditAccount")
            btn.visible = False
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtRefNo")
            txt.visible = False
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtFormula")
            txt.visible = False 
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvRefNo")
            rfv.Visible = False
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvFormula")
            rfv.Visible = False     
            chk = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("chkBegBalance")
            chk.Visible = False
        Else 
            btn = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("btnEditAccount")
            btn.visible = False
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtRefNo")
            txt.visible = True
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtFormula")
            txt.visible = True
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvRefNo")
            rfv.Visible = True
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvFormula")
            rfv.Visible = True
            chk = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("chkBegBalance")
            chk.Visible = False
       End If
    End Sub


    Sub onClick_Account(ByVal Sender As Object, ByVal E As EventArgs)
        Dim lbl As Label
        Dim txt As Textbox
        Dim strRowId As String
        Dim strDesc As String
        Dim strID As String

        strID = CType(sender,Button).ID
        If strID = "btnAccount" Then
            strRowId = CType(sender, Button).CommandArgument.trim
            strDesc = CType(sender,Button).CommandName.trim
        Else
            lbl = tmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("lblRowId")
            strRowId = lbl.text.trim
            txt = tmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtDescription")
            strDesc = txt.text.trim
        End If

        If strRowId <> "" Then
            Response.Write("<Script Language=""JavaScript"">pop_Account=window.open(""../../CB/Trx/CB_Trx_TemplAccount.aspx?rowid=" & strRowId & _
                            "&strdesc=" & strDesc & _
                            """, null ,""'pop_Account',width=800,height=600,top=50,left=150,status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");pop_Account.focus();</Script>")
        End If
    End Sub


    Sub DEDR_Add(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim dataSet As dataSet = LoadDtlData()
        Dim newRow As DataRow
        Dim Updbutton As LinkButton
        Dim Droplist As DropDownList
        Dim lbl As Label
        Dim txt As TextBox
        Dim btn As Button
        Dim intDispSeq As Integer
        Dim strStmtAccYear As String
        Dim rfv As RequiredFieldValidator
        Dim chk As CheckBox

        newRow = dataSet.Tables(0).NewRow()
        newRow.Item("ReportCode") = Trim(txtCode.Text)
        newRow.Item("RowId") = 0
        newRow.Item("DispSeq") = 0
        newRow.Item("Description") = ""
        newRow.Item("RowType") = 1
        newRow.Item("RefNo") = ""
        newRow.Item("Formula") = ""
        newRow.Item("BegBalance") = 0
        dataSet.Tables(0).Rows.Add(newRow)

        TmplList.DataSource = dataSet
        TmplList.DataBind()

        TmplList.EditItemIndex = TmplList.Items.Count - 1
        TmplList.DataBind()

        lblOper.Text = "add"
        
        If CInt(TmplList.EditItemIndex) > 0 Then
            lbl = TmplList.Items.Item(CInt(TmplList.EditItemIndex) - 1).FindControl("lblDispSeq")
            intDispSeq = CInt(lbl.text) + 1
        Else
            intDispSeq = 1
        End If
        
        txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtDispSeq")
        txt.Text = intDispSeq
        txt.Readonly  = True

        btn = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("btnEditAccount")
        btn.Visible = False

        Droplist = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("ddlRowType")
        BindRowTypeList(Droplist)

        txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtRefNo")
        txt.Visible = True

        Updbutton = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("Delete")
        Updbutton.Visible = False

        rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvRefNo")
        rfv.Visible = True

        chk = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("chkBegBalance")
        chk.Visible = True

    End Sub

    Sub DEDR_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim Droplist As DropDownList
        Dim Label As Label
        Dim txt As TextBox
        Dim Updbutton As LinkButton
        Dim btn As Button
        Dim rfv As RequiredFieldValidator
        Dim strRowType As String
        Dim chk As CheckBox

        lblOper.text = "upd"
        
        Label = E.Item.FindControl("lblRowType")
        strRowType = Label.text

        TmplList.EditItemIndex = CInt(E.Item.ItemIndex)
        BindGrid()

        Droplist = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("ddlRowType")
        BindRowTypeList(Droplist, strRowType)

        If Droplist.SelectedItem.value = objCBTrx.EnumRowType.Entry Then
            Droplist.visible = True
            btn = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("btnEditAccount")
            btn.Visible = True
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtRefNo")
            txt.Visible = True
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtFormula")
            txt.Visible = False
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvRefNo")
            rfv.Visible = True
            chk = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("chkBegBalance")
            chk.Visible = true
        ElseIf Droplist.SelectedItem.value = objCBTrx.EnumRowType.Header Then
            btn = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("btnEditAccount")
            btn.Visible = False
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtRefNo")
            txt.Visible = False
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtFormula")
            txt.Visible = False
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvRefNo")
            rfv.Visible = False
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvFormula")
            rfv.Visible = False
            chk = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("chkBegBalance")
            chk.Visible = False
        Else
            btn = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("btnEditAccount")
            btn.Visible = False
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtRefNo")
            txt.Visible = True
            txt = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("txtFormula")
            txt.Visible = True
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvRefNo")
            rfv.Visible = True
            rfv = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("rfvFormula")
            rfv.Visible = True
            chk = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("chkBegBalance")
            chk.Visible = False
        End If
        Updbutton = TmplList.Items.Item(CInt(TmplList.EditItemIndex)).FindControl("Delete")
        Updbutton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
    End Sub


    Sub DEDR_Update(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Dim strOpCode As String
        Dim blnUpdate As Boolean
        Dim blnCheckSeq As Boolean
        Dim blnDupSeq As Boolean = False
        Dim blnCheckRef As Boolean = True
        Dim blnDupRef As Boolean = False

        Dim list As DropDownList
        Dim lbl As Label
        Dim txt As TextBox
        Dim chk As CheckBox
        
        Dim strReportCode As String
        Dim strRowId As String
        Dim strDispSeq As String
        Dim strDesc As String
        Dim strRowType As String
        Dim strRefNo As String
        Dim strFormula As String
        Dim intBalance As Integer

        strReportCode = Trim(txtCode.Text)
        
        lbl = E.Item.FindControl("lblRowId")
        strRowId = lbl.Text.Trim

        txt = E.Item.FindControl("txtDispSeq")
        strDispSeq = txt.Text.Trim

        txt = E.Item.FindControl("txtDescription")
        strDesc = txt.Text.Trim

        list = E.Item.FindControl("ddlRowType")
        strRowType = list.SelectedItem.Value

        chk = E.Item.FindControl("chkBegBalance")
        intBalance = chk.Checked   

        If Trim(strRowType) = objCBTrx.EnumRowType.Entry Then
            txt = E.Item.FindControl("txtRefNo")
            strRefNo = txt.text.trim
            strFormula = ""
        ElseIf Trim(strRowType) = objCBTrx.EnumRowType.Header Then
            strRefNo = ""
            strFormula = ""
            blnCheckRef = False
        Else
            txt = E.Item.FindControl("txtRefNo")
            strRefNo = txt.text.trim
            txt = E.Item.FindControl("txtFormula")
            strFormula = txt.text.trim     
         End If

        strParam = strReportCode & chr(9) & _
                   strRowId & chr(9) & _
                   strDispSeq & chr(9) & _
                   strDesc & chr(9) & _
                   strRowType & chr(9) & _
                   strRefNo & chr(9) & _
                   strFormula & chr(9) & _ 
                   intBalance

        If lblOper.text.trim = "add" Then
            strOpCode = strOpCd_Add
            blnUpdate = False
            blnCheckSeq = False
        Else
            strOpCode = strOpCd_Upd
            blnUpdate = True
            blnCheckSeq = True
        End If

        Try
            intErrNo = objCBTrx.mtdUpdTreasuryTemplDtl(strOpCode, _
                                                  strOpCd_Get, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  strParam, _
                                                  blnCheckSeq, _
                                                  blnCheckRef, _
                                                  blnDupSeq, _
                                                  blnDupRef, _
                                                  blnUpdate)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=CB_TRX_TREASURYTMPL_UPD&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_TreasuryTemplate.aspx")
        End Try

        If blnDupSeq = True Then
            lbl = E.Item.FindControl("lblDupSeq")
            lbl.visible = True
            Exit Sub
        End If

        If blnCheckRef = True And blnDupRef = True Then
            lbl = E.Item.FindControl("lblDupRef")
            lbl.visible = True
            Exit Sub
        End If

        TmplList.EditItemIndex = -1
        BindGrid()
    End Sub


    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        Dim lbl As Label
        Dim txt As TextBox
        Dim list As DropDownList
        
        Dim strRowId As String
        
        lbl = E.Item.FindControl("lblRowId")
        strRowId = lbl.Text.Trim

        strParam = strRowId 
        Try
            intErrNo = objCBTrx.mtdDelTreasuryTemplDtl(strOpCd_Del, _
                                                    strOpCdDelAcc, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam)
        Catch Exp As System.Exception
            Response.Redirect("../../include/mesg/ErrorMessage.aspx?errcode=CB_TRX_TREASURYTMPL_DEL&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_TreasuryTemplate.aspx")
        End Try

        TmplList.EditItemIndex = -1
        BindGrid()
    End Sub


    Sub DEDR_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        TmplList.EditItemIndex = -1
        BindGrid()
    End Sub


    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CB_trx_TreasuryTemplateList.aspx")
    End Sub
    

    Sub DeleteBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
    End Sub

    Sub SaveBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)

        Dim strOpCd As String 
        Dim strParam  As String
        Dim intErrNo As Integer
        Dim strDocCode As String
        
        If txtCode.Enabled = True THEN 
            strOpCd = "CB_CLSTRX_TREASURYTEMPL_ADD"
        Else
            strOpCd = "CB_CLSTRX_TREASURYTEMPL_UPD"
        End If
       
        strParam =  txtCode.Text & "|" & txtName.Text & "|" & txtDescription.text

        Try     
            intErrNo = objCBTrx.mtdUpdTreasuryTempl(strOpCd, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strParam)
            Catch Exp As Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_CLSTRX_TREASURYTEMPL_UPDATE&errmesg=" & Exp.Message & "&redirect=CB/trx/cb_trx_TreasuryTemplateList.aspx")
        End Try
        
        DisplayData()
        BindGrid()
    End Sub


    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblAccount.text = GetCaption(objLangCap.EnumLangCap.Account)
        TmplList.Columns(7).HeaderText = lblAccount.text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CB_TRX_TREASURYTMPL_GETLANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=CB/Trx/cb_trx_TreasuryTemplate.aspx")
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
