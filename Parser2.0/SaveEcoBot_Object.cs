using System;
using System.Collections.Generic;

namespace Parser2._0
{
    class SaveEcoBot_Object
    {
        public string id { get; set; }
        public string cityName { get; set; }
        public string stationName { get; set; }
        public string localName { get; set; }
        public string timezone { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public List<SaveEcoBotObject_Pollutant> pollutants { get; set; }
        public SaveEcoBot_Object()
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
