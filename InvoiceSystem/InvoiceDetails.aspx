<%@ Page Title="Invoice Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InvoiceDetails.aspx.cs" Inherits="InvoiceSystem.InvoiceDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2>Invoice Details</h2>
            <asp:FormView ID="FormViewInvoice" runat="server" DefaultMode="ReadOnly" CssClass="table">
                <ItemTemplate>
                    <div class="row">
                        <div class="col-md-6">
                            <h3>Invoice #<%# Eval("InvoiceID") %></h3>
                            <p><strong>Date:</strong> <%# Eval("InvoiceDate", "{0:yyyy-MM-dd}") %></p>
                            <p><strong>Status:</strong> <%# Eval("Status") %></p>
                            <p><strong>Total Amount:</strong> <%# Eval("TotalAmount", "{0:C}") %></p>
                        </div>
                        <div class="col-md-6">
                            <h3>Customer Information</h3>
                            <p><strong>Name:</strong> <%# Eval("CustomerName") %></p>
                            <p><strong>Email:</strong> <%# Eval("Email") %></p>
                            <p><strong>Phone:</strong> <%# Eval("Phone") %></p>
                            <p><strong>Address:</strong><br />
                                <%# Eval("Address") %><br />
                                <%# Eval("City") %>, <%# Eval("State") %> <%# Eval("ZipCode") %>
                            </p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:FormView>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h3>Invoice Items</h3>
            <asp:GridView ID="GridViewInvoiceItems" runat="server" CssClass="table table-striped table-hover"
                AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="ItemID" HeaderText="Item ID" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="LineTotal" HeaderText="Line Total" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <asp:Button ID="btnEdit" runat="server" Text="Edit Invoice" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete Invoice" CssClass="btn btn-danger" OnClick="btnDelete_Click" 
                        OnClientClick="return confirm('Are you sure you want to delete this invoice?');" />
            <asp:Button ID="btnBack" runat="server" Text="Back to Invoices" CssClass="btn btn-default" OnClick="btnBack_Click" />
        </div>
    </div>
</asp:Content> 