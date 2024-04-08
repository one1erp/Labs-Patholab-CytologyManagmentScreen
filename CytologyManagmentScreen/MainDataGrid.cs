using LSSERVICEPROVIDERLib;
using Microsoft.Office.Interop.Excel;
using Oracle.ManagedDataAccess.Client;
using Patholab_Common;
using Patholab_DAL_V1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using Telerik.WinControls.UI;



namespace CytologyManagmentScreen
{
    public partial class MainDataGrid : System.Windows.Forms.UserControl
    {

        private INautilusDBConnection _ntlsCon;

        private DataLayer _dal = null;


        private WindowsFormsHost winformsHostAliq;
        private WindowsFormsHost winformsHostSpecificLog;
        private WindowsFormsHost winformsHostMain;
        private System.Windows.Controls.Label _label;
        LogDataGrid gridView_2;
        AliqDataGrid gridView_3;
        OracleConnection _oraCon;


        GridViewRowInfo totalRow;
        public MainDataGrid(INautilusDBConnection ntlsCon, WindowsFormsHost a, WindowsFormsHost b, WindowsFormsHost c, System.Windows.Controls.Label label, DataLayer dal, OracleConnection oraCon, System.Windows.Controls.Button button)
        {
            InitializeComponent();
            _ntlsCon = ntlsCon;
            winformsHostSpecificLog = a;
            winformsHostAliq = b;
            winformsHostMain = c;
            _label = label;
            _dal = dal;
            _oraCon = oraCon;
            init();
            button.Click += Button_Clicked;
            radGridView1.FilterChanged += RadGridView_FilterChanged;
            totalRow = radGridView1.MasterTemplate.Rows.AddNew();
            totalRow.Cells[6].Value = $"{radGridView1.ChildRows.Count} מקרים בתהליך";


            totalRow.PinPosition = PinnedRowPosition.Bottom;
            totalRow.IsCurrent = false;



        }

        private void RadGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            // Notify WPF application about filtering
            GridFilterChanged?.Invoke(this, EventArgs.Empty);

            var a = radGridView1.ChildRows.Count;

            totalRow.Cells[6].Value = $"{radGridView1.ChildRows.Count} מקרים בתהליך";
        }

        public event EventHandler ButtonClicked;


        public int GetRowCount()
        {
            return radGridView1.ChildRows.Count;
        }


        public event EventHandler GridFilterChanged;

  

        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            // Raise the ButtonClicked event when the Button is clicked
            ButtonClicked?.Invoke(this, EventArgs.Empty);
            Grid grid = (Grid)winformsHostMain.Parent;
            Grid gridFather = (Grid)grid.Parent;
            RestoreSavedGridLayout(gridFather);
        }

        private void init()

        {

            try

            {

                gridView_2 = new LogDataGrid(_oraCon);

                gridView_3 = new AliqDataGrid(_dal);

                SetList();

            }

            catch (Exception ex)

            {

                System.Windows.MessageBox.Show(ex.Message);

            }

        }

        private void SetList()
        {
            List<SDGsLogRowList> listSdgs_log = new List<SDGsLogRowList>();
            var a = _dal.GetAll<SDG_LOG_CYTOLOGY>();

            var _SDGs_LOG_LIST = (from sdg in _dal.GetAll<SDG_LOG_CYTOLOGY>()
                                  select new SDGsLogRowList()
                                  {
                                      SDG_ID = sdg.SDG_ID,
                                      Name = sdg.NAME,
                                      U_Patholab_Number = sdg.U_PATHOLAB_NUMBER,
                                      Last_Application_Code = sdg.LAST_APPLICATION_CODE,
                                      Patholog_Name = sdg.PATHOLOG_NAME,
                                      Info = sdg.INFO,
                                      Received_On = sdg.RECEIVED_ON,
                                      Part = sdg.PART,
                                      Priority = sdg.PRIORITY,

                                  }).ToList();


            listSdgs_log.AddRange(_SDGs_LOG_LIST);

            radGridView1.DataSource = _SDGs_LOG_LIST;
        }
        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) 
            {
                var sdg = radGridView1.Rows[e.RowIndex].DataBoundItem as SDGsLogRowList;
                if (sdg != null)
                {
                    SetGridLayout();

                    try
                    {
                        gridView_3.SetDataGridAliquots(sdg.SDG_ID);
                        winformsHostAliq.Child = gridView_3;

                    }
                    catch(Exception ex) { 

                    Logger.WriteLogFile(ex);
                    
                    }


                    _label.Content = sdg.U_Patholab_Number;

                    gridView_2.SetGrid(sdg.SDG_ID);
                    winformsHostSpecificLog.Child = gridView_2;



                }
            }
        }

        private List<ColumnDefinition> savedColumnDefinitions = new List<ColumnDefinition>();



        private void RestoreSavedGridLayout(Grid gridFather)
        {
            Grid grid1 = (Grid)winformsHostAliq.Parent;
            grid1.Visibility = Visibility.Collapsed;
            Grid grid = (Grid)winformsHostMain.Parent;
            Grid gridFather_1 = (Grid)grid.Parent;
            gridFather_1.ColumnDefinitions.Clear();
        }
        private void SetGridLayout()
        {
            Grid grid = (Grid)winformsHostMain.Parent;
            Grid gridFather = (Grid)grid.Parent;

            if (grid != null)
            {

                // Apply new pagination settings
                gridFather.ColumnDefinitions.Clear();
                ColumnDefinition colDef1 = new ColumnDefinition();
                colDef1.Width = new GridLength(1, GridUnitType.Star);
                ColumnDefinition colDef2 = new ColumnDefinition();
                colDef2.Width = new GridLength(1.5, GridUnitType.Star);
                gridFather.ColumnDefinitions.Add(colDef1);
                gridFather.ColumnDefinitions.Add(colDef2);
            }

            Grid grid1 = (Grid)winformsHostAliq.Parent;
            grid1.Visibility = Visibility.Visible;
            Grid.SetColumn(grid1, 0);
            Grid.SetColumn(grid, 1);
        }

    }
}

