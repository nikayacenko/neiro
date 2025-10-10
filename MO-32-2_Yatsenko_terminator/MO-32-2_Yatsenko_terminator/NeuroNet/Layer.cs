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
            //double[,] weights;
            tempWeights = weightsInitialls(MemoryMod.INIT, pathDirWeights + name_Layer + "memory.csv");
            weightsInitialls(MemoryMod.SET, pathDirWeights + name_Layer + "memory.csv");
        }
        private double[,] weightsInitialls(MemoryMod mm, string path)
        {
            Random random = new Random();
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMod.GET:

                    break;

                case MemoryMod.SET:
                    string setPath = path;
                    string tmpStr = "";

                    for (int i = 0; i < numofneurons; i++)
                    {
                        tmpStr += " "+ tempWeights[i,0].ToString();
                        for(int j = 0; j < numofprevneurons; j++)
                        {
                            tmpStr += ";" + tempWeights[i, j].ToString();
                        }
                        tmpStr += "\n";
                    }

                    File.AppendAllText(setPath, tmpStr);

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
        private double[,] newWeights(int a, int b, double[,] inputWeights)
        {
            double[,] weights = inputWeights;
            for(int i = 0;i < a; i++)
            {
                double sr = calculateSr(weights, i, b);
                double deviation = calculateDev(weights, i, b, sr);
                normalizeWeights(weights, i, b, sr, deviation);

                //for (int j = 0; j < b; j++)
                //{
                //    if (weights[i, j] > 1) weights[i, j] = 1;
                //    if (weights[i, j] < -1) weights[i, j] = -1;
                //}
            }

            return weights;
        }
        private double calculateSr(double[,]weights,int row, int lenghts)
        {
            double sum = 0;
            for(int i = 0;i<lenghts;i++)
            {
                sum += weights[row, i];
            }
            return sum / lenghts;
        }
        private double calculateDev(double[,]weights,int row,int length,double sr)
        {
            double sum = 0;
            for(int i = 0;i<length;i++)
            {
                double dif = weights[row, i]-sr;
                sum += Pow(dif, 2);
            }
            return Sqrt(sum/length);
        }
        private void normalizeWeights(double[,]weights, int row, int lenghts,double sr, double dev) 
        { 
            for(int j = 0;j<lenghts;j++)
            {
                weights[row, j] = (weights[row, j] - sr) / dev;
            }
        }
    }
}
