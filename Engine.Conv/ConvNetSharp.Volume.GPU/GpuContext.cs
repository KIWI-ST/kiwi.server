using System;
using System.Diagnostics;
using ManagedCuda;
using ManagedCuda.CudaBlas;
using ManagedCuda.CudaDNN;

namespace ConvNetSharp.Volume.GPU
{
    public class GpuContext
    {
        
        public static int DeviceCount
        {
            get { return CudaContext.GetDeviceCount(); }
        }

        private static readonly Lazy<GpuContext> DefaultContextLazy = new Lazy<GpuContext>(() => new GpuContext(0));
        private CudaContext _cudaContext;
        private CudaStream _defaultStream;

        public GpuContext(int deviceId = 0)
        {
            CudaContext = new CudaContext(deviceId, true);
            CudaBlasHandle = new CudaBlas();
            var props = this.CudaContext.GetDeviceInfo();
            DefaultBlockCount = props.MultiProcessorCount * 32;
            DefaultThreadsPerBlock = props.MaxThreadsPerBlock;
            WarpSize = props.WarpSize;
            DefaultStream = new CudaStream();
            CudnnContext = new CudaDNNContext();
        }

        public CudaBlas CudaBlasHandle { get; }

        public CudaContext CudaContext
        {
            get { return this._cudaContext; }
            private set { this._cudaContext = value; }
        }

        public CudaDNNContext CudnnContext { get; }

        public static GpuContext Default => DefaultContextLazy.Value;

        public int DefaultBlockCount { get; }

        public int DefaultThreadsPerBlock { get; }

        public int WarpSize { get; }

        public CudaStream DefaultStream
        {
            get { return this._defaultStream; }
            set { this._defaultStream = value; }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            if (disposing)
            {
                Dispose(ref this._defaultStream);
                Dispose(ref this._cudaContext);
            }
        }

        public void Dispose<T>(ref T field) where T : class, IDisposable
        {
            if (field != null)
            {
                try
                {
                    field.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                field = null;
            }
        }

        ~GpuContext()
        {
            Dispose(false);
        }
    }
}