using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.Admin
{
    public partial class Category : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sad;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;   
            bool isValidToExecute = false;
            int categoryId = Convert.ToInt32(hdnId.Value);
            con=new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Cud", con);
            cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());  
            cmd.Parameters.AddWithValue("@IsAcctive", cbIsActive.Checked);
            if (fucCategoryImage.HasFile)
            {
                if (Utils.IsValidExtension(fucCategoryImage.FileName))
                {
                    Guid obj = Guid.NewGuid(); 
                    fileExtension=Path.GetExtension(fucCategoryImage.FileName); 
                    imagePath="Images/Category/" + obj.ToString() + fileExtension;
                    fucCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + obj.ToString() + fileExtension);
                    cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please select .jpg, .jpeg or .png image";
                    lblMsg.CssClass = "aler alert-danger";
                    isValidToExecute= false; 
                }
            }
            else
            {
                isValidToExecute = true;
            }
            if (isValidToExecute) 
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    actionName = categoryId == 0 ? "inserted" : "update";
                    lblMsg.Visible = true;
                    lblMsg.Text = " Category " + actionName + " successfully ";
                    lblMsg.CssClass = "alert alert-success";
                    //getCategories();
                    clear();
                }
                catch (Exception ex)
                {
                     lblMsg.Visible = true;
                    lblMsg.Text = "Error - " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }    
                finally 
                {
                    con.Close(); 
                }
            }

        }

        private void clear()
        {
           txtName.Text= string.Empty;  
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Add";
        }
    }
}