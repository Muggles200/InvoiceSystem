using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

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

        private void LoadDashboardData()
        {
            // In a real application, this data would come from a database
            // For demo purposes, we'll create sample data

            // Update summary statistics
            LabelOpenInvoices.Text = "12";
            LabelPaidAmount.Text = "$4,850.00";
            LabelOverdueInvoices.Text = "3";

            // Load recent invoices
            DataTable recentInvoices = GetRecentInvoices();
            GridViewRecentInvoices.DataSource = recentInvoices;
            GridViewRecentInvoices.DataBind();

            // Load payment reminders
            DataTable reminders = GetPaymentReminders();
            RepeaterReminders.DataSource = reminders;
            RepeaterReminders.DataBind();
        }

        private DataTable GetRecentInvoices()
        {
            // In a real application, this would be a database query
            DataTable dt = new DataTable();
            dt.Columns.Add("InvoiceID", typeof(int));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("InvoiceDate", typeof(DateTime));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("Status", typeof(string));

            // Add sample data
            dt.Rows.Add(1001, "Acme Corporation", DateTime.Now.AddDays(-2), 1250.00m, "Pending");
            dt.Rows.Add(1002, "Globex Industries", DateTime.Now.AddDays(-5), 3450.75m, "Paid");
            dt.Rows.Add(1003, "Wayne Enterprises", DateTime.Now.AddDays(-7), 875.50m, "Overdue");
            dt.Rows.Add(1004, "Stark Industries", DateTime.Now.AddDays(-10), 2340.00m, "Pending");
            dt.Rows.Add(1005, "Umbrella Corporation", DateTime.Now.AddDays(-14), 1780.25m, "Paid");

            return dt;
        }

        private DataTable GetPaymentReminders()
        {
            // In a real application, this would be a database query
            DataTable dt = new DataTable();
            dt.Columns.Add("InvoiceID", typeof(int));
            dt.Columns.Add("CustomerName", typeof(string));
            dt.Columns.Add("DaysRemaining", typeof(int));
            dt.Columns.Add("Amount", typeof(decimal));

            // Add sample data
            dt.Rows.Add(1001, "Acme Corporation", 2, 1250.00m);
            dt.Rows.Add(1004, "Stark Industries", 5, 2340.00m);
            dt.Rows.Add(1007, "Aperture Science", 3, 1850.75m);

            return dt;
        }
    }
}