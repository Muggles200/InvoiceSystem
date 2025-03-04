<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InvoiceSystem._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="bg-light p-4 rounded-3 shadow-sm mb-4">
        <div class="container-fluid py-4">
            <h1 class="display-5 fw-bold"><i class="fas fa-file-invoice-dollar me-3"></i>Invoice Management System</h1>
            <p class="col-md-8 fs-4">Welcome to your comprehensive invoice management dashboard. Track, create, and manage invoices efficiently.</p>
            <div class="mt-3">
                <a href="AddInvoice.aspx" class="btn btn-primary btn-lg me-2">
                    <i class="fas fa-plus-circle me-2"></i>Create New Invoice
                </a>
                <a href="AddCustomer.aspx" class="btn btn-outline-primary btn-lg">
                    <i class="fas fa-user-plus me-2"></i>Add New Customer
                </a>
            </div>
        </div>
    </div>

    <!-- Statistics Row -->
    <div class="row mb-4">
        <div class="col-md-3">
            <div class="card text-white bg-primary mb-3 dashboard-card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <span>Open Invoices</span>
                    <i class="fas fa-file-invoice fa-2x opacity-50"></i>
                </div>
                <div class="card-body">
                    <h3 class="card-title display-4">
                        <asp:Label ID="LabelOpenInvoices" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p class="card-text">Invoices awaiting payment</p>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card text-white bg-success mb-3 dashboard-card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <span>Paid This Month</span>
                    <i class="fas fa-dollar-sign fa-2x opacity-50"></i>
                </div>
                <div class="card-body">
                    <h3 class="card-title display-4">
                        <asp:Label ID="LabelPaidAmount" runat="server" Text="$0.00"></asp:Label>
                    </h3>
                    <p class="card-text">Revenue this month</p>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card text-white bg-danger mb-3 dashboard-card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <span>Overdue</span>
                    <i class="fas fa-exclamation-circle fa-2x opacity-50"></i>
                </div>
                <div class="card-body">
                    <h3 class="card-title display-4">
                        <asp:Label ID="LabelOverdueInvoices" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p class="card-text">Invoices past due date</p>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card text-white bg-info mb-3 dashboard-card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <span>Customers</span>
                    <i class="fas fa-users fa-2x opacity-50"></i>
                </div>
                <div class="card-body">
                    <h3 class="card-title display-4">
                        <asp:Label ID="LabelTotalCustomers" runat="server" Text="0"></asp:Label>
                    </h3>
                    <p class="card-text">Active customer accounts</p>
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-4">
        <!-- Invoice Section -->
        <div class="col-md-8">
            <div class="card shadow-sm mb-4">
                <div class="card-header bg-light">
                    <h3 class="mb-0"><i class="fas fa-history me-2"></i>Recent Invoices</h3>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="GridViewRecentInvoices" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="False" DataKeyNames="InvoiceID" BorderWidth="0" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="InvoiceID" HeaderText="Invoice #" ReadOnly="True" />
                                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                                <asp:BoundField DataField="InvoiceDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='<%# GetStatusBadgeClass(Eval("Status").ToString()) %>'>
                                            <%# Eval("Status") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <a href='<%# "InvoiceDetails.aspx?InvoiceID=" + Eval("InvoiceID") %>' class="btn btn-sm btn-outline-primary">
                                            <i class="fas fa-eye"></i> View
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert alert-info">
                                    <i class="fas fa-info-circle me-2"></i>No recent invoices available.
                                </div>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="table-light" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-footer text-end">
                    <a href="Invoices.aspx" class="btn btn-sm btn-outline-primary">
                        <i class="fas fa-list me-1"></i>View All Invoices
                    </a>
                </div>
            </div>

            <!-- Recent Customers Section -->
            <div class="card shadow-sm">
                <div class="card-header bg-light">
                    <h3 class="mb-0"><i class="fas fa-user-friends me-2"></i>Recent Customers</h3>
                </div>
                <div class="card-body">
                    <div class="table-responsive">
                        <asp:GridView ID="GridViewRecentCustomers" runat="server" CssClass="table table-striped table-hover" 
                            AutoGenerateColumns="False" DataKeyNames="CustomerID" BorderWidth="0" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="CustomerID" HeaderText="ID" ReadOnly="True" />
                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="City" HeaderText="City" />
                                <asp:BoundField DataField="State" HeaderText="State" />
                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>
                                        <div class="btn-group">
                                            <a href='<%# "EditCustomer.aspx?CustomerID=" + Eval("CustomerID") %>' class="btn btn-sm btn-outline-primary">
                                                <i class="fas fa-edit"></i>
                                            </a>
                                            <a href='<%# "AddInvoice.aspx?CustomerID=" + Eval("CustomerID") %>' class="btn btn-sm btn-outline-success">
                                                <i class="fas fa-file-invoice"></i>
                                            </a>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert alert-info">
                                    <i class="fas fa-info-circle me-2"></i>No customers available.
                                </div>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="table-light" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-footer text-end">
                    <a href="Customers.aspx" class="btn btn-sm btn-outline-primary">
                        <i class="fas fa-users me-1"></i>View All Customers
                    </a>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card shadow-sm mb-4">
                <div class="card-header bg-info text-white d-flex justify-content-between align-items-center">
                    <h4 class="mb-0"><i class="fas fa-bolt me-2"></i>Quick Actions</h4>
                </div>
                <div class="card-body">
                    <div class="list-group">
                        <asp:HyperLink ID="HyperLinkNewInvoice" runat="server" CssClass="list-group-item list-group-item-action d-flex justify-content-between align-items-center" NavigateUrl="~/AddInvoice.aspx">
                            <span><i class="fas fa-plus-circle me-2"></i>Create New Invoice</span>
                            <i class="fas fa-chevron-right"></i>
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLinkNewCustomer" runat="server" CssClass="list-group-item list-group-item-action d-flex justify-content-between align-items-center" NavigateUrl="~/AddCustomer.aspx">
                            <span><i class="fas fa-user-plus me-2"></i>Add New Customer</span>
                            <i class="fas fa-chevron-right"></i>
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLinkManageInvoices" runat="server" CssClass="list-group-item list-group-item-action d-flex justify-content-between align-items-center" NavigateUrl="~/Invoices.aspx">
                            <span><i class="fas fa-list me-2"></i>Manage Invoices</span>
                            <i class="fas fa-chevron-right"></i>
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLinkManageCustomers" runat="server" CssClass="list-group-item list-group-item-action d-flex justify-content-between align-items-center" NavigateUrl="~/Customers.aspx">
                            <span><i class="fas fa-users me-2"></i>Manage Customers</span>
                            <i class="fas fa-chevron-right"></i>
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLinkReports" runat="server" CssClass="list-group-item list-group-item-action d-flex justify-content-between align-items-center" NavigateUrl="~/Reports.aspx">
                            <span><i class="fas fa-chart-bar me-2"></i>View Reports</span>
                            <i class="fas fa-chevron-right"></i>
                        </asp:HyperLink>
                    </div>
                </div>
            </div>

            <div class="card shadow-sm">
                <div class="card-header bg-warning d-flex justify-content-between align-items-center">
                    <h4 class="mb-0"><i class="fas fa-bell me-2"></i>Payment Reminders</h4>
                </div>
                <div class="card-body p-0">
                    <asp:Repeater ID="RepeaterReminders" runat="server">
                        <ItemTemplate>
                            <div class="p-3 border-bottom">
                                <div class="d-flex justify-content-between align-items-center">
                                    <h5 class="mb-1">Invoice #<%# Eval("InvoiceID") %></h5>
                                    <span class="badge bg-warning"><%# Eval("DaysRemaining") %> days</span>
                                </div>
                                <p class="mb-1"><%# Eval("CustomerName") %></p>
                                <small class="text-muted">Amount: <%# Eval("Amount", "{0:C}") %></small>
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-success m-3">
                                <i class="fas fa-check-circle me-2"></i>No upcoming payments due soon.
                            </div>
                        </EmptyDataTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
