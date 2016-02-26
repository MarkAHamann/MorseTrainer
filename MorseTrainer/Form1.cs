/*
    Morse Trainer
    Copyright (C) 2016 Mark Hamann

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MorseTrainer
{
    public partial class Form1 : Form
    {
        [Flags]
        public enum ControlToUpdate
        {
            None = 0x00,
            Slider = 0x01,
            TextBox = 0x02,
            All = 0x03
        }

        private static readonly String CONFIG_FILE_NAME = "config.cfg";

        #region Form Maintenance
        public Form1()
        {
            InitializeComponent();

            // Setup objects
            _toneGenerator = new ToneGenerator();
            _charGenerator = new CharGenerator();
            _runner = new Runner();
            _player = new SoundPlayerAsync();
            _analyzer = new Analyzer(txtAnalysis);
            _builder = new WordToToneBuilder(_toneGenerator);
            _recorded = new StringBuilder();

            // Do initialization
            FrequencyInitialize(
                sliderFrequency, txtFrequency,
                400,
                ToneGenerator.MIN_FREQUENCY,
                ToneGenerator.MAX_FREQUENCY,
                50
                );

            WPMInitialize(
                sliderWPM, txtWPM,
                20.0f,
                ToneGenerator.MIN_WPM,
                ToneGenerator.MAX_WPM,
                0.5f
                );

            FarnsworthWPMInitialize(
                sliderFarnsworth, txtFarnsworth,
                20.0f,
                ToneGenerator.MIN_FARNSWORTH_WPM,
                ToneGenerator.MAX_FARNSWORTH_WPM,
                0.5f
                );

            DurationInitialize(
                sliderDuration, txtDuration,
                30,
                Runner.MIN_DURATION,
                Runner.MAX_DURATION,
                30
                );

            StartDelayInitialize(
                sliderStartDelay, txtStartDelay,
                0,
                Runner.MIN_START_DELAY,
                Runner.MAX_START_DELAY,
                1
                );

            StopDelayInitialize(
                sliderStopDelay, txtStopDelay,
                0,
                Runner.MIN_STOP_DELAY,
                Runner.MAX_STOP_DELAY,
                1
                );

            VolumeInitialize(
                sliderVolume, txtVolume,
                1.0f,
                ToneGenerator.MIN_VOLUME,
                ToneGenerator.MAX_VOLUME,
                0.1f
                );

            _runner.StartDelayEnter += _runner_StartDelayEnter;
            _runner.StartDelayExit += _runner_StartDelayExit;
            _runner.MorseEnter += _runner_MorseEnter;
            _runner.MorseExit += _runner_MorseExit;
            _runner.StopDelayEnter += _runner_StopDelayEnter;
            _runner.StopDelayExit += _runner_StopDelayExit;
            _runner.Abort += _runner_Abort;
            _player.Dequeued += _player_Dequeued;
            _player.PlayingFinished += _player_PlayingFinished;
            _toneGenerator.UpdateRequired += _toneGenerator_UpdateRequired;

            cmbKoch.Items.Clear();
            for (int i = 0; i < Koch.Length; ++i)
            {
                cmbKoch.Items.Add(MorseInfo.ExpandProsigns(Koch.Order[i].ToString()));
            }

            Config config = Config.Load(CONFIG_FILE_NAME);
            ApplyConfig(config);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Config.Save(ExtractConfig(), CONFIG_FILE_NAME);
            if (_runner.IsRunning)
            {
                _runner.RequestStop();
            }
            _player.CloseAndJoin();
        }

        #endregion

        #region Configuration
        private void ApplyConfig(Config config)
        {
            Frequency = config.Frequency;
            FrequencySlider = config.Frequency;
            FrequencyText = config.Frequency;

            WPM = config.WPM;
            WPMSlider = config.WPM;
            WPMText = config.WPM;

            FarnsworthWPM = config.FarnsworthWPM;
            FarnsworthWPMSlider = config.FarnsworthWPM;
            FarnsworthWPMText = config.FarnsworthWPM;

            Duration = config.Duration;
            DurationSlider = config.Duration;
            DurationText = config.Duration;

            StartDelay = config.StartDelay;
            StartDelaySlider = config.StartDelay;
            StartDelayText = config.StartDelay;

            StopDelay = config.StopDelay;
            StopDelaySlider = config.StopDelay;
            StopDelayText = config.StopDelay;

            Volume = config.Volume;
            VolumeSlider = config.Volume;
            VolumeText = config.Volume;

            if (config.GenerationMethod == CharGenerator.Method.Custom)
            {
                btnKoch.Checked = false;
                btnCustom.Checked = true;
            }
            else
            {
                btnCustom.Checked = false;
                btnKoch.Checked = true;
            }
            cmbKoch.SelectedIndex = config.KochIndex;
            chkFavorNew.Checked = config.FavorNew;
            txtCustom.Text = config.Custom;
        }

        private Config ExtractConfig()
        {
            Config config = new Config();
            config.Frequency = (UInt16)sliderFrequency.Value;
            config.WPM = (float)sliderWPM.Value / 2.0f;
            config.FarnsworthWPM = (float)sliderFarnsworth.Value / 2.0f;
            config.Duration = (UInt16)(sliderDuration.Value);

            config.StartDelay = (UInt16)sliderStartDelay.Value;
            config.StopDelay = (UInt16)sliderStopDelay.Value;
            config.Volume = (float)sliderVolume.Value / 10.0f;

            config.GenerationMethod = btnKoch.Checked ? CharGenerator.Method.Koch : CharGenerator.Method.Custom;
            config.KochIndex = (UInt16)cmbKoch.SelectedIndex;
            config.FavorNew = chkFavorNew.Checked;
            config.Custom = txtCustom.Text;

            return config;
        }
        #endregion

        #region Runner/Tone Generator/Start Button Events
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_runner.IsRunning)
            {
                _player.ClearQueue();
                btnStartStop.Enabled = false;
                _runner.RequestStop();
            }
            else
            {
                _recorded.Clear();
                String word = _charGenerator.CreateRandomString();
                _builder.StartBuildAsync(word, new AsyncCallback(FirstWaveReadyCallback));
                btnStartStop.Text = "Stop";
                txtAnalysis.Focus();
                txtAnalysis.Clear();
            }
        }

        private void FirstWaveReadyCallback(IAsyncResult result)
        {
            WaveStream waveStream = (WaveStream)result.AsyncState;
            _player.Enqueue(waveStream);
            _runner.RequestStart();
        }

        private void WaveReadyCallback(IAsyncResult result)
        {
            WaveStream waveStream = (WaveStream)result.AsyncState;
            if (waveStream != null)
            {
                _player.Enqueue(waveStream);
            }
        }

        private void _runner_StartDelayEnter(object sender, EventArgs e)
        {
        }

        private void _runner_StartDelayExit(object sender, EventArgs e)
        {
        }

        private void _runner_MorseEnter(object sender, EventArgs e)
        {
            _player.Start();
            String word = _charGenerator.CreateRandomString();
            _builder.StartBuildAsync(word, new AsyncCallback(WaveReadyCallback));
        }

        private void _player_Dequeued(object sender, EventArgs e)
        {
            if (_runner.ContinueMorse)
            {
                _builder.StartBuildAsync(_charGenerator.CreateRandomString(), new AsyncCallback(WaveReadyCallback));
            }
        }

        private void _toneGenerator_UpdateRequired(object sender, EventArgs e)
        {
            // This indicates that the tone has changed. If we are currently sending code,
            // we need to remove any queued tones and in progress tones and send new tones instead
            _toneGenerator.Update();
            _player.ClearQueue();
            _builder.StartBuildAsync(_charGenerator.CreateRandomString(), new AsyncCallback(WaveReadyCallback));
        }

        private void _player_PlayingFinished(object sender, EventArgs e)
        {
            _runner.AcknowledgeSendEnd();
        }

        private void _runner_MorseExit(object sender, EventArgs e)
        {
            _player.Stop();
            _player.ClearQueue();
        }

        private void _runner_StopDelayEnter(object sender, EventArgs e)
        {
        }

        private void _runner_StopDelayExit(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EventHandler(_runner_StopDelayExit), sender, e);
            }
            else
            {
                Analyze();
                btnStartStop.Text = "Start";
                btnStartStop.Enabled = true;
            }
        }

        private void _runner_Abort(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                Invoke(new EventHandler(_runner_Abort), sender, e);
            }
            else
            {
                //Analyze();
                btnStartStop.Text = "Start";
                btnStartStop.Enabled = true;
            }
        }
        #endregion

        #region Analysis
        private void Analyze()
        {
            String sent = _player.Sent;
            String recorded = _recorded.ToString();
            _analyzer.Analyze(sent, recorded);
        }

        #endregion

        #region User Interface

        #region Helper Functions
        private int ScrollSnap(int scrollValue, int min, int max, int increment)
        {
            if (scrollValue < min || scrollValue > max)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (increment != 1)
            {
                scrollValue += increment / 2;
                scrollValue -= scrollValue % increment;
            }
            return scrollValue;
        }

        #endregion

        #region Frequency
        private void FrequencyInitialize(TrackBar slider, TextBox textbox, UInt16 defaultValue, UInt16 min, UInt16 max, UInt16 increment)
        {
            slider.Value = FrequencyValueToSlider(defaultValue);
            slider.Minimum = FrequencyValueToSlider(min);
            slider.Maximum = FrequencyValueToSlider(max);
            slider.TickFrequency = FrequencyValueToSlider(increment);
        }

        private UInt16 FrequencySliderToValue(int scroll)
        {
            return (UInt16)scroll;
        }

        private int FrequencyValueToSlider(UInt16 frequency)
        {
            return frequency;
        }

        private UInt16 FrequencyTextToValue(String text)
        {
            UInt16 frequency = 0;
            if (!UInt16.TryParse(text, out frequency))
            {
                throw new ArgumentException("Unparseable", "frequency");
            }
            return frequency;
        }

        private String FrequencyValueToText(UInt16 frequency)
        {
            return String.Format("{0}", frequency);
        }

        public UInt16 FrequencySlider
        {
            get
            {
                return FrequencySliderToValue(sliderFrequency.Value);
            }
            set
            {
                sliderFrequency.Value = FrequencyValueToSlider(value);
            }
        }

        public UInt16 FrequencyText
        {
            get
            {
                return FrequencyTextToValue(txtFrequency.Text);
            }
            set
            {
                txtFrequency.Text = FrequencyValueToText(value);
                txtFrequency.BackColor = SystemColors.Window;

            }
        }

        public UInt16 Frequency
        {
            get
            {
                return _toneGenerator.Frequency;
            }
            set
            {
                _toneGenerator.Frequency = value;
            }
        }

        private void sliderFrequency_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar slider = (TrackBar)sender;
                int sliderValue = ScrollSnap(
                    slider.Value,
                    FrequencyValueToSlider(ToneGenerator.MIN_FREQUENCY),
                    FrequencyValueToSlider(ToneGenerator.MAX_FREQUENCY),
                    FrequencyValueToSlider(50));
                if (slider.Value != sliderValue)
                {
                    slider.Value = sliderValue;
                }
                UInt16 freq = FrequencySliderToValue(slider.Value);
                Frequency = freq;
                FrequencyText = freq;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtFrequency_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            try
            {
                if (e.KeyChar == '\r')
                {
                    UInt16 freq = FrequencyTextToValue(textbox.Text);
                    Frequency = freq;
                    FrequencySlider = freq;
                    textbox.BackColor = SystemColors.Window;
                }
            }
            catch
            {
                textbox.BackColor = Color.Red;
            }
        }
        #endregion

        #region WPM
        private void WPMInitialize(TrackBar slider, TextBox textbox, float defaultValue, float min, float max, float increment)
        {
            slider.Value = WPMValueToSlider(defaultValue);
            slider.Minimum = WPMValueToSlider(min);
            slider.Maximum = WPMValueToSlider(max);
            slider.TickFrequency = WPMValueToSlider(increment);
        }

        private float WPMSliderToValue(int scroll)
        {
            return (float)(scroll / 2.0f);
        }

        private int WPMValueToSlider(float farnsworthWpm)
        {
            return (int)Math.Round(farnsworthWpm*2);
        }

        private float WPMTextToValue(String text)
        {
            float farnsworthWpm = 0;
            if (!float.TryParse(text, out farnsworthWpm))
            {
                throw new ArgumentException("Unparseable", "farnsworthWpm");
            }
            return farnsworthWpm;
        }

        private String WPMValueToText(float farnsworthWpm)
        {
            return String.Format("{0:#0.0}", farnsworthWpm);
        }

        public float WPMSlider
        {
            get
            {
                return WPMSliderToValue(sliderWPM.Value);
            }
            set
            {
                sliderWPM.Value = WPMValueToSlider(value);
            }
        }

        public float WPMText
        {
            get
            {
                return WPMTextToValue(txtWPM.Text);
            }
            set
            {
                txtWPM.Text = WPMValueToText(value);
                txtWPM.BackColor = SystemColors.Window;
            }
        }

        public float WPM
        {
            get
            {
                return _toneGenerator.WPM;
            }
            set
            {
                _toneGenerator.WPM = value;
                if (_toneGenerator.WPM < _toneGenerator.FarnsworthWPM)
                {
                    FarnsworthWPM = value;
                    FarnsworthWPMSlider = value;
                    FarnsworthWPMText = value;
                }
            }
        }

        private void sliderWPM_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar slider = (TrackBar)sender;
                int sliderValue = ScrollSnap(slider.Value, 
                    WPMValueToSlider(ToneGenerator.MIN_WPM),
                    WPMValueToSlider(ToneGenerator.MAX_WPM),
                    WPMValueToSlider(0.5f));
                if (slider.Value != sliderValue)
                {
                    slider.Value = sliderValue;
                }
                float val = WPMSliderToValue(slider.Value);
                WPM = val;
                WPMText = val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtWPM_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            try
            {
                if (e.KeyChar == '\r')
                {
                    float val = WPMTextToValue(textbox.Text);
                    WPM = val;
                    WPMSlider = val;
                    textbox.BackColor = SystemColors.Window;
                }
            }
            catch
            {
                textbox.BackColor = Color.Red;
            }
        }
        #endregion

        #region FarnsworthWPM
        private void FarnsworthWPMInitialize(TrackBar slider, TextBox textbox, float defaultValue, float min, float max, float increment)
        {
            slider.Value = FarnsworthWPMValueToSlider(defaultValue);
            slider.Minimum = FarnsworthWPMValueToSlider(min);
            slider.Maximum = FarnsworthWPMValueToSlider(max);
            slider.TickFrequency = FarnsworthWPMValueToSlider(increment);
        }

        private float FarnsworthWPMSliderToValue(int scroll)
        {
            return (float)(scroll / 2.0f);
        }

        private int FarnsworthWPMValueToSlider(float farnsworthWpm)
        {
            return (int)Math.Round(farnsworthWpm * 2);
        }

        private float FarnsworthWPMTextToValue(String text)
        {
            float farnsworthWpm = 0;
            if (!float.TryParse(text, out farnsworthWpm))
            {
                throw new ArgumentException("Unparseable", "farnsworthWpm");
            }
            return farnsworthWpm;
        }

        private String FarnsworthWPMValueToText(float farnsworthWpm)
        {
            return String.Format("{0:#0.0}", farnsworthWpm);
        }

        public float FarnsworthWPMSlider
        {
            get
            {
                return FarnsworthWPMSliderToValue(sliderFarnsworth.Value);
            }
            set
            {
                sliderFarnsworth.Value = FarnsworthWPMValueToSlider(value);
            }
        }

        public float FarnsworthWPMText
        {
            get
            {
                return FarnsworthWPMTextToValue(txtFarnsworth.Text);
            }
            set
            {
                txtFarnsworth.Text = FarnsworthWPMValueToText(value);
                txtFarnsworth.BackColor = SystemColors.Window;
            }
        }

        public float FarnsworthWPM
        {
            get
            {
                return _toneGenerator.FarnsworthWPM;
            }
            set
            {
                _toneGenerator.FarnsworthWPM = value;
                if (_toneGenerator.WPM < _toneGenerator.FarnsworthWPM)
                {
                    WPM = value;
                    WPMSlider = value;
                    WPMText = value;
                }
            }
        }

        private void sliderFarnsworth_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar slider = (TrackBar)sender;
                int sliderValue = ScrollSnap(slider.Value,
                    FarnsworthWPMValueToSlider(ToneGenerator.MIN_FARNSWORTH_WPM),
                    FarnsworthWPMValueToSlider(ToneGenerator.MAX_FARNSWORTH_WPM),
                    FarnsworthWPMValueToSlider(0.5f));
                if (slider.Value != sliderValue)
                {
                    slider.Value = sliderValue;
                }
                float val = FarnsworthWPMSliderToValue(slider.Value);
                FarnsworthWPM = val;
                FarnsworthWPMText = val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtFarnsworth_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            try
            {
                if (e.KeyChar == '\r')
                {
                    float val = FarnsworthWPMTextToValue(textbox.Text);
                    FarnsworthWPM = val;
                    FarnsworthWPMSlider = val;
                    textbox.BackColor = SystemColors.Window;
                }
            }
            catch
            {
                textbox.BackColor = Color.Red;
            }
        }
        #endregion

        #region Duration
        private void DurationInitialize(TrackBar slider, TextBox textbox, UInt16 defaultValue, UInt16 min, UInt16 max, UInt16 increment)
        {
            slider.Minimum = DurationValueToSlider(min);
            slider.Maximum = DurationValueToSlider(max);
            slider.Value = DurationValueToSlider(defaultValue);
            slider.TickFrequency = DurationValueToSlider(increment);
        }

        private int DurationSliderToValue(int scroll)
        {
            return scroll;
        }

        private int DurationValueToSlider(int duration)
        {
            return duration;
        }

        private int DurationTextToValue(String text)
        {
            int duration = 0;
            if (!int.TryParse(text, out duration))
            {
                throw new ArgumentException("Unparseable", "duration");
            }
            return duration;
        }

        private String DurationValueToText(int duration)
        {
            return String.Format("{0}", duration);
        }

        public int DurationSlider
        {
            get
            {
                return DurationSliderToValue(sliderDuration.Value);
            }
            set
            {
                sliderDuration.Value = DurationValueToSlider(value);
            }
        }

        public int DurationText
        {
            get
            {
                return DurationTextToValue(txtDuration.Text);
            }
            set
            {
                txtDuration.Text = DurationValueToText(value);
                txtDuration.BackColor = SystemColors.Window;

            }
        }

        public int Duration
        {
            get
            {
                return _runner.SendDuration;
            }
            set
            {
                _runner.SendDuration = value;
            }
        }

        private void sliderDuration_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar slider = (TrackBar)sender;
                int sliderValue = ScrollSnap(
                    slider.Value,
                    DurationValueToSlider(Runner.MIN_DURATION),
                    DurationValueToSlider(Runner.MAX_DURATION),
                    DurationValueToSlider(30));
                if (slider.Value != sliderValue)
                {
                    slider.Value = sliderValue;
                }
                int val = DurationSliderToValue(slider.Value);
                Duration = val;
                DurationText = val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            try
            {
                if (e.KeyChar == '\r')
                {
                    int val = DurationTextToValue(textbox.Text);
                    Duration = val;
                    DurationSlider = val;
                    textbox.BackColor = SystemColors.Window;
                }
            }
            catch
            {
                textbox.BackColor = Color.Red;
            }
        }
        #endregion

        #region Start Delay
        private void StartDelayInitialize(TrackBar slider, TextBox textbox, UInt16 defaultValue, UInt16 min, UInt16 max, UInt16 increment)
        {
            slider.Minimum = StartDelayValueToSlider(min);
            slider.Maximum = StartDelayValueToSlider(max);
            slider.Value = StartDelayValueToSlider(defaultValue);
            slider.TickFrequency = StartDelayValueToSlider(increment);
        }

        private int StartDelaySliderToValue(int scroll)
        {
            return scroll;
        }

        private int StartDelayValueToSlider(int startDelay)
        {
            return startDelay;
        }

        private int StartDelayTextToValue(String text)
        {
            int startDelay = 0;
            if (!int.TryParse(text, out startDelay))
            {
                throw new ArgumentException("Unparseable", "startDelay");
            }
            return startDelay;
        }

        private String StartDelayValueToText(int startDelay)
        {
            return String.Format("{0}", startDelay);
        }

        public int StartDelaySlider
        {
            get
            {
                return StartDelaySliderToValue(sliderStartDelay.Value);
            }
            set
            {
                sliderStartDelay.Value = StartDelayValueToSlider(value);
            }
        }

        public int StartDelayText
        {
            get
            {
                return StartDelayTextToValue(txtStartDelay.Text);
            }
            set
            {
                txtStartDelay.Text = StartDelayValueToText(value);
                txtStartDelay.BackColor = SystemColors.Window;

            }
        }

        public int StartDelay
        {
            get
            {
                return _runner.StartDelay;
            }
            set
            {
                _runner.StartDelay = value;
            }
        }

        private void sliderStartDelay_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar slider = (TrackBar)sender;
                int sliderValue = ScrollSnap(
                    slider.Value,
                    StartDelayValueToSlider(Runner.MIN_START_DELAY),
                    StartDelayValueToSlider(Runner.MAX_START_DELAY),
                    StartDelayValueToSlider(1));
                if (slider.Value != sliderValue)
                {
                    slider.Value = sliderValue;
                }
                int val = StartDelaySliderToValue(slider.Value);
                StartDelay = val;
                StartDelayText = val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtStartDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            try
            {
                if (e.KeyChar == '\r')
                {
                    int val = StartDelayTextToValue(textbox.Text);
                    StartDelay = val;
                    StartDelaySlider = val;
                    textbox.BackColor = SystemColors.Window;
                }
            }
            catch
            {
                textbox.BackColor = Color.Red;
            }
        }
        #endregion

        #region Stop Delay
        private void StopDelayInitialize(TrackBar slider, TextBox textbox, UInt16 defaultValue, UInt16 min, UInt16 max, UInt16 increment)
        {
            slider.Value = StopDelayValueToSlider(defaultValue);
            slider.Minimum = StopDelayValueToSlider(min);
            slider.Maximum = StopDelayValueToSlider(max);
            slider.TickFrequency = StopDelayValueToSlider(increment);
        }

        private int StopDelaySliderToValue(int scroll)
        {
            return scroll;
        }

        private int StopDelayValueToSlider(int stopDelay)
        {
            return stopDelay;
        }

        private int StopDelayTextToValue(String text)
        {
            int stopDelay = 0;
            if (!int.TryParse(text, out stopDelay))
            {
                throw new ArgumentException("Unparseable", "stopDelay");
            }
            return stopDelay;
        }

        private String StopDelayValueToText(int stopDelay)
        {
            return String.Format("{0}", stopDelay);
        }

        public int StopDelaySlider
        {
            get
            {
                return StopDelaySliderToValue(sliderStopDelay.Value);
            }
            set
            {
                sliderStopDelay.Value = StopDelayValueToSlider(value);
            }
        }

        public int StopDelayText
        {
            get
            {
                return StopDelayTextToValue(txtStopDelay.Text);
            }
            set
            {
                txtStopDelay.Text = StopDelayValueToText(value);
                txtStopDelay.BackColor = SystemColors.Window;

            }
        }

        public int StopDelay
        {
            get
            {
                return _runner.StopDelay;
            }
            set
            {
                _runner.StopDelay = value;
            }
        }

        private void sliderStopDelay_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar slider = (TrackBar)sender;
                int sliderValue = ScrollSnap(
                    slider.Value,
                    StopDelayValueToSlider(Runner.MIN_STOP_DELAY),
                    StopDelayValueToSlider(Runner.MAX_STOP_DELAY),
                    StopDelayValueToSlider(1));
                if (slider.Value != sliderValue)
                {
                    slider.Value = sliderValue;
                }
                int val = StopDelaySliderToValue(slider.Value);
                StopDelay = val;
                StopDelayText = val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtStopDelay_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            try
            {
                if (e.KeyChar == '\r')
                {
                    int val = StopDelayTextToValue(textbox.Text);
                    StopDelay = val;
                    StopDelaySlider = val;
                    textbox.BackColor = SystemColors.Window;
                }
            }
            catch
            {
                textbox.BackColor = Color.Red;
            }
        }
        #endregion

        #region Volume
        private void VolumeInitialize(TrackBar slider, TextBox textbox, float defaultValue, float min, float max, float increment)
        {
            slider.Minimum = VolumeValueToSlider(min);
            slider.Maximum = VolumeValueToSlider(max);
            slider.Value = VolumeValueToSlider(defaultValue);
            slider.TickFrequency = VolumeValueToSlider(increment);
        }

        private float VolumeSliderToValue(int scroll)
        {
            return (float)(scroll / 10.0f);
        }

        private int VolumeValueToSlider(float volume)
        {
            return (int)Math.Round(volume * 10.0f);
        }

        private float VolumeTextToValue(String text)
        {
            float volume = 0;
            if (!float.TryParse(text, out volume))
            {
                throw new ArgumentException("Unparseable", "volume");
            }
            return volume;
        }

        private String VolumeValueToText(float volume)
        {
            return String.Format("{0:0}", volume*10);
        }

        public float VolumeSlider
        {
            get
            {
                return VolumeSliderToValue(sliderVolume.Value);
            }
            set
            {
                sliderVolume.Value = VolumeValueToSlider(value);
            }
        }

        public float VolumeText
        {
            get
            {
                return VolumeTextToValue(txtVolume.Text);
            }
            set
            {
                txtVolume.Text = VolumeValueToText(value);
                txtVolume.BackColor = SystemColors.Window;

            }
        }

        public float Volume
        {
            get
            {
                return _toneGenerator.Volume;
            }
            set
            {
                _toneGenerator.Volume = value;
            }
        }

        private void sliderVolume_Scroll(object sender, EventArgs e)
        {
            try
            {
                TrackBar slider = (TrackBar)sender;
                int sliderValue = ScrollSnap(
                    slider.Value,
                    VolumeValueToSlider(ToneGenerator.MIN_VOLUME),
                    VolumeValueToSlider(ToneGenerator.MAX_VOLUME),
                    VolumeValueToSlider(0.1f));
                if (slider.Value != sliderValue)
                {
                    slider.Value = sliderValue;
                }
                float val = VolumeSliderToValue(slider.Value);
                Volume = val;
                VolumeText = val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtVolume_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            try
            {
                if (e.KeyChar == '\r')
                {
                    float val = VolumeTextToValue(textbox.Text);
                    Volume = val;
                    VolumeSlider = val;
                    textbox.BackColor = SystemColors.Window;
                }
            }
            catch
            {
                textbox.BackColor = Color.Red;
            }
        }
        #endregion

        #region Koch/Custom
        private void btnCustom_Click(object sender, EventArgs e)
        {
            _charGenerator.GenerationMethod = CharGenerator.Method.Custom;
        }

        private void btnKoch_Click(object sender, EventArgs e)
        {
            _charGenerator.GenerationMethod = CharGenerator.Method.Koch;
        }

        #endregion

        #region Koch Combo
        private void cmbKoch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = cmbKoch.SelectedIndex;
                if (index >= 0 && index < Koch.Length)
                {
                    _charGenerator.KochIndex = index;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Favor New
        private void chkFavorNew_CheckStateChanged(object sender, EventArgs e)
        {
            _charGenerator.FavorNew = chkFavorNew.CheckState == CheckState.Checked;
        }

        #endregion

        #region Custom String
        #endregion

        #endregion

        #region Context Menu Items
        private void mnuContextRestoreDefaults_Click(object sender, EventArgs e)
        {
            Config.Delete(CONFIG_FILE_NAME);
            ApplyConfig(Config.Load(CONFIG_FILE_NAME));
        }

        private void mnuContextSetProsignKeys_Click(object sender, EventArgs e)
        {

        }

        private void mnuContextAbout_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }
        #endregion

        #region User Key
        private bool UserKey(char key)
        {
            bool processed = false;
            String expanded = MorseInfo.ExpandProsigns(key.ToString()).ToUpperInvariant();
            txtAnalysis.AppendText(expanded);
            _recorded.Append(expanded);
            processed = true;

            return processed;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_runner.IsListenMode)
            {
                e.Handled = UserKey(e.KeyChar);
            }
        }
        #endregion

        #region Private Fields
        private ToneGenerator _toneGenerator;
        private CharGenerator _charGenerator;
        private WordToToneBuilder _builder;
        private SoundPlayerAsync _player;
        private Runner _runner;
        private Analyzer _analyzer;
        private StringBuilder _recorded;

        #endregion
    }
}
