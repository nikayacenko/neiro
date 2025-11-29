using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    class Network
    {
        //слои 15 72 35 10
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(72, 15, NeuronType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(35, 72, NeuronType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 35, NeuronType.Output, nameof(output_layer));

        private double[] fact = new double[10];//массив фактического выхода
        private double[] e_error_avr;//среднее значение энергии ошибки  (cумма квадратов ошибок
        private double[] train_accuracy; // Точность на обучающей выборке

        public double[] Fact { get => fact; }
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }
        public double[] Train_accuracy { get => train_accuracy; set => train_accuracy = value; }

        public Network() { }

        public void ForwardPass(Network net, double[] netInput)
        {
            net.hidden_layer1.Data = netInput;
            net.hidden_layer1.Recognize(null, net.hidden_layer2);
            net.hidden_layer2.Recognize(null, net.output_layer);
            net.output_layer.Recognize(net, null);
        }

        // Упрощенная версия с мини-батчами
        public void Train(Network net)
        {
            net.input_layer = new InputLayer(NetworkMode.Train);
            int epoches = 12;
            int batchSize = 8; // размер мини-батча

            e_error_avr = new double[epoches];
            train_accuracy = new double[epoches];

            int totalSamples = net.input_layer.Trainset.GetLength(0);
            int totalBatches = (int)Math.Ceiling((double)totalSamples / batchSize);

            for (int k = 0; k < epoches; k++)
            {
                e_error_avr[k] = 0;
                train_accuracy[k] = 0;
                int correctPredictions = 0;

                net.input_layer.Shuffling_Array_Rows(net.input_layer.Trainset);

                // Обрабатываем батчами
                for (int batch = 0; batch < totalBatches; batch++)
                {
                    int startIndex = batch * batchSize;
                    int endIndex = Math.Min(startIndex + batchSize, totalSamples);
                    int currentBatchSize = endIndex - startIndex;

                    double batchError = 0;

                    // Накопление градиентов для текущего батча
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        double[] tmpTrain = new double[15];
                        for (int j = 0; j < tmpTrain.Length; j++)
                        {
                            tmpTrain[j] = net.input_layer.Trainset[i, j + 1];
                        }

                        // Прямой проход
                        ForwardPass(net, tmpTrain);

                        // Подсчет точности
                        int predictedClass = net.Fact.ToList().IndexOf(net.Fact.Max());
                        int actualClass = (int)net.input_layer.Trainset[i, 0];
                        if (predictedClass == actualClass)
                        {
                            correctPredictions++;
                        }

                        // Вычисление ошибки
                        double tmpSumError = 0;
                        double[] errors = new double[net.fact.Length];
                        for (int x = 0; x < errors.Length; x++)
                        {
                            if (x == actualClass)
                            {
                                errors[x] = 1.0 - net.fact[x];
                            }
                            else
                            {
                                errors[x] = -net.fact[x];
                            }
                            tmpSumError += errors[x] * errors[x] / 2;
                        }
                        batchError += tmpSumError / errors.Length;

                        // Обратный проход - накапливаем градиенты
                        // Временно сохраняем вычисленные градиенты
                        net.output_layer.CalculateAndStoreGradients(errors);
                        net.hidden_layer2.CalculateAndStoreGradients(net.output_layer.GetLastGradientSums());
                        net.hidden_layer1.CalculateAndStoreGradients(net.hidden_layer2.GetLastGradientSums());
                    }

                    // Применяем усредненные градиенты для батча
                    net.output_layer.ApplyBatchGradients(currentBatchSize);
                    net.hidden_layer2.ApplyBatchGradients(currentBatchSize);
                    net.hidden_layer1.ApplyBatchGradients(currentBatchSize);

                    e_error_avr[k] += batchError;
                }

                e_error_avr[k] /= totalSamples;
                train_accuracy[k] = (double)correctPredictions / totalSamples;

                // Вывод прогресса для отладки
                Console.WriteLine($"Epoch {k + 1}: Error = {e_error_avr[k]:F4}, Accuracy = {train_accuracy[k]:P2}");
            }

            net.input_layer = null;

            //запись скорректированных весов в память
            net.hidden_layer1.WeightsInitializer(MemoryMod.SET, nameof(hidden_layer1) + "_memory.csv");
            net.hidden_layer2.WeightsInitializer(MemoryMod.SET, nameof(hidden_layer2) + "_memory.csv");
            net.output_layer.WeightsInitializer(MemoryMod.SET, nameof(output_layer) + "_memory.csv");
        }

        // Тестирование без изменений
        public void Test(Network net)
        {
            net.input_layer = new InputLayer(NetworkMode.Test);
            int epoches = 5;
            double tmpSumError;
            double[] errors;

            e_error_avr = new double[epoches];
            for (int k = 0; k < epoches; k++)
            {
                e_error_avr[k] = 0;
                net.input_layer.Shuffling_Array_Rows(net.input_layer.Testset);
                for (int i = 0; i < net.input_layer.Testset.GetLength(0); i++)
                {
                    double[] tmpTrain = new double[15];
                    for (int j = 0; j < tmpTrain.Length; j++)
                    {
                        tmpTrain[j] = net.input_layer.Testset[i, j + 1];
                    }

                    ForwardPass(net, tmpTrain);

                    tmpSumError = 0;
                    errors = new double[net.fact.Length];
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == net.input_layer.Testset[i, 0])
                        {
                            errors[x] = 1.0 - net.fact[x];
                        }
                        else
                        {
                            errors[x] = -net.fact[x];
                        }
                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length;
                }
                e_error_avr[k] /= net.input_layer.Testset.GetLength(0);
            }

            net.input_layer = null;
        }
    }
}