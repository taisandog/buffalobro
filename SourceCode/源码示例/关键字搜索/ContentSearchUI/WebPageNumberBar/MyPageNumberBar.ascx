<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyPageNumberBar.ascx.cs" Inherits="WebPageNumberBar_MyPageNumberBar" %>
<div>
			<asp:LinkButton id="lbFrist" runat="server" OnClick="lbFrist_Click" CausesValidation="False">&lt;&lt;</asp:LinkButton>&nbsp;
		
				<asp:LinkButton id="lb1" runat="server" OnClick="btnPage_Click" CausesValidation="False"></asp:LinkButton>
				<asp:LinkButton id="lb2" runat="server" OnClick="btnPage_Click" CausesValidation="False"></asp:LinkButton>
				<asp:LinkButton id="lb3" runat="server" OnClick="btnPage_Click" CausesValidation="False"></asp:LinkButton>
				<asp:Label id="lb4" runat="server"></asp:Label>
				<asp:LinkButton id="lb5" runat="server" OnClick="btnPage_Click" CausesValidation="False"></asp:LinkButton>
				<asp:LinkButton id="lb6" runat="server" OnClick="btnPage_Click" CausesValidation="False"></asp:LinkButton>
				<asp:LinkButton id="lb7" runat="server" OnClick="btnPage_Click" CausesValidation="False"></asp:LinkButton>
				<asp:LinkButton id="lb8" runat="server" OnClick="btnPage_Click" CausesValidation="False"></asp:LinkButton>
		
			        <textarea rows="1" runat="server" style="overflow:hidden;font-size:10px; width: 18px; height: 11px;" id="txtPage"></textarea>
    &nbsp;<asp:LinkButton id="lbLast" runat="server" OnClick="lbLast_Click" CausesValidation="False">&gt;&gt;</asp:LinkButton>
            页数:(<asp:Label id="labCur" runat="server">1</asp:Label>/<asp:Label id="labTotle" runat="server">20</asp:Label>)
            <div style="display:none">
         <asp:Button ID="btnGo" onclick="btnGo_Click" runat="server" CausesValidation="False" Width="0px"/>
    </div>
</div>