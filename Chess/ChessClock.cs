using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class ChessClock
    {
            public int TimeLeft { get; private set; }   // w sekundach
            public bool IsRunning { get; private set; }

            private readonly int increment;             // np. +2 sekundy po ruchu

            public event Action<int>? TimeChanged;      // powiadamia GUI
            public event Action? TimeExpired;           // czas się skończył

            public ChessClock(int initialSeconds, int incrementSeconds = 0)
            {
                TimeLeft = initialSeconds;
                increment = incrementSeconds;
            }

            public void Start()
            {
                IsRunning = true;
            }

            public void Stop()
            {
                IsRunning = false;
            }

            public void Tick()
            {
                if (!IsRunning)
                    return;

                TimeLeft--;

                TimeChanged?.Invoke(TimeLeft);

                if (TimeLeft <= 0)
                {
                    TimeExpired?.Invoke();
                    Stop();
                }
            }

            public void ApplyIncrement()
            {
                TimeLeft += increment;
                TimeChanged?.Invoke(TimeLeft);
            }
        }

}
