using System;
using System.Collections.Generic;
using System.Text;

namespace MyCharter.ChartElements.Axis
{
    public enum DateFormat
    {
        DDMMYYYY1,  // DD/MM/YYYY
        DDMMYYYY2,  // DD-MM-YYYY
        DDMM1,      // DD/MM
        DDMM2,      // DD-MM
        Wwww,       // Monday
        WWWWW,      // MONDAY
        Www,        // Mon
        WWW,        // MON
        WwwDD,      // Mon 26
        WWWDD,      // MON 26
    }
}
