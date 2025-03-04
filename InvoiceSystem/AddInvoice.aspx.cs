using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InvoiceSystem
{
    public partial class AddInvoice : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCustomers();
                // Initialize with today's date
                txtInvoiceDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                
                // Create initial empty item
                InitializeInvoiceItems();
            }
        }

        private void LoadCustomers()
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = "SELECT CustomerID, Name FROM Customers ORDER BY Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                
                ddlCustomers.DataSource = rdr;
                ddlCustomers.DataTextField = "Name";
                ddlCustomers.DataValueField = "CustomerID";
                ddlCustomers.DataBind();
                
                conn.Close();
                
                // Add placeholder item
                ddlCustomers.Items.Insert(0, new ListItem("-- Select Customer --", ""));
            }
        }

        protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlCustomers.SelectedValue))
            {
                int customerID = Convert.ToInt32(ddlCustomers.SelectedValue);
                LoadCustomerDetails(customerID);
            }
            else
            {
                // Clear customer details if no customer is selected
                lblEmail.Text = string.Empty;
                lblPhone.Text = string.Empty;
                lblAddress.Text = string.Empty;
            }
        }

        private void LoadCustomerDetails(int customerID)
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = "SELECT Email, Phone, Address, City, State, ZipCode FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                
                if (rdr.Read())
                {
                    lblEmail.Text = rdr["Email"].ToString();
                    lblPhone.Text = rdr["Phone"].ToString();
                    lblAddress.Text = string.Format("{0}, {1}, {2} {3}", 
                        rdr["Address"].ToString(), 
                        rdr["City"].ToString(), 
                        rdr["State"].ToString(), 
                        rdr["ZipCode"].ToString());
                }
                
                conn.Close();
            }
        }

        private void InitializeInvoiceItems()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            
            // Add an empty row
            DataRow dr = dt.NewRow();
            dr["Description"] = string.Empty;
            dr["Quantity"] = 1;
            dr["UnitPrice"] = 0.00;
            dt.Rows.Add(dr);
            
            // Store in ViewState for persistence
            ViewState["InvoiceItems"] = dt;
            
            // Bind to repeater
            rptInvoiceItems.DataSource = dt;
            rptInvoiceItems.DataBind();
            
            // Set initial total
            CalculateTotalAmount();
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddInvoiceItem();
        }

        private void AddInvoiceItem()
        {
            DataTable dt = ViewState["InvoiceItems"] as DataTable;
            
            DataRow dr = dt.NewRow();
            dr["Description"] = string.Empty;
            dr["Quantity"] = 1;
            dr["UnitPrice"] = 0.00;
            dt.Rows.Add(dr);
            
            ViewState["InvoiceItems"] = dt;
            rptInvoiceItems.DataSource = dt;
            rptInvoiceItems.DataBind();
        }

        protected void RemoveItem_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.CommandArgument);
            
            DataTable dt = ViewState["InvoiceItems"] as DataTable;
            
            if (dt.Rows.Count > 1)
            {
                dt.Rows.RemoveAt(index);
                ViewState["InvoiceItems"] = dt;
                rptInvoiceItems.DataSource = dt;
                rptInvoiceItems.DataBind();
                
                CalculateTotalAmount();
            }
        }

        private void CalculateTotalAmount()
        {
            decimal total = 0;
            DataTable dt = ViewState["InvoiceItems"] as DataTable;
            
            foreach (DataRow row in dt.Rows)
            {
                int quantity = Convert.ToInt32(row["Quantity"]);
                decimal unitPrice = Convert.ToDecimal(row["UnitPrice"]);
                total += quantity * unitPrice;
            }
            
            txtTotalAmount.Text = total.ToString("F2");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid && ValidateForm())
            {
                SaveInvoice();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrEmpty(ddlCustomers.SelectedValue))
            {
                // Display error - customer required
                return false;
            }
            
            // Get items from repeater
            DataTable dt = GetItemsFromRepeater();
            
            if (dt.Rows.Count == 0)
            {
                // Display error - at least one item required
                return false;
            }
            
            return true;
        }

        private DataTable GetItemsFromRepeater()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("UnitPrice", typeof(decimal));
            
            foreach (RepeaterItem item in rptInvoiceItems.Items)
            {
                TextBox txtDescription = (TextBox)item.FindControl("txtDescription");
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                
                string description = txtDescription.Text;
                int quantity = Convert.ToInt32(txtQuantity.Text);
                decimal unitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                
                if (!string.IsNullOrEmpty(description) && quantity > 0 && unitPrice > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr["Description"] = description;
                    dr["Quantity"] = quantity;
                    dr["UnitPrice"] = unitPrice;
                    dt.Rows.Add(dr);
                }
            }
            
            return dt;
        }

        private void SaveInvoice()
        {
            int customerID = Convert.ToInt32(ddlCustomers.SelectedValue);
            DateTime invoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
            string status = ddlStatus.SelectedValue;
            decimal totalAmount = Convert.ToDecimal(txtTotalAmount.Text);
            
            // Get items from repeater
            DataTable items = GetItemsFromRepeater();
            
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                
                try
                {
                    // Insert invoice
                    string insertInvoiceQuery = @"
                        INSERT INTO Invoices (CustomerID, InvoiceDate, Status, TotalAmount)
                        VALUES (@CustomerID, @InvoiceDate, @Status, @TotalAmount);
                        SELECT SCOPE_IDENTITY();";
                    
                    SqlCommand cmd = new SqlCommand(insertInvoiceQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    
                    // Get the new invoice ID
                    int invoiceID = Convert.ToInt32(cmd.ExecuteScalar());
                    
                    // Insert invoice items
                    foreach (DataRow row in items.Rows)
                    {
                        string insertItemQuery = @"
                            INSERT INTO InvoiceItems (InvoiceID, Description, Quantity, UnitPrice)
                            VALUES (@InvoiceID, @Description, @Quantity, @UnitPrice)";
                        
                        SqlCommand itemCmd = new SqlCommand(insertItemQuery, conn, transaction);
                        itemCmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                        itemCmd.Parameters.AddWithValue("@Description", row["Description"]);
                        itemCmd.Parameters.AddWithValue("@Quantity", row["Quantity"]);
                        itemCmd.Parameters.AddWithValue("@UnitPrice", row["UnitPrice"]);
                        
                        itemCmd.ExecuteNonQuery();
                    }
                    
                    // Commit transaction
                    transaction.Commit();
                    
                    // Redirect to invoice details
                    Response.Redirect($"InvoiceDetails.aspx?InvoiceID={invoiceID}");
                }
                catch (Exception ex)
                {
                    // Roll back transaction if there's an error
                    transaction.Rollback();
                    // Display error message
                    throw;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Invoices.aspx");
        }
    }
} 