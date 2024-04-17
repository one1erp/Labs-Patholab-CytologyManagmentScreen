using LSExtensionWindowLib;
using LSSERVICEPROVIDERLib;
using Oracle.ManagedDataAccess.Client;
using Patholab_Common;
using Patholab_DAL_V1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CytologyManagmentScreen
{
    public partial class LogDataGrid : UserControl
    {
        string connectionString;
        private INautilusDBConnection ntlsCon;
        OracleConnection _oraCon;

        public LogDataGrid(OracleConnection oraCon)
        {
            _oraCon = oraCon;
            InitializeComponent();
            //connectionString = Utils.ConString;
        }


        internal void SetGrid(long sDG_ID)
        {

            string query = $"SELECT sl.sdg_id,sl.time,sl.description,pe.PHRASE_DESCRIPTION as info " +
                $"FROM lims_sys.sdg_log sl " +
                $"LEFT JOIN lims_sys.PHRASE_ENTRY pe ON pe.PHRASE_ID = 241 AND pe.PHRASE_NAME = sl.APPLICATION_CODE WHERE sl.sdg_id ={sDG_ID}";


            List<SpecificSdgLogRow> rowsFromDB = FetchDataFromDB(_oraCon, query, reader =>
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

    }
}
