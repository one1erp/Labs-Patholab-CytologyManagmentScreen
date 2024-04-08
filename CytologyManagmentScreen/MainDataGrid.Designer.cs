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
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridView1
            // 
            this.radGridView1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(0, 0);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ReadOnly = true;
            this.radGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.radGridView1.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellClick);
            this.radGridView1.CellFormatting += radGridView1_CellFormatting;
            this.radGridView1.MasterTemplate.EnableFiltering = true;
            this.radGridView1.MasterTemplate.ShowFilteringRow = false;
            this.radGridView1.MasterTemplate.ShowHeaderCellButtons = true;
           
            // 
            // 
            // 
            this.radGridView1.RootElement.AccessibleDescription = null;
            this.radGridView1.RootElement.AccessibleName = null;
            this.radGridView1.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 300, 187);
            this.radGridView1.Size = new System.Drawing.Size(347, 329);
            this.radGridView1.TabIndex = 1;
            // 
            // MainDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radGridView1);
            this.Name = "MainDataGrid";
            this.Size = new System.Drawing.Size(347, 329);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            this.ResumeLayout(false);

        }
        private void radGridView1_CellFormatting(object sender, CellFormattingEventArgs e)
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

        public Telerik.WinControls.UI.RadGridView radGridView1;
    }
}
