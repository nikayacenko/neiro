using System;
using System.IO;
using System.Windows.Forms;
using static System.Math;

namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    abstract class Layer
    {
        protected string name_Layer;
        string pathDirWeights;
        string pathFileWeights;
        protected int numofneurons;
        protected int numofprevneurons;
        //protected const double learningrate = 0.030551;
        //protected const double momentum = 0.005d;

        //protected const double learningrate = 0.0305515;
        //protected const double momentum = 0.005d;

        //protected const double learningrate = 0.0305515585;
        //protected const double momentum = 0.005555501d;

        //protected const double learningrate = 0.039905515585;
        //protected const double momentum = 0.004555509d;

        protected const double learningrate = 0.6115559;
        protected const double momentum = 0.095d;

        //protected const double learningrate = 0.0725d;
        //protected const double momentum = 0.072d;

        protected double[,] lastdeltaweights;
        //protected double[,] temporaryWeights;// массив для проверки SET
        protected Neuron[] neurons;

        public Neuron[] Neurons { get => neurons; set => neurons = value; }
        public double[] Data//передача входных данных
        {
            set
            {
                for (int i = 0; i < numofneurons; ++i)
                {
                    Neurons[i].Activator(value);
                }
            }
        }

        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            //int i, j;
            numofneurons = non;
            numofprevneurons = nopn;
            Neurons = new Neuron[non];//определение массива нейронов
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            lastdeltaweights = new double[non, nopn + 1];
            double[,] temporaryWeights;//временный массив синаптических весов текущего слоя
            if (File.Exists(pathFileWeights))
            {
                temporaryWeights = WeightsInitializer(MemoryMod.GET, pathFileWeights); //или Weights
            }
            else
            {
                Directory.CreateDirectory(pathDirWeights);
                temporaryWeights = WeightsInitializer(MemoryMod.INIT, pathFileWeights);//или Weights
            }
            for (int i = 0; i < non; i++)//цикл формиррования нейрона слоя
            {
                double[] tmp_weights = new double[nopn + 1];
                for (int j = 0; j < nopn + 1; j++)//
                {
                    tmp_weights[j] = temporaryWeights[i, j];// или Weights
                }
                Neurons[i] = new Neuron(tmp_weights, nt);//заполнение массива нейронами
            }
            //temporaryWeights = WeightsInitializer(MemoryMode.INIT, pathDirWeights + name_Layer + "_memory.csv");
            WeightsInitializer(MemoryMod.SET, pathFileWeights);

        }


        public double[,] WeightsInitializer(MemoryMod mm, string path)
        {
            double[,] weights = new double[numofneurons, numofprevneurons + 1];
            char[] delim = new char[] { ';', ' ' };
            string tmpStr;
            string[] tmpStrWeights;


            switch (mm)
            {
                case MemoryMod.GET:
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_element;
                    for (int i = 0; i < numofneurons; i++)
                    {
                        memory_element = tmpStrWeights[i].Split(delim);
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                case MemoryMod.SET:
                    string SETpath = path;
                    string strSET = "";
                    for (int i = 0; i < numofneurons; i++)
                    {
                        strSET += Neurons[i].Weights[0].ToString();//временные
                        for (int j = 1; j < numofprevneurons + 1; j++)
                        {
                            strSET += ";" + Neurons[i].Weights[j];//временные 
                        }
                        strSET += "\n";
                        //strSET += temporaryWeights[i, 0].ToString();//временные
                        //for (int j = 1; j < numofprevneurons + 1; j++)
                        //{
                        //    strSET += ";" + temporaryWeights[i, j];//временные 
                        //}
                        //strSET += "\n";

                    }
                    File.WriteAllText(SETpath, strSET);
                    break;

                case MemoryMod.INIT:
                    weights = RandomInit(numofneurons, numofprevneurons + 1);
                    weights = SrChanger(numofneurons, numofprevneurons + 1, weights);
                    break;


            }
            return weights;
        }

        private double[,] RandomInit(int a, int b)
        {
            double[,] weights = new double[a, b];
            Random random = new Random();
            for (int i = 0; i < a; i++)
            {

                for (int j = 0; j < b; j++)
                {
                    weights[i, j] = random.NextDouble();

                }
            }
            return weights;
        }

        private double[,] SrChanger(int a, int b, double[,] cweights)//создает отклонение=1 и мат ожидание 0
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
                double maxAbs = 0;
                for(int j = 0; j < b; j++)
                {
                    double absVal = Math.Abs(weights[i, j]);
                    if (absVal > maxAbs) maxAbs = absVal;
                }
                if(maxAbs > 0)
                {
                    for(int j = 0; j < b; j++)
                    {
                        weights[i, j] /= maxAbs;
                    }
                }

            }
            return weights;
        }


        abstract public void Recognize(Network net, Layer nextLayer);

        abstract public double[] BackwardPass(double[] stuff);


    }
}
