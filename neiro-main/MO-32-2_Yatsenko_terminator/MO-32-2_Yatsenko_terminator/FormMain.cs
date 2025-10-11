using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MO_32_2_Yatsenko_terminator.NeuroNet;

namespace MO_32_2_Yatsenko_terminator
{
    public partial class FormMain : Form
    {
        private double[] inputPixel; //массив входных данных
        public FormMain()
        {
            InitializeComponent();

            inputPixel = new double[15];
        }

        //обработчик события клика пиксельной кнопки
        private void changing_state_pixel(object sender, EventArgs e)
        {
            //если кнопка изначально белая
            if (((Button)sender).BackColor == Color.OrangeRed)
            {
                ((Button)sender).BackColor = Color.MidnightBlue;
                inputPixel[((Button)sender).TabIndex] = 1d;
            }
            else
            {
                ((Button)sender).BackColor = Color.OrangeRed;
                inputPixel[((Button)sender).TabIndex] = 0d;
            }
        }

        private void button_SaveTrainSample_Click(Object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown_NecesseryOutput.Value.ToString();

            for(int i = 0; i < inputPixel.Length; i++)
            {
                tmpStr += " " + inputPixel[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr);
        }
        private void button_SaveTestSample_Click(Object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = numericUpDown_NecesseryOutput.Value.ToString();

            for (int i = 0; i < inputPixel.Length; i++)
            {
                tmpStr += " " + inputPixel[i].ToString();
            }
            tmpStr += "\n";

            File.AppendAllText(path, tmpStr);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            HiddenLayer hiddenLayer1 = new HiddenLayer(5, 7, NeuronType.Hidden, nameof(hiddenLayer1));
        }

    }
}
