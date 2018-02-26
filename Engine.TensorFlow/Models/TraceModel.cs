using System;
using tf = TensorFlow;

namespace Engine.TensorFlow.Models
{

    public static class NumpyRandom
    {
        private static readonly Random Rng = new Random();

        public static double Randn()
        {
            var u1 = 1.0 - Rng.NextDouble();
            var u2 = 1.0 - Rng.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return randStdNormal;
        }
    }

    public class TraceModel
    {

        public void Run()
        {

        }

    }
}
