
Imports System
Imports System.Data
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Text.RegularExpressions
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction


Public Class CM_Trx_ContractMatchDet : Inherits Page

    Protected WithEvents dgDispatchDet As DataGrid
    Protected WithEvents dgContractDet As DataGrid
    Protected WithEvents lblLastMatchingId As Label
    Protected WithEvents lblMatchingId As Label
    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents ddlBuyer As DropDownList
    Protected WithEvents lblBDQty As Label
    Protected WithEvents lblDispatchQty As Label
    Protected WithEvents lblTotalMatchedQty As Label
    Protected WithEvents lblMatchedQty As Label
    Protected WithEvents lblCFQty As Label
    Protected WithEvents lblPeriod As Label
    Protected WithEvents lblStatus As Label
    Protected WithEvents lblDateCreated As Label
    Protected WithEvents lblLastUpdate As Label
    Protected WithEvents lblUpdatedBy As Label

    Protected WithEvents tbcode As HtmlInputHidden
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents DelBtn As ImageButton
    Protected WithEvents GenInvoiceBtn As ImageButton

    Protected WithEvents lblErrMessage As Label
    Protected WithEvents lblHiddenSts As Label
    Protected WithEvents lblHasDispatch As Label
    Protected WithEvents lblShowGenInvoice As Label
    Protected WithEvents lblParamTicketNo As Label
    Protected WithEvents lblParamContractNo As Label
    Protected WithEvents lblParamPrice As Label
    Protected WithEvents lblParamMatchedQty As Label
    Protected WithEvents lblParamPrevMatchedQty As Label
    Protected WithEvents lblBillParty As Label
    Protected WithEvents rfvBuyer As RequiredFieldValidator
    Protected WithEvents lblPleaseSelect As Label
    Protected WithEvents lblSelect As Label
    Protected WithEvents lblCode As Label

    Protected WithEvents lblDONo As Label
    Protected WithEvents ddlDONo As DropDownList
    Dim objDataSet as New Object()

    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objCMTrx As New agri.CM.clsTrx()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objWMTrx As New agri.WM.clsTrx()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
	Dim objGLTrx As New agri.GL.ClsTrx()
	

    Dim objMatchDs As New Object()
    Dim objCheckDs As New Object()
    Dim objBuyerDs As New Object()
    Dim objBDQtyDs As New Object()
    Dim objDispQtyDs As New Object()
    Dim objDispatchDs As New Object()
    Dim objContractDs As New Object()
    Dim objLangCapDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer

    Dim strMatchingId As String = ""
    Dim intStatus As Integer
    Dim intMaxLen As Integer = 0

	
	Protected WithEvents txtInvoiceDate As TextBox
	Protected WithEvents lblFmt As Label
    Protected WithEvents lblDate As Label
	Protected WithEvents lblDateError As Label
	
	Dim strParamName As String
	Dim strParamValue As String

    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim strLocType as String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_ARACCMONTH")
        strAccYear = Session("SS_ARACCYEAR")
        intCMAR = Session("SS_CMAR")
        strLocType = Session("SS_LOCTYPE")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMContractMatching), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            strMatchingId = Trim(IIf(Request.QueryString("tbcode") <> "", Request.QueryString("tbcode"), Request.Form("tbcode")))
            intStatus = CInt(lblHiddenSts.Text)
            If Not IsPostBack Then
                If strMatchingId <> "" Then
                    tbcode.Value = strMatchingId
                    onLoad_Display()
                    BindDispatchDetail()
                    BindContractDetail()
                    onLoad_BindButton()
                Else
                    lblPeriod.Text = strAccMonth & "/" & strAccYear
                    lblBDQty.Text = 0
                    lblDispatchQty.Text = 0
                    lblTotalMatchedQty.Text = 0
                    lblMatchedQty.Text = 0
                    lblCFQty.Text = 0
                    BindProductList("")
                    BindBuyerList("")
                    BindDONo("", "", "")
                    onLoad_BindButton()
                End If
            End If
        End If
        lblDoNo.Text = "Delivery Order No. :"
    End Sub


    Sub BindProductList(ByVal pv_strProdCode As String)
        Dim intCnt As Integer
        If ddlProduct.Items.Count = 0 Then
            ddlProduct.Items.Add(New ListItem("Select Product", ""))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.FFB), objWMTrx.EnumWeighBridgeTicketProduct.FFB))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
        End If

        If Trim(pv_strProdCode) <> "" Then
            For intCnt = 0 To ddlProduct.Items.Count - 1
                If ddlProduct.Items(intCnt).Value = pv_strProdCode Then
                    Exit For
                End If
            Next
            ddlProduct.SelectedIndex = intCnt
        Else
            ddlProduct.SelectedIndex = 0
        End If

    End Sub


    Sub BindBuyerList(ByVal pv_strBuyerCode As String)
        Dim strParam As String
        Dim strOpCdGet As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        Dim strBuyerCode as string
        Dim strProductCode as string

        strParam = "" & "|" & _
                   "" & "|" & _
                   objGLSetup.EnumBillPartyStatus.Active & "|" & _
                   "" & "|" & _
                   "BP.BillPartyCode" & "|" & _
                   "ASC" & "|"
        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCdGet, strParam, objBuyerDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_BUYERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractMatchList.aspx")
        End Try

        If objBuyerDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objBuyerDs.Tables(0).Rows.Count - 1
                objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode"))
                objBuyerDs.Tables(0).Rows(intCnt).Item("Name") = objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") & " (" & Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
                If objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = pv_strBuyerCode Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objBuyerDs.Tables(0).NewRow()
        dr("BillPartyCode") = ""
        dr("Name") = lblSelect.Text & lblBillParty.Text
        objBuyerDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBuyer.DataSource = objBuyerDs.Tables(0)
        ddlBuyer.DataValueField = "BillPartyCode"
        ddlBuyer.DataTextField = "Name"
        ddlBuyer.DataBind()
        ddlBuyer.SelectedIndex = intSelectedIndex


    End Sub

    Sub onChange_BDQty(ByVal pv_strProductCode As String, ByVal pv_strBuyerCode As String, ByRef pr_decBDQty As Decimal)
        Dim strOpCd As String = "CM_CLSTRX_CONTRACT_MATCH_BDQTY_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim strSort As String

        strSearch = "where LocCode = '" & strLocation & _
                    "' and ProductCode = '" & pv_strProductCode & _
                    "' and BuyerCode = '" & pv_strBuyerCode & _
                    "' and Status in ('" & objCMTrx.EnumContractMatchStatus.Active & "', '" & objCMTrx.EnumContractMatchStatus.Confirmed & "') "

        strSort = "order by MatchingId desc "

        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd, strParam, 0, objBDQtyDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_BDQTY_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try

        If objBDQtyDs.Tables(0).Rows.Count > 0 Then
            pr_decBDQty = objBDQtyDs.Tables(0).Rows(0).Item("CFQty")
            lblLastMatchingId.Text = Trim(objBDQtyDs.Tables(0).Rows(0).Item("MatchingId"))
        End If
    End Sub

    Sub onChange_DispatchQty(ByVal pv_strProductCode As String, ByVal pv_strBuyerCode As String, ByVal pv_strTicketNo As String, ByRef pr_decDispQty As Decimal, ByVal pv_strDONo as String)
        Dim strOpCd As String = "CM_CLSTRX_CONTRACT_MATCH_DISPATCHQTY_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim strSort As String

        strSearch = "where LocCode = '" & strLocation & "' " & _
                    "and Status = '" & objWMTrx.EnumWeighBridgeTicketStatus.Active & "' " & _
                    "and BuyerNetWeight > 0 " & _
                    "and ProductCode = '" & pv_strProductCode & "' " & _
                    "and CustomerCode ='" & pv_strBuyerCode & "' " & _
                    "and DeliveryNoteNo ='" & pv_strDONo & "' "
        


        If pv_strTicketNo = "all" Then
            strSearch = strSearch & "and MatchingId = '' "
        Else
            strSearch = strSearch & "and TicketNo in ('" & Trim(pv_strTicketNo) & "') "
        End If

        strSort = ""
        strParam = strSearch & "|" & strSort

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd, strParam, 0, objDispQtyDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_DISPQTY_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try

        If objDispQtyDs.Tables(0).Rows.Count > 0 Then
            pr_decDispQty = objDispQtyDs.Tables(0).Rows(0).Item("DispatchQty")
        End If
    End Sub

    Sub onChange_TotalMatchedQty(ByVal pv_decBDQty As Decimal, ByVal pv_decDispatchQty As Decimal, ByRef pr_decTotalMatchedQty As Decimal)
        Dim decTotalMatchedQty As Decimal

        decTotalMatchedQty = pv_decBDQty + pv_decDispatchQty
        pr_decTotalMatchedQty = decTotalMatchedQty
    End Sub

    Sub onChange_CFQty(ByVal pv_decTotalMatchedQty As Decimal, ByVal pv_decMatchedQty As Decimal, ByRef pr_decCFQty As Decimal)
        Dim decCFQty As Decimal

        decCFQty = pv_decTotalMatchedQty - pv_decMatchedQty
        pr_decCFQty = decCFQty
    End Sub

    Sub onChange_Qty(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strProductCode As String
        Dim strBuyerCode As String
        Dim strTicketNo As String = "all"
        Dim decBDQty As Decimal
        Dim decDispQty As Decimal
        Dim decTotalMatchedQty As Decimal
        Dim decMatchedQty As Decimal
        Dim decCFQty As Decimal

        Dim strDONo as String

        strProductCode = ddlProduct.SelectedItem.Value
        strBuyerCode = ddlBuyer.SelectedItem.Value

        strDONo = ddlDONo.SelectedItem.Value

        onChange_BDQty(strProductCode, strBuyerCode, decBDQty)
        onChange_DispatchQty(strProductCode, strBuyerCode, strTicketNo, decDispQty, strDONo)
        onChange_TotalMatchedQty(decBDQty, decDispQty, decTotalMatchedQty)

        decMatchedQty = CDbl(lblMatchedQty.Text)
        onChange_CFQty(decTotalMatchedQty, decMatchedQty, decCFQty)

        lblBDQty.Text = decBDQty
        lblDispatchQty.Text = decDispQty
        lblTotalMatchedQty.Text = decTotalMatchedQty
        lblCFQty.Text = decCFQty

        BindDispatchDetail()
        BindContractDetail()

        BindProductList(strProductCode)
        BindBuyerList(strBuyerCode)
        BindDONo("", strProductCode, strBuyerCode)

    End Sub


    Sub onChecked_Match(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strMatchExp As String
        Dim intCnt As Integer
        Dim cbx As CheckBox
        Dim lbl As Label
        Dim strProductCode As String
        Dim strBuyerCode As String
        Dim decBDQty As Decimal
        Dim decDispQty As Decimal
        Dim decTotalMatchedQty As Decimal
        Dim decMatchedQty As Decimal
        Dim decCFQty As Decimal

        Dim strDONo as String 


        strProductCode = ddlProduct.SelectedItem.Value
        strBuyerCode = ddlBuyer.SelectedItem.Value
        strDONo = ddlDONo.SelectedItem.Value

        For intCnt = 0 To dgDispatchDet.Items.Count - 1
            cbx = dgDispatchDet.Items.Item(intCnt).FindControl("cbMatch")
            If cbx.Checked = True Then
                lbl = dgDispatchDet.Items.Item(intCnt).FindControl("lblTicketNo")
                strMatchExp = strMatchExp & "','" & Trim(lbl.Text)
            End If
        Next

        If InStr(strMatchExp, "','") <> 0 Then
            strMatchExp = Right(strMatchExp, Len(strMatchExp) - 3)
            lblParamTicketNo.Text = Replace(strMatchExp, "','", "|")
        Else
            lblParamTicketNo.Text = strMatchExp
        End If

        decBDQty = CDbl(lblBDQty.Text)
        onChange_DispatchQty(strProductCode, strBuyerCode, strMatchExp, decDispQty, strDONo)
        onChange_TotalMatchedQty(decBDQty, decDispQty, decTotalMatchedQty)

        decMatchedQty = CDbl(lblMatchedQty.Text)

        onChange_CFQty(decTotalMatchedQty, decMatchedQty, decCFQty)

        lblBDQty.Text = decBDQty
        lblDispatchQty.Text = decDispQty
        lblTotalMatchedQty.Text = decTotalMatchedQty
        lblCFQty.Text = decCFQty

        BindProductList(strProductCode)
        BindBuyerList(strBuyerCode)

    End Sub

    Sub txtChange_MatchedQty(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strProductCode As String
        Dim strBuyerCode As String
        Dim intCnt As Integer
        Dim txt As TextBox
        Dim lbl As Label
        Dim decTotalMatchedQty As Decimal
        Dim decMatchedQty As Decimal
        Dim decPrevMatchedQty As Decimal
        Dim decNewMatchedQty As Decimal = 0
        Dim decCFQty As Decimal
        Dim decContractQty As Decimal
        Dim decContractBalQty As Decimal
        Dim decExtraQty As Decimal
        Dim intExtraQtyType As Integer
        Dim blnValidate As Boolean
        Dim strContractNoExp As String = ""
        Dim strPriceExp As String = ""
        Dim strMatchedQtyExp As String = ""
        Dim strPrevMatchedQtyExp As String = ""
        Dim oRegEx As Regex = New Regex("^\d{1,15}(\.\d{1,5}|\.|)$")
        Dim blnValid As Boolean
        Dim decLatestBalQty As Boolean

        strProductCode = ddlProduct.SelectedItem.Value
        strBuyerCode = ddlBuyer.SelectedItem.Value

        For intCnt = 0 To dgContractDet.Items.Count - 1
            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblContractNo")
            strContractNoExp = strContractNoExp & "|" & lbl.Text

            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblPrice")
            strPriceExp = strPriceExp & "|" & lbl.Text


            txt = dgContractDet.Items.Item(intCnt).FindControl("txtMatchedQty")
            blnValid = oRegEx.IsMatch(txt.Text)


            If blnValid Then
                decMatchedQty = CDbl(txt.Text)
                strMatchedQtyExp = strMatchedQtyExp & "|" & decMatchedQty

                lbl = dgContractDet.Items.Item(intCnt).FindControl("lblErrMatchedQtyFmt")
                lbl.Visible = False
            Else
                lbl = dgContractDet.Items.Item(intCnt).FindControl("lblErrMatchedQtyFmt")
                lbl.Visible = True
                Exit Sub
            End If


            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblContractQty")
            decContractQty = CDbl(lbl.Text)

            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblContractBalQty")
            decContractBalQty = CDbl(lbl.Text)

            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblExtraQty")
            decExtraQty = CDbl(lbl.Text)

            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblExtraQtyType")
            intExtraQtyType = CInt(lbl.Text)

            Validate_ContractMatchedQty(decContractQty, _
                                        decContractBalQty, _
                                        decExtraQty, _
                                        intExtraQtyType, _
                                        decPrevMatchedQty, _
                                        decMatchedQty, _
                                        blnValidate)

            If blnValidate = True Then
                lbl = dgContractDet.Items.Item(intCnt).FindControl("lblErrMatchedQty")
                lbl.Visible = False

                decNewMatchedQty = decNewMatchedQty + decMatchedQty
            Else
                lbl = dgContractDet.Items.Item(intCnt).FindControl("lblErrMatchedQty")
                lbl.Visible = True
                Exit Sub
            End If

        Next

        lblParamContractNo.Text = strContractNoExp
        lblParamPrice.Text = strPriceExp
        lblParamMatchedQty.Text = strMatchedQtyExp
        lblParamPrevMatchedQty.Text = strPrevMatchedQtyExp

        decTotalMatchedQty = CDbl(lblTotalMatchedQty.Text)
        onChange_CFQty(decTotalMatchedQty, decNewMatchedQty, decCFQty)

        lblMatchedQty.Text = decNewMatchedQty
        lblCFQty.Text = decCFQty

        BindProductList(strProductCode)
        BindBuyerList(strBuyerCode)

    End Sub

    Sub onLoad_Display()
        Dim strOpCd As String = "CM_CLSTRX_CONTRACT_MATCH_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim strTotalMatchedQty As String

        strSearch = "and ma.LocCode = '" & strLocation & "' and ma.MatchingId = '" & strMatchingId & "' "
        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd, strParam, 0, objMatchDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try

        If objMatchDs.Tables(0).Rows.Count > 0 Then
            lblMatchingId.Text = strMatchingId
            BindProductList(Trim(objMatchDs.Tables(0).Rows(0).Item("ProductCode")))
            BindBuyerList(Trim(objMatchDs.Tables(0).Rows(0).Item("BuyerCode")))
            lblBDQty.Text = ObjGlobal.GetIDDecimalSeparator_FreeDigit(Trim(objMatchDs.Tables(0).Rows(0).Item("BDQty")),5)
            lblDispatchQty.Text = ObjGlobal.GetIDDecimalSeparator_FreeDigit(Trim(objMatchDs.Tables(0).Rows(0).Item("DispatchQty")),5)
            strTotalMatchedQty = CDbl(Trim(objMatchDs.Tables(0).Rows(0).Item("BDQty"))) + CDbl(Trim(objMatchDs.Tables(0).Rows(0).Item("DispatchQty")))
            lblTotalMatchedQty.Text = ObjGlobal.GetIDDecimalSeparator_FreeDigit(strTotalMatchedQty,5)
            lblMatchedQty.Text = ObjGlobal.GetIDDecimalSeparator_FreeDigit(Trim(objMatchDs.Tables(0).Rows(0).Item("MatchedQty")),5)
            lblCFQty.Text = ObjGlobal.GetIDDecimalSeparator_FreeDigit(Trim(objMatchDs.Tables(0).Rows(0).Item("CFQty")),5)
            lblPeriod.Text = Trim(objMatchDs.Tables(0).Rows(0).Item("AccMonth")) & "/" & Trim(objMatchDs.Tables(0).Rows(0).Item("AccYear"))
            intStatus = CInt(Trim(objMatchDs.Tables(0).Rows(0).Item("Status")))
            lblHiddenSts.Text = Trim(objMatchDs.Tables(0).Rows(0).Item("Status"))
            lblStatus.Text = objCMTrx.mtdGetContractMatchStatus(Trim(objMatchDs.Tables(0).Rows(0).Item("Status")))
            lblDateCreated.Text = objGlobal.GetLongDate(objMatchDs.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objMatchDs.Tables(0).Rows(0).Item("UpdateDate"))
            lblUpdatedBy.Text = Trim(objMatchDs.Tables(0).Rows(0).Item("UserName"))

            BindDONo(Trim(objMatchDs.Tables(0).Rows(0).Item("DONo")), Trim(objMatchDs.Tables(0).Rows(0).Item("ProductCode")), Trim(objMatchDs.Tables(0).Rows(0).Item("BuyerCode")))
        End If
    End Sub

    Protected Function onLoad_DispatchDetail() As DataSet
        Dim strOpCd As String = "CM_CLSTRX_CONTRACT_MATCH_DISPATCHDET_GET"
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim intCnt As Integer
        Dim cbx As CheckBox

        If strMatchingId = "" Then
            strSearch = "where LocCode = '" & strLocation & "' " & _
                        "and Status = '" & objWMTrx.EnumWeighBridgeTicketStatus.Active & "' " & _
                        "and MatchingId = '' " & _
                        "and BuyerNetWeight > 0 " & _
                        "and ProductCode = '" & ddlProduct.SelectedItem.Value & "' " & _
                        "and CustomerCode ='" & ddlBuyer.SelectedItem.Value & "' "
        Else
            strSearch = "where LocCode = '" & strLocation & "' " & _
                        "and Status = '" & objWMTrx.EnumWeighBridgeTicketStatus.Active & "' " & _
                        "and MatchingId = '' " & _
                        "and BuyerNetWeight > 0 " & _
                        "and ProductCode = '" & ddlProduct.SelectedItem.Value & "' " & _
                        "and CustomerCode ='" & ddlBuyer.SelectedItem.Value & "' " & _
                        "or (LocCode = '" & strLocation & "' and MatchingId = '" & strMatchingId & "') "

        End If

        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd, strParam, 0, objDispatchDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_DISPATCHDETAIL_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try
        Return objDispatchDs
    End Function

    Sub BindDispatchDetail()
        Dim cbx As CheckBox
        Dim intCnt As Integer
        Dim cbMatchId As String
        Dim lbl As Label
        Dim strTicketExp As String = ""

        dgDispatchDet.DataSource = onLoad_DispatchDetail()
        dgDispatchDet.DataBind()

        If dgDispatchDet.Items.Count > 0 Then
            lblHasDispatch.Text = "yes"
        End If

        If strMatchingId = "" Then
            For intCnt = 0 To dgDispatchDet.Items.Count - 1
                cbx = dgDispatchDet.Items.Item(intCnt).FindControl("cbMatch")
                cbx.Checked = True
                lbl = dgDispatchDet.Items.Item(intCnt).FindControl("lblTicketNo")
                strTicketExp = strTicketExp & "|" & lbl.Text
            Next
        Else
            For intCnt = 0 To dgDispatchDet.Items.Count - 1
                lbl = dgDispatchDet.Items.Item(intCnt).FindControl("lblMatchId")
                cbMatchId = lbl.Text
                If Trim(cbMatchId) = Trim(strMatchingId) Then
                    cbx = dgDispatchDet.Items.Item(intCnt).FindControl("cbMatch")
                    cbx.Checked = True
                    lbl = dgDispatchDet.Items.Item(intCnt).FindControl("lblTicketNo")
                    strTicketExp = strTicketExp & "|" & lbl.Text
                End If
                If intStatus = objCMTrx.EnumContractMatchStatus.Confirmed Or intStatus = objCMTrx.EnumContractMatchStatus.Deleted Then
                    cbx = dgDispatchDet.Items.Item(intCnt).FindControl("cbMatch")
                    cbx.Enabled = False
                End If
            Next
        End If

        If InStr(strTicketExp, "|") <> 0 Then
            strTicketExp = Right(strTicketExp, Len(strTicketExp) - 1)
        End If
        lblParamTicketNo.Text = strTicketExp

    End Sub

    Protected Function onLoad_ContractDetail() As DataSet
        Dim strOpCd As String
        Dim strParam As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim strSearch_1 As String
        Dim strSearch_2 As String
        Dim strSort As String
        Dim intCnt As Integer
        Dim txt As TextBox
        Dim strDelMonth As String

        If strMatchingId = "" Then
            strOpCd = "CM_CLSTRX_CONTRACTMATCH_LINE_GET_NEW"
            strSearch = "where LocCode = '" & strLocation & "' " & _
                        "and ProductCode = '" & ddlProduct.SelectedItem.Value & "' " & _
                        "and BuyerCode = '" & ddlBuyer.SelectedItem.Value & "' " & _
                        "and Status = '" & objCMTrx.EnumContractStatus.Active & "' " & _
                        "and ContractType = '" & objCMTrx.EnumContractType.Sales & "' " & _
                        "and (ContractQty - MatchedQty > 0) "
			
			strSort = "ORDER BY DelYear, DelMonth, ContractDate"
			
        Else
            If intStatus = objCMTrx.EnumContractMatchStatus.Deleted Or intStatus = objCMTrx.EnumContractMatchStatus.Confirmed Then
                strOpCd = "CM_CLSTRX_CONTRACTMATCH_LN_GET"
                strSearch = "and ctr.LocCode = '" & strLocation & "' " & _
                            "and ln.MatchingId = '" & strMatchingId & "' "
				
				strSort = "ORDER BY ctr.DelYear, ctr.DelMonth, ctr.ContractDate"
				
            Else
                strOpCd = "CM_CLSTRX_CONTRACTMATCH_LINE_GET_UPD"
                strSearch = "and ln.MatchingId = '" & strMatchingId & "' "
                strSearch_1 = "and ln.MatchingId = '" & strMatchingId & "' "
                strSearch_2 = "where LocCode = '" & strLocation & "' " & _
                            "and ProductCode = '" & ddlProduct.SelectedItem.Value & "' " & _
                            "and BuyerCode = '" & ddlBuyer.SelectedItem.Value & "' " & _
                            "and Status = '" & objCMTrx.EnumContractStatus.Active & "' " & _
                            "and (ContractQty - MatchedQty > 0) "
				
				strSort = "ORDER BY DelYear, DelMonth, ContractDate"
				
            End If
        End If

        strParam = strSearch & "|" & strSearch_1 & "|" & strSort & "|" & strSearch_2

        Try
            intErrNo = objCMTrx.mtdGetContractMatchLn(strOpCd, strParam, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_CONTRACTDETAIL_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try
        Return objContractDs
    End Function

    Sub BindContractDetail()
        Dim intCnt As Integer
        Dim txt As TextBox
        Dim lbl As Label
        Dim strContractNoExp As String
        Dim strPriceExp As String
        Dim strPrevMatchedQtyExp As String
        Dim strMatchedQtyExp As String
        Dim strInvoiceNo As String = ""
        Dim decTotalMatchedQty As Decimal = 0

        dgContractDet.DataSource = onLoad_ContractDetail()
        dgContractDet.DataBind()

        If intStatus = objCMTrx.EnumContractMatchStatus.Confirmed Or intStatus = objCMTrx.EnumContractMatchStatus.Deleted Then
            For intCnt = 0 To dgContractDet.Items.Count - 1
                txt = dgContractDet.Items.Item(intCnt).FindControl("txtMatchedQty")
                txt.Enabled = False

				If intStatus = objCMTrx.EnumContractMatchStatus.Confirmed Then
					txt = dgContractDet.Items.Item(intCnt).FindControl("txtInvoiceDate")
					txt.Enabled = True
				End If
            Next
        End If

        For intCnt = 0 To dgContractDet.Items.Count - 1
            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblContractNo")
            strContractNoExp = strContractNoExp & "|" & lbl.Text

            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblPrice")
            strPriceExp = strPriceExp & "|" & lbl.Text


            txt = dgContractDet.Items.Item(intCnt).FindControl("txtMatchedQty")
            strMatchedQtyExp = strMatchedQtyExp & "|" & lbl.Text
            decTotalMatchedQty = decTotalMatchedQty + CDbl(lbl.Text)

            lbl = dgContractDet.Items.Item(intCnt).FindControl("lblInvoiceNo")
            strInvoiceNo = strInvoiceNo & lbl.Text
        Next


        If Trim(strInvoiceNo) = "" And decTotalMatchedQty > 0 Then
            lblShowGenInvoice.Text = "yes"
        End If

        lblParamContractNo.Text = strContractNoExp
        lblParamPrice.Text = strPriceExp
        lblParamMatchedQty.Text = strMatchedQtyExp
        lblParamPrevMatchedQty.Text = strPrevMatchedQtyExp


    End Sub

    Sub onLoad_BindButton()
        ddlProduct.Enabled = False
        ddlBuyer.Enabled = False
        SaveBtn.Visible = False
        DelBtn.Visible = False
        GenInvoiceBtn.Visible = False

        ddlDONo.Enabled = False

        Select Case intStatus
            Case objCMTrx.EnumContractMatchStatus.Active
                SaveBtn.Visible = True
                DelBtn.Visible = True
                DelBtn.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
                If Trim(lblShowGenInvoice.Text) = "yes" Then
                    GenInvoiceBtn.Visible = True
                End If
            Case objCMTrx.EnumContractMatchStatus.Deleted
            Case objCMTrx.EnumContractMatchStatus.Confirmed
                If Trim(lblShowGenInvoice.Text) = "yes" Then
                    GenInvoiceBtn.Visible = True
                End If
				SaveBtn.Visible = True 
            Case Else
                ddlProduct.Enabled = True
                ddlBuyer.Enabled = True
                SaveBtn.Visible = True
                ddlDONo.Enabled = True

        End Select

    End Sub

    Sub Validate_ContractMatchedQty(ByVal decContractQty As Decimal, ByVal decBalanceQty As Decimal, _
                                    ByVal decExtraQty As Decimal, ByVal intExtraQtyType As Integer, _
                                    ByVal decPrevMatchedQty As Decimal, ByVal decMatchedQty As Decimal, _
                                    ByRef blnValidate As Boolean)

        Dim decExtraBalQty As Decimal
        Dim decTotalBalQty As Decimal

        If intExtraQtyType = objCMTrx.EnumExtraQtyType.Percentage Then
            decTotalBalQty = decBalanceQty + (decContractQty * decExtraQty / 100)
        Else
            decTotalBalQty = decBalanceQty + decExtraQty
        End If

        decTotalBalQty = decTotalBalQty + decPrevMatchedQty - decMatchedQty

        If decTotalBalQty < 0 Then
            blnValidate = False
        Else
            blnValidate = True
        End If
    End Sub


    Sub Button_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_Add As String = "CM_CLSTRX_CONTRACTMATCH_ADD"
        Dim strOpCd_AddLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_ADD"
        Dim strOpCd_Upd As String = "CM_CLSTRX_CONTRACTMATCH_UPD"
        Dim strOpCd_DelLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_DEL"
        Dim strOpCd_UpdContract As String = "CM_CLSTRX_CONTRACT_REG_UPD"
        Dim strOpCd_UpdTicket As String = "CM_CLSTRX_CONTRACTMATCH_UPDTICKET"
        Dim strOpCd_GetMatchLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_GET"
        Dim strOpCd_UpdMatchLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_UPD"

        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String
        Dim strParamContract As String
        Dim strParamTicket As String

        Dim strProductCode As String
        Dim strBuyerCode As String
        Dim decBDQty As Decimal
        Dim decDispatchQty As Decimal
        Dim decMatchedQty As Decimal
        Dim decCFQty As Decimal
        Dim MatchAccMonth As String
        Dim MatchAccYear As String
        Dim arrPeriod As Array
        Dim pr_strMatchingId As String
        Dim strLastMatchingId As String

		
		Dim intCnt As Integer
		Dim lblContractNo As Label
		Dim txtInvoiceDate As TextBox
		Dim strUpdString As String
		Dim strInvoiceDate As String
		Dim strOpCdUpdInvoiceDate As String = "CM_CLSTRX_CONTRACTMATCH_LN_UPD"
		Dim lblInvoiceID As Label
		

        Dim strDONo as String 

        strMatchingId = lblMatchingId.Text
        strProductCode = Trim(ddlProduct.SelectedItem.Value)
        strBuyerCode = Trim(ddlBuyer.SelectedItem.Value)
        decBDQty = lblBDQty.Text
        decDispatchQty = lblDispatchQty.Text
        decMatchedQty = lblMatchedQty.Text
        decCFQty = lblCFQty.Text
        arrPeriod = Split(lblPeriod.Text, "/")
        MatchAccMonth = arrPeriod(0)
        MatchAccYear = arrPeriod(1)
        strLastMatchingId = lblLastMatchingId.Text
        strDONo = Trim(ddlDONo.SelectedItem.Value)

        If InStr(lblParamContractNo.Text, "|") <> 0 Then
            lblParamContractNo.Text = Right(lblParamContractNo.Text, Len(lblParamContractNo.Text) - 1)
        End If

        If InStr(lblParamPrice.Text, "|") <> 0 Then
            lblParamPrice.Text = Right(lblParamPrice.Text, Len(lblParamPrice.Text) - 1)
        End If

        If InStr(lblParamMatchedQty.Text, "|") <> 0 Then
            lblParamMatchedQty.Text = Right(lblParamMatchedQty.Text, Len(lblParamMatchedQty.Text) - 1)
        End If

        If InStr(lblParamPrevMatchedQty.Text, "|") <> 0 Then
            lblParamPrevMatchedQty.Text = Right(lblParamPrevMatchedQty.Text, Len(lblParamPrevMatchedQty.Text) - 1)
        End If

        lblParamContractNo.Text = Replace(lblParamContractNo.Text, "|", Chr(9))
        lblParamPrice.Text = Replace(lblParamPrice.Text, "|", Chr(9))
        lblParamMatchedQty.Text = Replace(lblParamMatchedQty.Text, "|", Chr(9))
        lblParamPrevMatchedQty.Text = Replace(lblParamPrevMatchedQty.Text, "|", Chr(9))

        strParamContract = Trim(lblParamContractNo.Text) & "|" & _
                           Trim(lblParamPrice.Text) & "|" & _
                           Trim(lblParamMatchedQty.Text) & "|" & _
                           Trim(lblParamPrevMatchedQty.Text) & "|" & _
                           strDONo                        

        strParamTicket = Trim(lblParamTicketNo.Text)



        If strCmdArgs = "Save" Then
			
			lblDateError.Visible = False
			If intStatus = objCMTrx.EnumContractMatchStatus.Confirmed Then
				For intCnt = 0 To dgContractDet.Items.Count - 1
					lblContractNo = dgContractDet.Items.Item(intCnt).FindControl("lblContractNo")
					txtInvoiceDate = dgContractDet.Items.Item(intCnt).FindControl("txtInvoiceDate")
					If Not Trim(txtInvoiceDate.Text) = "" Then
						strInvoiceDate = CheckDate (txtInvoiceDate.Text)

						If Trim(strInvoiceDate) = "" Then
							Exit Sub
						End If
					Else
						lblDateError.Visible = True
						Exit Sub
					End If

					strUpdString = "SET InvoiceDate = '" & strInvoiceDate & "' "
					strUpdString = strUpdString & " WHERE MatchingId = '" & Trim(lblMatchingId.text) & "' "
					strUpdString = strUpdString & " AND ContractNo = '" & Trim(lblContractNo.text) & "' "

					Try
						intErrNo = objCMTrx.mtdUpdateContractMacthLnInvoiceDate (strOpCdUpdInvoiceDate, strUpdString)
					Catch Exp As System.Exception
						Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCH_INVOICE_UPD&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchDet.aspx?tbcode=" & strMatchingId)
					End Try

					
					lblInvoiceID = dgContractDet.Items.Item(intCnt).FindControl("lblInvoiceNo")

					strUpdString = lblInvoiceID.text & "|" & strInvoiceDate

					Try
						intErrNo = objCMTrx.mtdUpdateInvoiceDate ("BI_CLSTRX_INVOICE_UPD", strUpdString)
					Catch Exp As System.Exception
						Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_UPD_INVOICE_DATE&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchDet.aspx?tbcode=" & strMatchingId)
					End Try
				Next
			Else
			
				blnIsUpdate = IIf(intStatus = 0, False, True)
				tbcode.Value = strMatchingId

				strParam = strLocation & Chr(9) & _
						strMatchingId & Chr(9) & _
						strProductCode & Chr(9) & _
						strBuyerCode & Chr(9) & _
						decBDQty & Chr(9) & _
						decDispatchQty & Chr(9) & _
						decMatchedQty & Chr(9) & _
						decCFQty & Chr(9) & _
						MatchAccMonth & Chr(9) & _
						MatchAccYear & Chr(9) & _
						objCMTrx.EnumContractMatchStatus.Active & Chr(9) & _
						strLastMatchingId

				Try

					intErrNo = objCMTrx.mtdProcessContractMatch(strOpCd_Add, _
																strOpCd_AddLine, _
																strOpCd_Upd, _
																strOpCd_DelLine, _
																strOpCd_UpdContract, _
																strOpCd_UpdTicket, _
																strOpCd_GetMatchLine, _
																strOpCd_UpdMatchLine, _
																strCompany, _
																strLocation, _
																strUserId, _
																strParam, _
																strParamContract, _
																strParamTicket, _
																pr_strMatchingId, _
																blnIsUpdate)

				Catch Exp As System.Exception
					Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchDet.aspx?tbcode=" & strMatchingId)
				End Try
			End If 
        Else
            strParam = strLocation & Chr(9) & _
                       strMatchingId & Chr(9) & _
                       strProductCode & Chr(9) & _
                       strBuyerCode & Chr(9) & _
                       decBDQty & Chr(9) & _
                       decDispatchQty & Chr(9) & _
                       decMatchedQty & Chr(9) & _
                       decCFQty & Chr(9) & _
                       MatchAccMonth & Chr(9) & _
                       MatchAccYear & Chr(9) & _
                       objCMTrx.EnumContractMatchStatus.Deleted & Chr(9) & _
                       strLastMatchingId
            Try

                intErrNo = objCMTrx.mtdProcessContractMatch(strOpCd_Add, _
                                                            strOpCd_AddLine, _
                                                            strOpCd_Upd, _
                                                            strOpCd_DelLine, _
                                                            strOpCd_UpdContract, _
                                                            strOpCd_UpdTicket, _
                                                            strOpCd_GetMatchLine, _
                                                            strOpCd_UpdMatchLine, _
                                                            strCompany, _
                                                            strLocation, _
                                                            strUserId, _
                                                            strParam, _
                                                            strParamContract, _
                                                            strParamTicket, _
                                                            pr_strMatchingId, _
                                                            True)

            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_DELETE&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchDet.aspx?tbcode=" & strMatchingId)
            End Try
        End If


        If strMatchingId = "" Then
            strMatchingId = pr_strMatchingId
            tbcode.Value = strMatchingId
        End If

        onLoad_Display()
        BindDispatchDetail()
        BindContractDetail()
        onLoad_BindButton()
    End Sub

    Sub Button_GenInvoice(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_GetContract = "CM_CLSTRX_CONTRACTMATCH_LN_GET"
        Dim strOpCd_AddInv As String = "CM_CLSTRX_CONTRACTMATCH_INVOICE_ADD"
        Dim strOpCd_AddInvLn As String = "CM_CLSTRX_CONTRACTMATCH_INVOICELN_ADD"
        Dim strOpCd_UpdMatch As String = "CM_CLSTRX_CONTRACTMATCH_UPD"
        Dim strOpCd_UpdMatchLn As String = "CM_CLSTRX_CONTRACTMATCH_LN_UPD"

        Dim strProductCode As String
        Dim strBuyerCode As String
        Dim decBDQty As Decimal
        Dim decDispatchQty As Decimal
        Dim decMatchedQty As Decimal
        Dim decCFQty As Decimal
        Dim MatchAccMonth As String
        Dim MatchAccYear As String
        Dim arrPeriod As Array
        Dim strParam As String
        Dim intErrNo As Integer

        strMatchingId = lblMatchingId.Text
        strBuyerCode = ddlBuyer.SelectedItem.Value


        arrPeriod = Split(lblPeriod.Text, "/")
        MatchAccMonth = arrPeriod(0)
        MatchAccYear = arrPeriod(1)

        strParam = strMatchingId & Chr(9) & _
                   strBuyerCode & Chr(9) & _
                   MatchAccMonth & Chr(9) & _
                   MatchAccYear


        Try
            intErrNo = objCMTrx.mtdGenInvoice(strOpCd_GetContract, _
                                              strOpCd_AddInv, _
                                              strOpCd_AddInvLn, _
                                              strOpCd_UpdMatch, _
                                              strOpCd_UpdMatchLn, _
                                              strCompany, _
                                              strLocation, _
                                              strUserId, _
                                              strParam)

        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_GENINVOICE&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchDet.aspx?tbcode=" & strMatchingId)
        End Try

        lblShowGenInvoice.Text = "no"

        onLoad_Display()
        BindDispatchDetail()
        BindContractDetail()
        onLoad_BindButton()

    End Sub

    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_ContractMatchList.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
        lblBillParty.Text = GetCaption(objLangCap.EnumLangCap.BillParty) & lblCode.Text
        rfvBuyer.ErrorMessage = lblPleaseSelect.Text & lblBillParty.Text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREG_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=CM/Setup/CM_Trx_ContractRegList.aspx")
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


	
	Protected Function CheckDate(ByVal strInvoiceDate As String) As String
        Dim strDateSetting As String
        Dim objSysCfgDs As DataSet
        Dim objDateFormat As String
        Dim strValidDate As String
		Dim strDateFormat As String

		strDateFormat = Session("SS_DATEFMT")
		strInvoiceDate = Trim(strInvoiceDate)

        If Not strInvoiceDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormat, strInvoiceDate, objDateFormat, strValidDate) = True Then
                Return strValidDate
            Else
                return ""
            End If
        End If
    End Function

	Sub DateValidation(source As Object, args As ServerValidateEventArgs)
		Dim strDate As String = args.Value
		Dim strValidatedDate As String
        
		Try
			strValidatedDate = CheckDate(strDate)
			If Trim(strValidatedDate) = "" Then
				args.IsValid = false
			Else
				args.IsValid = true
			End If
        Catch ex As System.Exception 
            args.IsValid = false
        End Try
    End Sub
	

    Sub BindDONo(ByVal pv_strDONo As String, ByVal pv_strProduct As String, ByVal pv_strSuppCode As String)
        Dim strParam As String
        Dim strOpCdGet As String = "WM_CLSTRX_WEIGHBRIDGE_TICKET_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim strSearch As String

        strSearch = IIf(pv_strDONo = "", "", "AND TIC.DeliveryNoteNo LIKE '" & Trim(pv_strDONo) & "%' ")
        strSearch = strSearch + IIf(pv_strProduct = "", "", "AND TIC.ProductCode LIKE '" & Trim(pv_strProduct) & "%' ")
        strSearch = strSearch + IIf(pv_strSuppCode = "", "", "AND TIC.CustomerCode LIKE '" & Trim(pv_strSuppCode) & "%' ")
        strSearch = strSearch

        strSearch = " AND " & Mid(Trim(strSearch), 5)
        strSearch = strSearch

        strParamName = "SEARCHSTR"
        strParamValue = strSearch

        Try
            intErrNo = objGLTrx.mtdGetDataCommon(strOpCdGet, _
                                                 strParamName, _
                                                 strParamValue, _
                                                 objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=WM_CLSTRX_WEIGHBRIDGE_TICKET_DETAILS_GET&errmesg=" & lblErrMessage.Text & "&redirect=WM/trx/WM_trx_WeighBridgeTicketDet.aspx")
        End Try


        'strParam = "|" & Trim(pv_strDONo) & "||" & Trim(pv_strProduct) & "|" & Trim(pv_strSuppCode) & "||||LocCode, TIC.TicketNo||" 

        'Try
        '    intErrNo = objWMTrx.mtdGetWeighBridgeTicket(strOpCdGet, _
        '                                                strLocation, _
        '                                                strUserId, _
        '                                                strAccMonth, _
        '                                                strAccYear, _
        '                                                strParam, _
        '                                                objDataSet)
        'Catch Exp As System.Exception
        '    Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=WM_CLSTRX_WEIGHBRIDGE_TICKET_DETAILS_GET&errmesg=" & lblErrMessage.Text & "&redirect=WM/trx/WM_trx_WeighBridgeTicketDet.aspx")
        'End Try

        If objDataSet.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objDataSet.Tables(0).Rows.Count - 1
                objDataSet.Tables(0).Rows(intCnt).Item("DeliveryNoteNo") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("DeliveryNoteNo"))
                objDataSet.Tables(0).Rows(intCnt).Item("DeliveryNoteNo") = Trim(objDataSet.Tables(0).Rows(intCnt).Item("DeliveryNoteNo"))
                If objDataSet.Tables(0).Rows(intCnt).Item("DeliveryNoteNo") = pv_strDONo Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objDataSet.Tables(0).NewRow()
        dr("DeliveryNoteNo") = ""
        dr("DeliveryNoteNo") = "Please Select Delivery Number"
        objDataSet.Tables(0).Rows.InsertAt(dr, 0)

        ddlDONo.DataSource = objDataSet.Tables(0)
        ddlDONo.DataValueField = "DeliveryNoteNo"
        ddlDONo.DataTextField = "DeliveryNoteNo"
        ddlDONo.DataBind()
        ddlDONo.SelectedIndex = intSelectedIndex

    End Sub

End Class
