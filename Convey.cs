using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Convey
{
    public partial class Form1 : Form
    {
        int W = 100; // Ширина в клетках
        int H = 70; // Высота в клетках
        int D = 10; // Размер клетки в пикселях
        Field F;

        public Form1()
        {
            InitializeComponent();
            F = new Field(W, H);
            this.Width = W * D + 18;
            this.Height = H * D + 68;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    F.Rnd();
                    panel1.Invalidate();
                    break;
                case Keys.Escape:
                    F.Clear();
                    panel1.Invalidate();
                    break;
                case Keys.Space:
                    timer1.Enabled = !timer1.Enabled;
                    break;
            }
        }

        private void Form1_MouseDown(object sender, 
            MouseEventArgs e)
        {
            int I = e.X / D;
            int J = e.Y / D;
            if (F[I, J] == 0) F[I, J] = 1;
            else F[I, J] = 0;
            Rectangle R = new Rectangle(I * D, J * D, D, D);
            panel1.Invalidate(R);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int I = e.X / D;
            int J = e.Y / D;
            if (e.Button == MouseButtons.Left)
                F[I, J] = 1;
            if (e.Button==MouseButtons.Right)
                F[I, J] = 0;
            Rectangle R = new Rectangle(I * D, J * D, D, D);
            panel1.Invalidate(R);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            F.Step(panel1,D);
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            F.Paint(e.Graphics, D);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F.Rnd();
            panel1.Invalidate();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            F.Clear();
            panel1.Invalidate();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void parametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Dialog1 Dial = new Dialog1();

           // Установка элементов управления 
           Dial.numericWidth.Value = W;
           Dial.numericHeight.Value = H;
           Dial.trackBarCell.Value = D;
           Dial.numericQuant.Value = timer1.Interval;

           if (Dial.ShowDialog() == DialogResult.OK)
           {
               int NH = (int) Dial.numericHeight.Value;
               int NW = (int) Dial.numericWidth.Value;
               int ND = (int) Dial.trackBarCell.Value;
              
               if (NH != H || NW != W || ND !=D)
               {
                   if (NH != H || NW != W)
                       F = new Field(W = NW, H = NH);
                   if (ND != D) D = ND;
                   
                   this.Width = W * D + 18;
                   this.Height = H * D + 68;

                   panel1.Invalidate();
               }
               timer1.Interval = (int) Dial.numericQuant.Value;
             
           };
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int Quant = timer1.Interval;
            if (e.Delta > 0)
                Quant += 50;
            else
                Quant -= 50;
            if (Quant < 50) Quant = 50;
            if (Quant > 500) Quant = 500;
            timer1.Interval = Quant;

        }

    }
}
