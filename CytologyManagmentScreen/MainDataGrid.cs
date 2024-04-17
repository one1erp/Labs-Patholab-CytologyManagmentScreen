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
using System.Windows.Input;
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
        public MainDataGrid(INautilusDBConnection ntlsCon, WindowsFormsHost a, WindowsFormsHost b, WindowsFormsHost c, System.Windows.Controls.Label label, DataLayer dal, OracleConnection oraCon, System.Windows.Controls.Button button, Cyto_screen cyto_screen)
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
            cyto_screen.DataListChanged += HandleDataListChanged;

        }
        Dictionary<char, object> DataList = new Dictionary<char, object> { { 'C', null }, { 'B', null }, { 'P', null } };

        private void HandleDataListChanged(char type)
        {
            SetList(type);
            Grid grid = (Grid)winformsHostMain.Parent;
            Grid gridFather = (Grid)grid.Parent;
            RestoreSavedGridLayout(gridFather);
        }

        private void RadGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            // Notify WPF application about filtering
            GridFilterChanged?.Invoke(this, EventArgs.Empty);
            totalRow.Cells[6].Value = $"{radGridView1.ChildRows.Count-1} מקרים בתהליך";
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

                SetList('C');

           
            }

            catch (Exception ex)

            {
                System.Windows.MessageBox.Show(ex.Message);
            }

        }

        private void SetList(char type)
        {
            if (DataList[type] == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                radGridView1.DataSource = ExecuteQueryAndGetResults(type);
                Mouse.OverrideCursor = null;
            }
            else
            {
                radGridView1.DataSource = DataList[type];
            }

            if (!radGridView1.MasterTemplate.Rows[radGridView1.MasterTemplate.Rows.Count - 1].Cells[6].ToString().Contains("מקרים"))
            {
                totalRow = radGridView1.MasterTemplate.Rows.AddNew();
                totalRow.Cells[6].Value = $"{radGridView1.ChildRows.Count-1} מקרים בתהליך";
                radGridView1.CurrentRow = radGridView1.MasterTemplate.Rows[0];
                totalRow.PinPosition = PinnedRowPosition.Bottom;
                
            }

        }
        private List<T> FetchDataFromDB<T>(OracleConnection connection, string query, Func<OracleDataReader, T> mapFunc)
        {
            List<T> result = new List<T>();

            using (OracleCommand command = connection.CreateCommand())
            {
                command.CommandText = query;

                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T item = mapFunc(reader);
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        public List<SDGsLogRowList> ExecuteQueryAndGetResults(char type)
        {
            string query = $"select * from lims.SDG_LOG_CYTOLOGY where name LIKE '{type}%'";

            List<SDGsLogRowList> resultList = FetchDataFromDB(_oraCon, query, reader =>
            {
                return new SDGsLogRowList
                {
                    SDG_ID = Convert.ToInt64(reader["SDG_ID"]),
                    Name = reader["name"].ToString(),
                    U_Patholab_Number = reader["u_patholab_number"].ToString(),
                    Last_Application_Code = reader["last_application_code"].ToString(),
                    Patholog_Name = reader["patholog_name"].ToString(),
                    Info = reader["INFO"].ToString(),
                    Received_On = reader["received_on"] == DBNull.Value ? null : (DateTime?)reader["received_on"],
                    Part = reader["part"].ToString(),
                    Priority = reader["PRIORITY"].ToString()
                };
            });

            DataList[type] = resultList;
            return resultList;
        }


        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var sdg = radGridView1.Rows[e.RowIndex].DataBoundItem as SDGsLogRowList;
                if (sdg != null)
                {
                    SetGridLayout();
                    gridView_3.SetDataGridAliquots(sdg.SDG_ID);
                    winformsHostAliq.Child = gridView_3;

                    _label.Content = sdg.U_Patholab_Number;

                    gridView_2.SetGrid(sdg.SDG_ID);
                    winformsHostSpecificLog.Child = gridView_2;

                }
            }
        }


        #region grid layouts functions
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
        #endregion


    }
}

