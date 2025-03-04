<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="InvoiceSystem.Customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="d-flex justify-content-between align-items-center mb-3">
                <div>
                    <h2><i class="fas fa-users me-2"></i>Customer Management</h2>
                    <p class="lead">
                        View, add, edit, and delete customers.
                    </p>
                </div>
                <div>
                    <asp:Button ID="btnAddCustomer" runat="server" Text="Add New Customer" 
                        CssClass="btn btn-primary" OnClick="btnAddCustomer_Click">
                        <i class="fas fa-plus-circle me-2"></i>Add New Customer
                    </asp:Button>
                </div>
            </div>
        </div>
    </div>
    
    <div class="card shadow-sm">
        <div class="card-header bg-light">
            <div class="row">
                <div class="col-md-6">
                    <div class="input-group">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search customers..."></asp:TextBox>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-secondary" />
                    </div>
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <asp:GridView ID="GridViewCustomers" runat="server" CssClass="table table-striped table-hover"
                    AutoGenerateColumns="False" DataKeyNames="CustomerID" OnRowCommand="GridViewCustomers_RowCommand"
                    BorderWidth="0" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="CustomerID" HeaderText="ID" ReadOnly="True" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone" />
                        <asp:BoundField DataField="City" HeaderText="City" />
                        <asp:BoundField DataField="State" HeaderText="State" />
                        <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <div class="btn-group">
                                    <a href='<%# "EditCustomer.aspx?CustomerID=" + Eval("CustomerID") %>' 
                                       class="btn btn-outline-primary btn-sm">
                                        <i class="fas fa-edit"></i> Edit
                                    </a>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" 
                                        CommandArgument='<%# Eval("CustomerID") %>'
                                        CssClass="btn btn-outline-danger btn-sm" 
                                        OnClientClick="return confirm('Are you sure you want to delete this customer?');">
                                        <i class="fas fa-trash-alt"></i> Delete
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="alert alert-info">
                            <i class="fas fa-info-circle me-2"></i>No customers available. Click "Add New Customer" to create one.
                        </div>
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="table-light" />
                </asp:GridView>
            </div>
        </div>
        <div class="card-footer">
            <div class="row">
                <div class="col-md-6">
                    <asp:Literal ID="litCustomerCount" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 