using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    public class SoundPlayerAsync : IDisposable
    {
        public SoundPlayerAsync()
        {
            _mediaSoundPlayer = new System.Media.SoundPlayer();
            _queue = new Queue<WaveStream>();
            _sentString = new StringBuilder();

            _stopThread = false;
            _thread = new System.Threading.Thread(ThreadMain);
            _thread.Name = "Async Player";
            _thread.Start();
        }

        private void ThreadMain()
        {
            while (!_stopThread)
            {
                WaveStream waveToPlay = Dequeue();
                if (waveToPlay != null)
                {
                    _mediaSoundPlayer.Stream = waveToPlay.Stream;
                    _mediaSoundPlayer.Load();
                    _mediaSoundPlayer.PlaySync();
                    _sentString.Append(waveToPlay.Text);
                    _sentString.Append(' ');
                    // All done
                    if (Count == 0)
                    {
                        OnPlayingFinished();
                    }
                }
            }
            lock(this)
            {
                _stopped = true;
                System.Threading.Monitor.Pulse(this);
            }
        }

        private WaveStream Dequeue()
        {
            WaveStream wave = null;
            lock (this)
            {
                while (_queue.Count == 0 && !_stopThread)
                {
                    System.Threading.Monitor.Wait(this);
                }
                if (_queue.Count > 0)
                {
                    wave = _queue.Dequeue();
                    if (_queue.Count == 0)
                    {
                        OnQueueEmpty();
                    }
                }
            }
            return wave;
        }

        public void Start(WaveStream wave)
        {
            Enqueue(wave);
            _sentString.Clear();
        }

        public String Sent
        {
            get
            {
                return _sentString.ToString().TrimEnd();
            }
        }

        /// <summary>
        /// Give the SoundPlayerAsync a wave to play immediately (if noty busy) or
        /// following the currently playing/enqueued waves
        /// </summary>
        /// <param name="wave"></param>
        public void Enqueue(WaveStream wave)
        {
            lock(this)
            {
                _queue.Enqueue(wave);
                System.Threading.Monitor.Pulse(this);
            }
        }

        /// <summary>
        /// Clear all waves from the queue when aborting
        /// </summary>
        public void Clear()
        {
            lock(this)
            {
                _queue.Clear();
                System.Threading.Monitor.Pulse(this);
            }
        }

        /// <summary>
        /// Gets the number of enqueued waves
        /// </summary>
        public int Count
        {
            get
            {
                lock(this)
                {
                    return _queue.Count;
                }
            }
        }

        /// <summary>
        /// Playing has finished and no more waves are enqueued
        /// </summary>
        public event EventHandler PlayingFinished;
        protected void OnPlayingFinished()
        {
            EventHandler handler = PlayingFinished;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The queue has been emptied. Action in this should be quick
        /// </summary>
        public event EventHandler QueueEmpty;
        protected void OnQueueEmpty()
        {
            EventHandler handler = QueueEmpty;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Close the thread.
        /// </summary>
        public void CloseAndJoin()
        {
            lock(this)
            {
                if (_thread != null && !_stopThread)
                {
                    _stopThread = true;
                    _stopped = false;
                    _queue.Clear();
                    System.Threading.Monitor.Pulse(this);
                    if (!_stopped)
                    {
                        System.Threading.Monitor.Wait(this);
                    }
                    _thread.Join();
                    _thread = null;
                }
            }

        }

        private System.Threading.Thread _thread;
        private bool _stopThread;
        private bool _stopped;
        private Queue<WaveStream> _queue;
        private System.Media.SoundPlayer _mediaSoundPlayer;
        private StringBuilder _sentString;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    CloseAndJoin();

                    if (_queue != null)
                    {
                        _queue.Clear();
                        _queue = null;
                    }

                    if (_mediaSoundPlayer != null)
                    {
                        _mediaSoundPlayer.Stop();
                        _mediaSoundPlayer.Dispose();
                        _mediaSoundPlayer = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SoundPlayerAsync() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
