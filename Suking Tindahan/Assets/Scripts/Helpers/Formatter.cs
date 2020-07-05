using Player.Data;

namespace Helpers
{
    public static class Formatter
    {
        public static string ToNiceString(this Size size)
        {
            switch (size)
            {
                case Size.S100: return "100%";
                case Size.S90: return "90%";
                case Size.S80: return "80%";
                case Size.S70: return "70%";
                case Size.S60: return "60%";
                case Size.S50: return "50%";
                case Size.S40: return "40%";
                case Size.S30: return "30%";
                case Size.S110: return "110%";
                case Size.S120: return "120%";
                case Size.S130: return "130%";
                case Size.S140: return "140%";
                case Size.S150: return "150%";
                case Size.S160: return "160%";
                case Size.S170: return "170%";
                default:
                    return "???";
            }
        }
    }
}