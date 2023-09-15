using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static GMap.NET.Entity.OpenStreetMapGeocodeEntity;

namespace AURORATD
{
    public class Data_json
    {
        [DataMember]
        public string alert { get; set; }
        [DataMember]
        public string tanggal { get; set; }
        [DataMember]
        public string jam { get; set; }
        [DataMember]
        public string lokasi { get; set; }
        [DataMember]
        public string penerima { get; set; }
        [DataMember]
        public string hpPenerima { get; set; }
        [DataMember]
        public string lat { get; set; }
        [DataMember]
        public string lon{ get; set; }
        [DataMember]
        public string suhu { get; set; }
        [DataMember]
        public string hum { get; set; }
        [DataMember]
        public string kCO { get; set; }
        [DataMember]
        public string pm25 { get; set; }
        [DataMember]
        public string pm10 { get; set; }
        [DataMember]
        public string cemar { get; set; }
        [DataMember]
        public string ISPU { get; set; }
        [DataMember]
        public  string kategori { get; set; }
    }
}
