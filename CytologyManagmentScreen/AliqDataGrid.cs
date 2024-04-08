using Patholab_Common;
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

        private DataLayer dal = null;

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
                var currentSdg = dal.FindBy<SDG>(x => x.SDG_ID == _sdg_id).FirstOrDefault();
                var samplesList = currentSdg.SAMPLEs.ToList();
                var aliqList = new List<ALIQUOT>();
                var aliqUserList = new List<ALIQUOT_USER>();

                aliqList = samplesList.SelectMany(x => x.ALIQUOTs).ToList();
                aliqUserList = aliqList.Select(x => x.ALIQUOT_USER).ToList();

                aliqStationList = dal.GetPhraseEntries("AliquotStationTrace").ToList();


                var aliqRowList = (from aliq in aliqUserList
                                   select new AliqRow()
                                   {
                                       ALIQUOT_ID = aliq.ALIQUOT_ID,
                                       U_ALIQUOT_STATION = aliq.U_ALIQUOT_STATION != null ? GetPhraseInfo(aliq.U_ALIQUOT_STATION) : null,
                                       U_GLASS_TYPE = aliq.U_GLASS_TYPE,
                                       U_IS_CELL_BLOCK = aliq.U_IS_CELL_BLOCK,
                                       U_LAST_LABORANT = aliq.OPERATOR != null ? aliq.OPERATOR.FULL_NAME : null,
                                       U_OLD_ALIQUOT_STATION = aliq.U_OLD_ALIQUOT_STATION != null ? GetPhraseInfo(aliq.U_OLD_ALIQUOT_STATION) : null,
                                       U_ALIQUOT_NAME = aliq.ALIQUOT.NAME
                                   }).ToList();

                dataGridAliquots.DataSource = aliqRowList;
            }
            catch (Exception ex)
            {

                Logger.WriteLogFile(ex);

            }
        }
        
        private string GetPhraseInfo(string phraseName)
        {
            return aliqStationList.FirstOrDefault(x=>x.PHRASE_NAME == phraseName).PHRASE_INFO;
        }
    }
}

