using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CyBLE_MTK_Application
{
    #region Enums
    public enum SettingsImpExp { None, Export, Import };
    #endregion

    public partial class CyBLEMTKSettingsImpExp : Form
    {
        private SettingsImpExp currentAction;
        public SettingsImpExp CurrentAction
        {
            get
            {
                return currentAction;
            }
            set
            {
                currentAction = value;

                if (currentAction == SettingsImpExp.Import)
                {
                    this.Text = "Import Settings";
                    this.WhatToDoLabel.Text = "Select settings that you want to import.";
                }
                else if (currentAction == SettingsImpExp.Export)
                {
                    this.Text = "Export Settings";
                    this.WhatToDoLabel.Text = "Select settings that you want to export.";
                }
            }
        }

        private string[] appSettings;
        public string[] ImpExpSettings
        {
            set
            {
                appSettings = value;
                DataTable myTable;
                DataColumn colItem1;
                DataGridViewCheckBoxColumn colItem2;
                DataRow NewRow;
                DataView myView;

                // DataTable to hold data that is displayed in DataGrid
                myTable = new DataTable("myTable");

                // the three columns in the table
                colItem1 = new DataColumn("Settings", Type.GetType("System.String"));
                colItem2 = new DataGridViewCheckBoxColumn();
                colItem2.Name = (currentAction == SettingsImpExp.None)?"Imprt/Export":currentAction.ToString();

                // add the columns to the table
                myTable.Columns.Add(colItem1);

                for (int i = 0; i < appSettings.Count(); i++)
                {
                    NewRow = myTable.NewRow();
                    NewRow["Settings"] = appSettings[i];
                    myTable.Rows.Add(NewRow);
                }


                // DataView for the DataGridView
                myView = new DataView(myTable);
                myView.AllowDelete = false;
                myView.AllowEdit = true;
                myView.AllowNew = false;

                // Assign DataView to DataGrid
                this.PropertiesDataGridView.DataSource = myView;
                DataGridViewColumn column = this.PropertiesDataGridView.Columns[0];
                column.ReadOnly = true;
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

                this.PropertiesDataGridView.Columns.Add(colItem2);
                DataGridViewColumn column1 = this.PropertiesDataGridView.Columns[1];
                column1.ReadOnly = true;
                column1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column1.SortMode = DataGridViewColumnSortMode.NotSortable;

                this.PropertiesDataGridView.RowHeadersVisible = false;
                this.PropertiesDataGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.PropertiesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        public CyBLEMTKSettingsImpExp()
        {
            InitializeComponent();
            currentAction = SettingsImpExp.None;
        }
    }
}
