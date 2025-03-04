using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace InvoiceSystem
{
    public partial class AddCustomer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize form
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                SaveCustomer();
            }
        }

        private void SaveCustomer()
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string city = txtCity.Text.Trim();
            string state = txtState.Text.Trim();
            string zipCode = txtZipCode.Text.Trim();

            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string insertQuery = @"
                    INSERT INTO Customers (Name, Email, Phone, Address, City, State, ZipCode)
                    VALUES (@Name, @Email, @Phone, @Address, @City, @State, @ZipCode);
                    SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@State", state);
                cmd.Parameters.AddWithValue("@ZipCode", zipCode);

                conn.Open();
                int customerID = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();

                // Redirect to Customers page
                Response.Redirect("Customers.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Customers.aspx");
        }
    }
} 