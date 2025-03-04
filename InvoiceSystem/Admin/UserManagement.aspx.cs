using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using InvoiceSystem.Models;

namespace InvoiceSystem.Admin
{
    public partial class UserManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is an admin
            if (!User.Identity.IsAuthenticated || !IdentityHelper.IsUserInRole(User.Identity.GetUserId(), "Admin"))
            {
                Response.Redirect("~/Account/AccessDenied.aspx");
                return;
            }

            if (!IsPostBack)
            {
                BindUsers();
            }
        }

        private void BindUsers()
        {
            var context = new ApplicationDbContext();
            var users = context.Users.ToList();
            UsersGridView.DataSource = users;
            UsersGridView.DataBind();
        }

        protected string GetUserRoles(string userId)
        {
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roles = userManager.GetRoles(userId);
            return string.Join(", ", roles);
        }

        protected bool IsUserInRole(string userId, string roleName)
        {
            return IdentityHelper.IsUserInRole(userId, roleName);
        }

        protected void UsersGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string userId = e.CommandArgument.ToString();
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            try
            {
                switch (e.CommandName)
                {
                    case "MakeAdmin":
                        userManager.AddToRole(userId, "Admin");
                        ShowSuccessMessage("User has been added to the Admin role.");
                        break;

                    case "RemoveAdmin":
                        userManager.RemoveFromRole(userId, "Admin");
                        ShowSuccessMessage("User has been removed from the Admin role.");
                        break;

                    case "DeleteUser":
                        var user = userManager.FindById(userId);
                        if (user != null)
                        {
                            // Don't allow deleting the current user
                            if (userId == User.Identity.GetUserId())
                            {
                                ShowErrorMessage("You cannot delete your own account.");
                                return;
                            }

                            var result = userManager.Delete(user);
                            if (result.Succeeded)
                            {
                                ShowSuccessMessage("User has been deleted successfully.");
                            }
                            else
                            {
                                ShowErrorMessage("Error deleting user: " + string.Join(", ", result.Errors));
                                return;
                            }
                        }
                        break;
                }

                // Rebind the grid to show changes
                BindUsers();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error: " + ex.Message);
            }
        }

        private void ShowSuccessMessage(string message)
        {
            SuccessPanel.Visible = true;
            ErrorPanel.Visible = false;
            SuccessMessage.Text = message;
        }

        private void ShowErrorMessage(string message)
        {
            SuccessPanel.Visible = false;
            ErrorPanel.Visible = true;
            ErrorMessage.Text = message;
        }
    }
} 