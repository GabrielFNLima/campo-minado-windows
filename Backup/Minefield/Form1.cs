using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Minefield
{
    public partial class Form1 : Form
    {

        const int TOTAL = 15;

        const int WIDTH = 15;
        const int HEIGHT = 10;

        Button[,] buttons = new Button[WIDTH, HEIGHT];

        Label[,] labels = new Label[WIDTH, HEIGHT];

        bool[,] minas = new bool[WIDTH, HEIGHT];

        int clock = 0;
        
        bool fim = false;

        public Form1()
        {
            InitializeComponent();
            for (int x = 0; x < WIDTH; ++x)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Button btn = new Button
                    {
                        Dock = DockStyle.Fill,
                        Margin = new Padding(0),
                        Name = x.ToString() + ":" + y.ToString(),
                        TabStop = false,
                    };
                    btn.Click += new System.EventHandler(this.btn_Click);
                    btn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
                    buttons[x, y] = btn;

                    Label lbl = new Label
                    {
                        Dock = DockStyle.Fill,
                        Visible = false,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Microsoft Sans Serif", 14F,
                            FontStyle.Bold,GraphicsUnit.Point,
                            ((byte)(0))),
                    };
                    labels[x, y] = lbl;

                    this.tableLayoutPanel1.Controls.Add(btn, x + 1, y + 1);
                    this.tableLayoutPanel1.Controls.Add(lbl, x + 1, y + 1);
                }
            }
            New();
        }//Fomr1

        private void New()
        {
            for (int x = 0; x < WIDTH; ++x)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    minas[x, y] = false;
                    buttons[x, y].Visible = true;
                    buttons[x, y].Text = "";
                    labels[x, y].Visible = false;
                    labels[x, y].Text = "";
                    labels[x, y].ForeColor = Color.Black;
                    labels[x, y].BackColor = System.Drawing.SystemColors.Control;


                }
            }
            Random rand = new Random();
            for (int b = 0; b < TOTAL; ++b)
            {
                int col = rand.Next(WIDTH);
                int lin = rand.Next(HEIGHT);
                if (minas[col, lin])
                {
                    --b;
                }
                else
                {
                    minas[col, lin] = true;
                }
            }
            label1.Text = TOTAL.ToString();
            timer1.Stop();
            clock = 0;
            label2.Text = "0:00.0";
            fim = false;


        }//New

        private void btn_Click(object sender, EventArgs e)
        {
            if (fim)
            {
                return;
            }
            timer1.Start();
            Button btn = (Button)sender;
            String[] idx = btn.Name.Split(':');
            int x = int.Parse(idx[0]);
            int y = int.Parse(idx[1]);
            buttons[x, y].Visible = false;
            labels[x, y].Visible = true;
            if (minas[x, y])
            {
                labels[x, y].Text = "@";
                labels[x, y].ForeColor = Color.Red;
                fim = true;
                revelar();
                //timer1.Stop();
            }
            else
            {
                int qt = Contar(x,y);
                if (qt > 0)
                {
                    labels[x, y].Text = qt.ToString();
                    labels[x, y].ForeColor = defColor(qt);
                }
                else
                {
                    HideButtons(x, y);
                }
            }

        }//btn_Click
        private void revelar()
        {
            for (int i = 0; i < WIDTH; i++)
            {
                for (int j = 0; j < HEIGHT; j++)
                {
                    if (minas[i, j])
                    {
                        buttons[i, j].Visible = false;
                        labels[i, j].Visible = true;
                        labels[i, j].Text = "@";
                    }   
                }
            }
        }
         private void btn_MouseUp(object sender, MouseEventArgs e)
         {
             if (fim)
             {
                 return;
             }

             Button btn = (Button)sender;


             if (e.Button == MouseButtons.Right)
             {
                 switch (btn.Text)
                 {
                     case "!":
                         btn.Text = "?";
                         break;
                     case "?":
                         btn.Text = " ";
                         label1.Text = (int.Parse(label1.Text) + 1).ToString();
                         break;
                     default:
                         btn.Text = "!";
                         label1.Text = (int.Parse(label1.Text) - 1).ToString();
                         break;
                 }
             }
            
         }//btn_MouseClick*/
        
         private void HideButtons(int x, int y)
         {
             buttons[x, y].Visible = false;
             labels[x, y].Visible = true;
             int count = Contar(x, y);

             if (count == 0)
             {
                 for (int a = x - 1; a <= x + 1 ; a++)
                 {
                     for (int b = y - 1; b <= y + 1; b++)
                     {
                         if (a >= 0 && a < WIDTH && b >= 0 && b < HEIGHT)
                         {
                             if (buttons[a, b].Visible)
                             {
                                 HideButtons(a, b);
                             }
                         }
                     }
                 }
             }
             else
             {
                 labels[x, y].Text = count.ToString();
                 labels[x, y].ForeColor = defColor(count);
             }//IF count



         }//HideButtons

        private Color defColor(int val)
        {

            switch (val)
            {
                case 1:
                    return Color.Blue;
                    break;
                case 2:
                    return Color.Green;
                    break;
                case 3:
                    return Color.Orange;
                    break;
                case 4:
                    return Color.Red;
                    break;
                default:
                    return Color.Indigo;

            }

        }//defColor*/

        private int Contar(int col, int lin)
        {
            int contador = 0;
            for (int x = col - 1; x <= col + 1; ++x)
            {
                for (int y = lin - 1; y <= lin + 1; ++y)
                {
                    if (x >= 0 && x < WIDTH && y >= 0 && y < HEIGHT)
                    {
                        if (minas[x, y])
                        {
                            contador++;
                        }
                    }
                }//for y
            }//for x
            return contador;
        }//Contar

        private void button1_Click(object sender, EventArgs e)
        {
            New();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (fim)
            {
                return;
            }
            clock++;
            label2.Text = (clock / 600).ToString();
            label2.Text += ":";
            int seg = (clock % 600) / 10;
            label2.Text += seg < 10 ? "0" : "";
            label2.Text += seg.ToString();
            label2.Text += ".";
            label2.Text += clock % 10;

            //label2.Text = clock.ToString();
        }

    }
}
