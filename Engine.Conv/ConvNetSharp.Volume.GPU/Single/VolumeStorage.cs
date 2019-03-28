﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ManagedCuda;
using ManagedCuda.BasicTypes;

namespace ConvNetSharp.Volume.GPU.Single
{
    public unsafe class VolumeStorage : VolumeStorage<float>, IDisposable, IVolumeStorage<float>
    {
        private readonly CudaHostMemoryRegion _hostPointer;
        private readonly bool _isOwner;
        private bool _allocatedOnDevice;
        private bool _disposed;

        public VolumeStorage(Shape shape, GpuContext context, long length = -1) : base(shape)
        {
            this.Context = context;

            // Take care of unkown dimension
            if (length != -1)
            {
                this.Shape.GuessUnkownDimension(length);
            }

            // Host 
            this._hostPointer = InitializeSharedMemory(this.Shape.TotalLength);

            this._isOwner = true;
        }

        public VolumeStorage(float[] array, Shape shape, GpuContext context) : this(shape, context, array.Length)
        {
            this.Context = context;

            if (this.Shape.TotalLength != array.Length)
            {
                throw new ArgumentException("Wrong dimensions");
            }

            // Fill host buffer
            for (var i = 0; i < array.Length; i++)
            {
                this.HostBuffer[i] = array[i];
            }

            this.Location = DataLocation.Host;
        }

        public VolumeStorage(VolumeStorage storage, Shape newShape) : base(newShape)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            if (storage._hostPointer == null)
            {
                throw new ArgumentException();
            }

            this._isOwner = false;
            this._hostPointer = storage._hostPointer;
            this.Shape = newShape;
            this.Context = storage.Context;
            this._allocatedOnDevice = storage._allocatedOnDevice;

            storage.CopyToDevice();

            this.Location = DataLocation.Device;
            this.DeviceBuffer = new CudaDeviceVariable<float>(storage.DeviceBuffer.DevicePointer);

            this.ConvolutionBackwardFilterStorage = storage.ConvolutionBackwardFilterStorage;
            this.ConvolutionBackwardStorage = storage.ConvolutionBackwardStorage;
            this.ConvolutionStorage = storage.ConvolutionStorage;
            this.ReductionStorage = storage.ReductionStorage;
            this.DropoutStorage = storage.DropoutStorage;
            this.DropoutStateStorage = storage.DropoutStateStorage;
        }

        public long GpuMemory => this.Shape.TotalLength * sizeof(double);

        public CudaDeviceVariable<byte> ConvolutionBackwardFilterStorage { get; set; }

        public CudaDeviceVariable<byte> ConvolutionBackwardStorage { get; set; }

        public CudaDeviceVariable<byte> ConvolutionStorage { get; set; }

        public CudaDeviceVariable<byte> ReductionStorage { get; set; }

        public DataLocation Location { get; set; }

        public float* HostBuffer => (float*)this._hostPointer.Start;

        public CudaDeviceVariable<float> DeviceBuffer { get; private set; }

        public GpuContext Context { get; }

        public CudaDeviceVariable<byte> DropoutStorage { get; set; }

        public CudaDeviceVariable<byte> DropoutStateStorage { get; set; }

        public void Dispose()
        {
            Dispose(true);
        }

