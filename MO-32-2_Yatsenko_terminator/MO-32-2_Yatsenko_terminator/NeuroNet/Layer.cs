using System;
using System.IO;
using System.Windows.Forms;
using static System.Math;

namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    abstract class Layer
    {
        //поля
        protected string name_Layer;
        string pathDirWeights;
        string pathFileWeights;
        protected int numofneurons;
        protected int numofprevneurons;
        protected const double learningrate = 0.060;
        protected const double momentum = 0.050d;
        protected double[,] lastdeltaweights;
        protected double[,] tempWeights;
        protected Neuron[] neurons;

        //Свойства
        public Neuron[] Neurons { get => neurons; set => neurons = value; }
        public double[] Data
        {
            set
            {
                for(int i = 0; i< numofneurons; i++)
                {
                    Neurons[i].Activator(value);
                }
            }
        }
        //конструктор
        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            int i, j;
            numofneurons = non;
            numofprevneurons = nopn;
            Neurons = new Neuron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";
            if (!Directory.Exists(pathDirWeights))
            {
                Directory.CreateDirectory(pathDirWeights);
            }
            lastdeltaweights = new double[non, nopn + 1];
            double[,] Weights;
            //tempWeights = weightsInitialls(MemoryMod.INIT, pathDirWeights + name_Layer + "memory.csv");
            //weightsInitialls(MemoryMod.SET, pathDirWeights + name_Layer + "memory.csv");
            if (File.Exists(pathFileWeights))
            {
                Weights = weightsInitialls(MemoryMod.GET, pathFileWeights);
            }
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = weightsInitialls(MemoryMod.INIT, pathFileWeights);
                //for (i = 0; i < non; i++)//цикл формиррования нейрона слоя
                //{
                //    double[] tmp_weights = new double[nopn + 1];
                //    for (j = 0; j < nopn + 1; j++)
                //    {
                //        tmp_weights[j] = Weights[i, j];
                //    }
                //    Neurons[i] = new Neuron(tmp_weights, nt);//заполнение массива нейронами
                //}


            }


            for (i = 0; i < non; i++)//цикл формиррования нейрона слоя
            {
                double[] tmp_weights = new double[nopn + 1];
                for (j = 0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                Neurons[i] = new Neuron(tmp_weights, nt);//заполнение массива нейронами
            }
            if (!File.Exists(pathFileWeights))
            {
                weightsInitialls(MemoryMod.SET, pathFileWeights);
            }
        }
        private double[,] weightsInitialls(MemoryMod mm, string path)
        {
            Random random = new Random();
            char[] delim = new char[] { ';', ' ' };
            string tmpStr;
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMod.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_elemnt;
                    for (int i = 0; i < numofneurons; i++)
                    {
                        memory_elemnt = tmpStrWeights[i].Split(delim);
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_elemnt[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }

                    break;

                case MemoryMod.SET:
                    string setPath = path;
                    tmpStr = "";

                    for (int i = 0; i < numofneurons; i++)
                    {
                        tmpStr += Neurons[i].Weights[0].ToString();
                        for (int j = 1; j < numofprevneurons+1; j++)
                        {
                            tmpStr += ";" + Neurons[i].Weights[j].ToString();
                        }
                        tmpStr += "\n";
                    }

                    File.WriteAllText(path, tmpStr);

                    break;

                case MemoryMod.INIT:
                    for(int i = 0; i<numofneurons; i++)
                    {
                        for (int j = 0; j < numofprevneurons+1; j++)
                            weights[i,j] = random.NextDouble();
                    }
                    //нормализация к мат ожиданию=0 и отклонению=1
                    weights = newWeights(numofneurons, numofprevneurons + 1, weights);
                    break;

            }
            return weights;
        }

        private double[,] newWeights(int a, int b, double[,] cweights)//создает отклонение=1 и мат ожидание 0
        {
            double[,] weights = cweights;
            for (int i = 0; i < a; i++)
            {
                double sr = 0;
                for (int j = 0; j < b; j++)
                {
                    sr += weights[i, j];
                }
                sr /= (double)b;
                double s = 0;
                for (int j = 0; j < b; j++)
                {
                    weights[i, j] = weights[i, j] - sr;
                    s += weights[i, j];
                }
                s /= (double)b;//перестраховка 
                double disp = 0;
                for (int j = 0; j < b; j++)
                {
                    disp += Pow(weights[i, j] - s, 2);
                }
                disp /= (double)b;
                for (int j = 0; j < b; j++)
                {
                    weights[i, j] = weights[i, j] / Sqrt(disp);
                }

            }
            return weights;
        }
        abstract public void Recognize(Network net, Layer nextLayer); //для прямых проходов
        abstract public double[] BackwardPass(double[] stuff); //и обратных
    }
}
