using System;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Brain.Model.DL.Language
{
    [Serializable]
    public class SoftMax: Layer
    {
        // Dimensions.
        private int size_output;
        private int size_input;

        // State.
        private double[][] node_output;
        private double[][] vcx;

        // Parameters.
        private double[] b_node_output;
        private double[][] w_node_output;

        // Gradients.
        private double[] db_node_output;
        private double[][] dw_node_output;

        // Caches.
        private double[] cb_node_output;
        private double[][] cw_node_output;

        public SoftMax(int size_input, int size_output, int _bufferSize)
        {
            this.size_output = size_output;
            this.size_input = size_input;
            BufferSize = _bufferSize;
            ResetState();
            ResetParameters();
            ResetGradients();
            ResetCaches();
        }

        public override int Count()
        {
            return size_output + size_input * size_output;
        }

        public override double[][] Forward(double[][] buffer, bool reset)
        {
            for (var t = 1; t < BufferSize; t++)
            {
                vcx[t] = buffer[t];
                var row_vcx_state = vcx[t];
                var vy = b_node_output.ToArray();
                for(int j=0;j<size_output;j++)
                {
                    var row_w_node_output = w_node_output[j];
                    for (var i = 0; i < size_input; i++)
                        vy[j] += row_vcx_state[i] * row_w_node_output[i];
                }
                node_output[t] = Calculate(vy);
            }

            return node_output;
        }

        public override double[][] Backward(double[][] grads)
        {
            var grads_out = new double[BufferSize][];
            for (var t = BufferSize - 1; t > 0; t--)
            {
                var row_vcx_state = vcx[t];
                var row_grads_out = new double[size_input];
                for(int j = 0; j < size_output; j++)
                {
                    db_node_output[j] += grads[t][j];

                    var row_w_node_output = w_node_output[j];
                    var row_dw_node_output = dw_node_output[j];

                    for (var i = 0; i < size_input; i++)
                    {
                        row_grads_out[i] += row_w_node_output[i] * grads[t][j];
                        row_dw_node_output[i] += row_vcx_state[i] * grads[t][j];
                    }
                }
                grads_out[t] = row_grads_out;
            }

            Update();
            ResetGradients();

            return grads_out;
        }

        protected override void ResetState()
        {
            node_output = new double[BufferSize][];
            vcx = new double[BufferSize][];

            for (var i = 0; i < BufferSize; i++)
            {
                node_output[i] = new double[size_output];
                vcx[i] = new double[size_input];
            }
        }

        protected override void ResetParameters()
        {
            b_node_output = new double[size_output];
            w_node_output = new double[size_output][];

            for (var j = 0; j < size_output; j++)
            {
                w_node_output[j] = new double[size_input];
                for (var i = 0; i < size_input; i++)
                    w_node_output[j][i] = RandomWeight();
            }
        }

        protected override void ResetGradients()
        {
            db_node_output = new double[size_output];
            dw_node_output = new double[size_output][];

            for (var i = 0; i < size_output; i++)
                dw_node_output[i] = new double[size_input];
        }

        protected override void ResetCaches()
        {
            cb_node_output = new double[size_output];
            cw_node_output = new double[size_output][];

            for (var j = 0; j < size_output; j++)
                cw_node_output[j] = new double[size_input];
        }

        protected override void Update()
        {
            for(int j = 0; j < size_output; j++)
            {
                cb_node_output[j] = rmsDecay * cb_node_output[j] + (1 - rmsDecay) * Math.Pow(db_node_output[j], 2);
                b_node_output[j] -= Clip(db_node_output[j]) * LearningRate / Math.Sqrt(cb_node_output[j] + 1e-6);

                for (var i = 0; i < size_input; i++)
                {
                    cw_node_output[j][i] = rmsDecay * cw_node_output[j][i] + (1 - rmsDecay) * Math.Pow(cw_node_output[j][i], 2);
                    w_node_output[j][i] -= Clip(dw_node_output[j][i]) * LearningRate / Math.Sqrt(cw_node_output[j][i] + 1e-6);
                }
            }
        }

        private static double[] Calculate(double[] vx)
        {
            var sum = 0.0;
            var length = vx.Length;
            for (var i = 0; i < length; i++) sum += Math.Exp(vx[i]);

            if (double.IsInfinity(sum)) throw new Exception("Gradient explosion - try lower learning rate.");

            var y = new double[length];
            for (var i = 0; i < length; i++) y[i] = Math.Exp(vx[i]) / sum;
            return y;
        }
    }
}
