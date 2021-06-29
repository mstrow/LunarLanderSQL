using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LanderGame
{
    public static class DataHelper
    {
        public static int map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
