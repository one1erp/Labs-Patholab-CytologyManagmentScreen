using LSSERVICEPROVIDERLib;
using Patholab_DAL_V1;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using Telerik.WinControls.UI;



namespace CytologyManagmentScreen
{
    public partial class MainDataGrid : System.Windows.Forms.UserControl
    {

        private DataLayer _dal = null;
        private WindowsFormsHost winformsHostAliq;
        private WindowsFormsHost winformsHostSpecificLog;
        private WindowsFormsHost winformsHostMain;
        private Label _label;
        public event EventHandler ButtonChangeLayoutClicked;
        public event EventHandler GridFilterChanged;
        LogDataGrid specificSDGLogDataGrid;
        AliqDataGrid aliqDataGrid;
        GridViewRowInfo totalRow;
        Dictionary<char, object> DataList = new Dictionary<char, object> { { 'C', null }, { 'B', null }, { 'P', null } };

        public MainDataGrid(WindowsFormsHost forMainArea, WindowsFormsHost forSpecificRowLogArea, WindowsFormsHost forAliqsArea, Label label, DataLayer dal, Button button, Cyto_screen cyto_screen)
        {
            InitializeComponent();
            winformsHostMain = forMainArea;
            winformsHostSpecificLog = forSpecificRowLogArea;
            winformsHostAliq = forAliqsArea;

            _label = label;
            _dal = dal;

            InitAreas();

            button.Click += BtnChangeLayout_Clicked;
            mainRadGridView.FilterChanged += RadGridView_FilterChanged;
            cyto_screen.DataListChanged += HandleDataListChanged;

        }

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
            totalRow.Cells[6].Value = $"{mainRadGridView.ChildRows.Count - 1} מקרים בתהליך";
        }

        private void BtnChangeLayout_Clicked(object sender, RoutedEventArgs e)
        {            
            Grid grid = (Grid)winformsHostMain.Parent;
            Grid gridFather = (Grid)grid.Parent;
            RestoreSavedGridLayout(gridFather);
        }

        private void InitAreas()
        {
            try
            {
                SetList('C');

                specificSDGLogDataGrid = new LogDataGrid(_dal);

                aliqDataGrid = new AliqDataGrid(_dal);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SetList(char type)
        {
            if (DataList[type] == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                mainRadGridView.DataSource = ExecuteQueryAndGetResults(type);
                Mouse.OverrideCursor = null;
            }
            else
            {
                mainRadGridView.DataSource = DataList[type];
            }

            ChangeCurrentListNumOfRows();
        }

        private void ChangeCurrentListNumOfRows()
        {
            var currentRowList = mainRadGridView.MasterTemplate.Rows;
            currentRowList.RemoveAt(currentRowList.Count - 1);
            totalRow = currentRowList.AddNew();
            totalRow.Cells[6].Value = $"{currentRowList.Count - 1} מקרים בתהליך";
            mainRadGridView.CurrentRow = currentRowList[0];
            totalRow.PinPosition = PinnedRowPosition.Bottom;
        }

        private void mainRadGridView_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var sdg = mainRadGridView.Rows[e.RowIndex].DataBoundItem as SDGsLogRowList;
                if (sdg != null)
                {
                    SetGridLayout();
                    aliqDataGrid.SetDataGridAliquots(sdg.SDG_ID);
                    winformsHostAliq.Child = aliqDataGrid;

                    _label.Content = sdg.U_Patholab_Number;

                    specificSDGLogDataGrid.SetGrid(sdg.SDG_ID);
                    winformsHostSpecificLog.Child = specificSDGLogDataGrid;

                }
            }
        }

        public List<SDGsLogRowList> ExecuteQueryAndGetResults(char type)
        {
            string query = $"select * from lims.SDG_LOG_CYTOLOGY where name LIKE '{type}%'";

            List<SDGsLogRowList> resultList = _dal.FetchDataFromDB(query, reader =>
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



        #region grid layouts functions
        private void RestoreSavedGridLayout(Grid gridFather)
        {
            Grid detailsGrid = (Grid)winformsHostAliq.Parent;
            detailsGrid.Visibility = Visibility.Collapsed;
            Grid grid = (Grid)winformsHostMain.Parent;
            Grid containerGrid = (Grid)grid.Parent;
            containerGrid.ColumnDefinitions.Clear();
        }
        private void SetGridLayout()
        {
            Grid grid = (Grid)winformsHostMain.Parent;
            Grid containerGrid = (Grid)grid.Parent;

            if (grid != null)
            {

                // Apply new pagination settings
                containerGrid.ColumnDefinitions.Clear();
                ColumnDefinition colDef1 = new ColumnDefinition();
                colDef1.Width = new GridLength(1, GridUnitType.Star);
                ColumnDefinition colDef2 = new ColumnDefinition();
                colDef2.Width = new GridLength(1.5, GridUnitType.Star);
                containerGrid.ColumnDefinitions.Add(colDef1);
                containerGrid.ColumnDefinitions.Add(colDef2);
            }

            Grid grid1 = (Grid)winformsHostAliq.Parent;
            grid1.Visibility = Visibility.Visible;
            Grid.SetColumn(grid1, 0);
            Grid.SetColumn(grid, 1);
        }
        #endregion


    }
}

