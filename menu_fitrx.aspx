<%@ Page Language="vb" src="../include/menu_fitrx.aspx.vb" Inherits="menu_fitrx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>GG-Menu</title>
    
    <link href="include/css/gopalms.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin: 0">
     
    <form id="form1" runat="server" >

<table cellpadding="0" cellspacing="0" style="width: 100%">
	<tr>
		<td class="cell-left" valign="top">
		<table cellpadding="0" cellspacing="0" style="width: 254px">
			<tr>
				<td valign="top">                              
                    <div class="panel">
                        <table id="tblGLHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>

				    <button class="accordion">General Ledger</button>					
					<div class="panel">
                        <table id="tblGL"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL01" runat="server" cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_Journal_list.aspx" target="middleFrame" text="Journal"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL02" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_PostJournal_list.aspx" target="middleFrame" text="Post Journal"></asp:hyperlink>
                                </div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL03" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_JournalAdj_list.aspx" target="middleFrame" text="Journal Adjustment"></asp:hyperlink></div></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="Hyperlink5" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_NotaDebet_List.aspx" target="middleFrame" text="Nota Debet"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL04" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_Budget_list.aspx" target="middleFrame" text="Budget"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL05" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_Budget_Item_list.aspx" target="middleFrame" text="Budget Item"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL06" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_BudgetProd_list.aspx" target="middleFrame" text="Budget Produksi"></asp:hyperlink>
                                </div></a></td>
							</tr>

                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL07" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_BudgetProd_Estate_list.aspx" target="middleFrame" text="Budget Produksi Estate"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL08" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_BudgetVeh_list.aspx" target="middleFrame" text="Budget Kendaraan"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink id="lnkGL09" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_BudgetPupuk.aspx" target="middleFrame" text="Budget Pupuk"></asp:hyperlink>
                                </div></a></td>
							</tr>
                            <tr height="20">
								<td><a href="#"><div class="childmenu">
								<asp:hyperlink id="lnkGL10" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_PDOList.aspx" target="middleFrame" text="PDO"></asp:hyperlink>
                                </div></a></td>
							</tr>
							
							<tr height="20">
								<td><a href="#"><div class="childmenu">
								<asp:hyperlink id="lnkGL11" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_PDDList.aspx" target="middleFrame" text="PDD"></asp:hyperlink>
                                </div></a></td>
							</tr>
						</table>
					</div>

                    <div class="panel">
                        <table id="tblSpc1"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>
                    <div class="panel">
                        <table id="tblAPHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>
     
        <button class="accordion">Account Payable</button>					
					<div class="panel">
                        <table id="tblAP"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkAP06" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_invrcvNotelist.aspx" target="middleFrame" text="Invoice Reception"></asp:hyperlink></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkAP01" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_InvRcvList.aspx" target="middleFrame" text="Credited Invoice"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkAP02" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_DNList.aspx" target="middleFrame" text="Supplier Debit Note"></asp:hyperlink></a></td>
							</tr>

                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkAP03" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_CNList.aspx"  target="middleFrame" text="Supplier Credit Note"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkAP04" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_CJList.aspx" target="middleFrame" text="Creditor Journal"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkAP05" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_invrcv_wm_list.aspx" target="middleFrame" text="Weighing Credit Invoice"></asp:hyperlink></a></td>
							</tr>
						</table>
					</div>

                     <div class="panel">
                        <table id="tblSpc2"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div> 
                    
                     <div class="panel">
                        <table id="tblBillHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div>              

     <button class="accordion">Account Receivable</button>					
					<div class="panel">
                        <table id="tblBill"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkBill01" NavigateUrl="/en/BI/Trx/BI_Trx_InvoiceList.aspx" runat="server" target="middleFrame" text="Invoice"></asp:hyperlink></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkBill02" runat="server" NavigateUrl="/en/BI/Trx/BI_Trx_DNList.aspx" target="middleFrame" text="Debit Note"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkBill03" runat="server" NavigateUrl="/en/BI/Trx/BI_Trx_CNList.aspx" target="middleFrame" text="Credit Note"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkBill04" runat="server" NavigateUrl="/en/BI/Trx/BI_trx_JournalList.aspx" target="middleFrame" text="Debtor Journal"></asp:hyperlink></a></td>
							</tr>
                            
						</table>
					</div>

                     <div class="panel">
                        <table id="tblSpc3"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div> 
                    <div class="panel">
                        <table id="tblCBHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div> 

    <button class="accordion">Cash Bank Management</button>					
					<div class="panel">
                        <table id="tblCB"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB01" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_PayList.aspx" target="middleFrame" text="Payment"></asp:hyperlink></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB02" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_ReceiptList.aspx" target="middleFrame" text="Receipt"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB03" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_CashBankList.aspx" target="middleFrame" text="Cash Bank"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB04" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_RekonsileList.aspx" target="middleFrame" text="Bank Rekonciliation"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB05" runat="server" NavigateUrl="/en/CB/Trx/CB_trx_SaldoBank_list.aspx" target="middleFrame" text="Bank Balance"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB06" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_WithdrawalList.aspx" target="middleFrame" text="Withdrawal"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB07" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_FundingList.aspx" target="middleFrame" text="Bank Loan"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB08" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_ReimbursementList.aspx" target="middleFrame" text="Reimbursement"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkCB09" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_StaffAdvanceList.aspx" target="middleFrame" text="Staff Advance"></asp:hyperlink></a></td>
							</tr>
						</table>
					</div>

                    <div class="panel">
                        <table id="tblSpc4"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div> 
                    <div class="panel">
                        <table id="tblFAHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div> 

  <button class="accordion">Fixed Asset</button>					
					<div class="panel">
                        <table id="tblFA"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkFA01" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetAddList.aspx" target="middleFrame" text="Asset Addition"></asp:hyperlink></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkFA02" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetDeprList.aspx" target="middleFrame" text="Allow Depreciation"></asp:hyperlink></a></td>
							</tr>
                            
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkFA03" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetDispList.aspx" target="middleFrame" text="Allow Disposal"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkFA04" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetWOList.aspx" target="middleFrame" text="Asset Write Off"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkFA05" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetTranList.aspx" target="middleFrame" text="Asset Transfer"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkFA06" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetRevList.aspx" target="middleFrame" text="Asset Revaluation"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkFA07" runat="server" NavigateUrl="/en/FA/Trx/FA_trx_LeaseFinancingList.aspx" target="middleFrame" text="Leasing & Financing"></asp:hyperlink></a></td>
							</tr>
						</table>
					</div>

                    <div class="panel">
                        <table id="tblSpc5"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div> 
                   <div class="panel">
                        <table id="tblTXHead"  cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
                        </table>
                    </div> 


