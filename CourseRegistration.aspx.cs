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
    public partial class CourseRegistration : System.Web.UI.Page
    {
        String course, facno;
        int semester;
        static String connstr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        SqlConnection con = new SqlConnection(connstr);
        SqlCommand cmd;
        String prereq;
        SqlDataReader dr;
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        DataTable resultsTable = new DataTable(), course_MapTable = new DataTable(), timeTable = new DataTable();
        int count;
        protected void Page_Load(object sender, EventArgs e)
        {
            div1.Visible = false;
            if (Session["Student"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM RegistrationForms WHERE EnrollNo=@session", con);
                cmd.Parameters.AddWithValue("session", Session["Student"].ToString());
                con.Open();
                if (cmd.ExecuteScalar() != null)
                {
                    con.Close();
                    Response.Write("<script>alert('Already Registered');window.location='RegisteredCourses.aspx'</script>");
                }
                con.Close();
                cmd = new SqlCommand("SELECT Fac_No,Course,Sem FROM Students WHERE Enroll_No=@session", con);
                cmd.Parameters.AddWithValue("session", Session["Student"].ToString());
                con.Open();
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    facno = dr[0].ToString();
                    course = dr[1].ToString();
                    semester = Convert.ToInt32(dr[2]); ;
                }
                dr.Close();
                con.Close();
                da.SelectCommand = new SqlCommand("SELECT * FROM Results WHERE Enroll=@enroll", con);
                da.SelectCommand.Parameters.AddWithValue("@enroll", Session["Student"].ToString());
                da.Fill(ds, "Results");
                da.SelectCommand = new SqlCommand("SELECT * FROM Course_Map", con);
                da.Fill(ds, "Course_Map");
                da.SelectCommand = new SqlCommand("SELECT * FROM TimeTable", con);
                da.Fill(ds, "TimeTable");
                resultsTable = ds.Tables["Results"];
                course_MapTable = ds.Tables["Course_Map"];
                timeTable = ds.Tables["TimeTable"];
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
                hcell4.Text = "Status";
                hrow.Cells.Add(hcell4);
                var chkbx = new CheckBox
                {
                    ID = "cb1",
                    Enabled = false
                };
                hcell5.Controls.Add(chkbx);
                hrow.Cells.Add(hcell5);
                myTable.Rows.Add(hrow);
                count = 1;
                foreach (DataRow course_MapRow in course_MapTable.Rows)
                {
                    String str = "You need to clear ";
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
                    var chkbx1 = new CheckBox();
                    chkbx1.ID = "cb" + course_MapRow[1].ToString();
                    chkbx1.CheckedChanged += changeFunction;
                    chkbx1.AutoPostBack = true;
                    TableCell cell = new TableCell();
                    TableCell cell1 = new TableCell();
                    TableCell cell2 = new TableCell();
                    TableCell cell3 = new TableCell();
                    TableCell cell4 = new TableCell();
                    TableCell cell5 = new TableCell();
                    cell.Text = course_MapRow[0].ToString();
                    row.Cells.Add(cell);
                    cell1.Text = course_MapRow[1].ToString();
                    row.Cells.Add(cell1);
                    cell2.Text = course_MapRow[2].ToString();
                    row.Cells.Add(cell2);
                    cell3.Text = course_MapRow[4].ToString();
                    row.Cells.Add(cell3);
                    foreach (DataRow resultRow in resultsTable.Rows)
                    {
                        int brk = 0;
                        int rowIndex = 0;
                        foreach (var rowCell in resultRow.ItemArray)
                        {
                            if (rowCell.ToString() == course_MapRow["Subject_Code"].ToString() && Convert.ToInt32(resultRow[rowIndex + 1]) >= 44)
                            {
                                cell4.Text = "Cleared";
                                chkbx1.Enabled = false;
                                brk = 1;
                                break;
                            }
                            else
                            {
                                if (DBNull.Value.Equals(course_MapRow["Prereq"]))
                                {
                                    cell4.Text = string.Format("<img src='images/TickBlack.png' style='height:20px; width:20px' />");
                                    chkbx1.Enabled = true;
                                }
                                else
                                {
                                    int fails = 0;
                                    prereq = course_MapRow["Prereq"].ToString();
                                    if (prereq.Contains(","))
                                    {
                                        String[] prereqArray = prereq.Split(',');
                                        foreach (String prereqArrayElement in prereqArray)
                                        {
                                            foreach (DataRow resultRow4Pre in resultsTable.Rows)//could this be optimized?
                                            {
                                                if (resultRow4Pre["Enroll"].ToString() == Session["Student"].ToString())
                                                {
                                                    int count1 = 0;
                                                    int resultRow4PreIndex = 0;
                                                    foreach (var resultRowElement in resultRow4Pre.ItemArray)
                                                    {
                                                        if (resultRowElement.ToString() == prereqArrayElement)
                                                        {
                                                            if (Convert.ToInt32(resultRow4Pre[resultRow4PreIndex + 1]) < 44)
                                                            {
                                                                str += resultRowElement.ToString() + " & ";
                                                                cell4.Text = string.Format("<img src='images/CrossBlue.png' style='height:20px; width:20px' />");
                                                                //cell4.ToolTip = "You need to clear Prerequistes";
                                                                // row.ToolTip = str;
                                                                chkbx1.Enabled = false;
                                                                fails++;
                                                                count1++;
                                                            }
                                                        }
                                                        resultRow4PreIndex++;
                                                    }
                                                    if (count1 == 0)
                                                    {
                                                        cell4.Text = string.Format("<img src='images/CrossBlue.png' style='height:20px; width:20px' />");
                                                        chkbx1.Enabled = false;
                                                        fails++;
                                                    }
                                                }
                                            }
                                        }
                                        if (fails == 0)
                                        {
                                            cell4.Text = string.Format("<img src='images/TickBlack.png' style='height:20px; width:20px' />");
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataRow dr1 in resultsTable.Rows)
                                        {
                                            int count1 = 0;
                                            int c = 0;
                                            foreach (var item1 in dr1.ItemArray)
                                            {
                                                if (dr1["Enroll"].ToString() == Session["Student"].ToString())
                                                {
                                                    if (item1.ToString() == prereq)
                                                    {
                                                        count1++;
                                                        if (Convert.ToInt32(dr1[c + 1]) < 44)
                                                        {
                                                            cell4.Text = string.Format("<img src='images/CrossBlue.png' style='height:20px; width:20px' />");
                                                            row.ToolTip = "You need to clear " + prereq;
                                                            chkbx1.Enabled = false;
                                                            fails++;
                                                        }
                                                    }
                                                    c++;
                                                }
                                            }
                                            if (count1 == 0)
                                            {
                                                cell4.Text = string.Format("<img src='images/CrossBlue.png' style='height:20px; width:20px' />");
                                                row.ToolTip = "You need to clear " + prereq;
                                                chkbx1.Enabled = false;
                                                fails++;
                                            }
                                        }
                                        if (fails == 0)
                                        {
                                            cell4.Text = string.Format("<img src='images/TickBlack.png' style='height:20px; width:20px' />");
                                        }
                                    }
                                }
                            }
                            rowIndex++;
                        }
                        if (brk == 1)
                        {
                            break;
                        }
                    }
                    row.Cells.Add(cell4);
                    cell5.Controls.Add(chkbx1);
                    row.Cells.Add(cell5);
                    myTable.Rows.Add(row);
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            int sum = 0;
            List<String> subs = new List<String>();
            try
            {
                foreach (TableRow tr in myTable.Rows)
                {
                    TableCell tc = tr.Cells[5];
                    CheckBox cb = tc.Controls[0] as CheckBox;
                    if (cb.Checked)
                    {
                        sum += Convert.ToInt32(tr.Cells[3].Text);
                        subs.Add(tr.Cells[1].Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            if (sum < 15)
            {
                Response.Write("<script>alert('You need to apply for 15 Credits atleast.');</script>");
            }
            else if (sum > 40)
            {
                Response.Write("<script>alert('You cannot apply for more than 40 Credits.');</script>");
            }
            else
            {
                cmd = new SqlCommand("INSERT INTO RegistrationForms (EnrollNo,FacNo,Course,Sem,Sub1,Sub2,Sub3,Sub4,Sub5,Sub6) values(@enroll,@fac,@course,@sem,@sub1,@sub2,@sub3,@sub4,@sub5,@sub6)", con);
                cmd.Parameters.AddWithValue("enroll", Session["Student"].ToString());
                cmd.Parameters.AddWithValue("fac", facno);
                cmd.Parameters.AddWithValue("course", course);
                cmd.Parameters.AddWithValue("sem", semester);
                int i = 1;
                foreach (String elem in subs)
                {
                    cmd.Parameters.AddWithValue("sub" + i, subs[i - 1].ToString());
                    i++;
                }
                while (i <= 6)
                {
                    cmd.Parameters.AddWithValue("sub" + i++, "");
                }
                con.Open();
                cmd.ExecuteScalar();
                con.Close();
                Response.Write("<script>alert('Registered Successfully');window.location='RegisteredCourses.aspx'</script>");
            }
        }
        protected void changeFunction(object sender, EventArgs e)
        {
            CheckBox cbsender = (CheckBox)sender;
            cbsender.Focus();
            var senderSub = "a";
            int senderCredits = 0;
            foreach (TableRow tr in myTable.Rows)
            {
                if (tr.Cells[5].Controls[0] == cbsender)
                {
                    senderSub = tr.Cells[1].Text;
                    senderCredits = Convert.ToInt32(tr.Cells[3].Text);
                }
            }
            int sum = 0;
            Boolean check = false;
            try
            {
                foreach (TableRow tr in myTable.Rows)
                {
                    TableCell tc = tr.Cells[5];
                    CheckBox cb = tc.Controls[0] as CheckBox;
                    if (cb.Checked)
                    {
                        foreach (DataRow dtr in timeTable.Rows)
                        {
                            int i = 0;
                            foreach (var item1 in dtr.ItemArray)
                            {
                                if (item1.ToString() == senderSub.ToString())
                                {
                                    if (i % 2 == 0)
                                    {
                                        if (dtr[i - 1].ToString() == tr.Cells[1].Text && cb != cbsender)
                                        {
                                            String str = "Classes of " + tr.Cells[1].Text + " & " + senderSub.ToString() + " Clash";
                                            Response.Write("<script>alert('" + str + "');</script>");
                                            cbsender.Checked = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (dtr[i + 1].ToString() == tr.Cells[1].Text && cb != cbsender)
                                        {
                                            String str = "Classes of " + tr.Cells[1].Text + " & " + senderSub.ToString() + " Clash";
                                            Response.Write("<script>alert('" + str + "');</script>");
                                            cbsender.Checked = false;
                                            break;
                                        }
                                    }
                                }
                                i++;
                            }
                        }
                        check = true;
                        sum += Convert.ToInt32(tr.Cells[3].Text);
                        if (sum > 40)
                        {
                            Response.Write("<script>alert('Total selected credits are more than Credits Allowed');</script>");
                            cbsender.Checked = false;
                            sum -= senderCredits;
                        }
                    }
                    if (check)
                    {
                        div1.Visible = true;
                    }
                    else
                    {
                        div1.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            Label1.Text = sum.ToString();
        }
    }
}