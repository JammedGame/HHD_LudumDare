using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Mathematics
{
    public class TimeCode
    {
        public int H;
        public int M;
        public int S;
        public int F;
        public int SF;
        public int SFC;
        public int SPF;
        public string Standard;
        public TimeCode()
        {
            this.H = 0;
            this.M = 0;
            this.S = 0;
            this.F = 0;
            this.SF = 0;
            this.SFC = 30;
            this.SPF = 1;
            this.Standard = "Unknown";
        }
        public TimeCode(TimeCode Old)
        {
            this.H = Old.H;
            this.M = Old.M;
            this.S = Old.S;
            this.F = Old.F;
            this.SF = Old.SF;
            this.SFC = Old.SFC;
            this.SPF = Old.SPF;
            this.Standard = Old.Standard;
        }
        public TimeCode(double DataRate)
        {
            this.H = 0;
            this.M = 0;
            this.S = 0;
            this.F = 0;
            this.SF = 0;
            if ((int)(DataRate) % 25 == 0) this.Standard = "PAL";
            else this.Standard = "SMPTE";
            if (Standard == "PAL") this.SFC = 25;
            if (Standard == "NTSC") this.SFC = 30;
            if (Standard == "SMPTE") this.SFC = 30;
            DataRate = Math.Ceiling(DataRate);
            this.SPF = (int)DataRate / SFC;
        }
        public TimeCode(int SPF)
        {
            this.H = 0;
            this.M = 0;
            this.S = 0;
            this.F = 0;
            this.SF = 0;
            this.SFC = 30;
            this.SPF = SPF;
            this.Standard = "Unknown";
        }
        public TimeCode(string TCVal, double DataRate)
        {
            this.H = Convert.ToInt32(TCVal.Substring(0, 2));
            this.M = Convert.ToInt32(TCVal.Substring(3, 2));
            this.S = Convert.ToInt32(TCVal.Substring(6, 2));
            this.F = Convert.ToInt32(TCVal.Substring(9, 2));
            if (TCVal.Length == 15) this.SF = Convert.ToInt32(TCVal.Substring(12, 2));
            else this.SF = 0;
            DataRate = Math.Ceiling(DataRate);
            this.SFC = 30;
            this.SPF = (int)DataRate / SFC;
            this.Standard = "Unknown";
        }
        public TimeCode(string TCVal, int SPF)
        {
            this.H = Convert.ToInt32(TCVal.Substring(0, 2));
            this.M = Convert.ToInt32(TCVal.Substring(3, 2));
            this.S = Convert.ToInt32(TCVal.Substring(6, 2));
            this.F = Convert.ToInt32(TCVal.Substring(9, 2));
            if (TCVal.Length == 15) this.SF = Convert.ToInt32(TCVal.Substring(12, 2));
            else this.SF = 0;
            this.SFC = 30;
            this.SPF = SPF;
            this.Standard = "Unknown";
        }
        public TimeCode(string TCVal, double DataRate, string Standard)
        {
            this.H = Convert.ToInt32(TCVal.Substring(0, 2));
            this.M = Convert.ToInt32(TCVal.Substring(3, 2));
            this.S = Convert.ToInt32(TCVal.Substring(6, 2));
            this.F = Convert.ToInt32(TCVal.Substring(9, 2));
            if (TCVal.Length == 15) this.SF = Convert.ToInt32(TCVal.Substring(12, 2));
            else this.SF = 0;
            if (Standard == "FILM") this.SFC = 24;
            if (Standard == "PAL") this.SFC = 25;
            if (Standard == "NTSC") this.SFC = 30;
            if (Standard == "SMPTE") this.SFC = 30;
            DataRate = Math.Ceiling(DataRate);
            this.SPF = (int)DataRate / SFC;
            this.Standard = Standard;
        }
        public TimeCode(string TCVal, int SPF, string Standard)
        {
            this.H = Convert.ToInt32(TCVal.Substring(0, 2));
            this.M = Convert.ToInt32(TCVal.Substring(3, 2));
            this.S = Convert.ToInt32(TCVal.Substring(6, 2));
            this.F = Convert.ToInt32(TCVal.Substring(9, 2));
            if (TCVal.Length == 15) this.SF = Convert.ToInt32(TCVal.Substring(12, 2));
            else this.SF = 0;
            this.SPF = SPF;
            this.Standard = Standard;
            if (Standard == "FILM") this.SFC = 24;
            else if (Standard == "PAL") this.SFC = 25;
            else if (Standard == "NTSC") this.SFC = 30;
            else if (Standard == "SMPTE") this.SFC = 30;
            else this.SFC = 30;
        }
        public TimeCode(long TimeVal, double DataRate)
        {
            this.H = (int)TimeVal / (60 * 60 * 30 * SPF);
            TimeVal %= 60 * 60 * 30 * SPF;
            this.M = (int)TimeVal / (60 * 30 * SPF);
            TimeVal %= 60 * 30 * SPF;
            this.S = (int)TimeVal / (30 * SPF);
            TimeVal %= 30 * SPF;
            this.F = (int)TimeVal / SPF;
            TimeVal %= SPF;
            this.SF = (int)TimeVal;
            DataRate = Math.Ceiling(DataRate);
            this.Standard = "SMPTE";
            this.SFC = 30;
            this.SPF = (int)DataRate / SFC;
        }
        public TimeCode(long TimeVal, int SPF)
        {
            this.H = (int)TimeVal / (60 * 60 * 30 * SPF);
            TimeVal %= 60 * 60 * 30 * SPF;
            this.M = (int)TimeVal / (60 * 30 * SPF);
            TimeVal %= 60 * 30 * SPF;
            this.S = (int)TimeVal / (30 * SPF);
            TimeVal %= 30 * SPF;
            this.F = (int)TimeVal / SPF;
            TimeVal %= SPF;
            this.SF = (int)TimeVal;
            this.SFC = 30;
            this.SPF = SPF;
            this.Standard = "SMPTE";
        }
        public TimeCode(long TimeVal, int SPF, string Standard)
        {
            this.Standard = Standard;
            this.SPF = SPF;
            if (Standard == "FILM") this.SFC = 24;
            else if (Standard == "PAL") this.SFC = 25;
            else if (Standard == "NTSC") this.SFC = 30;
            else if (Standard == "SMPTE") this.SFC = 30;
            else this.SFC = 30;
            if (SPF == 0) SPF = 1;
            this.H = (int)TimeVal / (60 * 60 * SFC * SPF);
            TimeVal %= 60 * 60 * SFC * SPF;
            this.M = (int)TimeVal / (60 * SFC * SPF);
            TimeVal %= 60 * SFC * SPF;
            this.S = (int)TimeVal / (SFC * SPF);
            TimeVal %= 30 * SPF;
            this.F = (int)TimeVal / SPF;
            TimeVal %= SPF;
            this.SF = (int)TimeVal;
        }
        public TimeCode(int H, int M, int S, int F)
        {
            this.H = H;
            this.M = M;
            this.S = S;
            this.F = F;
            this.SF = 0;
            this.SPF = 1;
            this.Standard = "SMPTE";
            this.SFC = 30;
        }
        public TimeCode(int H, int M, int S, int F, double DataRate)
        {
            this.H = H;
            this.M = M;
            this.S = S;
            this.F = F;
            this.SF = 0;
            this.Standard = "SMPTE";
            this.SFC = 30;
            DataRate = Math.Ceiling(DataRate);
            this.SPF = (int)DataRate / SFC;
        }
        public TimeCode(int H, int M, int S, int F, int SPF)
        {
            this.H = H;
            this.M = M;
            this.S = S;
            this.F = F;
            this.SF = 0;
            this.SPF = SPF;
            this.Standard = "SMPTE";
            this.SFC = 30;
        }
        public TimeCode(int H, int M, int S, int F, int SPF, string Standard)
        {
            this.H = H;
            this.M = M;
            this.S = S;
            this.F = F;
            this.SF = 0;
            this.SPF = SPF;
            this.Standard = Standard;
            if (Standard == "FILM") this.SFC = 24;
            else if (Standard == "PAL") this.SFC = 25;
            else if (Standard == "NTSC") this.SFC = 30;
            else if (Standard == "SMPTE") this.SFC = 30;
            else this.SFC = 30;
        }
        public TimeCode(int H, int M, int S, int F, int SF, double DataRate)
        {
            this.H = H;
            this.M = M;
            this.S = S;
            this.F = F;
            this.SF = SF;
            this.SFC = 30;
            DataRate = Math.Ceiling(DataRate);
            this.SPF = (int)DataRate / SFC;
            this.Standard = "SMPTE";
        }
        public TimeCode(int H, int M, int S, int F, int SF, int SPF)
        {
            this.H = H;
            this.M = M;
            this.S = S;
            this.F = F;
            this.SF = SF;
            this.SFC = 30;
            this.SPF = SPF;
            this.Standard = "SMPTE";
        }
        public TimeCode(int H, int M, int S, int F, int SF, int SPF, string Standard)
        {
            this.H = H;
            this.M = M;
            this.S = S;
            this.F = F;
            this.SF = SF;
            this.SPF = SPF;
            this.Standard = Standard;
            if (Standard == "FILM") this.SFC = 24;
            else if (Standard == "PAL") this.SFC = 25;
            else if (Standard == "NTSC") this.SFC = 30;
            else if (Standard == "SMPTE") this.SFC = 30;
            else this.SFC = 30;
        }
        public TimeCode(int H, int M, int S, int F, int SF, int SPF, string Standard, bool DropFrame)
        {
            this.H = H;
            this.M = M;
            this.S = S;
            this.F = F;
            this.SF = SF;
            this.SPF = SPF;
            this.Standard = Standard;
            if (Standard == "FILM") this.SFC = 24;
            else if (Standard == "PAL") this.SFC = 25;
            else if (Standard == "NTSC") this.SFC = 30;
            else if (Standard == "SMPTE") this.SFC = 30;
            else this.SFC = 30;
            if (DropFrame)
            {
                long TimeVal = this.ToTimeVal();
                TimeVal = (long)(TimeVal * (999 * 1.0 / 1000));
                this.H = (int)TimeVal / (60 * 60 * SFC * SPF);
                TimeVal %= 60 * 60 * SFC * SPF;
                this.M = (int)TimeVal / (60 * SFC * SPF);
                TimeVal %= 60 * SFC * SPF;
                this.S = (int)TimeVal / (SFC * SPF);
                TimeVal %= 30 * SPF;
                this.F = (int)TimeVal / SPF;
                TimeVal %= SPF;
                this.SF = (int)TimeVal;
            }
        }
        public static TimeCode operator +(TimeCode A, TimeCode B)
        {
            if (A.SPF != B.SPF) return new TimeCode();
            return new TimeCode(A.ToTimeVal() + B.ToTimeVal(), A.SPF, A.Standard);
        }
        public static TimeCode operator +(TimeCode A, long B)
        {
            return new TimeCode(A.ToTimeVal() + B, A.SPF, A.Standard);
        }
        public static TimeCode operator -(TimeCode A, TimeCode B)
        {
            if (A.SPF != B.SPF) return new TimeCode();
            return new TimeCode(A.ToTimeVal() - B.ToTimeVal(), A.SPF, A.Standard);
        }
        public bool IsValid()
        {
            if (H == 0 && M == 0 && S == 0 && F == 0) return false;
            else return true;
        }
        public long ToTimeVal()
        {
            return (this.H * 60 * 60 * SFC * SPF) + (this.M * 60 * SFC * SPF) + (this.S * SFC * SPF) + (this.F * SPF) + this.SF;
        }
        public double ToSeconds()
        {
            return (this.H * 60 * 60) + (this.M * 60) + this.S + (this.F * 1.0 / SFC) + (this.SF * 1.0 / SPF * 1.0 / SFC);
        }
        public override string ToString()
        {
            string TimeCodeString = this.H.ToString("00") + ":" + this.M.ToString("00") + ":" + this.S.ToString("00") + ":" + this.F.ToString("00");
            if (this.SPF > 1) TimeCodeString += "(" + this.SF.ToString("00") + ")";
            return TimeCodeString;
        }
        public string ToStringNoSubframe()
        {
            string TimeCodeString = this.H.ToString("00") + ":" + this.M.ToString("00") + ":" + this.S.ToString("00") + ":" + this.F.ToString("00");
            return TimeCodeString;
        }
        public long GetFrame(TimeCode A)
        {
            return A.ToTimeVal() - this.ToTimeVal();
        }
        public long ToMiliseconds()
        {
            int MilSec = 0;
            MilSec += H * 60 * 60 * 1000;
            MilSec += M * 60 * 1000;
            MilSec += S * 1000;
            MilSec += (int)(((F * 1.0) / (SFC)) * 1000);
            MilSec += (int)(((SF * 1.0) / (SPF * SFC)) * 1000);
            return (this.H * 60 * 60 * SFC * SPF) + (this.M * 60 * SFC * SPF) + (this.S * SFC * SPF) + (this.F * SPF) + this.SF;
        }
    }
}
