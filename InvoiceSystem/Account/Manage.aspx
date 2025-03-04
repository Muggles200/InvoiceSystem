<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="InvoiceSystem.Account.Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title"><%: Title %>.</h2>

        <div>
            <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
                <p class="text-success"><%: SuccessMessage %></p>
            </asp:PlaceHolder>
        </div>

        <div class="col-md-12">
            <div class="row">
                <h4>Change your account settings</h4>
                <hr />
                
                <div class="col-md-6">
                    <h5>Profile Information</h5>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserFirstName">First Name</asp:Label>
                        <asp:TextBox runat="server" ID="UserFirstName" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="UserFirstName"
                            CssClass="text-danger" ErrorMessage="First name is required." 
                            ValidationGroup="ProfileUpdate" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserLastName">Last Name</asp:Label>
                        <asp:TextBox runat="server" ID="UserLastName" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="UserLastName"
                            CssClass="text-danger" ErrorMessage="Last name is required." 
                            ValidationGroup="ProfileUpdate" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserCompanyName">Company Name</asp:Label>
                        <asp:TextBox runat="server" ID="UserCompanyName" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="UserEmail">Email</asp:Label>
                        <asp:TextBox runat="server" ID="UserEmail" TextMode="Email" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <asp:Button runat="server" Text="Update Profile" CssClass="btn btn-primary" 
                                    OnClick="UpdateProfile_Click" ValidationGroup="ProfileUpdate" />
                    </div>
                </div>
                
                <div class="col-md-6">
                    <dl class="dl-horizontal">
                        <dt>Password:</dt>
                        <dd>
                            <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Change]" Visible="false" ID="ChangePassword" runat="server" />
                            <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Create]" Visible="false" ID="CreatePassword" runat="server" />
                        </dd>
                        <dt>External Logins:</dt>
                        <dd><%: LoginsCount %>
                            <asp:HyperLink NavigateUrl="/Account/ManageLogins" Text="[Manage]" runat="server" />

                        </dd>
                        <%--
                            Phone Numbers can used as a second factor of verification in a two-factor authentication system.
                            See <a href="https://go.microsoft.com/fwlink/?LinkId=403804">this article</a>
                            for details on setting up this ASP.NET application to support two-factor authentication using SMS.
                            Uncomment the following blocks after you have set up two-factor authentication
                        --%>
                        <%--
                        <dt>Phone Number:</dt>
                        <% if (HasPhoneNumber)
                            { %>
                        <dd>
                            <asp:HyperLink NavigateUrl="/Account/AddPhoneNumber" runat="server" Text="[Add]" />
                        </dd>
                        <% }
                            else
                            { %>
                        <dd>
                            <asp:Label Text="" ID="PhoneNumber" runat="server" />
                            <asp:HyperLink NavigateUrl="/Account/AddPhoneNumber" runat="server" Text="[Change]" /> &nbsp;|&nbsp;
                            <asp:LinkButton Text="[Remove]" OnClick="RemovePhone_Click" runat="server" />
                        </dd>
                        <% } %>
                        --%>

                        <dt>Two-Factor Authentication:</dt>
                        <dd>
                            <p>
                                There are no two-factor authentication providers configured. See <a href="https://go.microsoft.com/fwlink/?LinkId=403804">this article</a>
                                for details on setting up this ASP.NET application to support two-factor authentication.
                            </p>
                            <% if (TwoFactorEnabled)
                                { %> 
                            <%--
                            Enabled
                            <asp:LinkButton Text="[Disable]" runat="server" CommandArgument="false" OnClick="TwoFactorDisable_Click" />
                            --%>
                            <% }
                                else
                                { %> 
                            <%--
                            Disabled
                            <asp:LinkButton Text="[Enable]" CommandArgument="true" OnClick="TwoFactorEnable_Click" runat="server" />
                            --%>
                            <% } %>
                        </dd>
                    </dl>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
