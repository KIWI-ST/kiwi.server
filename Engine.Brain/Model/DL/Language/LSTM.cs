using System;
using System.Linq;
using System.Threading.Tasks;

namespace Engine.Brain.Model.DL.Language
{
    public class LSTM : Layer
    {
        // Dimensions.
        private int size_output;
        private int size_input;
        private int size_total;

        // State.
        private double[][] gate_input;
        private double[][] gate_forget;
        private double[][] gate_output;
        private double[][] node_input;
        private double[][] node_cell;
        private double[][] node_output;
        private double[][] vcx;

        // Parameters.
        private double[] b_gate_input;
        private double[] b_gate_forget;
        private double[] b_gate_output;
        private double[] b_node_input;
        private double[][] w_gate_input;
        private double[][] w_gate_forget;
        private double[][] w_gate_output;
        private double[][] w_node_input;

        // Gradients.
        private double[] db_gate_input;
        private double[] db_gate_forget;
        private double[] db_gate_output;
        private double[] db_node_input;
        private double[][] dw_gate_input;
        private double[][] dw_gate_forget;
        private double[][] dw_gate_output;
        private double[][] dw_node_input;

        // RmsProp Caches.
        private double[] cb_gate_input;
        private double[] cb_gate_forget;
        private double[] cb_gate_output;
        private double[] cb_node_input;
        private double[][] cw_gate_input;
        private double[][] cw_gate_forget;
        private double[][] cw_gate_output;
        private double[][] cw_node_input;

        public override int Count()
        {
            return size_output * 4 + size_total * size_output * 4;
        }

        public LSTM(int size_input, int size_output)
        {
            this.size_output = size_output;
            this.size_input = size_input;
            size_total = size_input + size_output;

            ResetState();
            ResetParameters();
            ResetGradients();
            ResetCaches();
        }

        public override double[][] Forward(double[][] buffer, bool reset)
        {
            if (reset)
            {
                node_cell[0] = new double[size_output];
                node_output[0] = new double[size_output];
            }
            else
            {
                node_cell[0] = node_cell[BufferSize - 1].ToArray();
                node_output[0] = node_output[BufferSize - 1].ToArray();
            }

            for (var t = 1; t < BufferSize; t++)
            {
                buffer[t].CopyTo(vcx[t], 0);
                node_output[t - 1].CopyTo(vcx[t], size_input);
                var row_vcx_state = vcx[t];

                var row_gate_input = gate_input[t];
                var row_gate_forget = gate_forget[t];
                var row_gate_output = gate_output[t];
                var row_node_input = node_input[t];

                Parallel.For(0, size_output, options, j =>
                {
                    row_gate_input[j] = b_gate_input[j];
                    row_gate_forget[j] = b_gate_forget[j];
                    row_gate_output[j] = b_gate_output[j];
                    row_node_input[j] = b_node_input[j];

                    var row_w_gate_input = w_gate_input[j];
                    var row_w_gate_forget = w_gate_forget[j];
                    var row_w_gate_ouput = w_gate_output[j];
                    var row_w_node_input = w_node_input[j];

                    for (var i = 0; i < size_total; i++)
                    {
                        row_gate_input[j] += row_w_gate_input[i] * row_vcx_state[i];
                        row_gate_forget[j] += row_w_gate_forget[i] * row_vcx_state[i];
                        row_gate_output[j] += row_w_gate_ouput[i] * row_vcx_state[i];
                        row_node_input[j] += row_w_node_input[i] * row_vcx_state[i];
                    }

                    row_gate_input[j] = Sigmoid(row_gate_input[j]);
                    row_gate_forget[j] = Sigmoid(row_gate_forget[j]);
                    row_gate_output[j] = Sigmoid(row_gate_output[j]);
                    row_node_input[j] = Tanh(row_node_input[j]);
                });

                var row_node_cell = node_cell[t];
                var row_node_cell_p = node_cell[t - 1];
                var row_node_output = node_output[t];

                for (var i = 0; i < size_output; i++)
                {
                    row_node_cell[i] = row_node_input[i] * row_gate_input[i] + row_node_cell_p[i] * row_gate_forget[i];
                    row_node_output[i] = Tanh(row_node_cell[i]) * row_gate_output[i];
                }
            }

            return node_output;
        }

