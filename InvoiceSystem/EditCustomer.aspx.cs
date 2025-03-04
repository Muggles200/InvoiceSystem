using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace InvoiceSystem
{
    public partial class EditCustomer : Page
    {
        private int customerID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["CustomerID"] != null)
            {
                customerID = Convert.ToInt32(Request.QueryString["CustomerID"]);
                lblCustomerID.Text = customerID.ToString();

                if (!IsPostBack)
                {
                    LoadCustomerData();
                }
            }
            else
            {
                Response.Redirect("Customers.aspx");
            }
        }

        private void LoadCustomerData()
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = @"
                    SELECT CustomerID, Name, Email, Phone, Address, City, State, ZipCode
                    FROM Customers 
                    WHERE CustomerID = @CustomerID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    // Set form values
                    txtName.Text = rdr["Name"].ToString();
                    txtEmail.Text = rdr["Email"].ToString();
                    txtPhone.Text = rdr["Phone"].ToString();
                    txtAddress.Text = rdr["Address"].ToString();
                    txtCity.Text = rdr["City"].ToString();
                    txtState.Text = rdr["State"].ToString();
                    txtZipCode.Text = rdr["ZipCode"].ToString();
                }
                else
                {
                    Response.Redirect("Customers.aspx");
                }

                conn.Close();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                UpdateCustomer();
            }
        }

        private void UpdateCustomer()
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
                string updateQuery = @"
                    UPDATE Customers 
                    SET Name = @Name, 
                        Email = @Email, 
                        Phone = @Phone, 
                        Address = @Address, 
                        City = @City, 
                        State = @State, 
                        ZipCode = @ZipCode
                    WHERE CustomerID = @CustomerID";

                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@State", state);
                cmd.Parameters.AddWithValue("@ZipCode", zipCode);

                conn.Open();
                cmd.ExecuteNonQuery();
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