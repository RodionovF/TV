using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_2_TV
{
    public partial class Form1 : Form
    {
        public int n;
        public int size;
        double min;
        double max;
        double[] ksi = null;
        double[] zi = null;
        double[] ni = null;
        double[] board = null;
        double lambda;
        public Form1()
        {
            InitializeComponent();
        }
        public void Computing()
            {
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.Rows.Clear();
            var random = new Random(DateTime.Now.Millisecond);
            double tmp;
            lambda = Convert.ToDouble(textBox1.Text); // параметр распределения
            

            n = Convert.ToInt32(textBox2.Text);
           ksi = new double[n];

            for (int j = 0; j < n; ++j)
            {
                ksi[j] = 0;
            }
            for (int j = 0; j < n; ++j)
            {
                tmp = random.NextDouble();
                ksi[j] = -((Math.Log(tmp)) / lambda);
            }
            Array.Sort(ksi);
            min = ksi[0];
            max = ksi[n - 1];
           for (int j = 0; j < n; ++j)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[j].Cells[0].Value = Convert.ToString(j);
                dataGridView1.Rows[j].Cells[1].Value = Convert.ToString(ksi[j]);

            }
        }

        public void Counted()
        {
            double sum=0;
            double sum2 = 0;
            dataGridView4.Rows[0].Cells[0].Value = Convert.ToString(1/lambda);
            dataGridView4.Rows[0].Cells[1].Value = Convert.ToString(1 / (lambda*lambda));
            for(int i =0; i<n;i++)
            {
                sum += ksi[i];
            }
            dataGridView4.Rows[0].Cells[2].Value = Convert.ToString(sum/n);
            dataGridView4.Rows[0].Cells[3].Value = Convert.ToString(Math.Abs(sum / n - 1/lambda));
            for (int i = 0; i < n; i++)
            {
                sum2 += Math.Pow((ksi[i] - sum/n),2);
            }
            dataGridView4.Rows[0].Cells[4].Value = Convert.ToString(sum2 / n);
            dataGridView4.Rows[0].Cells[5].Value = Convert.ToString(Math.Abs(sum2 / n - 1 / (lambda*lambda)));
            if (n % 2 == 1)
                dataGridView4.Rows[0].Cells[6].Value = Convert.ToString(ksi[n / 2]);
            else
                dataGridView4.Rows[0].Cells[6].Value = Convert.ToString((ksi[n / 2 -1] + ksi[n / 2 ]) / 2);
            dataGridView4.Rows[0].Cells[7].Value = Convert.ToString(max-min);
        }
        public void Painted()
        {
            chart2.Series["Функция Распределения"].Points.Clear();
            chart2.Series["Выборочная Функция Распределения"].Points.Clear();
            chart2.Series["points"].Points.Clear();
            int ci=0;
            double eps = 0.01;
            double max_raz = 0;
            double c1 = 0;
            double c2 = 0;
            double[] psi = new double[n+1];
            psi[0] = 0;
            for (int i = 1; i < n+1; i++)
            {
                
                psi[i] = ksi[i-1]+eps;
            }
            chart2.Series["Функция Распределения"].Points.AddXY(0,0);
            for (int i = 0; i < n+1; i++)
            {
                c1 = 1 - Math.Pow(Math.E, -lambda * psi[i]);
                chart2.Series["Функция Распределения"].Points.AddXY(psi[i] - eps, c1);
                chart2.Series["points"].Points.AddXY(psi[i] - eps, 1 - Math.Pow(Math.E, -lambda * psi[i]));
                //for ( int j=0; j<n; j++)
                //{
                //    if (psi[i] > ksi[j])
                //        ci++;
                //    else break;
                //}
                c2 = (double)(i) / n;
                chart2.Series["Выборочная Функция Распределения"].Points.AddXY(psi[i], c2);
                ci = 0;
                if (Math.Abs(c2 - c1) > max_raz)
                    max_raz = Math.Abs(c2 - c1);
            }
            label6.Text = Convert.ToString(max_raz);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Computing();
            Counted();
            Painted();
        }

        public void Form_cotrol(TextBox textBox1, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
            {
                // цифра
                return;
            }

            if (e.KeyChar == '.')
            {
                // точку заменим запятой
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',')
            {
                if (textBox1.Text.IndexOf(',') != -1)
                {

                    // запятая уже есть в поле редактирования
                    e.Handled = true;
                }
                return;
            }

            if (Char.IsControl(e.KeyChar))
            {
                // <Enter>, <Backspace>, <Esc>
                if (e.KeyChar == (char)Keys.Enter)
                    // нажата клавиша <Enter>
                    // установить курсор на кнопку OK
                    button1.Focus();
                return;
            }

            // остальные символы запрещены
            e.Handled = true;
        }

        public void mod_form_conrtol(KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
            {
                // цифра
                return;
            }

            if (Char.IsControl(e.KeyChar))
            {
                // <Enter>, <Backspace>, <Esc>
                if (e.KeyChar == (char)Keys.Enter)
                    // нажата клавиша <Enter>
                    // установить курсор на кнопку OK
                    button1.Focus();
                return;
            }

            // остальные символы запрещены
            e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Form_cotrol(textBox1, e);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            mod_form_conrtol(e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Series["Гистограмма"].Points.Clear();
            chart1.Series["Теоритическая плотность"].Points.Clear();
            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }

            dataGridView3.Rows.Clear();
            dataGridView3.ColumnCount = size+1;
            dataGridView3.RowCount = 3;


            for (int j = 0; j < size+1; ++j)
            {
                board[j] = Convert.ToDouble(dataGridView2.Rows[j].Cells[0].Value);
                dataGridView3.Rows[0].Cells[j].Value = Convert.ToString(board[j]); // Для ТВ практики
            }
            //for (int j = 0; j < size; ++j)
            //{

            //    zi[j] = (Convert.ToDouble(dataGridView2.Rows[j].Cells[0].Value) + Convert.ToDouble(dataGridView2.Rows[j + 1].Cells[0].Value)) / (double)2;
            //    dataGridView3.Rows[0].Cells[j].Value = Convert.ToString(zi[j]);
            //}

            Refresh();

            for (int i = 0; i < n; i++)
            {
                for (int j = 1; j < size + 1; j++)
                {
                    if ((ksi[i] <= board[j]) && (ksi[i] >= board[j - 1]))
                    {
                        ni[j - 1]++;


                        break;
                    }
                }

            }

            for (int j = 0; j < size; j++)
            {

                dataGridView3.Rows[1].Cells[j].Value = Convert.ToString(ni[j]);
                dataGridView3.Rows[2].Cells[j].Value = Convert.ToString(ni[j] / n);
            }

            for (int i = 0; i < size; i++)
            {
                chart1.Series["Гистограмма"].Points.AddXY(Convert.ToDouble(dataGridView3.Rows[0].Cells[i].Value), Convert.ToDouble(dataGridView3.Rows[2].Cells[i].Value));
                chart1.Series["Теоритическая плотность"].Points.AddXY(Convert.ToDouble(dataGridView3.Rows[0].Cells[i].Value), Convert.ToDouble(dataGridView3.Rows[1].Cells[i].Value));
            }
            double maxx = 0;
            for (int i = 0; i < size; i++)
            { 
                if (Math.Abs(Convert.ToDouble(dataGridView3.Rows[1].Cells[i].Value) - Convert.ToDouble(dataGridView3.Rows[2].Cells[i].Value)) > maxx)
                maxx = Math.Abs(Convert.ToDouble(dataGridView3.Rows[1].Cells[i].Value) - Convert.ToDouble(dataGridView3.Rows[2].Cells[i].Value));
        }
            label4.Text = Convert.ToString(maxx);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series["Теоритическая плотность"].Points.Clear();
            chart1.Series["Гистограмма"].Points.Clear();
            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView2.RowCount = size;
            dataGridView2.ColumnCount = 1;
            dataGridView3.ColumnCount = size+1;
            dataGridView3.RowCount = 3;


            for (int j = 0; j < size + 1; ++j)
            {
                dataGridView2.Rows.Add();
                board[j] = (min + ((max - min) / size) * j);
                dataGridView2.Rows[j].Cells[0].Value = Convert.ToString(board[j]);

            }
            for (int j = 0; j < size+1; ++j)
            {
                dataGridView3.Rows[0].Cells[j].Value = Convert.ToString(board[j]); // Для ТВ практики
            } // Для ТВ практики

            //    zi[j] = (Convert.ToDouble(dataGridView2.Rows[j].Cells[0].Value) + Convert.ToDouble(dataGridView2.Rows[j + 1].Cells[0].Value)) / (double)2;
            //    dataGridView3.Rows[0].Cells[j].Value = Convert.ToString(zi[j]);
            //}

            Refresh();

            for (int i = 0; i < n; i++)
            {
                for (int j = 1; j < size + 1; j++)
                {
                    if ((ksi[i] <= board[j]) && (ksi[i] >= board[j - 1]))
                    {
                        ni[j - 1]++;


                        break;
                    }
                }

            }

            for (int j = 0; j < size; j++)
            {
                dataGridView3.Rows[1].Cells[j].Value = Convert.ToString(ni[j]);
                dataGridView3.Rows[2].Cells[j].Value = Convert.ToString(ni[j]/n);
            }

            for (int i = 0; i < size; i++)
            {
                chart1.Series["Гистограмма"].Points.AddXY(Convert.ToDouble(dataGridView3.Rows[0].Cells[i].Value), Convert.ToDouble(dataGridView3.Rows[2].Cells[i].Value));
                chart1.Series["Теоритическая плотность"].Points.AddXY(Convert.ToDouble(dataGridView3.Rows[0].Cells[i].Value), Convert.ToDouble(dataGridView3.Rows[1].Cells[i].Value));
            }

            double maxx = 0;
            for (int i = 0; i < size; i++)
            {
                if (Math.Abs(Convert.ToDouble(dataGridView3.Rows[1].Cells[i].Value) - Convert.ToDouble(dataGridView3.Rows[2].Cells[i].Value)) > maxx)
                    maxx = Math.Abs(Convert.ToDouble(dataGridView3.Rows[1].Cells[i].Value) - Convert.ToDouble(dataGridView3.Rows[2].Cells[i].Value));
            }
            label4.Text = Convert.ToString(maxx);

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            size = Convert.ToInt32(textBox3.Text);
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.Rows.Clear();
            dataGridView2.RowCount = size+1;
            dataGridView2.ColumnCount = 1;
            dataGridView2.Rows[0].Cells[0].Value = Convert.ToString(min);
            dataGridView2.Rows[size].Cells[0].Value = Convert.ToString(max);

            dataGridView3.Rows.Clear();
            dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView3.ColumnCount = size;
            dataGridView3.RowCount = 3;
            ni = new double[size+1];
            zi = new double[size + 1];
            board = new double[size + 1];
            for (int j = 0; j < size; ++j)
            {
                ni[j] = 0;
            }
            Refresh();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            mod_form_conrtol(e);
        }
    }
}
