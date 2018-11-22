using System.Collections.Generic;

namespace Engine.Brain.Entity
{


    public class WordNeuron : Neuron
    {

        public string Name { get; protected set; }

        public double[] Syn0  { get; protected set; }

        public List<Neuron> Neurons { get; protected set; }

        public int[] CodeArray { get; protected set; }


        public WordNeuron(string name, double freq,int size)
        {
            Name = name;
            Freq = freq;
            Syn0 = new double[size];
            //
            for (int i = 0; i < Syn0.Length; i++)
                Syn0[i] = (NP.Random() - 0.5) / size;
        }

        public WordNeuron(string name,double freq,int category,int size)
        {
            Name = name;
            Freq = freq;
            Syn0 = new double[size];
            Category = category;
            //
            for (int i = 0; i < Syn0.Length; i++)
                Syn0[i] = (NP.Random() - 0.5) / size;
        }


        public List<Neuron> MakeNeurons()
        {
            if (Neurons != null)
            {
                return Neurons;
            }
            Neuron neuron = this;
            Neurons = new List<Neuron>();
            while ((neuron = neuron.Parent) != null)
            {
                Neurons.Add(neuron);
            }
            Neurons.Reverse();
            CodeArray = new int[Neurons.Count];
            for (int i = 1; i < Neurons.Count; i++)
            {
                CodeArray[i - 1] = Neurons[i].Code;
            }
            CodeArray[CodeArray.Length - 1] = Code;
            return Neurons;
        }


    }
}
