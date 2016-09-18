using Stock.Common;

namespace Stock.DataProvider
{
    public enum IBStandardHistoryDataRange
    {
        [EnumDescription("1 D")]
        Day = 1,
        [EnumDescription("2 D")]
        TwoDay = 2,
        [EnumDescription("1 W")]
        Week = 7,
        [EnumDescription("1 M")]
        Month = 30,
        [EnumDescription("3 M")]
        Quater = 90,
        [EnumDescription("6 M")]
        HalfYear = 180,
        [EnumDescription("1 Y")]
        Year = 365,
        [EnumDescription("1800 S")]
        HalfHour = 1800

    }
    public enum IBStandardHistoryBarSize
    {
        [EnumDescription("1 day")]
        Day = 1,
        [EnumDescription("1 hour")]
        Hour = 12,
        [EnumDescription("30 mins")]
        Min30 = 230,
        [EnumDescription("15 mins")]
        Min15 = 215,
        [EnumDescription("10 mins")]
        Min10 = 210,
        [EnumDescription("5 mins")]
        Min5 = 205,
        [EnumDescription("3 mins")]
        Min3 = 203,
        [EnumDescription("2 mins")]
        Min2 = 202,
        [EnumDescription("1 min")]
        Min1 = 201,
        [EnumDescription("30 secs")]
        Sec30 = 330,
        [EnumDescription("15 secs")]
        Sec15 = 315,
        [EnumDescription("5 secs")]
        Sec05 = 305,
        [EnumDescription("1 secs")]
        Sec01 = 301

    }
}
