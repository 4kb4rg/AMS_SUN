<%@ Page Language="vb" src="../../../include/Admin_Location_NearestLocList.aspx.vb" Inherits="Admin_Location_NearestLocList" %>
<%@ Register TagPrefix="UserControl" Tagname="MenuAdmin" src="../../menu/menu_admin.ascx"%>
<%@ Register TagPrefix="Preference" Tagname="PrefHdl" src="../../include/preference/preference_handler.ascx"%>
<html>
	<head>
		<title>Product Model List</title>
	</head>
	<Preference:PrefHdl id=PrefHdl runat="server" />
		<body>
		    <form runat="server" ID="Form1">
			<asp:label id="SQLStatement" Visible="False" Runat="server"></asp:label>
			<asp:label id="SortExpression" Visible="False" Runat="server"></asp:label>
			<asp:label id="blnUpdate" Visible="False" Runat="server"></asp:label>
			<asp:Label id=lblErrMessage visible=false Text="Error while initiating component." runat=server />
			<asp:label id=lblCode visible=false text=" Code" runat=server />
			<asp:label id=lblPleaseEnter visible=false text="Please enter " runat=server />
			<asp:label id=lblList visible=false text=" LIST" runat=server />
			<asp:label id="curStatus" Visible="False" Runat="server"></asp:label>
			<asp:label id="sortcol" Visible="False" Runat="server"></asp:label>
			<asp:Label id="ErrorMessage" runat="server" />
			<asp:label id=strState visible=false runat=server />

			<table border="0" cellspacing="1" cellpadding="1" width="100%">
				<tr>
					<td colspan="6"><UserControl:MenuAdmin id=MenuAdmin runat="server" /></td>
				</tr>
				<tr>
					<td class="mt-h" colspan="4" width=60%><asp:label id="lblTitle" runat="server" /> LIST</td>
					<td align="right" colspan="2" width=40%><asp:label id="lblTracker" runat="server"/></td>
				</tr>
				<tr>
					<td colspan=6><hr size="1" noshade></td>
				</tr>
				<tr>
					<td colspan=6 width=100% class="mb-c">
						<table width="100%" cellspacing="0" cellpadding="3" border="0" align="center">
							<tr class="mb-t">
								<td width="10%" height="26" valign=bottom>
									<asp:label id="lblNearestLoc" runat="server" /> :<BR>
									<asp:TextBox id=srchNearestLoc width=100% maxlength="4" runat="server"/>
								</td>
								<td width="30%" height="26" valign=bottom>
									<asp:label id="lblDesc" runat="server" />:<BR>
									<asp:TextBox id=srchDesc width=100%  runat="server"/>
								</td>							
								<td width="10%" height="26" valign=bottom>Status :<BR><asp:DropDownList id="srchStatusList" width=100% runat=server /></td>
								<td width="10%" height="26" valign=bottom>Last Updated By :<BR><asp:TextBox id=srchUpdateBy width=100% maxlength="128" runat="server"/></td>
								<td width="8%" height="26" valign=bottom align=right><asp:Button  Text="Search" OnClick=srchBtn_Click runat="server"/></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<TD colspan = 6 >					
					<asp:DataGrid id="EventData"
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
					
					<asp:TemplateColumn SortExpression="NearLocCode" ItemStyle-Width="20%" HeaderStyle-Width="20%">
						<ItemTemplate>
							<%# Trim(Container.DataItem("NearLocCode")) %>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="txtNearestLoc" 
									Text='<%# Trim(Container.DataItem("NearLocCode")) %>'
									runat="server"/>							
							<asp:label id="lblDupMsg"  Text="<br>Code already exists" Visible = false forecolor=red Runat="server"/>
							<asp:RequiredFieldValidator id=validateNearestLoc display=dynamic runat=server 
									ControlToValidate=txtNearestLoc />
						</EditItemTemplate>
					</asp:TemplateColumn>
	
					<asp:TemplateColumn SortExpression="Description" ItemStyle-Width="20%" HeaderStyle-Width="20%">
						<ItemTemplate>
							<%# Trim(Container.DataItem("Description")) %>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id="txtDescription"  Text='<%# Trim(Container.DataItem("Description")) %>' runat="server"/>	
							<asp:RequiredFieldValidator id=validateDescription display=dynamic runat=server 
									ControlToValidate=txtDescription />														
						</EditItemTemplate>
					</asp:TemplateColumn>								


					<asp:TemplateColumn HeaderText="Last Update" SortExpression="A.UpdateDate">
						<ItemTemplate>
							<%# objGlobal.GetLongDate(Container.DataItem("UpdateDate")) %>
						</ItemTemplate>
						<EditItemTemplate >
							<asp:TextBox id="UpdateDate"  Visible=False size=8 
								Text='<%# objGlobal.GetLongDate(Now()) %>'
								runat="server"/>
							<asp:TextBox id="CreateDate" Visible=False
								Text='<%# Container.DataItem("CreateDate") %>'
								runat="server"/>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Status" SortExpression="A.Status">
						<ItemTemplate>
							<%# objAdmin.mtdGetNearLocStatus(Container.DataItem("Status")) %>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:DropDownList id="StatusList" size=1  Visible=False runat=server />
							<asp:TextBox id="Status" Readonly=TRUE Visible = False
								Text='<%# Container.DataItem("Status")%>'
								runat="server"/>
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Updated By" SortExpression="B.UserName">
						<ItemTemplate>
							<%# Container.DataItem("UserName") %>
						</ItemTemplate>
						<EditItemTemplate >
							<asp:TextBox id="UserName" Readonly=TRUE  Visible=False size=8 
								Text='<%# Session("SS_USERID") %>'
								runat="server"/>
						</EditItemTemplate>
					</asp:TemplateColumn>
					
					<asp:TemplateColumn>
					
						<ItemTemplate>
						<asp:LinkButton id="Edit" CommandName="Edit"   Text="Edit"
							runat="server"/>
						</ItemTemplate>
						
						<EditItemTemplate>
						
						<asp:LinkButton id="Update" CommandName="Update" Text="Save"
							runat="server"/>
						<asp:LinkButton id="Delete" CommandName="Delete" Text="Delete" CausesValidation=False
							runat="server"/>
						<asp:LinkButton id="Cancel" CommandName="Cancel" Text="Cancel" CausesValidation=False
							runat="server"/>
							
						</EditItemTemplate>
					</asp:TemplateColumn>

					</Columns>
					</asp:DataGrid><BR>
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
						<asp:ImageButton id=ibNew OnClick="DEDR_Add" imageurl="../../images/butt_new.gif" AlternateText="New Nearest Location" runat="server"/>
						
					</td>
				</tr>
			</table>
				</FORM>

		</body>
</html>
