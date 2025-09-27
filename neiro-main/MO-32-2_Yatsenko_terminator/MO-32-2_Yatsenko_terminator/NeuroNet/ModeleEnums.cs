using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_32_2_Yatsenko_terminator.NeuroNet
{
    enum MemoryMod //режим работы памяти
    {
        GET, //считывание
        SET, //сохранение
        INIT //инициализация
    }
    enum NeuronType
    {
        Hidden, //скрытый
        Output //выходной
    }
    enum NetworkMode
    {
        Train, //обучение
        Test, //проверка
        Demo //распознование
    }
}
