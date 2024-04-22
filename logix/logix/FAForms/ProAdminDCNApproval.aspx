<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MasterForm.Master" AutoEventWireup="true" CodeBehind="ProAdminDCNApproval.aspx.cs"
    Inherits="logix.FAForm.ProAdminDCNApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <link href="../Theme/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Theme/bootstrap/css/bootstrap-select.css" />
     <link href="../Theme/assets/css/system.css" rel="stylesheet" />
    <!-- Theme -->
    <link href="../Theme/assets/css/new_style.css" rel="stylesheet" type="text/css" />
    <link href="../Theme/assets/css/main.css" rel="stylesheet" type="text/css" />
    <link href="../Theme/assets/css/plugins.css" rel="stylesheet" type="text/css" />
    <link href="../Theme/assets/css/icons.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../Theme/assets/css/fontawesome/font-awesome.min.css" />
    <link href="../Theme/assets/css/systemFA.css" rel="stylesheet" type="text/css" />
    <link href="../Theme/assets/css/buttonicon.css" rel="stylesheet" />
    <link href="../CSS/Finance.css" rel="stylesheet" />
    <!--=== JavaScript ===-->

    <%--  <script type="text/javascript" src="../Theme/Content/assets/js/libs/jquery-1.10.2.min.js"></script>--%>

    <!-- Smartphone Touch Events -->

    <!-- General -->
    <!-- Polyfill for min/max-width CSS3 Media Queries (only for IE8) -->
    <link href="../Styles/chosen.css" rel="stylesheet" />
    <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
    <link href="../Styles/Chosenlogin.css" rel="stylesheet" />
    <link href="../Styles/DropDownButton.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="../Theme/Content/plugins/slimscroll/jquery.slimscroll.min.js"></script>
    <script type="text/javascript" src="../Theme/Content/plugins/slimscroll/jquery.slimscroll.horizontal.min.js"></script>
       <script type="text/javascript" src="../js/helper.js"></script>
    <script type="text/javascript" src="../js/TextField.js"></script>

    <!-- App -->
      <script type="text/javascript">

          function dropdown(sender, args) {
              $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true });
          }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {



            $('.selectpicker').selectpicker();

            "use strict";

            App.init(); // Init layout and core plugins
            Plugins.init(); // Init all plugins
            FormComponents.init(); // Init all form-specific plugins

            //$('select.styled').customSelect();

        });


    </script>


    <script type="text/javascript">
        function disableBtn(btnID, newText) {
            //initialize to avoid 'Page_IsValid is undefined' JavaScript error
            Page_IsValid = null;
            //check if the page request any validation
            // if yes, check if the page was valid
            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
                //you can pass in the validation group name also
            }
            //variables
            var btn = document.getElementById(btnID);
            var isValidationOk = Page_IsValid;
            /********NEW UPDATE************************************/
            //if not IE then enable the button on unload before redirecting/ rendering
            if (navigator.appName !== 'Microsoft Internet Explorer') {
                EnableOnUnload(btnID, btn.value);
            }
            /***********END UPDATE ****************************/
            // isValidationOk is not null
            if (isValidationOk !== null) {
                //page was valid
                if (isValidationOk) {
                    btn.disabled = true;
                    btn.value = newText;
                    btn.style.background = 'url(~/images/ajax-loader.gif)';
                }
                else {//page was not valid
                    btn.disabled = false;
                }
            }
            else {//the page don't have any validation request
                setTimeout("setImage('" + btnID + "')", 10);
                btn.disabled = true;
                btn.value = newText;
            }
        }

        //set the background image of the button
        function setImage(btnID) {
            var btn = document.getElementById(btnID);
            btn.style.background = 'url(images/Loading.gif)';
        }

        //enable the button and restore the original text value
        function EnableOnUnload(btnID, btnText) {
            window.onunload = function () {
                var btn = document.getElementById(btnID);
                btn.disabled = false;
                btn.value = btnText;
            }
        }
    </script>

    <style type="text/css">
        .hide {
            display: none;
        }

        .div_Grid {
            width: 100%;
            height: 451px;
            float: left;
            margin-left: 0%;
            margin-top: 0%;
            margin-bottom: 0%;
            overflow: auto;
            border: solid 1px #b1b1b1;
        }



        .table-fixed thead {
            float: left;
            display: block;
        }

        .table-fixed tbody {
            height: 410px;
            overflow-y: auto;
            width: 100%;
        }

        .table-fixed thead, .table-fixed tbody, .table-fixed tr, .table-fixed td, .table-fixed th {
            display: inline-block;
        }

            .table-fixed tbody td {
                float: left;
                /*border-bottom-width: 0;*/
                display: block;
            }

            .table-fixed thead > tr > th {
                float: left;
                /*border-bottom-width: 0;*/
                display: block;
            }

                .table-fixed thead > tr > th:last-child {
                    float: left;
                }

            .table-fixed tbody td:last-child {
                float: left;
            }

            .table-fixed tbody td:last-child {
                float: left;
            }

                .table-fixed tbody td:last-child::after {
                    clear: both;
                }
 
        @media only screen and (max-width: 1280px) {
            .CustomerWidth {
                width: 350px !important;
            }

            .PrePareWidth {
                width: 155px !important;
            }
        }
        .widget-header .breadcrumb {
    padding: 0px 20px !important;
}
        .widget.box .widget-content {
    top: 0px !important;
    padding-top: 45px !important;
}
 .BillType1 {
    width: 14.6%;
    float: left;
    margin: 10px 0% 5px 0px;
}
    </style>






    <%--<link href="../Styles/ProAdminDCNApproval.css" rel="stylesheet" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="logix_CPH" runat="server">

    <%-- <!-- Breadcrumbs line -->
          <div class="crumbs">
        <ul id="breadcrumbs" class="breadcrumb">
              <li><i class="icon-home"></i><a href="#"></a>Home </li>
           <li><a href="#" title="" id="lblHead" runat="server"> </a> </li>
              <li><a href="#" title="">Approval</a> </li>
              <li class="current"><a href="#" title="" id="HeaderLabel" runat="server">Profoma CN Approval - Admin</a> </li>
            </ul>
      </div>
          <!-- /Breadcrumbs line -->--%>





    <div >
        <div class="col-md-12">

            <div class="widget box" runat="server">

                <div class="widget-header">
                    <div>
                    <h4 class="hide"><i class="icon-umbrella"></i>
                        <asp:Label ID="lbl_Header" runat="server" Text=""></asp:Label></h4>
                        <!-- Breadcrumbs line -->
    <div class="crumbs">
        <ul id="breadcrumbs" class="breadcrumb">
            <li><i class="icon-home"></i><a href="#"></a>Home </li>
            <li><a href="#">Approval</a> </li>
            <li><a href="#" title="" id="lbl_head" runat="server">Profoma CNApproval - Admin </a></li>
            <li>
                <asp:Label ID="lbnl_logyear" runat="server"></asp:Label></li>
        </ul>
    </div>
                        </div>

                      <div class="FixedButtons">
     
    <div style="float: left;">
        <div class="btn ico-back-dated" id="datedid" runat="server" visible="false"  >
            <asp:Button ID="btn_backdated1" runat="server" Text="Back Dated" ToolTip="Back Dated" OnClick="btn_backdated1_Click"  Visible="false"/>
        </div>
    </div>
    <div class="right_btn">
        <div class="btn btn-servicetax1" style="display: none;">
            <asp:Button ID="btn_backdated" runat="server" Text="Service Tax" ToolTip="Service TAX" OnClick="btn_backdated_Click" Visible="false" />
        </div>

        <div class="btn ico-approve">
            <asp:Button ID="btn_Approve" runat="server"  Text="Approve" ToolTip="Approve" OnClick="btn_Approve_Click" OnClientClick="disableBtn(this.id, 'Loading...')" UseSubmitBehavior="False" /></div>
        <div class="btn ico-cancel" id="btn_cancel1" runat="server">
            <asp:Button ID="btn_cancel" runat="server"  Text="Cancel" ToolTip="Cancel" OnClick="btn_cancel_Click" /></div>
    </div>


