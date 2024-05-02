using Oracle.ManagedDataAccess.Client;
using Patholab_DAL_V1;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CytologyManagmentScreen
{
    public partial class LogDataGrid : UserControl
    {

        private DataLayer _dal;

        public LogDataGrid(DataLayer dal)
        {
            InitializeComponent();
            _dal = dal;
        }


        internal void SetGrid(long sdg_id)
        {

            string query = $"SELECT sl.sdg_id,sl.time,sl.description,pe.PHRASE_DESCRIPTION as info " +
                $"FROM lims_sys.sdg_log sl " +
                $"LEFT JOIN lims_sys.PHRASE_ENTRY pe ON pe.PHRASE_ID = 241 AND pe.PHRASE_NAME = sl.APPLICATION_CODE WHERE sl.sdg_id ={sdg_id}";

            List<SpecificSdgLogRow> rowsFromDB = _dal.FetchDataFromDB(query, reader =>
            {
                return new SpecificSdgLogRow
                {
                    SdgId = Convert.ToInt32(reader[0]),
                    Time = Convert.ToDateTime(reader[1]),
                    Description = reader[2].ToString(),
                    Info = reader[3].ToString()
                };
            });

            gridDataLog.DataSource = rowsFromDB;
        }       
    }
}
