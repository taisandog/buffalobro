<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UrlPagingBar.ascx.cs" Inherits="UrlPagingBar" %>
<div style="font-size:9pt;">��[<SPAN   
id="lblCP" runat="server">0</SPAN>]����¼ &nbsp;<asp:HyperLink ID="btnFirsh" runat="server">��ҳ</asp:HyperLink>&nbsp;<asp:HyperLink id="btnUp" runat="server">��һҳ</asp:HyperLink> 
&nbsp; &nbsp;<asp:HyperLink id="btnNext" runat="server">��һҳ</asp:HyperLink>&nbsp; &nbsp;<asp:HyperLink ID="btnLast" runat="server">ĩҳ</asp:HyperLink>&nbsp;
        <textarea rows="1" runat="server" style="overflow:hidden;font-size:10px; width: 18px; height: 11px;" id="txtPage"></textarea>/<asp:Label
     ID="lblPage" runat="server" Text="0"></asp:Label>ҳ
    <div style="display:none">
         <asp:Button ID="btnGo" onclick="btnGo_Click" runat="server" CausesValidation="False" Width="0px"/>
    </div>
    </div>