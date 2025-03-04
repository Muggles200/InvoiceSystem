<%@ Page Title="Add Customer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCustomer.aspx.cs" Inherits="InvoiceSystem.AddCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="page-header">
                <h2><i class="fas fa-user-plus me-2"></i>Add New Customer</h2>
                <p class="lead">Enter customer information below</p>
            </div>
            <hr />
        </div>
    </div>

    <div class="card shadow-sm">
        <div class="card-body">
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group">
                        <label for="<%=txtName.ClientID %>" class="form-label">Name:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fas fa-user"></i></span>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required placeholder="Full Name"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                            ErrorMessage="Name is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label for="<%=txtEmail.ClientID %>" class="form-label">Email:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fas fa-envelope"></i></span>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="example@domain.com"></asp:TextBox>
                        </div>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                    </div>

                    <div class="form-group">
                        <label for="<%=txtPhone.ClientID %>" class="form-label">Phone:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fas fa-phone"></i></span>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="(123) 456-7890"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="form-group">
                        <label for="<%=txtAddress.ClientID %>" class="form-label">Address:</label>
                        <div class="input-group">
                            <span class="input-group-text"><i class="fas fa-map-marker-alt"></i></span>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Street Address"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="<%=txtCity.ClientID %>" class="form-label">City:</label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="<%=txtState.ClientID %>" class="form-label">State:</label>
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" MaxLength="2" placeholder="ST"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <label for="<%=txtZipCode.ClientID %>" class="form-label">Zip Code:</label>
                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" placeholder="12345"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-md-12">
                    <div class="form-group d-flex gap-2">
                        <asp:Button ID="btnSave" runat="server" Text="Save Customer" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content> 