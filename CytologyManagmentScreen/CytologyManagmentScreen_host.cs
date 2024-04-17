﻿using LSExtensionWindowLib;
using LSSERVICEPROVIDERLib;
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

            var w = new Cyto_screen(sp, xmlProcessor, _ntlsCon, _ntlsSite, _ntlsUser, dal);
            elementHost1.Child = w;
            w.Initilaize();
            w.Focus();

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