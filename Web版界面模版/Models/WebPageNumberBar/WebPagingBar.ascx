<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebPagingBar.ascx.cs" Inherits="WebPageNumberBar.WebPagingBar" %>
<div style="font-size:9pt;">共[<SPAN   
id="lblCP" runat="server">0</SPAN>]条记录 &nbsp;<asp:LinkButton ID="btnFirsh" runat="server" CausesValidation="False" OnClick="btnFirsh_Click"
        Text="首页"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton id="btnUp" onclick="btnUp_Click" Text="上一页" runat="server" CausesValidation="False"></asp:LinkButton> 
&nbsp; &nbsp;<asp:LinkButton id="btnNext" onclick="btnNext_Click" Text="下一页" runat="server" CausesValidation="False" ></asp:LinkButton>&nbsp; &nbsp;<asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" OnClick="btnLast_Click"
        Text="末页"></asp:LinkButton>&nbsp;
        <textarea cols="1" rows="1" runat="server" style="overflow:hidden;font-size:10px; width: 18px; height: 11px; resize: none;" id="txtPage"></textarea>/<asp:Label
     ID="lblPage" runat="server" Text="0"></asp:Label>页
    <div style="display:none">
         <asp:Button ID="btnGo" onclick="btnGo_Click" runat="server" CausesValidation="False" Width="0px"/>
    </div>
    </div>