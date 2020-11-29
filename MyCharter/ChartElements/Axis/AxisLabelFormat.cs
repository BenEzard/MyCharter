namespace MyCharter.ChartElements.Axis
{
    /// <summary>
    /// The format of the labels on the Axis.
    /// </summary>
    public enum AxisLabelFormat
    {
        NONE,
        DATE_DDMMYY1,           // DD/MM/YY
        DATE_DDMMYY2,           // DD-MM-YY
        DATE_DDMMYYYY1,         // DD/MM/YYYY
        DATE_DDMMYYYY2,         // DD-MM-YYYY
        DATE_DDMM1,             // DD/MM
        DATE_DDMM2,             // DD-MM
        DATE_Wwww,              // Monday
        DATE_WWWWW,             // MONDAY
        DATE_Www,               // Mon
        DATE_WWW,               // MON
        DATE_WwwDD,             // Mon 26
        DATE_WWWDD,             // MON 26

        DATETIME_DDMMYYYY1_HHMM24,         // DD/MM/YYYY 13:04
        DATETIME_DDMMYYYY2_HHMM24,         // DD-MM-YYYY 13:04
        DATETIME_DDMM1_HHMM24,             // DD/MM 13:04
        DATETIME_DDMM2_HHMM24,             // DD-MM 13:04
        DATETIME_Wwww_HHMM24,              // Monday 13:04
        DATETIME_WWWWW_HHMM24,             // MONDAY 13:04
        DATETIME_Www_HHMM24,               // Mon 13:04
        DATETIME_WWW_HHMM24,               // MON 13:04
        DATETIME_WwwDD_HHMM24,             // Mon 26 13:04
        DATETIME_WWWDD_HHMM24,             // MON 26 13:04

        NUMBER_ALL,             // Full Number Value 1234
        NUMBER_THOU_SEP_COMMA,  // Thousands, comma-separated. 1,234
        NUMBER_THOU_SEP_SPACE,  // Thousands, space-separated. 1 234
        NUMBER_THOU_ABBR,       // Thousands, abbreviated 1.2k
        NUMBER_MIL_ABBR,       // Thousands, abbreviated 1.2m
    }
}
