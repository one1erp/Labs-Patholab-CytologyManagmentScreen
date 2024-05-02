using LSExtensionWindowLib;
using LSSERVICEPROVIDERLib;
using Oracle.ManagedDataAccess.Client;
using Patholab_Common;
using Patholab_DAL_V1;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;




namespace CytologyManagmentScreen

{

    [ComVisible(true)]

    [ProgId("CytologyManagmentScreen.CytologyManagmentScreen")]

    public partial class CytologyManagmentScreen_host : UserControl, IExtensionWindow

    {

        #region Private members



        private INautilusProcessXML xmlProcessor;

        private INautilusUser _ntlsUser;

        private IExtensionWindowSite2 _ntlsSite;

        private INautilusServiceProvider sp;

        private INautilusDBConnection _ntlsCon;

        private DataLayer dal = null;

        long sid = 1;

        #endregion


        public CytologyManagmentScreen_host()
        {

            try
            {
                InitializeComponent();

                BackColor = Color.FromName("Control");

                this.Dock = DockStyle.Fill;

                this.AutoSize = true;

                this.AutoSizeMode = AutoSizeMode.GrowAndShrink;  
                
            }

            catch (Exception e)

            {
                MessageBox.Show(e.Message);
            }

        }


        #region implementing interface



        public bool CloseQuery()
        {
            DialogResult res = MessageBox.Show(@"?האם אתה בטוח שברצונך לצאת ", "CytologyManagmentScreen", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)

            {

                if (dal != null)
                {
                    dal.Close();

                    dal = null;
                }

                if (_ntlsSite != null) _ntlsSite = null;

                this.Dispose();

                return true;

            }

            else

            {
                return false;
            }

        }



        public WindowRefreshType DataChange()
        {
            return LSExtensionWindowLib.WindowRefreshType.windowRefreshNone;
        }



        public WindowButtonsType GetButtons()
        {
            return LSExtensionWindowLib.WindowButtonsType.windowButtonsNone;
        }


        public void Internationalise()
        {

        }


        public void PreDisplay()
        {

            xmlProcessor = Utils.GetXmlProcessor(sp);

            _ntlsUser = Utils.GetNautilusUser(sp);

            InitializeData();

        }


        private void InitializeData()
        {
            dal = new DataLayer();
            dal.Connect(_ntlsCon);

            var oraCon = GetOracleConnection(_ntlsCon);

            var w = new Cyto_screen(oraCon, sp, xmlProcessor, _ntlsCon, _ntlsSite, _ntlsUser, dal);
            elementHost1.Child = w;
            w.Initilaize();
            w.Focus();

        }
        public OracleConnection GetOracleConnection(INautilusDBConnection ntlsCon)
        {
            OracleConnection connection = null;

            if (ntlsCon != null)
            {
                try
                {
                    // Get the ADO.NET connection string from the provided interface
                    string _connectionString = ntlsCon.GetADOConnectionString();

                    // Split the connection string by ';' to extract relevant information
                    var splitted = _connectionString.Split(';');

                    // Initialize a new connection string
                    var cs = "";

                    // Build the connection string excluding the first element (assumed to be the provider)
                    for (int i = 1; i < splitted.Length; i++)
                    {
                        cs += splitted[i] + ';';
                    }


                    // Get the username from the provided interface
                    var username = ntlsCon.GetUsername();


                    // If username is empty, construct a new connection string with default user
                    if (string.IsNullOrEmpty(username))
                    {
                        var serverDetails = ntlsCon.GetServerDetails();
                        cs = "User Id=/;Data Source=" + serverDetails + ";";
                    }

                    // Create a new Oracle connection using the prepared connection string
                    connection = new OracleConnection(cs);


                    // Open the connection to the Oracle database
                    connection.Open();


                    // Get the LIMS user password from the provided interface
                    string limsUserPassword = ntlsCon.GetLimsUserPwd();


                    // Determine the SQL command to set the role based on the presence of LIMS user password
                    string roleCommand = string.IsNullOrEmpty(limsUserPassword) ? "set role lims_user" : "set role lims_user identified by " + limsUserPassword;


                    // Execute the role-setting command on the Oracle connection
                    OracleCommand command = new OracleCommand(roleCommand, connection);

                    command.ExecuteNonQuery();

                    // Get the session ID from the provided interface
                    double _session_id = ntlsCon.GetSessionId();

                    // Prepare and execute the SQL command to connect to the specified session
                    string sSql = string.Format("call lims.lims_env.connect_same_session({0})", _session_id);
                    command = new OracleCommand(sSql, connection);
                    command.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    // Catch and rethrow any exceptions that occur during the Oracle connection establishment
                    throw new Exception("An error occurred while establishing Oracle connection: " + e.Message);
                }

            }

            return connection;
        }

        public void RestoreSettings(int hKey)

        {

        }



        public bool SaveData()
        {
            return true;
        }



        public void SaveSettings(int hKey)

        {

        }



        public void SetParameters(string parameters)

        {

        }



        public void SetServiceProvider(object serviceProvider)

        {

            sp = serviceProvider as NautilusServiceProvider;

            _ntlsCon = Utils.GetNtlsCon(sp);

            this.sid = (long)_ntlsCon.GetSessionId();



        }



        public void SetSite(object site)

        {

            _ntlsSite = (IExtensionWindowSite2)site;

            _ntlsSite.SetWindowInternalName("Cyto_managment_screen");

            _ntlsSite.SetWindowRegistryName("Cyto_managment_screen");

            _ntlsSite.SetWindowTitle("Cyto_managment_screen");

        }



        public void Setup()

        {

        }



        public WindowRefreshType ViewRefresh()

        {

            return LSExtensionWindowLib.WindowRefreshType.windowRefreshNone;

        }



        public void refresh()

        {

        }



        #endregion

    }
}