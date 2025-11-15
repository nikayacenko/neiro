using System;
using System.IO;

namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    internal class InputLayer
    {
        // поля
        private double[,] trainset; // 100 изображений в обучающей выборке
        private double[,] testset; // 10 в тестовой выборке

        // свойства
        public double[,] Trainset { get => trainset; }
        public double[,] Testset { get => testset; }

        //конструктор
        public InputLayer(NetworkMode nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] tmpArrStr;
            string[] tmpStr;

            switch (nm)
            {
                case NetworkMode.Train:
                    tmpArrStr = File.ReadAllLines(path + "train.txt"); //считывание обучающей выборки
                    trainset = new double[tmpArrStr.Length, 16]; // определение массива выборки

                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' '); // разбиение строки

                        for (int j = 0; j < 16; j++)
                        {
                            trainset[i, j] = double.Parse(tmpStr[j]); //заполнение строки
                        }
                    }
                    Shuffling_Array_Rows(trainset); // перетасовка методом Фишера-Йетса
                    break;
                case NetworkMode.Test: // тестовая выборка
                    tmpArrStr = File.ReadAllLines(path + "test.txt");
                    testset = new double[tmpArrStr.Length, 16];

                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');

                        for (int j = 0; j < 16; j++)
                        {
                            testset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    Shuffling_Array_Rows(testset);
                    break;
            }
        }

        // перетасовка методом Фишера-Йетса
        public void Shuffling_Array_Rows(double[,] arr)
        {
            Random random = new Random();
            int rows = arr.GetLength(0);
            int cols = arr.GetLength(1);
            for (int i = rows - 1; i >= 1; i--)
            {

                int r = random.Next(i);
                for (int j = 0; j < cols; j++)
                {
                    double temp = arr[i, j];
                    arr[i, j] = arr[r, j];
                    arr[r, j] = temp;
                }

            }
        }
    }
}
