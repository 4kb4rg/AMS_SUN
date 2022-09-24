
Imports System
Imports System.Data
Imports System.Math
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.Page
Imports System.Web.UI.Control
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Strings
Imports agri.GlobalHdl.clsGlobalHdl
Imports agri.CT
Imports agri.GL
Imports agri.PWSystem.clsLangCap

Public Class IN_CanteenItem : Inherits Page

    Dim dsStockItem AS Dataset

    Protected objCT As New agri.CT.clsSetup()
    Protected objIN As New agri.IN.clsSetup()
    Protected objGL As New agri.GL.clsSetup()
    Protected objAdmin As New agri.Admin.clsUOM()
    Protected objGlobal As New agri.GlobalHdl.clsGlobalHdl()
    Protected objAdminLoc As New agri.Admin.clsLoc()

    Protected WithEvents ErrorMessage as Label
    Protected WithEvents lblErrMessage as Label
    Protected WithEvents blnUpdate as Label
    Protected WithEvents CTItemCode as textBox
    Protected WithEvents Desc as textBox
    Protected WithEvents Status as Label
    Protected WithEvents CreateDate as Label
    Protected WithEvents UpdateDate as Label
    Protected WithEvents lblDupMsg as Label
    Protected WithEvents UpdateBy as Label
    Protected WithEvents lstItemCode As DropDownList
    Protected WithEvents txtBin As TextBox
    Protected WithEvents Fuel_Yes As RadioButton
    Protected WithEvents Fuel_No As RadioButton
    Protected WithEvents Fuel_Lub As RadioButton
    Protected WithEvents InitialCost As Label
    Protected WithEvents HighestCost as Label
    Protected WithEvents LowestCost as Label
    Protected WithEvents AvrgCost as Label
    Protected WithEvents LatestCost as Label
    Protected WithEvents DiffAverageCost as Label
    Protected WithEvents purchaseACNo as TextBox
    Protected WithEvents IssueACNo as TextBox
    Protected WithEvents LatestPrice as TextBox
    Protected WithEvents AvrgPrice as TextBox
    Protected WithEvents FixedPrice as TextBox
    Protected WithEvents HandQty as Label
    Protected WithEvents HoldQty as Label
    Protected WithEvents OrderQty as Label
    Protected WithEvents ReorderLvl as TextBox
    Protected WithEvents ReorderQty as TextBox
    Protected WithEvents Remark as TextBox
    Protected WithEvents Fixed_Price as RadioButton
    Protected WithEvents Latest_Price as RadioButton
    Protected WithEvents Avrg_Price as RadioButton
    Protected WithEvents lstProdType as DropDownList
    Protected WithEvents lstProdCat as DropDownList
    Protected WithEvents lstProdBrand as DropDownList
    Protected WithEvents lstProdModel as DropDownList
    Protected WithEvents lstProdMat as DropDownList
    Protected WithEvents lstStockAnalysis as DropDownList
    Protected WithEvents lstVehExpense As DropDownList
    Protected WithEvents PurchaseUOM as DropDownList
    Protected WithEvents InventoryUOM as DropDownList
    Protected WithEvents lstAccCode as DropDownList
    Protected WithEvents Delete as ImageButton
    Protected WithEvents Save as ImageButton
    Protected WithEvents SynchronizeData as ImageButton
    Protected WithEvents Label2 as Label
    Protected WithEvents Label3 as Label
    Protected WithEvents Label5 as Label
    Protected WithEvents Label6 as Label
    Protected WithEvents Label7 as Label
    Protected WithEvents Label8 as Label
    Protected WithEvents Label9 as Label
    Protected WithEvents Label10 as Label
    Protected WithEvents lblCode as Label
    Protected WithEvents lblChargeTo as Label

    Dim strOppCd_GET As String = "IN_CLSSETUP_INVITEM_DETAILS_GET"
    Dim strOppCd_ADD As String = "IN_CLSSETUP_INVITEM_DETAILS_ADD"
    Dim strOppCd_UPD As String = "IN_CLSSETUP_INVITEM_DETAILS_UPD"

    Dim strOppCd_ItemList_GET As String = "IN_CLSSETUP_INVITEM_LIST_GET"

    Dim strOppCd_ProdType_GET As String = "IN_CLSSETUP_PRODTYPE_LIST_GET"
    Dim strOppCd_ProdCat_GET As String = "IN_CLSSETUP_PRODCAT_LIST_GET"
    Dim strOppCd_ProdBrand_GET As String = "IN_CLSSETUP_PRODBRAND_LIST_GET"
    Dim strOppCd_ProdModel_GET As String = "IN_CLSSETUP_PRODMODEL_LIST_GET"
    Dim strOppCd_ProdMaterial_GET As String = "IN_CLSSETUP_PRODMAT_LIST_GET"
    Dim strOppCd_StockAna_GET As String = "IN_CLSSETUP_STOCKANA_LIST_GET"
    Dim strOppCd_VehExpense_GET As String = "GL_CLSSETUP_VEHEXPENSE_LIST_GET"
    Dim strOppCd_AccountCode_GET As String = "GL_CLSSETUP_ACCOUNTCODE_LIST_GET"
    Dim strOppCd_UOM_GET As String = "ADMIN_CLSUOM_UOM_LIST_GET"

    Dim objAR As New agri.GlobalHdl.clsAccessRights()
    Dim objLangCap As New agri.PWSystem.clsLangCap()

    Dim objDataSet As New Dataset()
    Dim objLangCapDs As New Dataset()

    Dim ItemCode As String 
    Dim strParam As String 
    Dim strCompany As String
    Dim strLocation As String
    Dim strUserId As String
    Dim strAccMonth As String
    Dim strAccYear As String
    Dim strLangCode As String
    Dim intCTAR As Integer
    Dim intErrNo As Integer
    Dim strAccountTag As String
    Dim strLocType as String

    Sub Page_Load(Sender As Object, E As EventArgs)
        strCompany = Session("SS_COMPANY")
        strLocation = Session("SS_LOCATION")
        strUserId = Session("SS_USERID")
        strLangCode = Session("SS_LANGCODE")
        strAccMonth = Session("SS_INACCMONTH")
        strAccYear = Session("SS_INACCYEAR")
        intCTAR = Session("SS_CTAR")
        strLocType = Session("SS_LOCTYPE")
        
        If strUserId = "" Then
            Response.Redirect("/SessionExpire.aspx")
        ElseIf objAR.mtdHasAccessRights(objAR.mtdGetAccessRights(objAR.EnumCTAccessRights.CTItem), intCTAR) = False Then
            Response.Redirect("/" & strLangCode & "/include/mesg/AccessRights.aspx")

        Else
            onload_GetLangCap()
            If Not IsPostBack Then
                ItemCode = Request.QueryString("ID")

                If Not ItemCode = "" Then
                    DisplayData()
                    DisplayDropDown()
                    BindItemCodeDropList()

                    blnUpdate.Text = True
                    lstItemCode.Enabled = False
                    DisableControl()
                Else
                    DisplayDropDown()
                    BindItemCodeDropList()
                    ItemCode = lstItemCode.SelectedItem.Value
                    DisplayDropDown()
                    blnUpdate.Text = False
                    Delete.Visible = False
                End If
            End If
        End If
   End Sub


    Sub onload_GetLangCap()
        GetEntireLangCap()
        strAccountTag = GetCaption(objLangCap.EnumLangCap.Account)
        Label2.text = GetCaption(objLangCap.EnumLangCap.ProdType)
        Label3.text = GetCaption(objLangCap.EnumLangCap.ProdCat)
        Label5.text = GetCaption(objLangCap.EnumLangCap.ProdBrand)
        Label6.text = GetCaption(objLangCap.EnumLangCap.ProdModel)
        Label7.text = GetCaption(objLangCap.EnumLangCap.ProdMat)
        Label8.text = GetCaption(objLangCap.EnumLangCap.StockAnalysis)
        Label9.text = GetCaption(objLangCap.EnumLangCap.VehExpense) & lblCode.text
        Label10.text = lblChargeTo.text & strAccountTag & lblCode.text
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
            Response.Redirect("/include/mesg/ErrorMessage.aspx?errcode=CT_SETUP_CANTEEN_ITEM_GET_LANGCAP&errmesg=" & lblErrMessage.Text & "&redirect=ct/setup/CT_CanteenItem.aspx")
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



    Protected Function LoadData(ByVal pv_blnIsMaster As Boolean) As DataSet
        strParam =  ItemCode & "|" & objCT.EnumInventoryItemType.CanteenItem
        If Not Request.QueryString("ID") = "" Then
            If pv_blnIsMaster = False Then
                strOppCd_GET = "IN_CLSSETUP_INVITEM_DETAILS_GET"
            Else
                strOppCd_GET = "IN_CLSSETUP_INVMASTER_DETAILS_GET"
            End If
        Else
            strOppCd_GET = "IN_CLSSETUP_INVMASTER_DETAILS_GET"
        End If

        Try
            intErrNo = objCT.mtdGetMasterDetail(strOppCd_GET, strParam, objDataSet)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_CANTEENITEM_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=CT/Setup/CT_CanteenItem.aspx")
        End Try
        Return objDataSet
    End Function
    
    Sub DisableControl()
        CTItemCode.Enabled = False
        Desc.Enabled = False
        purchaseACNo.Enabled = False
        IssueACNo.Enabled = False
        lstProdType.Enabled = False
        lstProdCat.Enabled = False
        lstProdBrand.Enabled = False
        lstProdModel.Enabled = False
        lstProdMat.Enabled = False
        lstStockAnalysis.Enabled = False
        PurchaseUOM.Enabled = False
        InventoryUOM.Enabled = False
        Fuel_Yes.Enabled = False
        Fuel_No.Enabled = False
        Fuel_Lub.Enabled = False
        lstVehExpense.Enabled = False
        lstAccCode.Enabled = False

        If dsStockItem.Tables(0).Rows(0).Item("Status") = objIN.EnumStockItemStatus.Deleted Then
            Delete.ImageUrl = "../../images/butt_Undelete.gif"
            Delete.Attributes("onclick") = ""
            Fixed_Price.Enabled = False
            Latest_Price.Enabled = False
            Avrg_Price.Enabled = False
            ReorderLvl.Enabled = False
            ReorderQty.Enabled = False
            LatestPrice.Enabled = False
            AvrgPrice.Enabled = False
            FixedPrice.Enabled = False
            Remark.Enabled = False
            InitialCost.Enabled = False
            HighestCost.Enabled = False
            LowestCost.Enabled = False
            AvrgCost.Enabled = False
            LatestCost.Enabled = False
            HandQty.Enabled = False
            HoldQty.Enabled = False
            OrderQty.Enabled = False
            UpdateDate.Enabled = False
            UpdateBy.Enabled = False
            Status.Enabled = False
            CreateDate.Enabled = False
            txtBin.Enabled = False
        Else
            Delete.Attributes("onclick") = "javascript:return ConfirmAction('delete');"
        End If
    End Sub

    Sub DisplayData()
        Dim objMasterDs As New Dataset()
        dsStockItem = LoadData(False)

        CTItemCode.Text = Trim(dsStockItem.Tables(0).Rows(0).Item("ItemCode"))
        Desc.Text = Trim(dsStockItem.Tables(0).Rows(0).Item("Description"))
        Status.Text = Trim(objCT.mtdGetStockItemStatus(dsStockItem.Tables(0).Rows(0).Item("Status")))
        CreateDate.Text = Trim(objGlobal.Getlongdate(dsStockItem.Tables(0).Rows(0).Item("CreateDate")))
        UpdateDate.Text = Trim(objGlobal.Getlongdate(dsStockItem.Tables(0).Rows(0).Item("UpdateDate")))
        UpdateBy.Text = Trim(dsStockItem.Tables(0).Rows(0).Item("UserName"))
        InitialCost.Text = ObjGlobal.GetIDDecimalSeparator(dsStockItem.Tables(0).Rows(0).Item("InitialCost"))
        HighestCost.Text = ObjGlobal.GetIDDecimalSeparator(dsStockItem.Tables(0).Rows(0).Item("HighCost"))
        LowestCost.Text = ObjGlobal.GetIDDecimalSeparator(dsStockItem.Tables(0).Rows(0).Item("LowCost"))
        AvrgCost.Text = ObjGlobal.GetIDDecimalSeparator(dsStockItem.Tables(0).Rows(0).Item("AverageCost"))
        LatestCost.Text = ObjGlobal.GetIDDecimalSeparator(dsStockItem.Tables(0).Rows(0).Item("LatestCost"))
        purchaseACNo.Text = Trim(dsStockItem.Tables(0).Rows(0).Item("purchaseACcNo"))
        IssueACNo.Text = Trim(dsStockItem.Tables(0).Rows(0).Item("IssueACcNo"))
        LatestPrice.Text = FormatNumber(dsStockItem.Tables(0).Rows(0).Item("SellLatestCost"), 5, True, False, False)
        AvrgPrice.Text = FormatNumber(dsStockItem.Tables(0).Rows(0).Item("SellAverageCost"), 5, True, False, False)
        DiffAverageCost.Text = FormatNumber(dsStockItem.Tables(0).Rows(0).Item("DiffAverageCost"), 5, True, False, False)        
        FixedPrice.Text = ObjGlobal.DisplayForEditCurrencyFormat(dsStockItem.Tables(0).Rows(0).Item("SellFixedPrice"))
        HandQty.Text = ObjGlobal.GetIDDecimalSeparator_FreeDigit(dsStockItem.Tables(0).Rows(0).Item("QtyonHand"), 5)
        HoldQty.Text = ObjGlobal.GetIDDecimalSeparator_FreeDigit(dsStockItem.Tables(0).Rows(0).Item("QtyOnHold"), 5)
        OrderQty.Text = ObjGlobal.GetIDDecimalSeparator_FreeDigit(dsStockItem.Tables(0).Rows(0).Item("QtyOnOrder"), 5)
        ReorderLvl.Text = FormatNumber(dsStockItem.Tables(0).Rows(0).Item("ReOrderLevel"), 5, True, False, False)
        ReorderQty.Text = FormatNumber(dsStockItem.Tables(0).Rows(0).Item("QtyReOrder"), 5, True, False, False)
        txtBin.Text = Trim(dsStockItem.Tables(0).Rows(0).Item("Bin"))
        Remark.Text = Trim(dsStockItem.Tables(0).Rows(0).Item("Remark"))
    
        If dsStockItem.Tables(0).Rows(0).Item("FuelTypeInd") = objIN.EnumFuelItemIndicator.Fuel Then
            Fuel_Yes.Checked = True
            Fuel_Lub.Checked = False
            Fuel_No.Checked = False
        ElseIf dsStockItem.Tables(0).Rows(0).Item("FuelTypeInd") = objIN.EnumFuelItemIndicator.Lubricant Then
            Fuel_Yes.Checked = False
            Fuel_Lub.Checked = True
            Fuel_No.Checked = False
        Else
            Fuel_Yes.Checked = False
            Fuel_Lub.Checked = False
            Fuel_No.Checked = True
        End If

        If dsStockItem.Tables(0).Rows(0).Item("UsePrice") = objCT.EnumSellingPriceForItem.Fixed Then
            Fixed_Price.Checked = True
        ElseIf dsStockItem.Tables(0).Rows(0).Item("UsePrice") = objCT.EnumSellingPriceForItem.PercentageOfLatestCost Then
            Latest_Price.Checked = True
        ElseIf dsStockItem.Tables(0).Rows(0).Item("UsePrice") = objCT.EnumSellingPriceForItem.PercentageOfAverageCost Then
            Avrg_Price.Checked = True
        End If

        objMasterDs = LoadData(True)

        If objMasterDs.Tables(0).Rows.Count > 0 Then
            If objMasterDs.Tables(0).Rows(0).Item("Status") = objCT.EnumStockItemStatus.Deleted And _
                dsStockItem.Tables(0).Rows(0).Item("Status") = objCT.EnumStockItemStatus.Deleted Then
                Delete.Visible = False
            End If
        End If
    End Sub

    Sub DisplayDropDown()

        BindDropList(strOppCd_ProdType_GET, "ProdTypeCode", "ProdTypeCode", objCT.EnumInventoryMasterType.ProductType, _
                       objGlobal.EnumModule.Inventory, objCT.EnumProductTypeStatus.Active, lstProdType)
        BindDropList(strOppCd_ProdCat_GET, "ProdCatCode", "ProdCatCode", objCT.EnumInventoryMasterType.ProductCategory, _
                        objGlobal.EnumModule.Inventory, objCT.EnumProductCategoryStatus.Active, lstProdCat)
        BindDropList(strOppCd_ProdBrand_GET, "ProdBrandCode", "ProdBrandCode", objCT.EnumInventoryMasterType.ProductBrand, _
                        objGlobal.EnumModule.Inventory, objCT.EnumProductBrandStatus.Active, lstProdBrand)
        BindDropList(strOppCd_ProdModel_GET, "ProdModelCode", "ProdModelCode", objCT.EnumInventoryMasterType.ProductModel, _
                        objGlobal.EnumModule.Inventory, objCT.EnumProductModelStatus.Active, lstProdModel)
        BindDropList(strOppCd_ProdMaterial_GET, "ProdMatCode", "ProdMatCode", objCT.EnumInventoryMasterType.ProductMaterial, _
                        objGlobal.EnumModule.Inventory, objCT.EnumProductMaterialStatus.Active, lstProdMat)
        BindDropList(strOppCd_StockAna_GET, "StockAnalysisCode", "StockAnalysisCode", objCT.EnumInventoryMasterType.StockAnalysis, _
                        objGlobal.EnumModule.Inventory, objCT.EnumStockAnalysisStatus.Active, lstStockAnalysis)
        BindDropList(strOppCd_VehExpense_GET, "ExpenseCode", "VehExpenseCode", objGL.EnumGLMasterType.VehicleExpense, _
                        objGlobal.EnumModule.GeneralLedger, objGL.EnumVehicleExpenseStatus.Active, lstVehExpense)
        BindDropList(strOppCd_AccountCode_GET, "ActCode", "AccCode", objGL.EnumGLMasterType.AccountCode, _
                        objGlobal.EnumModule.GeneralLedger, objGL.EnumAccountCodeStatus.Active, lstAccCode)
        BindDropList(strOppCd_UOM_GET, "PurchaseUOM", "UOMCode", "0", _
                         objGlobal.EnumModule.Admin, objAdmin.EnumUOMStatus.Active, PurchaseUOM)
        BindDropList(strOppCd_UOM_GET, "UOMCode", "UOMCode", "0", _
                         objGlobal.EnumModule.Admin, objAdmin.EnumUOMStatus.Active, InventoryUOM)
    End Sub

    Sub BindDropList(ByVal Oppcode As String, ByVal ItemKeyField As String, ByVal MasterKeyField As String, ByVal MasterType As Integer, _
    ByVal ModuleType As Integer, ByVal Status As Integer, ByVal BindToList As DropDownList)

        Dim strParam As String
        Dim dsForDropDown As DataSet
        Dim dsForInactiveItem As DataSet
        Dim SelectedIndex As Integer = -1
        Dim intCnt As Integer
        Dim TblAlias As String
        Dim DataTextField As String
        Dim drinsert As DataRow

        If ModuleType = objGlobal.EnumModule.Inventory Then
            Select Case MasterType
                Case objCT.EnumInventoryMasterType.ProductType
                    TblAlias = "PType"
                Case objCT.EnumInventoryMasterType.ProductCategory
                    TblAlias = "PCat"
                Case objCT.EnumInventoryMasterType.ProductBrand
                    TblAlias = "PBrand"
                Case objCT.EnumInventoryMasterType.ProductModel
                    TblAlias = "PModel"
                Case objCT.EnumInventoryMasterType.ProductMaterial
                    TblAlias = "PMat"
                Case objCT.EnumInventoryMasterType.StockAnalysis
                    TblAlias = "StockAna"
            End Select
        ElseIf ModuleType = objGlobal.EnumModule.GeneralLedger Then
            Select Case MasterType
                Case objGL.EnumGLMasterType.VehicleExpense
                    TblAlias = "Veh"
                Case objGL.EnumGLMasterType.AccountCode
                    TblAlias = "Acc"
            End Select
        ElseIf ModuleType = objGlobal.EnumModule.Admin Then
            TblAlias = "UOM"
        End If

        If ModuleType = objGlobal.EnumModule.Admin Then
            DataTextField = "UOMDESC"
        Else
            DataTextField = "Description"
        End If

        strParam = "ORDER BY " & MasterKeyField & "|" & "AND " & TblAlias & ".Status =" & Status
        If ModuleType = objGlobal.EnumModule.GeneralLedger And MasterType = objGL.EnumGLMasterType.AccountCode Then
            strParam += " AND " & TblAlias & ".AccType=" & objGL.EnumAccountType.BalanceSheet & _
                        " AND " & TblAlias & ".NurseryInd=" & objGL.EnumNurseryAccount.No
        End If

        Try
            Select Case ModuleType
                Case objGlobal.EnumModule.Admin
                    intErrNo = objAdmin.mtdGetMasterList(Oppcode, strParam, dsForDropDown)
                Case objGlobal.EnumModule.GeneralLedger
                    intErrNo = objGL.mtdGetMasterList(Oppcode, strParam, MasterType, dsForDropDown)
                Case objGlobal.EnumModule.Inventory
                    intErrNo = objCT.mtdGetMasterList(Oppcode, strParam, MasterType, dsForDropDown)
            End Select
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_CANTEENITEM_DETAILS&errmesg=" & lblErrMessage.Text & "&redirect=CT/Setup/CT_CanteenItem.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If Not ItemCode = "" Then
                If Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(MasterKeyField)) = Trim(dsStockItem.Tables(0).Rows(0).Item(ItemKeyField)) Then
                    SelectedIndex = intCnt + 1
                End If
            End If
        Next intCnt

        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = " "
        drinsert(1) = "Please Select a Code "
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        BindToList.DataSource = dsForDropDown.Tables(0)
        BindToList.DataValueField = MasterKeyField
        BindToList.DataTextField = DataTextField
        BindToList.DataBind()

        If SelectedIndex = -1 And Not ItemCode = "" Then
            strParam = "Order by " & MasterKeyField & "|" & "AND " & MasterKeyField & " = '" & _
                        Trim(dsStockItem.Tables(0).Rows(0).Item(ItemKeyField)) & "'"

            Try
                intErrNo = objCT.mtdGetMasterList(Oppcode, strParam, MasterType, dsForInactiveItem)
                If dsForInactiveItem.Tables(0).Rows.Count > 0 Then  
                    BindToList.Items.Add(New ListItem(Trim(dsForInactiveItem.Tables(0).Rows(0).Item(0)) & _
                     " (" & objGlobal.mtdGetItemDBStatus(objGlobal.EnumItemDBStatus.Deleted) & ") ", Trim(dsForInactiveItem.Tables(0).Rows(0).Item(0))))
                    SelectedIndex = BindToList.Items.Count - 1
                Else 
                    BindToList.Items.Add(New ListItem(Trim(dsStockItem.Tables(0).Rows(0).Item(ItemKeyField)) & _
                       " (" & objGlobal.mtdGetItemDBStatus(objGlobal.EnumItemDBStatus.Unknown) & ") ", Trim(dsStockItem.Tables(0).Rows(0).Item(ItemKeyField))))
                    SelectedIndex = 0
                End If

            Catch Exp As System.Exception
                Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Setup/CT_CanteenItem.aspx")
            End Try

        End If

        BindToList.SelectedIndex = SelectedIndex

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
        If Not dsForInactiveItem Is Nothing Then
            dsForInactiveItem = Nothing
        End If
    End Sub

    Sub BindItemCodeDropList()

        Dim strOpCdItemCode_Get As String = "IN_CLSSETUP_INVMASTER_LIST_GET"
        Dim dsForDropDown As DataSet
        Dim SelectedIndex As Integer = -1
        Dim intCnt As Integer
        Dim TblAlias As String
        Dim DataTextField As String
        Dim SearchStr As String
        Dim strParam As String
        Dim drinsert As DataRow

        SearchStr = "AND ItemType = '" & objIN.EnumInventoryItemType.CanteenItem & "' AND itm.Status = '" & objIN.EnumStockItemStatus.Active & "' AND Not ItemCode In (Select ItemCode From IN_Item where ItemType =" & objIN.EnumInventoryItemType.CanteenItem & " ) "

        strParam = "ORDER BY ItemCode Desc|" & SearchStr

        Try
            intErrNo = objIN.mtdGetMasterList(strOpCdItemCode_Get, strParam, objIN.EnumInventoryMasterType.StockItem, dsForDropDown)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=GET_STOCKITEM&errmesg=" & lblErrMessage.Text & "&redirect=IN/Setup/IN_stockitem.aspx")
        End Try

        For intCnt = 0 To dsForDropDown.Tables(0).Rows.Count - 1
            dsForDropDown.Tables(0).Rows(intCnt).Item(0) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0))
            dsForDropDown.Tables(0).Rows(intCnt).Item(1) = Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(0)) & " ( " & _
                                                          Trim(dsForDropDown.Tables(0).Rows(intCnt).Item(1)) & " )"
            If Not ItemCode = "" Then
                If UCase(Trim(dsForDropDown.Tables(0).Rows(intCnt).Item("ItemCode"))) = UCase(Trim(objDataSet.Tables(0).Rows(0).Item("ItemCode"))) Then
                    SelectedIndex = intCnt + 1
                End If
            End If
        Next intCnt
        drinsert = dsForDropDown.Tables(0).NewRow()
        drinsert(0) = ""
        drinsert(1) = "Please Select a Code "
        dsForDropDown.Tables(0).Rows.InsertAt(drinsert, 0)

        lstItemCode.DataSource = dsForDropDown.Tables(0)
        lstItemCode.DataValueField = "ItemCode"
        lstItemCode.DataTextField = "Description"
        lstItemCode.DataBind()

        If SelectedIndex = -1 And Not ItemCode = "" Then

            lstItemCode.Items.Add(New ListItem(Trim(objDataSet.Tables(0).Rows(0).Item("ItemCode")), Trim(objDataSet.Tables(0).Rows(0).Item("ItemCode"))))
            SelectedIndex = lstItemCode.Items.Count - 1
            lstItemCode.SelectedIndex = SelectedIndex

        End If

        If Not dsForDropDown Is Nothing Then
            dsForDropDown = Nothing
        End If
    End Sub

    Sub LoadItemMaster(ByVal Sender As Object, ByVal E As EventArgs)
        ItemCode = lstItemCode.SelectedItem.Value
        If ItemCode <> "" Then
            DisplayData()
            DisableControl()
        Else
            CTItemCode.Text = ""
            Desc.Text = ""
            Status.Text = ""
            CreateDate.Text = ""
            UpdateDate.Text = ""
            UpdateBy.Text = ""
            InitialCost.Text = "0"
            HighestCost.Text = "0"
            LowestCost.Text = "0"
            AvrgCost.Text = "0"
            LatestCost.Text = "0"
            purchaseACNo.Text = ""
            IssueACNo.Text = ""
            LatestPrice.Text = "0"
            AvrgPrice.Text = "0"
            DiffAverageCost.Text = "0"
            FixedPrice.Text = "0"
            HandQty.Text = "0"
            HoldQty.Text = "0"
            OrderQty.Text = "0"
            ReorderLvl.Text = "0"
            ReorderQty.Text = "0"
            txtBin.Text = ""
            Remark.Text = ""

        End If
        DisplayDropDown()

    End Sub

    Sub UpdateData(ByVal strAction As String)
        Dim FuelTypeInd As String
        Dim UseSellPrice As String
        Dim blnDupKey As Boolean = False
        Dim strStatus As String
        Dim strParam As String
        Dim strCompany As String = Session("SS_COMPANY")
        Dim strLocation As String = Session("SS_LOCATION")
        Dim strUserId As String = Session("SS_USERID")

        If Fuel_Yes.Checked Then
            FuelTypeInd = objIN.EnumFuelItemIndicator.Fuel
        ElseIf Fuel_Lub.Checked Then
            FuelTypeInd = objIN.EnumFuelItemIndicator.Lubricant
        Else
            FuelTypeInd = objIN.EnumFuelItemIndicator.No
        End If

        If Fixed_Price.Checked Then
            UseSellPrice = objCT.EnumSellingPriceForItem.Fixed
        ElseIf Latest_Price.Checked Then
            UseSellPrice = objCT.EnumSellingPriceForItem.PercentageOfLatestCost
        ElseIf Avrg_Price.Checked Then
            UseSellPrice = objCT.EnumSellingPriceForItem.PercentageOfAverageCost
        End If

        Select Case strAction
            Case "Save"
                strStatus = IIf(Status.Text = objCT.mtdGetStockItemStatus(objCT.EnumStockItemStatus.Deleted), _
                                    objCT.EnumStockItemStatus.Deleted, _
                                    objCT.EnumStockItemStatus.Active)
            Case "Delete"
                strStatus = IIf(Status.Text = objCT.mtdGetStockItemStatus(objCT.EnumStockItemStatus.Active), _
                                    objCT.EnumStockItemStatus.Deleted, _
                                    objCT.EnumStockItemStatus.Active)

        End Select

        strParam = _
        Trim(CTItemCode.Text) & "|" & _
        Trim(Desc.Text) & "|" & _
        Trim(txtBin.Text) & "|" & _
        Trim(objIN.EnumInventoryItemType.CanteenItem) & "|" & _
        Trim(lstProdType.SelectedItem.Value) & "|" & _
        Trim(lstProdCat.SelectedItem.Value) & "|" & _
        Trim(FuelTypeInd) & "|" & _
        Trim(lstProdBrand.SelectedItem.Value) & "|" & _
        Trim(lstProdModel.SelectedItem.Value) & "|" & _
        Trim(lstProdMat.SelectedItem.Value) & "|" & _
        Trim(lstStockAnalysis.SelectedItem.Value) & "|" & _
        Trim(lstVehExpense.SelectedItem.Value) & "|" & _
        Trim(lstAccCode.SelectedItem.Value) & "|" & _
        Trim(InventoryUOM.SelectedItem.Value) & "|" & _
        Trim(PurchaseUOM.SelectedItem.Value) & "|0|" & _
        IIf(Trim(ReorderLvl.Text) = "", 0, Trim(ReorderLvl.Text)) & "|" & _
        IIf(Trim(OrderQty.Text) = "", 0, ObjGlobal.DBDecimalFormat(OrderQty.Text)) & "|" & _
        IIf(Trim(ReorderQty.Text) = "", 0, Trim(ReorderQty.Text)) & "|" & _
        Trim(purchaseACNo.Text) & "|" & _
        Trim(IssueACNo.Text) & "|" & _
        Trim(UseSellPrice) & "|" & _
        IIf(Trim(FixedPrice.Text) = "", 0, Trim(FixedPrice.Text)) & "|" & _
        Trim(LatestPrice.Text) & "|" & _
        Trim(AvrgPrice.Text) & "|" & _
        Trim(Remark.Text) & "|" & _
        Trim(strStatus) & "|"

        Try

            intErrNo = objIN.mtdUpdInvItemsDetails(strOppCd_ADD, _
                                                        strOppCd_UPD, _
                                                        strOppCd_ItemList_GET, _
                                                        strUserId, _
                                                        strLocation, _
                                                        strParam, _
                                                        blnDupKey, _
                                                        blnUpdate.Text)

        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=ADD_ITEMTODROPDOWN&errmesg=" & lblErrMessage.Text & "&redirect=CT/Setup/CT_CanteenItem.aspx")
        End Try

        If blnDupKey Then
            lblDupMsg.Visible = True
        Else
            Select Case blnUpdate.Text
                Case True
                    Response.Redirect("CT_CanteenItem.aspx")
                Case False
                    Response.Redirect("CT_CanteenItem_Detail.aspx")
            End Select
        End If
    End Sub

    Sub btnSave_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateData("Save")

    End Sub

    Sub btnDelete_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        UpdateData("Delete")
        Response.Redirect("CT_CanteenItem.aspx")
    End Sub

    Sub btnSynchronizeData_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Dim strOpCode_GetMaster As String = "IN_CLSSETUP_INVMASTER_DETAILS_GET"
        Dim strOpCode_UpdItem   As String = "IN_CLSSETUP_INVITEM_DETAILS_UPD"
        Dim strParam As String
        Dim strLocation As String = Session("SS_LOCATION")
        Dim strUserId As String = Session("SS_USERID")
        
        strParam = Trim(CTItemCode.Text) & "|" & _
                   Trim(objIN.EnumInventoryItemType.CanteenItem)
        Try
            intErrNo = objIN.mtdSynchronizeItemData(strOpCode_GetMaster, _
                                                    strOpCode_UpdItem, _
                                                    strParam, _
                                                    strUserId, _
                                                    strLocation)
        Catch Exp As System.Exception
            Response.Redirect("../../../include/mesg/ErrorMessage.aspx?errcode=UPDATE_CANTEENITEM&errmesg=" & lblErrMessage.Text & "&redirect=CT/Setup/CT_CanteenItem.aspx")
        End Try
        
        Call LoadItemMaster(Sender, E)
    End Sub

    Sub btnBack_Click(ByVal Sender As Object, ByVal E As ImageClickEventArgs)
        Response.Redirect("CT_CanteenItem.aspx")
    End Sub

End Class
