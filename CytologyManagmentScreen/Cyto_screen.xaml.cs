using LSExtensionWindowLib;
using LSSERVICEPROVIDERLib;
using Oracle.ManagedDataAccess.Client;
using Patholab_DAL_V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.WinControls.UI;

namespace CytologyManagmentScreen
{
    /// <summary>
    /// Interaction logic for Cyto_screen.xaml
    /// </summary>
    public partial class Cyto_screen : UserControl
    {
        public Cyto_screen()
        {
            InitializeComponent();
        }
        public INautilusServiceProvider ServiceProvider { get; set; }
        private INautilusProcessXML xmlProcessor;
        private INautilusUser _ntlsUser;
        private IExtensionWindowSite2 _ntlsSite;
        private INautilusServiceProvider sp;
        public INautilusDBConnection _ntlsCon;
        private DataLayer dal;
        public bool DEBUG;
        private List<PHRASE_ENTRY> RotherStatus;
        private SDG_USER sdg;
        private U_DEBIT_USER debit;
        private long? _operator_id;
        private double _session_id;
        MainDataGrid gridView_1;
        OracleConnection oraCon;
        int rowCount;

        public Cyto_screen(INautilusServiceProvider sp, INautilusProcessXML xmlProcessor, INautilusDBConnection _ntlsCon,
           IExtensionWindowSite2 _ntlsSite, INautilusUser _ntlsUser)
        {
            if (_ntlsUser.GetRoleName().ToUpper() == "DEBUG") Debugger.Launch();
            InitializeComponent();
            this.ServiceProvider = sp;
            this.sp = sp;
            this.xmlProcessor = xmlProcessor;
            this._ntlsCon = _ntlsCon;
            this._ntlsSite = _ntlsSite;
            this._ntlsUser = _ntlsUser;
            this.DataContext = this;
        }

        public void Initilaize()
        {

            dal = new DataLayer();
            dal.Connect(_ntlsCon);
            _operator_id = (long)_ntlsUser.GetOperatorId();
            _session_id = _ntlsCon.GetSessionId();


            detailesGrid.Visibility = Visibility.Collapsed;
            data.ColumnDefinitions.Clear();
            oraCon = GetConnection(_ntlsCon);

            gridView_1 = new MainDataGrid(_ntlsCon, winformsHostSpecificLog, winformsHostAliq, winformsHostMain, lbl_SDGname, dal, oraCon, btn);
            //gridView_1.ButtonClicked += HandleButtonClick;

            winformsHostMain.Child = gridView_1;
            
            

        }

        private void PrintRadGridView(RadGridView radGridView)
        {
            RadPrintDocument printSettings = new RadPrintDocument();

            printSettings.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            printSettings.DefaultPageSettings.Landscape = true;

            radGridView.Print(true, printSettings);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PrintRadGridView(gridView_1.radGridView1);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        public OracleConnection GetConnection(INautilusDBConnection ntlsCon)
        {

            OracleConnection connection = null;

            if (ntlsCon != null)
            {


                // Initialize variables
                String roleCommand;
                // Try/Catch block
                try
                {


                    var C = ntlsCon.GetServerIsProxy();
                    var C2 = ntlsCon.GetServerName();
                    var C4 = ntlsCon.GetServerType();

                    var C6 = ntlsCon.GetServerExtra();

                    var C8 = ntlsCon.GetPassword();
                    var C9 = ntlsCon.GetLimsUserPwd();
                    var C10 = ntlsCon.GetServerIsProxy();
                    var DD = _ntlsSite;




                    var u = _ntlsUser.GetOperatorName();
                    var u1 = _ntlsUser.GetWorkstationName();



                    string _connectionString = ntlsCon.GetADOConnectionString();

                    var splited = _connectionString.Split(';');

                    var cs = "";

                    for (int i = 1; i < splited.Count(); i++)
                    {
                        cs += splited[i] + ';';
                    }
                    //<<<<<<< .mine
                    var username = ntlsCon.GetUsername();
                    if (string.IsNullOrEmpty(username))
                    {
                        var serverDetails = ntlsCon.GetServerDetails();
                        cs = "User Id=/;Data Source=" + serverDetails + ";";
                    }


                    //Create the connection
                    connection = new OracleConnection(cs);



                    // Open the connection
                    connection.Open();

                    // Get lims user password
                    string limsUserPassword = ntlsCon.GetLimsUserPwd();

                    // Set role lims user
                    if (limsUserPassword == "")
                    {
                        // LIMS_USER is not password protected
                        roleCommand = "set role lims_user";
                    }
                    else
                    {
                        // LIMS_USER is password protected.
                        roleCommand = "set role lims_user identified by " + limsUserPassword;
                    }

                    // set the Oracle user for this connecition
                    OracleCommand command = new OracleCommand(roleCommand, connection);

                    // Try/Catch block
                    try
                    {
                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                    catch (Exception f)
                    {
                        // Throw the exception
                        throw new Exception("Inconsistent role Security : " + f.Message);
                    }

                    // Get the session id
                    _session_id = ntlsCon.GetSessionId();

                    // Connect to the same session
                    string sSql = string.Format("call lims.lims_env.connect_same_session({0})", _session_id);

                    // Build the command
                    command = new OracleCommand(sSql, connection);

                    // Execute the command
                    command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    // Throw the exception
                    throw e;
                }

                // Return the connection
            }

            return connection;

        }

        
    }
}

