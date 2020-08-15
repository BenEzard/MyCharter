namespace MyCharter.ChartElements.Axis
{
    public enum AxisLabelFormat
    {
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
        NUMBER_ALL,             // Full Number Value 1234
        NUMBER_THOU_SEP_COMMA,  // Thousands, comma-separated. 1,234
        NUMBER_THOU_SEP_SPACE,  // Thousands, space-separated. 1 234
        NUMBER_THOU_ABBR,       // Thousands, abbreviated 1.2k
        NUMBER_MIL_ABBR,       // Thousands, abbreviated 1.2k
    }
}
