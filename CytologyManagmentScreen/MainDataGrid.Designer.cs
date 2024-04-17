using CytologyManagmentScreen;
using System;
using System.Drawing;
using Telerik.WinControls.UI;

namespace CytologyManagmentScreen
{
    partial class MainDataGrid
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
            this.mainRadGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.mainRadGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainRadGridView.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // mainRadGridView
            // 
            this.mainRadGridView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.mainRadGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainRadGridView.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.mainRadGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.mainRadGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.mainRadGridView.Name = "mainRadGridView";
            this.mainRadGridView.ReadOnly = true;
            this.mainRadGridView.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainRadGridView.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.mainRadGridView_CellClick);
            this.mainRadGridView.CellFormatting += mainRadGridView_CellFormatting;
            this.mainRadGridView.MasterTemplate.EnableFiltering = true;
            this.mainRadGridView.MasterTemplate.ShowFilteringRow = false;
            this.mainRadGridView.MasterTemplate.ShowHeaderCellButtons = true;
           
            // 
            // 
            // 
            this.mainRadGridView.RootElement.AccessibleDescription = null;
            this.mainRadGridView.RootElement.AccessibleName = null;
            this.mainRadGridView.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 300, 187);
            this.mainRadGridView.Size = new System.Drawing.Size(347, 329);
            this.mainRadGridView.TabIndex = 1;
            // 
            // MainDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainRadGridView);
            this.Name = "MainDataGrid";
            this.Size = new System.Drawing.Size(347, 329);
            ((System.ComponentModel.ISupportInitialize)(this.mainRadGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainRadGridView)).EndInit();
            this.ResumeLayout(false);

        }
        private void mainRadGridView_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement is GridDataCellElement)
            {
                GridDataCellElement dataCell = e.CellElement as GridDataCellElement;
                dataCell.TextAlignment = ContentAlignment.MiddleCenter;
            }
            if (e.CellElement.RowInfo is GridViewDataRowInfo && e.CellElement.Value is DateTime)
            {
                DateTime cellValue = (DateTime)e.CellElement.Value;
                e.CellElement.Text = cellValue.Date.ToShortDateString();
            }
        }



        #endregion

        public Telerik.WinControls.UI.RadGridView mainRadGridView;
    }
}
