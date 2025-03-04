<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="InvoiceSystem.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="container py-4">
            <div class="p-5 mb-4 bg-light rounded-3">
                <div class="container-fluid py-2">
                    <h1 class="display-5 fw-bold">About Invoice System</h1>
                    <p class="col-md-12 fs-4">A comprehensive solution for managing your business invoices</p>
                </div>
            </div>

            <div class="row align-items-md-stretch">
                <div class="col-md-6">
                    <div class="h-100 p-5 text-white bg-primary rounded-3">
                        <h2>Features</h2>
                        <ul>
                            <li>Create and manage invoices</li>
                            <li>Track payments and due dates</li>
                            <li>Manage customers and contact information</li>
                            <li>Generate financial reports</li>
                            <li>Email invoices directly to customers</li>
                            <li>User management with role-based security</li>
                        </ul>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="h-100 p-5 bg-light border rounded-3">
                        <h2>Our Mission</h2>
                        <p>Our mission is to provide small to medium-sized businesses with an easy-to-use yet powerful invoice management solution that streamlines the billing process and improves cash flow management.</p>
                        <p>We believe in creating simple, intuitive software that allows businesses to focus on what they do best while we handle the financial tracking.</p>
                    </div>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-md-12">
                    <div class="h-100 p-5 bg-light border rounded-3">
                        <h2>Technical Information</h2>
                        <p>Invoice System is built using ASP.NET Web Forms with a SQL Server backend. The application follows industry best practices for security and performance.</p>
                        <p>Version: 1.0</p>
                        <p>Last Updated: <%= DateTime.Now.ToString("MMMM yyyy") %></p>
                    </div>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