</div>


                </div>
                <div class="widget-content">
                      
                    <div class="FormGroupContent4" >
                         <div class="BillType1 blueheighlight">
                            <asp:label id="Labell" runat="server" text="Voucher Type"></asp:label>
                            <asp:dropdownlist id="ddl_voutype" tooltip="Voucher Type" runat="server" autopostback="True" cssclass="chzn-select" width="100%" data-placeholder="Voucher Type" tabindex="3" onselectedindexchanged="ddl_voutype_SelectedIndexChanged">
                               <asp:ListItem Value="0" Text=""></asp:ListItem>
                                  
                                 <asp:ListItem Value="4" Text="Admin Sales Invoice"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Admin Purchase Invoice"></asp:ListItem>


                            </asp:dropdownlist>
                        </div>

                    </div>
                    <div class="FormGroupContent4 boxmodal">

                                                           

                        <div class="gridpnl">
                            <%--class="div_Grid"--%>
                            <asp:GridView ID="Grd_Approval" runat="server" AutoGenerateColumns="False" CssClass="TblGrid FixedHeader"
                                Width="100%" ForeColor="Black" OnRowDataBound="Grd_Approval_RowDataBound"
                                DataKeyNames="vouyear,vouno,voutypeid" ShowHeaderWhenEmpty="True" OnPreRender="Grd_Approval_PreRender" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Sl #">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                            <HeaderStyle Wrap="false" />
                                        </asp:TemplateField><%--1--%>
                                    <asp:BoundField DataField="vouno" HeaderText="ProRef #">
                                        <HeaderStyle Width="100px"  />
                                        <ItemStyle Width="100px"  />
                                    </asp:BoundField><%--2--%>
                                    <asp:BoundField DataField="voutype" HeaderText="Voutype">
                                        <HeaderStyle Width="100px"  />
                                        <ItemStyle Width="100px"  />
                                    </asp:BoundField><%--3--%>
                                    <asp:BoundField DataField="refno" HeaderText="Ref #">
                                        <HeaderStyle Width="300px"  />
                                        <ItemStyle Width="300px"  />
                                    </asp:BoundField><%--4--%>
                                    <asp:BoundField DataField="customer" HeaderText="Customer">
                                        <HeaderStyle Width="400px"  CssClass="CustomerWidth" />
                                        <ItemStyle Width="400px"  CssClass="CustomerWidth" />
                                    </asp:BoundField><%--5--%>
                                    <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="TxtAlign1" HeaderStyle-CssClass="TxtAlign1">
                                        <HeaderStyle Width="200px"  />
                                        <ItemStyle HorizontalAlign="Right" Width="200px"  />
                                    </asp:BoundField><%--6--%>
                                    <asp:BoundField DataField="preparedby" HeaderText="Prepared by">
                                        <HeaderStyle Width="215px"  CssClass="PrePareWidth" />
                                        <ItemStyle HorizontalAlign="Center" Width="215px"  CssClass="PrePareWidth" />
                                    </asp:BoundField><%--7--%>
                                    <asp:BoundField DataField="tdstype" HeaderText="TDS Type" ><%--9--%>
    <HeaderStyle Wrap="false" HorizontalAlign="center" />
    <ItemStyle Wrap="false" HorizontalAlign="Right" Width="100px" />
