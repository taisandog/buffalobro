<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NFPageBar.ascx.cs" Inherits="WebPageNumberBar.WebPageNumberBar_NFPageBar" %>
<div style="text-align:center;">
    <asp:LinkButton ID="lbFrist" runat="server" OnClick="lbFrist_Click" CausesValidation="False">首页</asp:LinkButton>
    <asp:LinkButton ID="lbPri" runat="server" OnClick="lbPri_Click" CausesValidation="False">上一页</asp:LinkButton>
    &nbsp;
<asp:LinkButton ID="lbNext" runat="server" OnClick="lbNext_Click" CausesValidation="False">下一页</asp:LinkButton>
    <asp:LinkButton ID="lbLast" runat="server" OnClick="lbLast_Click" CausesValidation="False">末页</asp:LinkButton>(<asp:Label ID="labCur"
    runat="server">1</asp:Label>/<asp:Label ID="labTotle" runat="server">20</asp:Label>)
</div>