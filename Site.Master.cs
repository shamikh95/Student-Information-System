using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Student_Information_System
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        static String connstr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
                
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["Student"] != null)
            {
                SqlConnection con = new SqlConnection(connstr);
                SqlCommand cmd=new SqlCommand("Select S_Name FROM Students WHERE Enroll_No=@session",con);
                SqlDataReader dr;
                cmd.Parameters.AddWithValue("@session",Session["Student"].ToString());
                con.Open();
                dr=cmd.ExecuteReader();
                if (dr.Read())
                {
                    Label1.Text = dr[0].ToString();
                }
                con.Close();
                lilogin.Visible = false;
                liname.Visible = true;
                lilogout.Visible = true;
                lilogin1.Visible = false;
            }
            else
            {
                lilogout.Visible = false;
                lilogin1.Visible = true;
                lilogin.Visible = true;
                liname.Visible = false;
            }
        }
    }
}
