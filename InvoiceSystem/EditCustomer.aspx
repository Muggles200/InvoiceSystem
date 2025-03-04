<%@ Page Title="Edit Customer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditCustomer.aspx.cs" Inherits="InvoiceSystem.EditCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2>Edit Customer #<asp:Label ID="lblCustomerID" runat="server"></asp:Label></h2>
            <hr />
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label for="<%=txtName.ClientID %>">Name:</label>
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                    ErrorMessage="Name is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label for="<%=txtEmail.ClientID %>">Email:</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Invalid email format" CssClass="text-danger" Display="Dynamic"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </div>

            <div class="form-group">
                <label for="<%=txtPhone.ClientID %>">Phone:</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-group">
                <label for="<%=txtAddress.ClientID %>">Address:</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="<%=txtCity.ClientID %>">City:</label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="<%=txtState.ClientID %>">State:</label>
                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" MaxLength="2"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="<%=txtZipCode.ClientID %>">Zip Code:</label>
                <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_Click" CausesValidation="false" />
            </div>
        </div>
    </div>
</asp:Content> 