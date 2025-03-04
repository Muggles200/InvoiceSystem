<%@ Page Title="Access Denied" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="InvoiceSystem.Account.AccessDenied" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <main aria-labelledby="title">
        <h2 id="title" class="text-danger"><%: Title %></h2>
        <div class="alert alert-danger">
            <h4>You do not have permission to access this resource.</h4>
            <p>Please contact your administrator if you believe this is an error.</p>
        </div>
        <div>
            <a href="~/" class="btn btn-primary">Return to Home</a>
        </div>
    </main>
</asp:Content> 