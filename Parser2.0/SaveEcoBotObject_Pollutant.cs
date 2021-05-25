using System;


namespace Parser2._0
{
    class SaveEcoBotObject_Pollutant
    {
        internal string pol;
        internal string unit;
        internal DateTime time;
        internal float value;
        internal string averaging;
        internal SaveEcoBotObject_Pollutant()
        {
            pol = null;
            unit = null;
            time = DateTime.MinValue;
            value = 0;
            averaging = null;
        }
    }
}
