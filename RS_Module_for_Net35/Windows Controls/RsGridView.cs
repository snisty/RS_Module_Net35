using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RS_Module_for_Net35.Windows_Controls
{
    public partial class RsGridView : UserControl
    {
        public RsGridView()
        {
            InitializeComponent();
        }

        DataTable BindData = new DataTable();

        /// <summary>
        /// 새 컬럼을 만듭니다.
        /// </summary>
        /// <param name="ColumnName"></param>
        public void AddColumn(string ColumnName)
        {
            BindData.Columns.Add(ColumnName);
            dataGridView_Main.DataSource = BindData;
        }

        /// <summary>
        /// 새 행을 만듭니다.
        /// </summary>
        public void AddRow()
        {
            BindData.Rows.Add();
            dataGridView_Main.DataSource = BindData;
        }

        /// <summary>
        /// 특정 셀의 값을 설정합니다.
        /// </summary>
        public void SetText(int iRow, string ColumnName, string Text)
        {
            BindData.Rows[iRow][ColumnName] = Text;
            dataGridView_Main.DataSource = BindData;
        }

        /// <summary>
        /// 특정 셀의 값을 가져옵니다.
        /// </summary>
        public string GetText(int iRow, string ColumnName)
        {
            return BindData.Rows[iRow][ColumnName].ToString();
        }

        /// <summary>
        /// 그리드를 클리어합니다.
        /// </summary>
        public void Clear()
        {
            BindData = new DataTable();
            dataGridView_Main.DataSource = BindData;
        }
    }
}
