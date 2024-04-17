using Patholab_DAL_V1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CytologyManagmentScreen
{
    public partial class AliqDataGrid : UserControl
    {

        private DataLayer dal;

        private List<PHRASE_ENTRY> aliqStationList;

        public AliqDataGrid(DataLayer dal)
        {
            InitializeComponent();
            this.dal = dal;            
        }

        public void SetDataGridAliquots(long _sdg_id)
        {
            try
            {
                //get current sdg by id
                var currentSdg = dal.FindBy<SDG>(x => x.SDG_ID == _sdg_id).FirstOrDefault();

                //get current sdg's aliquot_user list
                var aliqUserList = currentSdg.SAMPLEs.SelectMany(s => s.ALIQUOTs).ToList().Select(a => a.ALIQUOT_USER).ToList();

                //get all aliquot_station names 
                aliqStationList = dal.GetPhraseEntries("AliquotStationTrace").ToList();

                var aliqRowList = aliqUserList.Select(item => {

                    var uAliquotStation = GetPhraseInfo(item.U_ALIQUOT_STATION);
                    var uOldAliquotStation = GetPhraseInfo(item.U_OLD_ALIQUOT_STATION);

                    return new AliqRow()
                    {
                        ALIQUOT_ID = item.ALIQUOT_ID,
                        U_ALIQUOT_STATION = uAliquotStation,
                        U_GLASS_TYPE = item.U_GLASS_TYPE,
                        U_IS_CELL_BLOCK = item.U_IS_CELL_BLOCK,
                        U_LAST_LABORANT = item.OPERATOR?.FULL_NAME,
                        U_OLD_ALIQUOT_STATION = uOldAliquotStation,
                        U_ALIQUOT_NAME = item.ALIQUOT.NAME
                    };
                }).ToList();

                dataGridAliquots.DataSource = aliqRowList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SetDataGridAliquots {ex.Message}");
            }
        }

        private string GetPhraseInfo(string phraseName)
        {
            var phraseEntry = aliqStationList.FirstOrDefault(x => x.PHRASE_NAME == phraseName);
            return phraseEntry?.PHRASE_INFO ?? string.Empty;
        }


    }
}

