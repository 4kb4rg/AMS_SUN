

Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic


Public Class CT_IssueDet : Inherits Page

    Protected WithEvents dgStkTx As DataGrid
    Protected WithEvents lblStckTxID As Label
    Protected WithEvents lblErrMessage As Label
    Protected WithEvents IssueType As Label
    Protected WithEvents lblBlkTag As Label
    Protected WithEvents lblVehTag As Label
    Protected WithEvents lblVehExpTag As Label
    Protected WithEvents lblAccCodeTag As Label
    Protected WithEvents lblEmpTag As Label
    Protected WithEvents lblBPartyTag As Label
    Protected WithEvents lblBillTo As Label
    Protected WithEvents lblReprint As Label
    Protected WithEvents lstBillParty As DropDownList
    Protected WithEvents lstEmpID As DropDownList
    Protected WithEvents lstAccCode As DropDownList
    Protected WithEvents lstVehExp As DropDownList
    Protected WithEvents lstVehCode As DropDownList
    Protected WithEvents lstBlock As DropDownList
    Protected WithEvents lstBillTo As DropDownList

    Protected WithEvents Status As Label
    Protected WithEvents CreateDate As Label
    Protected WithEvents UpdateDate As Label
    Protected WithEvents UpdateBy As Label
    Protected WithEvents lblPrintDate As Label
    Protected WithEvents lblDNNoteID As Label
    Protected WithEvents lblTotAmtFig As Label
    Protected WithEvents txtRefSIS As TextBox
    Protected WithEvents txtRefDate As TextBox
    Protected WithEvents txtRemarks As TextBox
    Protected WithEvents txtQty As TextBox
    Protected WithEvents lstItem As DropDownList
    Protected WithEvents txtItemRef As TextBox
    Protected WithEvents lblDNIDTag As Label
    Protected WithEvents lblConfirmErr As Label
    Protected WithEvents tblAdd As HtmlTable
    Protected WithEvents validateQty As RequiredFieldValidator
    Protected WithEvents lblError As Label
    Protected WithEvents lblStock As Label
    Protected WithEvents lblUnDel As Label
    Protected WithEvents lblAccCodeErr As Label
    Protected WithEvents lblEmpErr As Label
    Protected WithEvents lblBillPartyErr As Label
    Protected WithEvents lblBlockErr As Label
    Protected WithEvents lblVehCodeErr As Label
    Protected WithEvents lblVehExpCodeErr As Label
    Protected WithEvents lblItemCodeErr As Label
    Protected WithEvents lblVehwarn As Label
    Protected WithEvents lblVehExpWarn As Label
    Protected WithEvents lblStatusHid As Label
    Protected WithEvents AccountCode As Label
    Protected WithEvents BillParty As Label
    Protected WithEvents EmployeeCode As Label
    Protected WithEvents Add As ImageButton
    Protected WithEvents PRDelete As ImageButton
    Protected WithEvents Save As ImageButton
    Protected WithEvents Confirm As ImageButton
    Protected WithEvents DebitNote As ImageButton
    Protected WithEvents Print As ImageButton
    Protected WithEvents btnNew As ImageButton
    Protected WithEvents Cancel As ImageButton
    Protected WithEvents Find As HtmlInputButton
    Protected WithEvents FindAcc As HtmlInputButton
    Protected WithEvents FindEmp As HtmlInputButton
    Protected WithEvents lblPleaseSelectOne As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblCode As Label
    Protected WithEvents cblDisplayCost As CheckBoxList
    Protected WithEvents lblTotPriceFig As Label

    Protected WithEvents RowAcc As HtmlTableRow
    Protected WithEvents RowBlk As HtmlTableRow
    Protected WithEvents RowVeh As HtmlTableRow
    Protected WithEvents RowVehExp As HtmlTableRow
    Protected WithEvents RowChargeLevel As HtmlTableRow
    Protected WithEvents RowPreBlk As HtmlTableRow
    Protected WithEvents lblPreBlockErr As Label
    Protected WithEvents lblPreBlkTag As Label
    Protected WithEvents lstPreBlock As DropDownList
    Protected WithEvents lstChargeLevel As DropDownList
    Protected WithEvents hidBlockCharge As HtmlInputHidden
    Protected WithEvents hidChargeLocCode As HtmlInputHidden
    Dim PreBlockTag As String

    Protected objCTtx As New agri.CT.clstrx()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objINstp As New agri.IN.clsSetup()
    Dim objGLset As New agri.GL.clsSetup()
    Dim objHRTrx As New agri.HR.clsTrx()
    Dim objGLtrx As New agri.GL.clsTrx()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdmin As New agri.Admin.clsShare()

    Dim strOpCdStckTxLine_GET As String = "CT_CLSTRX_STOCKISSUE_LINE_GET"
    Dim strOpCdStckTxDet_ADD As String = "CT_CLSTRX_STOCKISSUE_DETAIL_ADD"
    Dim strOpCdStckTxDet_UPD As String = "CT_CLSTRX_STOCKISSUE_DETAIL_UPD"
    Dim strOpCdItem_Details_GET As String = "IN_CLSSETUP_STOCKITEM_DETAILS_GET"
    Dim strOpCdItem_Details_UPD As String = "IN_CLSTRX_STOCKITEM_DETAIL_UPD"
    Dim strOpCdItem_Details_GET2 As String = "CT_CLSSETUP_ITEM_DETAILS_GET_BY_LOCATION"
    Dim objAR As New agri.GlobalHdl.clsAccessRights()

    Dim objDataSet As New DataSet()
    Dim dsGrid As New DataSet()
    Dim objLangCapDs As New DataSet()

    Dim intErrNo As Integer
    Dim strParam As String = ""

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strARAccMonth As String
    Dim strARAccYear As String
    Dim intCTAR As Integer
    Dim intConfigsetting As Integer

    Dim AccTag As String
    Dim VehTag As String
    Dim BlkTag As String
    Dim VehExpTag As String
    Dim strRefSIS As String = ""
    Dim strRefDate As String = ""
    Dim strRefRemark As String = ""
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType as String


    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        Dim IssueTyp As String
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")
        strARAccMonth = Session("SS_ARACCMONTH")
        strARAccYear = Session("SS_ARACCYEAR")
        intCTAR = Session("SS_CTAR")
        intConfigsetting = Session("SS_CONFIGSETTING")

        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTIssue), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            strRefSIS = IIf(Trim(txtRefSIS.Text) = "", "", Trim(txtRefSIS.Text))
            strRefDate = IIf(Trim(txtRefDate.Text) = "", "", Trim(txtRefDate.Text))
            If strRefSIS <> "" Or strRefDate <> "" Then
                strRefRemark = "Chit: " & strRefSIS & " Date: " & strRefDate
                txtRefSIS.Text = ""
                txtRefDate.Text = ""
            Else
                strRefRemark = ""
            End If

            If Not Page.IsPostBack Then
                BindChargeLevelDropDownList()
                BindBlockDropList("")

                lblStckTxID.Text = Request.QueryString("Id")

                If lblStckTxID.Text = "" Then
                    IssueTyp = Request.QueryString("isType")
                    Select Case IssueTyp
                        Case "Issue"
                            IssueType.Text = objCTtx.EnumStockIssueType.OwnUse
                        Case "Staff"
                            IssueType.Text = objCTtx.EnumStockIssueType.StaffPayroll
                        Case "External"
                            IssueType.Text = objCTtx.EnumStockIssueType.External
                    End Select
                Else
                    LoadStockTxDetails()

                    If objDataSet.Tables(0).Rows.Count > 0 Then
                        DisplayFromDB(False)
                        BindGrid()
                    End If

                End If
                PageControl()
                IssueTypeControl()
            End If

            lblError.Visible = False
            lblStock.Visible = False
            lblUnDel.Visible = False
            lblConfirmErr.Visible = False
            lblAccCodeErr.Visible = False
            lblPreBlockErr.Visible = False
            lblBlockErr.Visible = False
            lblVehCodeErr.Visible = False
            lblVehExpCodeErr.Visible = False
            lblItemCodeErr.Visible = False
            lblBillPartyErr.Visible = False
            lblEmpErr.Visible = False
        End If
    End Sub

    Sub BindChargeLevelDropDownList()
        lstChargeLevel.Items.Add(New ListItem(PreBlockTag, objLangCap.EnumLangCap.Block))
        lstChargeLevel.Items.Add(New ListItem(BlkTag, objLangCap.EnumLangCap.SubBlock))
        lstChargeLevel.selectedIndex = Session("SS_BLOCK_CHARGE_DEFAULT")
        ToggleChargeLevel()
    End Sub
    
    Sub lstChargeLevel_OnSelectedIndexChanged(sender As Object, e As EventArgs)
        ToggleChargeLevel()
    End Sub
    
    Sub ToggleChargeLevel()
        If lstChargeLevel.selectedIndex = 0 Then
            RowBlk.Visible = False
            RowPreBlk.Visible = True
            hidBlockCharge.value = "yes"
        Else
            RowBlk.Visible = True
            RowPreBlk.Visible = False
            hidBlockCharge.value = ""
        End If
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                BlkTag = GetCaption(objLangCap.EnumLangCap.Block)
            Else
                BlkTag = GetCaption(objLangCap.EnumLangCap.SubBlock)
            End If
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKISSUE_DETAIL_GET_LANGCAP_COSTLEVEL&errmesg=" & lblErrMessage.Text & "&redirect=ct/trx/ct_trx_StockIssue_List.aspx")
        End Try

        AccTag = GetCaption(objLangCap.EnumLangCap.Account)
        VehTag = GetCaption(objLangCap.EnumLangCap.Vehicle)
        VehExpTag = GetCaption(objLangCap.EnumLangCap.VehExpense)
        BillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty)

        lblAccCodeTag.Text = AccTag & lblCode.Text & " (DR) :* "
        lblVehTag.Text = VehTag & lblCode.Text & " : "
        lblBlkTag.Text = BlkTag & lblCode.Text & " : "
        lblVehExpTag.Text = VehExpTag & lblCode.Text & " : "
        lblBPartyTag.Text = BillParty.Text & lblCode.Text & " :* "

        dgStkTx.Columns(2).HeaderText = AccTag & lblCode.Text
        dgStkTx.Columns(3).HeaderText = BlkTag & lblCode.Text
        dgStkTx.Columns(4).HeaderText = VehTag & lblCode.Text
        dgStkTx.Columns(5).HeaderText = VehExpTag & lblCode.Text

        lblAccCodeErr.Text = "<BR>" & lblPleaseSelectOne.Text & AccTag & lblCode.Text
        lblVehCodeErr.Text = lblPleaseSelectOne.Text & VehTag & lblCode.Text
        lblBlockErr.Text = lblPleaseSelectOne.Text & BlkTag & lblCode.Text
        lblVehExpCodeErr.Text = lblPleaseSelectOne.Text & VehExpTag & lblCode.Text
        lblBillPartyErr.Text = lblPleaseSelectOne.Text & BillParty.Text & lblCode.Text

        AccountCode.Text = AccTag
        PreBlockTag = GetCaption(objLangCap.EnumLangCap.Block)
        lblPreBlkTag.Text = PreBlockTag & lblCode.Text & " : "
        lblPreBlockErr.Text = lblPleaseSelectOne.Text & PreBlockTag & lblCode.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKISSUE_DETAIL_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ct/trx/ct_trx_StockIssue_List.aspx")
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



    Sub IssueTypeControl()
        Select Case IssueType.Text
            Case objCTtx.EnumStockIssueType.OwnUse
                RowAcc.Visible = True
                RowChargeLevel.Visible = Session("SS_BLOCK_CHARGE_VISIBLE")
                ToggleChargeLevel()
                RowVeh.Visible = True
                RowVehExp.Visible = True
                DebitNote.Visible = False
                BindItemCodeList()
                BindAccCodeDropList()
                BindBlockDropList("")
                BindVehicleCodeDropList("")
                BindVehicleExpDropList(True)

                Select Case lblStatusHid.Text
                    Case CStr(objCTtx.EnumStockIssueStatus.Confirmed), CStr(objCTtx.EnumStockIssueStatus.Deleted), _
                        CStr(objCTtx.EnumStockIssueStatus.Cancelled), CStr(objCTtx.EnumStockIssueStatus.DbNote)
                        tblAdd.Visible = False

                    Case Else
                        tblAdd.Visible = True
                End Select
                CheckVehicleUse()

            Case objCTtx.EnumStockIssueType.StaffPayroll, objCTtx.EnumStockIssueType.StaffDN

                If objCTtx.EnumStockIssueType.StaffPayroll = IssueType.Text Then
                    DebitNote.Visible = False
                End If

                RowAcc.Visible = False
                RowChargeLevel.Visible = False
                RowPreBlk.Visible = False
                RowBlk.Visible = False
                RowVeh.Visible = False
                RowVehExp.Visible = False

                lblEmpTag.Visible = True
                lstEmpID.Visible = True
                lblBillTo.Visible = True
                lstBillTo.Visible = True
                BindItemCodeList()
                BindBillToList()
                BindEmployeeDropList()

                Select Case lblStatusHid.Text
                    Case CStr(objCTtx.EnumStockIssueStatus.Confirmed), CStr(objCTtx.EnumStockIssueStatus.Deleted), _
                        CStr(objCTtx.EnumStockIssueStatus.Cancelled), CStr(objCTtx.EnumStockIssueStatus.DbNote)
                        lstEmpID.Enabled = False
                        lstBillTo.Enabled = False
                        FindEmp.Visible = False
                    Case Else
                        lstEmpID.Enabled = True
                        lstBillTo.Enabled = True
                        FindEmp.Visible = True
                End Select

                If Not lblStckTxID.Text = "" Then
                    If objDataSet.Tables(0).Rows.Count > 0 Then
                        BindEmployeeDropList(Trim(objDataSet.Tables(0).Rows(0).Item("PSEmpCode")))
                        lstEmpID.Enabled = False
                        FindEmp.Visible = False
                    End If
                End If

            Case objCTtx.EnumStockIssueType.External

                RowAcc.Visible = False
                RowChargeLevel.Visible = False
                RowPreBlk.Visible = False
                RowBlk.Visible = False
                RowVeh.Visible = False
                RowVehExp.Visible = False

                lblBPartyTag.Visible = True
                lstBillParty.Visible = True
                BindBillPartyDropList()
                BindItemCodeList()

                If Not lblStckTxID.Text = "" Then
                    lstBillParty.Enabled = False
                End If


        End Select

    End Sub

    Sub PageControl()

        IssueTypeControl()
        txtRefSIS.Enabled = False
        txtRefDate.Enabled = False
        txtRemarks.Enabled = False
        cblDisplayCost.Visible = False
        validateQty.Visible = False
        Find.Visible = False
        Save.Visible = False
        Confirm.Visible = False
        Print.Visible = False
        PRDelete.Visible = False
        DebitNote.Visible = False
        Cancel.Visible = False
        btnNew.Visible = False
        Select Case Trim(lblStatusHid.text)
            Case Trim(CStr(objCTtx.EnumStockIssueStatus.Deleted))
                PRDelete.Visible = True
                btnNew.Visible = True
                PRDelete.ImageUrl = "../../images/butt_undelete.gif"
                PRDelete.AlternateText = "Undelete"
                PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('undelete');"
            Case Trim(CStr(objCTtx.EnumStockIssueStatus.Confirmed))
                btnNew.Visible = True
                If IssueType.text = objCTtx.EnumStockIssueType.External Or _
                   IssueType.text = objCTtx.EnumStockIssueType.StaffDN Then
                    DebitNote.Visible = True
                End If
                Print.Visible = True
                cblDisplayCost.Visible = True
            Case Trim(CStr(objCTtx.EnumStockIssueStatus.DBNote))
                btnNew.Visible = True
                Print.Visible = True
                cblDisplayCost.Visible = True
            Case Else
                txtRefSIS.Enabled = True
                txtRefDate.Enabled = True
                txtRemarks.Enabled = True
                cblDisplayCost.Visible = True
                validateQty.Visible = True
                Find.Visible = True
                Save.Visible = True
                If Trim(lblStckTxID.Text) <> "" Then
                    Confirm.Visible = True
                    PRDelete.Visible = True
                    btnNew.Visible = True
                    PRDelete.ImageUrl = "../../images/butt_delete.gif"
                    PRDelete.AlternateText = "Delete"
                    PRDelete.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                    If Trim(IssueType.text) <> Cstr(objCTtx.EnumStockIssueType.External) Then
                        Print.Visible = True
                        cblDisplayCost.Visible = True
                    End If
                End If
        End Select
        DisableItemTable()
    End Sub

    Sub DisableItemTable()

        If lblStatusHid.Text = CStr(objCTtx.EnumStockIssueStatus.Deleted) _
        Or lblStatusHid.Text = CStr(objCTtx.EnumStockIssueStatus.Cancelled) _
        Or lblStatusHid.Text = CStr(objCTtx.EnumStockIssueStatus.DbNote) Then
            tblAdd.Visible = False
            validateQty.Visible = False
        ElseIf lblStatusHid.Text = CStr(objCTtx.EnumStockIssueStatus.Confirmed) Then
            tblAdd.Visible = False
        ElseIf lblStatusHid.Text = CStr(objCTtx.EnumStockIssueStatus.Active) Then
            tblAdd.Visible = True
            validateQty.Visible = True

        End If

    End Sub

    Sub DataGrid_ItemCreated(ByVal Sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lbl As Label
        Select Case e.Item.ItemType
            Case ListItemType.Header
                Select Case IssueType.Text
                    Case objCTtx.EnumStockIssueType.OwnUse
                    Case objCTtx.EnumStockIssueType.StaffPayroll, objCTtx.EnumStockIssueType.StaffDN
                        e.Item.Cells(2).Visible = False
                        e.Item.Cells(3).Visible = False
                        e.Item.Cells(4).Visible = False
                        e.Item.Cells(5).Visible = False
                    Case objCTtx.EnumStockIssueType.External
                        e.Item.Cells(2).Visible = False
                        e.Item.Cells(3).Visible = False
                        e.Item.Cells(4).Visible = False
                        e.Item.Cells(5).Visible = False
                End Select

            Case ListItemType.Item, ListItemType.AlternatingItem
                Select Case IssueType.Text
                    Case objCTtx.EnumStockIssueType.OwnUse
                        lbl = e.Item.FindControl("AccCode")
                        lbl.Visible = True
                    Case objCTtx.EnumStockIssueType.StaffPayroll, objCTtx.EnumStockIssueType.StaffDN
                        dgStkTx.Columns(2).Visible = False
                        dgStkTx.Columns(3).Visible = False
                        dgStkTx.Columns(4).Visible = False
                        dgStkTx.Columns(5).Visible = False

                    Case objCTtx.EnumStockIssueType.External
                        dgStkTx.Columns(2).Visible = False
                        dgStkTx.Columns(3).Visible = False
                        dgStkTx.Columns(4).Visible = False
                        dgStkTx.Columns(5).Visible = False
                End Select
        End Select

        If lblStatusHid.Text = CStr(objCTtx.EnumStockIssueStatus.Active) Then
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim DeleteButton As LinkButton
                    DeleteButton = e.Item.FindControl("Delete")
                    DeleteButton.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
            End Select
        Else
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim DeleteButton As LinkButton
                    DeleteButton = e.Item.FindControl("Delete")
                    DeleteButton.Visible = False
            End Select
        End If


    End Sub


    Sub DisplayFromDB(ByVal pv_blnIsRedirect As Boolean)
        lblStatusHid.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Status"))
        IssueType.Text = Trim(objDataSet.Tables(0).Rows(0).Item("IssueType"))
        Status.Text = objCTtx.mtdGetStockIssueStatus(Trim(objDataSet.Tables(0).Rows(0).Item("Status")))
        CreateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("CreateDate")))
        UpdateDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("UpdateDate")))
        UpdateBy.Text = Trim(objDataSet.Tables(0).Rows(0).Item("UserName"))
        txtRemarks.Text = Trim(objDataSet.Tables(0).Rows(0).Item("Remark"))

        Dim intDate1 as integer
        Dim intDate2 as integer

        intDate1 = InStr(Trim(txtRemarks.Text), "Date:")
        txtRefSIS.Text = right(left(Trim(txtRemarks.Text), (intDate1 - 1)), (intDate1 - 6))

        txtRefDate.Text = right(Trim(txtRemarks.Text), (len(Trim(txtRemarks.Text)) - (5 + len(trim(txtRefSIS.Text)))))
        intDate2 = InStr(Trim(txtRefDate.Text), "Remarks:")
        txtRefDate.Text = right(left(Trim(txtRefDate.Text), (intDate2 - 1)), (intDate2 - 7))

        txtRemarks.Text = right(Trim(txtRemarks.Text), (len(Trim(txtRemarks.Text)) - (5 + len(trim(txtRefSIS.Text)) + 7 + len(trim(txtRefDate.Text)) + 10)))

        lblTotAmtFig.Text = ObjGlobal.GetIDDecimalSeparator(Trim(objDataSet.Tables(0).Rows(0).Item("TotalAmount")))     
        lblTotPriceFig.Text = ObjGlobal.GetIDDecimalSeparator(Trim(objDataSet.Tables(0).Rows(0).Item("TotalPrice"))) 
        lblPrintDate.Text = objGlobal.GetLongDate(Trim(objDataSet.Tables(0).Rows(0).Item("PrintDate")))
        If Not Trim(objDataSet.Tables(0).Rows(0).Item("DNID")) = "" Then
            lblDNIDTag.Visible = True
            lblDNNoteID.Text = Trim(objDataSet.Tables(0).Rows(0).Item("DNID"))
            If pv_blnIsRedirect = True Then
                Response.Redirect("../../BI/trx/BI_trx_DNDet.aspx?dbnid=" & lblDNNoteID.Text & "&referer=" & Request.ServerVariables("SCRIPT_NAME") & "?Id=" & lblStckTxID.Text)
            End If
        End If
    End Sub

    Sub BindGrid()
        Dim PageNo As Integer

        dgStkTx.DataSource = LoadDataGrid()
        dgStkTx.DataBind()
    End Sub

    Protected Function LoadDataGrid() As DataSet

        Dim strParam As String

        strParam = Trim(lblStckTxID.Text)

        Try
            intErrNo = objCTtx.mtdGetStockTransactDetails(strOpCdStckTxLine_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam, _
                                                          dsGrid)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_STOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
        End Try
        Return dsGrid
    End Function

    Sub LoadStockTxDetails()

        Dim strOpCdStckTxDet_GET As String = "CT_CLSTRX_STOCKISSUE_DETAIL_GET"
        Dim StockCode As String
        strParam = Trim(lblStckTxID.Text)

        Try
            intErrNo = objCTtx.mtdGetStockTransactDetails(strOpCdStckTxDet_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strParam, _
                                                          objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_STOCKISSUELINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
        End Try

    End Sub

    Sub DEDR_Delete(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)

        If lblStatusHid.Text = CStr(objCTtx.EnumStockIssueStatus.Active) Then

            Dim strOpCdStckTxLine_Det_GET As String = "CT_CLSTRX_STOCKISSUE_LINE_DETAILS_GET"
            Dim strOpCdStckTxLine_DEL As String = "CT_CLSTRX_STOCKISSUE_LINE_DEL"
            Dim lbl As Label
            Dim LnID As String
            Dim ItemCode As String
            Dim strParam As String
            Dim StrTxParam As String
            Dim TxID As String
            Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

            lbl = E.Item.FindControl("LnID")
            LnID = lbl.Text
            lbl = E.Item.FindControl("ItemCode")
            ItemCode = lbl.Text

            strParam = LnID & "|" & ItemCode
            Try
                intErrNo = objCTtx.mtdDelStockTransactLn(strOpCdStckTxLine_DEL, _
                                                        strOpCdStckTxLine_Det_GET, _
                                                        strOpCdItem_Details_UPD, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId, _
                                                        strAccMonth, _
                                                        strAccYear, _
                                                        strParam)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DEL_STOCKISSUELINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
                End If
            End Try

            StrTxParam = lblStckTxID.Text & "||||||||||||||||"

            Try
                intErrNo = objCTtx.mtdUpdStockIssueDetail(strOpCdStckTxDet_ADD, _
                                                          strOpCdStckTxDet_UPD, _
                                                          strOpCdStckTxLine_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strARAccMonth, _
                                                          strARAccYear, _
                                                          StrTxParam.ToString, _
                                                          objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenIssue), _
                                                          ErrorChk, _
                                                          TxID)

                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

                lblStckTxID.Text = TxID
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
                End If
            End Try
            LoadStockTxDetails()
            DisplayFromDB(False)
            BindGrid()
            BindItemCodeList()
        End If
    End Sub

    Sub BindBillToList()
        lstBillTo.Items.Clear()
        lstBillTo.Items.Add(New ListItem(objCTtx.mtdGetStockIssueType(objCTtx.EnumStockIssueType.StaffPayroll), objCTtx.EnumStockIssueType.StaffPayroll))
        lstBillTo.Items.Add(New ListItem(objCTtx.mtdGetStockIssueType(objCTtx.EnumStockIssueType.StaffDN), objCTtx.EnumStockIssueType.StaffDN))
        If Not lblStckTxID.Text = "" Then
            Select Case Trim(objDataSet.Tables(0).Rows(0).Item("IssueType"))
                Case objCTtx.EnumStockIssueType.StaffPayroll
                    lstBillTo.SelectedIndex = 0
                Case objCTtx.EnumStockIssueType.StaffDN
                    lstBillTo.SelectedIndex = 1
            End Select

        End If

    End Sub

    Sub BindBlockDropList(ByVal pv_strAccCode As String, Optional ByVal pv_strBlkCode As String = "")
        Dim dsForDropDown As DataSet
        Dim intSelectedIndex As Integer = 0
        Dim intCnt As Integer
        Dim strOpCdBlockList_Get As String
        Dim strParam As String
        Dim dr As DataRow

        Try
            If objSysCfg.mtdHasConfigValue(objSysCfg.mtdGetConfigSetting(objSysCfg.EnumConfig.BlockCostLevel), intConfigsetting) = True Then
                strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_BLOCK_GET"
                strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLset.EnumBlockStatus.Active
            Else
                strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_SUBBLOCK_GET"
                strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLset.EnumSubBlockStatus.Active
            End If
            intErrNo = objGLset.mtdGetAccountBlock(strOpCdBlockList_Get, _
                                                     strParam, _
                                                     dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            If dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
                Exit For
            End If
        Next

        dr = dsForDropDown.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("_Description") = lblSelect.Text & BlkTag & lblCode.Text

        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)
        lstBlock.DataSource = dsForDropDown.Tables(0)
        lstBlock.DataValueField = "BlkCode"
        lstBlock.DataTextField = "_Description"
        lstBlock.DataBind()
        lstBlock.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If

        strOpCdBlockList_Get = "GL_CLSSETUP_ACCOUNT_PREBLOCK_GET"
        intSelectedIndex = 0
        Try
            strParam = pv_strAccCode & "|" & Session("SS_LOCATION") & "|" & objGLset.EnumBlockStatus.Active
            intErrNo = objGLset.mtdGetAccountBlock(strOpCdBlockList_Get, _
                                                     strParam, _
                                                     dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GL_CLSSETUP_ACCOUNT_BLOCK_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode"))
            dsForDropDown.Tables(0).Rows(intCnt).Item("Description") = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode")) & " (" & Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("Description")) & ")"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("BlkCode") = Trim(pv_strBlkCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next

        dr = dsForDropDown.Tables(0).NewRow()
        dr("BlkCode") = ""
        dr("Description") = lblSelect.Text & PreBlockTag & lblCode.Text

        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)
        lstPreBlock.DataSource = dsForDropDown.Tables(0)
        lstPreBlock.DataValueField = "BlkCode"
        lstPreBlock.DataTextField = "Description"
        lstPreBlock.DataBind()
        lstPreBlock.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindAccCodeDropList(Optional ByVal pv_strAccCode As String = "")

        Dim strOpCd As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
        Dim dr As DataRow
        Dim strParam As String = "Order By ACC.AccCode|And ACC.Status = '" & objGLset.EnumAccountCodeStatus.Active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0
        Dim dsForDropDown As DataSet

        Try
            intErrNo = objGLset.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLset.EnumGLMasterType.AccountCode, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=BindAccCodeDropList&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            If dsForDropDown.Tables(0).Rows(intCnt).Item("AccCode") = Trim(pv_strAccCode) Then
                intSelectedIndex = intCnt + 1
                Exit For 
            End If
        Next

        dr = dsForDropDown.Tables(0).NewRow()
        dr("AccCode") = ""
        dr("_Description") = lblSelect.Text & AccTag & lblCode.Text

        dsForDropDown.Tables(0).Rows.InsertAt(dr, 0)

        lstAccCode.DataSource = dsForDropDown.Tables(0)
        lstAccCode.DataValueField = "AccCode"
        lstAccCode.DataTextField = "_Description"
        lstAccCode.DataBind()
        lstAccCode.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub CallCheckVehicleUse(ByVal sender As Object, ByVal e As System.EventArgs)
        CheckVehicleUse()
    End Sub

    Sub CheckVehicleUse()
        Dim intAccType As Integer
        Dim intAccPurpose As Integer
        Dim intNurseryInd As Integer    

        Dim strAcc As String = Request.Form("lstAccCode")
        Dim strBlk As String = Request.Form("lstBlock")
        Dim strVeh As String = Request.Form("lstVehCode")
        Dim strVehExp As String = Request.Form("lstVehExp")

        If lstAccCode.SelectedIndex = 0 Then
            txtItemRef.Text = ""
            Exit Sub
        Else
            GetAccountDetails(strAcc, intAccType, intAccPurpose, intNurseryInd)
        End If

        If intAccType = objGLset.EnumAccountType.ProfitAndLost Then
            Select Case intAccPurpose
                Case objGLset.EnumAccountPurpose.NonVehicle
                    BindBlockDropList(strAcc, strBlk)
                    BindVehicleCodeDropList("")
                    BindVehicleExpDropList(True)
                Case objGLset.EnumAccountPurpose.VehicleDistribution
                    BindBlockDropList("")
                    BindVehicleCodeDropList(strAcc, strVeh)
                    BindVehicleExpDropList(False, strVehExp)
                Case Else
                    BindBlockDropList(strAcc, strBlk)
                    BindVehicleCodeDropList("%", strVeh)
                    BindVehicleExpDropList(False, strVehExp)
            End Select
        ElseIf intAccType = objGLset.EnumAccountType.BalanceSheet And intNurseryInd = objGLset.EnumNurseryAccount.Yes Then  
            BindBlockDropList(strAcc, strBlk)
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
        Else
            BindBlockDropList("")
            BindVehicleCodeDropList("")
            BindVehicleExpDropList(True)
        End If

    End Sub

    Sub GetAccountDetails(ByVal pv_strAccCode As String, _
                      ByRef pr_strAccType As Integer, _
                      ByRef pr_strAccPurpose As Integer, _
                      ByRef pr_strNurseryInd As Integer)

        Dim _objAccDs As New DataSet
        Dim strOpCd As String = "GL_CLSSETUP_CHARTOFACCOUNT_GET_BY_ACCCODE"
        Dim strParam As String = pv_strAccCode
        Dim intErrNo As Integer

        Try
            intErrNo = objGLset.mtdGetAccount(strOpCd, _
                                                strParam, _
                                                _objAccDs, _
                                                True)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=GET_ACCOUNT_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        If _objAccDs.Tables(0).Rows.Count = 1 Then
            pr_strAccType = CInt(_objAccDs.Tables(0).Rows(0).Item("AccType"))
            pr_strAccPurpose = CInt(_objAccDs.Tables(0).Rows(0).Item("AccPurpose"))
            pr_strNurseryInd = Convert.ToInt16(_objAccDs.Tables(0).Rows(0).Item("NurseryInd"))
        End If
    End Sub

    Function CheckField() As Boolean
        Select Case IssueType.Text
            Case objCTtx.EnumStockIssueType.External
                If lstBillParty.SelectedItem.Value = "" Then
                    lblBillPartyErr.Visible = True
                    Return True
                End If
            Case objCTtx.EnumStockIssueType.StaffPayroll, objCTtx.EnumStockIssueType.StaffDN

                If lblStckTxID.Text = "" Then
                    If Request.Form("lstEmpID") = "" Then
                        lblEmpErr.Visible = True
                        Return True
                    End If

                End If

            Case Else
                Return False
        End Select

    End Function

    Function CheckRequiredField() As Boolean
        Dim strItem As String = Request.Form("lstItem")
        Dim intAccType As Integer
        Dim intAccPurpose As Integer
        Dim intNurseryInd As Integer
        Dim strAcc As String
        Dim strPreBlk As String = Request.Form("lstPreBlock")
        Dim strBlk As String
        Dim strVeh As String
        Dim strVehExp As String

        If IssueType.Text = objCTtx.EnumStockIssueType.OwnUse Then

            strAcc = Request.Form("lstAccCode").Trim
            strBlk = lstBlock.SelectedItem.Value.Trim
            strVeh = Request.Form("lstVehCode").Trim
            strVehExp = Request.Form("lstVehExp").Trim
            GetAccountDetails(strAcc, intAccType, intAccPurpose, intNurseryInd)

        End If

        If strItem.Trim = "" Then
            lblItemCodeErr.Visible = True
            Return True
        End If

        Select Case IssueType.Text
            Case objCTtx.EnumStockIssueType.OwnUse
                If strAcc = "" Then
                    lblAccCodeErr.Visible = True
                    Return True
                End If

                If intAccType = objGLset.EnumAccountType.BalanceSheet And intNurseryInd = objGLset.EnumNurseryAccount.No Then
                    Return False
                ElseIf intAccType = objGLset.EnumAccountType.BalanceSheet And intNurseryInd = objGLset.EnumNurseryAccount.Yes Then
                    If lstChargeLevel.SelectedIndex = 0 Then
                        If strPreBlk = "" Then
                            lblPreBlockErr.Visible = True
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        If strBlk = "" Then
                            lblBlockErr.Visible = True
                            Return True
                        Else
                            Return False
                        End If
                    End If
                Else

                    If strPreBlk = "" And lstChargeLevel.SelectedIndex = 0 And (Not intAccPurpose = objGLset.EnumAccountPurpose.VehicleDistribution) Then
                        lblBlockErr.Visible = True
                        Return True
                    ElseIf strBlk = "" And lstChargeLevel.SelectedIndex = 1 And (Not intAccPurpose = objGLset.EnumAccountPurpose.VehicleDistribution) Then
                        lblPreBlockErr.Visible = True
                        Return True
                    ElseIf strVeh = "" And intAccPurpose = objGLset.EnumAccountPurpose.VehicleDistribution Then
                        lblVehCodeErr.Visible = True
                        Return True
                    ElseIf strVehExp = "" And intAccPurpose = objGLset.EnumAccountPurpose.VehicleDistribution Then
                        lblVehExpCodeErr.Visible = True
                        Return True
                    ElseIf strVeh <> "" And strVehExp = "" And intAccPurpose = objGLset.EnumAccountPurpose.others Then
                        lblVehExpCodeErr.Visible = True
                        Return True
                    ElseIf strVeh = "" And intAccPurpose = objGLset.EnumAccountPurpose.others And strVehExp <> "" Then
                        lblVehCodeErr.Visible = True
                        Return True
                    Else
                        Return False
                    End If
                End If

            Case Else
                Return False
        End Select
    End Function

    Sub BindVehicleCodeDropList(ByVal pv_strAccCode As String, Optional ByVal pv_strVehCode As String = "")

        Dim dsForDropDown As DataSet
        Dim strOpCd As String
        Dim drinsert As DataRow
        Dim strParam As New StringBuilder
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intMasterType As Integer
        Dim intSelectedIndex As Integer = 0

        strParam.Append("|AccCode = '" & pv_strAccCode & "' AND LocCode = '" & Session("SS_LOCATION") & "' AND Status = '" & objGLset.EnumVehicleStatus.Active & "'")

        Try
            strOpCd = "GL_CLSSETUP_VEH_LIST_GET"
            intErrNo = objGLset.mtdGetMasterList(strOpCd, _
                                                   strParam.ToString, _
                                                   objGLset.EnumGLMasterType.Vehicle, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=GL/Trx/GL_Trx_Journal_List.aspx")
        End Try




        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("VehCode") = Trim(pv_strVehCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = lblSelect.Text & VehTag & lblCode.Text

        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstVehCode.DataSource = dsForDropDown.Tables(0)
        lstVehCode.DataValueField = "VehCode"
        lstVehCode.DataTextField = "Description"
        lstVehCode.DataBind()
        lstVehCode.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindVehicleExpDropList(ByVal pv_IsBlankList As Boolean, Optional ByVal pv_strVehExpCode As String = "")

        Dim dsForDropDown As DataSet
        Dim strOpCd As String = "GL_CLSSETUP_VEHEXPENSE_LIST_GET"
        Dim drinsert As DataRow
        Dim strParam As String = "Order By VehExpenseCode ASC|And Veh.Status = '" & objGLset.EnumVehicleExpenseStatus.active & "'"
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer = 0

        Try
            If pv_IsBlankList Then
                strParam += " And Veh.VehExpensecode = ''"
            End If
            intErrNo = objGLset.mtdGetMasterList(strOpCd, _
                                                   strParam, _
                                                   objGLset.EnumGLMasterType.VehicleExpense, _
                                                   dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=JR_TRX_ADTRX_VEHEXPENSE_GET&errmesg=" & lblErrMessage.Text & "&redirect=")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("VehExpenseCode") = Trim(pv_strVehExpCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = lblSelect.Text & VehExpTag & lblCode.Text

        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstVehExp.DataSource = dsForDropDown.Tables(0)
        lstVehExp.DataValueField = "VehExpenseCode"
        lstVehExp.DataTextField = "Description"
        lstVehExp.DataBind()
        lstVehExp.SelectedIndex = intSelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub BindEmployeeDropList(Optional ByVal pv_strEmpCode As String = "")

        Dim strOpCdEmployee_Get As String = "HR_CLSSETUP_EMPLOYEE_LIST_GET"
        Dim dsForDropDown As DataSet
        Dim dsForInactiveItem As DataSet
        Dim SelectedIndex As Integer = -1
        Dim intCnt As Integer
        Dim TblAlias As String
        Dim DataTextField As String
        Dim SearchStr As String
        Dim strParam As New StringBuilder("")
        Dim drinsert As DataRow
        Dim intSelectedIndex As Integer = 0

        Try
            strParam.Append("|||")
            strParam.Append(objHRTrx.EnumEmpStatus.active)
            strParam.Append("|")
            strParam.Append(strLocation)
            strParam.Append("|Mst.EmpCode|ASC")

            intErrNo = objHRTrx.mtdGetEmployeeList(strOpCdEmployee_Get, strParam.ToString, dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_StockIssue_List.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If dsForDropDown.Tables(0).Rows(intCnt).Item("EmpCode") = Trim(pv_strEmpCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = "Select one Employee Code"
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstEmpID.DataSource = dsForDropDown.Tables(0)
        lstEmpID.DataValueField = "EmpCode"
        lstEmpID.DataTextField = "EmpName"
        lstEmpID.DataBind()
        lstEmpID.SelectedIndex = intSelectedIndex







        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
        If Not dsForInactiveItem Is Nothing Then
            dsForInactiveItem = Nothing
        End If
    End Sub

    Sub BindBillPartyDropList()

        Dim strOpCdBillParty_Get As String = "GL_CLSTRX_BILLPARTY_LIST_GET"
        Dim dsForDropDown As DataSet
        Dim dsForInactiveItem As DataSet
        Dim SelectedIndex As Integer = -1
        Dim intCnt As Integer
        Dim TblAlias As String
        Dim DataTextField As String
        Dim SearchStr As String
        Dim strParam As New StringBuilder("")
        Dim drinsert As DataRow

        Try
            strParam.Append("|")
            strParam.Append(objGLset.EnumBillPartyStatus.active)
            strParam.Append("|||")
            strParam.Append("|BillPartyCode|ASC")

            intErrNo = objGLtrx.mtdGetGLMasterList(strOpCdBillParty_Get, strParam.ToString, dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_StockIssue_List.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If Not lblStckTxID.Text = "" Then
                If UCase(Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("BillPartyCode"))) = UCase(Trim(objDataSet.Tables(0).Rows(0).Item("BillPartyCode"))) Then
                    SelectedIndex = intCnt + 1
                End If
            End If
        Next intCnt
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = lblSelect.Text & BillParty.Text & lblCode.Text
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstBillParty.DataSource = dsForDropDown.Tables(0)
        lstBillParty.DataValueField = "BillPartyCode"
        lstBillParty.DataTextField = "Name"
        lstBillParty.DataBind()

        If SelectedIndex = -1 And Not lblStckTxID.Text = "" Then

            Try
                strParam.Remove(0, strParam.Length)
                strParam.Append("||")
                strParam.Append(" AND BillPartyCode = '" & Trim(objDataSet.Tables(0).Rows(0).Item("psEmpCode")) & "'")
                strParam.Append("||")
                strParam.Append("|BillPartyCode|ASC")

                intErrNo = objGLtrx.mtdGetGLMasterList(strOpCdBillParty_Get, strParam.ToString, dsForInactiveItem)

            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_BILLPARTYNOTFOUND&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_StockIssue_List.aspx")
            End Try

            If dsForInactiveItem.Tables(0).Rows.Count > 0 Then  
                lstBillParty.Items.Add(New ListItem(Trim(dsForInactiveItem.Tables(0).Rows(0).Item(0)) & _
                " (" & objGlobal.mtdGetItemDBStatus(objGlobal.EnumItemDBStatus.Unknown) & ") ", Trim(dsForInactiveItem.Tables(0).Rows(0).Item(0))))
                SelectedIndex = lstBillParty.Items.Count - 1
            Else 
                SelectedIndex = 0
            End If
        End If

        lstBillParty.SelectedIndex = SelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
        If Not dsForInactiveItem Is Nothing Then
            dsForInactiveItem = Nothing
        End If
    End Sub

    Sub BindItemCodeList(Optional ByVal pv_strItemCode As String = "")

        Dim strOpCdItem_List_GET As String = "IN_CLSSETUP_STOCKITEM_LIST_GET"
        Dim dsItemCodeDropList As DataSet
        Dim intCnt As Integer
        Dim strparam As String
        Dim intSelectedIndex As Integer = 0

        strparam = objINstp.EnumInventoryItemType.CanteenItem & "|" & objINstp.EnumStockItemStatus.Active & "|" & lblStckTxID.Text & "|" & "itm.ItemCode"

        Try
            intErrNo = objINstp.mtdGetFilteredItemList(strOpCdItem_List_GET, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strAccMonth, _
                                                       strAccYear, _
                                                       strparam, _
                                                       objCTtx.EnumInventoryTransactionType.StockIssue, _
                                                       dsItemCodeDropList)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTOSERVTYPEDROPDOWNLIST&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_StockIssue_List.aspx")
        End Try

        For intCnt = 0 To dsItemCodeDropList.Tables(0).Rows.Count - 1
            dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0) = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item(0))
            dsItemCodeDropList.Tables(0).Rows(intCnt).Item(1) = Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("ItemCode")) & " ( " _
                                                                & Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("Description")) & " ), " & _
                                                                FormatCurrency(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("AverageCost")), 2) & ", " & _
                                                                FormatNumber(Trim(dsItemCodeDropList.Tables(0).Rows(intCnt).Item("QtyOnHand")), 5) & " Unit"
            If dsItemCodeDropList.Tables(0).Rows(intCnt).Item("ItemCode") = Trim(pv_strItemCode) Then
                intSelectedIndex = intCnt + 1
            End If
        Next intCnt

        Dim drinsert As DataRow
        drinsert = dsItemCodeDropList.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = "Select Item Code"
        dsItemCodeDropList.Tables(0).Rows.InsertAt(drinsert, 0)

        lstItem.DataSource = dsItemCodeDropList.Tables(0)
        lstItem.DataValueField = "ItemCode"
        lstItem.DataTextField = "Description"
        lstItem.DataBind()
        lstItem.SelectedIndex = intSelectedIndex

        DisableItemTable()

        If Not dsItemCodeDropList Is Nothing Then
            dsItemCodeDropList = Nothing
        End If
    End Sub

    Sub btnAdd_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

        Dim strOpCdStckTxLine_ADD As String = "CT_CLSTRX_STOCKISSUE_LINE_ADD"
        Dim strOpCodeGLSubBlkByBlk As String = "GL_CLSSETUP_SUBBLOCK_BY_BLOCK_GET"
        Dim strParamList As String
        Dim strTxLnParam As New StringBuilder
        Dim strStatus As String
        Dim TxID As String
        Dim IssType As String
        Dim strAmount As String
        Dim StrTxParam As New StringBuilder
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim strItem As String = Request.Form("lstItem")
        Dim strItemRef As String = txtItemRef.Text

        Dim strAcc As String
        Dim strBlk As String
        Dim strVeh As String
        Dim strVehExp As String

        If IssueType.Text = objCTtx.EnumStockIssueType.OwnUse Then
            strAcc = Request.Form("lstAccCode")
            strBlk = Request.Form("lstBlock")
            strVeh = Request.Form("lstVehCode")
            strVehExp = Request.Form("lstVehExp")
        End If

        If CheckRequiredField() Or CheckField() Then
            Exit Sub
        End If

        If IssueType.Text = objCTtx.EnumStockIssueType.StaffPayroll Or IssueType.Text = objCTtx.EnumStockIssueType.StaffDN Then
            IssType = lstBillTo.SelectedItem.Value
        Else
            IssType = IssueType.Text
        End If

        If lblStckTxID.Text = "" Then
            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("|||||||")
            If IssueType.Text = objCTtx.EnumStockIssueType.StaffPayroll Or IssueType.Text = objCTtx.EnumStockIssueType.StaffDN Then
                StrTxParam.Append(Request.Form("lstEmpID"))
            End If
            StrTxParam.Append("|")
            If IssueType.Text = objCTtx.EnumStockIssueType.External Then
                StrTxParam.Append(lstBillParty.SelectedItem.Value)
            End If
            StrTxParam.Append("|")
            StrTxParam.Append(IssType)
            StrTxParam.Append("|0|")
            StrTxParam.Append(strRefRemark & " Remarks: " & txtRemarks.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccMonth)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccYear)
            StrTxParam.Append("|")
            StrTxParam.Append(strLocation)
            StrTxParam.Append("|")

            Try
                intErrNo = objCTtx.mtdUpdStockIssueDetail(strOpCdStckTxDet_ADD, _
                                                          strOpCdStckTxDet_UPD, _
                                                          strOpCdStckTxLine_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strARAccMonth, _
                                                          strARAccYear, _
                                                          StrTxParam.ToString, _
                                                          objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenIssue), _
                                                          ErrorChk, _
                                                          TxID)

                lblStckTxID.Text = TxID
                If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                    lblError.Visible = True
                End If

            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
                End If
            End Try

        End If

        strTxLnParam.Append(lblStckTxID.Text)
        strTxLnParam.Append("|")
        strTxLnParam.Append(strItem)
        strTxLnParam.Append("|")
        strTxLnParam.Append(txtQty.Text)
        strTxLnParam.Append("|")
        strTxLnParam.Append(IssueType.Text)
        strTxLnParam.Append("|")

        If IssueType.Text = objCTtx.EnumStockIssueType.OwnUse Then

            strTxLnParam.Append(strAcc)
            strTxLnParam.Append("|")
            If lstChargeLevel.SelectedIndex = 1 Then
                strTxLnParam.Append(Request.Form("lstBlock").Trim)
            Else
                strTxLnParam.Append(Request.Form("lstPreBlock").Trim)
            End If
            strTxLnParam.Append("|")
            strTxLnParam.Append(strVeh)
            strTxLnParam.Append("|")
            strTxLnParam.Append(strVehExp)
        Else
            strTxLnParam.Append("|||")
        End If

        strTxLnParam.Append("|")
        strTxLnParam.Append(strItemRef)

        Try
            If lstChargeLevel.SelectedIndex = 0 And RowPreBlk.Visible = True Then
                strParamList = Session("SS_LOCATION") & "|" & _
                                       lstAccCode.SelectedItem.Value.Trim & "|" & _
                                       lstPreBlock.SelectedItem.Value.Trim & "|" & _
                                       objGLset.EnumBlockStatus.Active
                intErrNo = objCTtx.mtdAddStockIssueLnByBlock(strOpCodeGLSubBlkByBlk, _
                                                             strParamList, _
                                                             strOpCdStckTxLine_ADD, _
                                                             strOpCdItem_Details_GET2, _
                                                             strOpCdItem_Details_UPD, _
                                                             strCompany, _
                                                             strLocation, _
                                                             strUserId, _
                                                             strAccMonth, _
                                                             strAccYear, _
                                                             ErrorChk, _
                                                             objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.StockIssueLn), _
                                                             strTxLnParam.ToString)
            Else
                intErrNo = objCTtx.mtdAddStockIssueLn(strOpCdStckTxLine_ADD, _
                                                    strOpCdItem_Details_GET2, _
                                                    strOpCdItem_Details_UPD, _
                                                    strCompany, _
                                                    strLocation, _
                                                    strUserId, _
                                                    strAccMonth, _
                                                    strAccYear, _
                                                    ErrorChk, _
                                                    objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.StockIssueLn), _
                                                    strTxLnParam.ToString)
            End If
            Select Case ErrorChk
                Case objCTtx.EnumInventoryErrorType.OverFlow
                    lblError.Visible = True
                Case objCTtx.EnumInventoryErrorType.InsufficientQty
                    lblStock.Visible = True
            End Select

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDISSUELINE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
            End If
        End Try

        StrTxParam.Remove(0, StrTxParam.Length)
        StrTxParam.Append(lblStckTxID.Text)
        StrTxParam.Append("||||||||||||||||")

        Try
            intErrNo = objCTtx.mtdUpdStockIssueDetail(strOpCdStckTxDet_ADD, _
                                                      strOpCdStckTxDet_UPD, _
                                                      strOpCdStckTxLine_GET, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strARAccMonth, _
                                                      strARAccYear, _
                                                      StrTxParam.ToString, _
                                                      objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenIssue), _
                                                      ErrorChk, _
                                                      TxID)
            If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

            lblStckTxID.Text = TxID
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
            End If
        End Try

        LoadStockTxDetails()
        DisplayFromDB(False)
        BindGrid()
        PageControl()

        txtQty.Text = ""

        If Not StrTxParam Is Nothing Then
            StrTxParam = Nothing
        End If
        If Not strTxLnParam Is Nothing Then
            strTxLnParam = Nothing
        End If


    End Sub

    Sub btnSave_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim TxID As String
        Dim IssType As String
        Dim strAllowVehicle As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim StrTxParam As New StringBuilder

        If Not IssueType.Text = objCTtx.EnumStockIssueType.OwnUse Then
            If CheckField() Then
                Exit Sub
            End If
        End If

        If IssueType.Text = objCTtx.EnumStockIssueType.StaffPayroll Or IssueType.Text = objCTtx.EnumStockIssueType.StaffDN Then
            IssType = lstBillTo.SelectedItem.Value
        Else
            IssType = IssueType.Text
        End If

        If lblStckTxID.Text = "" Then
            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("|||||||")
            If IssueType.Text = objCTtx.EnumStockIssueType.StaffPayroll Or IssueType.Text = objCTtx.EnumStockIssueType.StaffDN Then
                StrTxParam.Append(Request.Form("lstEmpID"))
            End If
            StrTxParam.Append("|")
            If IssueType.Text = objCTtx.EnumStockIssueType.External Then
                StrTxParam.Append(lstBillParty.SelectedItem.Value)
            End If
            StrTxParam.Append("|")
            StrTxParam.Append(IssType)
            StrTxParam.Append("|0|")
            StrTxParam.Append(strRefRemark & " Remarks: " & txtRemarks.Text)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccMonth)
            StrTxParam.Append("|")
            StrTxParam.Append(strAccYear)
            StrTxParam.Append("|")
            StrTxParam.Append(strLocation)
            StrTxParam.Append("|")
        Else
            StrTxParam.Append(lblStckTxID.Text)
            StrTxParam.Append("|||||||")
            If IssueType.Text = objCTtx.EnumStockIssueType.StaffPayroll Or IssueType.Text = objCTtx.EnumStockIssueType.StaffDN Then
                StrTxParam.Append(Request.Form("lstEmpID"))
            End If
            StrTxParam.Append("|")
            If IssueType.Text = objCTtx.EnumStockIssueType.External Then
                StrTxParam.Append(lstBillParty.SelectedItem.Value)
            End If
            StrTxParam.Append("|")
            StrTxParam.Append(IssType)
            StrTxParam.Append("||")
            StrTxParam.Append(strRefRemark & " Remarks: " & txtRemarks.Text)
            StrTxParam.Append("|||||")
        End If

        Try
            intErrNo = objCTtx.mtdUpdStockIssueDetail(strOpCdStckTxDet_ADD, _
                                                      strOpCdStckTxDet_UPD, _
                                                      strOpCdStckTxLine_GET, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strARAccMonth, _
                                                      strARAccYear, _
                                                      StrTxParam.ToString, _
                                                      objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenIssue), _
                                                      ErrorChk, _
                                                      TxID)

            lblStckTxID.Text = TxID
            If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
            End If
        End Try
        If Not StrTxParam Is Nothing Then
            StrTxParam = Nothing
        End If

        LoadStockTxDetails()
        DisplayFromDB(False)
        PageControl()

    End Sub

    Sub btnConfirm_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim strOpCodeGetAllLines As String = "CT_CLSTRX_CANTEENISSUE_LINE_GET_ALL"
        Dim strOpCodeUpdLine As String = "CT_CLSTRX_CANTEENISSUE_LINE_SYN"

        If Not LoadDataGrid.Tables(0).Rows.Count > 0 Then
            lblConfirmErr.Visible = True
            Exit Sub
        End If


        StrTxParam = lblStckTxID.Text & "|" & IssueType.Text
        Try
            intErrNo = objCTtx.mtdSynStockIssueLn(strOpCodeGetAllLines, _
                                                  strOpCodeUpdLine, _
                                                  strOpCdItem_Details_GET2, _
                                                  strOpCdItem_Details_UPD, _
                                                  strCompany, _
                                                  strLocation, _
                                                  strUserId, _
                                                  ErrorChk, _
                                                  StrTxParam)
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=REMOVEINVENTORYITEM&errmesg=" & lblErrMessage.Text & "&redirect=IN/Trx/IN_Trx_Stocktransfer_List.aspx")
            End If
        End Try
        Select Case ErrorChk
            Case objCTtx.EnumInventoryErrorType.OverFlow
                lblError.Visible = True
                Exit Sub
            Case objCTtx.EnumInventoryErrorType.InsufficientQty
                lblStock.Visible = True
                Exit Sub
            Case Else
                lblError.Visible = False
                lblStock.Visible = False
        End Select

        StrTxParam = lblStckTxID.Text & "|||||||||||" & strRefRemark & " Remarks: " & txtRemarks.Text & "|||||" & objCTtx.EnumStockIssueStatus.Confirmed
        Try
            intErrNo = objCTtx.mtdUpdStockIssueDetail(strOpCdStckTxDet_ADD, _
                                                      strOpCdStckTxDet_UPD, _
                                                      strOpCdStckTxLine_GET, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strARAccMonth, _
                                                      strARAccYear, _
                                                      StrTxParam.ToString, _
                                                      objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenIssue), _
                                                      ErrorChk, _
                                                      TxID)

            lblStckTxID.Text = TxID
            If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
            End If
        End Try

        LoadStockTxDetails()
        DisplayFromDB(False)
        PageControl()
        IssueTypeControl()
        BindGrid()

    End Sub
    Sub btnDebitNote_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim strDocTypeId As String = objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.BIDebitNote) & "|" & objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.BIDebitNoteLn)
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError
        Dim strEmpCode As String = Request.Form("lstEmpID")
        Dim strBillParty As String = Request.Form("lstBillParty")

        Select Case IssueType.Text
            Case objCTtx.EnumStockIssueType.StaffDN
                strEmpCode = IIf(strEmpCode = "", lstEmpID.SelectedItem.Value, strEmpCode)
                StrTxParam = lblStckTxID.Text & "|||||||" & strEmpCode & "||" & IssueType.Text & "|||||||" & objCTtx.EnumStockIssueStatus.DBNote
            Case Else
                strBillParty = IIf(strBillParty = "", lstBillParty.SelectedItem.Value, strBillParty)
                StrTxParam = lblStckTxID.Text & "||||||||" & strBillParty & "|" & IssueType.Text & "|||||||" & objCTtx.EnumStockIssueStatus.DBNote
        End Select
        Try
            intErrNo = objCTtx.mtdUpdStockIssueDetail(strOpCdStckTxDet_ADD, _
                                                      strOpCdStckTxDet_UPD, _
                                                      strOpCdStckTxLine_GET, _
                                                      strCompany, _
                                                      strLocation, _
                                                      strUserId, _
                                                      strAccMonth, _
                                                      strAccYear, _
                                                      strARAccMonth, _
                                                      strARAccYear, _
                                                      StrTxParam.ToString, _
                                                      strDocTypeId, _
                                                      ErrorChk, _
                                                      TxID)

            lblStckTxID.Text = TxID
            If ErrorChk = objCTtx.EnumInventoryErrorType.OverFlow Then
                lblError.Visible = True
            End If

        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
            End If
        End Try
        LoadStockTxDetails()
        DisplayFromDB(True)
        PageControl()
        BindGrid()


    End Sub







    Sub btnCancel_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

        Try
            intErrNo = objCTtx.mtdAdjustInvItemLevel(strOpCdStckTxLine_GET, _
                                                     strOpCdItem_Details_UPD, _
                                                     strOpCdItem_Details_GET2, _
                                                     strCompany, _
                                                     strLocation, _
                                                     strUserId, _
                                                     strAccMonth, _
                                                     strAccYear, _
                                                     lblStckTxID.Text, _
                                                     objCTtx.EnumTransactionAction.Cancel, _
                                                     ErrorChk)
        Catch Exp As System.Exception
            If intErrNo <> -5 Then
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=REMOVEINVENTORYITEM&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
            End If
        End Try
        StrTxParam = lblStckTxID.Text & "||||||||||||||||" & objCTtx.EnumStockIssueStatus.Cancelled

        If intErrNo = 0 Then
            Try
                intErrNo = objCTtx.mtdUpdStockIssueDetail(strOpCdStckTxDet_ADD, _
                                                          strOpCdStckTxDet_UPD, _
                                                          strOpCdStckTxLine_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strARAccMonth, _
                                                          strARAccYear, _
                                                          StrTxParam.ToString, _
                                                          objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenIssue), _
                                                          ErrorChk, _
                                                          TxID)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CANCELSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
                End If
            End Try
        End If

        If ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            lblUnDel.Visible = True
        End If
        LoadStockTxDetails()
        DisplayFromDB(False)
        PageControl()
        BindGrid()

    End Sub

    Sub btnDelete_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim StrTxParam As String
        Dim TxID As String
        Dim ErrorChk As Integer = objCTtx.EnumInventoryErrorType.NoError

        If lblStatusHid.Text = CStr(objCTtx.EnumStockIssueStatus.Deleted) Then
            Try
                intErrNo = objCTtx.mtdAdjustInvItemLevel(strOpCdStckTxLine_GET, _
                                                         strOpCdItem_Details_UPD, _
                                                         strOpCdItem_Details_GET2, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         lblStckTxID.Text, _
                                                         objCTtx.EnumTransactionAction.Undelete, _
                                                         ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UNDELETESTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "||||||||||||||||" & objCTtx.EnumStockIssueStatus.Active
        Else
            Try
                intErrNo = objCTtx.mtdAdjustInvItemLevel(strOpCdStckTxLine_GET, _
                                                         strOpCdItem_Details_UPD, _
                                                         strOpCdItem_Details_GET2, _
                                                         strCompany, _
                                                         strLocation, _
                                                         strUserId, _
                                                         strAccMonth, _
                                                         strAccYear, _
                                                         lblStckTxID.Text, _
                                                         objCTtx.EnumTransactionAction.Delete, _
                                                         ErrorChk)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=DELETENEWSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
                End If
            End Try
            StrTxParam = lblStckTxID.Text & "||||||||||||||||" & objCTtx.EnumStockIssueStatus.Deleted
        End If

        If intErrNo = 0 And Not ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            Try
                intErrNo = objCTtx.mtdUpdStockIssueDetail(strOpCdStckTxDet_ADD, _
                                                          strOpCdStckTxDet_UPD, _
                                                          strOpCdStckTxLine_GET, _
                                                          strCompany, _
                                                          strLocation, _
                                                          strUserId, _
                                                          strAccMonth, _
                                                          strAccYear, _
                                                          strARAccMonth, _
                                                          strARAccYear, _
                                                          StrTxParam.ToString, _
                                                          objGlobal.mtdGetDocPrefix(objGlobal.EnumDocType.CanteenIssue), _
                                                          ErrorChk, _
                                                          TxID)
            Catch Exp As System.Exception
                If intErrNo <> -5 Then
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ADDNEWSTOCKISSUE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/CT_Trx_StockIssue_List.aspx")
                End If
            End Try
        End If

        If ErrorChk = objCTtx.EnumInventoryErrorType.InsufficientQty Then
            lblUnDel.Visible = True
        End If
        LoadStockTxDetails()
        DisplayFromDB(False)
        PageControl()
        BindGrid()

    End Sub

    Sub btnPreview_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strUpdString As String = ""
        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strStockTxId As String
        Dim strIssueType As String
        Dim strDisplayCost As String = "0"

        Dim AccountTag As String
        Dim BlockTag As String
        Dim VehicleTag As String
        Dim VehExpenseTag As String

        AccountTag = AccTag
        BlockTag = BlkTag
        VehicleTag = VehTag
        VehExpenseTag = VehExpTag

        strStockTxId = Trim(lblStckTxID.Text)
        strUpdString = "where StockIssueID = '" & strStockTxId & "'"
        strStatus = Trim(Status.Text)
        intStatus = CInt(Trim(lblStatusHid.Text))
        strPrintDate = Trim(lblPrintDate.Text)
        strIssueType = Trim(IssueType.Text)
        strTable = "CT_STOCKISSUE"
        strSortLine = ""

        If cblDisplayCost.Items(0).Selected Then
            strDisplayCost = "1"
        End If

        If intStatus = objCTtx.EnumStockIssueStatus.Confirmed Or intStatus = objCTtx.EnumStockIssueStatus.DbNote Then
            If strPrintDate = "" Then
                Try
                    intErrNo = objAdmin.mtdUpdPrintDate(strOpCodePrint, _
                                                        strUpdString, _
                                                        strTable, _
                                                        strCompany, _
                                                        strLocation, _
                                                        strUserId)
                Catch Exp As System.Exception
                    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CT_TRX_STOCKISSUEDETAIL_UPD_PRINTDATE&errmesg=" & lblErrMessage.Text & "&redirect=CT/Trx/ct_trx_stockissue_List.aspx")
                End Try
                LoadStockTxDetails()
                DisplayFromDB(False)
                PageControl()
            Else
                strStatus = strStatus & " (re-printed)"
            End If
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CT_Rpt_StockIssueDet.aspx?strStockIssueId=" & strStockTxId & _
                       "&strPrintDate=" & strPrintDate & _
                       "&strStatus=" & strStatus & _
                       "&strDisplayCost=" & strDisplayCost & _
                       "&strIssueType=" & strIssueType & _
                       "&strSortLine=" & strSortLine & _
                       "&AccountTag=" & AccountTag & _
                       "&BlockTag=" & BlockTag & _
                       "&VehicleTag=" & VehicleTag & _
                       "&VehExpenseTag=" & VehExpenseTag & _
                       """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")
    End Sub

    Sub btnNew_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim intIssueType As Integer = CInt(IssueType.Text)
        Dim strIssueType As String
        Select Case intIssueType
            Case objCTtx.EnumStockIssueType.OwnUse
                strIssueType = "Issue"
            Case objCTtx.EnumStockIssueType.StaffPayroll
                strIssueType = "Staff"
            Case objCTtx.EnumStockIssueType.External
                strIssueType = "External"
            Case Else
                strIssueType = "Issue"
        End Select
        Response.Redirect("CT_Trx_StockIssue_Details.aspx?isType=" & strIssueType)
    End Sub

    Sub btnBack_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("../../CT/Trx/CT_Trx_StockIssue_List.aspx")
    End Sub

End Class
