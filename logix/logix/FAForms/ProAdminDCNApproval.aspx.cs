﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess.Accounts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Drawing;

namespace logix.FAForm
{
    public partial class ProAdminDCNApproval : System.Web.UI.Page
    {

       
        protected void Grd_Approval_PreRender(object sender, EventArgs e)
        {
            Grd_Approval.UseAccessibleHeader = true;
            Grd_Approval.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        DataAccess.LogDetails Logobj = new DataAccess.LogDetails();
        DataAccess.Accounts.Approval Appobj = new DataAccess.Accounts.Approval();
        DataAccess.Accounts.AdminDCNNo obj_da_AdminDNCN = new DataAccess.Accounts.AdminDCNNo();
        DataAccess.Accounts.ProAdminDCNNo ProDCNObj = new DataAccess.Accounts.ProAdminDCNNo();

        DataAccess.Accounts.ProAdminDCNNo obj_da_ProAdminDNCN = new DataAccess.Accounts.ProAdminDCNNo();
        DataAccess.Accounts.Approval obj_da_Approval = new DataAccess.Accounts.Approval();
        DataAccess.Accounts.Invoice obj_da_Invoice = new DataAccess.Accounts.Invoice();
        DataAccess.FAMaster.MasterLedger obj_da_Ledger = new DataAccess.FAMaster.MasterLedger();
        DataAccess.LogDetails obj_da_Log = new DataAccess.LogDetails();
        DataAccess.FAVoucher obj_da_FAVoucher = new DataAccess.FAVoucher();
        DataAccess.Masters.MasterEmployee Obj_Emp = new DataAccess.Masters.MasterEmployee();
        DataAccess.Masters.MasterCustomer customerobj = new DataAccess.Masters.MasterCustomer();
        DataAccess.Masters.MasterCustomer obj_da_Customer = new DataAccess.Masters.MasterCustomer();
        DataAccess.FAVoucher obj_da_FA = new DataAccess.FAVoucher();
        DataAccess.LogDetails da_obj_Log = new DataAccess.LogDetails();
        DataAccess.Accounts.Reversal obj_da_Reversal = new DataAccess.Accounts.Reversal();
        DataAccess.Accounts.ProAdminDCNNo ProDCNObj2 = new DataAccess.Accounts.ProAdminDCNNo();
        int chkledgerid, customerid;
        string time, Lst_DNno;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "SpanTagMoveInputBottom();MuiTextField();dropdown();", true);


            string Ccode = Convert.ToString(Session["Ccode"]);

            if (Ccode != "")
            {

                obj_da_AdminDNCN.GetDataBase(Ccode);
                ProDCNObj.GetDataBase(Ccode);
                obj_da_ProAdminDNCN.GetDataBase(Ccode);
                Appobj.GetDataBase(Ccode);
                obj_da_Ledger.GetDataBase(Ccode);
                Obj_Emp.GetDataBase(Ccode);
                customerobj.GetDataBase(Ccode);
                obj_da_Customer.GetDataBase(Ccode);
                obj_da_FA.GetDataBase(Ccode);
                obj_da_Approval.GetDataBase(Ccode);
                obj_da_Invoice.GetDataBase(Ccode);


                da_obj_Log.GetDataBase(Ccode);
                obj_da_Reversal.GetDataBase(Ccode);




            }

            if (Session["LoginUserName"] == null || Session["LoginEmpId"] == null || Session["LoginDivisionId"] == null || Session["LoginBranchid"] == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Master", "alertify.alert('Session TimeOut');window.open('https://demo.copperhawk.tech/','_top');", true);
            }

            ((ScriptManager)Master.FindControl("ScriptManager1")).RegisterPostBackControl(btn_cancel);
            if (Session["Loginyear"].ToString() == Session["Vouyear"].ToString())
            {

            }
            else
            {
                btn_Approve.Visible = false;
            }
            if (Request.QueryString.ToString().Contains("FormName"))
            {
                lbl_Header.Text = Request.QueryString["FormName"].ToString();

                if (lbl_Header.Text == "DN Proforma to Commercial-Admin")
                {
                    lbl_Header.Text = "Profoma DNApproval - Admin";
                    lbl_head.InnerText = "Profoma DNApproval - Admin";
                    hid_type.Value = "DN";
                    btn_backdated.Visible = true;
                }
                else
                {
                    lbl_Header.Text = "Profoma CNApproval - Admin";
                    lbl_head.InnerText = "Profoma CNApproval - Admin";
                    hid_type.Value = "PA";
                    btn_backdated.Visible = true;
                }
            }

