using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MazeForm.Properties;

namespace MazeForm
{
    public partial class FrmMaze : Form
    {
        public FrmMaze()
        {
            InitializeComponent();

        }

        private Maze m = null;
        Point p = new Point(1, 1);
        private void btnCreateMaze_Click(object sender, EventArgs e)
        {
            int size;
            int.TryParse(tBSize.Text, out size);
            m = new Maze(size, size, true);
            CreateType ct = CreateType.Prim;
            switch (cmbCreateType.SelectedIndex)
            {
                case 0:
                    ct = CreateType.Prim;
                    break;
                case 1:
                    ct = CreateType.RecursiveDivision;
                    break;
                case 2:
                    ct = CreateType.RecursiveBacktrack;
                    break;
                case 3:
                    ct = CreateType.DfsGraph;
                    break;
            }
            m.CreateMaze(ct, new Point(0, 0));
            lbCreateTime.Text = m.CreateTime.TotalMilliseconds + "毫秒";
            m.DrawMaze(pBMain);
            
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            m.FindPath(FindType.DfsFind, p);
            m.DrawPath(pBMain);
        }

        private void btnAStar_Click(object sender, EventArgs e)
        {
            //m.GraphDFS();
            //m.DrawMaze(pBMain);
            m.FindPath(FindType.AStarFind, p);
            m.DrawPath(pBMain);
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            FindType ft = FindType.DfsFind;
            switch (cmbFindType.SelectedIndex)
            {
                case 0:
                    ft = FindType.DfsFind;
                    break;
                case 1:
                    ft = FindType.BFSFind;
                    break;
                case 2:
                    ft = FindType.AStarFind;
                    break;
            }
            m.FindPath(ft, p);
            lbFindTime.Text = m.FindTime.TotalMilliseconds + "毫秒";
            m.DrawGoPath(pBMain);
        }

        private void frmMaze_Load(object sender, EventArgs e)
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable, true);
            CheckForIllegalCrossThreadCalls = false;
            cmbCreateType.SelectedIndex = 0;
            cmbFindType.SelectedIndex = 0;
            tBSize.Text = 10.ToString();
            btnCreateMaze_Click(null, null);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FindType ft = FindType.DfsFind;
            switch (cmbFindType.SelectedIndex)
            {
                case 0:
                    ft = FindType.DfsFind;
                    break;
                case 1:
                    ft = FindType.BFSFind;
                    break;
                case 2:
                    ft = FindType.AStarFind;
                    break;
            }
            m.FindPath(ft, p);
            lbFindTime.Text = m.FindTime.TotalMilliseconds + "毫秒";
            m.DrawPath(pBMain);
        }

        private void cmbFindType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbFindType.SelectedIndex)
            {
                case 0:
                    btnShowFind.Enabled = true;
                    break;
                case 1:
                    btnShowFind.Enabled = true;
                    break;
                case 2:
                    btnShowFind.Enabled = true;
                    break;
            }
        }

        private void btnShowCreate_Click(object sender, EventArgs e)
        {
            m.DrawCreate(pBMain);
        }
    }
}
