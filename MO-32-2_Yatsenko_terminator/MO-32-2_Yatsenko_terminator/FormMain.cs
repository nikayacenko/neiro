using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MO_32_2_Yatsenko_terminator.NeuroNet;

namespace MO_32_2_Yatsenko_terminator
{
    public partial class FormMain : Form
    {
        private double[] inputPixel; //массив входных данных
        private Network network; //объявление нейросети
        public FormMain()
        {
            InitializeComponent();

            inputPixel = new double[15];
            network = new Network();
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

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            network.ForwardPass(network, inputPixel);
            label_Output.Text = network.Fact.ToList().IndexOf(network.Fact.Max()).ToString();
            label_Probability.Text = (100 * network.Fact.Max()).ToString("0.00") + " %";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //double[,] trainset;
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            //string[] tmpArrStr;
            //string[] tmpStr;

            //tmpArrStr = File.ReadAllLines(path + "train.txt"); //считывание обучающей выборки
            //trainset = new double[tmpArrStr.Length, 16]; // определение массива выборки

            //for (int i = 0; i < tmpArrStr.Length; i++)
            //{
            //    tmpStr = tmpArrStr[i].Split(' '); // разбиение строки

            //    for (int j = 0; j < 16; j++)
            //    {
            //        trainset[i, j] = double.Parse(tmpStr[j]); //заполнение строки
            //    }
            //}
            //path = "10";
            //Shuffling_Array_Rows(trainset); // перетасовка методом Фишера-Йетса
        }

        private void button_training_Click(object sender, EventArgs e)
        {
            network.Train(network);
            for(int i = 0; i < network.E_error_avr.Length; i++)
            {
                chart_Earn.Series[0].Points.AddY(network.E_error_avr[i]);
            }
            // Добавляем данные точности на второй график (chart_Accuracy)
            for (int i = 0; i < network.Train_accuracy.Length; i++)
            {
                chart_Accuracy.Series[0].Points.AddY(network.Train_accuracy[i]*100);
            }
        }

        private void testing_Click(object sender, EventArgs e)
        {
            network.Test(network);
            for (int i = 0; i < network.E_error_avr.Length; i++)
            {
                chart_Earn.Series[0].Points.AddY(network.E_error_avr[i]);
            }
        }
        //public void Shuffling_Array_Rows(double[,] arr)
        //{
        //    // Метод Фишера-Йетса для перетасовки строк массива
        //    Random random = new Random();
        //    int rowCount = arr.GetLength(0);
        //    int colCount = arr.GetLength(1);

        //    // Проходим от последней строки к первой
        //    for (int i = rowCount - 1; i > 0; i--)
        //    {
        //        // Генерируем случайный индекс от 0 до i
        //        int j = random.Next(0, i + 1);

        //        // Меняем местами строки i и j
        //        for (int col = 0; col < colCount; col++)
        //        {
        //            double temp = arr[i, col];
        //            arr[i, col] = arr[j, col];
        //            arr[j, col] = temp;
        //        }
        //    }
        //}
    }
}
