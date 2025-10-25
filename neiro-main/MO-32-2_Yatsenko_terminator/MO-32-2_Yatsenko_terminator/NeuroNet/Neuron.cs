using static System.Math;


namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    class Neuron
    {
        //поля
        private NeuronType type; //тип
        private double[] weights; //вес
        private double[] inputs; //входы
        private double output; //выход
        private double derivative; //производная

        //константы для функции активации
        //private double a = 0.01d;

        //свойства
        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }
        public double Output { get => output; }
        public double Derivative { get => derivative; }

        //конструктор
        public Neuron(double[] memoryWeights, NeuronType typeNeuron)
        {
            type = typeNeuron;
            weights = memoryWeights;
        }

        //метод активации нейрона
        public void Activator(double[] i)
        {
            inputs = i; //передача вектора входного сигнала в массив входных данных нейрона
            double sum = weights[0]; //аффиное преобразование через смещение(нулевой вес - порог)
            for(int j = 0; j < inputs.Length; j++)//цикл вычисления индуцированного поля нейрона
            {
                sum += inputs[j] * weights[j + 1];//линейное преобразование входных сигналов
            }
            switch (type)
            {
                case NeuronType.Hidden: //для нейронов скрытого поля
                    output = HyperTg(sum);
                    derivative = HyperTg_Derivator(sum);
                    break;
                case NeuronType.Output: //для нейронов выходного слоя
                    output = Exp(sum);
                    break;
            }
        }

        //функция гиперболический тангенс
        //слои 15 72 35 10
        private double HyperTg(double sum)
        {
            double output = (Exp(sum) - Exp(-sum)) / (Exp(sum) + Exp(-sum));
            return output;
        }
        private double HyperTg_Derivator(double sum)
        {
            double deriv = 4 * Exp(2 * sum) / Pow((Exp(2 * sum) + 1), 2);
            return deriv;
        }
    }
}
