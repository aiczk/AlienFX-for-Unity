using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace AlienFX.Util
{
    public static class NativeMethods
    {
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);
    }

    // ReSharper disable once InconsistentNaming
    internal class UnManagedDll : IDisposable
    {
        private bool disposed;
        private IntPtr moduleHandle;
        
        public UnManagedDll(string dllPath)
        {
            disposed = false;
            moduleHandle = IntPtr.Zero;
            
            Load(dllPath);
        }
        
        ~UnManagedDll() => Dispose();

        private void Load(string dllPath)
        {
            if (!File.Exists(dllPath))
                throw new FileNotFoundException($"The {dllPath} does not exist.");

            moduleHandle = NativeMethods.LoadLibrary(dllPath);
            var isAvailable = moduleHandle != IntPtr.Zero && moduleHandle != null;
            
            if (!isAvailable)
                throw new FileNotFoundException($"The {dllPath} does not exist.");
        }
        
        public T GetProcAddress<T>() where T : class => GetProcAddress<T>("");

        public T GetProcAddress<T>(string alias) where T : class
        {
            if (Cache<T>.Value != null) 
                return Cache<T>.Value;

            var funcName = string.IsNullOrEmpty(alias) ? typeof(T).Name : alias;
            var procAddress = NativeMethods.GetProcAddress(moduleHandle, funcName);

            if (procAddress == IntPtr.Zero)
                throw new NotImplementedException($"{funcName} not found in AlienFxSDK.");
            
            return Cache<T>.Value = Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T)) as T;
        }
        
        public void Dispose()
        {
            if (disposed) 
                return;

            disposed = true;

            NativeMethods.FreeLibrary(moduleHandle);
            moduleHandle = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        private static class Cache<T>
        {
            public static T Value { get; set; }
        }
    }
}