</asp:BoundField><%--8--%>

<asp:BoundField DataField="tdsdesc" HeaderText="TDS Desc" ><%--10--%>
    <HeaderStyle Wrap="false" HorizontalAlign="center" />
    <ItemStyle Wrap="false" HorizontalAlign="Right" Width="100px" />
</asp:BoundField><%--9--%>

<%--NewOne--%>
<asp:BoundField DataField="tdssection" HeaderText="TDS Section" ><%--11--%>
    <HeaderStyle Wrap="false" HorizontalAlign="center" />
    <ItemStyle Wrap="false" HorizontalAlign="Right" />
</asp:BoundField><%--10--%>

<asp:TemplateField HeaderText="TDS %"  ><%--12--%>
    <ItemTemplate><%----%>
        <asp:TextBox ID="TDSPERS" runat="server" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" Text='<%#Eval("tdsper")%>' ToolTip='<%#Eval("tdsper")%>' Style="width: 50px; text-align: right; border: 1px solid #e0e0e0" Enabled="false"></asp:TextBox>
        <%--onkeyup="IsDoubleCheck_Grid(this);"  OnTextChanged="TDSPERS_TextChanged" AutoPostBack="true" --%>
    </ItemTemplate>
    <ItemStyle Wrap="false" HorizontalAlign="Right" Width="50px" />
