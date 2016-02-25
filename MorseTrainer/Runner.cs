using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    /// <summary>
    /// The Runner class handles the timing of the morse code runnning.
    /// It tracks 5 basic states:
    ///     Idle: no tests being run
    ///     StartDelay: countdown to starting the morse code
    ///     Sending: Sending morse code
    ///     SendFinished: Send time is up, waiting for last word to finish up
    ///     StopDelay: Still allowing user to enter characters
    /// </summary>
    public class Runner : IDisposable
    {
        private enum State
        {
            // waiting
            Idle,

            // waiting
            StartRequest,

            // user has started and delay is counting
            StartDelay,

            // sending morse
            Sending,

            // send is done, but finishing up the rest of the word
            SendFinished,

            // send is 
            StopDelay,

            // stop requested
            StopRequest
        }

        public const int MIN_DURATION = 30;
        public const int MAX_DURATION = 60*10;
        public const int MIN_START_DELAY = 0;
        public const int MAX_START_DELAY = 5;
        public const int MIN_STOP_DELAY = 0;
        public const int MAX_STOP_DELAY = 5;

        public Runner()
        {
            _timer = new System.Timers.Timer();
            _timer.Enabled = true;
            _timer.AutoReset = false;
            _timer.Elapsed += _timer_Elapsed;
        }

        /// <summary>
        /// Gets whether the runner is idle (false) or in one of the running states (true)
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return _state != State.Idle;
            }
        }

        /// <summary>
        /// Gets is the runner is in a mode where the user input should be recorded.
        /// These states are Sending, SendFinished, and StopDelay
        /// </summary>
        public bool IsListenMode
        {
            get
            {
                return _state == State.Sending ||
                    _state == State.SendFinished || 
                    _state == State.StopDelay;
            }
        }

        /// <summary>
        /// Gets a bool that indicate if the morse is being sent and a new word
        /// should be sent to the tone generator.
        /// </summary>
        public bool ContinueMorse
        {
            get
            {
                return _state == State.Sending;
            }
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            State nextState = StateExit();
            StateEnter(nextState);
        }

        private State StateExit()
        {
            State nextState = _state;
            switch (_state)
            {
                case State.StartDelay:
                    _timer.Stop();
                    OnStartDelayExit();
                    nextState = State.Sending;
                    break;
                case State.Sending:
                    _timer.Stop();
                    OnMorseExit();
                    nextState = State.SendFinished;
                    break;
                case State.StopDelay:
                    _timer.Stop();
                    OnStopDelayExit();
                    nextState = State.Idle;
                    break;
            }
            return nextState;
        }


        private void StateEnter(State nextState)
        {
            _state = nextState;
            switch (_state)
            {
                case State.StartDelay:
                    _timer.Interval = _startDelay * 1000;
                    _timer.Start();
                    OnStartDelayEnter();
                    break;
                case State.Sending:
                    _timer.Interval = _sendDuration * 1000;
                    _timer.Start();
                    OnMorseEnter();
                    break;
                case State.StopDelay:
                    _timer.Interval = _stopDelay * 1000;
                    _timer.Start();
                    OnStopDelayEnter();
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the start delay in seconds. This is the time for the user
        /// to recover from pressing the start button to positioning their hands
        /// for input, for example.
        /// </summary>
        public int StartDelay
        {
            get
            {
                return _startDelay;
            }
            set
            {
                _startDelay = value;
            }
        }

        /// <summary>
        /// Gets or sets the stop delay in seconds. This is the time after the last word
        /// was sent to let the user input the remaining letters.
        /// </summary>
        public int StopDelay
        {
            get
            {
                return _stopDelay;
            }
            set
            {
                _stopDelay = value;
            }
        }

        /// <summary>
        /// Gets or sets the send duration in seconds.
        /// </summary>
        public int SendDuration
        {
            get
            {
                return _sendDuration;
            }
            set
            {
                _sendDuration = value;
            }
        }

        /// <summary>
        /// Starts the runner.
        /// </summary>
        public void RequestStart()
        {
            State startState = (_startDelay > 0) ? State.StartDelay : State.Sending;
            StateEnter(startState);
        }

        /// <summary>
        /// Exits the current state and then fires the Abort event. The abort event
        /// is only fired if the runner is not running
        /// </summary>
        public void RequestStop()
        {
            bool abort = _state != State.Idle;
            _state = StateExit();

            // stop request during start delay
            if (_state == State.Sending)
            {
                _state = State.Idle;
            }
            if (abort)
            {
                OnAbort();
            }
        }

        /// <summary>
        /// Used when the SendMorseEnd is sent while sending is still occurring.
        /// Call this in the ToneGenerator.CharactersSent event
        /// </summary>
        public void AcknowledgeSendEnd()
        {
            if (_state == State.SendFinished)
            {
                StateEnter(State.StopDelay);
            }
        }

        /// <summary>
        /// Countdown to morse code sending has started
        /// </summary>
        public event EventHandler StartDelayEnter;
        protected void OnStartDelayEnter()
        {
            EventHandler handler = StartDelayEnter;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// The countdown delay has ended
        /// </summary>
        public event EventHandler StartDelayExit;
        protected void OnStartDelayExit()
        {
            EventHandler handler = StartDelayExit;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Start morse code sending.
        /// </summary>
        public event EventHandler MorseEnter;
        protected void OnMorseEnter()
        {
            EventHandler handler = MorseEnter;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Stop feeding tone generator with characters and let it finish up
        /// </summary>
        public event EventHandler MorseExit;
        protected void OnMorseExit()
        {
            EventHandler handler = MorseExit;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Morse code has stopped, but the user is still allowed to type in keys
        /// </summary>
        public event EventHandler StopDelayEnter;
        protected void OnStopDelayEnter()
        {
            EventHandler handler = StopDelayEnter;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Post sending delay is over, time to analyze
        /// </summary>
        public event EventHandler StopDelayExit;
        protected void OnStopDelayExit()
        {
            EventHandler handler = StopDelayExit;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Abort
        /// </summary>
        public event EventHandler Abort;
        protected void OnAbort()
        {
            EventHandler handler = Abort;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private State _state;
        private System.Timers.Timer _timer;
        private int _startDelay;
        private int _sendDuration;
        private int _stopDelay;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_timer != null)
                    {
                        _timer.Stop();
                        _timer.Dispose();
                        _timer = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Runner() {
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
