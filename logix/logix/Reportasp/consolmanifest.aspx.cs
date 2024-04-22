using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataTable = System.Data.DataTable;

namespace logix.Reportasp
{
    public partial class consolmanifest : System.Web.UI.Page
    {
        string bid;
        string jobno;
        string hf_bl, hf_dbl, hf_nt, length, breadth, width;
        int noof;
        double grosswt, chargewt;
        DataAccess.Accounts.OSDNCN objosdncn = new DataAccess.Accounts.OSDNCN();
        DataAccess.Masters.MasterDivision masterObj = new DataAccess.Masters.MasterDivision();
        //DataTable dtcust = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            string Ccode = Convert.ToString(Session["Ccode"]);

            if (Ccode != "")
            {

                objosdncn.GetDataBase(Ccode);
                masterObj.GetDataBase(Ccode);



            }


            if (Session["LoginUserName"] == null || Session["LoginEmpId"] == null || Session["LoginBranchid"] == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Master", "alertify.alert('Session TimeOut');window.open('http://CHawk.copperhawk.tech'_top');", true);
            }
            try
            {
                if (Request.QueryString.ToString().Contains("jobno"))
                {
                    jobno = Request.QueryString["jobno"];
                    bid = Request.QueryString["bid"];
                    hf_bl = Request.QueryString["hf_bl"];
                    hf_dbl = Request.QueryString["hf_dbl"];
                    hf_nt = Request.QueryString["hf_nt"];
                }
                if (hf_bl == "0")
                {
                    thead.Text = "CONSOL MANIFEST";

                }
                else
                {
                    thead.Text = "BOOKING";
                    bl_visi.Visible = false;
                }

                if (hf_nt != "Y")
                {
                    lblnotifedname.Visible = false;
                    lblnotifiedaddress.Visible = false;
                }
                //DataAccess.Masters.MasterDivision masterObj = new DataAccess.Masters.MasterDivision();
                DataTable dtlogo = masterObj.Getlogo(Convert.ToInt32(Session["LoginDivisionId"]));
                if (dtlogo.Rows.Count > 0)
                {
                    byte[] logoimageBytes = ((byte[])dtlogo.Rows[0]["dlogo"]);
                    string base64String = Convert.ToBase64String(logoimageBytes);
                    lbl_img.ImageUrl = "data:image/png;base64," + base64String;
                }
                System.Data.DataSet set = new System.Data.DataSet();
                set = objosdncn.Getagentbooking(jobno.ToString(), Convert.ToInt32(bid));
                DataTable dtcust = set.Tables[0];
                if (dtcust.Rows.Count > 0)
                {
                    if (hf_dbl == "N")
                    {
                        lblpreparename.Text = dtcust.Rows[0]["branchname"].ToString();
                        lblprepareaddress.Text = dtcust.Rows[0]["masteraddress"].ToString();
                        lblconsignname.Text = dtcust.Rows[0]["customername"].ToString();
                        lblconsignaddress.Text = dtcust.Rows[0]["address"].ToString();
                        lbl_cunname.Text = dtcust.Rows[0]["branchcountryname"].ToString();
                        lbl_counname.Text = dtcust.Rows[0]["agentcountryname"].ToString();
                    }
                    else
                    {
                        lblpreparename.Text = dtcust.Rows[0]["sname"].ToString();
                        lblprepareaddress.Text = dtcust.Rows[0]["saddress"].ToString();
                        lblconsignname.Text = dtcust.Rows[0]["cname"].ToString();
                        lblconsignaddress.Text = dtcust.Rows[0]["caddress"].ToString();
                        lbl_cunname.Text = dtcust.Rows[0]["shipcountryname"].ToString();
                        lbl_counname.Text = dtcust.Rows[0]["consgcountryname"].ToString();
                    }
                    lblbrname.Text = dtcust.Rows[0]["branchname"].ToString();
                    lbladdress.Text = dtcust.Rows[0]["masteraddress"].ToString();
                    lblphone.Text = dtcust.Rows[0]["masterphone"].ToString();
                    lblfax.Text = dtcust.Rows[0]["masterfax"].ToString();
                    lblnotifedname.Text = dtcust.Rows[0]["n1name"].ToString();
                    lblnotifiedaddress.Text = dtcust.Rows[0]["n1address"].ToString();
                    lblblno.Text = dtcust.Rows[0]["mawblno"].ToString();
                    lblbldate.Text = Convert.ToDateTime(dtcust.Rows[0]["mawbldate"].ToString()).ToString("dd/MM/yyyy");
                    lblcarrier.Text = dtcust.Rows[0]["carriercustomername"].ToString();
                    lblloading.Text = dtcust.Rows[0]["polairportcode"].ToString();
                    lbldestination.Text = dtcust.Rows[0]["podairportcode"].ToString();


                    for (int i = 0; i < dtcust.Rows.Count; i++)
                    {
                        noof += Convert.ToInt32(dtcust.Rows[i]["noofpkgs"].ToString());

                        grosswt += Convert.ToDouble(dtcust.Rows[i]["grosswt"].ToString());

                        chargewt += Convert.ToDouble(dtcust.Rows[i]["chargewt"].ToString());

                    }
                    lblpackage.Text = noof.ToString();
                    lblgross.Text = grosswt.ToString();
                    lblcharge.Text = chargewt.ToString();
                    lbldesc.Text = " " + dtcust.Rows[0]["descn"].ToString();
                    lblconsol.Text = dtcust.Rows[0]["jobno"].ToString();
                    lblflightno.Text = dtcust.Rows[0]["flightno"].ToString().ToUpper();
                    lblflightdate.Text = Convert.ToDateTime(dtcust.Rows[0]["flightdate"].ToString()).ToString("dd/MM/yyyy");



                    for (int i = 0; i < dtcust.Rows.Count; i++)
                    {
                        int a = i + 1;
                        lbldetails.Text += " <tr style=''>";
                        lbldetails.Text += "<td><label>" + a + "</label></td>";
                        if (hf_dbl == "N")
                        {
                            lbldetails.Text += "<td><label>" + dtcust.Rows[i]["hawblno"].ToString() + "</label></td>";
                        }
                        else
                        {
                            lbldetails.Text += "<td><label></label></td>";
                        }
                        lbldetails.Text += "<td><label>" + dtcust.Rows[i]["noofpkgs"].ToString() + " " + dtcust.Rows[i]["descn"].ToString() + "</label></td>";
                        lbldetails.Text += "<td><label>" + dtcust.Rows[i]["grosswt"].ToString() + "</label></td>";
                        lbldetails.Text += "<td><label>" + dtcust.Rows[i]["volwt"].ToString() + "  </label></td>";
                        lbldetails.Text += "<td><label>" + dtcust.Rows[i]["cargotype"].ToString() + "</label></td>";
                        lbldetails.Text += "<td><label>" + dtcust.Rows[i]["shippername"].ToString() + "<br>" + dtcust.Rows[i]["shipcountryname"].ToString() + "</label></td>";
                        lbldetails.Text += "<td><label>" + dtcust.Rows[i]["consigneename"].ToString() + "<br>" + dtcust.Rows[i]["consgcountryname"].ToString() + "</label></td><td>";

                        string[] strSplit = dtcust.Rows[i]["Dimensions"].ToString().Split(',');

                        //foreach (DataRow row in dtcust.Rows)
                        //{
                        for (int k = 0; k < strSplit.Length; k++)
                        {
                            lbldetails.Text += strSplit[k].ToString() + "</br>";
                        }
                        lbldetails.Text += "</td></tr>";
                        //}


                        //for (int j = 0; j < set.Tables[1].Rows.Count; j++)
                        //{
                        //    length = set.Tables[1].Rows[j]["length"] != DBNull.Value ? set.Tables[1].Rows[j]["length"].ToString() : "0";
                        //    breadth = set.Tables[1].Rows[j]["breadth"] != DBNull.Value ? set.Tables[1].Rows[j]["breadth"].ToString() : "0";
                        //    width = set.Tables[1].Rows[j]["width"] != DBNull.Value ? set.Tables[1].Rows[j]["width"].ToString() : "0";
                        //    lbldetails.Text += "<label>" + length.ToString() + " X " + breadth.ToString() + " X " + width.ToString() + " X " + set.Tables[1].Rows[j]["pieces"].ToString() + "</br>" ;
                        //}
                        //lbldetails.Text += "</td></tr>";
                    }

                    //noof = 0;
                    //grosswt = 0;
                    chargewt = 0;
                    for (int i = 0; i < dtcust.Rows.Count; i++)
                    {
                        //noof += Convert.ToInt32(dtcust.Rows[i]["noofpkgs"].ToString());

                        //grosswt += Convert.ToDouble(dtcust.Rows[i]["grosswt"].ToString());

                        chargewt += Convert.ToDouble(dtcust.Rows[i]["chargewt"].ToString());

                    }
                    //lblnoofpkg.Text = noof.ToString();
                    //lbltotalgr.Text = grosswt.ToString();
                    //lbltotalvol.Text = chargewt.ToString();
                    //lbldesc2.Text = dtcust.Rows[0]["descn"].ToString();
                    lbltotalcharge.Text = chargewt.ToString();


                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Button), "   ", "alertify.alert('" + message + "');", true);
            }



        }
    }
}