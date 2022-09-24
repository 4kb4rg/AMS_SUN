<%@ Page Language="vb" src="../../../include/admin_comp_det.aspx.vb" Inherits="admin_comp_det" %>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<html>

<head>
	<title>Company Details</title>
	<Preference:PrefHdl id=PrefHdl runat="server" />
</head>
<body>
	<form id=frmCoDet runat=server>
		<table border=0 cellspacing=0 cellpading=2 width="100%">
		<tr>
			<td colspan=5>
				<UserControl:MenuAdmin id=MenuAdmin runat="server" />
			</td>
		</tr>
		<tr>
			<td class="mt-h" colspan="5"><asp:label id="lblTitle" runat="server" /> DETAILS</td>
		</tr>
		<tr>
			<td colspan=5><hr size="1" noshade></td>
		</tr>
		<tr>
			<td width=20%><asp:label id="lblCompany" runat="server" /> Code :*</td>
			<td width=30%>
				<asp:TextBox id=txtCompCode width=50% maxlength=8 runat=server />
				<asp:Label id=lblErrCompCode visible=false forecolor=red text="<br>The code has been used, please try another another code." runat=server />
				<asp:RequiredFieldValidator id=validateCompCode display=dynamic runat=server 
					ErrorMessage="<br>Please enter company code. " 
					ControlToValidate=txtCompCode />
				<asp:RegularExpressionValidator id=revCompCode 
					ControlToValidate="txtCompCode"
					ValidationExpression="[a-zA-Z0-9\-]{1,8}"
					Display="Dynamic"
					text="<br>Alphanumeric without any space in between only."
					runat="server"/>
			</td>
			<td width=5%>&nbsp;</td>
			<td width=15%>Status :</td>
			<td width=30%><asp:Label id=lblStatus runat=server /></td>
		</tr>
		<tr>
			<td height=25><asp:label id="lblCompName" runat="server" /> :*</td>
			<td><asp:TextBox id=txtCompName width=100% maxlength=64 runat=server />
				<asp:RequiredFieldValidator id=validateCompName display=dynamic runat=server 
					ErrorMessage="<br>Please enter company name. " 
					ControlToValidate=txtCompName />			
			</td>
			<td>&nbsp;</td>
			<td>Date Created :</td>
			<td><asp:Label id=lblDateCreated runat=server /></td>
		</tr>
		<tr>
			<td>Address :*</td>
			<td rowspan="5" valign=top>
			<textarea rows="7" id=txtAddress cols="29" runat=server></textarea>
			<asp:Label id=lblErrAddress visible=false forecolor=red text="<br>Maximum length for address is up to 512 characters only." runat=server />
			<asp:RequiredFieldValidator id=validateAddress display=dynamic runat=server 
				ErrorMessage="<br>Please enter address. " 
				ControlToValidate=txtAddress />
			</td>
			<td>&nbsp;</td>
			<td height=25>Last Update :</td>
			<td><asp:Label id=lblLastUpdate runat=server /></td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td>&nbsp;</td>
			<td height=25>Updated By :</td>
			<td><asp:Label id=lblUpdateBy runat=server /></td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td>&nbsp;</td>
			<td height=25>&nbsp;</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td>&nbsp;</td>
			<td height=25>&nbsp;</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td>&nbsp;</td>
			<td height=25>&nbsp;</td>
			<td>&nbsp;</td>
		</tr>
		<tr>
			<td>Post Code :*</td>
			<td><asp:TextBox id="txtPostCode" width="50%" maxlength="16" runat="server" BackColor="Lavender"/>
				
			</td>
			<td>&nbsp;</td>
			<td>Tel. No. :*</td>
			<td><asp:TextBox id=txtTelNo width=100% maxlength=16 runat=server />
				<asp:RequiredFieldValidator id=validateTelNo display=dynamic runat=server 
					ErrorMessage="<br>Please enter telephone number. " 
					ControlToValidate=txtTelNo />
				<asp:RegularExpressionValidator id="validateNumTelNo" 
                     ControlToValidate="txtTelNo"
                     ValidationExpression="[\d\-\(\)]{1,16}"
                     Display="dynamic"
                     ErrorMessage="Phone number must be in numeric digits"
                     EnableClientScript="True" 
                     runat="server"/>
			</td>
		</tr>
		<tr>
			<td>City :*</td>
			<td><asp:TextBox id=txtCity width=100% maxlength=32 runat=server />
				<asp:RequiredFieldValidator id=validateCity display=dynamic runat=server 
					ErrorMessage="<br>Please enter city. " 
					ControlToValidate=txtCity />
				<asp:CompareValidator id="validateCityState" display=dynamic runat="server" 
					ControlToValidate="txtCity" Text="<br>The value must whole characters. " 
					Type="string" Operator="DataTypeCheck"/>
			</td>
			<td>&nbsp;</td>
			<td>Fax. No. :*</td>
			<td><asp:TextBox id=txtFaxNo width=100% maxlength=16 runat=server />
				<asp:RequiredFieldValidator id=validateFaxNo display=dynamic runat=server 
					ErrorMessage="<br>Please enter fax number. " 
					ControlToValidate=txtFaxNo />
				<asp:RegularExpressionValidator id="validateNumFaxNo" 
                     ControlToValidate="txtFaxNo"
                     ValidationExpression="[\d\-\(\)]{1,16}"
                     Display="dynamic"
                     ErrorMessage="Fax number must be in numeric digits"
                     EnableClientScript="True" 
                     runat="server"/>
			</td>
		</tr>
		<tr>
			<td>State :*</td>
			<td><asp:TextBox id=txtState width=100% maxlength=32 runat=server />
				<asp:RequiredFieldValidator id=validateState display=dynamic runat=server 
					ErrorMessage="<br>Please enter state. " 
					ControlToValidate=txtState />
				<asp:CompareValidator id="validateStrState" display=dynamic runat="server" 
					ControlToValidate="txtState" Text="<br>The value must be whole characters. " 
					Type="string" Operator="DataTypeCheck"/>
			</td>
			<td>&nbsp;</td>
			<td>NPWP Pusat Reference No. :*</td>
			<td><asp:Textbox id=txtNPWP width=100% maxlength=20 runat=server/>
			<asp:RequiredFieldValidator id=validateNPWP display=dynamic runat=server 
					ErrorMessage="<br>Please enter NPWP Pusat Reference No. " 
					ControlToValidate=txtNPWP />
			</td>
		</tr>		
		<tr>
			<td>Country :*</td>
			<td>
				<asp:DropDownList id=ddlCountry width=100% runat=server />
				<asp:Label id=lblErrCountry visible=false forecolor=red text="<br>Please select one country code." runat=server/>
    		</td>
    		<comment>
			<td>&nbsp;</td>
			<td>Socso Reference No :</td>
			<td><asp:Textbox id=txtSocsoRef width=100% maxlength=32 runat=server/></td>
			</comment>
		</tr>
		<tr>
			<td></td>
			<td>
				<asp:DropDownList id=ddlRDP width=100% runat=server />
				<asp:Label id=lblErrRDP visible=false forecolor=red text="<br>Please select one Running Digit Prefix." runat=server/>
    		</td>
		</tr>
		<tr>
			<td colspan=5>&nbsp;</td>
		</tr>
		<tr>
			<td colspan="5">
				<table id=tblSelection width="100%" class="mb-c" cellspacing="0" cellpadding="6" border="0" align="center" runat=server>
					<tr>						
						<td>
							<table border="0" cellspacing="0" width="100%" cellpadding="2">
								<tr>
									<td colspan=2>
										Select <asp:label id="lblLoc1" runat="server" /> and click "Add <asp:label id="lblLoc2" runat="server" />" button. 
										
	    							</td>
								</tr>
								<tr>
									<td colspan=2>
										<asp:DropDownList id=ddlLocation width=100% runat=server /> 
										<asp:Label id=lblErrLoc visible=false forecolor=red runat=server/>
	    							</td>
								</tr>
								<tr>
									<td colspan=2>
										<asp:ImageButton id=btnAddLoc imageurl="../../images/butt_add.gif" alternatetext="Add Location" onclick=btnAddLoc_Click runat=server />
	    							</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr><td colspan=5><asp:Label id=lblErrDelete forecolor=red visible=false text="Unable to delete location because it has been associated in system configuration." runat=server /></td></tr>
		<tr>
			<td colspan=5>
				<asp:DataGrid id=dgLocation
							BorderColor=black
							BorderWidth=0
							GridLines=both
							CellPadding=1
							CellSpacing=1
							width=100% 
							AutoGenerateColumns=false 
							OnDeleteCommand=DEDR_DeleteLoc 
							runat=server>
					<HeaderStyle CssClass="mr-h" />
					<ItemStyle CssClass="mr-l" />
					<AlternatingItemStyle CssClass="mr-r" />
						<Columns>
							<asp:BoundColumn DataField="LocCode" />
							<asp:BoundColumn DataField="Description" />
							<asp:TemplateColumn ItemStyle-HorizontalAlign=Right HeaderStyle-VerticalAlign=Bottom ItemStyle-VerticalAlign=Bottom>
								<ItemTemplate>
									<asp:LinkButton id=lbDelete CommandName=Delete Text=Delete runat=server />
								</ItemTemplate>
							</asp:TemplateColumn>	
						</Columns>
				</asp:DataGrid>
			</td>
		</tr>		
		<tr>
			<td colspan=5>&nbsp;</td>
		</tr>
		<tr>
			<td colspan="5">
				<asp:ImageButton id=btnSave imageurl="../../images/butt_save.gif" alternatetext=Save onclick=btnSave_Click runat=server/>
				<asp:ImageButton id=btnDelete CausesValidation=False imageurl="../../images/butt_delete.gif" visible=true alternatetext=Delete onclick=btnDelete_Click runat=server />
				<asp:ImageButton id=btnUnDelete CausesValidation=False imageurl="../../images/butt_undelete.gif" visible=true alternatetext=Undelete onclick=btnUnDelete_Click runat=server />
				<asp:ImageButton id=btnBack CausesValidation=False imageurl="../../images/butt_back.gif" alternatetext=Back onclick=btnBack_Click runat=server />
			</td>
		</tr>
		</table>
		<asp:Label id=lblErrMesage visible=false Text="Error while initiating component." runat=server />
		<asp:Label id="lblCode" visible="false" text=" Code" runat="server" />
		<asp:Label id="lblSelect" visible="false" text="Select " runat="server" />
		<asp:Label id="lblErrSelect" visible="false" text="Please select " runat="server" />
		<asp:Label id="lblErrEnter" visible="false" text="<br>Please enter " runat="server" />
		<asp:Label id=lblHiddenStatus visible=false text="0" runat=server/>
		<input type=hidden id=hidCompCode runat=server />
	</form>    
</body>

</html>
