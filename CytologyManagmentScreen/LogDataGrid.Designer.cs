using Telerik.WinControls.UI;

namespace CytologyManagmentScreen
{
    partial class LogDataGrid
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.gridDataLog = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataLog.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // gridDataLog
            // 
            this.gridDataLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDataLog.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.gridDataLog.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.gridDataLog.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gridDataLog.Name = "gridDataLog";
            this.gridDataLog.ReadOnly = true;
            this.gridDataLog.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridDataLog.MasterTemplate.EnableFiltering = true;
            this.gridDataLog.MasterTemplate.ShowFilteringRow = false;
            this.gridDataLog.MasterTemplate.ShowHeaderCellButtons = true;

            // 
            // LogDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridDataLog);
            this.Name = "LogDataGrid";
            this.Size = new System.Drawing.Size(498, 313);
            ((System.ComponentModel.ISupportInitialize)(this.gridDataLog.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Telerik.WinControls.UI.RadGridView gridDataLog;
    }
}