        public override void Clear()
        {
            Debug.Assert(!this._disposed);

            switch (this.Location)
            {
                case DataLocation.Host:
                    {
                        for (var i = 0; i < this.Shape.TotalLength; i++)
                        {
                            this.HostBuffer[i] = 0.0f;
                        }
                    }
                    break;
                case DataLocation.Device:
                    {
                        var res = DriverAPINativeMethods.Memset.cuMemsetD16_v2(this.DeviceBuffer.DevicePointer, 0, this.DeviceBuffer.Size * 2);
                        if (res != CUResult.Success)
                        {
                            throw new CudaException(res);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void CopyFrom(VolumeStorage<float> source)
        {
            Debug.Assert(!this._disposed);

            var real = source as VolumeStorage;

            if (!ReferenceEquals(this, real))
            {
                if (this.Shape.TotalLength != real.Shape.TotalLength)
                {
                    throw new ArgumentException($"origin and destination volume should have the same number of weight ({this.Shape.TotalLength} != {real.Shape}).");
                }

                real.CopyToDevice();

                if (this.DeviceBuffer == null)
                {
                    this.DeviceBuffer = new CudaDeviceVariable<float>(this.Shape.TotalLength);
                }

                var res = DriverAPINativeMethods.SynchronousMemcpy_v2.cuMemcpy(
                    this.DeviceBuffer.DevicePointer,
                    real.DeviceBuffer.DevicePointer,
                    this.Shape.TotalLength * sizeof(float));

                if (res != CUResult.Success)
                {
                    throw new CudaException(res);
                }

                this.Location = DataLocation.Device;
            }
            else
            {
                CopyToDevice();
            }
        }

        public void CopyToDevice()
        {
            Debug.Assert(!this._disposed);

            if (this.Location == DataLocation.Host)
            {
                Debug.WriteLine("CopyToDevice");

                // Device 
                if (!this._allocatedOnDevice)
                {
                    this.DeviceBuffer = new CudaDeviceVariable<float>(this.Shape.TotalLength);
                    this._allocatedOnDevice = true;
                }

                var res = DriverAPINativeMethods.AsynchronousMemcpy_v2.cuMemcpyHtoDAsync_v2(
                    this.DeviceBuffer.DevicePointer, this._hostPointer.Start, this.DeviceBuffer.SizeInBytes,
                    this.Context.DefaultStream.Stream);

                if (res != CUResult.Success)
                {
                    throw new CudaException(res);
                }

                // Synchro
                this.Context.DefaultStream.Synchronize();

                this.Location = DataLocation.Device;
            }
        }

        public void CopyToHost()
        {
            Debug.Assert(!this._disposed);

            if (this.Location == DataLocation.Device)
            {
                var res = DriverAPINativeMethods.AsynchronousMemcpy_v2.cuMemcpyDtoHAsync_v2(
                    new IntPtr(this.HostBuffer),
                    this.DeviceBuffer.DevicePointer, this.DeviceBuffer.SizeInBytes, this.Context.DefaultStream.Stream);

                if (res != CUResult.Success)
                {
                    throw new CudaException(res);
                }

                // Synchro
                this.Context.DefaultStream.Synchronize();

                this.Location = DataLocation.Host;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            this._disposed = true;

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            if (this._hostPointer != null && this.HostBuffer != default(float*))
            {
                if (this._isOwner)
                {
                    this._hostPointer.Dispose();
                }
            }

            if (this._isOwner)
            {
                this.DeviceBuffer?.Dispose();
                this.ConvolutionBackwardFilterStorage?.Dispose();
                this.ConvolutionBackwardStorage?.Dispose();
                this.ConvolutionStorage?.Dispose();
                this.ReductionStorage?.Dispose();
                this.DropoutStorage?.Dispose();
                this.DropoutStateStorage?.Dispose();
            }
            else
            {
                this.DeviceBuffer = null;
            }
        }

        private static void FillWithZeroes(IntPtr memoryStart, long size)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    ZeroMemory(memoryStart, (UIntPtr)size);
                    break;
                default:
                    var buffer = (byte*)memoryStart;
                    for (var i = 0; i < size; i++)
                    {
                        buffer[i] = 0;
                    }
                    break;
            }
        }

        ~VolumeStorage()
        {
            Dispose(false);
        }

        public override float Get(int[] coordinates)
        {
            CopyToHost();

            var length = coordinates.Length;
            return Get(coordinates[0], length > 1 ? coordinates[1] : 0, length > 2 ? coordinates[2] : 0, length > 3 ? coordinates[3] : 0);
        }

        public override float Get(int w, int h, int c, int n)
        {
            CopyToHost();

            return this.HostBuffer[
                w + h * this.Shape.Dimensions[0] + c * this.Shape.Dimensions[0] * this.Shape.Dimensions[1] +
                n * this.Shape.Dimensions[0] * this.Shape.Dimensions[1] * this.Shape.Dimensions[2]];
        }

        public override float Get(int w, int h, int c)
        {
            CopyToHost();
            return
                this.HostBuffer[
                    w + h * this.Shape.Dimensions[0] + c * this.Shape.Dimensions[0] * this.Shape.Dimensions[1]];
        }

        public override float Get(int w, int h)
        {
            CopyToHost();
            return this.HostBuffer[w + h * this.Shape.Dimensions[0]];
        }

        public override float Get(int i)
        {
            CopyToHost();
            return this.HostBuffer[i];
        }

        private static CudaHostMemoryRegion InitializeSharedMemory(long elementCount)
        {
            var sharedMemory = new CudaHostMemoryRegion(elementCount * sizeof(float));

            // Zero out
            FillWithZeroes(sharedMemory.Start, sharedMemory.ByteCount);
            return sharedMemory;
        }

        public override void Set(int[] coordinates, float value)
        {
            CopyToHost();

            var length = coordinates.Length;
            Set(coordinates[0], length > 1 ? coordinates[1] : 0, length > 2 ? coordinates[2] : 0, length > 3 ? coordinates[3] : 0, value);
        }

        public override void Set(int w, int h, int c, int n, float value)
        {
            CopyToHost();
            this.HostBuffer[
                w + h * this.Shape.Dimensions[0] + c * this.Shape.Dimensions[0] * this.Shape.Dimensions[1] +
                n * this.Shape.Dimensions[0] * this.Shape.Dimensions[1] * this.Shape.Dimensions[2]] = value;
        }

        public override void Set(int w, int h, int c, float value)
        {
            CopyToHost();
            this.HostBuffer[
                    w + h * this.Shape.Dimensions[0] + c * this.Shape.Dimensions[0] * this.Shape.Dimensions[1]] =
                value;
        }

        public override void Set(int w, int h, float value)
        {
            CopyToHost();
            this.HostBuffer[w + h * this.Shape.Dimensions[0]] = value;
        }

        public override void Set(int i, float value)
        {
            CopyToHost();
            this.HostBuffer[i] = value;
        }

        public override float[] ToArray()
        {
            CopyToHost();

            var array = new float[this.Shape.TotalLength];
            Marshal.Copy(new IntPtr(this.HostBuffer), array, 0, (int)this.Shape.TotalLength);
            return array;
        }

        [DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
        private static extern void ZeroMemory(IntPtr dest, UIntPtr size);
    }
}