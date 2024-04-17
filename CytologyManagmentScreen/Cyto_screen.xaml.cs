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
        public INautilusServiceProvider ServiceProvider { get; set; }
        private INautilusProcessXML xmlProcessor;
        private INautilusUser _ntlsUser;
        private IExtensionWindowSite2 _ntlsSite;
        private INautilusServiceProvider sp;
        public INautilusDBConnection _ntlsCon;
        private DataLayer dal;

        private MainDataGrid mainDataGrid;
        public delegate void DataListChangedEventHandler(char type);
        public event DataListChangedEventHandler DataListChanged;
        private char _type;


        public Cyto_screen(INautilusServiceProvider sp, INautilusProcessXML xmlProcessor, INautilusDBConnection _ntlsCon,
           IExtensionWindowSite2 _ntlsSite, INautilusUser _ntlsUser, DataLayer _dal)
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
            this.dal = _dal;
        }

        public void Initilaize()
        {
            
            detailsGrid.Visibility = Visibility.Collapsed;
            data.ColumnDefinitions.Clear();

            mainDataGrid = new MainDataGrid(winformsHostMain, winformsHostSpecificLog, winformsHostAliq, lbl_SDGname, dal, btn, this);

            winformsHostMain.Child = mainDataGrid;
                        
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            switch (radioButton.Name)
            {
                case "radioButtonCyto":
                    {
                        ChangeDataList('C');
                        break;
                    }
                case "radioButtonHisto":
                    {
                        ChangeDataList('B');
                        break;
                    }
                case "radioButtonPap":
                    {
                        ChangeDataList('P');
                        break;
                    }
            }
        }

        private void ChangeDataList(char type)
        {
            _type = type;
            DataListChanged?.Invoke(_type);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintRadGridView(mainDataGrid.mainRadGridView);
        }

        private void PrintRadGridView(RadGridView radGridView)
        {
            RadPrintDocument printSettings = new RadPrintDocument();

            printSettings.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            printSettings.DefaultPageSettings.Landscape = true;

            radGridView.Print(true, printSettings);
        }





    }
}

