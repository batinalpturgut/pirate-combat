using System;
using System.Collections.Generic;
using System.Diagnostics;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;

namespace Root.Scripts.Utilities
{
    public static class Timer
    {
        private static readonly Stack<Stopwatch> StartTimerPool;
        private static readonly Stack<Stopwatch> StopTimerPool;
        private const int InitialSize = 3;

        static Timer()
        {
            StartTimerPool = new Stack<Stopwatch>(InitialSize);
            StopTimerPool = new Stack<Stopwatch>(InitialSize);
            for (int i = 0; i < InitialSize; i++)
            {
                StartTimerPool.Push(new Stopwatch());
            }

            StopwatchPool = new Stack<Stopwatch>(InitialSize);
            for (int i = 0; i < InitialSize; i++)
            {
                StopwatchPool.Push(new Stopwatch());
            }
        }

        public static void StartTimer()
        {
            if (StartTimerPool.Peek() == null)
            {
                Stopwatch newStopwatch = new Stopwatch();
                newStopwatch.Start();
                StopTimerPool.Push(newStopwatch);
            }
            else
            {
                Stopwatch start = StartTimerPool.Pop();
                start.Start();
                StopTimerPool.Push(start);
            }
        }

        public static void StopTimer()
        {
            Stopwatch stop = StopTimerPool.Pop();

            if (stop == null)
            {
                Log.Console($"There is no timer started.");
                return;
            }

            stop.Stop();
            TimeSpan timeSpan = stop.Elapsed;
            Log.Console($@"Elapsed time: {timeSpan:hh\:mm\:ss\.fff}");
            stop.Reset();
            StartTimerPool.Push(stop);
        }

        private static readonly Stack<Stopwatch> StopwatchPool;

        public static Stopwatch Start()
        {
            if (StopwatchPool.Peek() == null)
            {
                Stopwatch newStopwatch = new Stopwatch();
                newStopwatch.Start();
                return newStopwatch;
            }

            Stopwatch stopwatch = StopwatchPool.Pop();
            stopwatch.Start();
            return stopwatch;
        }

        public static void Stop(Stopwatch stopwatch)
        {
            if (!stopwatch.IsRunning)
            {
                Log.Console($"This timer has not been started", LogType.Error);
                return;
            }

            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            Log.Console($@"Elapsed time: {timeSpan:hh\:mm\:ss\.fff}");
            stopwatch.Reset();
            StopwatchPool.Push(stopwatch);
        }
    }
}