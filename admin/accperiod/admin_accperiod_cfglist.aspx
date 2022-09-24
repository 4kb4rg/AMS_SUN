<%@ Page Language="vb" src="../../../include/admin_accperiod_cfglist.aspx.vb" Inherits="admin_accperiod_cfglist" %>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<html>
	<head>
		<title>Period Configuration List</title>
		<Preference:PrefHdl id=PrefHdl runat="server" />
	</head>
		<body>
		    <form runat="server">
				<asp:label id="SQLStatement" Visible="False" Runat="server"></asp:label>
				<asp:label id="SortExpression" Visible="False" Runat="server"></asp:label>
				<asp:label id="blnUpdate" Visible="False" Runat="server"></asp:label>
				<asp:label id="curStatus" Visible="False" Runat="server"></asp:label>
				<asp:label id="sortcol" Visible="False" Runat="server"></asp:label>

				<table border="0" cellspacing="1" width="100%">
					<tr>
						<td colspan="6">
							<UserControl:MenuAdmin id=MenuAdmin runat="server" />
						</td>
					</tr>
					<tr>
						<td class="mt-h" width="100%" colspan="3" height="19">PERIOD CONFIGURATION LIST</td>
						<td colspan="3" align=right><asp:label id="lblTracker" runat="server"/></td>
					</tr>
					<tr>
						<td colspan=6><hr size="1" noshade></td>
					</tr>
					<tr>
						<td colspan=6 width=100% class="mb-c">
							<table width="100%" cellspacing="0" cellpadding="3" border="0" align="center">
								<tr class="mb-t">
									<td width="15%">Accounting Year :<BR><asp:TextBox id=srchAccYear width=100% maxlength="8" runat="server"/></td>
									<td width="45%">&nbsp;</td>
									<td width="10%">Status :<BR>
										<asp:DropDownList id="srchStatusList" width=100% runat=server>
										</asp:DropDownList>
									</td>
									<td width="20%" align=left>Last Update By :<BR><asp:TextBox id=srchUpdateBy width=100% maxlength="10" runat="server"/></td>
									<td width="10%" valign=bottom align=left><asp:Button  Text="Search" OnClick=srchBtn_Click runat="server"/></td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td colspan=6>
							<asp:DataGrid id=dgResult
								AutoGenerateColumns="false" width="100%" runat="server"
								GridLines = none
								Cellpadding = "2"
								OnEditCommand="DEDR_Edit"
								OnUpdateCommand="DEDR_Update"
								OnCancelCommand="DEDR_Cancel"
								AllowPaging="True" 
								Allowcustompaging="False"
								Pagesize="15" 
								OnPageIndexChanged="OnPageChanged"
								Pagerstyle-Visible="False"
								OnSortCommand="Sort_Grid" 
								AllowSorting="True">
							<HeaderStyle CssClass="mr-h" />
							
							<ItemStyle CssClass="mr-l" />
							
							<AlternatingItemStyle CssClass="mr-r" />
							<Columns>
								<asp:TemplateColumn HeaderText="Accounting Year" SortExpression="HD.AccYear">
									<ItemTemplate>
										<%# Container.DataItem("AccYear") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="txtAccYear" runat="server" MaxLength="4" Size="4"
											Text='<%# Container.DataItem("AccYear") %>'	/>
										<BR>
										<asp:RequiredFieldValidator id=rfvAccYear display=dynamic runat=server 
											ErrorMessage="Please enter accounting year"
											ControlToValidate="txtAccYear" />
										<asp:RegularExpressionValidator id=revAccYear
											ControlToValidate="txtAccYear"
											ValidationExpression="[0-9]{1,4}"
											Display="Dynamic"
											text="<br>Please enter valid 4 digits of year."
											runat="server"/>
										<asp:label id="lblDupMsg" Text="Please try another accounting year." Visible = false forecolor=red Runat="server"/>
									</EditItemTemplate>
								</asp:TemplateColumn>
			
								<asp:TemplateColumn HeaderText="Maximum Accounting Period" SortExpression="HD.MaxPeriod">
									<ItemTemplate>
										<%# Container.DataItem("MaxPeriod") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="txtMaxPeriod" runat="server" Size="2" MaxLength="2"
											Text='<%# Container.DataItem("MaxPeriod") %>' />
										<asp:RequiredFieldValidator id=rfvMaxPeriod display=dynamic runat=server 
											ErrorMessage="<br>Please enter maximum accounting period."
											ControlToValidate="txtMaxPeriod" />															
										<asp:RangeValidator id="rvMaxPeriod"
											ControlToValidate="txtMaxPeriod"
											display=dynamic 
											MinimumValue="1"
											MaximumValue="23"
											Type="Integer"
											EnableClientScript="true"
											Text="<br>The value must be from 1 to 23."
											runat="server"/>
										<asp:label id="lblErrYear" Text="You are only allowed to change future accounting period." Visible=false forecolor=red Runat="server"/>
									</EditItemTemplate>
								</asp:TemplateColumn>
					
								<asp:TemplateColumn HeaderText="Last Update" SortExpression="HD.UpdateDate">
									<ItemTemplate>
										<%# objGlobal.GetLongDate(Container.DataItem("UpdateDate")) %>
									</ItemTemplate>
									<EditItemTemplate >
										<asp:TextBox id="txtUpdateDate" runat="server" Readonly=TRUE size=8 
											Text='<%# objGlobal.GetLongDate(Now()) %>' />
										<asp:TextBox id="txtCreateDate" runat="server" Visible=False
											Text='<%# Container.DataItem("CreateDate") %>' />
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Status" SortExpression="HD.Status">
									<ItemTemplate>
										<%# objAdmin.mtdGetAccPeriodCfgStatus(Container.DataItem("Status")) %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DropDownList id="ddlStatusList" size=1 runat=server />
										<asp:TextBox id="txtStatus" runat="server" Readonly=TRUE Visible = False
											Text='<%# Container.DataItem("Status")%>' />
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Updated By" SortExpression="UserName">
									<ItemTemplate>
										<%# Container.DataItem("UserName") %>
									</ItemTemplate>
									<EditItemTemplate >
										<asp:TextBox id="txtUserName" runat="server" Readonly=TRUE size=8 
											Text='<%# Session("SS_USERID") %>'	/>
									</EditItemTemplate>
								</asp:TemplateColumn>
					
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:LinkButton id="Edit" runat="server" CommandName="Edit" Text="Edit" />
									</ItemTemplate>
									<EditItemTemplate>
										<asp:LinkButton id="Update" runat="server" CommandName="Update" Text="Save" />
										<asp:LinkButton id="Cancel" runat="server" CommandName="Cancel" Text="Cancel"
											 CausesValidation=False />
									</EditItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
					</td>
				</tr>
				<tr >
					<td align=right colspan="6">
						<asp:Imagebutton id="btnPrev" runat="server" imageurl="../../images/icn_prev.gif" alternatetext="Previous" commandargument="prev" onClick="btnPrevNext_Click" />
			            <asp:dropdownlist id="lstDropList" AutoPostBack="True" onselectedindexchanged="PagingIndexChanged" runat="server" />
			         	<asp:Imagebutton id="btnNext" runat="server"  imageurl="../../images/icn_next.gif" alternatetext="Next" commandargument="next" onClick="btnPrevNext_Click" />
					</td>
				</tr>
				<tr>
					<td align="left" width="100%" ColSpan=6>
						<asp:ImageButton id=ibNew onClick=DEDR_Add imageurl="../../images/butt_new.gif" AlternateText="New Accounting" runat="server"/>
						<asp:ImageButton id=ibPrint imageurl="../../images/butt_print.gif" AlternateText=Print visible=false runat="server"/>
					</td>
				</tr>
			</table>
		</FORM>
	</body>
</html>
