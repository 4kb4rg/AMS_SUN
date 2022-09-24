<%@ Page Language="vb" src="../include/master.aspx.vb" Inherits="master" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GG-Menu</title>
     <link href="include/css/gopalms.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style4
        {
            width: 4px;
        }
        .style5
        {
            width: 170px;
            height: 57px;
        }
        .style6
        {
            width: 335px;
        }
        .style7
        {
            text-align: left;
            height: 197px;
            width: 4px;
        }
        .style8
        {
            width: 95px;
        }
    </style>
</head>
<body style="margin: 0; background-color: #d0d0d0;">
    <form id="form1"  class="main-modul-bg-app-list-pu" runat="server">
      <table cellpadding="0" cellspacing="0" style="width: 100%; height: 98%;" align="center">
	<tr>
		<td class="cell-left" valign="top">
		
		<table cellpadding="0" cellspacing="0" style="width: 100%; height: 50px">
			<tr>
				<td class="cell-right" valign="bottom">
				<table cellpadding="0" cellspacing="0" style="width: 100%">
					<tr>
						<td style="width: 100%" class="cell-right"><table cellpadding="0" cellspacing="0" class="style1">
                                <tr>
                                    <td style="text-align:left" class="style8">
										 	
											&nbsp;</td>
                                    <td style="text-align:left" class="style6">
										 	
											<img class="style5" src="images/lgsubmenu.png" /></td>
                                    <td style="text-align:right">
										 	
											&nbsp;<a href="../login.aspx"> 
											</a>
										    <a href="../default.aspx">
											&nbsp;</a><span class="font9"><strong>Welcome, 
                 <asp:Label id="lblUser" runat="server" cssclass="login"  />
               &nbsp;</strong></span></td>
                                </tr>
                            </table>
                        </td>
						<td valign="bottom">
							<table align="right" cellpadding="0" cellspacing="0" style="width: 120px">
								<tr>
									<td style="height: 60px">
										<div class="dropdown">	
										</div>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
																			 	
											<a href="system/user/setlocation.aspx"><img src="images/btlogin.png" 
                                            class="button" />
                                            </a><a href="../login.aspx">
                                            &nbsp;<img src="../images/btlogout.png" class="button" /></a></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td style="width: 100%; text-align:center " class="white-on-black50"><span class="font9">A D M I N&nbsp;&nbsp; A C C E S S</span></td>
						<td valign="bottom">
							&nbsp;</td>
					</tr>
					<tr>
						<td style="width: 100%" class="cell-right">&nbsp;</td>
						<td valign="bottom">
							&nbsp;</td>
					</tr>
				</table>
				</td>
			</tr>
		</table>
		<table cellpadding="0" cellspacing="0" style="width: 100%">
			<tr>
				<td style="width: 90px" valign="top">
				</td>
				<td style="width: 534px" valign="top">
				<table cellpadding="0" cellspacing="0" style="width: 100%">
					<tr>
						<td style="width: 178px; " valign="top" class="cell-left" rowspan="2">
				            <a href="menu_fi.aspx"></a></td>
						<td style="width: 178px; height: 178px" valign="top" class="cell-center">
                        &nbsp;&nbsp;</td>
				    
						<td style="width: 178px; height: 178px" valign="top" class="cell-center">
                        
           <table cellpadding="0" cellspacing="0" style="width: 175px">
 
			<tr>
				<td valign="top">
 
  			    <button class="accordion">General Ledger</button>					
					<div class="panel">
                        <table id="tlbDT"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
											<tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkStpFI01" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_AccCls.aspx" text="Kumpulan Kelas Akaun" cssclass="lb-mt"></asp:hyperlink></div>
                                </a></td>
							</tr>
							<tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkStpFI02" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_AccClsGrp.aspx" text="Kelas Akaun" cssclass="lb-mt"></asp:hyperlink></div>
                                </a></td>
							</tr>
							<tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI03" runat="server" NavigateUrl="/en/GL/Setup/GL_Setup_ActivityGrp.aspx" target="middleFrame" text="Kumpulan Aktiviti"></asp:hyperlink></div>
                                </a></td>
							</tr>
							<tr visible="true" >
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI04" runat="server" NavigateUrl="/en/GL/Setup/GL_Setup_Activity.aspx" target="middleFrame" text="Aktiviti"></asp:hyperlink></div>
                                </a></td>
							</tr>
							<tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI05" runat="server" NavigateUrl="/en/GL/Setup/GL_Setup_SubActivity.aspx" target="middleFrame" text="Sub Activiti"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI06" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_ExpenseCode.aspx" text="Expense Code"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI07" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_VehicleSubGrpCode.aspx" text="Grup Biaya Kendaraan Code"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI08" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_VehicleSubCode.aspx" text="Biaya Kenderaan Code"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true" > 
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI09" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_VehicleType.aspx" text="Jenis Kenderaan"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI10" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_Vehicle.aspx" text="Kenderaan"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI11" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_BlockGrp.aspx" text="Divisi"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI12" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_Block.aspx" text="Tahun Tanam"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI13" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_SubBlock.aspx" text="Blok"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI13b" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_TBM_TM.aspx" text="Mutasi Blok TBM - TM"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true" >
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI14" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_ChartOfAccGrp.aspx" text="Group COA"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI15" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_ChartOfAcc.aspx" text="COA"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI16" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_DoubleEntry.aspx" text="Double Entry Setup"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI28" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_PDOType.aspx" text="PDO Group"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI21" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_ALKOHList.aspx" text="Alokasi Overhead"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI22" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_FSList.aspx" text="Financial Statement Report"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI23" runat="server" target="middleFrame" NavigateUrl="/en/menu/menu_admin_page.aspx" text="Admin"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI25" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_Budget.aspx" text="Budget"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI17" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_BalanceSheet.aspx" text="Balance Sheet"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr  visible="true">
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpFI18" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_ProfitLoss.aspx" text="Profit and Loss"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr  visible="true">
								<td><a href="#"><div class="childmenu">
                               <asp:hyperlink class="lb-mt" id="lnkStpFI19" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_COGS.aspx" text="COGS Account Setup Page 1"></asp:hyperlink></div>
                                </a></td>
							</tr>
                            <tr  visible="true">
								<td><a href="#"><div class="childmenu">
                               <asp:hyperlink class="lb-mt" id="lnkStpFI20" runat="server" target="middleFrame" NavigateUrl="/en/GL/Setup/GL_Setup_COGS2.aspx" text="COGS Account Setup Page 2"></asp:hyperlink></div>
                                </a></td>
							</tr>
						</table>
					</div>
 
 
