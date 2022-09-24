<%@ Page Language="vb" src="../../../include/admin_comp_list.aspx.vb" Inherits="admin_comp_list" %>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<html>

<head>
<title>Company List</title>
<Preference:PrefHdl id=PrefHdl runat="server" />
</head>
	<body>
	    <form id=frmUserList runat="server">
			<asp:label id="SortExpression" Visible="False" Runat="server" />
			<table border="0" cellspacing="1" cellpadding="1" width="100%">
				<tr>
					<td colspan="6">
						<UserControl:MenuAdmin id=MenuAdmin runat="server" />
					</td>
				</tr>
				<tr>
					<td class="mt-h" colspan="3"><asp:label id="lblTitle" runat="server" /> LIST</td>
					<td colspan="3" align=right><asp:label id="lblTracker" runat="server"/></td>
				</tr>
				<tr>
					<td colspan=6><hr size="1" noshade></td>
				</tr>
				<tr>
					<td colspan=6 width=100% class="mb-c">
						<table width="100%" cellspacing="0" cellpadding="3" border="0" align="center">
							<tr class="mb-t">
								<td valign=bottom width=15% align=left>
									<asp:label id="lblCompany" runat="server" /> Code :<BR>
									<asp:TextBox id=txtCoCode width=100% maxlength="20" runat="server" />
								</td>
								<td valign=bottom width=45% align=left>
									<asp:label id="lblCompName" runat="server" /> :<BR>
									<asp:TextBox id=txtName width=100% maxlength="128" runat="server"/></td>
								<td valign=bottom width=10% align=left>Status :<BR>
									<asp:DropDownList id="ddlStatus" width=100% runat=server>
										<asp:ListItem value="0" >All</asp:ListItem>
										<asp:ListItem value="1" Selected>Active</asp:ListItem>
										<asp:ListItem value="2">Deleted</asp:ListItem>
									</asp:DropDownList>
								</td>
								<td valign=bottom width=20% align=left>LastUpdate By :<BR><asp:TextBox id=txtLastUpdate width=100% maxlength="10" runat="server"/></td>
								<td valign=bottom width=10% align=left><asp:Button id=SearchBtn Text="Search" OnClick=srchBtn_Click runat="server"/></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan=6>					
						<asp:DataGrid id=dgCoList
							AutoGenerateColumns=false width=100% runat=server
							GridLines=none 
							Cellpadding=2 
							AllowPaging=True 
							Allowcustompaging=False 
							Pagesize=15 
							OnPageIndexChanged=OnPageChanged 
							Pagerstyle-Visible=False 
							OnDeleteCommand=DEDR_Delete 
							OnSortCommand=Sort_Grid  
							AllowSorting=True>
							
							<HeaderStyle CssClass="mr-h" />
							
							<ItemStyle CssClass="mr-l" />
							
							<AlternatingItemStyle CssClass="mr-r" />
							
							<Columns>
								<asp:BoundColumn Visible=False HeaderText="Company Code" DataField="CompCode" />
								
								<asp:HyperLinkColumn HeaderText="Company Code" 
													 SortExpression="CompCode" 
													 DataNavigateUrlField="CompCode" 
													 DataNavigateUrlFormatString="admin_comp_det.aspx?compcode={0}" 
													 DataTextField="CompCode" />
								
								<asp:HyperLinkColumn HeaderText="Name" 
													 SortExpression="CompName" 
													 DataNavigateUrlField="CompCode" 
													 DataNavigateUrlFormatString="admin_comp_det.aspx?compcode={0}" 
													 DataTextField="CompName" />
							
								<asp:TemplateColumn HeaderText="Last Update" SortExpression="UpdateDate">
									<ItemTemplate>
										<%# objGlobal.GetLongDate(Container.DataItem("UpdateDate")) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn HeaderText="Status" SortExpression="Status">
									<ItemTemplate>
										<%# objAdminComp.mtdGetCompanyStatus(Container.DataItem("Status")) %>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn HeaderText="Updated By" SortExpression="UpdateID">
									<ItemTemplate>
										<%# Container.DataItem("UpdateID") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn ItemStyle-HorizontalAlign=Right HeaderStyle-VerticalAlign=Bottom ItemStyle-VerticalAlign=Bottom>
									<ItemTemplate>
										<asp:LinkButton id=lbDelete CommandName=Delete Text=Delete runat=server />
										<asp:Label id=lblStatus Text='<%# Trim(Container.DataItem("Status")) %>' Visible=False runat=server />
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid>
					</td>
				</tr>
				<tr>
					<td align=right colspan="6">
						<asp:ImageButton id="btnPrev" runat="server" imageurl="../../images/icn_prev.gif" alternatetext="Previous" commandargument="prev" onClick="btnPrevNext_Click" />
						<asp:DropDownList id="lstDropList" AutoPostBack="True" onSelectedIndexChanged="PagingIndexChanged" runat="server" />
			         	<asp:Imagebutton id="btnNext" runat="server"  imageurl="../../images/icn_next.gif" alternatetext="Next" commandargument="next" onClick="btnPrevNext_Click" />
					</td>
				</tr>
				<tr>
					<td align="left" ColSpan=6>
						<asp:ImageButton id=NewCoBtn onClick=NewCoBtn_Click imageurl="../../images/butt_new.gif" AlternateText="New Company" runat="server"/>
						<asp:ImageButton id=ibPrint imageurl="../../images/butt_print.gif" AlternateText=Print visible=false runat="server"/>
						<asp:Label id=SortCol Visible=False Runat="server" />
						<asp:Label id=lblErrMesage visible=false Text="Error while initiating component." runat=server />
						<asp:Label id=lblCode visible=false Text=" Code" runat=server />
					</td>
				</tr>
			</table>
		</FORM>
	</body>
</html>