<button class="accordion">Tax Management</button>					
					<div class="panel">
                        <table id="tblTX" cellSpacing="1" cellPadding="0" width="100%" border="0" runat="server">
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkTX01" runat="server" NavigateUrl="/en/TX/trx/TX_trx_TaxVerificationList.aspx" target="middleFrame" text="Tax Verification List"></asp:hyperlink></a></td>
							</tr>
							<tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkTX02" runat="server" NavigateUrl="/en/TX/trx/TX_trx_TaxVerifiedList.aspx" target="middleFrame" text="Tax Slip"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkTX03" runat="server" NavigateUrl="/en/TX/trx/TX_trx_PPNNoList.aspx" target="middleFrame" text="Assigining Tax Number"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkTX04" runat="server" NavigateUrl="/en/TX/trx/TX_trx_FPEntryList.aspx" target="middleFrame" text="Tax Invoice"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <asp:hyperlink class="lb-mt" id="lnkTX05" runat="server" NavigateUrl="/en/TX/trx/TX_trx_PPNMasukanList.aspx" target="middleFrame" text="Input VAT"></asp:hyperlink></a></td>
							</tr>
                            <tr>
								<td><a href="#"><div class="childmenu">
                                <<asp:hyperlink class="lb-mt" id="lnkTX06" runat="server" NavigateUrl="/en/TX/trx/TX_trx_PPNKeluaranList.aspx" target="middleFrame" text="Output VAT"></asp:hyperlink></a></td>
							</tr>
 
						</table>
					</div>

                <div style="position:absolute; top:0px; width:86%; left:125px; height:1500px" >          
                    <iframe id="Iframe1" name="middleFrame"  style="border-style: none; border-color: inherit; border-width: 0; width:100%; height:100%; background-color:white; margin-top:0px; margin-left: 80px;"
				        scrolling="auto" src="black.aspx"  ></iframe>            
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
			</tr>
		</table>

		</td>
	</tr>
