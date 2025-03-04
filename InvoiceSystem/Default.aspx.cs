using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace InvoiceSystem
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardData();
            }
        }

        // Helper method to determine status badge class
        protected string GetStatusBadgeClass(string status)
        {
            switch (status.ToLower())
            {
                case "paid":
                    return "status-badge status-paid";
                case "pending":
                    return "status-badge status-pending";
                case "overdue":
                    return "status-badge status-overdue";
                default:
                    return "status-badge";
            }
        }

        private void LoadDashboardData()
        {
            // Get real data from the database
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                // Update summary statistics
                LabelOpenInvoices.Text = GetOpenInvoicesCount(connection).ToString();
                LabelPaidAmount.Text = GetPaidAmountThisMonth(connection).ToString("C");
                LabelOverdueInvoices.Text = GetOverdueInvoicesCount(connection).ToString();
                LabelTotalCustomers.Text = GetTotalCustomersCount(connection).ToString();

                // Load recent invoices
                GridViewRecentInvoices.DataSource = GetRecentInvoices(connection);
                GridViewRecentInvoices.DataBind();

                // Load recent customers
                GridViewRecentCustomers.DataSource = GetRecentCustomers(connection);
                GridViewRecentCustomers.DataBind();

                // Load payment reminders
                RepeaterReminders.DataSource = GetPaymentReminders(connection);
                RepeaterReminders.DataBind();
            }
        }

        private int GetOpenInvoicesCount(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(
                "SELECT COUNT(*) FROM Invoices WHERE Status = 'Pending'", connection))
            {
                return (int)command.ExecuteScalar();
            }
        }

        private decimal GetPaidAmountThisMonth(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(
                @"SELECT ISNULL(SUM(Amount), 0) FROM Invoices 
                  WHERE Status = 'Paid' 
                  AND MONTH(PaymentDate) = MONTH(GETDATE()) 
                  AND YEAR(PaymentDate) = YEAR(GETDATE())", connection))
            {
                object result = command.ExecuteScalar();
                return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
            }
        }

        private int GetOverdueInvoicesCount(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(
                @"SELECT COUNT(*) FROM Invoices 
                  WHERE Status = 'Pending' 
                  AND DueDate < GETDATE()", connection))
            {
                return (int)command.ExecuteScalar();
            }
        }

        private int GetTotalCustomersCount(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(
                "SELECT COUNT(*) FROM Customers", connection))
            {
                return (int)command.ExecuteScalar();
            }
        }

        private DataTable GetRecentInvoices(SqlConnection connection)
        {
            DataTable dt = new DataTable();
            
            using (SqlCommand command = new SqlCommand(
                @"SELECT TOP 5 I.InvoiceID, C.Name AS CustomerName, I.InvoiceDate, I.Amount, I.Status
                FROM Invoices I
                INNER JOIN Customers C ON I.CustomerID = C.CustomerID
                ORDER BY I.InvoiceDate DESC", connection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            
            return dt;
        }

        private DataTable GetRecentCustomers(SqlConnection connection)
        {
            DataTable dt = new DataTable();
            
            using (SqlCommand command = new SqlCommand(
                @"SELECT TOP 5 CustomerID, Name, Email, City, State
                FROM Customers
                ORDER BY CustomerID DESC", connection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            
            return dt;
        }

        private DataTable GetPaymentReminders(SqlConnection connection)
        {
            DataTable dt = new DataTable();
            
            using (SqlCommand command = new SqlCommand(
                @"SELECT I.InvoiceID, C.Name AS CustomerName, 
                  DATEDIFF(day, GETDATE(), I.DueDate) AS DaysRemaining, I.Amount
                FROM Invoices I
                INNER JOIN Customers C ON I.CustomerID = C.CustomerID
                WHERE I.Status = 'Pending'
                AND I.DueDate > GETDATE()
                AND DATEDIFF(day, GETDATE(), I.DueDate) <= 7
                ORDER BY I.DueDate ASC", connection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            
            return dt;
        }
    }
}