using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;
using Engine.Brain.Utils;

namespace Engine.Brain.Model.DL
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
    public class DCGANet
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
        public DCGANet(string deviceName, int w = 32, int h = 32, int c = 3, int latent_dim =32)
        {
            _device = NP.CNTKHelper.GetDeviceByName(deviceName);
            var label_var = CNTK.Variable.InputVariable(shape: new CNTK.NDShape(0), dataType: CNTK.DataType.Float, name: "label_var");
            var generator = CreateGenerator(latent_dim, c);
            var discriminator = CreateDiscriminator(w,h,c);
            var gan = discriminator.Clone(CNTK.ParameterCloningMethod.Share, replacements: new Dictionary<CNTK.Variable, CNTK.Variable>() { { discriminator.Arguments[0], generator } });

            var discriminator_loss = CNTK.CNTKLib.BinaryCrossEntropy(discriminator, label_var);
            var discriminator_learner = CNTK.CNTKLib.AdaDeltaLearner(
              parameters: new CNTK.ParameterVector((System.Collections.ICollection)discriminator.Parameters()),
              learningRateSchedule: new CNTK.TrainingParameterScheduleDouble(1));
            var discriminator_trainer = CNTK.CNTKLib.CreateTrainer(discriminator, discriminator_loss, discriminator_loss, new CNTK.LearnerVector() { discriminator_learner });

            var gan_loss = CNTK.CNTKLib.BinaryCrossEntropy(gan, label_var);
            var gan_learner = CNTK.CNTKLib.AdaDeltaLearner(
              parameters: new CNTK.ParameterVector((System.Collections.ICollection)generator.Parameters()),
              learningRateSchedule: new CNTK.TrainingParameterScheduleDouble(1));
            var gan_trainer = CNTK.CNTKLib.CreateTrainer(gan, gan_loss, gan_loss, new CNTK.LearnerVector() { gan_learner });
        }

        /// <summary>
        /// super parameters accroding to DCGAN
        /// </summary>
        private void InitializationTwoPalyerNetwork()
        {
            //Function discriminatorNet = CreateDiscriminator
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latent_dim"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        private Function CreateGenerator(int latent_dim, int channels)
        {
            var input_variable = Variable.InputVariable(new int[] { latent_dim }, CNTK.DataType.Float, name: "generator_input");
            var x = NP.CNTKHelper.Dense(input_variable, 128 * 16 * 16, _device, null);
            //set leakrelu alpha equals 0.3
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = CNTK.CNTKLib.Reshape(x, new int[] { 16, 16, 128 });
            x = NP.CNTKHelper.Convolution2D(x, 256, new int[] { 5, 5 }, _device, null ,true);
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = NP.CNTKHelper.ConvolutionTranspose(x,new int[] { 4, 4 },256,_device, null, true, new int[] { 2, 2 }, true, new int[] { 32, 32 });
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = NP.CNTKHelper.Convolution2D(x, 256, new int[] { 5, 5 }, _device, use_padding:true);
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = NP.CNTKHelper.Convolution2D(x, 256, new int[] { 5, 5 }, _device, use_padding: true);
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = NP.CNTKHelper.Convolution2D(x, channels, new int[] { 7, 7 }, _device, CNTKLib.Tanh, use_padding: true);
            return x; ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Function CreateDiscriminator(int w, int h, int c)
        {
            var input_variable = Variable.InputVariable(new int[] { w, h, c }, DataType.Double, name: "discriminator_input");
            var x = NP.CNTKHelper.Convolution2D(input_variable, 128, new int[] { 3, 3 },_device);
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = NP.CNTKHelper.Convolution2D(x, 128, new int[] { 4, 4 }, _device, strides: new int[] { 2, 2 });
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = NP.CNTKHelper.Convolution2D(x, 128, new int[] { 4, 4 }, _device, strides: new int[] { 2, 2 });
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = NP.CNTKHelper.Convolution2D(x, 128, new int[] { 4, 4 }, _device, strides: new int[] { 2, 2 });
            x = CNTKLib.LeakyReLU(x, 0.3);
            x = CNTKLib.Dropout(x, 0.4);
            x = NP.CNTKHelper.Dense(x, 1, _device, CNTKLib.Sigmoid);
            return x;
        }

    }
}
