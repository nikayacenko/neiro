using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    class HiddenLayer:Layer
    {
        public HiddenLayer(int non, int nopn, NeuronType nt, string nm_Layer) : base(non, nopn, nt, nm_Layer) { 
        }
    }
}
