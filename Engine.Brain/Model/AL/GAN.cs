using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.AL
{
    /// <summary>
    /// adversarial nets
    /// 
    /// GAN implemented based on CNTK
    /// paper:
    /// http://papers.nips.cc/paper/5423-generative-adversarial-nets.pdf
    /// 
    /// 
    /// </summary>
    public class GAN
    {

        private readonly DeviceDescriptor _device;
        /// <summary>
        /// 1.generator network
        /// 2.discriminator network
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="c"></param>
        /// <param name="generatorNet"></param>
        /// <param name="discriminatorNet"></param>
        public GAN(int w, int h, int c, string deviceName)
        {
            _device = NP.CNTK.GetDeviceByName(deviceName);
        }

        /// <summary>
        /// super parameters accroding to DCGAN
        /// </summary>
        private void InitializationTwoPalyerNetwork()
        {

        }

        //private Function CreateDiscriminator(int latent_dim)
        //{
        //    var input_variable = CNTK.Variable.InputVariable(new int[] { latent_dim }, CNTK.DataType.Float, name: "generator_input");
        //    var x = NP.CNTK.Dense(input_variable, 128 * 16 * 16, _device);
        //    //x = CNTK.CNTKLib.LeakyReLU(x);
        //    x = CNTK.CNTKLib.Reshape(x, new int[] { 16, 16, 128 });
        //    x = NP.CNTK.Convolution2D(x, 256, new int[] { 5, 5 }, _device, use_padding: true, activation: CNTK.CNTKLib.LeakyReLU);
        //    x = Util.ConvolutionTranspose(x, computeDevice,
        //      filter_shape: new int[] { 4, 4 },
        //      num_filters: 256,
        //      strides: new int[] { 2, 2 },
        //      output_shape: new int[] { 32, 32 },
        //      use_padding: true,
        //      activation: CNTK.CNTKLib.LeakyReLU);
        //    x = Util.Convolution2D(x, 256, new int[] { 5, 5 }, computeDevice, use_padding: true, activation: CNTK.CNTKLib.LeakyReLU);
        //    x = Util.Convolution2D(x, 256, new int[] { 5, 5 }, computeDevice, use_padding: true, activation: CNTK.CNTKLib.LeakyReLU);
        //    x = Util.Convolution2D(x, channels, new int[] { 7, 7 }, computeDevice, use_padding: true, activation: CNTK.CNTKLib.Tanh);
        //    return x; ;
        //}

        //private Function CreateGenerator()
        //{

        //}

    }
}
