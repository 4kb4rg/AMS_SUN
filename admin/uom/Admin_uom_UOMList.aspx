<%@ Page Language="vb" src="../../../include/Admin_uom_UOMList.aspx.vb" Inherits="Admin_UOMList" %>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<html>
	<head>
		<title>Unit of Measurement List</title>
		<Preference:PrefHdl id=PrefHdl runat="server" />
	</head>
		<body>
		    <form runat="server">
				<asp:label id="SQLStatement" Visible="False" Runat="server"></asp:label>
				<asp:label id="SortExpression" Visible="False" Runat="server"></asp:label>
				<asp:label id="blnUpdate" Visible="False" Runat="server"></asp:label>
    			<asp:Label id=lblErrMessage visible=false Text="Error while initiating component." runat=server />
				<asp:label id="curStatus" Visible="False" Runat="server"></asp:label>
				<asp:label id="sortcol" Visible="False" Runat="server"></asp:label>
				<asp:Label id="ErrorMessage" runat="server" />

				<table border="0" cellspacing="1" bordercolor="#111111" width="100%" style="border-collapse: collapse">
					<tr>
						<td colspan="6">
							<UserControl:MenuAdmin id=MenuAdmin runat="server" />
						</td>
					</tr>
					<tr>
						<td class="mt-h" width="100%" colspan="3" height="19">UNIT OF MEASUREMENT LIST</td>
						<td colspan="3" align=right><asp:label id="lblTracker" runat="server"/></td>
					</tr>
					<tr>
						<td colspan=6><hr size="1" noshade></td>
					</tr>
					<tr>
						<td colspan=6 width=100% class="mb-c">
							<table width="100%" cellspacing="0" cellpadding="3" border="0" align="center">
								<tr class="mb-t">
									<td width="15%">UOM Code :<BR><asp:TextBox id=srchUOMCode width=100% maxlength="8" runat="server"/></td>
									<td width="45%">Description :<BR><asp:TextBox id=srchDesc width=100% maxlength="64" runat="server"/></td>
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
							<asp:DataGrid id=dgUOM
								AutoGenerateColumns="false" width="100%" runat="server"
								GridLines = none
								Cellpadding = "2"
								OnEditCommand="DEDR_Edit"
								OnUpdateCommand="DEDR_Update"
								OnCancelCommand="DEDR_Cancel"
								OnDeleteCommand="DEDR_Delete"
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
								<asp:TemplateColumn HeaderText="UOM Code" SortExpression="UOMCode">
									<ItemTemplate>
										<%# Container.DataItem("UOMCode") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="UOMCode" runat="server" MaxLength="8" Size="8"
											Text='<%# Container.DataItem("UOMCode") %>'	/>
										<BR>
										<asp:RequiredFieldValidator id=validateCode display=dynamic runat=server 
											ErrorMessage="Please Enter UOM Code"
											ControlToValidate=UOMCode />
										<asp:RegularExpressionValidator id=revCode 
											ControlToValidate="UOMCode"
											ValidationExpression="[a-zA-Z0-9\-]{1,8}"
											Display="Dynamic"
											text="<br>Alphanumeric without any space in between only."
											runat="server"/>
										<asp:label id="lblDupMsg"  Text="Code already exist" Visible = false forecolor=red Runat="server"/>
									</EditItemTemplate>
								</asp:TemplateColumn>
			
								<asp:TemplateColumn HeaderText="Description" SortExpression="UOMDesc">
									<ItemTemplate>
										<%# Container.DataItem("UOMDesc") %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:TextBox id="UOMDesc" runat="server" Size=35 MaxLength="64"
											Text='<%# Trim(Container.DataItem("UOMDesc")) %>' />
										<BR>
										<asp:RequiredFieldValidator id=validateDesc display=dynamic runat=server 
											ErrorMessage="Please Enter UOM Description"
											ControlToValidate=UOMDesc />															
									</EditItemTemplate>
								</asp:TemplateColumn>
					
								<asp:TemplateColumn HeaderText="Last Update" SortExpression="UOM.UpdateDate">
									<ItemTemplate>
										<%# objGlobal.GetLongDate(Container.DataItem("UpdateDate")) %>
									</ItemTemplate>
									<EditItemTemplate >
										<asp:TextBox id="UpdateDate" runat="server" Readonly=TRUE size=8 
											Text='<%# objGlobal.GetLongDate(Now()) %>' />
										<asp:TextBox id="CreateDate" runat="server" Visible=False
											Text='<%# Container.DataItem("CreateDate") %>' />
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Status" SortExpression="UOM.Status">
									<ItemTemplate>
										<%# objAdmin.mtdGetUOMStatus(Container.DataItem("Status")) %>
									</ItemTemplate>
									<EditItemTemplate>
										<asp:DropDownList id="StatusList" size=1 runat=server />
										<asp:TextBox id="Status" runat="server" Readonly=TRUE Visible = False
											Text='<%# Container.DataItem("Status")%>' />
									</EditItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="Updated By" SortExpression="UserName">
									<ItemTemplate>
										<%# Container.DataItem("UserName") %>
									</ItemTemplate>
									<EditItemTemplate >
										<asp:TextBox id="UserName" runat="server" Readonly=TRUE size=8 
											Text='<%# Session("SS_USERID") %>'	/>
									</EditItemTemplate>
								</asp:TemplateColumn>
					
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:LinkButton id="Edit" runat="server" CommandName="Edit" Text="Edit" />
									</ItemTemplate>
									<EditItemTemplate>
										<asp:LinkButton id="Update" runat="server" CommandName="Update" Text="Save" />
										<asp:LinkButton id="Delete" runat="server" CommandName="Delete" Text="Delete" />
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
						<asp:ImageButton id=ibNew onClick=DEDR_Add imageurl="../../images/butt_new.gif" AlternateText="New Unit of Measurement" runat="server"/>
						<asp:ImageButton id=ibPrint imageurl="../../images/butt_print.gif" AlternateText=Print visible=false runat="server"/>
					</td>
				</tr>
			</table>
		</FORM>
	</body>
</html>