        public override double[][] Backward(double[][] grads)
        {
            var grads_out = new double[BufferSize][];
            var d_node_cell = new double[size_output];
            var d_node_output = new double[size_output];

            for (var t = BufferSize - 1; t > 0; t--)
            {
                grads_out[t] = new double[size_input];

                var row_grads_out = grads_out[t];
                var row_gate_input = gate_input[t];
                var row_gate_forget = gate_forget[t];
                var row_gate_output = gate_output[t];

                var row_node_input = node_input[t];
                var row_node_cell = node_cell[t];
                var row_node_cell_p = node_cell[t - 1];
                var row_node_output = node_output[t];

                Parallel.For(0, size_output, options, j =>
                {
                    d_node_output[j] += Clip(grads[t][j]);
                    var tanh_cell = Tanh(row_node_cell[j]);
                    var d_gate_output = tanh_cell * d_node_output[j];

                    d_node_cell[j] += row_gate_output[j] * d_node_output[j] * (1 - tanh_cell * tanh_cell);

                    var d_gate_forget = row_node_cell_p[j] * d_node_cell[j];
                    var d_gate_input = row_node_input[j] * d_node_cell[j];
                    var d_node_input = row_gate_input[j] * d_node_cell[j];

                    d_node_cell[j] = gate_forget[t][j] * d_node_cell[j];

                    d_node_input = dTanh(row_node_input[j]) * d_node_input;
                    d_gate_input = dSigmoid(row_gate_input[j]) * d_gate_input;
                    d_gate_forget = dSigmoid(row_gate_forget[j]) * d_gate_forget;
                    d_gate_output = dSigmoid(row_gate_output[j]) * d_gate_output;

                    db_node_input[j] += d_node_input;
                    db_gate_input[j] += d_gate_input;
                    db_gate_forget[j] += d_gate_forget;
                    db_gate_output[j] += d_gate_output;

                    var row_dw_node_input = dw_node_input[j];
                    var row_dw_gate_input = dw_gate_input[j];
                    var row_dw_gate_forget = dw_gate_forget[j];
                    var row_dw_gate_output = dw_gate_output[j];

                    var row_w_node_input = w_node_input[j];
                    var row_w_gate_input = w_gate_input[j];
                    var row_w_gate_forget = w_gate_forget[j];
                    var row_w_gate_output = w_gate_output[j];

                    var row_vcx_state = vcx[t];

                    for (var i = 0; i < size_total; i++)
                    {
                        row_dw_node_input[i] += d_node_input * row_vcx_state[i];
                        row_dw_gate_input[i] += d_gate_input * row_vcx_state[i];
                        row_dw_gate_forget[i] += d_gate_forget * row_vcx_state[i];
                        row_dw_gate_output[i] += d_gate_output * row_vcx_state[i];

                        if (i < size_input)
                            row_grads_out[i] += row_w_node_input[i] * d_node_input;
                        else
                        {
                            d_node_output[i - size_input] = row_w_node_input[i] * d_node_input;
                            d_node_output[i - size_input] += row_w_gate_input[i] * d_gate_input;
                            d_node_output[i - size_input] += row_w_gate_forget[i] * d_gate_forget;
                            d_node_output[i - size_input] += row_w_gate_output[i] * d_gate_output;
                        }
                    }
                });
            }

            Update();
            ResetGradients();

            return grads_out;
        }

        protected override void ResetState()
        {
            gate_output = new double[BufferSize][];
            gate_forget = new double[BufferSize][];
            node_input = new double[BufferSize][];
            gate_input = new double[BufferSize][];
            node_output = new double[BufferSize][];
            node_cell = new double[BufferSize][];
            vcx = new double[BufferSize][];

            for (var i = 0; i < BufferSize; i++)
            {
                gate_output[i] = new double[size_output];
                gate_forget[i] = new double[size_output];
                gate_input[i] = new double[size_output];
                node_input[i] = new double[size_output];
                node_output[i] = new double[size_output];
                node_cell[i] = new double[size_output];
                vcx[i] = new double[size_total];
            }
        }

        protected override void ResetParameters()
        {
            b_gate_output = new double[size_output];
            b_gate_forget = new double[size_output];
            b_gate_input = new double[size_output];
            b_node_input = new double[size_output];

            w_gate_output = new double[size_output][];
            w_gate_forget = new double[size_output][];
            w_gate_input = new double[size_output][];
            w_node_input = new double[size_output][];

            for (var j = 0; j < size_output; j++)
            {
                w_gate_output[j] = new double[size_total];
                w_gate_forget[j] = new double[size_total];
                w_gate_input[j] = new double[size_total];
                w_node_input[j] = new double[size_total];

                for (var i = 0; i < size_total; i++)
                {
                    w_gate_output[j][i] = RandomWeight();
                    w_gate_forget[j][i] = RandomWeight();
                    w_gate_input[j][i] = RandomWeight();
                    w_node_input[j][i] = RandomWeight();
                }
            }
        }

