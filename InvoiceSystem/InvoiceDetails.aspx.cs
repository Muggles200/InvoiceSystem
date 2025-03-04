using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace InvoiceSystem
{
    public partial class InvoiceDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["InvoiceID"] != null)
                {
                    int invoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
                    LoadInvoiceDetails(invoiceID);
                    LoadInvoiceItems(invoiceID);
                }
                else
                {
                    Response.Redirect("Invoices.aspx");
                }
            }
        }

        private void LoadInvoiceDetails(int invoiceID)
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = @"
                    SELECT i.InvoiceID, i.InvoiceDate, i.Status, i.TotalAmount,
                           c.Name AS CustomerName, c.Email, c.Phone, 
                           c.Address, c.City, c.State, c.ZipCode
                    FROM Invoices i
                    JOIN Customers c ON i.CustomerID = c.CustomerID
                    WHERE i.InvoiceID = @InvoiceID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                conn.Close();

                if (dt.Rows.Count > 0)
                {
                    FormViewInvoice.DataSource = dt;
                    FormViewInvoice.DataBind();
                }
            }
        }

        private void LoadInvoiceItems(int invoiceID)
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = @"
                    SELECT ii.ItemID, ii.Description, ii.Quantity, ii.UnitPrice,
                           (ii.Quantity * ii.UnitPrice) AS LineTotal
                    FROM InvoiceItems ii
                    WHERE ii.InvoiceID = @InvoiceID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                conn.Close();

                GridViewInvoiceItems.DataSource = dt;
                GridViewInvoiceItems.DataBind();
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            int invoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            Response.Redirect($"EditInvoice.aspx?InvoiceID={invoiceID}");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int invoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
            DeleteInvoice(invoiceID);
            Response.Redirect("Invoices.aspx");
        }

        private void DeleteInvoice(int invoiceID)
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                // First delete all invoice items
                string deleteItemsQuery = "DELETE FROM InvoiceItems WHERE InvoiceID = @InvoiceID";
                SqlCommand deleteItemsCmd = new SqlCommand(deleteItemsQuery, conn);
                deleteItemsCmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                deleteItemsCmd.ExecuteNonQuery();

                // Then delete the invoice
                string deleteInvoiceQuery = "DELETE FROM Invoices WHERE InvoiceID = @InvoiceID";
                SqlCommand deleteInvoiceCmd = new SqlCommand(deleteInvoiceQuery, conn);
                deleteInvoiceCmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                deleteInvoiceCmd.ExecuteNonQuery();

                conn.Close();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Invoices.aspx");
        }
    }
} 