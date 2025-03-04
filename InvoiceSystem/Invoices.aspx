<%@ Page Title="Invoices" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Invoices.aspx.cs" Inherits="InvoiceSystem.Invoices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2>Invoice Management</h2>
            <p>
                View, add, edit, and delete invoices.
            </p>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <asp:Button ID="btnAddInvoice" runat="server" Text="Add New Invoice" CssClass="btn btn-primary" OnClick="btnAddInvoice_Click" />
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <asp:GridView ID="GridViewInvoices" runat="server" CssClass="table table-striped table-hover"
                AutoGenerateColumns="False" DataKeyNames="InvoiceID" OnRowCommand="GridViewInvoices_RowCommand">
                <Columns>
                    <asp:BoundField DataField="InvoiceID" HeaderText="Invoice #" ReadOnly="True" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                    <asp:BoundField DataField="InvoiceDate" HeaderText="Date" 
                                    DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:HyperLinkField Text="View" HeaderText="Details"
                        DataNavigateUrlFields="InvoiceID"
                        DataNavigateUrlFormatString="InvoiceDetails.aspx?InvoiceID={0}" />
                    <asp:HyperLinkField Text="Edit" HeaderText="Edit"
                        DataNavigateUrlFields="InvoiceID"
                        DataNavigateUrlFormatString="EditInvoice.aspx?InvoiceID={0}" />
                    <asp:ButtonField ButtonType="Button" CommandName="Delete" Text="Delete" 
                        ControlStyle-CssClass="btn btn-danger btn-sm" 
                        OnClientClick="return confirm('Are you sure you want to delete this invoice?');" />
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-info">
                        No invoices available. Click "Add New Invoice" to create one.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
