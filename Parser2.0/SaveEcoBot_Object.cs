using System.Collections.Generic;

namespace Parser2._0
{
    class SaveEcoBot_Object
    {
        internal int id;
        internal string cityName;
        internal string stationName;
        internal string localName;
        internal string timezone;
        internal float latitude;
        internal float longitude;
        internal List<SaveEcoBotObject_Pollutant> pollutants;
        internal SaveEcoBot_Object()
        {
            int id = 0;
            string cityName = null;
            string stationName = null;
            string localName = null;
            string timezone = null;
            float latitude = 0;
            float longitude = 0;
            pollutants = new List<SaveEcoBotObject_Pollutant>();
        }
    }
}
