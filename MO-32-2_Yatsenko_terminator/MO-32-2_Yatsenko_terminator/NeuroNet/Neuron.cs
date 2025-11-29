using static System.Math;


namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    class Neuron
    {
        private NeuronType type;
        private double[] weights;
        private double[] inputs;
        private double output;
        private double derivative; //производная 

        //константы 
        private double a = 0.01d;

        //свойства
        public double[] Weights
        {
            get => weights;
            set => weights = value;
        }

        public double[] Inputs { get => inputs; set => inputs = value; }
        public double Output { get => output; } //выходной сигнал   yj
        public double Derivative { get => derivative; }

        public Neuron(double[] memoryWeights, NeuronType typeNeuron)
        {
            type = typeNeuron;
            weights = memoryWeights;
        }

        public void Activator(double[] i)
        {
            inputs = i; //передача вектора входного сигнала в массив 

            //vj и порог
            double sum = weights[0];
            for (int j = 0; j < inputs.Length; j++)
            {
                sum += inputs[j] * weights[j + 1];  //Взвешенная сумма всех входных сигналов
            }

            switch (type)
            {
                case NeuronType.Hidden:
                    output = HpTan(sum);
                    derivative = HpTan_Derivativator(sum);
                    break;
                case NeuronType.Output:
                    output = sum;
                    break;
            }

        }

        private double HpTan(double sum)
        {

            double output = (Exp(sum) - Exp(-sum)) / (Exp(sum) + Exp(-sum));
            return output;
        }
        private double HpTan_Derivativator(double sum)
        {
            //double deriv = 4 / Pow(Exp(sum) + Exp(-sum), 2);
            return 1 - output * output;
            //return deriv;
        }


    }
}
