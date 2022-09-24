<%@ Page Language="vb" src="../../../include/admin_accperiod.aspx.vb" Inherits="admin_accperiod" %>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<html>
	<head>
		<title>Accounting Period & Physical Period</title>
		<Preference:PrefHdl id=PrefHdl runat="server" />
		<script language="javascript">
			
			function fnBindAccMonth(ddlAccMonth, txtAccYear) {
				var doc = document.frmMain;
				var intAccYear = parseFloat(txtAccYear.value);
				var arrPeriod, arrData;
				var intMaxPeriod, intYear, intMonth;
                var intSelected, i;
                
                if (intAccYear != 'NaN') {
                    if (intAccYear >= 1901 && intAccYear <= 2029) {
                        intSelected = ddlAccMonth.selectedIndex;
				        arrPeriod = doc.hidPeriodData.value.split(";");
				        intMaxPeriod = 12;
				        
                        for (i = 0; i < arrPeriod.length; i++) {
                            arrData = arrPeriod[i].split("/");
                            intMonth = parseFloat(arrData[0]);
                            intYear = parseFloat(arrData[1]);
                            if (intMonth != 'NaN' && intYear != 'NaN') {
                                if (intAccYear == intYear) {
                                    intMaxPeriod = intMonth;
                                    break;
                                }
                            }
                        }
                        
                        if (ddlAccMonth.options.length < intMaxPeriod) {
                            for(i = ddlAccMonth.options.length + 1; i <= intMaxPeriod; i++) {
                                var objOption = document.createElement("OPTION");
                                objOption.text = i;
                                objOption.value = i;
                                ddlAccMonth.options.add(objOption);
                            }
                        }
                        else if(ddlAccMonth.options.length > intMaxPeriod) {
                            for(i = ddlAccMonth.options.length - 1; i >= intMaxPeriod; i--) {
                                ddlAccMonth.options.remove(i);
                            }
                            
                            if (ddlAccMonth.selectedIndex == -1) {
                                ddlAccMonth.selectedIndex = 0;
                            }
                        }
                    }
                }
			}
		</script>		
	</head>
	<body>
	    <form id=frmMain runat="server">
			<asp:Label id="SortExpression" Visible="False" Runat="server" />
			<table border="0" cellspacing="0" cellpadding="2" width="100%">
				<tr>
					<td colspan="3">
						<UserControl:MenuAdmin id=MenuAdmin runat="server" />
					</td>
				</tr>
				<tr>
					<td class="mt-h" colspan="3">ACCOUNTING PERIOD & PHYSICAL PERIOD - <asp:Label id=lblMyLocation runat=server /><asp:label id="lblTracker" runat="server"/>
				</tr>
				<tr>
					<td colspan=2><hr size="1" noshade></td>
				</tr>
				<tr>
					<td colspan=2 height=25>Please choose Accounting Period.</td>
				</tr>
				<tr>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Inventory / Canteen / Workshop Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlINAccMonth width=20% runat=server />
								<asp:TextBox id=txtINAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlINAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validateIN display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtINAccYear/>															
								<asp:RangeValidator id=rangeINAccYear display=dynamic runat=server
									ControlToValidate=txtINAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Nursery Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlNUAccMonth width=20% runat=server />
								<asp:TextBox id=txtNUAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlNUAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validateNUAccYear display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtNUAccYear/>															
								<asp:RangeValidator id=rangeNUAccYear display=dynamic runat=server
									ControlToValidate=txtNUAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Account Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan=2>&nbsp;</td>
				</tr>
				<tr>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Purchasing Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlPUAccMonth width=20% runat=server />
								<asp:TextBox id=txtPUAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlPUAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validatePU display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtPUAccYear/>															
								<asp:RangeValidator id=rangePUAccYear display=dynamic runat=server
									ControlToValidate=txtPUAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Account Payable Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlAPAccMonth width=20% runat=server />
								<asp:TextBox id=txtAPAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlAPAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validateAP display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtAPAccYear/>															
								<asp:RangeValidator id=rangeAPAccYear display=dynamic runat=server
									ControlToValidate=txtAPAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan=2>&nbsp;</td>
				</tr>
				<tr>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Payroll Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlPRAccMonth width=20% runat=server />
								<asp:TextBox id=txtPRAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlPRAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validatePR display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtPRAccYear/>															
								<asp:RangeValidator id=rangePRAccYear display=dynamic runat=server
									ControlToValidate=txtPRAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Production / Weighing Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlPDAccMonth width=20% runat=server />
								<asp:TextBox id=txtPDAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlPDAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validatePD display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtPDAccYear/>															
								<asp:RangeValidator id=rangePDAccYear display=dynamic runat=server
									ControlToValidate=txtPDAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan=2>&nbsp;</td>
				</tr>
				<tr>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Account Receivable / Contract Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlARAccMonth width=20% runat=server />
								<asp:TextBox id=txtARAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlARAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validateAR display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtARAccYear/>															
								<asp:RangeValidator id=rangeARAccYear display=dynamic runat=server
									ControlToValidate=txtARAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Fixed Asset Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlFAAccMonth width=20% runat=server />
								<asp:TextBox id=txtFAAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlFAAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validateFA display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtFAAccYear/>															
								<asp:RangeValidator id=rangeFAAccYear display=dynamic runat=server
									ControlToValidate=txtFAAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan=2>&nbsp;</td>
				</tr>
				<tr>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>General Ledger Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlGLAccMonth width=20% runat=server />
								<asp:TextBox id=txtGLAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlGLAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validateGL display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtGLAccYear/>															
								<asp:RangeValidator id=rangeGLAccYear display=dynamic runat=server
									ControlToValidate=txtGLAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Cash And Bank Accounting Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlCBAccMonth width=20% runat=server />
								<asp:TextBox id=txtCBAccYear width=20% onblur="fnBindAccMonth(document.frmMain.ddlCBAccMonth, this);" maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validateCB display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Account Year." 
									ControlToValidate=txtCBAccYear/>															
								<asp:RangeValidator id=rangeCBAccYear display=dynamic runat=server
									ControlToValidate=txtCBAccYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Accounting Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
					
					
					<td height=25 width=50% valign=top>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td colspan=2>&nbsp;</td>
				</tr>
				<tr>
					<td colspan=2 height=25>Please choose Physical Period.</td>
				</tr>
				<tr>
					<td height=25 width=50% valign=top>
						<table border="0" cellspacing="0" cellpadding="2" width="100%">
							<tr>
								<td colspan=2><u><b>Physical Period</b></u></td>
							</tr>
							<tr class="mr-r">
								<td width=20%>Period :*</td>
								<td width=80%>
								<asp:DropDownList id=ddlPhyMonth width=20% runat=server />
								<asp:TextBox id=txtPhyYear width=20% maxlength=4 runat=server /> 
								<asp:RequiredFieldValidator id=validatePhy display=dynamic runat=server 
									ErrorMessage="<br>Please enter the Physical Year." 
									ControlToValidate=txtPhyYear/>															
								<asp:RangeValidator id=rangePhyYear display=dynamic runat=server
									ControlToValidate=txtPhyYear
									MinimumValue="1901"
									MaximumValue="2029"
									Type="Integer"
									EnableClientScript="True"
									Text="<br>The Physical Year must be between 1901 to 2029." />
								</td>
							</tr>
						</table>
					</td>
					<td height=25 width=50% valign=top>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td colspan=2>&nbsp;</td>
				</tr>
				<tr>
					<td align="left" colspan=2>
						<asp:ImageButton id=SaveBtn imageurl="../../images/butt_save.gif" onClick=SaveBtn_Click AlternateText=" Save " runat="server"/>
						<asp:Label id=lblErrAccPeriod visible=false forecolor=red text="Some accounting periods are not configured. You can only have active accounting periods which have been configured." runat=server />
					</td>
				</tr>
			</table>
			<Input type=hidden id=hidPeriodData value="" runat=server/>
		</FORM>
	</body>
</html>
