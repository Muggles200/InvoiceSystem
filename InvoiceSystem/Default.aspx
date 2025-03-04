<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InvoiceSystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron bg-light mb-4 p-4 rounded-3">
        <h1>Invoice Management System</h1>
        <p class="lead">Welcome to your comprehensive invoice management dashboard. Track, create, and manage invoices efficiently.</p>
    </div>

    <div class="row mb-4">
        <div class="col-md-8">
            <h2>Dashboard</h2>
            <div class="row">
                <div class="col-md-4">
                    <div class="card text-white bg-primary mb-3">
                        <div class="card-header">Open Invoices</div>
                        <div class="card-body">
                            <h3 class="card-title">
                                <asp:Label ID="LabelOpenInvoices" runat="server" Text="0"></asp:Label>
                            </h3>
                            <p class="card-text">Invoices awaiting payment</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card text-white bg-success mb-3">
                        <div class="card-header">Paid This Month</div>
                        <div class="card-body">
                            <h3 class="card-title">
                                <asp:Label ID="LabelPaidAmount" runat="server" Text="$0.00"></asp:Label>
                            </h3>
                            <p class="card-text">Revenue this month</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card text-white bg-danger mb-3">
                        <div class="card-header">Overdue</div>
                        <div class="card-body">
                            <h3 class="card-title">
                                <asp:Label ID="LabelOverdueInvoices" runat="server" Text="0"></asp:Label>
                            </h3>
                            <p class="card-text">Invoices past due date</p>
                        </div>
                    </div>
                </div>
            </div>

            <h3 class="mt-4">Recent Invoices</h3>
            <asp:GridView ID="GridViewRecentInvoices" runat="server" CssClass="table table-striped table-hover" 
                AutoGenerateColumns="False" DataKeyNames="InvoiceID">
                <Columns>
                    <asp:BoundField DataField="InvoiceID" HeaderText="Invoice #" ReadOnly="True" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                    <asp:BoundField DataField="InvoiceDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:HyperLinkField Text="View" HeaderText="Details"
                        DataNavigateUrlFields="InvoiceID"
                        DataNavigateUrlFormatString="InvoiceDetails.aspx?InvoiceID={0}" />
                </Columns>
            </asp:GridView>
        </div>

        <div class="col-md-4">
            <div class="card mb-4">
                <div class="card-header bg-info text-white">
                    <h4 class="mb-0">Quick Actions</h4>
                </div>
                <div class="card-body">
                    <ul class="list-group">
                        <li class="list-group-item">
                            <asp:HyperLink ID="HyperLinkNewInvoice" runat="server" CssClass="btn btn-primary w-100" NavigateUrl="~/NewInvoice.aspx">
                                <i class="fas fa-plus-circle"></i> Create New Invoice
                            </asp:HyperLink>
                        </li>
                        <li class="list-group-item">
                            <asp:HyperLink ID="HyperLinkManageInvoices" runat="server" CssClass="btn btn-secondary w-100" NavigateUrl="~/Invoices.aspx">
                                <i class="fas fa-list"></i> Manage Invoices
                            </asp:HyperLink>
                        </li>
                        <li class="list-group-item">
                            <asp:HyperLink ID="HyperLinkManageCustomers" runat="server" CssClass="btn btn-secondary w-100" NavigateUrl="~/Customers.aspx">
                                <i class="fas fa-users"></i> Manage Customers
                            </asp:HyperLink>
                        </li>
                        <li class="list-group-item">
                            <asp:HyperLink ID="HyperLinkReports" runat="server" CssClass="btn btn-secondary w-100" NavigateUrl="~/Reports.aspx">
                                <i class="fas fa-chart-bar"></i> View Reports
                            </asp:HyperLink>
                        </li>
                    </ul>
                </div>
            </div>

            <div class="card">
                <div class="card-header bg-warning">
                    <h4 class="mb-0">Payment Reminders</h4>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="RepeaterReminders" runat="server">
                        <ItemTemplate>
                            <div class="alert alert-warning">
                                Invoice #<%# Eval("InvoiceID") %> for <%# Eval("CustomerName") %> 
                                is due in <%# Eval("DaysRemaining") %> days.
                                <br />
                                <small>Amount: <%# Eval("Amount", "{0:C}") %></small>
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-success">No upcoming payments due soon.</div>
                        </EmptyDataTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
