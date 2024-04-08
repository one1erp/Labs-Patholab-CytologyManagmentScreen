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
            List<SpecificSdgLogRow> rows = new List<SpecificSdgLogRow>();

            string query = $"SELECT sl.sdg_id,sl.time,sl.description,pe.PHRASE_DESCRIPTION as info " +
                $"FROM lims_sys.sdg_log sl " +
                $"LEFT JOIN lims_sys.PHRASE_ENTRY pe ON pe.PHRASE_ID = 241 AND pe.PHRASE_NAME = sl.APPLICATION_CODE WHERE sl.sdg_id ={sDG_ID}";




            try
            {
                using (OracleCommand cmd = new OracleCommand(query, _oraCon))
                {

                    try
                    {
                        using (var reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                SpecificSdgLogRow clf = new SpecificSdgLogRow
                                {
                                    SdgId = Convert.ToInt32(reader[0]),
                                    Time = Convert.ToDateTime(reader[1]),
                                    Description = reader[2].ToString(),
                                    Info = reader[3].ToString(),
                                };

                                rows.Add(clf);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(2+ ex.Message);
                    }

                   
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(1+ ex.Message);
            }






            gridDataLog.DataSource = rows;
        }
        

    }
}
