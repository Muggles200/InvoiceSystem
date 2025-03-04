<%@ Page Title="User Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="InvoiceSystem.Admin.UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    
    <div class="row">
        <div class="col-md-12">
            <asp:Panel ID="SuccessPanel" runat="server" Visible="false" CssClass="alert alert-success">
                <asp:Literal ID="SuccessMessage" runat="server"></asp:Literal>
            </asp:Panel>
            <asp:Panel ID="ErrorPanel" runat="server" Visible="false" CssClass="alert alert-danger">
                <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
            </asp:Panel>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <h3>Users</h3>
            <asp:GridView ID="UsersGridView" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-striped table-bordered" DataKeyNames="Id"
                OnRowCommand="UsersGridView_RowCommand">
                <Columns>
                    <asp:BoundField DataField="UserName" HeaderText="Email" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:TemplateField HeaderText="Roles">
                        <ItemTemplate>
                            <asp:Literal ID="RolesLiteral" runat="server" Text='<%# GetUserRoles(Eval("Id").ToString()) %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="MakeAdminButton" runat="server" CommandName="MakeAdmin" 
                                CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-primary"
                                Visible='<%# !IsUserInRole(Eval("Id").ToString(), "Admin") %>'>
                                Make Admin
                            </asp:LinkButton>
                            <asp:LinkButton ID="RemoveAdminButton" runat="server" CommandName="RemoveAdmin" 
                                CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-warning"
                                Visible='<%# IsUserInRole(Eval("Id").ToString(), "Admin") %>'>
                                Remove Admin
                            </asp:LinkButton>
                            <asp:LinkButton ID="DeleteUserButton" runat="server" CommandName="DeleteUser" 
                                CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger"
                                OnClientClick="return confirm('Are you sure you want to delete this user?');">
                                Delete
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content> 