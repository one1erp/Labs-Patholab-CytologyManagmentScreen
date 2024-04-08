namespace CytologyManagmentScreen
{
    partial class AliqDataGrid
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
            this.dataGridAliquots = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAliquots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAliquots.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridAliquots
            // 
            this.dataGridAliquots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridAliquots.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.dataGridAliquots.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.dataGridAliquots.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.dataGridAliquots.Name = "dataGridAliquots";
            this.dataGridAliquots.ReadOnly = true;
            this.dataGridAliquots.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridAliquots.Size = new System.Drawing.Size(510, 396);
            this.dataGridAliquots.TabIndex = 1;
            this.dataGridAliquots.MasterTemplate.EnableFiltering = true;
            this.dataGridAliquots.MasterTemplate.ShowFilteringRow = false;
            this.dataGridAliquots.MasterTemplate.ShowHeaderCellButtons = true;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridAliquots);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(510, 396);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAliquots.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridAliquots)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView dataGridAliquots;
    }
}
