<%@ Page Title="Edit Invoice" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditInvoice.aspx.cs" Inherits="InvoiceSystem.EditInvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <h2>Edit Invoice #<asp:Label ID="lblInvoiceID" runat="server"></asp:Label></h2>
            <hr />
        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <h3>Customer Information</h3>
            <div class="form-group">
                <label>Select Customer:</label>
                <asp:DropDownList ID="ddlCustomers" runat="server" CssClass="form-control" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            
            <div class="form-group">
                <label>Email:</label>
                <asp:Label ID="lblEmail" runat="server" CssClass="form-control-static" Text=""></asp:Label>
            </div>
            
            <div class="form-group">
                <label>Phone:</label>
                <asp:Label ID="lblPhone" runat="server" CssClass="form-control-static" Text=""></asp:Label>
            </div>
            
            <div class="form-group">
                <label>Address:</label>
                <asp:Label ID="lblAddress" runat="server" CssClass="form-control-static" Text=""></asp:Label>
            </div>
        </div>
        
        <div class="col-md-6">
            <h3>Invoice Details</h3>
            <div class="form-group">
                <label>Invoice Date:</label>
                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Status:</label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Draft" Value="Draft"></asp:ListItem>
                    <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                    <asp:ListItem Text="Paid" Value="Paid"></asp:ListItem>
                    <asp:ListItem Text="Cancelled" Value="Cancelled"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <h3>Invoice Items</h3>
            <table class="table table-bordered" id="invoiceItems">
                <thead>
                    <tr>
                        <th>Description</th>
                        <th>Quantity</th>
                        <th>Unit Price</th>
                        <th>Line Total</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptInvoiceItems" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hfItemID" runat="server" Value='<%# Eval("ItemID") %>' />
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Text='<%# Eval("Description") %>'></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control quantity" Text='<%# Eval("Quantity") %>' 
                                        TextMode="Number" min="1" step="1" onchange="calculateLineTotal(this)"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control price" Text='<%# Eval("UnitPrice") %>' 
                                        TextMode="Number" min="0.01" step="0.01" onchange="calculateLineTotal(this)"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLineTotal" runat="server" CssClass="form-control line-total" 
                                        Text='<%# String.Format("{0:F2}", Convert.ToDecimal(Eval("Quantity")) * Convert.ToDecimal(Eval("UnitPrice"))) %>' 
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-danger" Text="X" OnClick="RemoveItem_Click" 
                                        CommandArgument='<%# Container.ItemIndex %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="btn btn-info" OnClick="btnAddItem_Click" />
            <div class="form-group pull-right">
                <label>Total Amount:</label>
                <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-12">
            <hr />
            <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_Click" />
        </div>
    </div>
    
    <script type="text/javascript">
        function calculateLineTotal(element) {
            var row = $(element).closest('tr');
            var quantity = parseFloat(row.find('.quantity').val()) || 0;
            var price = parseFloat(row.find('.price').val()) || 0;
            var lineTotal = quantity * price;
            
            row.find('.line-total').val(lineTotal.toFixed(2));
            
            // Calculate grand total
            var grandTotal = 0;
            $('.line-total').each(function() {
                grandTotal += parseFloat($(this).val()) || 0;
            });
            
            $('#<%= txtTotalAmount.ClientID %>').val(grandTotal.toFixed(2));
        }
        
        $(document).ready(function() {
            // Calculate initial totals
            $('.quantity').each(function() {
                calculateLineTotal(this);
            });
        });
    </script>
</asp:Content> 