</asp:TemplateField><%--11--%>

<%--NewOne--%>
<asp:TemplateField HeaderText="TDS Section" ><%--13--%> 
    <HeaderStyle Wrap="false" />

    <ItemTemplate>
        <asp:DropDownList ID="ddl_section" Width="100%" runat="server" data-placeholder="--Section--" ToolTip="Section" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" AutoPostBack="true" OnSelectedIndexChanged="ddl_section_SelectedIndexChanged" OnCellContentClick="Grd_TDS_CellContentClick">
            <%-- CssClass="chzn-select"--%>
            <asp:ListItem Value="0" Text="Select"></asp:ListItem>
        </asp:DropDownList>
    </ItemTemplate>
    <ItemStyle Width="80px" />
</asp:TemplateField><%--12--%>

<asp:TemplateField HeaderText="TDS Section" ><%--14--%>
    <HeaderStyle Wrap="false" />

    <ItemTemplate>
        <asp:DropDownList ID="ddl_section1" runat="server" Width="100%" data-placeholder="--Section--" ToolTip="Section" AutoPostBack="true" OnSelectedIndexChanged="ddl_section_SelectedIndexChanged1" OnCellContentClick="Grd_TDS_CellContentClick">
            <%-- CssClass="chzn-select"--%>
            <asp:ListItem Value="0" Text="Select"></asp:ListItem>

        </asp:DropDownList>
    </ItemTemplate>
    <ItemStyle Width="80px" />
</asp:TemplateField><%--13--%>

<asp:TemplateField HeaderText="TDS%" > <%--15--%>
    <ItemTemplate>
        <asp:TextBox ID="txtpercentage" runat="server" Enabled="false"  ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" Style="text-align: right; width: 50px; border: 1px solid #e0e0e0!important;"
            onkeyup="IsDoubleCheck_Grid(this);"  Text='<%#Eval("tdsper")%>' ToolTip='<%#Eval("tdsper")%>'  ></asp:TextBox>
    </ItemTemplate>

    <ItemStyle Wrap="false" HorizontalAlign="Right" Width="50px" />
</asp:TemplateField><%--14--%>

<asp:TemplateField HeaderText="Amount" ><%--16--%>
    <ItemTemplate>
        <asp:TextBox ID="TDSAmount" runat="server" Text='<%#Eval("tdsamount","{0:n}")%>' ToolTip='<%#Eval("tdsamount")%>' Enabled="false" Style="text-align: right; width: 50px; border: 1px solid #e0e0e0!important;" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide"></asp:TextBox>
        <%--onkeyup="IsDoubleCheck_Grid(this);"  OnTextChanged="TDSPERS_TextChanged" AutoPostBack="true" --%>
    </ItemTemplate>
    <ItemStyle Wrap="false" HorizontalAlign="Right" Width="50px" />
</asp:TemplateField><%--15--%>
                                    <asp:TemplateField HeaderText="Approve">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Chk_Approval" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="80px"  />
                                        <ItemStyle HorizontalAlign="Center" Width="80px"  />
                                    </asp:TemplateField><%--16--%>
                                    <asp:BoundField DataField="stamt" HeaderText="stamt" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" /><%--17--%>
                                    <asp:BoundField DataField="SupplyTo" HeaderText="SupplyTo" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" /><%--18--%>
                                    <%-- <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="Lnk_Approval" runat="server" CommandName="select" Font-Underline="false"
                        CssClass="Arrow">⇛</asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>--%>
                                </Columns>
                                <EmptyDataRowStyle CssClass="EmptyRowStyle" />
                                <HeaderStyle CssClass="GridHeader" />
                                <AlternatingRowStyle CssClass="GrdAltRow" />
                            </asp:GridView>
                        </div>

                    </div>
                  
                </div>

            </div>
        </div>
    </div>


    <asp:HiddenField ID="hid_type" runat="server" />
    <asp:HiddenField ID="hid_supplyto" runat="server" />
    <asp:HiddenField ID="hid_stamt" runat="server" />

    <asp:HiddenField ID="hid_refno" runat="server" />
    <asp:HiddenField ID="hid_vouyear" runat="server" />
      
</asp:Content>
