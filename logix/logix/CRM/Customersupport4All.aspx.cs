﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;


namespace logix.CRM
{
    public partial class Customersupport4All : System.Web.UI.Page
    {
        DataAccess.Reportasp objRpt = new DataAccess.Reportasp();
        DataAccess.LogDetails ObjLog = new DataAccess.LogDetails();
        DataTable dt;
        DataTable dtevent;
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "SpanTagMoveInputBottom();MuiTextField();", true);
            if (!IsPostBack)
            {
                string taskId = Request.QueryString["taskid"];
                loaddropdown();

                binddata();
                txt_container.SelectedItem.Text = "";
                ddlevent.SelectedItem.Text = "";
                txt_booking.SelectedItem.Text = "";
                txt_customer.SelectedItem.Text = "";
                if (Session["StrTranType"].ToString() != "")
                {
                    ddl_product.SelectedValue = Session["StrTranType"].ToString();
                }
                //if (Request.QueryString.ToString().Contains("event"))
                //{
                //    ddlevent.SelectedItem.Text = Request.QueryString["event"].ToString();
                //    ddlevent_SelectedIndexChanged(sender, e);
                //}
                if (taskId!=null)
                {
                    ddlevent.SelectedItem.Text = taskId.ToString();
                    ddlevent_SelectedIndexChanged(sender, e);
                }

            }


        }


       protected void binddata ()
        {
            dt = objRpt.GetCustomersupportall(Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString(), Convert.ToInt32(Session["LoginEmpId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                grd.DataSource = dt;
                grd.DataBind();
              //  dtevent = objRpt.Getmastereventdetails("0");

                for (int j = 0; j < grd.Rows.Count; j++)
                {
                    var ddl1 = (DropDownList)grd.Rows[j].FindControl("ddl_NextEvent");
                    dtevent = objRpt.Getmastereventdetails(dt.Rows[j]["nextevent"].ToString());
                    ddl1.Items.Add("");
                    ddl1.DataSource = dtevent;
                    ddl1.DataTextField = "Events";

                    ddl1.DataBind();
                    ddl1.SelectedItem.Text = dt.Rows[j]["nextevent"].ToString();
                    TextBox lnkbtn = (TextBox)(grd.Rows[j].FindControl("occuredon"));
                    // lnkbtn.Text = Utility.fn_ConvertDate(dt.Rows[j]["occuredon"].ToString());
                }
                
            }
            dtevent = objRpt.Getmastereventdetails("0");
            ddlevent.Items.Clear();
            ddlevent.Items.Add("");
            ddlevent.DataSource = dtevent;
            ddlevent.DataTextField = "Events";
            ddlevent.DataBind();
        }
       [WebMethod]
       public static List<string> GetBookingPending(string prefix)
       {
           List<string> List_Result = new List<string>();
           string StrTranType = "";
           if (HttpContext.Current.Session["StrTranType"] != null)
           {
               StrTranType = HttpContext.Current.Session["StrTranType"].ToString();
           }

           DataAccess.Marketing.Booking bookingobj = new DataAccess.Marketing.Booking();
           DataTable dtbooking = new DataTable();
           dtbooking = bookingobj.GetBookingPending(StrTranType, Convert.ToInt32(HttpContext.Current.Session["LoginBranchid"]), prefix.ToUpper());
           List_Result = Utility.Fn_TableToList(dtbooking, "bookingno", "bookno");
           return List_Result;
       }
        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                 
                    e.Row.Attributes.Add("onmouseover", "this.originalcolor=this.style.backgroundColor;" + " this.style.backgroundColor='#FDCB0A';");
                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalcolor;");

                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(grd, "Select$" + e.Row.RowIndex);
                    e.Row.Attributes["style"] = "cursor:pointer";
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.WebControls.Button), "logix", "alertify.alert('" + message + "');", true);
            }
        }

        protected void grd_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grd.Rows)
            {
                DropDownList Txt = (DropDownList)grd.Rows[row.RowIndex].FindControl("LastEvent");
                if(Txt.SelectedValue!="0")
                {
                    DropDownList nextTxt = (DropDownList)grd.Rows[row.RowIndex].FindControl("LastEvent");
                    int next=Convert.ToInt32(Txt.SelectedValue) + 1;
                    nextTxt.SelectedValue = next.ToString();
                }
            }
        }
        private void Grd_TDS_CellContentClick(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > 0)
            {
                int rowindex = e.Row.RowIndex;
                GridViewRow row = this.grd.Rows[rowindex];
            }

        }
        protected void ddl_LastEvent_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = 0;
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            index = row.RowIndex;
            DropDownList Txt = (DropDownList)grd.Rows[row.RowIndex].FindControl("LastEvent");
            DropDownList Txt1 = (DropDownList)grd.Rows[row.RowIndex].FindControl("NextEvent");
            
        }

        protected void ddl_NextEvent_SelectedIndexChanged(object sender, EventArgs e)
        {

           
            int index = 0;
            GridViewRow row = ((GridViewRow)((DropDownList)sender).NamingContainer);
            index = row.RowIndex;
            DropDownList Txt = (DropDownList)grd.Rows[row.RowIndex].FindControl("NextEvent");
           



        }
        private bool IsValidEmailId(string InputEmail)
        {
            //Regex To validate Email Address
            Regex regex = new Regex(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$");

            Match match = regex.Match(InputEmail);
            if (match.Success)
                return true;
            else
                return false;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Boolean check =false;
            for (int i = 0; i < grd.Rows.Count; i++)
            {


                // DropDownList Txt = (DropDownList)grd.Rows[i].FindControl("ddl_LastEvent");
                DropDownList Txt1 = (DropDownList)grd.Rows[i].FindControl("ddl_NextEvent");
                TextBox date = (TextBox)grd.Rows[i].FindControl("updateddt");


                if (date.Text != "")
                {

                    objRpt.InsOEeventdetails(Convert.ToInt32(grd.Rows[i].Cells[4].Text), grd.Rows[i].Cells[1].Text, grd.Rows[i].Cells[3].Text, Txt1.SelectedValue,
                        Convert.ToDateTime(Utility.fn_ConvertDate(date.Text.ToString())), Convert.ToInt32(Session["LoginEmpId"].ToString()), Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString());
                    check = true;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "logix", "alertify.alert(' " + Txt1.SelectedValue + " updated for the booking "+grd.Rows[i].Cells[2].Text+"');", true);
                  //  sendMail(Convert.ToInt32(grd.Rows[i].Cells[4].Text), grd.Rows[i].Cells[1].Text, Txt1.SelectedValue);
                }


            }
            if (check == false)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "logix", "alertify.alert('Kindly Select the date...');", true);
                return;
            }

             dt = objRpt.GetCustomersupportall(Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString(), Convert.ToInt32(Session["LoginEmpId"].ToString()));
             if (dt.Rows.Count > 0)
             {
                 grd.DataSource = dt;
                 grd.DataBind();
              

                 for (int j = 0; j < grd.Rows.Count; j++)
                 {
                     var ddl1 = (DropDownList)grd.Rows[j].FindControl("ddl_NextEvent");

                     dtevent = objRpt.Getmastereventdetails( dt.Rows[j]["nextevent"].ToString());
                     ddl1.DataSource = dtevent;
                     ddl1.DataTextField = "Events";

                     ddl1.DataBind();
                     ddl1.SelectedItem.Text = dt.Rows[j]["nextevent"].ToString();

                 }
             }
        }

        protected void grd_PreRender(object sender, EventArgs e)
        {

            if (grd.Rows.Count > 0)
            {
                grd.UseAccessibleHeader = true;
                grd.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            binddata();
            loaddropdown();
            txt_container.SelectedItem.Text = "";
            txt_job.Text = "";
            txt_booking.SelectedItem.Text = "";
            txt_customer.SelectedItem.Text = "";
            ddlevent.SelectedItem.Text = "";
            
            ddl_product.SelectedValue = Session["StrTranType"].ToString();
        }

        [WebMethod(EnableSession = true)]
        public static List<string> GetCustomer(string prefix)
        {
            List<string> list_result = new List<string>();
            DataTable dt = new DataTable();
            DataAccess.Masters.MasterCustomer obj_MasterCustomer = new DataAccess.Masters.MasterCustomer();
            // dt = obj_MasterCustomer.GetCustomerName(prefix);GetLikeCustomerAll
            //////dt = obj_MasterCustomer.SPLikeCustomerAll4Customer(prefix.ToUpper());

            //kalai

             
                dt = obj_MasterCustomer.SPLikeCustomerAll4Customernewtype(prefix.ToUpper(), 'C');
            
            //  list_result = Utility.Fn_TableToList(dt, "customername", "customerid");
            list_result = Utility.Fn_TableToList_Cust1(dt, "customer", "customerid", "address");
            return list_result;
        }
        protected void loaddropdown()
        {
            DataSet ds = objRpt.GEtdropdownvalues4CS(Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString(), 0);
           
                DataTable dt1 = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                DataTable dt3 = ds.Tables[2];
                //  dtevent = objRpt.Getmastereventdetails("0");
                if (dt2.Rows.Count > 0)
                {
                    for (int j = 0; j <= dt2.Rows.Count + 1; j++)
                {
                    txt_booking.Items.Clear();
                    txt_booking.Items.Add("");
                    txt_booking.DataSource = dt2;
                    txt_booking.DataTextField = "shiprefno";

                    txt_booking.DataBind();

                }
                }
                if (dt3.Rows.Count > 0)
                {
                    for (int k = 0; k <= dt3.Rows.Count + 1; k++)
                {
                    txt_container.Items.Clear();
                    txt_container.Items.Add("");
                    txt_container.DataSource = dt3;
                    txt_container.DataTextField = "containerno";

                    txt_container.DataBind();
                }
                }
                if (dt1.Rows.Count > 0)
                {
                    for (int l = 0; l <= dt1.Rows.Count + 1; l++)
                {
                    txt_customer.Items.Clear();
                    txt_customer.Items.Add("");
                    txt_customer.DataSource = dt1;
                    txt_customer.DataTextField = "customer";

                    txt_customer.DataBind();
                }
                }

                //txt_customer.SelectedValue = "0";
                //txt_container.SelectedValue = "0";
                //txt_booking.SelectedValue = "0";
            
        }

        [WebMethod]
        public static List<string> GetLikeCustomer(string prefix)
        {
            List<string> List_Result = new List<string>();
            DataAccess.Masters.MasterCustomer Cusobj = new DataAccess.Masters.MasterCustomer();
            DataTable dtCarrier = new DataTable();
            dtCarrier = Cusobj.GetLikeCustomer(prefix.ToUpper(), "C");

            List_Result = Utility.Fn_TableToList(dtCarrier, "customer", "customerid", "customername", "address");
            return List_Result;
        }

        protected void text_TextChanged(object sender, EventArgs e)
        {
            binddata();
        }

        protected void txt_job_TextChanged(object sender, EventArgs e)
        {
            dt = objRpt.GetCustomersupportall(Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString(), Convert.ToInt32(Session["LoginEmpId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                //grd.DataSource = dt;
                //grd.DataBind();
                DataView obj_dtview = new DataView(dt);
                if (txt_booking.SelectedItem.Text != "" && txt_container.SelectedItem.Text != "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and jobno =" + txt_job.Text + " and blno ='" + txt_container.SelectedItem.Text+"'";
                }
                else if (txt_booking.SelectedItem.Text != "" && txt_container.SelectedItem.Text == "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and jobno =" + txt_job.Text;

                }
                else if (txt_booking.SelectedItem.Text == "" && txt_container.SelectedItem.Text == "")
                {
                    obj_dtview.RowFilter = "jobno = (" + txt_job.Text + ")";

                }
                
                dt = obj_dtview.ToTable();
                grd.DataSource = dt;
                grd.DataBind();
               // dtevent = objRpt.Getmastereventdetails();

                for (int j = 0; j < grd.Rows.Count; j++)
                {
                    var ddl1 = (DropDownList)grd.Rows[j].FindControl("ddl_NextEvent");
                    dtevent = objRpt.Getmastereventdetails(dt.Rows[j]["nextevent"].ToString());
                    ddl1.Items.Add("");
                    ddl1.DataSource = dtevent;
                    ddl1.DataTextField = "Events";

                    ddl1.DataBind();
                    ddl1.SelectedItem.Text = dt.Rows[j]["nextevent"].ToString();
                    TextBox lnkbtn = (TextBox)(grd.Rows[j].FindControl("occuredon"));
                    // lnkbtn.Text = Utility.fn_ConvertDate(dt.Rows[j]["occuredon"].ToString());
                }
                dtevent = objRpt.Getmastereventdetails("0");
                ddlevent.Items.Add("");
                ddlevent.DataSource = dtevent;
                ddlevent.DataTextField = "Events";
            }
        }

        protected void txt_booking_TextChanged(object sender, EventArgs e)
        {
            dt = objRpt.GetCustomersupportall(Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString(), Convert.ToInt32(Session["LoginEmpId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                //grd.DataSource = dt;
                //grd.DataBind();
                DataView obj_dtview = new DataView(dt);
                //if (txt_job.Text != "" && txt_container.SelectedItem.Text != "")
                //{
                //    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and jobno =" + txt_job.Text + " and containerno ='" + txt_container.SelectedItem.Text+"'";
                //}
                //else if ( txt_job.Text != "" && txt_container.SelectedItem.Text == "")
                //{
                //    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and jobno =" + txt_job.Text;                  
                   
                //}


                if (txt_job.Text == "" && txt_container.SelectedItem.Text == "" && txt_customer.Text == "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "'";
                }
                else if (txt_job.Text == "" && txt_container.SelectedItem.Text == "" && txt_customer.Text != "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and customer ='" + txt_customer.SelectedItem.Text + "'";
                }
                else if (txt_job.Text == "" && txt_container.SelectedItem.Text != "" && txt_customer.Text != "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and customer ='" + txt_customer.SelectedItem.Text + " 'and containerno ='" + txt_container.SelectedItem.Text + "'";
                }
                else if (txt_job.Text != "" && txt_container.SelectedItem.Text != "" && txt_customer.Text != "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and customer ='" + txt_customer.SelectedItem.Text + " 'and containerno ='" + txt_container.SelectedItem.Text + "' and jobno =" + txt_job.Text;
                }

                else if (txt_job.Text != "" && txt_container.SelectedItem.Text == "" && txt_customer.Text == "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and jobno =" + txt_job.Text;
                }
                else if (txt_job.Text == "" && txt_container.SelectedItem.Text != "" && txt_customer.Text == "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + " 'and containerno ='" + txt_container.SelectedItem.Text + "'";
                }
                else if (txt_job.Text == "" && txt_container.SelectedItem.Text == "" && txt_customer.Text != "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and customer ='" + txt_customer.SelectedItem.Text + "'";
                }







                dt = obj_dtview.ToTable();
                grd.DataSource = dt;
                grd.DataBind();
               // dtevent = objRpt.Getmastereventdetails();

                for (int j = 0; j < grd.Rows.Count; j++)
                {
                    var ddl1 = (DropDownList)grd.Rows[j].FindControl("ddl_NextEvent");
                    dtevent = objRpt.Getmastereventdetails(dt.Rows[j]["nextevent"].ToString());
                    ddl1.Items.Add("");
                    ddl1.DataSource = dtevent;
                    ddl1.DataTextField = "Events";

                    ddl1.DataBind();
                    ddl1.SelectedItem.Text = dt.Rows[j]["nextevent"].ToString();
                    TextBox lnkbtn = (TextBox)(grd.Rows[j].FindControl("occuredon"));
                    // lnkbtn.Text = Utility.fn_ConvertDate(dt.Rows[j]["occuredon"].ToString());
                }
                dtevent = objRpt.Getmastereventdetails("0");
                ddlevent.Items.Add("");
                ddlevent.DataSource = dtevent;
                ddlevent.DataTextField = "Events";
            }
        }

        protected void txt_bl_TextChanged(object sender, EventArgs e)
        {
            dt = objRpt.GetCustomersupportall(Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString(), Convert.ToInt32(Session["LoginEmpId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                //grd.DataSource = dt;
                //grd.DataBind();
                DataView obj_dtview = new DataView(dt);
                if (txt_job.Text != "" && txt_booking.SelectedItem.Text != "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and jobno =" + txt_job.Text + " and containerno ='" + txt_container.SelectedItem.Text+"'";
                }
                else if (txt_container.SelectedItem.Text != ""&& txt_job.Text == "" && txt_booking.SelectedItem.Text == "")
                {
                    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "'";

                }
                else if (txt_job.Text != "" && txt_container.SelectedItem.Text != "")
                {
                    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "' and jobno =" + txt_job.Text;
                }
                else if (txt_job.Text == "" && txt_container.SelectedItem.Text == "")
                {
                    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "'";
                }
                else if (txt_job.Text == "" && txt_container.SelectedItem.Text != "" && txt_booking.SelectedItem.Text != "")
                {
                    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "'";
                }
                dt = obj_dtview.ToTable();
                grd.DataSource = dt;
                grd.DataBind();
               // dtevent = objRpt.Getmastereventdetails();

                for (int j = 0; j < grd.Rows.Count; j++)
                {
                    var ddl1 = (DropDownList)grd.Rows[j].FindControl("ddl_NextEvent");
                    dtevent = objRpt.Getmastereventdetails(dt.Rows[j]["nextevent"].ToString());
                    ddl1.Items.Add("");
                    ddl1.DataSource = dtevent;
                    ddl1.DataTextField = "Events";

                    ddl1.DataBind();
                    ddl1.SelectedItem.Text = dt.Rows[j]["nextevent"].ToString();
                    TextBox lnkbtn = (TextBox)(grd.Rows[j].FindControl("occuredon"));
                    // lnkbtn.Text = Utility.fn_ConvertDate(dt.Rows[j]["occuredon"].ToString());
                }
                dtevent = objRpt.Getmastereventdetails("0");
                ddlevent.Items.Add("");
                ddlevent.DataSource = dtevent;
                ddlevent.DataTextField = "Events";
            }
        }

        public void sendMail(int job,string bookingno,string eventname)
        {
            string msub = "";
            string sendqry="";
            string cont = "";
            string remarks = "";
            //string sendqry = "";
            string strinvpl;
            DataTable obj_dt = new DataTable();
            DataTable obj_dt1 = new DataTable();
            DataTable obj_dt2 = new DataTable();
            DataAccess.ForwardingExports.ShippingBill obj_da_shpngbill = new DataAccess.ForwardingExports.ShippingBill();
            DataAccess.Masters.MasterPort obj_da_port = new DataAccess.Masters.MasterPort();
            DataAccess.Masters.MasterCustomer obj_da_customer = new DataAccess.Masters.MasterCustomer();
            //sendqry = sendqry + Session["Companyaddress"].ToString();
            DataTable dtmail = objRpt.Geteventcontent4mail(job, Convert.ToInt32(Session["LoginBranchid"]),eventname, Session["StrTranType"].ToString());
            msub =  eventname+" - Booking # " + bookingno;
            sendqry = sendqry + "<table border=1 width=100% text=blue><tr><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=3 COLOR=Black><B>Please find below gate out details</B></FONT></td></tr><br>";

            sendqry = sendqry + "<table width=100%><tr><td align=left><FONT FACE=sans-serif SIZE=2>Dear Sir / Madam,</FONT></td></tr></table><br>";
            sendqry = sendqry + "<table width=100%><tr><td align=left><FONT FACE=sans-serif SIZE=2>Thank you for your valuable shipment. This is to bring to your kind attention that your shipment has stuffed & the tentative sailing details are as follows</FONT></td></tr></table><br>";
            //        sendqry = sendqry & "<tr><td align=left><FONT FACE=sans-serif SIZE=2>Note : Please forward Invoice and Packing List copies for this Shipment</FONT></td></tr><br><br>"
            //sendqry = sendqry & "<table width=100%><tr><td align=left><FONT FACE=sans-serif SIZE=2>Job # : " & txt_job.Text & "</FONT></td><td></td><td></td><td></td><td align=left><FONT FACE=sans-serif SIZE=2>Booking # : " & txt_book.Text & "</FONT></td></tr></table>"
            sendqry = sendqry + "<table width=100%><tr><td align=left><FONT FACE=sans-serif SIZE=2>Job # :" + job + "</FONT></td></tr>";
            sendqry = sendqry + "<tr><td align=left><FONT FACE=sans-serif SIZE=2>Booking # : " + bookingno + "</FONT></td></tr></table>";
            sendqry = sendqry + "<table width=100%><tr><td align=left><FONT FACE=sans-serif SIZE=2>POR : " + dtmail.Rows[0]["por"].ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>POL : " + dtmail.Rows[0]["pol"].ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>POD : " + dtmail.Rows[0]["pod"].ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>FD : " + dtmail.Rows[0]["fd"].ToString() + "</FONT></td></tr></table><br>";

            sendqry = sendqry + "<table border=1><tr><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Container</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>SizeType</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Seal #</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>eventdate</B></FONT></td> </tr><br>";
            for (int j = 0; j <= dtmail.Rows.Count - 1; j++)
            {


                sendqry = sendqry + "<tr><td align=left><FONT FACE=sans-serif SIZE=2>" + dtmail.Rows[0]["containerno"].ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + dtmail.Rows[0]["sizetype"].ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + dtmail.Rows[0]["sealno"].ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + dtmail.Rows[0]["eventdate"].ToString()+"</FONT></td></tr>";

            }

            Utility.SendMail("Sindhuja.Velayutham@ltsolutions.co.in", "Sindhuja.Velayutham@ltsolutions.co.in", msub, sendqry, "", "Sindhuja01@)@@", "", "");

            //int j = 0;
            //if (Grd_sb.Rows.Count > 0)
            //{
            //    if (jobtype != 3)
            //    {
            //        sendqry = sendqry + "<table border=1><tr><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>S/Bill # & Date</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Exporter</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Container / Size / Seal</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>No.Of Pkgs.</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Volume</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Gross Wt.</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Remarks</B></FONT></td></tr><br>";
            //        for (j = 0; j <= Grd_sb.Rows.Count - 1; j++)
            //        {
            //            cont = "";
            //            obj_dt1 = obj_da_shpngbill.GetContainerDetails(Convert.ToInt32(txt_job.Text.ToString()), Grd_sb.Rows[j].Cells[0].Text.ToString(), Convert.ToInt32(Session["LoginBranchid"]), Convert.ToInt32(Session["LoginDivisionid"]));
            //            if (obj_dt1.Rows.Count > 0)
            //            {
            //                for (int i = 0; i <= obj_dt1.Rows.Count - 1; i++)
            //                {
            //                    cont = cont + obj_dt1.Rows[i][0].ToString() + " / " + obj_dt1.Rows[i][1].ToString() + " / " + obj_dt1.Rows[i][2] + "  , ";
            //                }
            //                if (!string.IsNullOrEmpty(cont))
            //                {
            //                    cont = cont.Remove(cont.Length - 2, 2);
            //                }
            //            }
            //            sendqry = sendqry + "<tr><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[0].Text.ToString() + " & " + Grd_sb.Rows[j].Cells[1].Text.ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[2].Text.ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + cont + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[4].Text.ToString() + "  " + Grd_sb.Rows[j].Cells[5].Text.ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[7].Text.ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[6].Text.ToString() + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[9].Text.ToString() + "</FONT></td></tr>";

            //        }
            //    }
            //    else
            //    {
            //        sendqry = sendqry + "<table border=1><tr><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>S/Bill # & Date</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Exporter</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Container / Size / Seal</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>No.Of Pkgs.</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Gross Wt.</B></FONT></td><td align=center BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black><B>Remarks</B></FONT></td></tr><br>";
            //        for (j = 0; j <= Grd_sb.Rows.Count - 1; j++)
            //        {
            //            cont = "";
            //            obj_dt2 = obj_da_shpngbill.GetContainerDetails(Convert.ToInt32(txt_job.Text.ToString()), Grd_sb.Rows[j].Cells[0].Text, Convert.ToInt32(Session["LoginBranchid"]), Convert.ToInt32(Session["LoginDivisionid"]));
            //            if (obj_dt2.Rows.Count > 0)
            //            {
            //                for (int i = 0; i <= obj_dt2.Rows.Count - 1; i++)
            //                {
            //                    cont = cont + obj_dt2.Rows[i][0].ToString() + " / " + obj_dt2.Rows[i][1].ToString() + " / " + obj_dt2.Rows[i][2].ToString() + "  , ";
            //                }
            //                if (!string.IsNullOrEmpty(cont))
            //                {
            //                    cont = cont.Remove(cont.Length - 2, 2);
            //                }
            //            }
            //            sendqry = sendqry + "<tr><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[0].Text + " & " + Grd_sb.Rows[j].Cells[1].Text + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[2].Text + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + cont + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[4].Text + "  " + Grd_sb.Rows[j].Cells[5].Text + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[6].Text + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_sb.Rows[j].Cells[9].Text + "</FONT></td></tr>";
            //        }
            //    }

            //    cont = "";
            //    sendqry = sendqry + "</table>";
            //}

            //sendqry = sendqry + "<table border=1><tr><td align=left BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black>Feeder Vessel</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + txt_vessel.Text + "</FONT></td></tr><br>";
            //sendqry = sendqry + "<tr><td align=left BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black>ETD  " + txt_pol.Text + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + txt_etd.Text + "</FONT></td></tr>";
            //sendqry = sendqry + "<tr><td align=left BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black>ETA  " + txt_pod.Text + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + txt_eta.Text + "</FONT></td></tr>";
            //for (int i = 0; i <= Grd_vessel.Rows.Count - 1; i++)
            //{
            //    sendqry = sendqry + "<tr><td align=left BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black>Intended to Connect On</FONT></td></tr><br>";
            //    sendqry = sendqry + "<tr><td align=left BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black>Mother Vessel</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_vessel.Rows[i].Cells[0].Text + "</FONT></td></tr>";
            //    sendqry = sendqry + "<tr><td align=left BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black>ETD  " + Grd_vessel.Rows[i].Cells[1].Text + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_vessel.Rows[i].Cells[2].Text + "</FONT></td></tr>";
            //    sendqry = sendqry + "<tr><td align=left BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black>ETA  " + Grd_vessel.Rows[i].Cells[3].Text + "</FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_vessel.Rows[i].Cells[4].Text + "</FONT></td></tr>";
            //    sendqry = sendqry + "<tr><td align=left BGCOLOR=#CCCCCC><FONT FACE=Arial SIZE=2 COLOR=Black>Remarks </FONT></td><td align=left><FONT FACE=sans-serif SIZE=2>" + Grd_vessel.Rows[i].Cells[5].Text + "</FONT></td></tr>";
            //}
            //sendqry = sendqry + "</table><br>";
                           
            //sendqry = sendqry + "<table><tr><td align=left><FONT FACE=sans-serif SIZE=2>Assuring you our best service at all times & kindly acknowledge the receipt.</FONT></td></tr></table><br>";
            //sendqry = sendqry + "<table><tr><td align=left><FONT FACE=sans-serif SIZE=2>Thanks & Best Regards,</FONT></td></tr><br>";

        }

        protected void txt_customer_TextChanged(object sender, EventArgs e)
        {
            dt = objRpt.GetCustomersupportall(Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString(), Convert.ToInt32(Session["LoginEmpId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                //grd.DataSource = dt;
                //grd.DataBind();
                DataView obj_dtview = new DataView(dt);
                if (txt_job.Text != "" && txt_booking.SelectedItem.Text != "" && txt_container.SelectedItem.Text != "")
                {
                    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and jobno =" + txt_job.Text + " and containerno ='" + txt_container.SelectedItem.Text + "' and customer ='" + txt_customer.SelectedItem.Text + "'";
                }
                else if (txt_container.SelectedItem.Text != "" && txt_job.Text == "" && txt_booking.SelectedItem.Text == "")
                {
                    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "' and customer ='" + txt_customer.SelectedItem.Text + "'";

                }
                else if (txt_job.Text != "" && txt_container.SelectedItem.Text != "" && txt_booking.SelectedItem.Text == "")
                {
                    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "' and jobno =" + txt_job.Text + "' and customer ='" + txt_customer.SelectedItem.Text + "'";
                }
                else if (txt_job.Text == "" && txt_container.SelectedItem.Text == "" && txt_booking.SelectedItem.Text == "")
                {
                    obj_dtview.RowFilter = "customer ='" + txt_customer.SelectedItem.Text + "'";
                }
                else if (txt_job.Text == "" && txt_container.SelectedItem.Text != "" && txt_booking.SelectedItem.Text != "")
                {
                    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "' and bookingno = '" + txt_booking.SelectedItem.Text + "' and customer ='" + txt_customer.SelectedItem.Text + "'";
                }
                dt = obj_dtview.ToTable();
                grd.DataSource = dt;
                grd.DataBind();
                // dtevent = objRpt.Getmastereventdetails();

                for (int j = 0; j < grd.Rows.Count; j++)
                {
                    var ddl1 = (DropDownList)grd.Rows[j].FindControl("ddl_NextEvent");
                    dtevent = objRpt.Getmastereventdetails(dt.Rows[j]["nextevent"].ToString());
                    ddl1.Items.Add("");
                    ddl1.DataSource = dtevent;
                    ddl1.DataTextField = "Events";

                    ddl1.DataBind();
                    ddl1.SelectedItem.Text = dt.Rows[j]["nextevent"].ToString();
                    TextBox lnkbtn = (TextBox)(grd.Rows[j].FindControl("occuredon"));
                    // lnkbtn.Text = Utility.fn_ConvertDate(dt.Rows[j]["occuredon"].ToString());
                }
                dtevent = objRpt.Getmastereventdetails("0");
                ddlevent.Items.Add("");
                ddlevent.DataSource = dtevent;
                ddlevent.DataTextField = "Events";
            }
        }

        

        protected void ddl_product_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlevent_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt = objRpt.GetCustomersupportall(Convert.ToInt32(Session["LoginBranchid"]), Session["StrTranType"].ToString(), Convert.ToInt32(Session["LoginEmpId"].ToString()));
            if (dt.Rows.Count > 0)
            {
                //grd.DataSource = dt;
                //grd.DataBind();
                DataView obj_dtview = new DataView(dt);
                if (txt_job.Text == "" && txt_container.SelectedItem.Text == "" && txt_booking.SelectedItem.Text == "" && txt_customer.Text == "")
                {
                    obj_dtview.RowFilter = "nextevent = '" + ddlevent.SelectedItem.Text + "'";
                }
                else if (txt_job.Text != "" && txt_container.SelectedItem.Text == "" && txt_booking.SelectedItem.Text == "" && txt_customer.Text == "")
                {
                    obj_dtview.RowFilter = "nextevent = '" + ddlevent.SelectedItem.Text +  "' and jobno =" + txt_job.Text  ;
                }
                else if (txt_job.Text != "" && txt_container.SelectedItem.Text != "" && txt_booking.SelectedItem.Text == "" && txt_customer.Text == "")
                {
                    obj_dtview.RowFilter = "nextevent = '" + ddlevent.SelectedItem.Text + "' and jobno =" + txt_job.Text + " and containerno ='" + txt_container.SelectedItem.Text + "'";
                }

                else if (txt_job.Text != "" && txt_container.SelectedItem.Text != "" && txt_booking.SelectedItem.Text != "" && txt_customer.Text == "")
                {
                    obj_dtview.RowFilter = "nextevent = '" + ddlevent.SelectedItem.Text + "' and jobno =" + txt_job.Text + " and containerno ='" + txt_container.SelectedItem.Text+ "'bookingno = '" + txt_booking.SelectedItem.Text+ "'";
                }
                else if (txt_job.Text != "" && txt_container.SelectedItem.Text != "" && txt_booking.SelectedItem.Text != "" && txt_customer.Text != "")
                {
                    obj_dtview.RowFilter = "nextevent = '" + ddlevent.SelectedItem.Text + "' and jobno =" + txt_job.Text + " and containerno ='" + txt_container.SelectedItem.Text + "'bookingno = '" + txt_booking.SelectedItem.Text + "' and customer ='" + txt_customer.SelectedItem.Text + "'";
                }





                //else if (txt_job.Text != "" && txt_booking.SelectedItem.Text != "")
                //{
                //    obj_dtview.RowFilter = "bookingno = '" + txt_booking.SelectedItem.Text + "' and jobno =" + txt_job.Text + " and containerno ='" + txt_container.SelectedItem.Text + "'";
                //}
                //else if (txt_container.SelectedItem.Text != "" && txt_job.Text == "" && txt_booking.SelectedItem.Text == "")
                //{
                //    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "'";

                //}
                //else if (txt_job.Text != "" && txt_container.SelectedItem.Text != "")
                //{
                //    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "' and jobno =" + txt_job.Text;
                //}
                //else if (txt_job.Text == "" && txt_container.SelectedItem.Text == "")
                //{
                //    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "'";
                //}
                //else if (txt_job.Text == "" && txt_container.SelectedItem.Text != "" && txt_booking.SelectedItem.Text != "")
                //{
                //    obj_dtview.RowFilter = "containerno = '" + txt_container.SelectedItem.Text + "'";
                //}
                dt = obj_dtview.ToTable();
                grd.DataSource = dt;
                grd.DataBind();
                // dtevent = objRpt.Getmastereventdetails();

                for (int j = 0; j < grd.Rows.Count; j++)
                {
                    var ddl1 = (DropDownList)grd.Rows[j].FindControl("ddl_NextEvent");
                    dtevent = objRpt.Getmastereventdetails(dt.Rows[j]["nextevent"].ToString());
                    ddl1.Items.Add("");
                    ddl1.DataSource = dtevent;
                    ddl1.DataTextField = "Events";

                    ddl1.DataBind();
                    ddl1.SelectedItem.Text = dt.Rows[j]["nextevent"].ToString();
                    TextBox lnkbtn = (TextBox)(grd.Rows[j].FindControl("occuredon"));
                    // lnkbtn.Text = Utility.fn_ConvertDate(dt.Rows[j]["occuredon"].ToString());
                }
                dtevent = objRpt.Getmastereventdetails("0");
                ddlevent.Items.Add("");
                ddlevent.DataSource = dtevent;
                ddlevent.DataTextField = "Events";
            }
        }
    }
}