</table>

           <%--<div id="Nav" style="position:absolute; width:20%; top:0px; left:0px; height:1000px">
            	
            <table>
			    <tr height="20">
			    <td width="20"></td>
			</tr>
			</table> 

			    <table id="tblGLHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblGL);">General Ledger</A></td>
							</tr>
						</table>
						<table id="tblGL" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL01" runat="server" cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_Journal_list.aspx" target="middleFrame" text="Journal"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL02" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_PostJournal_list.aspx" target="middleFrame" text="Post Journal"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL03" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_JournalAdj_list.aspx" target="middleFrame" text="Journal Adjustment"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="Hyperlink5" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_NotaDebet_List.aspx" target="middleFrame" text="Nota Debet"></asp:hyperlink></td>
							</tr>
            
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL04" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_Budget_list.aspx" target="middleFrame" text="Budget"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL05" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_Budget_Item_list.aspx" target="middleFrame" text="Budget Item"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL06" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_BudgetProd_list.aspx" target="middleFrame" text="Budget Produksi"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL07" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_BudgetProd_Estate_list.aspx" target="middleFrame" text="Budget Produksi Estate"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL08" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_BudgetVeh_list.aspx" target="middleFrame" text="Budget Kendaraan"></asp:hyperlink></td>
							</tr>

							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL09" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_BudgetPupuk.aspx" target="middleFrame" text="Budget Pupuk"></asp:hyperlink></td>
							</tr>
                            <tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL10" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_PDOList.aspx" target="middleFrame" text="PDO"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink id="lnkGL11" runat="server"  cssclass="lb-mt" NavigateUrl="/en/GL/Trx/GL_trx_PDDList.aspx" target="middleFrame" text="PDD"></asp:hyperlink></td>
							</tr>
							
							
						</table>

						<table id="tblSpc1" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>

			<table id="tblAPHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblAP);">Account Payable</A></td>
							</tr>
						</table>
						<table id="tblAP" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkAP06" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_invrcvNotelist.aspx" target="middleFrame" text="Invoice Reception"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkAP01" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_InvRcvList.aspx" target="middleFrame" text="Credited Invoice"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkAP02" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_DNList.aspx" target="middleFrame" text="Supplier Debit Note"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkAP03" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_CNList.aspx"  target="middleFrame" text="Supplier Credit Note"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkAP04" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_CJList.aspx" target="middleFrame" text="Creditor Journal"></asp:hyperlink></td>
							</tr>
                            <tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkAP05" runat="server" NavigateUrl="/en/AP/Trx/ap_trx_invrcv_wm_list.aspx" target="middleFrame" text="Weighing Credit Invoice"></asp:hyperlink></td>
							</tr>
						</table>

						<table id="tblSpc2" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>

                        <table id="tblBillHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblBill);">Account 
										Receivable</A></td>
							</tr>
						</table>
						<table id="tblBill" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkBill01" NavigateUrl="/en/BI/Trx/BI_Trx_InvoiceList.aspx" runat="server" target="middleFrame" text="Invoice"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkBill02" runat="server" NavigateUrl="/en/BI/Trx/BI_Trx_DNList.aspx" target="middleFrame" text="Debit Note"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkBill03" runat="server" NavigateUrl="/en/BI/Trx/BI_Trx_CNList.aspx" target="middleFrame" text="Credit Note"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkBill04" runat="server" NavigateUrl="/en/BI/Trx/BI_trx_JournalList.aspx" target="middleFrame" text="Debtor Journal"></asp:hyperlink></td>
							</tr>
						</table>

						<table id="tblSpc3" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>

			<table id="tblCBHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblCB);">Cash Bank Management</A></td>
							</tr>
						</table>
						<table id="tblCB" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB01" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_PayList.aspx" target="middleFrame" text="Payment"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB02" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_ReceiptList.aspx" target="middleFrame" text="Receipt"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB03" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_CashBankList.aspx" target="middleFrame" text="Cash Bank"></asp:hyperlink></td>
							</tr>
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB04" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_RekonsileList.aspx" target="middleFrame" text="Bank Rekonciliation"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB05" runat="server" NavigateUrl="/en/CB/Trx/CB_trx_SaldoBank_list.aspx" target="middleFrame" text="Bank Balance"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB06" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_WithdrawalList.aspx" target="middleFrame" text="Withdrawal"></asp:hyperlink></td>
							</tr>						
							
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB07" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_FundingList.aspx" target="middleFrame" text="Bank Loan"></asp:hyperlink></td>
							</tr>		
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB08" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_ReimbursementList.aspx" target="middleFrame" text="Reimbursement"></asp:hyperlink></td>
							</tr>	
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkCB09" runat="server" NavigateUrl="/en/CB/Trx/cb_trx_StaffAdvanceList.aspx" target="middleFrame" text="Staff Advance"></asp:hyperlink></td>
							</tr>	
							
						</table>

						<table id="tblSpc4" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>

			            <table id="tblFAHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblFA);">Fixed Asset</A></td>
							</tr>
						</table>
						<table id="tblFA" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkFA01" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetAddList.aspx" target="middleFrame" text="Asset Addition"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkFA02" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetDeprList.aspx" target="middleFrame" text="Allow Depreciation"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkFA03" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetDispList.aspx" target="middleFrame" text="Allow Disposal"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkFA04" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetWOList.aspx" target="middleFrame" text="Asset Write Off"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkFA05" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetTranList.aspx" target="middleFrame" text="Asset Transfer"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkFA06" runat="server" NavigateUrl="/en/FA/trx/FA_trx_AssetRevList.aspx" target="middleFrame" text="Asset Revaluation"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkFA07" runat="server" NavigateUrl="/en/FA/Trx/FA_trx_LeaseFinancingList.aspx" target="middleFrame" text="Leasing & Financing"></asp:hyperlink></td>
							</tr>	

						</table>
                        <table id="tblSpc5" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr>
								<td colSpan="2"><IMG height="0" src="images/spacer.gif" width="5" border="0"></td>
							</tr>
						</table>
						
						<table id="tblTXHead" cellSpacing="0" cellPadding="0" width="100%" runat="server">
							<tr height="20" >
								<td width="20"></td>
								<td width="14"><IMG src="images/arow.gif" border="0" align="left"></td>
								<td class="lb-hti"><A class="lb-tti" href="javascript:togglebox(tblTX);">Tax Management</A></td>
							</tr>
						</table>
						<table id="tblTX" style="VISIBILITY: hidden; POSITION: absolute" cellSpacing="0" cellPadding="0"
							width="100%" border="0" runat="server">
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkTX01" runat="server" NavigateUrl="/en/TX/trx/TX_trx_TaxVerificationList.aspx" target="middleFrame" text="Tax Verification List"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkTX02" runat="server" NavigateUrl="/en/TX/trx/TX_trx_TaxVerifiedList.aspx" target="middleFrame" text="Tax Slip"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkTX03" runat="server" NavigateUrl="/en/TX/trx/TX_trx_PPNNoList.aspx" target="middleFrame" text="Assigining Tax Number"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkTX04" runat="server" NavigateUrl="/en/TX/trx/TX_trx_FPEntryList.aspx" target="middleFrame" text="Tax Invoice"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkTX05" runat="server" NavigateUrl="/en/TX/trx/TX_trx_PPNMasukanList.aspx" target="middleFrame" text="Input VAT"></asp:hyperlink></td>
							</tr>
							<tr height="20">
								<td width="20"></td>
								<td width="14">&nbsp;<IMG src="images/leftdot.gif" border="0"></td>
								<td class="lb-mti"><asp:hyperlink class="lb-mt" id="lnkTX06" runat="server" NavigateUrl="/en/TX/trx/TX_trx_PPNKeluaranList.aspx" target="middleFrame" text="Output VAT"></asp:hyperlink></td>
							</tr>
							
						</table>


            </div>
         
             <div style="position:absolute; top:0px; width:85%; left:179px; height:1000px" >
          
              	<iframe id="Iframe1" name="middleFrame"  style="width:100%; height:100%; background-color:Black"
				 scrolling="auto" src="black.aspx"></iframe>
             
               </div>
            
           </td>
           <div class="BackgroundTopCorner"></div>--%>
          </form>
 
</body>
</html>

