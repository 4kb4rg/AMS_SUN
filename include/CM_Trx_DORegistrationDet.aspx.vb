
Imports System
Imports System.Data
Imports System.Math
Imports System.IO 
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports Microsoft.VisualBasic.Information
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.Interaction
Imports Microsoft.VisualBasic



Public Class CM_Trx_DORegistrationDet : Inherits Page
    Protected WithEvents txtDODate as TextBox
    Protected WithEvents txtDOQty as TextBox
    Protected WithEvents txtNPWP as TextBox

    Protected WithEvents txtContractQty as TextBox
    Protected WithEvents txtRemContractQty as TextBox
    Protected WithEvents txtAddress as HtmlTextArea

    Protected WithEvents lblDONo as TextBox
    Protected WithEvents txtMatched as TextBox

    Protected WithEvents lblStatus as Label
    Protected WithEvents lblDODate as Label
    Protected WithEvents lblDODateFmt as Label
    Protected WithEvents lblDateCreated as Label
    Protected WithEvents lblErrDOQty as Label
    Protected WithEvents lblLastUpdate as Label
    Protected WithEvents lblUpdatedBy as Label
    Protected WithEvents lblErrAddress as Label
    Protected WithEvents lblErrMessage as Label
    Protected WithEvents lblPleaseSelect as Label
    Protected WithEvents lblSelect as Label
    Protected WithEvents lblCode as Label
    Protected WithEvents lblErrRemQty as Label
    Protected WithEvents lblHiddenSts as Label
    Protected WithEvents lblActiveMatchExist as Label
    Protected WithEvents lblErrContractQty as label
    Protected WithEvents lblErrQty as label
    Protected WithEvents lblMsgUpdatingDO as Label
    Protected WithEvents lblShowGenInvoice As Label
    Protected WithEvents lblMsgGenerateInv as Label
    Protected WithEvents lblProductCat as Label
    Protected WithEvents lblMsgGenCheck as Label
    Protected WithEvents lblMsgWMCheck as Label
    Protected WithEvents lblErrContNo as Label
    Protected WithEvents lblErrBillParti as Label
    Protected WithEvents lblErrProduct as Label
    Protected WithEvents lblProduct as Label
    Protected WithEvents lblProductFlag as label
    Protected WithEvents lblMsgGenQtyDOCheck as label
    Protected WithEvents lblMsgQtyDOCheck as label
    Protected WithEvents lblMsgMatchDOCheck as label
    Protected WithEvents lblMsgCloseDO as label

    Protected WithEvents ddlContNo As DropDownList
    Protected WithEvents ddlBillParti As DropDownList
    Protected WithEvents ddlTerm As DropDownList
    Protected WithEvents ddlProduct As DropDownList
    Protected WithEvents rdDODest As RadioButtonList

    Protected WithEvents tbcode As HtmlInputHidden
	Protected WithEvents tbCtrNo As HtmlInputHidden
    Protected WithEvents txtRemContractQty1 As HtmlInputHidden
    Protected WithEvents SaveBtn As ImageButton
    Protected WithEvents PrintBtn As ImageButton
    Protected WithEvents UpdDOBtn As ImageButton
    Protected WithEvents GenInvoiceBtn As ImageButton
    Protected WithEvents DeactivateDOBtn as ImageButton
    Protected WithEvents btnDODate As Image

    Protected WithEvents rfvDODate As RequiredFieldValidator

    Protected WithEvents lblQtyMatched as Label
    Protected WithEvents txtQtyMatched as label

    Protected WithEvents taLoadDest as HtmlTextArea
    Protected WithEvents txtShipName as TextBox
    Protected WithEvents txtEstimationDate as TextBox
    Protected WithEvents lblEstDate as Label
    Protected WithEvents lblEstDateFmt as Label
    Protected WithEvents btnEstDate As Image

    Protected WithEvents ddlTransporter As DropDownList

	Protected WithEvents txtExpiredDate1 as TextBox
	Protected WithEvents lblExpDate1 as Label
    Protected WithEvents lblExpDate1Fmt as Label
    Protected WithEvents btnExpDate1 As Image
	
	Protected WithEvents txtExpiredDate2 as TextBox
	Protected WithEvents lblExpDate2 as Label
    Protected WithEvents lblExpDate2Fmt as Label
    Protected WithEvents btnExpDate2 As Image
	
	Protected WithEvents taProductQuality as HtmlTextArea
    Protected WithEvents btnDelete As ImageButton

    Protected WithEvents txtPackaging As TextBox
    Protected WithEvents taProductQuantity As HtmlTextArea
    Protected WithEvents taProductSpesification As HtmlTextArea
    Protected WithEvents taDeliveryNote As HtmlTextArea

    Protected WithEvents cbExcel As CheckBox


    Dim objLoc As New agri.Admin.clsLoc()
    Dim objCMTrx As New agri.CM.clsTrx()
    Dim objCMSetup As New agri.CM.clsSetup()
    Dim objGLSetup As New agri.GL.clsSetup()
    Dim objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objWMTrx As New agri.WM.clsTrx()
    Dim objPUSetup As New agri.PU.clsSetup()
    Dim objLangCap As New agri.PWSystem.clsLangCap()
    Dim objSysCfg As New agri.PWSystem.clsConfig()
    Dim objAdminLoc As New agri.Admin.clsLoc()
    Dim objIN As New agri.IN.clsTrx()
    Dim objWMSetup As New agri.WM.clsSetup()

    Dim objContractDs As New Object()
    Dim objLangCapDs As New Object()
    Dim objBuyerDs As New Object()

    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strLangCode As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim intCMAR As Integer
    Dim strDateFMt As String
    Dim intConfigsetting As Integer
    Dim intLevel As Integer

    Dim strContractNo As String = ""
    Dim intStatus As Integer
    Dim intMaxLen As Integer = 0

    Dim strPhyMonth As String
    Dim strPhyYear As String
    Dim strLastPhyYear As String
    Dim strLocType As String

    Dim SrchstrContNo As String
    Dim SrchstrBillParti As String
    Dim SrchstrTerm As String
    Dim SrchstrProduct As String

    Dim strDONo As String
    Dim strCtrNo As String

    Sub Page_Load(ByVal Sender As Object, ByVal E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_PMACCMONTH")
        strAccYear = Session("SS_PMACCYEAR")
        intCMAR = Session("SS_CMAR")
        strDateFMt = Session("SS_DATEFMT")
        intConfigsetting = Session("SS_CONFIGSETTING")
        strLocType = Session("SS_LOCTYPE")
        strPhyMonth = Session("SS_PHYMONTH")
        strPhyYear = Session("SS_PHYYEAR")
        strLastPhyYear = Session("SS_LASTPHYYEAR")
        strLocType = Session("SS_LOCTYPE")
        intLevel = Session("SS_USRLEVEL")

        lblProduct.Text = "<br>Product :*"
        lblQtyMatched.Text = "<br>Quantity Matched : "
        DeactivateDOBtn.Attributes.Add("onclick", "javascript:return ConfirmAction('close DO No');")
        GenInvoiceBtn.Attributes.Add("onclick", "javascript:return ConfirmAction('generate invoice (Generate invoice just can be run one time)');")

        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCMAccessRights.CMDORegistration), intCMAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")
        Else
            onload_GetLangCap()
            strDONo = Trim(IIf(Request.QueryString("tbcode") <> "", Request.QueryString("tbcode"), Request.Form("tbcode")))
            strCtrNo = Trim(IIf(Request.QueryString("tbCtrNo") <> "", Request.QueryString("tbCtrNo"), Request.Form("tbCtrNo")))

            intStatus = CInt(lblHiddenSts.Text)
            If Not IsPostBack Then
                If strDONo <> "" Then
                    tbcode.Value = strDONo
                    tbCtrNo.Value = strCtrNo
                    onLoad_Display(strDONo, strCtrNo)
                    onLoad_BindButton()
                    If rdDODest.Items(0).Selected = True Then
                        ValidateGenInvBtn()
                    End If
                Else
                    txtDODate.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    txtExpiredDate1.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), Now)
                    txtExpiredDate2.Text = DateAdd(DateInterval.Month, 1, CDate(Month(Now()) & "/1/" & Year(Now())))
                    txtExpiredDate2.Text = objGlobal.GetShortDate(Session("SS_DATEFMT"), DateAdd(DateInterval.Day, -1, CDate(txtExpiredDate2.Text)))

                    BindNPWP()
                    BindContractNoList("")
                    BindBuyerList("")
                    BindTermOfDelivery("")
                    BindProductList("")
                    BindTransporterList("")
                    txtContractQty.Text = "0"
                    txtRemContractQty.Text = "0"
                    txtRemContractQty1.Value = 0
                    txtDOQty.Text = "0"
                    txtMatched.Text = "0"
                    onLoad_BindButton()
                End If
            End If
        End If
        txtContractQty.Enabled = False
    End Sub

    Function CheckDate(ByVal pv_strDate As String, ByRef pr_strDate As String) As Boolean
        Dim strDateFormatCode As String = Session("SS_DATEFMT")
        Dim strDateFormat As String

        pr_strDate = ""
        CheckDate = True
        If Not pv_strDate = "" Then
            If objGlobal.mtdValidInputDate(strDateFormatCode, pv_strDate, strDateFormat, pr_strDate) = False Then
                lblDODateFmt.Text = strDateFormat
                lblEstDateFmt.Text = strDateFormat
                pr_strDate = ""
                CheckDate = False
            End If
        End If
    End Function

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

    Sub onLoad_Display(ByVal pv_strDONo As String, ByVal pv_strCtrNo As String)
        Dim strOpCd As String = "CM_CLSTRX_DO_REG_GET"
        Dim strOpCd_GetMatch As String = "CM_CLSTRX_DO_REG_GET_QTYMATCH"
        Dim strParam As String
        Dim strParamCheckMatchStatus As String
        Dim intErrNo As Integer
        Dim strSearch As String
        Dim strBalQty As String
        Dim strTransporter As String

        strSearch = " where cdo.LocCode = '" & strLocation & "' and cdo.DONo = '" & pv_strDONo & "' and cdo.ContractNo = '" & pv_strCtrNo & "' "
        strParam = strSearch & "|" & ""


        Try
            intErrNo = objCMTrx.mtdGetDOReg(strOpCd, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try


        If objContractDs.Tables(0).Rows.count > 0 Then
            lblDONo.Text = Trim(objContractDs.Tables(0).Rows(0).Item("DONo"))
            txtDODate.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("DODate"))
            txtDOQty.Text = Round(objContractDs.Tables(0).Rows(0).Item("DOQty"), CInt(Session("SS_ROUNDNO")))
            txtNPWP.Text = Trim(objContractDs.Tables(0).Rows(0).Item("npwp"))
            txtAddress.Value = Trim(objContractDs.Tables(0).Rows(0).Item("Address"))
            txtContractQty.Text = Round(objContractDs.Tables(0).Rows(0).Item("ContractQty"), CInt(Session("SS_ROUNDNO")))
            If CDbl(Trim(objContractDs.Tables(0).Rows(0).Item("RemContractQty"))) < 0 Then
                txtRemContractQty.Text = Round(0, CInt(Session("SS_ROUNDNO")))
                txtRemContractQty1.Value = Round(0, CInt(Session("SS_ROUNDNO")))
            Else
                txtRemContractQty.Text = Round(objContractDs.Tables(0).Rows(0).Item("RemContractQty"), CInt(Session("SS_ROUNDNO")))
                txtRemContractQty1.Value = Round(objContractDs.Tables(0).Rows(0).Item("RemContractQty"), CInt(Session("SS_ROUNDNO")))
            End If
            intStatus = Trim(objContractDs.Tables(0).Rows(0).Item("Status"))
            lblHiddenSts.Text = Trim(objContractDs.Tables(0).Rows(0).Item("Status"))
            lblStatus.Text = objCMTrx.mtdGetContractStatus(Trim(objContractDs.Tables(0).Rows(0).Item("Status")))
            lblDateCreated.Text = objGlobal.GetLongDate(objContractDs.Tables(0).Rows(0).Item("CreateDate"))
            lblLastUpdate.Text = objGlobal.GetLongDate(objContractDs.Tables(0).Rows(0).Item("UpdateDate"))
            lblUpdatedBy.Text = Trim(objContractDs.Tables(0).Rows(0).Item("UserName"))
            SrchstrContNo = Trim(objContractDs.Tables(0).Rows(0).Item("ContractNo"))
            SrchstrBillParti = Trim(objContractDs.Tables(0).Rows(0).Item("billpartycode"))
            SrchstrTerm = Trim(objContractDs.Tables(0).Rows(0).Item("termdelivery"))
            SrchstrProduct = Trim(objContractDs.Tables(0).Rows(0).Item("product"))
            strTransporter = Trim(objContractDs.Tables(0).Rows(0).Item("TransporterCode"))

            rdDODest.Items(0).Selected = IIf(CInt(Trim(objContractDs.Tables(0).Rows(0).Item("DODestination"))) = objCMTrx.EnumDODestination.BillParty, True, False)
            rdDODest.Items(1).Selected = IIf(CInt(Trim(objContractDs.Tables(0).Rows(0).Item("DODestination"))) = objCMTrx.EnumDODestination.Bulking, True, False)
            taLoadDest.Value = Trim(objContractDs.Tables(0).Rows(0).Item("LoadDest"))
            txtShipName.Text = Trim(objContractDs.Tables(0).Rows(0).Item("ShipName"))
            txtEstimationDate.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("EstimationDate"))
            taProductQuality.Value = Trim(objContractDs.Tables(0).Rows(0).Item("ProductQuality"))
            txtExpiredDate1.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("ExpiredDate1"))
            txtExpiredDate2.Text = objGlobal.GetShortDate(strDateFMt, objContractDs.Tables(0).Rows(0).Item("ExpiredDate2"))
            txtPackaging.Text = Trim(objContractDs.Tables(0).Rows(0).Item("Packaging"))
            taProductQuantity.Value = Trim(objContractDs.Tables(0).Rows(0).Item("ProductQuality"))
            taProductSpesification.Value = Trim(objContractDs.Tables(0).Rows(0).Item("ProductSpesification"))
            taDeliveryNote.Value = Trim(objContractDs.Tables(0).Rows(0).Item("DeliveryNote"))


            BindContractNoList(SrchstrContNo)
            BindBuyerList(SrchstrBillParti)
            BindTermOfDelivery(SrchstrTerm)
            BindProductList(SrchstrProduct)
            BindTransporterList(strTransporter)

            strSearch = " b.LocCode = '" & strLocation & "' and a.DONo = '" & pv_strDONo & "' "
            strParam = strSearch & "|" & ""


            Try
                intErrNo = objCMTrx.mtdGetDOReg(strOpCd_GetMatch, strParam, 0, objContractDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
            End Try

            If objContractDs.Tables(0).Rows.count > 0 Then
                txtMatched.Text = Round(objContractDs.Tables(0).Rows(0).Item("Qty"), CInt(Session("SS_ROUNDNO")))
            Else
                txtMatched.Text = Round(0, CInt(Session("SS_ROUNDNO")))
            End If

            If rdDODest.Items(0).Selected = True Then
                ddlContNo.Enabled = False
                ddlBillParti.Enabled = False
                ddlTerm.Enabled = False
                ddlProduct.Visible = False
                lblProduct.Visible = False
                txtQtyMatched.Visible = False
                lblQtyMatched.Visible = False

                If intStatus = objCMTrx.EnumContractStatus.Closed Or intStatus = objCMTrx.EnumContractStatus.Deleted Then
                    GenInvoiceBtn.Visible = False
                Else
                    GenInvoiceBtn.Visible = False
                End If

                DeactivateDOBtn.Visible = True
                lblDONo.Enabled = False
            Else
                ddlContNo.Enabled = False
                ddlBillParti.Enabled = False
                ddlTerm.Enabled = True
                ddlProduct.Enabled = False
                ddlProduct.Visible = True
                lblProduct.Visible = True
                txtQtyMatched.Visible = False
                lblQtyMatched.Visible = False
                DeactivateDOBtn.Visible = True
                GenInvoiceBtn.Visible = False
                lblDONo.Enabled = False
            End If
            rdDODest.Enabled = False
        End If
    End Sub


    Sub onLoad_BindButton()
        txtDODate.Enabled = False
        txtDOQty.Enabled = False
        txtNPWP.Enabled = False
        txtContractQty.Enabled = False
        txtRemContractQty.Enabled = False
        ddlContNo.Enabled = False
        ddlBillParti.Enabled = False
        ddlTerm.Enabled = False
        ddlProduct.Enabled = True
        SaveBtn.Visible = False
        PrintBtn.Visible = False
        UpdDOBtn.Visible = False
        GenInvoiceBtn.Visible = False
        DeactivateDOBtn.Visible = True
        lblMsgUpdatingDO.Visible = False
        lblMsgGenerateInv.Visible = False
        lblMsgGenCheck.Visible = False
        lblMsgWMCheck.Visible = False
        txtAddress.Visible = True
        lblMsgCloseDO.Visible = False

        txtShipName.Enabled = False
        taLoadDest.Disabled = True
        taProductQuality.Disabled = False
        txtEstimationDate.Enabled = False

        lblDONo.Enabled = False
        txtMatched.Enabled = False


        Select Case intStatus
            Case objCMTrx.EnumContractStatus.Active
                txtDODate.Enabled = True
                If intLevel >= 1 Then
                    txtDOQty.Enabled = True
                Else
                    txtDOQty.Enabled = False
                End If
                txtNPWP.Enabled = True
                txtContractQty.Enabled = False
                txtRemContractQty.Enabled = False
                ddlProduct.Enabled = False
                DeactivateDOBtn.Visible = True
                ddlTerm.Enabled = False
                SaveBtn.Visible = True
                PrintBtn.Visible = True
                UpdDOBtn.Visible = True
                lblMsgUpdatingDO.Visible = False
                lblMsgGenerateInv.Visible = False
                lblMsgGenCheck.Visible = False
                lblMsgWMCheck.Visible = False
                lblMsgCloseDO.Visible = False
                txtAddress.Visible = True
                txtShipName.Enabled = True
                taLoadDest.Disabled = False
                taProductQuality.Disabled = False
                txtEstimationDate.Enabled = True
                If Trim(lblShowGenInvoice.Text) = "yes" And rdDODest.Items(0).Selected = True Then
                    GenInvoiceBtn.Visible = False
                Else
                    GenInvoiceBtn.Visible = False
                End If

                lblDONo.Enabled = False
                txtMatched.Enabled = False

                If rdDODest.Items(1).Selected = True Then
                    ddlContNo.Enabled = False
                    ddlBillParti.Enabled = False
                    txtContractQty.Enabled = False
                    txtRemContractQty.Enabled = False
                    DeactivateDOBtn.Visible = True
                    ddlProduct.Visible = True
                    lblProduct.Visible = True
                    GenInvoiceBtn.Visible = False
                Else
                    ddlContNo.Enabled = False
                    ddlBillParti.Enabled = False
                    txtContractQty.Enabled = True
                    txtRemContractQty.Enabled = True
                    DeactivateDOBtn.Visible = True
                    ddlProduct.Visible = False
                    lblProduct.Visible = False
                    GenInvoiceBtn.Visible = False
                End If

            Case objCMTrx.EnumContractStatus.Deleted
                txtAddress.Visible = False
                txtDODate.Enabled = False
                txtDOQty.Enabled = False
                txtNPWP.Enabled = False
                txtContractQty.Enabled = False
                txtRemContractQty.Enabled = False
                ddlContNo.Enabled = False
                ddlBillParti.Enabled = False
                ddlProduct.Enabled = False
                DeactivateDOBtn.Visible = False
                ddlTerm.Enabled = False
                SaveBtn.Visible = False
                PrintBtn.Visible = False
                UpdDOBtn.Visible = False
                GenInvoiceBtn.Visible = False
                lblMsgUpdatingDO.Visible = False
                lblMsgGenerateInv.Visible = False
                lblMsgGenCheck.Visible = False
                lblMsgWMCheck.Visible = False
                lblMsgCloseDO.Visible = False
                txtShipName.Enabled = False
                taLoadDest.Disabled = True
                taProductQuality.Disabled = True
                txtEstimationDate.Enabled = False

                'If Trim(lblShowGenInvoice.Text) = "yes" Then
                '    GenInvoiceBtn.Visible = False
                'Else
                '    GenInvoiceBtn.Visible = False
                'End If
                GenInvoiceBtn.Visible = False
                btnDelete.Visible = False

                lblMsgGenCheck.Visible = False
                lblDONo.Enabled = False
                txtMatched.Enabled = False

            Case objCMTrx.EnumContractStatus.Closed
                txtDODate.Enabled = False
                txtDOQty.Enabled = False
                txtNPWP.Enabled = False
                txtContractQty.Enabled = False
                txtRemContractQty.Enabled = False
                DeactivateDOBtn.Visible = False
                ddlContNo.Enabled = False
                ddlBillParti.Enabled = False
                ddlProduct.Enabled = False
                txtAddress.Visible = False
                lblMsgCloseDO.Visible = False
                ddlTerm.Enabled = False
                SaveBtn.Visible = False
                PrintBtn.Visible = True
                UpdDOBtn.Visible = False
                GenInvoiceBtn.Visible = False
                lblMsgUpdatingDO.Visible = False
                lblMsgGenerateInv.Visible = False
                lblMsgGenCheck.Visible = False
                lblMsgWMCheck.Visible = False
                txtShipName.Enabled = False
                taLoadDest.Disabled = True
                taProductQuality.Disabled = True
                txtEstimationDate.Enabled = False
                lblDONo.Enabled = False
                txtMatched.Enabled = False

                If rdDODest.Items(1).Selected = True Then
                    ddlContNo.Enabled = False
                    ddlBillParti.Enabled = False
                    txtContractQty.Enabled = False
                    txtRemContractQty.Enabled = False
                    DeactivateDOBtn.Visible = False
                    ddlProduct.Visible = True
                    lblProduct.Visible = True
                    GenInvoiceBtn.Visible = False
                Else
                    ddlContNo.Enabled = False
                    ddlBillParti.Enabled = False
                    txtContractQty.Enabled = False
                    txtRemContractQty.Enabled = False
                    DeactivateDOBtn.Visible = False
                    ddlProduct.Visible = False
                    lblProduct.Visible = False
                    GenInvoiceBtn.Visible = False
                End If



            Case Else
                txtDODate.Enabled = True
                txtDOQty.Enabled = True
                txtNPWP.Enabled = True
                txtContractQty.Enabled = False
                txtRemContractQty.Enabled = False
                ddlContNo.Enabled = True
                ddlBillParti.Enabled = True
                ddlProduct.Enabled = False
                DeactivateDOBtn.Visible = True
                lblMsgCloseDO.Visible = False
                ddlTerm.Enabled = True
                SaveBtn.Visible = True
                PrintBtn.Visible = True
                UpdDOBtn.Visible = True
                GenInvoiceBtn.Visible = False
                lblMsgUpdatingDO.Visible = False
                lblMsgGenerateInv.Visible = False
                lblMsgGenCheck.Visible = False
                lblMsgWMCheck.Visible = False

                txtShipName.Enabled = True
                taLoadDest.Disabled = False
                taProductQuality.Disabled = True
                txtEstimationDate.Enabled = False
                lblDONo.Enabled = False
                txtMatched.Enabled = True

        End Select
    End Sub

    Sub SaveButton_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strCmdArgs As String = CType(Sender, ImageButton).CommandArgument
        Dim strOpCd_Upd As String = "CM_CLSTRX_DO_REG_UPD"
        Dim strOpCd_Get As String = "CM_CLSTRX_DO_REG_GET"
        Dim strOpCd_Add As String = "CM_CLSTRX_DO_REG_ADD"

        Dim strOpCd_GetMatchLine As String = "CM_CLSTRX_CONTRACTMATCH_LN_GET"
        Dim strOpCd_AddTemp As String = "CM_CLSTRX_DO_MATCHEDTEM_ADD"
        Dim strOpCd_GetMatch As String = "CM_CLSTRX_DO_REG_GET_QTYMATCH"
        Dim strOpCd As String = "CM_CLSTRX_DO_REG_UPDATING_DO"

        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String = ""

        Dim strOppCd As String = "IN_CLSTRX_PURREQ_MOVEID"
        Dim strOppCd_Back As String = "IN_CLSTRX_PURREQ_BACKID"
        Dim strOppCd_GetID As String = "IN_CLSTRX_PURREQ_GETID"
        Dim strContType As Integer
        Dim objCMID As Object
        Dim objPRDs As Object
        Dim strRunNo As Integer
        Dim strProdType As String

        Dim strDOIDFormat As String
        Dim strNewYear As String = ""
        Dim strTranPrefix As String = "CMDO"
        Dim strHistYear As String = ""
        Dim objCompDs As New Object
        Dim blnIsDetail As Boolean = True
        Dim strRunNumber As String
        Dim strTerm As String = ddlTerm.SelectedItem.Value
        Dim strPhyMonthRom As String = ""
        Dim strContNo As String = ddlContNo.SelectedItem.Value
        Dim strBillParty As String = ddlBillParti.SelectedItem.Value
        Dim strProduct As String = ddlProduct.SelectedItem.Value
        Dim dtUpdateDate As String = ""
        Dim strDONo As String = ""
        Dim strStatus As String = ""
        Dim strDODest As String = ""
        Dim strProductCode As String = ""
        Dim strSearch As String = ""
        Dim objMatchDs As New Object()
        Dim strTransporter As String = ddlTransporter.SelectedItem.Value
        Dim strProductQuality As String = taProductQuality.Value
        Dim strExpDate1 As String = Date_Validation(txtExpiredDate1.Text, False)
        Dim strExpDate2 As String = Date_Validation(txtExpiredDate2.Text, False)

        Dim strDODate As String = Date_Validation(txtDODate.Text, False)
        Dim strEstDate As String = Date_Validation(txtEstimationDate.Text, False)
        Dim indDate As String = ""

        If rdDODest.Items(1).Selected = True Then
            txtRemContractQty.Text = "0"
        End If

        blnIsUpdate = IIf(intStatus = 0, False, True)
        If intStatus = 0 Then
            strStatus = "1"
        Else
            strStatus = intStatus
        End If

        If txtDODate.Text = "" Then
            lblDODate.Visible = True
            Exit Sub
        Else
            lblDODate.Visible = False
        End If

        If strContNo = "Please Select Contract No" Then
            strContNo = ""
        End If
        If strBillParty = "Please Select Customer Code" Then
            strBillParty = ""
        End If

        If strContNo = "" And rdDODest.Items(0).Selected = True Then
            lblErrContNo.Visible = True
            Exit Sub
        Else
            lblErrContNo.Visible = False
        End If

        If strBillParty = "" And rdDODest.Items(0).Selected = True Then
            lblErrBillParti.Visible = True
            Exit Sub
        Else
            lblErrBillParti.Visible = False
        End If

        If strProduct = "" And rdDODest.Items(1).Selected = True Then
            lblErrProduct.Visible = True
            Exit Sub
        Else
            lblErrProduct.Visible = False
        End If

        If CDbl(txtRemContractQty.Text) < 0 Then
            lblErrQty.Visible = True
            Exit Sub
        Else
            lblErrQty.Visible = False
        End If

        If Len(strPhyMonth) = 1 Then
            strPhyMonth = "0" & strPhyMonth
        End If

        strParam = "where phyyear = '" & Right(Trim(strPhyYear), 2) & "' and tran_prefix = 'CMDO'" & "|"
        Try
            intErrNo = objIN.mtdGetPurchaseRequest(strOppCd_GetID, _
                                                   strParam, _
                                                   objIN.EnumPurReqDocType.StockPR, _
                                                   strAccMonth, _
                                                   strAccYear, _
                                                   strLocation, _
                                                   objPRDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=PURREQ_LIST_GET_DISPLAYPR&errmesg=" & lblErrMessage.Text & "&redirect=IN/trx/IN_PurReq.aspx")
        End Try


        If objPRDs.Tables(0).Rows.Count > 0 Then
            strNewYear = ""
            strRunNo = Trim(objPRDs.Tables(0).Rows(0).Item("Val"))
        Else
            strHistYear = Right(strLastPhyYear, 2)
            strNewYear = "1"
            strRunNo = 0
        End If

        If strPhyMonth = "1" Then
            strPhyMonthRom = "I"
        ElseIf strPhyMonth = "2" Then
            strPhyMonthRom = "II"
        ElseIf strPhyMonth = "3" Then
            strPhyMonthRom = "III"
        ElseIf strPhyMonth = "4" Then
            strPhyMonthRom = "IV"
        ElseIf strPhyMonth = "5" Then
            strPhyMonthRom = "V"
        ElseIf strPhyMonth = "6" Then
            strPhyMonthRom = "VI"
        ElseIf strPhyMonth = "7" Then
            strPhyMonthRom = "VII"
        ElseIf strPhyMonth = "8" Then
            strPhyMonthRom = "VIII"
        ElseIf strPhyMonth = "9" Then
            strPhyMonthRom = "IX"
        ElseIf strPhyMonth = "10" Then
            strPhyMonthRom = "X"
        ElseIf strPhyMonth = "11" Then
            strPhyMonthRom = "XI"
        Else
            strPhyMonthRom = "XII"
        End If

        If rdDODest.Items(1).Selected = True Then
            lblProductCat.Text = objWMTrx.mtdGetWeighBridgeTicketProductCode(strProduct)
        End If

        If rdDODest.Items(0).Selected = True Then
            strDODest = objCMTrx.EnumDODestination.BillParty
        Else
            strDODest = objCMTrx.EnumDODestination.Bulking
        End If

        If rdDODest.Items(0).Selected = True Then
            strProductCode = Trim(lblProductFlag.Text)
        Else
            strProductCode = strProduct
        End If

        Select Case strProductCode
            Case objWMTrx.EnumWeighBridgeTicketProduct.FFB
                strProdType = "FFB"
            Case objWMTrx.EnumWeighBridgeTicketProduct.CPO
                strProdType = "CPO"
            Case objWMTrx.EnumWeighBridgeTicketProduct.PK
                strProdType = "PK"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Others
                strProdType = "OTH"
            Case objWMTrx.EnumWeighBridgeTicketProduct.EFB
                strProdType = "JJK" '"EFB"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Shell
                strProdType = "CKG"
            Case objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang
                strProdType = "ABJ"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Fiber
                strProdType = "FBR"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Brondolan
                strProdType = "BRD"
            Case objWMTrx.EnumWeighBridgeTicketProduct.Solid
                strProdType = "SLD"
            Case objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah
                strProdType = "LMB"
            Case objWMTrx.EnumWeighBridgeTicketProduct.EFBOil
                strProdType = "EFO"
            Case objWMTrx.EnumWeighBridgeTicketProduct.MinyakKolam
                strProdType = "MKL"
            Case objWMTrx.EnumWeighBridgeTicketProduct.EFBPress
                strProdType = "EBP"
        End Select

        strAccYear = Year(strDODate)
        strAccMonth = Month(strDODate)

        strDOIDFormat = "/DO/" & strCompany & "/" & strLocation & "/" & strProdType & "/" & Mid(Trim(strAccYear), 3, 2)
        'strDOIDFormat = "/SSJA/DO-" & Trim(strProdType) & "/" & IIf(Len(Trim(strAccMonth)) = 1, "0" & strAccMonth, strAccMonth) & Mid(Trim(strAccYear), 3, 2)
        'strDOIDFormat = ""

        txtRemContractQty.Text = Round((txtRemContractQty1.Value - CDbl(txtDOQty.Text)), CInt(Session("SS_ROUNDNO")))

        strDONo = lblDONo.Text
        strParam = strDONo & Chr(9) & _
                   strDODate & Chr(9) & _
                   IIf(Trim(txtDOQty.Text) = "", "0", Trim(txtDOQty.Text)) & Chr(9) & _
                   IIf(Trim(txtNPWP.Text) = "", "", Trim(txtNPWP.Text)) & Chr(9) & _
                   IIf(Trim(txtAddress.Value) = "", "", Trim(txtAddress.Value)) & Chr(9) & _
                   strContNo & Chr(9) & _
                   strBillParty & Chr(9) & _
                   strTerm & Chr(9) & _
                   IIf(Trim(txtContractQty.Text) = "", "0", Trim(txtContractQty.Text)) & Chr(9) & _
                   IIf(Trim(txtRemContractQty.Text) = "", "0", Trim(txtRemContractQty.Text)) & Chr(9) & _
                   dtUpdateDate & Chr(9) & _
                   strStatus & Chr(9) & _
                   strLocation & Chr(9) & _
                   strAccMonth & Chr(9) & _
                   strAccYear & Chr(9) & _
                   strUserId & Chr(9) & _
                   strDOIDFormat & Chr(9) & strNewYear & Chr(9) & strTranPrefix & Chr(9) & strHistYear & Chr(9) & Right(strPhyYear, 2) & Chr(9) & _
                   strDODest & Chr(9) & strProductCode & Chr(9) & _
                   strEstDate & Chr(9) & _
                   IIf(Trim(txtShipName.Text) = "", "", Trim(txtShipName.Text)) & Chr(9) & _
                   IIf(Trim(taLoadDest.Value) = "", "", Trim(taLoadDest.Value)) & Chr(9) & _
                   strTransporter & Chr(9) & _
                   IIf(Trim(taProductQuality.Value) = "", "", Trim(taProductQuality.Value)) & Chr(9) & _
                   strExpDate1 & Chr(9) & _
                   strExpDate2 & Chr(9) & _
                   Trim(txtPackaging.Text) & Chr(9) & _
                   Trim(taProductQuantity.Value) & Chr(9) & _
                   Trim(taProductSpesification.Value) & Chr(9) & _
                   Trim(taDeliveryNote.Value)

        Try
            intErrNo = objCMTrx.mtdUpdDOReg(strOpCd_Get, _
                                               strOpCd_Add, _
                                               strOpCd_Upd, _
                                               strCompany, _
                                               strOppCd, _
                                               strLocation, _
                                               strUserId, _
                                               strParam, _
                                               False, _
                                               objCMID, _
                                               blnIsUpdate, _
                                               strTranPrefix)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_TRX_DOREGDET_ADD&errmesg=" & Exp.Message & "&redirect=CM/Trx/CM_Trx_DORegistrationDet.aspx")
        End Try

        strDONo = objCMID
        onLoad_Display(strDONo, ddlContNo.SelectedItem.Value)

        strSearch = "and loc.LocCode = '" & strLocation & "' and ln.DoNo = '" & Trim(strDONo) & "' and ln.contractno = '" & Trim(ddlContNo.SelectedItem.Value) & "'"
        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd_GetMatchLine, strParam, 0, objMatchDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try

        If objMatchDs.Tables(0).Rows.Count = 0 Then
            If txtMatched.Text = "" Then
                txtMatched.Text = 0
            End If
            strParam = Trim(lblDONo.Text) & "|" & Trim(strContNo) & "|" & CDbl(Trim(txtMatched.Text))

            Try
                intErrNo = objCMTrx.mtdAddContractMatchTemp(strOpCd_AddTemp, Trim(strCompany), Trim(strLocation), Trim(strUserId), strParam)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_DORegistrationDet.aspx")
            End Try

            strParam = Trim(lblDONo.Text) & "|" & Trim(strContNo) & "|" & _
                    Trim(strUserId) & "|" & Trim(strPhyMonth) & "|" & Trim(strPhyYear) & "|" & _
                    Trim(strDODest) & "|" & Trim(strAccMonth) & "|" & Trim(strAccYear)


            Try
                intErrNo = objCMTrx.mtdDORegUpdDO(strOpCd, _
                                                strCompany, _
                                                strLocation, _
                                                strUserId, _
                                                strParam)


                lblMsgUpdatingDO.Visible = True

            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_TRX_DOREGDET_UPDATEDO&errmesg=" & Exp.Message & "&redirect=CM/Trx/CM_Trx_DORegistrationDet.aspx")
            End Try

            strSearch = " b.LocCode = '" & strLocation & "' and a.DONo = '" & Trim(lblDONo.Text) & "' "
            strParam = strSearch & "|" & ""


            Try
                intErrNo = objCMTrx.mtdGetDOReg(strOpCd_GetMatch, strParam, 0, objContractDs)
            Catch Exp As System.Exception
                Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
            End Try

            If objContractDs.Tables(0).Rows.count > 0 Then
                txtMatched.Text = Trim(objContractDs.Tables(0).Rows(0).Item("Qty"))
            Else
                txtMatched.Text = "0"
            End If

        End If

        onLoad_Display(lblDONo.Text, Trim(strContNo))
    End Sub

    Sub btnPrintPrev_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCodePrint As String = "ADMIN_SHARE_UPD_PRINTDATE"
        Dim strUpdString As String = ""
        Dim strStatus As String
        Dim intStatus As Integer
        Dim strSortLine As String
        Dim strPrintDate As String
        Dim strTable As String
        Dim strDODestination As String
        Dim strContNo As String = ddlContNo.SelectedItem.Value

        If rdDODest.Items(0).Selected = True Then
            strDODestination = objCMTrx.EnumDODestination.BillParty
        Else
            strDODestination = objCMTrx.EnumDODestination.Bulking
        End If

        Response.Write("<Script Language=""JavaScript"">window.open(""../reports/CM_Rpt_DORegistrationDet.aspx?strDONo=" & lblDONo.Text & "&strContractNo=" & strContNo & _
                       "&strPrintDate=" & strPrintDate & "&strStatus=" & strStatus & "&strSortLine=" & strSortLine & "&DODest=" & strDODestination & "&strExportToExcel=" & IIf(cbExcel.Checked, "1", "0") & """, null ,""status=yes, resizable=yes, scrollbars=yes, toolbar=no, location=no"");</Script>")

    End Sub

    Sub UpdDOButton_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd As String = "CM_CLSTRX_DO_REG_UPDATING_DO"
        Dim strOpCd_WM As String = "WM_CLSTRX_WEIGHBRIDGE_TICKET_GET"
        Dim strOpCd_GetMatch As String = "CM_CLSTRX_DO_REG_GET_QTYMATCH"
        Dim strparam As String = ""
        Dim intErrNo As Integer
        Dim strDODest As String = ""
        Dim strSearch As String = ""
        Dim objMatchDs As New Object()
        Dim strContNo As String = ddlContNo.SelectedItem.Value
        Dim strBillParty As String = ""

        lblMsgGenCheck.visible = False
        lblMsgWMCheck.visible = False
        lblMsgGenQtyDOCheck.visible = False
        lblMsgMatchDOCheck.visible = False
        lblMsgQtyDOCheck.visible = False
        lblMsgCloseDO.visible = False


        If rdDODest.Items(0).Selected = True Then
            strDODest = objCMTrx.EnumDODestination.BillParty
        Else
            strDODest = objCMTrx.EnumDODestination.Bulking
        End If

        If strContNo = "Please Select Contract No" Then
            strContNo = ""
        End If
        If strBillParty = "Please Select Customer Code" Then
            strBillParty = ""
        End If


        strSearch = "and tic.LocCode = '" & strLocation & "' and tic.DeliveryNoteNo = '" & trim(lblDONo.text) & "' "
        strSearch = strSearch & " and tic.accmonth = '" & Trim(strAccMonth) & "' and tic.accyear = '" & trim(strAccYear) & "'"

        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd_WM, strParam, 0, objMatchDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try

        If objMatchDs.Tables(0).Rows.Count = 0 Then
            lblMsgWMCheck.visible = True
            Exit Sub
        End If


        strparam = Trim(lblDONo.Text) & "|" & Trim(strContNo) & "|" & _
                   Trim(strUserId) & "|" & Trim(strPhyMonth) & "|" & Trim(strPhyYear) & "|" & _
                   Trim(strDODest) & "|" & Trim(strAccMonth) & "|" & Trim(strAccYear)


        Try
            intErrNo = objCMTrx.mtdDORegUpdDO(strOpCd, _
                                               strCompany, _
                                               strLocation, _
                                               strUserId, _
                                               strParam)


            lblMsgUpdatingDO.visible = True

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_TRX_DOREGDET_UPDATEDO&errmesg=" & Exp.Message & "&redirect=CM/Trx/CM_Trx_DORegistrationDet.aspx")
        End Try

        strSearch = " b.LocCode = '" & strLocation & "' and a.DONo = '" & Trim(lblDONo.Text) & "' "
        strParam = strSearch & "|" & ""


        Try
            intErrNo = objCMTrx.mtdGetDOReg(strOpCd_GetMatch, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objContractDs.Tables(0).Rows.count > 0 Then
            txtMatched.Text = Trim(objContractDs.Tables(0).Rows(0).Item("Qty"))
        Else
            txtMatched.Text = "0"
        End If

    End Sub


    Sub Button_GenInvoice(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_GetContract = "CM_CLSTRX_CONTRACTMATCH_LN_GET1"
        Dim strOpCd_AddInv As String = "CM_CLSTRX_CONTRACTMATCH_INVOICE_ADD"
        Dim strOpCd_AddInvLn As String = "CM_CLSTRX_CONTRACTMATCH_INVOICELN_ADD"
        Dim strOpCd_UpdMatch As String = "CM_CLSTRX_CONTRACTMATCH_UPD"
        Dim strOpCd_UpdMatchLn As String = "CM_CLSTRX_CONTRACTMATCH_LN_UPD"
        Dim strOpCd As String = "CM_CLSTRX_CONTRACT_QTYVALUE_GET"
        Dim strOpCd_UpdStatus As String = "CM_CLSTRX_DO_REG_STATUS_UPD"
        Dim strOpCd_Upd As String = "CM_CLSTRX_DO_REG_UPD"
        Dim strOpCd_Get As String = "CM_CLSTRX_DO_REG_GET"
        Dim strOpCd_Add As String = "CM_CLSTRX_DO_REG_ADD"
        Dim strOppCd As String = "IN_CLSTRX_PURREQ_MOVEID"

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
        Dim strSearch As String
        Dim strMatchingId As String
        Dim strstatus As String
        Dim dblQtyExtra As Double
        Dim dblQtyLess As Double
        Dim dblQty As Double

        Dim strTranPrefix As String = "CMDO"
        Dim objCMID As Object

        Dim objMatchDs As New Object()

        lblMsgUpdatingDO.visible = False
        lblMsgWMCheck.visible = False
        lblMsgGenQtyDOCheck.visible = False
        lblMsgMatchDOCheck.visible = False
        lblMsgQtyDOCheck.visible = False
        lblMsgCloseDO.visible = False


        strSearch = "and ma.LocCode = '" & strLocation & "' and ln.dono = '" & trim(lblDONo.text) & "' "
        strSearch = strSearch & " and ln.contractno = '" & Trim(ddlContNo.SelectedItem.Value) & "' "

        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd, strParam, 0, objMatchDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try

        If objMatchDs.Tables(0).Rows.Count > 0 Then
            strMatchingId = Trim(objMatchDs.Tables(0).Rows(0).Item("MatchingId"))
            strBuyerCode = Trim(objMatchDs.Tables(0).Rows(0).Item("BuyerCode"))
            dblQtyExtra = Trim(objMatchDs.Tables(0).Rows(0).Item("qtyextra"))
            dblQtyLess = Trim(objMatchDs.Tables(0).Rows(0).Item("qtyless"))
            dblQty = Trim(objMatchDs.Tables(0).Rows(0).Item("qty"))
        Else
            strMatchingId = ""
            strBuyerCode = ""
            lblMsgGenCheck.visible = True
            Exit Sub
        End If



        MatchAccMonth = strAccMonth
        MatchAccYear = strAccYear

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





        UpdateStatusContract()

        onLoad_Display(strDONo, "")
        onLoad_BindButton()
        lblMsgGenerateInv.visible = True

    End Sub



    Sub BackBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_DORegistrationList.aspx")
    End Sub

    Sub onload_GetLangCap()
        GetEntireLangCap()
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
                If strLocType = objAdminLoc.EnumLocType.Mill Then
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTermMW"))
                Else
                    Return Trim(objLangCapDs.Tables(0).Rows(count).Item("BusinessTerm"))
                End If
                Exit For
            End If
        Next
    End Function


    Sub BindContractNoList(ByVal pv_strContNo As String)
        Dim strParam As String
        Dim strOpCdGet As String = "CM_CLSTRX_CONTRACT_REG_GET_DO"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim strSearch As String

        strSearch = " ctr.LocCode = '" & strLocation & "' and ctr.ContractNo like '%" & pv_strContNo & "' and ctr.status in ('1', '4') "
        strParam = strSearch & "|" & " Order By ((ctr.AccYear*100)+ctr.AccMonth) desc, ctr.ContractNo"


        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdGet, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objContractDs.Tables(0).Rows.Count - 1
                objContractDs.Tables(0).Rows(intCnt).Item("ContractNo") = Trim(objContractDs.Tables(0).Rows(intCnt).Item("ContractNo"))
                objContractDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objContractDs.Tables(0).Rows(intCnt).Item("ContractNo")) & ", " & Trim(objContractDs.Tables(0).Rows(intCnt).Item("Name"))
                If objContractDs.Tables(0).Rows(intCnt).Item("ContractNo") = pv_strContNo Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objContractDs.Tables(0).NewRow()
        dr("ContractNo") = ""
        dr("Name") = "Please Select Contract No"
        objContractDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlContNo.DataSource = objContractDs.Tables(0)
        ddlContNo.DataValueField = "ContractNo"
        ddlContNo.DataTextField = "Name"
        ddlContNo.DataBind()
        ddlContNo.SelectedIndex = intSelectedIndex
    End Sub

    Sub BindBuyerList(ByVal pv_strBuyerCode As String)
        Dim strParam As String
        Dim strOpCdGet As String = "GL_CLSSETUP_BILLPARTY_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        strParam = "" & "|" & _
                   "" & "|" & _
                   objGLSetup.EnumBillPartyStatus.Active & "|" & _
                   "" & "|" & _
                   "BP.BillPartyCode" & "|" & _
                   "ASC" & "|"
        Try
            intErrNo = objGLSetup.mtdGetBillParty(strOpCdGet, strParam, objBuyerDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_BUYERLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objBuyerDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objBuyerDs.Tables(0).Rows.Count - 1
                objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode"))
                'objBuyerDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode")) & " (" & Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
                objBuyerDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("NamePPN"))
                If objBuyerDs.Tables(0).Rows(intCnt).Item("BillPartyCode") = Trim(pv_strBuyerCode) Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objBuyerDs.Tables(0).NewRow()
        dr("BillPartyCode") = ""
        dr("Name") = "Please Select Customer Code"
        objBuyerDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlBillParti.DataSource = objBuyerDs.Tables(0)
        ddlBillParti.DataValueField = "BillPartyCode"
        ddlBillParti.DataTextField = "Name"
        ddlBillParti.DataBind()
        ddlBillParti.SelectedIndex = intSelectedIndex
    End Sub

    Sub BindTermOfDelivery(ByVal pv_strTerm As String)
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        'If ddlTerm.Items.Count = 0 Then
        '    ddlTerm.Items.Add(New ListItem("Select Term Of Delivery", ""))
        '    ddlTerm.Items.Add(New ListItem(objCMTrx.mtdGetTermOfDelivery(objCMTrx.EnumTermOfDelivery.Franco), objCMTrx.EnumTermOfDelivery.Franco))
        '    ddlTerm.Items.Add(New ListItem(objCMTrx.mtdGetTermOfDelivery(objCMTrx.EnumTermOfDelivery.Loco), objCMTrx.EnumTermOfDelivery.Loco))
        '    ddlTerm.Items.Add(New ListItem(objCMTrx.mtdGetTermOfDelivery(objCMTrx.EnumTermOfDelivery.CIF), objCMTrx.EnumTermOfDelivery.CIF))
        '    ddlTerm.Items.Add(New ListItem(objCMTrx.mtdGetTermOfDelivery(objCMTrx.EnumTermOfDelivery.FOB), objCMTrx.EnumTermOfDelivery.FOB))
        'End If

        If Trim(pv_strTerm) <> "" Then
            For intCnt = 0 To ddlTerm.Items.Count - 1
                If ddlTerm.Items(intCnt).Value = pv_strTerm Then
                    intSelectedIndex = intCnt
                End If
            Next
            ddlTerm.SelectedIndex = intSelectedIndex
        Else
            ddlTerm.SelectedIndex = 0
        End If

    End Sub

    Sub onChanged_ContractNo(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strParam As String
        Dim strOpCdGet As String = "CM_CLSTRX_CONTRACT_REG_GET"
        Dim strOpCdGet_DO As String = "CM_CLSTRX_DO_REG_QTYAMOUNT_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim strSearch As String
        Dim pv_strContNo As String = ddlContNo.SelectedItem.Value
        Dim dblQtyAmount As Double

        lblErrQty.visible = False
        lblMsgCloseDO.visible = False
        ddlBillParti.enabled = False
        ddlTerm.enabled = False
        lblErrContNo.visible = False
        taProductQuality.Disabled = False

        strSearch = "WHERE LocCode = '" & strLocation & "' and ContractNo like '%" & pv_strContNo & "' and status in ('1', '4') "
        If lblDONo.Text <> "" Then
            strSearch = strSearch & " and dono <> '" & Trim(lblDONo.text) & "' "
        End If
        strSearch = strSearch & " group by contractno "

        strparam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdGet_DO, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_DORegistrationDet.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            dblQtyAmount = trim(objContractDs.Tables(0).Rows(0).Item("qtyamount"))
        Else
            dblQtyAmount = 0
        End If

        strSearch = "and ctr.LocCode = '" & strLocation & "' and ctr.ContractNo like '%" & pv_strContNo & "' "
        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdGet, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_DORegistrationDet.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            txtContractQty.Text = Trim(objContractDs.Tables(0).Rows(0).Item("ContractQty"))
            'If Trim(txtDOQty.Text) = "" Then
            '    txtDOQty.Text = "0"
            'End If
            txtDOQty.Text = (CDbl(Trim(objContractDs.Tables(0).Rows(0).Item("ContractQty"))) - dblQtyAmount)
            txtRemContractQty.Text = (CDbl(Trim(objContractDs.Tables(0).Rows(0).Item("ContractQty"))) - CDbl(Trim(txtDOQty.Text)) - dblQtyAmount)
            txtRemContractQty1.Value = (CDbl(Trim(objContractDs.Tables(0).Rows(0).Item("ContractQty"))) - dblQtyAmount)
            lblProductCat.Text = objWMTrx.mtdGetWeighBridgeTicketProductCode(objContractDs.Tables(0).Rows(intCnt).Item("ProductCode"))
            lblProductFlag.Text = Trim(objContractDs.Tables(0).Rows(intCnt).Item("ProductCode"))
            BindBuyerList(Trim(objContractDs.Tables(0).Rows(0).Item("BuyerCode")))
            BindTermOfDelivery(Trim(objContractDs.Tables(0).Rows(0).Item("TermOfDelivery")))
            taProductSpesification.Value = Trim(objContractDs.Tables(0).Rows(intCnt).Item("ClaimQuality"))
            taProductQuality.Value = Trim(objContractDs.Tables(0).Rows(intCnt).Item("ProductQuality"))
            taLoadDest.Value = Trim(objContractDs.Tables(0).Rows(intCnt).Item("Consignment"))
            taDeliveryNote.Value = Trim(objContractDs.Tables(0).Rows(intCnt).Item("TimeOfDelivery"))
            'taLoadDest.Value = Trim(Replace(Replace(Trim(objContractDs.Tables(0).Rows(intCnt).Item("LoadDest")), "FRANCO", ""), "LOCO", ""))
        End If
        If CDbl(txtRemContractQty.Text) < 0 Then
            lblErrQty.Visible = True
            Exit Sub
        Else
            lblErrQty.Visible = False
        End If
    End Sub


    Sub Button_DeactivateDO(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_Upd As String = "CM_CLSTRX_DO_REG_UPD"
        Dim strOpCd_Get As String = "CM_CLSTRX_DO_REG_GET"
        Dim strOpCd_Add As String = "CM_CLSTRX_DO_REG_ADD"
        Dim strOppCd As String = "IN_CLSTRX_PURREQ_MOVEID"
        Dim strOpCd_check As String = "CM_CLSTRX_CONTRACT_QTYVALUE_GET"
        Dim strOpCd_checkbulk As String = "CM_CLSTRX_CONTRACT_QTYVALUEBULK_GET"
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String = ""
        Dim strstatus As String = ""
        Dim strTranPrefix As String = "CMDO"
        Dim objCMID As Object
        Dim strMatchingId As String
        Dim strBuyerCode As String
        Dim dblQtyExtra As Double
        Dim dblQtyLess As Double
        Dim dblQty As Double
        Dim strSearch As String = ""
        Dim objMatchDs As New Object()

        lblMsgUpdatingDO.visible = False
        lblMsgWMCheck.visible = False
        lblMsgGenQtyDOCheck.visible = False
        lblMsgMatchDOCheck.visible = False
        lblMsgQtyDOCheck.visible = False
        lblMsgCloseDO.visible = False

        strSearch = "and ma.LocCode = '" & strLocation & "' and ln.dono = '" & trim(lblDONo.text) & "' "

        strParam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContractMatch(strOpCd_checkbulk, strParam, 0, objMatchDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTMATCHDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/trx/CM_Trx_ContractMatchList.aspx")
        End Try

        If objMatchDs.Tables(0).Rows.Count > 0 Then
            strMatchingId = Trim(objMatchDs.Tables(0).Rows(0).Item("MatchingId"))
            strBuyerCode = Trim(objMatchDs.Tables(0).Rows(0).Item("BuyerCode"))
            dblQtyLess = Trim(objMatchDs.Tables(0).Rows(0).Item("qtyless"))
            dblQty = Trim(objMatchDs.Tables(0).Rows(0).Item("qty"))
        Else
            strMatchingId = ""
            strBuyerCode = ""
            lblMsgMatchDOCheck.visible = True
            Exit Sub
        End If

        If (dblQty < dblQtyLess) Then
            lblMsgQtyDOCheck.visible = True
            Exit Sub
        End If

        strstatus = objCMTrx.EnumContractStatus.Closed

        strParam = lblDONo.Text & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   trim(ddlContNo.SelectedItem.Value) & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   strstatus & Chr(9) & _
                   strLocation & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   ""

        Try
            intErrNo = objCMTrx.mtdUpdDOReg(strOpCd_GET, _
                                               strOpCd_ADD, _
                                               strOpCd_UPD, _
                                               strCompany, _
                                               strOppCd, _
                                               strLocation, _
                                               strUserId, _
                                               strParam, _
                                               False, _
                                               objCMID, _
                                               True, _
                                               strTranPrefix)

            lblMsgCloseDO.visible = True


        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_TRX_DOREGDET_ADD&errmesg=" & Exp.Message & "&redirect=CM/Trx/CM_Trx_DORegistrationDet.aspx")
        End Try

        UpdateStatusContract()
        lblShowGenInvoice.Text = "no"
        onload_display(strDONo, "")
        onLoad_BindButton()
    End Sub


    Sub GetRemainingQty(ByVal pv_strContNo As String)
        Dim strParam As String
        Dim strOpCdGet As String = "CM_CLSTRX_DO_REG_REMQTY_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer
        Dim strSearch As String
        Dim dblQtyAmount As Double

        strSearch = "WHERE LocCode = '" & strLocation & "' and ContractNo like '%" & pv_strContNo & "' "

        strparam = strSearch & "|" & ""

        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdGet, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_DORegistrationDet.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            txtRemContractQty.text = trim(objContractDs.Tables(0).Rows(0).Item("remainingqty"))
            txtRemContractQty1.value = CDbl(trim(objContractDs.Tables(0).Rows(0).Item("remainingqty")))
        End If
    End Sub

    Sub ValidateGenInvBtn()
        Dim strOpCdGet = "CM_CLSTRX_CONTRACTMATCH_LN_GET"
        Dim strparam As String = ""
        Dim intErrNo As Integer
        Dim strSearch As String = ""
        Dim dblContQty As Double
        Dim dblDOQty As Double
        Dim strInvoiceNo As String
        Dim strContractNo As String = ""

        strContractNo = Trim(ddlContNo.SelectedItem.Value)

        strSearch = " and ctr.LocCode = '" & strLocation & "' and ln.ContractNo like '%" & strContractNo & "' and ln.dono = '" & trim(lblDONo.text) & "' "
        strParam = strSearch & "|" & ""


        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCdGet, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            strInvoiceNo = Trim(objContractDs.Tables(0).Rows(0).Item("InvoiceId"))
        Else
            dblContQty = 0
            dblDOQty = 0
            strInvoiceNo = ""
        End If


        If strInvoiceNo = "" Then
            If intstatus = objCMTrx.EnumContractStatus.Closed Or intstatus = objCMTrx.EnumContractStatus.Deleted Then
                GenInvoiceBtn.visible = False
            Else
                GenInvoiceBtn.Visible = False
            End If
            lblShowGenInvoice.Text = "yes"
            'GenInvoiceBtn.Visible = False
        Else
            lblShowGenInvoice.Text = "no"
            GenInvoiceBtn.visible = False
        End If


    End Sub

    Sub BindNPWP()
        Dim strOpCd_Get As String = "ADMIN_CLSLOC_LOCATION_LIST_GET"
        Dim strParam As String = ""
        Dim intErrNo As Integer
        Dim objLocCodeDs As New Object()


        strParam = strLocation & "||||" & " loccode " & "||"

        Try
            intErrNo = objAdminLoc.mtdGetLocCode(strOpCd_Get, strParam, objLocCodeDs)
        Catch Exp As Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADMIN_GET_LOCATION&errmesg=" & lblErrMessage.Text & "&redirect=admin/location/admin_location_locdet.aspx")
        End Try

        If objLocCodeDs.Tables(0).Rows.Count > 0 Then
            txtNPWP.text = trim(objLocCodeDs.Tables(0).Rows(0).Item("NPWP"))
            txtAddress.Value = trim(objLocCodeDs.Tables(0).Rows(0).Item("Address"))
        End If

    End Sub

    Sub UpdateStatusContract()
        Dim strOpCd_Upd As String = "CM_CLSTRX_CONTRACT_REG_UPD"
        Dim strOpCd_Get As String = "CM_CLSTRX_CONTRACT_REG_GET"
        Dim strOpCd_Add As String = "CM_CLSTRX_CONTRACT_REG_ADD"
        Dim strOpCd_CheckQty As String = "CM_STDRPT_DO_REG_CONTRACT_UPD"
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String = ""
        Dim strContractNo As String = ""
        Dim strSearch As String = ""
        Dim objCMID As Object

        Dim strOppCd As String = "IN_CLSTRX_PURREQ_MOVEID"
        Dim strOppCd_Back As String = "IN_CLSTRX_PURREQ_BACKID"
        Dim strOppCd_GetID As String = "IN_CLSTRX_PURREQ_GETID"

        strContractNo = Trim(ddlContNo.SelectedItem.Value)


        strSearch = " where b.LocCode = '" & strLocation & "' and a.ContractNo like '%" & strContractNo & "' "
        strSearch = strSearch & " and c.accmonth = '" & trim(strAccMonth) & "' and c.accyear = '" & trim(strAccYear) & "'"

        strParam = strSearch & "|" & ""


        Try
            intErrNo = objCMTrx.mtdGetContract(strOpCd_CheckQty, strParam, 0, objContractDs)
        Catch Exp As System.Exception
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_GET&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegList.aspx")
        End Try

        If objContractDs.Tables(0).Rows.Count > 0 Then
            If CDbl(objContractDs.Tables(0).Rows(0).Item("totalqty")) >= CDbl(objContractDs.Tables(0).Rows(0).Item("contractqty")) Then
                strParam = Trim(ddlContNo.SelectedItem.Value) & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           Trim(objCMTrx.EnumContractStatus.Closed) & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           "" & Chr(9) & _
                           ""

                Try
                    intErrNo = objCMTrx.mtdUpdContract(strOpCd_Get, _
                                                       strOpCd_Add, _
                                                       strOpCd_Upd, _
                                                       strOppCd, _
                                                       strCompany, _
                                                       strLocation, _
                                                       strUserId, _
                                                       strParam, _
                                                       False, _
                                                       objCMID, _
                                                       True, _
                                                       "CMR")

                Catch Exp As System.Exception
                    Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CM_TRX_CONTRACTREGDET_SAVE&errmesg=" & lblErrMessage.Text & "&redirect=CM/Trx/CM_Trx_ContractRegDet.aspx?tbcode=" & strContractNo)
                End Try
            End If
        End If
    End Sub


    Sub onChange_DODestination(ByVal Sender As Object, ByVal E As EventArgs)
        Dim strDODestination As String
        strDODestination = rdDODest.SelectedItem.Value

        If rdDODest.SelectedValue = "1" Then
            ddlContNo.enabled = True
            ddlBillParti.enabled = True
            txtContractQty.enabled = False
            txtRemContractQty.enabled = False
            DeactivateDOBtn.visible = True
            ddlProduct.Enabled = False
            lblProduct.visible = False
            ddlProduct.visible = False
            GenInvoiceBtn.Visible = False
            lblQtyMatched.visible = False
            txtQtyMatched.visible = False
        Else
            ddlContNo.enabled = False
            ddlBillParti.enabled = False
            ddlContNo.selectedindex = 0
            ddlBillParti.selectedindex = 0
            txtContractQty.enabled = False
            txtRemContractQty.enabled = False
            txtContractQty.Text = 0
            txtRemContractQty.Text = 0
            DeactivateDOBtn.visible = True
            ddlProduct.Enabled = True
            lblProduct.visible = True
            ddlProduct.visible = True
            GenInvoiceBtn.visible = False
            lblQtyMatched.visible = False
            txtQtyMatched.visible = False
            txtRemContractQty.text = "0"
        End If
    End Sub

    Sub BindProductList(ByVal pv_strProdCode As String)
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        If ddlProduct.Items.Count = 0 Then
            ddlProduct.Items.Add(New ListItem("Select Product", ""))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.CPO), objWMTrx.EnumWeighBridgeTicketProduct.CPO))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.PK), objWMTrx.EnumWeighBridgeTicketProduct.PK))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.EFB), objWMTrx.EnumWeighBridgeTicketProduct.EFB))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Shell), objWMTrx.EnumWeighBridgeTicketProduct.Shell))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang), objWMTrx.EnumWeighBridgeTicketProduct.AbuJanjang))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Fiber), objWMTrx.EnumWeighBridgeTicketProduct.Fiber))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Brondolan), objWMTrx.EnumWeighBridgeTicketProduct.Brondolan))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Solid), objWMTrx.EnumWeighBridgeTicketProduct.Solid))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah), objWMTrx.EnumWeighBridgeTicketProduct.MinyakLimbah))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.FFB), objWMTrx.EnumWeighBridgeTicketProduct.FFB))
            ddlProduct.Items.Add(New ListItem(objWMTrx.mtdGetWeighBridgeTicketProduct(objWMTrx.EnumWeighBridgeTicketProduct.Others), objWMTrx.EnumWeighBridgeTicketProduct.Others))
        End If

        If Trim(pv_strProdCode) <> "" Then
            For intCnt = 0 To ddlProduct.Items.Count - 1
                If ddlProduct.Items(intCnt).Value = pv_strProdCode Then
                    intSelectedIndex = intCnt
                End If
            Next
            ddlProduct.SelectedIndex = intSelectedIndex
        Else
            ddlProduct.SelectedIndex = 0
        End If
    End Sub

    Sub BindTransporterList(ByVal pv_strBuyerCode As String)
        Dim strParam As String
        Dim strOpCdGet As String = "WM_CLSSETUP_TRANSPORTER_GET"
        Dim dr As DataRow
        Dim intErrNo As Integer
        Dim intCnt As Integer
        Dim intSelectedIndex As Integer

        strParam = "||" & objWMSetup.EnumTransporterStatus.Active & "|||TransporterCode|"
        Try
            intErrNo = objWMSetup.mtdGetTransporter(strOpCdGet, strParam, objBuyerDs)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=WM_CLSTRX_WEIGHBRIDGE_TICKET_TRANSPORTER_DROPDOWNLIST_GET&errmesg=" & lblErrMessage.Text & "&redirect=WM/trx/WM_trx_WeighBridgeTicketDet.aspx")
        End Try

        If objBuyerDs.Tables(0).Rows.Count > 0 Then
            For intCnt = 0 To objBuyerDs.Tables(0).Rows.Count - 1
                objBuyerDs.Tables(0).Rows(intCnt).Item("TransporterCode") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("TransporterCode"))
                objBuyerDs.Tables(0).Rows(intCnt).Item("Name") = Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("TransporterCode")) & " (" & Trim(objBuyerDs.Tables(0).Rows(intCnt).Item("Name")) & ")"
                If objBuyerDs.Tables(0).Rows(intCnt).Item("TransporterCode") = pv_strBuyerCode Then
                    intSelectedIndex = intCnt + 1
                End If
            Next
        End If

        dr = objBuyerDs.Tables(0).NewRow()
        dr("TransporterCode") = ""
        dr("Name") = "Please Select Transporter Code"
        objBuyerDs.Tables(0).Rows.InsertAt(dr, 0)

        ddlTransporter.DataSource = objBuyerDs.Tables(0)
        ddlTransporter.DataValueField = "TransporterCode"
        ddlTransporter.DataTextField = "Name"
        ddlTransporter.DataBind()
        ddlTransporter.SelectedIndex = intSelectedIndex
    End Sub

	
	Sub btnDelete_Click (ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCd_Upd As String = "CM_CLSTRX_DO_REG_UPD"
        Dim strOpCd_Get As String = "CM_CLSTRX_DO_REG_GET"
        Dim strOpCd_Add As String = "CM_CLSTRX_DO_REG_ADD"
        Dim strOppCd As String = "IN_CLSTRX_PURREQ_MOVEID"
        Dim strOpCd_check As String = "CM_CLSTRX_CONTRACT_QTYVALUE_GET"
        Dim strOpCd_checkbulk As String = "CM_CLSTRX_CONTRACT_QTYVALUEBULK_GET"
        Dim intErrNo As Integer
        Dim blnIsUpdate As Boolean
        Dim strParam As String = ""
        dim strstatus as string = ""
        Dim strTranPrefix as string = "CMDO"
        Dim objCMID As Object
        Dim strMatchingId as string 
        Dim strBuyerCode As String
        Dim dblQtyExtra as double
        Dim dblQtyLess as double
        Dim dblQty as double
        Dim strSearch as string = ""
        Dim objMatchDs as New Object()

        lblMsgUpdatingDO.visible = false 
        lblMsgWMCheck.visible = false 
        lblMsgGenQtyDOCheck.visible = false
        lblMsgMatchDOCheck.visible = false 
        lblMsgQtyDOCheck.visible = false
        lblMsgCloseDO.visible = false 
        
        strstatus = objCMTrx.EnumContractStatus.Deleted

        strParam = lblDONo.Text & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _            
                   trim(ddlContNo.SelectedItem.Value) & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   strstatus & Chr(9) & _
                   strLocation & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & "" & Chr(9)& "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   "" & Chr(9) & _
                   ""

        Try
            intErrNo = objCMTrx.mtdUpdDOReg(strOpCd_GET, _
                                               strOpCd_ADD, _
                                               strOpCd_UPD, _
                                               strCompany, _
                                               strOppCd, _
                                               strLocation, _
                                               strUserId, _
                                               strParam, _
                                               False, _
                                               objCMID, _
                                               True, _
                                               strTranPrefix)
            
             lblMsgCloseDO.visible = true 


        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=CM_TRX_DOREGDET_ADD&errmesg=" & Exp.Message & "&redirect=CM/Trx/CM_Trx_DORegistrationDet.aspx")
        End Try

        UpdateStatusContract()
        lblShowGenInvoice.Text = "no"
        onload_display(strDONo,"")
        onLoad_BindButton()
    End Sub
	
	Sub NewTBBtn_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CM_Trx_DORegistrationDet.aspx")
    End Sub


End Class
