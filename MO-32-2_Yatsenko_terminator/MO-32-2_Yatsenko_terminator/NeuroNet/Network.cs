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

        private double[] fact = new double[10];
        private double[] e_error_avr;
        public double[] Fact { get => fact; }
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }

        public Network() { }
    }
}
