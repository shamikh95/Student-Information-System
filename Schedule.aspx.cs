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
    public partial class Schedule : System.Web.UI.Page
    {

        static String connstr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        SqlConnection con = new SqlConnection(connstr);
        protected void Page_Load(object sender, EventArgs e)
        {
            int count = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            DataTable regForms = new DataTable(), timeTable = new DataTable();
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
                    da.SelectCommand = new SqlCommand("SELECT * FROM TimeTable", con);
                    da.Fill(ds, "TimeTable");
                    regForms = ds.Tables["RegistrationForms"];
                    timeTable = ds.Tables["TimeTable"];
                    TableHeaderRow hrow = new TableHeaderRow();
                    TableHeaderCell hcell = new TableHeaderCell();
                    TableHeaderCell hcell1 = new TableHeaderCell();
                    TableHeaderCell hcell2 = new TableHeaderCell();
                    TableHeaderCell hcell3 = new TableHeaderCell();
                    TableHeaderCell hcell4 = new TableHeaderCell();
                    TableHeaderCell hcell5 = new TableHeaderCell();
                    TableHeaderCell hcell6 = new TableHeaderCell();
                    hcell.Text = "Time";
                    hrow.Cells.Add(hcell);
                    hcell1.Text = "Mon";
                    hrow.Cells.Add(hcell1);
                    hcell2.Text = "Tue";
                    hrow.Cells.Add(hcell2);
                    hcell3.Text = "Wed";
                    hrow.Cells.Add(hcell3);
                    hcell4.Text = "Thu";
                    hrow.Cells.Add(hcell4);
                    hcell5.Text = "Fri";
                    hrow.Cells.Add(hcell5);
                    hcell6.Text = "Sat";
                    hrow.Cells.Add(hcell6);
                    Table1.Rows.Add(hrow);
                    count = 1;



                    foreach (DataRow timeTableRow in timeTable.Rows)
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
                        TableCell cell0 = new TableCell();
                        TableCell cell1 = new TableCell();
                        TableCell cell2 = new TableCell();
                        TableCell cell3 = new TableCell();
                        TableCell cell4 = new TableCell();
                        TableCell cell5 = new TableCell();
                        TableCell cell6 = new TableCell();
                        cell0.Text = timeTableRow[0].ToString();
                        row.Cells.Add(cell0);
                        foreach (DataRow regFormRow in regForms.Rows)
                        {
                            foreach (var item in regFormRow.ItemArray)
                            {
                                if (item.ToString() != "")
                                {
                                    int i = 0;
                                    foreach (var titem in timeTableRow.ItemArray)
                                    {
                                        if (titem.ToString() == item.ToString())
                                        {
                                            switch (i)
                                            {
                                                case 1:
                                                    cell1.Text = item.ToString() + ": L1";
                                                    row.Cells.Add(cell1);
                                                    break;
                                                case 2:
                                                    cell1.Text = item.ToString() + ": L2";
                                                    row.Cells.Add(cell1);
                                                    break;
                                                case 3:
                                                    cell2.Text = item.ToString() + ": L1";
                                                    row.Cells.Add(cell2);
                                                    break;
                                                case 4:
                                                    cell2.Text = item.ToString() + ": L2";
                                                    row.Cells.Add(cell2);
                                                    break;
                                                case 5:
                                                    cell3.Text = item.ToString() + ": L1";
                                                    row.Cells.Add(cell3);
                                                    break;
                                                case 6:
                                                    cell3.Text = item.ToString() + ": L2";
                                                    row.Cells.Add(cell3);
                                                    break;
                                                case 7:
                                                    cell4.Text = item.ToString() + ": L1";
                                                    row.Cells.Add(cell4);
                                                    break;
                                                case 8:
                                                    cell4.Text = item.ToString() + ": L2";
                                                    row.Cells.Add(cell4);
                                                    break;
                                                case 9:
                                                    cell5.Text = item.ToString() + ": L1";
                                                    row.Cells.Add(cell5);
                                                    break;
                                                case 10:
                                                    cell5.Text = item.ToString() + ": L2";
                                                    row.Cells.Add(cell5);
                                                    break;
                                                case 11:
                                                    cell6.Text = item.ToString() + ": L1";
                                                    row.Cells.Add(cell6);
                                                    break;
                                                case 12:
                                                    cell6.Text = item.ToString() + ": L2";
                                                    row.Cells.Add(cell6);
                                                    break;
                                            }
                                        }
                                        i++;
                                    }
                                }
                                row.Cells.Add(cell0);
                                row.Cells.Add(cell1);
                                row.Cells.Add(cell2);
                                row.Cells.Add(cell3);
                                row.Cells.Add(cell4);
                                row.Cells.Add(cell5);
                                row.Cells.Add(cell6);
                                Table1.Rows.Add(row);
                            }
                        }
                    }
                }
            }
        }
    }
}
