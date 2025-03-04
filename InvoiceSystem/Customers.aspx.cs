using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InvoiceSystem
{
    public partial class Customers : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomerData();
            }
        }

        private void BindCustomerData()
        {
            // Fetch customer list from DB
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = "SELECT CustomerID, Name, Email, Phone, City, State FROM Customers ORDER BY Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                conn.Close();
                GridViewCustomers.DataSource = dt;
                GridViewCustomers.DataBind();
            }
        }

        protected void GridViewCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int customerID = Convert.ToInt32(GridViewCustomers.DataKeys[index].Value);
                
                // Check if customer has any invoices
                if (HasRelatedInvoices(customerID))
                {
                    // Show error message - can't delete customer with invoices
                    ScriptManager.RegisterStartupScript(this, GetType(), "CustomerDeleteError",
                        "alert('This customer cannot be deleted because they have associated invoices. Please delete the invoices first.');", true);
                }
                else
                {
                    DeleteCustomer(customerID);
                    BindCustomerData();
                }
            }
        }

        private bool HasRelatedInvoices(int customerID)
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = "SELECT COUNT(*) FROM Invoices WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                conn.Close();
                return count > 0;
            }
        }

        private void DeleteCustomer(int customerID)
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string deleteQuery = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddCustomer.aspx");
        }
    }
} 