        protected override void ResetGradients()
        {
            db_gate_output = new double[size_output];
            db_gate_forget = new double[size_output];
            db_gate_input = new double[size_output];
            db_node_input = new double[size_output];

            dw_gate_output = new double[size_output][];
            dw_gate_forget = new double[size_output][];
            dw_gate_input = new double[size_output][];
            dw_node_input = new double[size_output][];

            for (var i = 0; i < size_output; i++)
            {
                dw_gate_output[i] = new double[size_total];
                dw_gate_forget[i] = new double[size_total];
                dw_gate_input[i] = new double[size_total];
                dw_node_input[i] = new double[size_total];
            }
        }

        protected override void ResetCaches()
        {
            cb_gate_output = new double[size_output];
            cb_gate_forget = new double[size_output];
            cb_gate_input = new double[size_output];
            cb_node_input = new double[size_output];

            cw_gate_output = new double[size_output][];
            cw_gate_forget = new double[size_output][];
            cw_gate_input = new double[size_output][];
            cw_node_input = new double[size_output][];

            for (var i = 0; i < size_output; i++)
            {
                cw_gate_output[i] = new double[size_total];
                cw_gate_forget[i] = new double[size_total];
                cw_gate_input[i] = new double[size_total];
                cw_node_input[i] = new double[size_total];
            }
        }

        protected override void Update()
        {
            Parallel.For(0, size_output, options, j =>
            {
                cb_gate_output[j] = rmsDecay * cb_gate_output[j] + (1 - rmsDecay) * Math.Pow(db_gate_output[j], 2);
                cb_gate_forget[j] = rmsDecay * cb_gate_forget[j] + (1 - rmsDecay) * Math.Pow(db_gate_forget[j], 2);
                cb_gate_input[j] = rmsDecay * cb_gate_input[j] + (1 - rmsDecay) * Math.Pow(db_gate_input[j], 2);
                cb_node_input[j] = rmsDecay * cb_node_input[j] + (1 - rmsDecay) * Math.Pow(db_node_input[j], 2);

                b_gate_output[j] -= Clip(db_gate_output[j]) * LearningRate / Math.Sqrt(cb_gate_output[j] + 1e-6);
                b_gate_forget[j] -= Clip(db_gate_forget[j]) * LearningRate / Math.Sqrt(cb_gate_forget[j] + 1e-6);
                b_gate_input[j] -= Clip(db_gate_input[j]) * LearningRate / Math.Sqrt(cb_gate_input[j] + 1e-6);
                b_node_input[j] -= Clip(db_node_input[j]) * LearningRate / Math.Sqrt(cb_node_input[j] + 1e-6);

                for (var i = 0; i < size_total; i++)
                {
                    cw_gate_output[j][i] = rmsDecay * cw_gate_output[j][i] + (1 - rmsDecay) * Math.Pow(dw_gate_output[j][i], 2);
                    cw_gate_forget[j][i] = rmsDecay * cw_gate_forget[j][i] + (1 - rmsDecay) * Math.Pow(dw_gate_forget[j][i], 2);
                    cw_gate_input[j][i] = rmsDecay * cw_gate_input[j][i] + (1 - rmsDecay) * Math.Pow(dw_gate_input[j][i], 2);
                    cw_node_input[j][i] = rmsDecay * cw_node_input[j][i] + (1 - rmsDecay) * Math.Pow(dw_node_input[j][i], 2);

                    w_gate_output[j][i] -= Clip(dw_gate_output[j][i]) * LearningRate / Math.Sqrt(cw_gate_output[j][i] + 1e-6);
                    w_gate_forget[j][i] -= Clip(dw_gate_forget[j][i]) * LearningRate / Math.Sqrt(cw_gate_forget[j][i] + 1e-6);
                    w_gate_input[j][i] -= Clip(dw_gate_input[j][i]) * LearningRate / Math.Sqrt(cw_gate_input[j][i] + 1e-6);
                    w_node_input[j][i] -= Clip(dw_node_input[j][i]) * LearningRate / Math.Sqrt(cw_node_input[j][i] + 1e-6);
                }
            });
        }
    }
}
