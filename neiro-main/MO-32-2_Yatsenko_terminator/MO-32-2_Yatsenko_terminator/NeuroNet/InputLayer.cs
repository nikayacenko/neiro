using System;
using System.IO;

namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    class InputLayer
    {
        //поля
        private double[,] trainset; //100 изображений в обучающей выборке
        private double[,] testset; //10 изображений в тестовой выборке

        //свойства
        public double[,] Trainset { get => trainset; }
        public double [,] Testset { get => testset; }

        //конструктор
        public InputLayer(NetworkMode nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] tmpArrStr;
            string[] tmpStr;
            switch (nm)
            {
                case NetworkMode.Train:
                    tmpArrStr = File.ReadAllLines(path + "train.txt");
                    trainset = new double[tmpArrStr.Length, 16];

                    for(int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');

                        for(int j = 0; j < 16; j++)
                        {
                            trainset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    Shuffling_Array_Rows(trainset);
                    break;
                case NetworkMode.Test:
                    tmpArrStr = File.ReadAllLines(path + "test.txt"); //счетывание из файла тестовой выборкм
                    testset = new double[tmpArrStr.Length, 16]; //определение массива тестовой выборки

                    for (int i = 0; i < tmpArrStr.Length; i++) //цикл перебора строк тестовой выборки
                    {
                        tmpStr = tmpArrStr[i].Split(' '); //разбиение i-ой строки на массив отльных симоволов

                        for (int j = 0; j < 16; j++) //цикл заполнения j-ой строки тестовой выборки
                        {
                            testset[i, j] = double.Parse(tmpStr[j]); //строковое значение числа преобразуется
                        }
                    }
                    Shuffling_Array_Rows(testset); //перетасовка тестовой выборкм методом Фишера-Йетса
                    break;
            }
        }
        public void Shuffling_Array_Rows(double[,]arr)
        {
            //меод фишера йетса
            //написать код
        }
    }
}
