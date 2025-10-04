using System;
using System.IO;
using System.Windows.Forms;

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
            numofneurons = nopn;
            Neurons = new Neuron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";
            lastdeltaweights = new double[non, nopn + 1];
            double[,] weights;
            weights = weightsInitialls(MemoryMod.INIT, pathFileWeights);
            weightsInitialls(MemoryMod.SET, pathFileWeights);
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
                    string setPath = AppDomain.CurrentDomain.BaseDirectory + "weights.txt";
                    string tmpStr = "";

                    for (int i = 0; i < numofneurons; i++)
                    {
                        tmpStr += " "+ weights[i,0].ToString();
                        for(int j = 0; j < numofprevneurons; j++)
                        {
                            tmpStr += ";" + weights[i, j].ToString();
                        }
                    }
                    tmpStr += "\n";

                    File.AppendAllText(setPath, tmpStr);

                    break;

                case MemoryMod.INIT:
                    for(int i = 0; i<numofneurons; i++)
                    {
                        for (int j = 0; j < numofprevneurons+1; j++)
                            weights[i,j] = random.NextDouble();
                    }

                    break;

            }
            return weights;
        }
    }
}
