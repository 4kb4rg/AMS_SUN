<%@ Page Language="vb" src="../../../include/Admin_location_LocList.aspx.vb" Inherits="Admin_LocList" %>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<html>
	<head>
		<title>Location List</title>
		<Preference:PrefHdl id=PrefHdl runat="server" />
	</head>
	<body>
		<form id=frmLocList runat="server">
    		<asp:Label id=lblErrMessage visible=false Text="Error while initiating component." runat=server />
    		<asp:label id="lblCode" visible="false" text=" Code" runat="server" />
			<asp:label id="SortExpression" Visible="False" Runat="server" />
			<table border="0" cellspacing="1" bordercolor="#111111" width="100%" style="border-collapse: collapse">
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
								<td width="15%" height="26"><asp:label id="lblLocation" runat="server" /> Code :<BR><asp:TextBox id=txtLocCode width=100% maxlength="8" runat="server" /></td>
								<td width="45%" height="26"><asp:label id="lblDesc" runat="server" /> :<BR><asp:TextBox id=txtDescription width=100% maxlength="128" runat="server"/></td>
								<td width="10%" height="26">Status :<BR>
									<asp:DropDownList id="ddlStatus" width=100% runat=server>
										<asp:ListItem value="1" selected>Active</asp:ListItem>
										<asp:ListItem value="2">Deleted</asp:ListItem>
										<asp:ListItem value="0">All</asp:ListItem>
									</asp:DropDownList>
								</td>
								<td width="20%" height="26" align=left>Last Updated By :<BR><asp:TextBox id=txtLastUpdate width=100% maxlength="128" runat="server"/></td>
								<td width="10%" height="26" valign=bottom align=left><asp:Button id=SearchBtn Text="Search" OnClick=srchBtn_Click runat="server"/></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan=6>					
						<asp:DataGrid id=dgLocCode runat=server
							AutoGenerateColumns=false width=100% 
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

								<asp:HyperLinkColumn HeaderText="Location Code" 
									SortExpression="LocCode" 
									DataNavigateUrlField="LocCode" 
									DataNavigateUrlFormatString="Admin_location_LocDet.aspx?LocCode={0}" 
									DataTextField="LocCode" />
								
								<asp:HyperLinkColumn HeaderText="Description" 
									SortExpression="Description" 
									DataNavigateUrlField="LocCode" 
									DataNavigateUrlFormatString="Admin_location_LocDet.aspx?LocCode={0}" 
									DataTextField="Description" />
								
								<asp:TemplateColumn HeaderText="Last Update" SortExpression="UpdateDate">
									<ItemTemplate>
										<%# objGlobal.GetLongDate(Container.DataItem("UpdateDate")) %>
									</ItemTemplate>
								</asp:TemplateColumn>
									
								<asp:TemplateColumn HeaderText="Status" SortExpression="Status">
									<ItemTemplate>
										<%# objAdmin.mtdGetLocStatus(Container.DataItem("Status")) %>
									</ItemTemplate>
								</asp:TemplateColumn>
									
								<asp:TemplateColumn HeaderText="Updated By" SortExpression="UpdateID">
									<ItemTemplate>
										<%# Container.DataItem("UserName") %>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn ItemStyle-HorizontalAlign=Right HeaderStyle-VerticalAlign=Bottom ItemStyle-VerticalAlign=Bottom>
									<ItemTemplate>
										<asp:Label id=lblLocCode visible=false text='<%# Container.DataItem("LocCode") %>' runat=server/>
										<asp:LinkButton id=lbDelete CommandName=Delete Text=Delete runat=server />
										<asp:Label id=lblStatus Text='<%# Trim(Container.DataItem("Status")) %>' Visible=False runat=server />
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
						</asp:DataGrid><BR>
					</td>
				</tr>
				<tr>
					<td align=right colspan="6">
						<asp:ImageButton id="btnPrev" runat="server" imageurl="../../images/icn_prev.gif" alternatetext="Previous" commandargument="prev" onClick="btnPrevNext_Click" />
						<asp:DropDownList id="lstDropList" runat="server"
							AutoPostBack="True" 
							onSelectedIndexChanged="PagingIndexChanged" />
			         	<asp:Imagebutton id="btnNext" runat="server"  imageurl="../../images/icn_next.gif" alternatetext="Next" commandargument="next" onClick="btnPrevNext_Click" />
					</td>
				</tr>
				<tr>
					<td align="left" ColSpan=6>
						<asp:ImageButton id=NewLocCodeBtn onClick=NewLocCodeBtn_Click imageurl="../../images/butt_new.gif" AlternateText="New Location" runat="server"/>
						<asp:ImageButton id=ibPrint imageurl="../../images/butt_print.gif" AlternateText=Print visible=false runat="server"/>
						<asp:Label id=SortCol Visible=False Runat="server" />
					</td>
				</tr>
			</table>
		</FORM>
	</body>
</html>
