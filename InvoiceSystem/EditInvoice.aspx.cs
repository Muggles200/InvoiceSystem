using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InvoiceSystem
{
    public partial class EditInvoice : Page
    {
        private int invoiceID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["InvoiceID"] != null)
            {
                invoiceID = Convert.ToInt32(Request.QueryString["InvoiceID"]);
                lblInvoiceID.Text = invoiceID.ToString();

                if (!IsPostBack)
                {
                    LoadCustomers();
                    LoadInvoiceData();
                    LoadInvoiceItems();
                }
            }
            else
            {
                Response.Redirect("Invoices.aspx");
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
            }
        }

        private void LoadInvoiceData()
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = @"
                    SELECT i.InvoiceID, i.CustomerID, i.InvoiceDate, i.Status, i.TotalAmount,
                           c.Name AS CustomerName, c.Email, c.Phone, 
                           c.Address, c.City, c.State, c.ZipCode
                    FROM Invoices i
                    JOIN Customers c ON i.CustomerID = c.CustomerID
                    WHERE i.InvoiceID = @InvoiceID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                
                if (rdr.Read())
                {
                    // Set form values
                    ddlCustomers.SelectedValue = rdr["CustomerID"].ToString();
                    txtInvoiceDate.Text = Convert.ToDateTime(rdr["InvoiceDate"]).ToString("yyyy-MM-dd");
                    ddlStatus.SelectedValue = rdr["Status"].ToString();
                    txtTotalAmount.Text = Convert.ToDecimal(rdr["TotalAmount"]).ToString("F2");
                    
                    // Display customer details
                    lblEmail.Text = rdr["Email"].ToString();
                    lblPhone.Text = rdr["Phone"].ToString();
                    lblAddress.Text = string.Format("{0}, {1}, {2} {3}", 
                        rdr["Address"].ToString(), 
                        rdr["City"].ToString(), 
                        rdr["State"].ToString(), 
                        rdr["ZipCode"].ToString());
                }
                else
                {
                    Response.Redirect("Invoices.aspx");
                }
                
                conn.Close();
            }
        }

        private void LoadInvoiceItems()
        {
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string query = @"
                    SELECT ItemID, InvoiceID, Description, Quantity, UnitPrice
                    FROM InvoiceItems 
                    WHERE InvoiceID = @InvoiceID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(rdr);
                conn.Close();
                
                // Store in ViewState for persistence
                ViewState["InvoiceItems"] = dt;
                
                // Bind to repeater
                rptInvoiceItems.DataSource = dt;
                rptInvoiceItems.DataBind();
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

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddInvoiceItem();
        }

        private void AddInvoiceItem()
        {
            DataTable dt = ViewState["InvoiceItems"] as DataTable;
            
            DataRow dr = dt.NewRow();
            dr["ItemID"] = DBNull.Value;  // New item doesn't have an ID yet
            dr["InvoiceID"] = invoiceID;
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
            
            foreach (RepeaterItem item in rptInvoiceItems.Items)
            {
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                
                int quantity = Convert.ToInt32(txtQuantity.Text);
                decimal unitPrice = Convert.ToDecimal(txtUnitPrice.Text);
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
            List<InvoiceItemData> items = GetItemsFromRepeater();
            
            if (items.Count == 0)
            {
                // Display error - at least one item required
                return false;
            }
            
            return true;
        }

        private List<InvoiceItemData> GetItemsFromRepeater()
        {
            List<InvoiceItemData> items = new List<InvoiceItemData>();
            
            foreach (RepeaterItem item in rptInvoiceItems.Items)
            {
                HiddenField hfItemID = (HiddenField)item.FindControl("hfItemID");
                TextBox txtDescription = (TextBox)item.FindControl("txtDescription");
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                TextBox txtUnitPrice = (TextBox)item.FindControl("txtUnitPrice");
                
                string description = txtDescription.Text;
                int quantity = Convert.ToInt32(txtQuantity.Text);
                decimal unitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                
                if (!string.IsNullOrEmpty(description) && quantity > 0 && unitPrice > 0)
                {
                    InvoiceItemData itemData = new InvoiceItemData
                    {
                        ItemID = string.IsNullOrEmpty(hfItemID.Value) ? (int?)null : Convert.ToInt32(hfItemID.Value),
                        Description = description,
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    };
                    
                    items.Add(itemData);
                }
            }
            
            return items;
        }

        private void SaveInvoice()
        {
            int customerID = Convert.ToInt32(ddlCustomers.SelectedValue);
            DateTime invoiceDate = Convert.ToDateTime(txtInvoiceDate.Text);
            string status = ddlStatus.SelectedValue;
            decimal totalAmount = Convert.ToDecimal(txtTotalAmount.Text);
            
            // Get items from repeater
            List<InvoiceItemData> items = GetItemsFromRepeater();
            
            string constr = ConfigurationManager.ConnectionStrings["InvoiceDBConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                
                try
                {
                    // Update invoice
                    string updateInvoiceQuery = @"
                        UPDATE Invoices 
                        SET CustomerID = @CustomerID, 
                            InvoiceDate = @InvoiceDate, 
                            Status = @Status, 
                            TotalAmount = @TotalAmount
                        WHERE InvoiceID = @InvoiceID";
                    
                    SqlCommand cmd = new SqlCommand(updateInvoiceQuery, conn, transaction);
                    cmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    
                    cmd.ExecuteNonQuery();
                    
                    // First delete all existing invoice items that are not in the updated list
                    List<int> keepItemIds = new List<int>();
                    foreach (InvoiceItemData item in items)
                    {
                        if (item.ItemID.HasValue)
                        {
                            keepItemIds.Add(item.ItemID.Value);
                        }
                    }
                    
                    string deleteItemsQuery = "DELETE FROM InvoiceItems WHERE InvoiceID = @InvoiceID";
                    if (keepItemIds.Count > 0)
                    {
                        deleteItemsQuery += " AND ItemID NOT IN (" + string.Join(",", keepItemIds) + ")";
                    }
                    
                    SqlCommand deleteItemsCmd = new SqlCommand(deleteItemsQuery, conn, transaction);
                    deleteItemsCmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                    deleteItemsCmd.ExecuteNonQuery();
                    
                    // Update or insert invoice items
                    foreach (InvoiceItemData item in items)
                    {
                        if (item.ItemID.HasValue)
                        {
                            // Update existing item
                            string updateItemQuery = @"
                                UPDATE InvoiceItems
                                SET Description = @Description,
                                    Quantity = @Quantity,
                                    UnitPrice = @UnitPrice
                                WHERE ItemID = @ItemID AND InvoiceID = @InvoiceID";
                            
                            SqlCommand updateItemCmd = new SqlCommand(updateItemQuery, conn, transaction);
                            updateItemCmd.Parameters.AddWithValue("@ItemID", item.ItemID.Value);
                            updateItemCmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                            updateItemCmd.Parameters.AddWithValue("@Description", item.Description);
                            updateItemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            updateItemCmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                            
                            updateItemCmd.ExecuteNonQuery();
                        }
                        else
                        {
                            // Insert new item
                            string insertItemQuery = @"
                                INSERT INTO InvoiceItems (InvoiceID, Description, Quantity, UnitPrice)
                                VALUES (@InvoiceID, @Description, @Quantity, @UnitPrice)";
                            
                            SqlCommand insertItemCmd = new SqlCommand(insertItemQuery, conn, transaction);
                            insertItemCmd.Parameters.AddWithValue("@InvoiceID", invoiceID);
                            insertItemCmd.Parameters.AddWithValue("@Description", item.Description);
                            insertItemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            insertItemCmd.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);
                            
                            insertItemCmd.ExecuteNonQuery();
                        }
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
            Response.Redirect($"InvoiceDetails.aspx?InvoiceID={invoiceID}");
        }

        // Helper class for invoice items
        private class InvoiceItemData
        {
            public int? ItemID { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
} 