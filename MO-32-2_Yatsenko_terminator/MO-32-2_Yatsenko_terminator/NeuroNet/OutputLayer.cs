using static System.Math;

namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    internal class OutputLayer : Layer
    {
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }

        private double[] lastGradientSums;

        public override void CalculateAndStoreGradients(double[] errors)
        {
            // Сохраняем gradient sums для следующего слоя
            lastGradientSums = CalculateGradientSums(errors);

            // Накопление градиентов весов
            if (accumulatedGradients == null)
                accumulatedGradients = new double[neurons.Length, numofprevneurons + 1];

            for (int i = 0; i < numofneurons; i++)
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double gradient;
                    if (n == 0)
                        gradient = errors[i];
                    else
                        gradient = neurons[i].Inputs[n - 1] * errors[i];

                    accumulatedGradients[i, n] += gradient;
                }
            }
        }

        public override void ApplyBatchGradients(int batchSize)
        {
            if (accumulatedGradients == null) return;

            for (int i = 0; i < numofneurons; i++)
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double avgGradient = accumulatedGradients[i, n] / batchSize;
                    double deltaw = momentum * lastdeltaweights[i, n] + learningrate * avgGradient;

                    lastdeltaweights[i, n] = deltaw;
                    neurons[i].Weights[n] += deltaw;
                }
            }

            // Сбрасываем накопленные градиенты
            accumulatedGradients = new double[neurons.Length, numofprevneurons + 1];
        }

        public override double[] GetLastGradientSums()
        {
            return lastGradientSums;
        }

        private double[] CalculateGradientSums(double[] errors)
        {
            double[] gr_sum = new double[numofprevneurons];
            for (int j = 0; j < numofprevneurons; j++)
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)
                {
                    sum += neurons[k].Weights[j + 1] * errors[k];
                }
                gr_sum[j] = sum;
            }
            return gr_sum;
        }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double e_sum = 0;
            for (int i = 0; i < neurons.Length; i++)
            {
                e_sum += Exp(neurons[i].Output);//перетащила софтмакс сюда полностью
            }

            for (int i = 0; i < neurons.Length; i++)
            {
                net.Fact[i] = Exp(neurons[i].Output) / e_sum;
            }
        }

        public override double[] BackwardPass(double[] errors)
        {
            double[] gr_sum = new double[numofprevneurons];//так как softmax на выходном слое то учитвается порог изза энтропии(?)
            for (int j = 0; j < numofprevneurons; j++)//цикл по нейронам второго скрытого слоя
            {
                double sum = 0;
                for (int k = 0; k < numofneurons; k++)//цикл по нейронам выходного слоя
                {
                    sum += neurons[k].Weights[j + 1] * errors[k]; //синаптические веса выходного слоя * ошибку

                }
                gr_sum[j] = sum;//массив для передачи в предыдущий слой
            }

            for (int i = 0; i < numofneurons; i++)//цикл коррекции синаптических весов
            {
                for (int n = 0; n < numofprevneurons + 1; n++)
                {
                    double deltaw;
                    if (n == 0) //если порог
                    {
                        deltaw = momentum * lastdeltaweights[i, n] + learningrate * errors[i];
                    }
                    else deltaw = momentum * lastdeltaweights[i, n] + learningrate * errors[i] * neurons[i].Inputs[n - 1];
                    lastdeltaweights[i, n] = deltaw;
                    neurons[i].Weights[n] += deltaw;
                }

            }
            return gr_sum;
        }

        public override void DropOut(double percent)
        {
            base.DropOut(percent);
        }

    }
}
