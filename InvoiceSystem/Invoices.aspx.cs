using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InvoiceSystem
{
    public partial class Invoices : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInvoiceData();
            }
        }

        private void BindInvoiceData()
        {
            // Fetch invoice list from DB
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = "SELECT I.InvoiceID, C.Name AS CustomerName, I.InvoiceDate, I.Status " +
                               "FROM Invoices I JOIN Customers C ON I.CustomerID = C.CustomerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                conn.Close();
                GridViewInvoices.DataSource = dt;
                GridViewInvoices.DataBind();
            }
        }

        protected void GridViewInvoices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int invoiceID = Convert.ToInt32(GridViewInvoices.DataKeys[index].Value);
                DeleteInvoice(invoiceID);
                BindInvoiceData();
            }
        }

        protected void btnAddInvoice_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddInvoice.aspx");
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
    }
}
