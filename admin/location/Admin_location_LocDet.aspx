<%@ Page Language="vb" src="../../../include/Admin_location_LocDet.aspx.vb" Inherits="Admin_LocDet" %>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<%@ Register Assembly="Infragistics2.WebUI.UltraWebTab.v7.3" Namespace="Infragistics.WebUI.UltraWebTab"
    TagPrefix="igtab" %>


<html>
	<head>
		<title>Location Details</title>
	</head>
	<body>
		<Preference:PrefHdl id=PrefHdl runat="server" />
		<Form id=frmMain runat="server">
			<asp:Label id=lblErrMessage visible=false Text="Error while initiating component." runat=server />
			<asp:Label id="lblCode" visible="false" text=" Code" runat="server" />
			<asp:Label id="lblErrEnter" visible="false" text="<br>Please enter " runat="server" />
			<asp:Label id="lblSelect" visible="false" text="Select " runat="server" />
			<table border=0 cellspacing=0 cellpadding=2 width=100%>
				<tr>
					<td colspan=5>
						<UserControl:MenuAdmin id=MenuAdmin runat="server" />
					</td>
				</tr>
				<tr>
					<td class="mt-h" colspan=5><asp:label id="lblTitle" runat="server" /> DETAILS</td>
				</tr>
				<tr>
					<td colspan=5><hr size="1" noshade></td>
				</tr>
				<tr>
					<td width=20% height=25><asp:label id="lblLocation" runat="server" /> Code :* </td>
					<td width=30%>
						<asp:TextBox id=txtLocCode width=50% maxlength=4 runat=server />
						<asp:RequiredFieldValidator id=rfvCode display=dynamic runat=server  
							ControlToValidate=txtLocCode />
						<asp:RegularExpressionValidator id=revCode 
							ControlToValidate="txtLocCode"
							ValidationExpression="[a-zA-Z0-9\-]{1,4}"
							Display="Dynamic"
							text="<br>Alphanumeric without any space in between only."
							runat="server"/>
						<asp:label id=lblErrBlank text="Please enter Location Code" visible=false forecolor=red runat=server />
						<asp:label id=lblErrDup text="Duplicate Location Code" visible=false forecolor=red runat=server /></td>
					<td width=5%>&nbsp;</td>
					<td width=15%>Status : </td>
					<td width=30%><asp:Label id=lblStatus runat=server />
								  <asp:Label id=lblHiddenStatus visible=false text="0" runat=server/></td>
				</tr>
				<tr>
					<td><asp:label id="lblDesc" runat="server" /> : </td>
					<td><asp:TextBox id=txtDescription width=100% maxlength=128 runat=server /> </td>
					<td>&nbsp;</td>
					<td>Date Created : </td>
					<td><asp:Label id=lblDateCreated runat=server /></td>
				</tr>
				<tr>
					<td height=25>Address :*</td>
					<td rowspan="5" valign=top>
						<textarea rows="7" id=txtAddress cols="29" runat=server></textarea>
						<asp:Label id=lblErrAddress visible=false forecolor=red text="<br>Maximum length for address is up to 512 characters only." runat=server />
						<asp:RequiredFieldValidator id=validateAddress display=dynamic 
							runat=server 
							ErrorMessage="<br>Please enter address. " 
							ControlToValidate=txtAddress /></td>
					<td>&nbsp;</td>
					<td>Last Updated : </td>
					<td><asp:Label id=lblLastUpdate runat=server /></td>
				</tr>				
				<tr>
					<td height=25>&nbsp;</td>
					<td>&nbsp;</td>
					<td>Updated By : </td>
					<td><asp:Label id=lblUpdatedBy runat=server /></td>
				</tr>
				<tr>
					<td height=25>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td height=25>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td height=25>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td height=25>Post Code :*</td>
					<td><asp:TextBox id=txtPostCode width=50% maxlength=16 runat=server />
						<asp:RequiredFieldValidator id=validatePostCode display=dynamic runat=server 
							ErrorMessage="<br>Please enter postcode. " 
							ControlToValidate=txtPostCode />
						<asp:CompareValidator id="validateNumPostCode" display=dynamic runat="server" 
							ControlToValidate="txtPostCode" Text="<br>The value must whole number. " 
							Type="integer" Operator="DataTypeCheck"/></td>
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
							runat="server"/></td>
				</tr>
				<tr>
					<td height=25>City :*</td>
					<td><asp:TextBox id=txtCity width=100% maxlength=32 runat=server />
						<asp:RequiredFieldValidator id=validateCity display=dynamic runat=server 
							ErrorMessage="<br>Please enter city. " 
							ControlToValidate=txtCity />
						<asp:CompareValidator id="validateCityState" display=dynamic runat="server" 
							ControlToValidate="txtCity" Text="<br>The value must whole characters. " 
							Type="string" Operator="DataTypeCheck"/></td>
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
							runat="server"/></td>
				</tr>
				<tr>
					<td height=25>State :*</td>
					<td><asp:TextBox id=txtState width=100% maxlength=32 runat=server />
						<asp:RequiredFieldValidator id=validateState display=dynamic runat=server 
							ErrorMessage="<br>Please enter state. " 
							ControlToValidate=txtState />
						<asp:CompareValidator id="validateStrState" display=dynamic runat="server" 
							ControlToValidate="txtState" Text="<br>The value must be whole characters. " 
							Type="string" Operator="DataTypeCheck"/>
					</td>
					<td>&nbsp;</td>
					<td>NPWP Reference No. :*</td>
					<td><asp:Textbox id=txtNPWP width=100% maxlength=20 runat=server/>
					<asp:RequiredFieldValidator id=validateNPWP display=dynamic runat=server 
						ErrorMessage="<br>Please enter NPWP Pusat Reference No. " 
						ControlToValidate=txtNPWP />
					</td>
				</tr>
				<tr>
					<td height=25>Country :*</td>
					<td><asp:DropDownList id=ddlCountry width=100% runat=server />
						<asp:Label id=lblErrCountry visible=false forecolor=red text="<br>Please select one country code." runat=server/>
    				</td>
					<td>&nbsp;</td>
					<td>JAMSOSTEK Reference No. :*</td>
					<td><asp:Textbox id=txtJamsostekNo width=100% maxlength=20 runat=server/>
					<asp:RequiredFieldValidator id=validateJamsostekNo display=dynamic runat=server 
						ErrorMessage="<br>Please enter JAMSOSTEK Reference No. " 
						ControlToValidate=txtJamsostekNo />
					</td>
				</tr>				
				<tr>
					<td height=25><asp:label id="lblAccount" runat="server" /> to charge : </td>
					<td><asp:DropDownList id=ddlAccount width=90% runat=server />
						<input type="button" id=btnFind value=" ... " onclick="javascript:findcode('frmMain','','ddlAccount','1','','','','','','','','','','','','',hidBlockCharge.value,hidChargeLocCode.value);" runat=server/>
					</td>
					<td>&nbsp;</td>
					<td>PPN Rate :</td>
					<td><asp:Textbox id=txtPPNRate width=50% maxlength=9 text=0 runat=server/>&nbsp;%
						<asp:RegularExpressionValidator id="RegularExpressiontxtPPNRate" 
						ControlToValidate="txtPPNRate"
						ValidationExpression="\d{1,3}\.\d{1,5}|\d{1,3}"
						Display="Dynamic"
						text = "Maximum length 3 digits and 5 decimal points"
						runat="server"/></td>
				</tr>
				<tr>
					<td height=25>Allow incomplete vehicle running distribution to close ?</td>
					<td><asp:RadioButton id="CloseAutoDistInd_Yes" 
							Checked="True"
							GroupName="CloseAutoDistInd"
							Text="Yes"
							TextAlign="Right"
							runat="server" /></td>
					<td>&nbsp;</td>
					<td>Upah Minimum Regional (UMR) :</td>
					<td><asp:Textbox id=txtUMR width=50% maxlength=9 text=0 runat=server/>
						<asp:RegularExpressionValidator id="RegularExpressiontxtUMR" 
						ControlToValidate="txtUMR"
						ValidationExpression="\d{1,14}\.\d{1,5}|\d{1,14}"
						Display="Dynamic"
						text = "Maximum length 5 decimal points"
						runat="server"/></td>					
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td height=25>&nbsp;</td>
					<td><asp:RadioButton id="CloseAutoDistInd_No" 
							Checked="False"
							GroupName="CloseAutoDistInd"
							Text="No"
							TextAlign="Right"
							runat="server" /></td>
					<td>&nbsp;</td>
					<td><asp:Label id="lblNearLocCode" runat="server" /> :</td>
					<td><asp:DropDownList id=ddlNearLocCode width=100%  runat=server /></td>
				</tr>
				
				<tr>
				<td height=25>Location Type </td>
					<td><asp:RadioButton id="LocType_Mill" 
							Checked="False"
							GroupName="LocTypeGroup"
							Text="Mill"
							TextAlign="Right"
							runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							Mill Capacity : 
							<asp:Textbox id=txtMillCapacity width=30% maxlength=14 text=0 runat=server/>
							<asp:RegularExpressionValidator id="RegularExpressiontxtMillCapacity" 
								ControlToValidate="txtMillCapacity"
								ValidationExpression="\d{1,9}\.\d{1,5}|\d{1,14}"
								Display="Dynamic"
								text = "Maximum length 5 decimal points"
								runat="server"/></td>
					<td>&nbsp;</td>
					<td>Manager :</td>
					<td><asp:Textbox id=txtEstMgr width=50% maxlength=32  runat=server/> </td>					
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
				<td height=25>&nbsp;</td>
					<td><asp:RadioButton id="LocType_Estate" 
							Checked="True"
							GroupName="LocTypeGroup"
							Text="Estate"
							TextAlign="Right"
							runat="server" /></td>
					<td>&nbsp;</td>
					<td>KASIE/KTU :</td>
					<td><asp:Textbox id=txtKasie width=50% maxlength=32  runat=server/> </td>					
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
				<td height=25>Level </td>
					<td><asp:RadioButton id="LocLevel_Estate" 
							Checked="True"
							GroupName="LocLevelGroup"
							Text="Estate"
							TextAlign="Right"
							runat="server" /></td>
					<td>&nbsp;</td>
				</tr>
				<tr>
				<td height=25>&nbsp;</td>
					<td><asp:RadioButton id="LocLevel_Perwakilan" 
							Checked="False"
							GroupName="LocLevelGroup"
							Text="Perwakilan"
							TextAlign="Right"
							runat="server" /></td>
					<td>&nbsp;</td>
				</tr>
				<tr>
				<td height=25>&nbsp;</td>
					<td><asp:RadioButton id="LocLevel_HQ" 
							Checked="False"
							GroupName="LocLevelGroup"
							Text="Kantor Pusat"
							TextAlign="Right"
							runat="server" /></td>
					<td>&nbsp;</td>
				</tr>
				
				<tr>
					<td>PR Running Digit prefix :*</td>
					<td>
						<asp:DropDownList id=ddlRDP width=100% runat=server />
						<asp:Label id=lblErrRDP visible=false forecolor=red text="<br>Please select one Running Digit Prefix." runat=server/>
    				</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td>Prefix(1 Digit)+Running Digits(3 Digits)</td> 
				</tr>

				<tr>
					<td height=25>Employee Code Prefix :</td>
					<td><asp:TextBox id=txtEmpPrefix width=25% maxlength=2 runat=server /></td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td valign=top height=25>Employee Code Running Digit :</td>
					<td valign=top>
						<asp:TextBox id=txtEmpCodeDigit width=25% maxlength=1 value=4 runat=server />
						<asp:RangeValidator 
							id="Range1"
							ControlToValidate="txtEmpCodeDigit"
							MinimumValue="0"
							MaximumValue="9"
							Type="integer"
							EnableClientScript="True"
							Text="Numeric number only."
							runat="server" display="dynamic"/>
						<br>
						Prefix(2 digits) + Year(2 digits) + Running Digit(Max 9 digits)</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td height=25>Current Running Number :</td>
					<td><asp:Label id=lblCurrId value=0 runat=server /></td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<!-- tr>
					<td align="left">Analysis Control :</td>
					<td align="left">Cost:</td>
					<td align="left">
						<asp:RadioButton visible=false id="AnaCtrlCost_Block" 
							Checked="True"
							GroupName="AnaCtrlCost"
							Text="Block"
							TextAlign="Right"
							runat="server" /></td>
					<td align="left">
						<asp:RadioButton visible=false id="AnaCtrlCost_SubBlock" 
							Checked="False"
							GroupName="AnaCtrlCost"
							Text="Sub Block"
							TextAlign="Right"
							runat="server" /></td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td align="left">&nbsp;</td>
					<td align="left">Yield:</td>
					<td align="left">
						<asp:RadioButton visible=false id="AnaCtrlYield_Block" 
							Checked="True"
							GroupName="AnaCtrlYield"
							Text="Block"
							TextAlign="Right"
							runat="server" /></td>
					<td align="left">
						<asp:RadioButton visible=false id="AnaCtrlYield_SubBlock" 
							Checked="False"
							GroupName="AnaCtrlYield"
							Text="Sub Block"
							TextAlign="Right"
							runat="server" /></td>
					<td>&nbsp;</td>
					<td>&nbsp;</td>
				</tr-->
				<tr><td colspan=5>&nbsp;</tr>
				<tr>
					<td colspan=5>
						<asp:ImageButton id=SaveBtn AlternateText="  Save  " imageurl="../../images/butt_save.gif" onClick=Button_Click CommandArgument=Save runat=server />
						<asp:ImageButton id=DelBtn AlternateText=" Delete " CausesValidation=False imageurl="../../images/butt_delete.gif" onClick=Button_Click CommandArgument=Del runat=server />
						<asp:ImageButton id=UnDelBtn AlternateText="Undelete" imageurl="../../images/butt_undelete.gif" onClick=Button_Click CommandArgument=UnDel runat=server />
						<asp:ImageButton id=BackBtn AlternateText="  Back  " CausesValidation=False imageurl="../../images/butt_back.gif" onClick=BackBtn_Click runat=server />
					</td>
				</tr>
				<tr><td colspan=5>&nbsp;</tr>
				<tr>
                    <td style="height: 24px;" colspan="5">
                        <igtab:UltraWebTab ID="TABBK" ThreeDEffect="False" TabOrientation="TopLeft"
                                SelectedTab="0" Font-Names="Tahoma" Font-Size="8pt" runat="server">
                                <DefaultTabStyle ForeColor="black" Height="22px">
                                </DefaultTabStyle>
                                <HoverTabStyle CssClass="ContentTabHover">
                                </HoverTabStyle>
                                <RoundedImage LeftSideWidth="6" RightSideWidth="6" SelectedImage="../../images/thumbs/ig_tab_winXP1.gif"
                                    NormalImage="../../images/thumbs/ig_tab_winXP3.gif" HoverImage="../../images/thumbs/ig_tab_winXP2.gif"
                                    FillStyle="LeftMergedWithCenter"></RoundedImage>
                                <SelectedTabStyle CssClass="ContentTabSelected">
                                </SelectedTabStyle>
                                <Tabs>
                                    <%--Bank Correspondent--%>
                                    <igtab:Tab Key="NRCDET" Text="Bank Correspondent" Tooltip="Bank Correspondent">
                                        <ContentPane>
                                            <table border="0" cellspacing="1" cellpadding="1" width="99%">
                                                <tr>
                                                    <td colspan="5">
                                                        <div id="div1" style="height: 300px;width:1010;overflow: auto;">		
                                                            <table id="tblSelection" cellSpacing="0" cellPadding="2" width="100%" border="0" runat=server>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Bank : </td>
                                                                    <td colSpan="5"><asp:DropDownList id=ddlBankCode width=90% runat=server />
                                                                                    <asp:label id=lblerrBankCode Visible=False Text="Tax object cannot be empty" forecolor=red Runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Bank Account Name : </td>
                                                                    <td colSpan="5"><asp:TextBox id=txtBankAccName width=90% maxlength=64 runat=server />
                                                                                    <asp:Label id=lblErrBankAccName visible=false forecolor=red text=" Please enter row number on reports." runat=server />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Bank Account Number : </td>
                                                                    <td colSpan="5"><asp:TextBox id=txtBankAccNo width=90% maxlength=32 runat=server />
                                                                                    <asp:Label id=lblerrBankAccNo visible=false forecolor=red text=" Please enter row number on reports." runat=server />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Bank Address : </td>
                                                                    <td colSpan="5"><asp:TextBox ID=txtBankAddress maxlength=512 width=90% Enabled=true Runat=server />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Bank Town : </td>
                                                                    <td colSpan="5"><asp:TextBox id=txtBankTown width=90% maxlength=64 runat=server />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Bank State : </td>
                                                                    <td colSpan="5"><asp:TextBox id=txtBankState width=90% maxlength=64 runat=server />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Bank Country : </td>
                                                                    <td colSpan="5"><asp:DropDownList id=ddlBankCountry width=90% runat=server />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Account Code : </td>
                                                                    <td colSpan="5"><asp:DropDownList id=ddlBankAccCode width=90% runat=server />
                                                                                    <asp:label id=lblErrBankAccCode Visible=False Text="Account code object cannot be empty" forecolor=red Runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td width="20%" height=25>Remark : </td>
                                                                    <td colSpan="5"><asp:TextBox id=txtBankRemark width=90% maxlength=64 runat=server />
                                                                    </td>
                                                                </tr>
                                                                <tr class="mb-c">
                                                                    <td colSpan="6">&nbsp;</td>
                                                                </tr>	
                                                                <tr class="mb-c">
                                                                    <td height=25 colspan=2>
                                                                        <asp:ImageButton id=AddBtn imageurl="../../images/butt_add.gif" alternatetext=Add CausesValidation=True onclick=AddBtn_Click UseSubmitBehavior="false" runat=server /> 									
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </div>
				                                        <div id="div2" style="height: 400px;width:1010;overflow: auto;"> 
                                                            <asp:DataGrid id=dgLineDet
						                                        AutoGenerateColumns="false" width="100%" runat="server"
						                                        GridLines=none
						                                        Cellpadding="2"
						                                        Pagerstyle-Visible="False"
						                                        OnEditCommand="DEDR_Edit"
						                                        OnCancelCommand="DEDR_Cancel"
						                                        OnDeleteCommand="DEDR_Delete"
						                                        AllowSorting="True">
						                                        <HeaderStyle CssClass="mr-h"/>
						                                        <ItemStyle CssClass="mr-l"/>
						                                        <AlternatingItemStyle CssClass="mr-r"/>
						                                        <Columns>	
							                                        <asp:TemplateColumn HeaderText="Bank">
								                                        <ItemTemplate>
									                                        <asp:Label visible=true Text=<%# Container.DataItem("BankDescr") %> id="lblBankDescr" runat="server" />
									                                        <asp:Label visible=false Text=<%# Container.DataItem("BankCode") %> id="lblBankCode" runat="server" />
								                                        </ItemTemplate>
							                                        </asp:TemplateColumn>
							                                        <asp:TemplateColumn HeaderText="Acc. Name">
								                                        <ItemTemplate>
									                                        <asp:Label visible=true Text=<%# Container.DataItem("BankAccName") %> id="lblBankAccName" runat="server" />
								                                        </ItemTemplate>
							                                        </asp:TemplateColumn>
							                                        <asp:TemplateColumn HeaderText="Acc. No">
								                                        <ItemTemplate> 
									                                        <ItemStyle />
										                                        <asp:Label Text=<%# Container.DataItem("BankAccNo") %> id="lblBankAccNo" runat="server" />
									                                        </ItemStyle>
								                                        </ItemTemplate>
							                                        </asp:TemplateColumn>		
							                                         <asp:TemplateColumn HeaderText="Bank Address">
								                                        <ItemTemplate> 
									                                        <ItemStyle />
										                                        <asp:Label Text=<%# Container.DataItem("BankAddress") %> id="lblBankAddress" runat="server" />
									                                        </ItemStyle>
								                                        </ItemTemplate>
							                                        </asp:TemplateColumn>	
							                                        <asp:TemplateColumn HeaderText="Bank Town">
								                                        <ItemTemplate> 
									                                        <ItemStyle />
										                                        <asp:Label Text=<%# Container.DataItem("BankTown") %> id="lblBankTown" runat="server" />
									                                        </ItemStyle>
								                                        </ItemTemplate>
							                                        </asp:TemplateColumn>	
							                                        <asp:TemplateColumn HeaderText="Account Code">
								                                        <ItemTemplate> 
									                                        <ItemStyle />
										                                        <asp:Label Text=<%# Container.DataItem("AccCode") %> id="lblBankAccCode" runat="server" />
									                                        </ItemStyle>
								                                        </ItemTemplate>
							                                        </asp:TemplateColumn>	
							                                        <asp:TemplateColumn HeaderText="Remark">
								                                        <ItemTemplate> 
									                                        <ItemStyle />
										                                        <asp:Label Text=<%# Container.DataItem("BankRemark") %> id="lblBankRemark" runat="server" />
									                                        </ItemStyle>
								                                        </ItemTemplate>
							                                        </asp:TemplateColumn>	
							                                        <asp:TemplateColumn ItemStyle-Width="5%" ItemStyle-HorizontalAlign=Right>
								                                        <ItemTemplate>
								                                            <asp:label text= '<%# Container.DataItem("BankState") %>' Visible=False id="lblBankState" runat="server" />
								                                            <asp:label text= '<%# Container.DataItem("BankCountry") %>' Visible=False id="lblBankCountry" runat="server" />
								                                            <asp:LinkButton id=lbEdit CommandName="Edit" Text="Edit" CausesValidation=False  runat="server"/>
									                                        <asp:LinkButton id=lbDelete CommandName="Delete" Text="Delete" CausesValidation=False runat=server />
								                                            <asp:LinkButton id=lbCancel CommandName="Cancel" Text="Cancel" CausesValidation=False  runat="server"/>
								                                        </ItemTemplate>
							                                        </asp:TemplateColumn>	
					                                        </Columns>										
					                                        </asp:DataGrid>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
				                                    <td colspan=5>&nbsp;</td>
			                                    </tr>
                                            </table>
                                        </ContentPane>
                                    </igtab:Tab>
                            </Tabs>
                        </igtab:UltraWebTab>
                    </td>
                </tr>
				<tr><td colspan=5>&nbsp;</tr>
				
				<Input Type=Hidden id=LocCode runat=server />
				<Input type=hidden id=hidBlockCharge value="" runat=server/>
				<Input type=hidden id=hidChargeLocCode value="" runat=server/>
                <Input type=hidden id=hidBankAccNo value="" runat=server/>
				
			</table>
		</form>
	</body>
</html>
