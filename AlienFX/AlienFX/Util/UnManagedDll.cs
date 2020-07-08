using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace AlienFX.Util
{
    public static class NativeMethods
    {
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpLibFileName);
        
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)]string lpProcName);
        
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);
    }

    // ReSharper disable once InconsistentNaming
    internal class UnManagedDll : IDisposable
    {
        private string DllPath;
        private bool isAvailable;
        private bool disposed;
        
        private IntPtr moduleHandle;
        private Dictionary<string, Delegate> functions;
        
        public UnManagedDll(string dllPath)
        {
            isAvailable = false;
            disposed = false;
            functions = new Dictionary<string, Delegate>();
            moduleHandle = IntPtr.Zero;
            
            Load(dllPath);
        }
        
        ~UnManagedDll() => Dispose(false);

        private void Load(string dllPath)
        {
            DllPath = dllPath;

            if (!File.Exists(DllPath))
            {
                isAvailable = false;
                throw new FileNotFoundException($"The {DllPath} does not exist.");
            }

            moduleHandle = NativeMethods.LoadLibrary(DllPath);
            isAvailable = moduleHandle != IntPtr.Zero && moduleHandle != null;
        }
        
        public T GetProcAddress<T>() where T : class => GetProcAddress<T>("");

        public T GetProcAddress<T>(string alias) where T : class
        {
            if (!isAvailable)
                throw new FileNotFoundException($"The {DllPath} does not exist.");

            var funcName = string.IsNullOrEmpty(alias) ? typeof(T).Name : alias;
            
            if (functions.ContainsKey(funcName)) 
                return functions[funcName] as T;
            
            var procAddress = NativeMethods.GetProcAddress(moduleHandle, funcName);

            if (procAddress == IntPtr.Zero)
                throw new NotImplementedException($"{funcName} not found in {DllPath}.");
            
            functions.Add(funcName, Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T)));
            //functions[funcName] = Marshal.GetDelegateForFunctionPointer(procAddress, typeof(T));
            
            return functions[funcName] as T;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed) 
                return;

            disposed = true;

            if (disposing) 
                functions = null;

            NativeMethods.FreeLibrary(moduleHandle);
            moduleHandle = IntPtr.Zero;
        }
    }
}