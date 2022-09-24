<%@ Page Language="vb" src="../../../include/Admin_uom_UomConvertionList.aspx.vb" Inherits="Admin_UomConvertionList" %>
<%@ Register TagPrefix="UserControl" TagName="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<html>
	<head>
		<title>Unit of Measurement Convertion List</title>
		<Preference:PrefHdl id=PrefHdl runat="server" />
	</head>
	<body>
		<form id=frmUOMConvertionList runat="server">
    		<asp:Label id=lblErrMessage visible=false Text="Error while initiating component." runat=server />
			<asp:label id="SortExpression" Visible="False" Runat="server" />
			<table border="0" cellspacing="1" bordercolor="#111111" width="100%" style="border-collapse: collapse">
				<tr>
					<td colspan="7">
						<UserControl:MenuAdmin id=MenuAdmin runat="server" />
					</td>
				</tr>
				<tr>
					<td class="mt-h" width="100%" colspan="4">UNIT OF MEASUREMENT CONVERTION LIST</td>
					<td colspan="3" align=right><asp:label id="lblTracker" runat="server"/></td>					
				</tr>
				<tr>
					<td colspan=7><hr size="1" noshade></td>
				</tr>
				<tr>
					<td colspan=7 width=100% class="mb-c">
						<table width="100%" cellspacing="0" cellpadding="3" border="0" align="center">
							<tr class="mb-t">
								<td width="18%">UOM From :<BR><asp:TextBox id=txtUOMFrom width=100% maxlength="8" runat="server" /></td>
								<td width="18%">UOM To :<BR><asp:TextBox id=txtUOMTo width=100% maxlength="8" runat="server" /></td>
								<td width="19%">&nbsp;</td>
								<td width="15%">Status :<BR>
									<asp:DropDownList id="ddlStatus" width=100% size=1 runat=server>
										<asp:ListItem value="1" selected>Active</asp:ListItem>
										<asp:ListItem value="2">Deleted</asp:ListItem>
										<asp:ListItem value="0">All</asp:ListItem>
									</asp:DropDownList>
								</td>
								<td width="20%" align=left>LastUpdate By :<BR><asp:TextBox id=txtLastUpdate width=100% maxlength="10" runat="server"/></td>
								<td width="10%" valign=bottom align=right><asp:Button id=SearchBtn Text="Search" OnClick=srchBtn_Click runat="server"/></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan=7>					
						<asp:DataGrid id=dgUOMConvertion runat=server
							AutoGenerateColumns=false width=100% 
							GridLines=none 
							Cellpadding=2 
							AllowPaging=True 
							Allowcustompaging=False 
							Pagesize=15 
							OnPageIndexChanged=OnPageChanged 
							Pagerstyle-Visible=False 
							OnEditCommand=DEDR_Edit
							OnDeleteCommand=DEDR_Delete 
							OnSortCommand=Sort_Grid  
							AllowSorting=True>
								
							<HeaderStyle CssClass="mr-h" />
							
							<ItemStyle CssClass="mr-l" />
							
							<AlternatingItemStyle CssClass="mr-r" />
							
							<Columns>
								<asp:BoundColumn Visible=False HeaderText="UOM From" DataField="UOMFrom" />
								<asp:TemplateColumn HeaderText="UOM From" SortExpression="UOMFrom">
									<ItemTemplate>
										<asp:LinkButton id="btnUOMFrom" CommandName=Edit Text=<%# Container.DataItem("UOMFrom") %>
											runat="server"/>	
									</ItemTemplate>
								</asp:TemplateColumn>
																
								<asp:TemplateColumn HeaderText="UOM To" SortExpression="UOMTo">
									<ItemTemplate>
										<asp:LinkButton id="btnUOMTo" CommandName=Edit Text=<%# Container.DataItem("UOMTo") %>
											runat="server"/>	
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn HeaderText="Rate" SortExpression="Rate">
									<ItemTemplate>
										<%# objGlobal.DisplayQuantityFormat(Container.DataItem("Rate")) %>
									</ItemTemplate>
								</asp:TemplateColumn>

								<asp:TemplateColumn HeaderText="Last Update" SortExpression="UpdateDate">
									<ItemTemplate>
										<%# objGlobal.GetLongDate(Container.DataItem("UpdateDate")) %>
									</ItemTemplate>
								</asp:TemplateColumn>
									
								<asp:TemplateColumn HeaderText="Status" SortExpression="Status">
									<ItemTemplate>
										<%# objAdmin.mtdGetUOMStatus(Container.DataItem("Status")) %>
									</ItemTemplate>
								</asp:TemplateColumn>
									
								<asp:TemplateColumn HeaderText="Updated By" SortExpression="UpdatedBy">
									<ItemTemplate>
										<%# Container.DataItem("UserName") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:LinkButton id=lbDelete CommandName=Delete Text=Delete runat=server/>
										<asp:Label id=lblStatus Text='<%# Trim(Container.DataItem("Status")) %>' Visible=False runat=server />
									</ItemTemplate>
								</asp:TemplateColumn>	
							</Columns>
						</asp:DataGrid>
					</td>
				</tr>
				<tr>
					<td align=right colspan="7">
						<asp:ImageButton id="btnPrev" runat="server" imageurl="../../images/icn_prev.gif" alternatetext="Previous" commandargument="prev" onClick="btnPrevNext_Click" />
						<asp:DropDownList id="lstDropList" runat="server" AutoPostBack="True" onSelectedIndexChanged="PagingIndexChanged" />
			         	<asp:Imagebutton id="btnNext" runat="server"  imageurl="../../images/icn_next.gif" alternatetext="Next" commandargument="next" onClick="btnPrevNext_Click" />
					</td>
				</tr>
				<tr>
					<td align="left" width="100%" colspan=6>
						<asp:ImageButton id=NewUOMConvertionBtn onClick=NewUOMConvertionBtn_Click imageurl="../../images/butt_new.gif" AlternateText="New UOM Convertion" runat="server"/>
						<asp:ImageButton id=ibPrint imageurl="../../images/butt_print.gif" AlternateText=Print visible=false runat="server"/>
						<asp:Label id=SortCol Visible=False Runat="server" />
					</td>
				</tr>
			</table>
		</FORM>
	</body>
</html>
