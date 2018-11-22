namespace Engine.Brain.Entity
{
    public class HiddenNeuron:Neuron
    {
        public double[] Syn1 { get; protected set; }

        public HiddenNeuron(int size)
        {
            Syn1 = new double[size];
        }

    }
}
