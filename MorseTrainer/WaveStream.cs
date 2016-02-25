using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    public class WaveStream
    {
        public WaveStream(String text, Int16[] waveform, UInt32 sampleRate, UInt32 samplesPerCycle)
        {
            _text = text;
            SetupStream(waveform, sampleRate, samplesPerCycle);
        }

        public WaveStream(String text, IEnumerable<Int16[]> waveforms, UInt32 sampleRate, UInt32 samplesPerCycle)
        {
            _text = text;
            SetupStream(waveforms, sampleRate, samplesPerCycle);
        }

        public String Text
        {
            get
            {
                return _text;
            }
        }

        public System.IO.Stream Stream
        {
            get
            {
                return _stream;
            }
        }

        // Create a stream
        // Header
        //      4 bytes "RIFF"
        //      4 bytes chunkSize
        //      4 bytes "WAVE"
        //      
        // Subchunk 1
        //      4 bytes subchunk ID ("fmt ")
        //      4 bytes subchunk size (16)
        //      2 bytes audio format (1)
        //      2 bytes num channels (1)
        //      4 bytes sample rate (8000)
        //      4 bytes byte rate = SampleRate * NumChannels * BitsPerSample/8 ()
        //      2 bytes block align = NumChannels * BitsPerSample/8 (2)
        //      2 bytes bits per sample (16)
        //
        // Subchunk 2
        //      4 bytes subchunk ID ("data")
        //      4 bytes subchunk size (2*waveform.Length)
        //      copy of waveform
        private void SetupStream(
            Int16[] waveform,
            UInt32 sampleRate,
            UInt32 samplesPerCycle
            )
        {
            List<Int16[]> waveforms = new List<Int16[]>();
            waveforms.Add(waveform);
            SetupStream(waveforms, sampleRate, samplesPerCycle);
        }

        private void SetupStream(
            IEnumerable<Int16[]> waveforms,
            UInt32 sampleRate,
            UInt32 samplesPerCycle
            )
        {
            // Create stream
            _stream = new System.IO.MemoryStream();

            UInt32 totalWaveformLength = GetSize(waveforms);

            _stream.Write(StringToBytes("RIFF"), 0, 4);
            _stream.Write(LittleEndian(36 + 2* totalWaveformLength, 4), 0, 4);
            _stream.Write(StringToBytes("WAVE"), 0, 4);

            _stream.Write(StringToBytes("fmt "), 0, 4);
            _stream.Write(LittleEndian(16, 4), 0, 4); // chunk1Size
            _stream.Write(LittleEndian(1, 2), 0, 2); // uncompressed linear
            _stream.Write(LittleEndian(1, 2), 0, 2); // 1 channel
            _stream.Write(LittleEndian(sampleRate, 4), 0, 4); // sampleRate
            _stream.Write(LittleEndian(sampleRate * 2, 4), 0, 4); // sampleRate * channels * bitrate / 8
            _stream.Write(LittleEndian(2, 2), 0, 2); // block align
            _stream.Write(LittleEndian(16, 2), 0, 2); // bits per sample

            // Fill in the tone
            _stream.Write(StringToBytes("data"), 0, 4);
            _stream.Write(LittleEndian(totalWaveformLength * 2, 4), 0, 4);
            foreach (Int16[] waveform in waveforms)
            {
                foreach (Int16 sample in waveform)
                {
                    _stream.Write(LittleEndian(sample, 2), 0, 2);
                }
            }
            _stream.Seek(0, System.IO.SeekOrigin.Begin);
        }

        private UInt32 GetSize(IEnumerable<Int16[]> partials)
        {
            UInt32 count = 0;
            foreach (Int16[] waveform in partials)
            {
                count += (UInt32)waveform.Length;
            }
            return count;
        }

        private void FillWaveForms(IEnumerable<UInt16[]> partials)
        {
            foreach (UInt16[] waveform in partials)
            {
                FillWaveForm(waveform);
            }
        }

        private void FillWaveForm(UInt16[] waveform)
        {
            foreach (Int16 sample in waveform)
            {
                _stream.Write(LittleEndian(sample, 2), 0, 2);
            }
        }

        private byte[] StringToBytes(String str)
        {
            return System.Text.UTF7Encoding.ASCII.GetBytes(str);
        }

        private byte[] LittleEndian(UInt32 n, int byteCount)
        {
            byte[] bytes = new byte[byteCount];
            int i = 0;
            while (i < byteCount)
            {
                bytes[i] = (byte)n;
                i++;
                n = n >> 8;
            }
            return bytes;
        }

        private byte[] LittleEndian(Int32 n, int byteCount)
        {
            return LittleEndian((UInt32)n, byteCount);
        }

        private String _text;
        private System.IO.MemoryStream _stream;
    }
}
