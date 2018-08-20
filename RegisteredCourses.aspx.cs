using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;
namespace Student_Information_System
{
    public partial class RegisteredCourses : System.Web.UI.Page
    {

        static String connstr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        SqlConnection con = new SqlConnection(connstr);
        protected void Page_Load(object sender, EventArgs e)
        {
            int count = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable regForms = new DataTable(), course_MapTable = new DataTable();
            if (Session["Student"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                da.SelectCommand = new SqlCommand("SELECT Sub1,Sub2,Sub3,Sub4,Sub5,Sub6 FROM RegistrationForms WHERE EnrollNo=@enroll", con);
                da.SelectCommand.Parameters.AddWithValue("@enroll", Session["Student"].ToString());
                da.Fill(ds, "RegistrationForms");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    Response.Write("<script>alert('You have not Registered Yet');window.location='CourseRegistration.aspx'</script>");
                }
                else
                {
                    da.SelectCommand = new SqlCommand("SELECT * FROM Course_Map", con);
                    da.Fill(ds, "Course_Map");
                    regForms = ds.Tables["RegistrationForms"];
                    course_MapTable = ds.Tables["Course_Map"];
                    TableHeaderRow hrow = new TableHeaderRow();
                    TableHeaderCell hcell = new TableHeaderCell();
                    TableHeaderCell hcell1 = new TableHeaderCell();
                    TableHeaderCell hcell2 = new TableHeaderCell();
                    TableHeaderCell hcell3 = new TableHeaderCell();
                    TableHeaderCell hcell4 = new TableHeaderCell();
                    TableHeaderCell hcell5 = new TableHeaderCell();
                    hcell.Text = "SNo.";
                    hrow.Cells.Add(hcell);
                    hcell1.Text = "SubCode";
                    hrow.Cells.Add(hcell1);
                    hcell2.Text = "SubName";
                    hrow.Cells.Add(hcell2);
                    hcell3.Text = "Credits";
                    hrow.Cells.Add(hcell3);
                    Table1.Rows.Add(hrow);
                    count = 1;
                    foreach (DataRow regFormRow in regForms.Rows)
                    {
                        int SNo = 1;
                        foreach (var item in regFormRow.ItemArray)
                        {
                            if (item.ToString() != "")
                            {
                                TableRow row = new TableRow();
                                if (count++ % 2 == 0)
                                {
                                    row.BackColor = Color.LightBlue;
                                }
                                else
                                {
                                    row.BackColor = Color.White;
                                }
                                row.Height = 35;
                                TableCell cell = new TableCell();
                                TableCell cell1 = new TableCell();
                                TableCell cell2 = new TableCell();
                                TableCell cell3 = new TableCell();
                                cell.Text = SNo++.ToString();
                                row.Cells.Add(cell);
                                cell1.Text = item.ToString();
                                row.Cells.Add(cell1);
                                foreach (DataRow courseMapRow in course_MapTable.Rows)
                                {
                                    if (courseMapRow[1].ToString() == item.ToString())
                                    {
                                        cell2.Text = courseMapRow[2].ToString();
                                        row.Cells.Add(cell2);
                                        cell3.Text = courseMapRow[4].ToString();
                                        row.Cells.Add(cell3);
                                    }
                                }
                                Table1.Rows.Add(row);
                            }
                        }
                    }
                }
            }
        }
    }
}