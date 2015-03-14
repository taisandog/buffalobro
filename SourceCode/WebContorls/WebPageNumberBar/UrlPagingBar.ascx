<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UrlPagingBar.ascx.cs" Inherits="UrlPagingBar" %>
<div style="font-size:9pt;">共[<SPAN   
id="lblCP" runat="server">0</SPAN>]条记录 &nbsp;<asp:HyperLink ID="btnFirsh" runat="server">首页</asp:HyperLink>&nbsp;<asp:HyperLink id="btnUp" runat="server">上一页</asp:HyperLink> 
&nbsp; &nbsp;<asp:HyperLink id="btnNext" runat="server">下一页</asp:HyperLink>&nbsp; &nbsp;<asp:HyperLink ID="btnLast" runat="server">末页</asp:HyperLink>&nbsp;
        <textarea rows="1" runat="server" style="overflow:hidden;font-size:10px; width: 18px; height: 11px;" id="txtPage"></textarea>/<asp:Label
     ID="lblPage" runat="server" Text="0"></asp:Label>页
    <div style="display:none">
         <asp:Button ID="btnGo" onclick="btnGo_Click" runat="server" CausesValidation="False" Width="0px"/>
    </div>
    </div>