using System;


namespace Parser2._0
{
    class SaveEcoBotObject_Pollutant
    {
        public string pol { get; set; }
        public string unit { get; set; }
        public string time { get; set; }
        public string value { get; set; }
        public string averaging { get; set; }
        public SaveEcoBotObject_Pollutant()
        {
            pol = null;
            unit = null;
            time = null;
            value = null;
            averaging = null;
        }
    }
}