            if (!IsPostBack)
            {
                try
                {
                    lbnl_logyear.Text = Session["LYEAR"].ToString();
                    Fn_LoadDetail();
                }
                catch (Exception ex)
                {
                    string message = ex.Message.ToString();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.WebControls.Button), "logix", "alertify.alert('" + message + "');", true);
                }
            }
        }

        private void Fn_LoadDetail()
        {
           // DataAccess.Accounts.AdminDCNNo obj_da_AdminDNCN = new DataAccess.Accounts.AdminDCNNo();
            DataAccess.Accounts.ProAdminDCNNo ProDCNObj = new DataAccess.Accounts.ProAdminDCNNo();
            string Ccode = Convert.ToString(Session["Ccode"]);
            ProDCNObj.GetDataBase(Ccode);
            DataTable obj_dt = new DataTable();
           // obj_dt = ProDCNObj.GetApproveProAdminDCNCOM_new(Convert.ToInt32(ddl_voutype.SelectedValue), int.Parse(Session["LoginBranchid"].ToString()));
            Grd_Approval.DataSource = obj_dt;
            Grd_Approval.DataBind();
            //DataAccess.Accounts.Invoice obj_da_Invoice = new DataAccess.Accounts.Invoice();
            DataTable dlp = new DataTable();
            dlp = obj_da_Invoice.sp_ddlsectionnew1();

            for (int j = 0; j < Grd_Approval.Rows.Count; j++)
            {
                var ddl1 = (DropDownList)Grd_Approval.Rows[j].FindControl("ddl_section");

                ddl1.DataSource = dlp;
                ddl1.DataTextField = "tdssection";

                ddl1.DataBind();

            }
        }

        protected void btn_Approve_Click(object sender, EventArgs e)
        {
            try
            {
                //DataAccess.Masters.MasterCustomer obj_da_Customer = new //DataAccess.Masters.MasterCustomer();
                //DataAccess.FAVoucher obj_da_FA = new //DataAccess.FAVoucher();
                int gsttype = 0, statename = 0, supplyto = 0;
                string gsttype_ = "", statename_ = "", supplyto_ = "";
                int int_Empid = 0, int_bid = 0, int_divisionid = 0, int_intdcno = 0, int_Vouyear = 0, int_DCno = 0, int_Voutypeid = 0, preparedby = 0,intVoutype=0;
                string Str_Trantype = Session["StrTranType"].ToString();
                int_Empid = int.Parse(Session["LoginEmpId"].ToString());
                int_bid = int.Parse(Session["LoginBranchid"].ToString());
                int_divisionid = int.Parse(Session["LoginDivisionId"].ToString());
                DataTable obj_dt = new DataTable();
                //DataAccess.Accounts.AdminDCNNo obj_da_AdminDNCN = new //DataAccess.Accounts.AdminDCNNo();
                //DataAccess.Accounts.ProAdminDCNNo obj_da_ProAdminDNCN = new //DataAccess.Accounts.ProAdminDCNNo();
                //DataAccess.Accounts.Approval obj_da_Approval = new //DataAccess.Accounts.Approval();
                //DataAccess.Accounts.Invoice obj_da_Invoice = new //DataAccess.Accounts.Invoice();
                //DataAccess.FAMaster.MasterLedger obj_da_Ledger = new //DataAccess.FAMaster.MasterLedger();
                //DataAccess.LogDetails obj_da_Log = new //DataAccess.LogDetails();
                //DataAccess.FAVoucher obj_da_FAVoucher = new //DataAccess.FAVoucher();
                //DataAccess.Accounts.ProAdminDCNNo ProDCNObj = new //DataAccess.Accounts.ProAdminDCNNo();
                //DataAccess.Masters.MasterEmployee Obj_Emp = new //DataAccess.Masters.MasterEmployee();
                bool flag = true;
                string StrScript = "", chargename = "";
                int int_Custid = 0;
                TextBox Txt = new TextBox();
                int countryid = 0;
                string tdstype = "", tdsdesc = "";
                string str_tdstype = "", str_tdsdesc = "";
                int int_tdstype = 0, int_tdsdesc = 0;
                int int_Vouyear1 = 0;
                int int_Invoiceno = 0, int_Refno = 0;
                double Amount = 0, TDS = 0, TDSAmount = 0, CSTAmount = 0, gstamt = 0;
                DataTable dcon = Appobj.Checkcountry(int_bid);
                int con = Convert.ToInt32(dcon.Rows[0]["countryid"]);
                string str_Voutype, type;
                foreach (GridViewRow row in Grd_Approval.Rows)
                {
                    CheckBox Chk = (CheckBox)row.FindControl("Chk_Approval");
                    if (Chk.Checked == true)
                    {
                        hid_refno.Value = row.Cells[1].Text.ToString();
                        DataTable dt = new DataTable();
                        dt = Appobj.approve_transfer(hid_refno.Value, Convert.ToInt32(Session["LoginBranchid"].ToString()), Session["StrTranType"].ToString(), hid_type.Value.ToString());
                        if (dt.Rows.Count > 0)
                        {
                            int_Vouyear = int.Parse(Grd_Approval.DataKeys[row.RowIndex].Values[0].ToString());
                            int_intdcno = int.Parse(Grd_Approval.DataKeys[row.RowIndex].Values[1].ToString());
                            preparedby = Obj_Emp.GetNEmpid(Grd_Approval.Rows[row.RowIndex].Cells[6].Text);
                            hid_stamt.Value = Grd_Approval.Rows[row.RowIndex].Cells[16].Text;
                            hid_supplyto.Value = Grd_Approval.Rows[row.RowIndex].Cells[17].Text;
                            intVoutype = int.Parse(Grd_Approval.DataKeys[row.RowIndex].Values[2].ToString());
                            if (Grd_Approval.Rows[row.RowIndex].Cells[2].Text == "Admin Sales Invoice")
                            {                                 
                                hid_type.Value = "DN";
                                str_Voutype = "X";
                                type= "X";


                            }
                            else
                            {                                 
                                hid_type.Value = "PA";
                                str_Voutype = "S";
                                type = "S";
                            }
                            bool ChkLedger = true;
                            /******************* For Auto mail *********************/
                            hid_refno.Value = row.Cells[1].Text.ToString();
                            //hid_vouyear.Value = Grd_Approval.DataKeys[row.RowIndex].Values[0].ToString();
                            hid_vouyear.Value = obj_da_FAVoucher.Getvouyearforautotransfer(int_bid).ToString();
                            /****************************************/

                            //if (preparedby == int_Empid)
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "alertify.alert('You have no rights to approve Voucher # " + int_intdcno + " prepared by you')", true);
                            //    continue;
                            //}

                            if (Session["hid_gstdate"] != null && int_divisionid == 1)      //NewOne    //10/1/2022
                            {

                                if (Convert.ToDateTime(obj_da_Log.GetDate().ToShortDateString()) >= Convert.ToDateTime(Session["hid_gstdate"].ToString()))
                                {
                                    if (hid_supplyto.Value != "0" )
                                    {
                                        if (Convert.ToDouble(hid_stamt.Value) > 0)
                                        {
                                            int int_custidnew;
                                            DataTable dt_list = new DataTable();
                                            //DataAccess.Masters.MasterCustomer customerobj = new //DataAccess.Masters.MasterCustomer();

                                            if (!string.IsNullOrEmpty(row.Cells[4].Text.ToString()))
                                            {
                                                int_custidnew = Convert.ToInt32(row.Cells[17].Text.ToString());
                                                dt_list = customerobj.GetIndianCustomergstadd(int_custidnew);
                                            }

                                            if (dt_list.Rows.Count > 0)
                                            {
                                                if (!string.IsNullOrEmpty(dt_list.Rows[0]["GSTGroup"].ToString()))
                                                {
                                                    if (dt_list.Rows[0]["GSTGroup"].ToString() == "N")
                                                    {
                                                        if (gsttype == 0)
                                                        {
                                                            gsttype_ = Grd_Approval.Rows[row.RowIndex].Cells[4].Text;
                                                        }
                                                        else
                                                        {
                                                            gsttype_ = " ," + Grd_Approval.Rows[row.RowIndex].Cells[4].Text;
                                                        }
                                                        gsttype = 1;
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    if (statename == 0)
                                                    {
                                                        statename_ = Grd_Approval.Rows[row.RowIndex].Cells[4].Text;
                                                    }
                                                    else
                                                    {
                                                        statename_ = " ," + Grd_Approval.Rows[row.RowIndex].Cells[4].Text;
                                                    }
                                                    statename = 1;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                if (statename == 0)
                                                {
                                                    statename_ = Grd_Approval.Rows[row.RowIndex].Cells[4].Text;
                                                }
                                                else
                                                {
                                                    statename_ = " ," + Grd_Approval.Rows[row.RowIndex].Cells[4].Text;
                                                }
                                                statename = 1;
                                                continue;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (supplyto == 0)
                                        {
                                            supplyto_ = int_intdcno.ToString();
                                        }
                                        else
                                        {
                                            supplyto_ = " ," + int_intdcno.ToString();
                                        }
                                        supplyto = 1;
                                        continue;
                                    }
                                }
                            }

                            if (hid_type.Value.ToString() == "DN")
                            {
                                obj_dt = obj_da_Invoice.GetPartyLedger4PAPROAdminwithCustCOM(int_intdcno, intVoutype, int_bid, int_Vouyear);
                            }

                            if (obj_dt.Rows.Count > 0)
                            {
                                for (int i = 0; i <= obj_dt.Rows.Count - 1; i++)
                                {
                                    chkledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(int.Parse(obj_dt.Rows[i]["chargeid"].ToString()), obj_dt.Rows[i]["opstype"].ToString(), Session["FADbname"].ToString());
                                    if (chkledgerid == 0)
                                    {
                                        if (chargename == "")
                                        {
                                            chargename = obj_dt.Rows[i]["chargename"].ToString();
                                        }
                                        else
                                        {
                                            if (chargename.Contains(obj_dt.Rows[i]["chargename"].ToString()))
                                            {

                                            }
                                            else
                                            {
                                                chargename += " , " + obj_dt.Rows[i]["chargename"].ToString();
                                            }
                                        }
                                        flag = false;
                                    }
                                }
                            }

                            if (flag == true)
                            {
                                //if (hid_type.Value.ToString() == "DN")
                                //{
                                int_DCno = obj_da_AdminDNCN.GetAdmvounoCOM(int_bid, intVoutype);
                                ////}
                                ////else
                                ////{
                                ////    int_DCno = obj_da_AdminDNCN.GetAdmCNno(int_bid);
                                ////}
                                int_Invoiceno = int_DCno;
                                obj_da_ProAdminDNCN.UpdApprovalProAdminDCNCOM(int_DCno, int_Empid, intVoutype, int_Vouyear, int_bid, int_intdcno);


                                if (hid_type.Value.ToString() == "PA")
                                {
                                    obj_da_Log.InsLogDetail(int_Empid, 1049, 1, int_bid, hid_type.Value.ToString() + "# - " + int_intdcno + "/Approve");
                                }
                                else
                                {
                                    obj_da_Log.InsLogDetail(int_Empid, 1050, 1, int_bid, hid_type.Value.ToString() + "# - " + int_intdcno + "/Approve");
                                }

                                Lst_DNno += int_DCno + ",";
                            }

                            if (flag == true)
                            {
                                if (hid_type.Value.ToString() == "DN")
                                {
                                    logix.CommanClass.TallyEDIFA.Fn_FATransfer("Admin Sales Invoice", int_DCno, int_DCno, "Remarks", "Ref No", int_bid);

                                    try
                                    {
                                        obj_dt = obj_da_Invoice.FAShowTallyDt(int_DCno, "DN-Admin", int.Parse(Session["Vouyear"].ToString()), int_bid);

                                        if (obj_dt.Rows.Count > 0)
                                        {
                                            int int_custid = int.Parse(obj_dt.Rows[0].ItemArray[4].ToString());

                                            DateTime date_Voudate = Convert.ToDateTime(obj_dt.Rows[0].ItemArray[1].ToString());
                                            int int_Ledgerid = 0;
                                            int_Ledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(int_custid, "C", Session["FADbname"].ToString());

                                            if (int_Ledgerid == 0)
                                            {
                                                int_Ledgerid = Fn_Getcustomergroupid(int_custid);
                                            }
                                            int_Voutypeid = obj_da_FAVoucher.Selvoutypeid("Admin Sales Invoice", Session["FADbname"].ToString());
                                            obj_da_Approval.InsLedgerOPBreakup(int_Ledgerid, int_DCno, Convert.ToDateTime(date_Voudate.ToShortDateString()), 'X', int_Vouyear, int_bid, double.Parse(row.Cells[3].Text.ToString()), "", 0.0, int_custid);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                else if (hid_type.Value.ToString() == "PA")
                                {
                                    DataTable dt_Credit = new DataTable();
                                    dt_Credit = obj_da_Approval.GetAgentCustomerOrNotradmincom(int_DCno, int_Vouyear, int_bid, "AC");

                                    if (dt_Credit.Rows.Count > 0)
                                    {
                                        customerid = Convert.ToInt32(dt_Credit.Rows[0]["customerid"].ToString());
                                        logix.CommanClass.TallyEDIFA.Fn_FATransfer("Admin Purchase Invoice", int_DCno, int_DCno, "Remarks", "Ref No", int_bid);

                                        try
                                        {
                                            DateTime padat, Vou_Date;
                                            DataTable DtSHead = new DataTable();
                                            DataTable dcndt = new DataTable();
                                            int custid = 0;
                                            DtSHead = obj_da_Invoice.FAShowTallyDt(int_DCno, "PA-Admin", int_Vouyear, int_bid);
                                            if (DtSHead.Rows.Count > 0)
                                            {
                                                custid = Convert.ToInt32(DtSHead.Rows[0]["customerid"].ToString());
                                            }
                                            int chkledgerid = 0;

                                            chkledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(customerid, "C", Session["FADbname"].ToString());
                                            if (chkledgerid == 0)
                                            {
                                                chkledgerid = Fn_Getcustomergroupid(custid);
                                            }
                                            int_Voutypeid = obj_da_FAVoucher.Selvoutypeid("Admin Purchase Invoice", Session["FADbname"].ToString());

                                            string custtype = "", fcur = "";
                                            double famt = 0.00, exrate;

                                            
                                            custtype = obj_da_Customer.GetCustomerType(customerid);

                                            if (custtype == "P")
                                            {
                                                dcndt = obj_da_Invoice.GetOtherDCNAmount(int_DCno, "ACNHead", int_bid, int_Vouyear);

                                                fcur = "";
                                                famt = 0.0;
                                                exrate = 0.0;
                                                if (dcndt.Rows.Count > 0)
                                                {
                                                    fcur = dcndt.Rows[0]["curr"].ToString();
                                                    famt = Convert.ToDouble(dcndt.Rows[0]["amt"].ToString());
                                                    exrate = Convert.ToDouble(dcndt.Rows[0]["exrate"].ToString());
                                                }
                                                obj_da_Approval.InsLedgerOPBreakup(chkledgerid, int_DCno, Convert.ToDateTime(DtSHead.Rows[0]["cndate"].ToString()), 'S', int_Vouyear, int_bid, Convert.ToDouble(row.Cells[3].Text.ToString()), fcur, famt, customerid);
                                            }
                                            else
                                            {
                                                obj_da_Approval.InsLedgerOPBreakup(chkledgerid, int_DCno, Convert.ToDateTime(DtSHead.Rows[0]["cndate"].ToString()), 'S', int_Vouyear, int_bid, Convert.ToDouble(row.Cells[3].Text.ToString()), "", 0.0, customerid);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                            }


                            
                            if (hid_type.Value.ToString() == "PA")
                            {

                                int_Custid = int.Parse(hid_supplyto.Value);

                                countryid = obj_da_Invoice.getcustomercountry(Convert.ToInt32(int_Custid));
                                //Amount = double.Parse((Grd_Approval.Rows[row.RowIndex].Cells[4].Text.ToString()));
                                //gstamt = double.Parse((Grd_Approval.Rows[row.RowIndex].Cells[12].Text.ToString()));
                                // Amount = Amount - gstamt;
                                DataTable Dt_LimitCheck = new DataTable();
                                int_Vouyear1 = Convert.ToInt32(obj_da_FA.Getvouyearforautotransfer(int_bid).ToString());
                                Dt_LimitCheck = obj_da_Approval.GetCustAmtLimt(int_Custid, int_bid);
                                Amount = obj_da_Approval.GetVoucherAmount4TDS(int_DCno, int_bid, int_Vouyear1, "S");
                               // log1.Info("Before TDS Procedure1- (PANO- " + int_Invoiceno + " | Branchid- " + int_bid + " |EmployeeID- " + int_Empid + " |Vouyear- " + int_Vouyear1 + ")");
                                if (Dt_LimitCheck.Rows.Count > 0)
                                {
                                    if (Amount > 0)
                                    {
                                        double cstamount = Convert.ToDouble(Dt_LimitCheck.Rows[0]["cstamount"].ToString()) - Amount;
                                        double AmtWidExem, AmtWidTds, AmtwitoutTds;
                                        double tdsemp, tdsper;
                                        if (Convert.ToDouble(cstamount) >= Convert.ToDouble(Dt_LimitCheck.Rows[0]["limit"].ToString()))
                                        {
                                            TDS = Convert.ToDouble(Txt.Text.ToString());
                                            TDSAmount = Convert.ToDouble((Amount * (TDS / 100)).ToString("#0.00"));
                                            //log1.Info("Before TDS Amount > 0- (PANO- " + int_Invoiceno + " | Branchid- " + int_bid + " |EmployeeID- " + int_Empid + " |Vouyear- " + int_Vouyear1 + ")");

                                        }
                                        else //if(Convert.ToInt32(Dt_LimitCheck.Rows[0]["cstamount"].ToString()) < Convert.ToInt32(Dt_LimitCheck.Rows[0]["limit"].ToString()))
                                        {
                                            double diff = Convert.ToDouble(cstamount) + Amount;
                                            if (diff > Convert.ToDouble(Dt_LimitCheck.Rows[0]["limit"].ToString()))
                                            {
                                                tdsper = Convert.ToDouble(Txt.Text.ToString());
                                                AmtWidExem = diff - Convert.ToDouble(Dt_LimitCheck.Rows[0]["limit"].ToString());
                                                AmtwitoutTds = Convert.ToDouble((AmtWidExem * (tdsper / 100)).ToString("#0.00"));
                                                tdsemp = Convert.ToDouble(Dt_LimitCheck.Rows[0]["tdsemp"].ToString());
                                                AmtWidTds = Convert.ToDouble(((Amount - AmtWidExem) * (tdsemp / 100)).ToString("#0.00"));
                                                TDSAmount = AmtwitoutTds + AmtWidTds;
                                                //log1.Info("Before TDS Amount < 0- (PANO- " + int_Invoiceno + " | Branchid- " + int_bid + " |EmployeeID- " + int_Empid + " |Vouyear- " + int_Vouyear1 + ")");

                                            }
                                            else
                                            {
                                                TDS = Convert.ToDouble(Dt_LimitCheck.Rows[0]["tdsemp"].ToString());
                                                TDSAmount = Convert.ToDouble((Amount * (TDS / 100)).ToString("#0.00"));
                                                //log1.Info("Before TDS Amount = 0- (PANO- " + int_Invoiceno + " | Branchid- " + int_bid + " |EmployeeID- " + int_Empid + " |Vouyear- " + int_Vouyear1 + ")");

                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToDouble(Txt.Text.ToString()) == 0.0)
                                        {
                                            TDSAmount = 0;
                                        }
                                        else
                                        {
                                            TDS = Convert.ToDouble(Txt.Text.ToString());
                                            //  TDSAmount = Amount * (TDS / 100);
                                            TDSAmount = Convert.ToDouble((Amount * (TDS / 100)).ToString("#0.00"));
                                        }
                                    }

                                }
                                else
                                {
                                    //DataTable dcon = Appobj.Checkcountry(Convert.ToInt32(row.Cells[0].Text.ToString()));
                                    //int con = Convert.ToInt32(dcon.Rows[0]["countryid"]);
                                    if (con == 1102 || con == 102)
                                    {
                                        ///////////haribalaji ////////2924---------------
                                        if (Txt.Text.ToString() == "")
                                        {
                                            Txt.Text = "0";
                                        }
                                        ///////////haribalaji ////////2924---------------
                                        if (Convert.ToDouble(Txt.Text.ToString()) == 0.0)
                                        {
                                            TDSAmount = 0;
                                        }
                                        else
                                        {
                                            TDS = Convert.ToDouble(Txt.Text.ToString());
                                            // TDSAmount = Amount * (TDS / 100);
                                            TDSAmount = Convert.ToDouble((Amount * (TDS / 100)).ToString("#0.00"));
                                        }
                                    }

                                }

                                //log1.Info("Before TDS insert  - (PANO- " + int_Invoiceno + " | Branchid- " + int_bid + " |EmployeeID- " + int_Empid + " |Vouyear- " + int_Vouyear1 + ")");

                                CSTAmount = Amount - TDSAmount;

                                if (str_Voutype == "S")
                                {
                                    obj_dt = obj_da_Invoice.GetPartyLedger4PAAdmin(int_DCno, "C", int.Parse(Session["LoginBranchid"].ToString()), int_Vouyear1);
                                }
                                for (int i = 0; i <= obj_dt.Rows.Count - 1; i++)
                                {
                                    int int_Ledgerid = 0;
                                    int_Ledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(int.Parse(obj_dt.Rows[i]["chargeid"].ToString()), "A", Session["FADbname"].ToString());
                                    if (int_Ledgerid == 0)
                                    {
                                        ChkLedger = false;
                                    }
                                }
                                string Str_ddlVoucherType = "", Str_ddlNarration = "", Str_ddlReference = "";
                                if (hid_type.Value.ToString() == "PURCHASE INVOICE")
                                {
                                    str_Voutype = "P";              //CN-OPS  -->P  //Admin-CN-->S//  Other-CN-->E
                                    type = "P";
                                }


                                //log1.Info("Before TDS insert-1  - (PANO- " + int_Invoiceno + " | Branchid- " + int_bid + " |EmployeeID- " + int_Empid + " |Vouyear- " + int_Vouyear1 + ")");

                                if (ChkLedger == true)
                                {
                                    if ((countryid == 1102) || (countryid == 102))
                                    {
                                        //log1.Info("Before TDS insert-2  - (PANO- " + int_Invoiceno + " | Branchid- " + int_bid + " |EmployeeID- " + int_Empid + " |Vouyear- " + int_Vouyear1 + ")");

                                        obj_da_Invoice.InsertPATDS(int_DCno, str_Voutype, int.Parse(Session["LoginBranchid"].ToString()), int_Custid, int_Vouyear1, CSTAmount, TDSAmount, "", Convert.ToDouble(Txt.Text.ToString()));
                                        //log1.Info("Before TDS insert-3  - (PANO- " + int_Invoiceno + " | Branchid- " + int_bid + " |EmployeeID- " + int_Empid + " |Vouyear- " + int_Vouyear1 + ")");

                                    }
                                    //if (hid_type.Value.ToString() == "Transfer To Commercial PA")
                                    // {
                                    //     str_Voutype="P";
                                    //     type = "P";
                                    // }

                                    if (str_Voutype == "P")
                                    {
                                        Str_ddlVoucherType = "Credit Note - Operations";
                                        Str_ddlNarration = "Vessel/Voyage/Container";
                                        Str_ddlReference = "BL No";
                                    }
                                    else if (str_Voutype == "E")
                                    {
                                        Str_ddlVoucherType = "Credit Note - Others";
                                        Str_ddlNarration = "Vessel/Voyage/Container";
                                        Str_ddlReference = "BL No";
                                    }
                                    else if (str_Voutype == "S")
                                    {
                                        Str_ddlVoucherType = "Admin Purchase Invoice";
                                        Str_ddlNarration = "Remarks";
                                        Str_ddlReference = "Ref No";
                                    }//raj

                                    //logix.CommanClass.TallyEDIFA.Fn_FATransfer(Str_ddlVoucherType, int_DCno, int_DCno, Str_ddlNarration, Str_ddlReference, Convert.ToInt32(Session["LoginBranchid"]));

                                    int int_Ledgerid = 0;
                                    int_Ledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(int_Custid, "C", Session["FADbname"].ToString());
                                    int_Voutypeid = obj_da_FAVoucher.Selvoutypeid(Str_ddlVoucherType, Session["FADbname"].ToString());
                                    if (int_Ledgerid == 0)
                                    {
                                        int_Ledgerid = Fn_Getcustomergroupid(int_Custid, Str_ddlVoucherType);
                                    }
                                    //DateTime dtdate = DateTime.Parse(Utility.fn_ConvertDate(Grd_Approval.Rows[row.RowIndex].Cells[12].Text));
                                    DateTime dtdate = obj_da_Log.GetDate();     //NewOne       //21/07/2022
                                    string Str_CustType = obj_da_Customer.GetCustomerType(int_Custid);
                                    if (Str_CustType == "P" || Str_CustType == "E")
                                    {
                                        DataTable dt1 = new DataTable();
                                        dt1 = obj_da_Invoice.GetOtherDCNAmount(int_DCno, "CNHead", int.Parse(Session["LoginBranchid"].ToString()), int.Parse(Session["Vouyear"].ToString()));
                                        string Str_Curr = "";
                                        double F_Curr = 0;
                                        if (dt1.Rows.Count > 0)
                                        {
                                            Str_Curr = dt1.Rows[0]["curr"].ToString();
                                            F_Curr = double.Parse(dt1.Rows[0]["amt"].ToString());
                                        }
                                        obj_da_Approval.InsLedgerOPBreakup(int_Ledgerid, int_DCno, dtdate, char.Parse(type), int.Parse(Session["Vouyear"].ToString()), int.Parse(Session["LoginBranchid"].ToString()), CSTAmount, Str_Curr, F_Curr, int_Custid);
                                    }
                                    else
                                    {

                                        obj_da_Approval.InsLedgerOPBreakup(int_Ledgerid, int_DCno, dtdate, char.Parse(type), int.Parse(Session["Vouyear"].ToString()), int.Parse(Session["LoginBranchid"].ToString()), CSTAmount, "", 0, int_Custid);
                                    }
                                }

                                //str_favourname = row.Cells[2].Text.ToString();
                                //obj_da_Cheque.UpdChequeRequest(int_Invoiceno, int.Parse(Session["Vouyear"].ToString()), Session["StrTranType"].ToString(), int.Parse(Session["LoginBranchid"].ToString()), int_Empid, "PA", char.Parse("C"), "", str_favourname);

                            }


                            /******************* For Auto mail *********************/
                            if (hid_type.Value.ToString() == "DN")
                            {
                                logix.CommanClass.TallyEDIFA.Fn_AutoMailVouchers(Convert.ToInt32(hid_refno.Value), "Pro-AdminDN", int_bid, Convert.ToInt32(hid_vouyear.Value), Session["FADbname"].ToString(), obj_da_Log.GetDate());
                            }
                            else if (hid_type.Value.ToString() == "PA" || hid_type.Value.ToString() == "CN")
                            {
                                logix.CommanClass.TallyEDIFA.Fn_AutoMailVouchers(Convert.ToInt32(hid_refno.Value), "Pro-AdminCN", int_bid, Convert.ToInt32(hid_vouyear.Value), Session["FADbname"].ToString(), obj_da_Log.GetDate());
                            }
                            /******************************************************/

                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(btn_transfer, typeof(Button), "UploadDocument", "alertify.alert('Kindly attached PDF for this Ref #');", true);
                            StrScript += "Kindly attached Vendor Document (PDF) for this Ref # : " + hid_refno.Value + "";
                          //  return;
                        }
                    }

                }

                if (int_DCno != 0)
                {
                    string Str_DNNO = Lst_DNno;

                    if (Str_DNNO != "")
                    {
                        string last = Str_DNNO.Substring(Str_DNNO.Length - 1, 1);

                        if (last == ",")
                        {
                            Str_DNNO = Str_DNNO.Substring(0, Str_DNNO.Length - 1);
                        }
                    }

                    if (chargename.Length > 0)
                    {
                        StrScript += "LedgerName Not Found in Financial for charge " + chargename + ". ";
                    }

                    if (gsttype == 1)
                    {
                        StrScript += "GST TYPE not Updated for the Customer Name :" + gsttype_ + ". ";
                    }

                    if (supplyto == 1)
                    {
                        StrScript += "State Name not Updated in Master Kindly update Master Customer for" + supplyto_ + ". ";
                    }

                    if (statename == 1)
                    {
                        StrScript += "Kindly Update SupplyTo Customer for the Voucher # " + statename_ + ". ";
                    }

                    if (hid_type.Value.ToString() == "DN")
                    {
                        StrScript += "DN # " + Str_DNNO + " Generated and Transfered" + ". ";
                    }
                    else
                    {
                        StrScript += "CN # " + Str_DNNO + " Generated and Transfered" + ". ";
                    }

                    if (hid_type.Value.ToString() == "DN")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AdminDNCNApproval", "alertify.alert('" + StrScript + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AdminDNCNApproval", "alertify.alert('" + StrScript + "');", true);
                    }

                    Fn_LoadDetail();
                }
                else
                {
                    if (gsttype == 1)
                    {
                        StrScript += "GST TYPE not Updated for the Customer Name :" + gsttype_ + ". ";
                    }
                    if (statename == 1)     
                    {
                        StrScript += "State Name not Updated in Master Kindly update Master Customer for" + statename_ + ". ";
                    }
                    if (supplyto == 1)
                    {
                        StrScript += "Kindly Update SupplyTo Customer for the Voucher # " + supplyto_ + ". ";
                    }
                    if (chargename.Length > 0)
                    {
                        StrScript += "LedgerName Not Found in Financial for charge " + chargename + ". ";
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AdminDNCNApproval", "alertify.alert('" + StrScript + "');", true);
                    Fn_LoadDetail();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.WebControls.Button), "logix", "alertify.alert('" + message + "');", true);
            }
        }

        protected void btn_backdated_Click(object sender, EventArgs e)
        {
            try
            {

                int int_Empid = 0, int_bid = 0, int_divisionid = 0, int_intdcno = 0, int_Vouyear = 0, int_DCno = 0, int_Voutypeid = 0, preparedby = 0;
                string Str_Trantype = Session["StrTranType"].ToString();
                int_Empid = int.Parse(Session["LoginEmpId"].ToString());
                int_bid = int.Parse(Session["LoginBranchid"].ToString());
                int_divisionid = int.Parse(Session["LoginDivisionId"].ToString());
                DataTable obj_dt = new DataTable();
                //DataAccess.Accounts.Approval obj_da_Approval = new //DataAccess.Accounts.Approval();
                //DataAccess.Accounts.Invoice obj_da_Invoice = new //DataAccess.Accounts.Invoice();
                //DataAccess.FAMaster.MasterLedger obj_da_Ledger = new //DataAccess.FAMaster.MasterLedger();
                //DataAccess.LogDetails obj_da_Log = new //DataAccess.LogDetails();
                //DataAccess.FAVoucher obj_da_FAVoucher = new //DataAccess.FAVoucher();
                //DataAccess.Accounts.ProAdminDCNNo ProDCNObj = new //DataAccess.Accounts.ProAdminDCNNo();
                //DataAccess.Masters.MasterEmployee Obj_Emp = new //DataAccess.Masters.MasterEmployee();
                bool flag = true;
                string StrScript = "";

                /******************* For Auto mail *********************/
                int Vyraer = Convert.ToInt32(HttpContext.Current.Session["Vouyear"]);

                string FAYear1;
                int vyear1;
                vyear1 = Vyraer;
                FAYear1 = vyear1.ToString();
                FAYear1 = FAYear1.Substring(2, 2);
                vyear1 = vyear1 + 1;
                FAYear1 = Convert.ToInt32(FAYear1) - 1 + Convert.ToString(vyear1 - 1).Substring(2, 2);
                string Str_DBname = "FA" + FAYear1;
                /******************************************************/



                foreach (GridViewRow row in Grd_Approval.Rows)
                {
                    CheckBox Chk = (CheckBox)row.FindControl("Chk_Approval");
                    if (Chk.Checked == true)
                    {
                        //int_Vouyear = int.Parse(Grd_Approval.DataKeys[row.RowIndex].Values[0].ToString());
                        int_intdcno = int.Parse(Grd_Approval.DataKeys[row.RowIndex].Values[1].ToString());

                        //DataAccess.FAVoucher obj_da_FA = new //DataAccess.FAVoucher();
                        int_Vouyear = Convert.ToInt32(obj_da_FA.Getvouyearforautotransfer(int_bid).ToString());

                        preparedby = Obj_Emp.GetNEmpid(Grd_Approval.Rows[row.RowIndex].Cells[4].Text);

                        /******************* For Auto mail *********************/
                        hid_refno.Value = row.Cells[0].Text.ToString();
                        //hid_vouyear.Value = Grd_Approval.DataKeys[row.RowIndex].Values[0].ToString();
                        hid_vouyear.Value = obj_da_FAVoucher.Getvouyearforautotransfer(int_bid).ToString();
                        /****************************************/

                        hid_stamt.Value = Grd_Approval.Rows[row.RowIndex].Cells[6].Text;
                        hid_supplyto.Value = Grd_Approval.Rows[row.RowIndex].Cells[7].Text;


                        if (preparedby == int_Empid)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "alertify.alert('You have no rights to approve Voucher # " + int_intdcno + " prepared by you')", true);
                            continue;
                        }

                        /*if (Session["hid_gstdate"] != null)
                        {
                            //if (hid_custtype.Value == "C")
                            //{
                            if (Convert.ToDateTime(obj_da_Log.GetDate().ToShortDateString()) >= Convert.ToDateTime(Session["hid_gstdate"].ToString()))
                            {

                                if (hid_supplyto.Value != "0")
                                {
                                    if (Convert.ToDouble(hid_stamt.Value) > 0)
                                    {

                                        int int_custidnew;
                                        DataTable dt_list = new DataTable();
//DataAccess.Masters.MasterCustomer customerobj = new //DataAccess.Masters.MasterCustomer();

                                        //int int_custid = Convert.ToInt32(hdncustid.Value);
                                        if (!string.IsNullOrEmpty(row.Cells[7].Text.ToString()))
                                        {
                                            int_custidnew = Convert.ToInt32(row.Cells[7].Text.ToString());
                                            dt_list = customerobj.GetIndianCustomergstadd(int_custidnew);
                                        }


                                        if (dt_list.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dt_list.Rows[0]["stateid"].ToString()))
                                            {

                                            }
                                            else
                                            {
                                                StrScript = "State Name not Updated in Master Kindly update Master Customer ";
ScriptManager.RegisterStartupScript(btn_Approve, typeof(Button), "Costing", "alertify.alert('" + StrScript + "');", true);
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            StrScript = "State Name not Updated in Master Kindly update Master Customer";
ScriptManager.RegisterStartupScript(btn_Approve, typeof(Button), "Costing", "alertify.alert('" + StrScript + "');", true);

                                            continue;
                                        }


                                    }
                                }
                                else
                                {
                                    StrScript = "Kindly Update SupplyTo Customer for the Voucher # " + int_intdcno;
ScriptManager.RegisterStartupScript(btn_Approve, typeof(Button), "Costing", "alertify.alert('" + StrScript + "');", true);

                                    continue;
                                }
                            }
                            //}
                        }*/



                        if (hid_type.Value.ToString() == "DN")
                        {
                            obj_dt = obj_da_Invoice.GetPartyLedger4PAPROAdminwithCust(int_intdcno, "D", int_bid, int_Vouyear);
                        }

                        if (obj_dt.Rows.Count > 0)
                        {
                            for (int i = 0; i <= obj_dt.Rows.Count - 1; i++)
                            {
                                chkledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(int.Parse(obj_dt.Rows[i]["chargeid"].ToString()), obj_dt.Rows[i]["opstype"].ToString(), Session["FADbname"].ToString());
                                if (chkledgerid == 0)
                                {
                                    flag = false;
                                }
                            }
                        }

                        //DataAccess.Accounts.AdminDCNNo obj_da_AdminDNCN = new //DataAccess.Accounts.AdminDCNNo();
                        //DataAccess.Accounts.ProAdminDCNNo obj_da_ProAdminDNCN = new //DataAccess.Accounts.ProAdminDCNNo();
                        if (flag == true)
                        {
                            if (hid_type.Value.ToString() == "DN")
                            {
                                int_DCno = obj_da_AdminDNCN.GetAdmDNnonew(int_bid);
                            }
                            else
                            {
                                int_DCno = obj_da_AdminDNCN.GetAdmCNnonew(int_bid);
                            }

                            obj_da_ProAdminDNCN.UpdApprovalProAdminDCNopsnew(int_DCno, int_Empid, hid_type.Value.ToString(), int_Vouyear, int_bid, int_intdcno);


                            if (hid_type.Value.ToString() == "PA")
                            {
                                obj_da_Log.InsLogDetail(int_Empid, 1049, 1, int_bid, hid_type.Value.ToString() + "# - " + int_intdcno + "/Approve");
                            }
                            else
                            {
                                obj_da_Log.InsLogDetail(int_Empid, 1050, 1, int_bid, hid_type.Value.ToString() + "# - " + int_intdcno + "/Approve");
                            }


                            Lst_DNno += int_DCno + ",";
                        }

                        if (flag == true)
                        {
                            if (hid_type.Value.ToString() == "DN")
                            {
                                //raj
                                logix.CommanClass.TallyEDIFA.Fn_FATransfer("Admin Sales Invoice", int_DCno, int_DCno, "Remarks", "Ref No", int_bid, "", 0, 0, "", -1);

                                try
                                {
                                    obj_dt = obj_da_Invoice.FAShowTallyDt(int_DCno, "DN-Admin", int.Parse(Session["Vouyear"].ToString()), int_bid);

                                    if (obj_dt.Rows.Count > 0)
                                    {
                                        int int_custid = int.Parse(obj_dt.Rows[0].ItemArray[4].ToString());

                                        DateTime date_Voudate = Convert.ToDateTime(obj_dt.Rows[0].ItemArray[1].ToString());
                                        int int_Ledgerid = 0;
                                        int_Ledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(int_custid, "C", Session["FADbname"].ToString());

                                        if (int_Ledgerid == 0)
                                        {
                                            int_Ledgerid = Fn_Getcustomergroupid(int_custid);
                                        }
                                        int_Voutypeid = obj_da_FAVoucher.Selvoutypeid("Admin Sales Invoice", Session["FADbname"].ToString());
                                        obj_da_Approval.InsLedgerOPBreakup(int_Ledgerid, int_DCno, Convert.ToDateTime(date_Voudate.ToShortDateString()), 'X', int_Vouyear, int_bid, double.Parse(row.Cells[3].Text.ToString()), "", 0.0, int_custid);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else if (hid_type.Value.ToString() == "PA")
                            {
                                DataTable dt_Credit = new DataTable();
                                dt_Credit = obj_da_Approval.GetAgentCustomerOrNot(int_DCno, int_Vouyear, int_bid, "AC");

                                if (dt_Credit.Rows.Count > 0)
                                {
                                    customerid = Convert.ToInt32(dt_Credit.Rows[0]["customerid"].ToString());
                                    logix.CommanClass.TallyEDIFA.Fn_FATransfer("Admin Purchase Invoice", int_DCno, int_DCno, "Remarks", "Ref No", int_bid, "", 0, 0, "", -1);

                                    try
                                    {
                                        DateTime padat, Vou_Date;
                                        DataTable DtSHead = new DataTable();
                                        DataTable dcndt = new DataTable();
                                        int custid = 0;
                                        DtSHead = obj_da_Invoice.FAShowTallyDt(int_DCno, "PA-Admin", int_Vouyear, int_bid);
                                        if (DtSHead.Rows.Count > 0)
                                        {
                                            custid = Convert.ToInt32(DtSHead.Rows[0]["customerid"].ToString());
                                        }
                                        int chkledgerid = 0;

                                        chkledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(customerid, "C", Session["FADbname"].ToString());
                                        if (chkledgerid == 0)
                                        {
                                            chkledgerid = Fn_Getcustomergroupid(custid);
                                        }
                                        int_Voutypeid = obj_da_FAVoucher.Selvoutypeid("Admin Purchase Invoice", Session["FADbname"].ToString());

                                        string custtype = "", fcur = "";
                                        double famt = 0.00, exrate;

                                        //DataAccess.Masters.MasterCustomer obj_da_Customer = new //DataAccess.Masters.MasterCustomer();
                                        custtype = obj_da_Customer.GetCustomerType(customerid);

                                        if (custtype == "P")
                                        {
                                            dcndt = obj_da_Invoice.GetOtherDCNAmount(int_DCno, "ACNHead", int_bid, int_Vouyear);

                                            fcur = "";
                                            famt = 0.0;
                                            exrate = 0.0;
                                            if (dcndt.Rows.Count > 0)
                                            {
                                                fcur = dcndt.Rows[0]["curr"].ToString();
                                                famt = Convert.ToDouble(dcndt.Rows[0]["amt"].ToString());
                                                exrate = Convert.ToDouble(dcndt.Rows[0]["exrate"].ToString());
                                            }
                                            obj_da_Approval.InsLedgerOPBreakup(chkledgerid, int_DCno, Convert.ToDateTime(DtSHead.Rows[0]["cndate"].ToString()), 'S', int_Vouyear, int_bid, Convert.ToDouble(row.Cells[3].Text.ToString()), fcur, famt, customerid);
                                        }
                                        else
                                        {
                                            obj_da_Approval.InsLedgerOPBreakup(chkledgerid, int_DCno, Convert.ToDateTime(DtSHead.Rows[0]["cndate"].ToString()), 'S', int_Vouyear, int_bid, Convert.ToDouble(row.Cells[3].Text.ToString()), "", 0.0, customerid);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }

                        }
                        /******************* For Auto mail *********************/
                        if (hid_type.Value.ToString() == "DN")
                        {
                            logix.CommanClass.TallyEDIFA.Fn_AutoMailVouchers(Convert.ToInt32(hid_refno.Value), "Pro-AdminDN", int_bid, Vyraer, Str_DBname, obj_da_Log.GetDate());
                        }
                        else if (hid_type.Value.ToString() == "PA" || hid_type.Value.ToString() == "CN")
                        {
                            logix.CommanClass.TallyEDIFA.Fn_AutoMailVouchers(Convert.ToInt32(hid_refno.Value), "Pro-AdminCN", int_bid, Vyraer, Str_DBname, obj_da_Log.GetDate());
                        }
                        /******************************************************/

                    }
                }

                if (int_DCno != 0)
                {
                    string Str_DNNO = Lst_DNno;

                    if (Str_DNNO != "")
                    {
                        string last = Str_DNNO.Substring(Str_DNNO.Length - 1, 1);

                        if (last == ",")
                        {
                            Str_DNNO = Str_DNNO.Substring(0, Str_DNNO.Length - 1);
                        }
                    }

                    if (hid_type.Value.ToString() == "DN")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AdminDNCNApproval", "alertify.alert('DN # " + Str_DNNO + " Generated and Transfered');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AdminDNCNApproval", "alertify.alert('CN # " + Str_DNNO + " Generated and Transfered');", true);
                    }
                    Fn_LoadDetail();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.WebControls.Button), "logix", "alertify.alert('" + message + "');", true);
            }
        }
        private int Fn_Getcustomergroupid(int int_Custid)
        {
            int int_Subgroupid = 0, int_Groupid = 0, int_Ledgerid = 0;
            //DataAccess.Masters.MasterCustomer obj_da_Customer = new //DataAccess.Masters.MasterCustomer();
            //DataAccess.FAMaster.MasterLedger obj_da_Ledger = new //DataAccess.FAMaster.MasterLedger();
            if (hid_type.Value.ToString() == "DN")
            {
                int_Subgroupid = 40;
                int_Groupid = 13;

                int_Ledgerid = obj_da_Ledger.InsLedgerHeadfromTally(obj_da_Customer.GetCustomername(int_Custid), int_Subgroupid, int_Groupid, 'G', int_Custid, 'C', Session["FADbname"].ToString());
            }
            return int_Ledgerid;
        }

        protected void btn_cancel_Click(object sender, EventArgs e)
        {
            if (btn_cancel.ToolTip == "Cancel")
            {
                if (Session["Loginyear"].ToString() == Session["Vouyear"].ToString())
                {

                }
                else
                {
                    btn_Approve.Visible = false;
                }
                Grd_Approval.DataSource = Utility.Fn_GetEmptyDataTable();
                Grd_Approval.DataBind();
                btn_cancel.Text = "Back";
                btn_cancel.ToolTip = "Back";
                btn_cancel1.Attributes["class"] = "btn ico-back";
            }
            else
            {
                // this.Response.End();

                if (Session["StrTranType"] != null)
                {
                    if (Session["StrTranType"].ToString() == "CO")
                    {
                        Response.Redirect("../Home/CorporateHome.aspx");
                    }
                    else if (Session["StrTranType"].ToString() == "BR")
                    {
                        Response.Redirect("../Home/Branch_home.aspx");

                    }
                    else if (Session["StrTranType"].ToString() == "AC")
                    {
                        Response.Redirect("../Home/Branch_home.aspx");
                    }
                    else if (Session["home"] != null)
                    {
                        if (Session["home"].ToString() == "FABR")
                        {
                            Response.Redirect("../Home/Branch_home.aspx");
                        }
                        else if (Session["home"].ToString() == "FAFC")
                        {
                            Response.Redirect("../Home/CorporateHome.aspx");
                        }
                    }

                }
                else if (Session["home"] != null)
                {
                    if (Session["home"].ToString() == "FABR")
                    {
                        Response.Redirect("../Home/Branch_home.aspx");
                    }
                    else if (Session["home"].ToString() == "FAFC")
                    {
                        Response.Redirect("../Home/CorporateHome.aspx");
                    }
                }
                else
                {
                    this.Response.End();
                }
            }
        }

        protected void Grd_Approval_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (e.Row.Cells[i].Text == "&nbsp;")
                    {
                        e.Row.Cells[i].Text = "";
                    }
                    e.Row.Cells[i].ToolTip = HttpUtility.HtmlDecode(e.Row.Cells[i].Text);
                }
            }
        }


        protected void btn_backdated1_Click(object sender, EventArgs e)
        {
            try
            {
                //DataAccess.FAVoucher obj_da_FA = new //DataAccess.FAVoucher();
                int gsttype = 0, statename = 0, supplyto = 0;
                string gsttype_ = "", statename_ = "", supplyto_ = "";
                int int_Empid = 0, int_bid = 0, int_divisionid = 0, int_intdcno = 0, int_Vouyear = 0, int_DCno = 0, int_Voutypeid = 0, preparedby = 0;
                string Str_Trantype = Session["StrTranType"].ToString();
                int_Empid = int.Parse(Session["LoginEmpId"].ToString());
                int_bid = int.Parse(Session["LoginBranchid"].ToString());
                int_divisionid = int.Parse(Session["LoginDivisionId"].ToString());
                DataTable obj_dt = new DataTable();
                //DataAccess.Accounts.AdminDCNNo obj_da_AdminDNCN = new //DataAccess.Accounts.AdminDCNNo();
                //DataAccess.Accounts.ProAdminDCNNo obj_da_ProAdminDNCN = new //DataAccess.Accounts.ProAdminDCNNo();
                //DataAccess.Accounts.Approval obj_da_Approval = new //DataAccess.Accounts.Approval();
                //DataAccess.Accounts.Invoice obj_da_Invoice = new //DataAccess.Accounts.Invoice();
                //DataAccess.FAMaster.MasterLedger obj_da_Ledger = new //DataAccess.FAMaster.MasterLedger();
                //DataAccess.LogDetails obj_da_Log = new //DataAccess.LogDetails();
                //DataAccess.FAVoucher obj_da_FAVoucher = new //DataAccess.FAVoucher();
                //DataAccess.Accounts.ProAdminDCNNo ProDCNObj = new //DataAccess.Accounts.ProAdminDCNNo();
                //DataAccess.Masters.MasterEmployee Obj_Emp = new //DataAccess.Masters.MasterEmployee();
                //DataAccess.Accounts.Reversal obj_da_Reversal = new //DataAccess.Accounts.Reversal();
                //DataAccess.LogDetails da_obj_Log = new //DataAccess.LogDetails();
                bool flag = true;
                string StrScript = "", chargename = "";
                // string dbname="";
                // string str = Session["FADbname"].ToString();
                //string year = Session["Loginyear"].ToString();
                string str_dispyear, FADbname, Logyear;
                DateTime startdate, todate, dt_Date;
                int year;
                dt_Date = da_obj_Log.GetDate().AddDays(-1);
                //LoginEmpId = 99999;
                if (dt_Date.Month > 3)
                {
                    year = dt_Date.Year - 1;
                }
                else
                {
                    year = dt_Date.Year - 2;
                }
                str_dispyear = Convert.ToString(year);
                str_dispyear = str_dispyear.Substring(2, 2);
                int dy;
                string dy1 = "";

                dy = Convert.ToInt32(str_dispyear) + 1;

                if (dy < 10)
                {
                    dy1 = dy1 + "0" + Convert.ToString(dy);
                }
                else
                {
                    dy1 = Convert.ToString(dy);

                }
                str_dispyear = str_dispyear + dy1;
                FADbname = "FA" + str_dispyear;
                //Logyear = "20" + str_dispyear.Substring(0, 2);
                //startdate = Convert.ToDateTime("04/01/" + Logyear);
                //todate = Convert.ToDateTime("03/31/" + (Convert.ToInt32(Logyear) + 1));
                int BookClosure_Status = obj_da_Reversal.CheckBookClosureStatus(Convert.ToInt32(Session["LoginBranchid"]), Convert.ToInt32(Session["LoginDivisionId"]), FADbname);
                if (BookClosure_Status > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(TextBox), "Reversal", "alertify.alert('Book Closed for the Financial Year.');", true);
                    return;
                }


                /******************* For Auto mail *********************/
                int Vyraer = Convert.ToInt32(HttpContext.Current.Session["Vouyear"]);

                string FAYear1;
                int vyear1;
                vyear1 = Vyraer;
                FAYear1 = vyear1.ToString();
                FAYear1 = FAYear1.Substring(2, 2);
                vyear1 = vyear1 + 1;
                FAYear1 = Convert.ToInt32(FAYear1) - 1 + Convert.ToString(vyear1 - 1).Substring(2, 2);
                string Str_DBname = "FA" + FAYear1;
                /******************************************************/

                foreach (GridViewRow row in Grd_Approval.Rows)
                {
                    CheckBox Chk = (CheckBox)row.FindControl("Chk_Approval");
                    if (Chk.Checked == true)
                    {

                        int_Vouyear = int.Parse(Grd_Approval.DataKeys[row.RowIndex].Values[0].ToString());


                        ///int_Vouyear -= 1;
                        int_intdcno = int.Parse(Grd_Approval.DataKeys[row.RowIndex].Values[1].ToString());
                        preparedby = Obj_Emp.GetNEmpid(Grd_Approval.Rows[row.RowIndex].Cells[4].Text);
                        hid_stamt.Value = Grd_Approval.Rows[row.RowIndex].Cells[6].Text;
                        hid_supplyto.Value = Grd_Approval.Rows[row.RowIndex].Cells[7].Text;

                        int_Vouyear = Convert.ToInt32(obj_da_FA.Getvouyearforautotransfer(int_bid).ToString());
                        //  hid_custtype1.Value = Grd_Approval.Rows[row.RowIndex].Cells[2].Text;


                        /******************* For Auto mail *********************/
                        bool bos = false;
                        /*******************************************************/


                        /******************* For Auto mail *********************/
                        hid_refno.Value = row.Cells[0].Text.ToString();
                        //hid_vouyear.Value = Grd_Approval.DataKeys[row.RowIndex].Values[0].ToString();
                        hid_vouyear.Value = obj_da_FAVoucher.Getvouyearforautotransfer(int_bid).ToString();
                        /****************************************/

                        if (preparedby == int_Empid)
                        {
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), "alertify.alert('You have no rights to approve Voucher # " + int_intdcno + " prepared by you')", true);
                            //continue;
                        }

                        if (Session["hid_gstdate"] != null)
                        {

                            if (Convert.ToDateTime(obj_da_Log.GetDate().ToShortDateString()) >= Convert.ToDateTime(Session["hid_gstdate"].ToString()))
                            {
                                if (hid_supplyto.Value != "0")
                                {
                                    if (Convert.ToDouble(hid_stamt.Value) > 0)
                                    {
                                        int int_custidnew;
                                        DataTable dt_list = new DataTable();
                                        //DataAccess.Masters.MasterCustomer customerobj = new //DataAccess.Masters.MasterCustomer();

                                        if (!string.IsNullOrEmpty(row.Cells[10].Text.ToString()))
                                        {
                                            int_custidnew = Convert.ToInt32(row.Cells[10].Text.ToString());
                                            dt_list = customerobj.GetIndianCustomergstadd(int_custidnew);
                                        }

                                        if (dt_list.Rows.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(dt_list.Rows[0]["GSTGroup"].ToString()))
                                            {
                                                if (dt_list.Rows[0]["GSTGroup"].ToString() == "N")
                                                {
                                                    if (gsttype == 0)
                                                    {
                                                        gsttype_ = Grd_Approval.Rows[row.RowIndex].Cells[2].Text;
                                                    }
                                                    else
                                                    {
                                                        gsttype_ = " ," + Grd_Approval.Rows[row.RowIndex].Cells[2].Text;
                                                    }
                                                    gsttype = 1;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                if (statename == 0)
                                                {
                                                    statename_ = Grd_Approval.Rows[row.RowIndex].Cells[2].Text;
                                                }
                                                else
                                                {
                                                    statename_ = " ," + Grd_Approval.Rows[row.RowIndex].Cells[2].Text;
                                                }
                                                statename = 1;
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (statename == 0)
                                            {
                                                statename_ = Grd_Approval.Rows[row.RowIndex].Cells[2].Text;
                                            }
                                            else
                                            {
                                                statename_ = " ," + Grd_Approval.Rows[row.RowIndex].Cells[2].Text;
                                            }
                                            statename = 1;
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    if (supplyto == 0)
                                    {
                                        supplyto_ = int_intdcno.ToString();
                                    }
                                    else
                                    {
                                        supplyto_ = " ," + int_intdcno.ToString();
                                    }
                                    supplyto = 1;
                                    continue;
                                }
                            }
                        }

                        if (hid_type.Value.ToString() == "DN")
                        {
                            obj_dt = obj_da_Invoice.GetPartyLedger4PAPROAdminwithCust(int_intdcno, "D", int_bid, int_Vouyear);
                        }

                        if (obj_dt.Rows.Count > 0)
                        {
                            for (int i = 0; i <= obj_dt.Rows.Count - 1; i++)
                            {
                                chkledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(int.Parse(obj_dt.Rows[i]["chargeid"].ToString()), obj_dt.Rows[i]["opstype"].ToString(), Session["FADbname"].ToString());
                                if (chkledgerid == 0)
                                {
                                    if (chargename == "")
                                    {
                                        chargename = obj_dt.Rows[i]["chargename"].ToString();
                                    }
                                    else
                                    {
                                        if (chargename.Contains(obj_dt.Rows[i]["chargename"].ToString()))
                                        {

                                        }
                                        else
                                        {
                                            chargename += " , " + obj_dt.Rows[i]["chargename"].ToString();
                                        }
                                    }
                                    flag = false;
                                }
                            }
                        }

                        if (flag == true)
                        {
                            if (hid_type.Value.ToString() == "DN")
                            {
                                int_DCno = obj_da_AdminDNCN.GetAdmDNno_backdated(int_bid);
                            }
                            else
                            {
                                int_DCno = obj_da_AdminDNCN.GetAdmCNno_backdated(int_bid);
                            }

                            obj_da_ProAdminDNCN.UpdApprovalProAdminDCN_backdated(int_DCno, int_Empid, hid_type.Value.ToString(), int_Vouyear, int_bid, int_intdcno);


                            if (hid_type.Value.ToString() == "PA")
                            {
                                obj_da_Log.InsLogDetail(int_Empid, 1049, 1, int_bid, hid_type.Value.ToString() + "# - " + int_intdcno + "/Approve");
                            }
                            else
                            {
                                obj_da_Log.InsLogDetail(int_Empid, 1050, 1, int_bid, hid_type.Value.ToString() + "# - " + int_intdcno + "/Approve");
                            }

                            Lst_DNno += int_DCno + ",";
                        }

                        if (flag == true)
                        {
                            if (hid_type.Value.ToString() == "DN")
                            {
                                logix.CommanClass.TallyEDIFA.Fn_FATransfer("Admin Sales Invoice", int_DCno, int_DCno, "Remarks", "Ref No", int_bid, "", 0, 0, "", -1);

                                try
                                {
                                    obj_dt = obj_da_Invoice.FAShowTallyDt(int_DCno, "DN-Admin", int.Parse(Session["Vouyear"].ToString()), int_bid);

                                    if (obj_dt.Rows.Count > 0)
                                    {
                                        int int_custid = int.Parse(obj_dt.Rows[0].ItemArray[4].ToString());

                                        DateTime date_Voudate = Convert.ToDateTime(obj_dt.Rows[0].ItemArray[1].ToString());
                                        int int_Ledgerid = 0;
                                        int_Ledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(int_custid, "C", Session["FADbname"].ToString());

                                        if (int_Ledgerid == 0)
                                        {
                                            int_Ledgerid = Fn_Getcustomergroupid(int_custid);
                                        }
                                        int_Voutypeid = obj_da_FAVoucher.Selvoutypeid("Admin Sales Invoice", Session["FADbname"].ToString());
                                        obj_da_Approval.InsLedgerOPBreakup(int_Ledgerid, int_DCno, Convert.ToDateTime(date_Voudate.ToShortDateString()), 'X', int_Vouyear, int_bid, double.Parse(row.Cells[3].Text.ToString()), "", 0.0, int_custid);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else if (hid_type.Value.ToString() == "PA")
                            {
                                DataTable dt_Credit = new DataTable();
                                dt_Credit = obj_da_Approval.GetAgentCustomerOrNot(int_DCno, int_Vouyear, int_bid, "AC");

                                if (dt_Credit.Rows.Count > 0)
                                {
                                    customerid = Convert.ToInt32(dt_Credit.Rows[0]["customerid"].ToString());
                                    logix.CommanClass.TallyEDIFA.Fn_FATransfer("Admin Purchase Invoice", int_DCno, int_DCno, "Remarks", "Ref No", int_bid, "", 0, 0, "", -1);

                                    try
                                    {
                                        DateTime padat, Vou_Date;
                                        DataTable DtSHead = new DataTable();
                                        DataTable dcndt = new DataTable();
                                        int custid = 0;
                                        DtSHead = obj_da_Invoice.FAShowTallyDt(int_DCno, "PA-Admin", int_Vouyear, int_bid);
                                        if (DtSHead.Rows.Count > 0)
                                        {
                                            custid = Convert.ToInt32(DtSHead.Rows[0]["customerid"].ToString());
                                        }
                                        int chkledgerid = 0;

                                        chkledgerid = obj_da_Ledger.ChkLedgeridfrmLedHead(customerid, "C", Session["FADbname"].ToString());
                                        if (chkledgerid == 0)
                                        {
                                            chkledgerid = Fn_Getcustomergroupid(custid);
                                        }
                                        int_Voutypeid = obj_da_FAVoucher.Selvoutypeid("Admin Purchase Invoice", Session["FADbname"].ToString());

                                        string custtype = "", fcur = "";
                                        double famt = 0.00, exrate;

                                        //DataAccess.Masters.MasterCustomer obj_da_Customer = new //DataAccess.Masters.MasterCustomer();
                                        custtype = obj_da_Customer.GetCustomerType(customerid);

                                        if (custtype == "P")
                                        {
                                            dcndt = obj_da_Invoice.GetOtherDCNAmount(int_DCno, "ACNHead", int_bid, int_Vouyear);

                                            fcur = "";
                                            famt = 0.0;
                                            exrate = 0.0;
                                            if (dcndt.Rows.Count > 0)
                                            {
                                                fcur = dcndt.Rows[0]["curr"].ToString();
                                                famt = Convert.ToDouble(dcndt.Rows[0]["amt"].ToString());
                                                exrate = Convert.ToDouble(dcndt.Rows[0]["exrate"].ToString());
                                            }
                                            obj_da_Approval.InsLedgerOPBreakup(chkledgerid, int_DCno, Convert.ToDateTime(DtSHead.Rows[0]["cndate"].ToString()), 'S', int_Vouyear, int_bid, Convert.ToDouble(row.Cells[3].Text.ToString()), fcur, famt, customerid);
                                        }
                                        else
                                        {
                                            obj_da_Approval.InsLedgerOPBreakup(chkledgerid, int_DCno, Convert.ToDateTime(DtSHead.Rows[0]["cndate"].ToString()), 'S', int_Vouyear, int_bid, Convert.ToDouble(row.Cells[3].Text.ToString()), "", 0.0, customerid);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                        /******************* For Auto mail *********************/
                        if (hid_type.Value.ToString() == "DN")
                        {
                            logix.CommanClass.TallyEDIFA.Fn_AutoMailVouchers(Convert.ToInt32(hid_refno.Value), "Pro-AdminDN", int_bid, Vyraer, Str_DBname, obj_da_Log.GetDate());
                        }
                        else if (hid_type.Value.ToString() == "PA" || hid_type.Value.ToString() == "CN")
                        {
                            logix.CommanClass.TallyEDIFA.Fn_AutoMailVouchers(Convert.ToInt32(hid_refno.Value), "Pro-AdminCN", int_bid, Vyraer, Str_DBname, obj_da_Log.GetDate());
                        }
                        /******************************************************/
                    }
                }

                if (int_DCno != 0)
                {
                    string Str_DNNO = Lst_DNno;

                    if (Str_DNNO != "")
                    {
                        string last = Str_DNNO.Substring(Str_DNNO.Length - 1, 1);

                        if (last == ",")
                        {
                            Str_DNNO = Str_DNNO.Substring(0, Str_DNNO.Length - 1);
                        }
                    }

                    if (chargename.Length > 0)
                    {
                        StrScript += "LedgerName Not Found in Financial for charge " + chargename + ". ";
                    }

                    if (gsttype == 1)
                    {
                        StrScript += "GST TYPE not Updated for the Customer Name :" + gsttype_ + ". ";
                    }

                    if (supplyto == 1)
                    {
                        StrScript += "State Name not Updated in Master Kindly update Master Customer for" + supplyto_ + ". ";
                    }

                    if (statename == 1)
                    {
                        StrScript += "Kindly Update SupplyTo Customer for the Voucher # " + statename_ + ". ";
                    }

                    if (hid_type.Value.ToString() == "DN")
                    {
                        StrScript += "DN # " + Str_DNNO + " Generated and Transfered" + ". ";
                    }
                    else
                    {
                        StrScript += "CN # " + Str_DNNO + " Generated and Transfered" + ". ";
                    }

                    if (hid_type.Value.ToString() == "DN")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AdminDNCNApproval", "alertify.alert('" + StrScript + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AdminDNCNApproval", "alertify.alert('" + StrScript + "');", true);
                    }

                    Fn_LoadDetail();
                }
                else
                {
                    if (gsttype == 1)
                    {
                        StrScript += "GST TYPE not Updated for the Customer Name :" + gsttype_ + ". ";
                    }
                    if (statename == 1)
                    {
                        StrScript += "State Name not Updated in Master Kindly update Master Customer for" + statename_ + ". ";
                    }
                    if (supplyto == 1)
                    {
                        StrScript += "Kindly Update SupplyTo Customer for the Voucher # " + supplyto_ + ". ";
                    }
                    if (chargename.Length > 0)
                    {
                        StrScript += "LedgerName Not Found in Financial for charge " + chargename + ". ";
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "AdminDNCNApproval", "alertify.alert('" + StrScript + "');", true);
                    Fn_LoadDetail();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                ScriptManager.RegisterClientScriptBlock(this, typeof(System.Web.UI.WebControls.Button), "logix", "alertify.alert('" + message + "');", true);
            }
        }

        protected void ddl_voutype_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbl_Header.Text = ddl_voutype.SelectedItem.Text;

           
            //DataAccess.Accounts.AdminDCNNo obj_da_AdminDNCN = new //DataAccess.Accounts.AdminDCNNo();
            //DataAccess.Accounts.ProAdminDCNNo ProDCNObj = new //DataAccess.Accounts.ProAdminDCNNo();
            DataTable obj_dt = new DataTable();
            obj_dt = ProDCNObj.GetApproveProAdminDCNCOM(Convert.ToInt32(ddl_voutype.SelectedValue), int.Parse(Session["LoginBranchid"].ToString()));
            if (ddl_voutype.SelectedValue != "0")
            {
                if (obj_dt.Rows.Count > 0)
                {

                    DataView obj_dtview = new DataView(obj_dt);
                    obj_dtview.RowFilter = "voutype='" + ddl_voutype.SelectedItem.Text + "' ";
                    obj_dt = obj_dtview.ToTable();
                    Grd_Approval.DataSource = obj_dt;
                    Grd_Approval.DataBind();
                }
            }
          
        }
        protected void ddl_section_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //DataAccess.Accounts.Invoice obj_da_Invoice = new //DataAccess.Accounts.Invoice();
                DataTable DT_per = new DataTable();
                DataTable dt = new DataTable();
                int RowIndex = ((GridViewRow)((DropDownList)sender).NamingContainer).RowIndex;
                DropDownList drp_section = ((DropDownList)Grd_Approval.Rows[RowIndex].FindControl("ddl_section"));
                DropDownList drp_section1 = ((DropDownList)Grd_Approval.Rows[RowIndex].FindControl("ddl_section1"));
                TextBox Txt_per = ((TextBox)Grd_Approval.Rows[RowIndex].FindControl("txtpercentage"));
                TextBox TDSdescnew = ((TextBox)Grd_Approval.Rows[RowIndex].FindControl("TDSdescnew"));

                DataTable dlp = new DataTable();
                dlp = obj_da_Invoice.sp_ddltds(drp_section.SelectedItem.Text);

                var ddl2 = (DropDownList)Grd_Approval.Rows[RowIndex].FindControl("ddl_section1");
                ddl2.Items.Clear();
                ddl2.Items.Add("");
                for (int i = 0; i < dlp.Rows.Count; i++)
                {
                    ddl2.Items.Add(dlp.Rows[i]["tdspercentage"].ToString());
                }
            }

            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        protected void ddl_section_SelectedIndexChanged1(object sender, EventArgs e)
        {
            try
            {
                //DataAccess.Accounts.Invoice obj_da_Invoice = new //DataAccess.Accounts.Invoice();
                DataTable DT_per = new DataTable();
                DataTable dt = new DataTable();
                int RowIndex = ((GridViewRow)((DropDownList)sender).NamingContainer).RowIndex;
                DropDownList drp_section = ((DropDownList)Grd_Approval.Rows[RowIndex].FindControl("ddl_section1"));
                TextBox Txt_per = ((TextBox)Grd_Approval.Rows[RowIndex].FindControl("txtpercentage"));
                TextBox TDSdescnew = ((TextBox)Grd_Approval.Rows[RowIndex].FindControl("TDSdescnew"));

                Txt_per.Text = drp_section.SelectedValue;

            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }
        protected void lnkedit_Click(object sender, EventArgs e)
        {
            Button lb = (Button)sender;
            GridViewRow GvRow = (GridViewRow)lb.NamingContainer;
            int Row_ID = GvRow.RowIndex;
            if (Grd_Approval.Rows[Row_ID].Cells[1].Text == "SALES INVOICE" || Grd_Approval.Rows[Row_ID].Cells[1].Text == "PURCHASE INVOICE" || Grd_Approval.Rows[Row_ID].Cells[1].Text == "SALES INVOICE OC" || Grd_Approval.Rows[Row_ID].Cells[1].Text == "PURCHASE INVOICE OC")
            {
                Response.Redirect("../Accounts/ProformaLV.aspx?1voutype=" + Grd_Approval.DataKeys[Row_ID].Values[2].ToString() + "&refno=" + Grd_Approval.Rows[Row_ID].Cells[2].Text + "&1vouyear=" + Grd_Approval.DataKeys[Row_ID].Values[0].ToString());
            }
            else
            {
                Response.Redirect("../Accounts/ProOSV.aspx?voutype=" + Grd_Approval.DataKeys[Row_ID].Values[2].ToString() + "&1refno=" + Grd_Approval.Rows[Row_ID].Cells[2].Text + "&1vouyear=" + Grd_Approval.DataKeys[Row_ID].Values[0].ToString());

            }
        }
        protected void lnkdownload_Click(object sender, EventArgs e)
        {
            Button lb = (Button)sender;
            GridViewRow GvRow = (GridViewRow)lb.NamingContainer;
            int Row_ID = GvRow.RowIndex;
            if (Grd_Approval.Rows[Row_ID].Cells[1].Text == "SALES INVOICE" || Grd_Approval.Rows[Row_ID].Cells[1].Text == "PURCHASE INVOICE" || Grd_Approval.Rows[Row_ID].Cells[1].Text == "SALES INVOICE OC" || Grd_Approval.Rows[Row_ID].Cells[1].Text == "PURCHASE INVOICE OC")
            {
                Response.Redirect("../Accounts/ProformaLV.aspx?rptvoutype=" + Grd_Approval.DataKeys[Row_ID].Values[2].ToString() + "&rptrefno=" + Grd_Approval.Rows[Row_ID].Cells[2].Text + "&rptvouyear=" + Grd_Approval.DataKeys[Row_ID].Values[0].ToString());
            }
            else
            {
                Response.Redirect("../Accounts/ProOSV.aspx?rptvtype=" + Grd_Approval.DataKeys[Row_ID].Values[2].ToString() + "&rptrefno=" + Grd_Approval.Rows[Row_ID].Cells[2].Text + "&rptvouyear=" + Grd_Approval.DataKeys[Row_ID].Values[0].ToString());

            }
        }
        private int Fn_Getcustomergroupid(int int_Custid, string Str_VType)
        {
            int int_Subgroupid = 0, int_Groupid = 0;
            //DataAccess.Masters.MasterCustomer obj_da_Customer = new //DataAccess.Masters.MasterCustomer();
            //DataAccess.FAMaster.MasterLedger obj_da_Ledger = new //DataAccess.FAMaster.MasterLedger();
            if (Str_VType == "Credit Note - Others")
            {
                if (obj_da_Customer.GetCustomerType(int_Custid) == "P")
                {
                    int_Subgroupid = 44;
                    int_Groupid = 12;
                }
                else
                {
                    int_Subgroupid = 67;
                    int_Groupid = 12;
                }
            }
            else if (Str_VType == "Credit Note - Operations")
            {
                int_Subgroupid = 67;
                int_Groupid = 12;
            }
            else if (Str_VType == "Admin Purchase Invoice")
            {
                int_Subgroupid = 41;
                int_Groupid = 12;
            }
            int int_Ledgerid = 0;
            int_Ledgerid = obj_da_Ledger.InsLedgerHeadfromTally(obj_da_Customer.GetCustomername(int_Custid), int_Subgroupid, int_Groupid, 'G', int_Custid, 'C', Session["FADbname"].ToString());

            return int_Ledgerid;
        }
    }
}