<%--
                    <button class="accordion">Testing</button>
					<div class="panel">
						<table cellpadding="0" cellspacing="1" style="width: 254px">
							<tr>
								<td><a href="#"><div class="fathermenu">Data Collection</div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="fathermenu">Processing</div></a></td>
							</tr>
							 
						</table>
					</div>--%>
					
					<script>
					    var acc = document.getElementsByClassName("accordion");
					    var i;


					    for (i = 0; i < acc.length; i++) {
					        acc[i].onclick = function () {
					            this.classList.toggle("active");
					            this.nextElementSibling.classList.toggle("hide");
					        }
					    }
					</script>			
 
				</td>
				<td valign="top">
 
  			        &nbsp;</td>
			</tr>
		</table>
                        </td>
				    
                    <td  valign="top" class="cell-right">                     
                            <button class="accordion">Inventory</button>					
					<div class="panel">
                        <table id="tlbStpIN" cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                               <asp:hyperlink id="lnkStpIN01" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_ProdType.aspx" text="Product Type" ></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkStpIN02" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_ProdBrand.aspx" text="Product Brand" ></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN03" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_ProdModel.aspx" text="Product Model"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN04" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_ProdCategory.aspx" text="Product Category" ></asp:hyperlink>
                                    </div></a></td>
							</tr>

                            <tr>
								<td ><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN05" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_ProdMaterial.aspx" text="Product Material" cssclass="lb-mt"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td ><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN06" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_StockAnalysis.aspx" text="Analisis Stock" cssclass="lb-mt"></asp:hyperlink></div></a></td>
							</tr>
							<tr >
								<td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN07" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_StockMaster.aspx" text="Stock Master" cssclass="lb-mt"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN08" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_StockItem.aspx" text="Stock Item" cssclass="lb-mt"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN09" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_DirectMaster.aspx" text="Direct Charge Master" cssclass="lb-mt"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN10" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_DirectCharge.aspx" text="Direct Charge Item" cssclass="lb-mt"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpIN11" runat="server" target="middleFrame" NavigateUrl="/en/IN/setup/IN_Approval_Level.aspx" text="User Level Approval" cssclass="lb-mt"></asp:hyperlink></div></a></td>
							</tr>

						</table>
					</div>
                        <button class="accordion">Purchasing</button>					
					<div class="panel">
                        <table id="tlbStpPU" cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								    <td><a href="#"><div class="childmenu"><asp:hyperlink class="lb-mt" id="lnkStpPU01" runat="server" target="middleFrame" NavigateUrl="/en/PU/setup/PU_setup_SuppList.aspx" text="Supplier"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								    <td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpPU02" runat="server" NavigateUrl="/en/CM/setup/CM_Setup_CurrencyList.aspx" target="middleFrame" text="Currency"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								    <td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpPU03" runat="server" NavigateUrl="/en/CM/setup/CM_setup_ExchangeRate.aspx" target="middleFrame" text="Exchange Rate"></asp:hyperlink></div></a></td>
							</tr>
			  			    <tr>
								    <td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpPU08" runat="server" target="middleFrame" NavigateUrl="/en/PU/setup/PU_setup_UserGroupList.aspx" text="User Group Item"></asp:hyperlink></div></a></td>
							    </tr>
				            <tr>
								    <td><a href="#"><div class="childmenu"><asp:hyperlink id="lnkStpPU09" runat="server" NavigateUrl="/en/PU/setup/PU_Setup_LocMGr.aspx" target="middleFrame" text="Purchasing Manager"></asp:hyperlink></div></a></td>
					        </tr>
                        </table>
                    </div>
                        
                        </td>
					</tr>
					<tr>
						<td style="width: 178px; height: 178px" valign="bottom" class="cell-center">&nbsp;</td>
						<td style="width: 178px; height: 178px" valign="bottom" class="cell-center">&nbsp;</td>
						<td style="width: 178px; height: 178px" valign="bottom" class="cell-right">
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
					</tr>
					<tr>
						<td class="cell-center" colspan="4" style="height: 43px" valign="bottom">
						<table cellpadding="0" cellspacing="0" style="width: 100%; height: 40px">
							<tr>
								
							</tr>
						</table>
						</td>
					</tr>
				</table>
				</td>
				<td valign="top" class="style4">
                    &nbsp;</td>
				<td style="width: 204px" valign="top">
                    <div class="panel">
                        <table id="tlbStpHRHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>
                    <div class="panel">
                        <table id="tlbSpc1"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>
                    <div class="panel">
                        <table id="tlbStpPRHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>
 

                    <div class="panel">
                        <table id="tlbStpHRHead_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>

                    <button class="accordion">Human Resources-Estate</button> 
                    <div class="panel">
                        <table id="tlbStpHR_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink13" runat="server" NavigateUrl="/en/HR/setup/HR_Setup_Religion.aspx" target="middleFrame" text="Agama" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink9" runat="server" NavigateUrl="/en/HR/setup/HR_setup_ICType.aspx" target="middleFrame" text="Tipe ID-Card" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink5" runat="server" NavigateUrl="/en/HR/setup/HR_setup_EmpType_Estate.aspx" target="middleFrame" text="Tipe Karyawan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink6" runat="server" NavigateUrl="/en/HR/setup/HR_setup_JobGroup_Estate.aspx" target="middleFrame" text="Group Jabatan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>			
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink19" runat="server" NavigateUrl="/en/HR/setup/HR_Setup_Jabatan_Estate.aspx" target="middleFrame" text="Jabatan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>	
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink34" runat="server" NavigateUrl="/en/GL/setup/TX_Setup_Kpp.aspx" target="middleFrame" text="Kantor Pelayanan Pajak" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>						 
						</table>
					</div>

                    <div class="panel">
                        <table id="tlbSpc3"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  
                    <div class="panel">
                        <table id="tlbStpPRHead_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>   
                    
                    <button class="accordion">Payroll-Estate</button> 
                    <div class="panel">
                        <table id="tlbStpPR_Estate"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink1" runat="server" NavigateUrl="/en/PR/setup/PR_setup_DivCode_Estate.aspx" target="middleFrame" text="Divisi" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
 					        <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink40" runat="server" NavigateUrl="/en/PR/setup/PR_setup_DivAsisten_Estate.aspx" target="middleFrame" text="Divisi Asisten-Mandor 1" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink2" runat="server" NavigateUrl="/en/PR/setup/PR_setup_YearPlan_Estate.aspx" target="middleFrame" text="Tahun Tanam" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink12" runat="server" NavigateUrl="/en/PR/setup/PR_setup_BlokList_Estate.aspx" target="middleFrame" text="Blok" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink3" runat="server" NavigateUrl="/en/PR/setup/PR_setup_BlokBJRList_Estate.aspx" target="middleFrame" text="Bjr" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink8" runat="server" NavigateUrl="/en/PR/setup/PR_setup_GolList_Estate.aspx" target="middleFrame" text="Gol Bulanan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink10" runat="server" NavigateUrl="/en/PR/setup/PR_setup_SalList_Estate.aspx" target="middleFrame" text="Upah Karyawan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink20" runat="server" NavigateUrl="/en/PR/setup/PR_setup_SalaryList.aspx" target="middleFrame" text="Alokasi Pekerjaan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink11" runat="server" NavigateUrl="/en/PR/setup/PR_Setup_TgCode_Estate.aspx" target="middleFrame" text="Kode Tanggungan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink14" runat="server" NavigateUrl="/en/PR/setup/PR_setup_BerasRt_Estate.aspx" target="middleFrame" text="Harga Beras" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink32" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmBrsList_Estate.aspx" target="middleFrame" text="Premi Beras" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink28" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_AstekList_Estate.aspx" target="middleFrame" text="Astek" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink29" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_CutOff_Estate.aspx" target="middleFrame" text="Cut-Off Gaji" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink30" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_AttCode_Estate.aspx" target="middleFrame" text="Kode Absensi" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink31" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_AttdList_Estate.aspx" target="middleFrame" text="Setting Kode Absensi" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink7a" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_JamKerja_Estate.aspx" target="middleFrame" text="Jam Kerja" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink7" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_Holiday_Estate.aspx" target="middleFrame" text="Hari Libur" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink7_Chg" runat="server" NavigateUrl="/en/PR/Setup/PR_Setup_Hari_Pengganti_Estate.aspx" target="middleFrame" text="Setting Hari Pengganti" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink33" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_OTList_Estate.aspx" target="middleFrame" text="Tarif Lembur" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink4" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiPanenList_Estate.aspx" target="middleFrame" text="Tabel Basis & Premi Panen" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink15" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_DendaList_Estate.aspx" target="middleFrame" text="Denda Panen" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink16" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmBrondolList_Estate.aspx" target="middleFrame" text="Premi Brondolan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink17" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiKrjnnList_Estate.aspx" target="middleFrame" text="Premi Kerajinan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink18" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiDriver_Estate.aspx" target="middleFrame" text="Premi Supir" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink21" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiMndorList_Estate.aspx" target="middleFrame" text="Premi Mandor" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink23" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmKaretList_Estate.aspx" target="middleFrame" text="Premi Deres" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink202" runat="server" NavigateUrl="/en/PR/Setup/PR_setup_PrmiLain_Estate.aspx" target="middleFrame" text="Premi Lain" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="Hyperlink1x" runat="server" target="middleFrame" NavigateUrl="/en/PR/Setup/PR_Setup_Vehicle_Estate.aspx" text="Jenis Kenderaan"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink26" runat="server" NavigateUrl="/en/PR/setup/PR_Setup_Aktivitikategori_Estate.aspx" target="middleFrame" text="Kategori Aktiviti" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink27" runat="server" NavigateUrl="/en/PR/setup/PR_Setup_AktivitiSubkategori_Estate.aspx" target="middleFrame" text="Sub Kategori Aktiviti" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink22" runat="server" NavigateUrl="/en/PR/setup/PR_setup_AktivitiList_Estate.aspx" target="middleFrame" text="Aktiviti" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink24" runat="server" NavigateUrl="/en/PR/setup/PR_setup_Borongan_Estate.aspx" target="middleFrame" text="Borongan" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink25" runat="server" NavigateUrl="/en/PR/setup/PR_setup_ComponentGajiList_Estate.aspx" target="middleFrame" text="Komponen Gaji" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink25a" runat="server" NavigateUrl="/en/PR/setup/PR_setup_Tunjangan_Estate.aspx" target="middleFrame" text="Tunjangan Gaji" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink35" runat="server" NavigateUrl="/en/PR/setup/PR_setup_PPH21List_Estate.aspx" target="middleFrame" text="PPH21" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink36" runat="server" NavigateUrl="/en/PR/setup/PR_setup_RapelList_Estate.aspx" target="middleFrame" text="Rapel" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>
                    
                    <div class="panel">
                        <table id="Table2"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  
                    <div class="panel">
                        <table id="tlbStpHRHead_MILL"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  
        

                    <div class="panel">
                        <table id="tlbSpc4"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>  
                    <div class="panel">
                        <table id="tlbStpPRHead_MILL"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							 
						</table>
					</div>            
                    &nbsp;</td>
				<td style="width: 426px" valign="top">
				<table cellpadding="0" cellspacing="0" style="width: 100%">
					 <tr>
						<td valign="bottom" class="style7">&nbsp;</td>
						<td style="width: 190px; height: 1px" valign="top" >
           
				    <button class="accordion"> Sales &amp; Distribution </button>					
					<div class="panel">
                        <table id="tlbStpSD"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkStpSD1" runat="server" NavigateUrl="/en/CM/setup/CM_Setup_CurrencyList.aspx" target="middleFrame" text="Currency" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
	                        <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkStpSD2" runat="server" NavigateUrl="/en/CM/setup/CM_setup_ExchangeRate.aspx" target="middleFrame" text="Exchange Rate" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkStpSD3" runat="server" NavigateUrl="/en/CM/setup/CM_setup_ContractQuality.aspx" target="middleFrame" text="Contract Quality" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkStpSD4" runat="server" NavigateUrl="/en/CM/setup/CM_setup_ClaimQuality.aspx" target="middleFrame" text="Claim Quality" cssclass="lb-mt"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpSD5" runat="server" target="middleFrame" NavigateUrl="/en/BI/setup/BI_setup_BillPartyList.aspx" text="Customer"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkStpSD6" runat="server" target="middleFrame" NavigateUrl="/en/WM/setup/WM_setup_TransporterList.aspx" text="Transporter"></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>
                        </td>
						<td style="width: 142px; height: 197px" valign="bottom" class="cell-center">&nbsp;</td>
						<td style="width: 142px; height: 197px" valign="bottom" class="cell-right">&nbsp;</td>
					</tr>
					</table>
				</td>
				<td style="width: 96px" valign="top">
				    &nbsp;</td>
			</tr>
		</table>
		</td>
	</tr>
</table>
      
         
        <div class="view" id="view">
        
       

        </div>
    </form>
</body>
</html>

