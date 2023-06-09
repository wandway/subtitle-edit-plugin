﻿using System;

namespace Nikse.SubtitleEdit.PluginLogic
{
    internal class TimeCode
    {
        private TimeSpan _time;

        internal TimeCode(TimeSpan timeSpan)
        {
            TimeSpan = timeSpan;
        }

        internal TimeCode(int hour, int minute, int seconds, int milliseconds)
        {
            _time = new TimeSpan(0, hour, minute, seconds, milliseconds);
        }

        internal int Hours
        {
            get { return _time.Hours; }
            set { _time = new TimeSpan(0, value, _time.Minutes, _time.Seconds, _time.Milliseconds); }
        }

        internal int Milliseconds
        {
            get { return _time.Milliseconds; }
            set { _time = new TimeSpan(0, _time.Hours, _time.Minutes, _time.Seconds, value); }
        }

        internal int Minutes
        {
            get { return _time.Minutes; }
            set { _time = new TimeSpan(0, _time.Hours, value, _time.Seconds, _time.Milliseconds); }
        }

        internal int Seconds
        {
            get { return _time.Seconds; }
            set { _time = new TimeSpan(0, _time.Hours, _time.Minutes, value, _time.Milliseconds); }
        }

        internal TimeSpan TimeSpan
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }

        internal double TotalMilliseconds
        {
            get { return _time.TotalMilliseconds; }
            set { _time = TimeSpan.FromMilliseconds(value); }
        }

        internal double TotalSeconds
        {
            get { return _time.TotalSeconds; }
            set { _time = TimeSpan.FromSeconds(value); }
        }

        public string ToHHMMSSFF()
        {
            return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", _time.Hours, _time.Minutes, _time.Seconds, SubtitleFormat.MillisecondsToFrames(_time.Milliseconds));
        }

        public string ToHHMMSSPeriodFF()
        {
            return string.Format("{0:00}:{1:00}:{2:00}.{3:00}", _time.Hours, _time.Minutes, _time.Seconds, SubtitleFormat.MillisecondsToFrames(_time.Milliseconds));
        }

        public override string ToString()
        {
            string s = string.Format("{0:00}:{1:00}:{2:00},{3:000}", _time.Hours, _time.Minutes, _time.Seconds, _time.Milliseconds);

            if (TotalMilliseconds >= 0)
                return s;
            else
                return "-" + s.Replace("-", string.Empty);
        }

        internal static double ParseHHMMSSFFToMilliseconds(string text)
        {
            string[] parts = text.Split(":,.".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4)
            {
                int hours;
                int minutes;
                int seconds;
                int frames;
                if (int.TryParse(parts[0], out hours) && int.TryParse(parts[1], out minutes) && int.TryParse(parts[2], out seconds) && int.TryParse(parts[3], out frames))
                {
                    TimeSpan ts = new TimeSpan(0, hours, minutes, seconds, SubtitleFormat.FramesToMilliseconds(frames));
                    return ts.TotalMilliseconds;
                }
            }
            return 0;
        }

        internal static double ParseToMilliseconds(string text)
        {
            string[] parts = text.Split(":,.".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 4)
            {
                int hours;
                int minutes;
                int seconds;
                int milliseconds;
                if (int.TryParse(parts[0], out hours) && int.TryParse(parts[1], out minutes) && int.TryParse(parts[2], out seconds) && int.TryParse(parts[3], out milliseconds))
                {
                    TimeSpan ts = new TimeSpan(0, hours, minutes, seconds, milliseconds);
                    return ts.TotalMilliseconds;
                }
            }
            return 0;
        }

        internal void AddTime(int hour, int minutes, int seconds, int milliseconds)
        {
            Hours += hour;
            Minutes += minutes;
            Seconds += seconds;
            Milliseconds += milliseconds;
        }

        internal void AddTime(long milliseconds)
        {
            _time = TimeSpan.FromMilliseconds(_time.TotalMilliseconds + milliseconds);
        }

        internal void AddTime(TimeSpan timeSpan)
        {
            _time = TimeSpan.FromMilliseconds(_time.TotalMilliseconds + timeSpan.TotalMilliseconds);
        }

        internal void AddTime(double milliseconds)
        {
            _time = TimeSpan.FromMilliseconds(_time.TotalMilliseconds + milliseconds);
        }

        internal string ToShortString()
        {
            string s;
            if (_time.Minutes == 0 && _time.Hours == 0)
                s = string.Format("{0:0},{1:000}", _time.Seconds, _time.Milliseconds);
            else if (_time.Hours == 0)
                s = string.Format("{0:0}:{1:00},{2:000}", _time.Minutes, _time.Seconds, _time.Milliseconds);
            else
                s = string.Format("{0:0}:{1:00}:{2:00},{3:000}", _time.Hours, _time.Minutes, _time.Seconds, _time.Milliseconds);

            if (TotalMilliseconds >= 0)
                return s;
            else
                return "-" + s.Replace("-", string.Empty);
        }

        internal string ToShortStringHHMMSSFF()
        {
            if (_time.Minutes == 0 && _time.Hours == 0)
                return string.Format("{0:00}:{1:00}", _time.Seconds, SubtitleFormat.MillisecondsToFrames(_time.Milliseconds));
            if (_time.Hours == 0)
                return string.Format("{0:00}:{1:00}:{2:00}", _time.Minutes, _time.Seconds, SubtitleFormat.MillisecondsToFrames(_time.Milliseconds));
            return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", _time.Hours, _time.Minutes, _time.Seconds, SubtitleFormat.MillisecondsToFrames(_time.Milliseconds));
        }
    }
}