using System;
using System.Diagnostics;

namespace Serilog.ProcessLogging
{
    public class ProcessInformation
    {
        public int GC0 { get; }
        public int GC1 { get; }
        public int GC2 { get; }
        public int TotalMemory { get; }
        public int NonpagedSystemMemorySize { get; }
        public int PagedMemorySize { get; }
        public int PagedSystemMemorySize { get; }
        public int PeakPagedMemorySize { get; }
        public int PeakVirtualMemorySize { get; }
        public int PeakWorkingSet { get; }
        public int PrivateMemorySize { get; }
        public DateTime StartTime { get; }
        public int ThreadCount { get; }
        public TimeSpan TotalProcessorTime { get; }
        public TimeSpan UserProcessorTime { get; }
        public int VirtualMemorySize { get; }
        public int WorkingSet { get; }

        private static int ToMb(long value) => (int)(value / 1048576L);

        public ProcessInformation(Process process)
        {
            GC0 = GC.CollectionCount(0);
            GC1 = GC.CollectionCount(1);
            GC2 = GC.CollectionCount(2);
            TotalMemory = ToMb(GC.GetTotalMemory(false));
            NonpagedSystemMemorySize = ToMb(process.NonpagedSystemMemorySize64);
            PagedMemorySize = ToMb(process.PagedMemorySize64);
            PagedSystemMemorySize = ToMb(process.PagedSystemMemorySize64);
            PeakPagedMemorySize = ToMb(process.PeakPagedMemorySize64);
            PeakVirtualMemorySize = ToMb(process.PeakVirtualMemorySize64);
            PeakWorkingSet = ToMb(process.PeakWorkingSet64);
            PrivateMemorySize = ToMb(process.PrivateMemorySize64);
            StartTime = process.StartTime;
            ThreadCount = process.Threads.Count;
            TotalProcessorTime = process.TotalProcessorTime;
            UserProcessorTime = process.UserProcessorTime;
            VirtualMemorySize = ToMb(process.VirtualMemorySize64);
            WorkingSet = ToMb(process.WorkingSet64);
        }

        public ProcessInformation(ProcessInformation previous, ProcessInformation current)
        {
            GC0 = current.GC0 - previous.GC0;
            GC1 = current.GC1 - previous.GC1;
            GC2 = current.GC2 - previous.GC2;
            TotalMemory = current.TotalMemory - previous.TotalMemory;
            NonpagedSystemMemorySize = current.NonpagedSystemMemorySize - previous.NonpagedSystemMemorySize;
            PagedMemorySize = current.PagedMemorySize - previous.PagedMemorySize;
            PagedSystemMemorySize = current.PagedSystemMemorySize - previous.PagedSystemMemorySize;
            PeakPagedMemorySize = current.PeakPagedMemorySize - previous.PeakPagedMemorySize;
            PeakVirtualMemorySize = current.PeakVirtualMemorySize - previous.PeakVirtualMemorySize;
            PeakWorkingSet = current.PeakWorkingSet - previous.PeakWorkingSet;
            PrivateMemorySize = current.PrivateMemorySize - previous.PrivateMemorySize;
            StartTime = current.StartTime;
            ThreadCount = current.ThreadCount - previous.ThreadCount;
            TotalProcessorTime = current.TotalProcessorTime - previous.TotalProcessorTime;
            UserProcessorTime = current.UserProcessorTime - previous.UserProcessorTime;
            VirtualMemorySize = current.VirtualMemorySize - previous.VirtualMemorySize;
            WorkingSet = current.WorkingSet - previous.WorkingSet;
        }
    }
}
