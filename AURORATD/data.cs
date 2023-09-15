using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers; 

namespace AURORATD
{
    [DelimitedRecord(",")]
    public class data
    {

        public string
            csvWaktu,
            csvLokasi,
            csvSuhu,
            csvKelembaban,
            csvCO,
            csvpm25,
            csvpm10,
            csvISPU,
            csvkategori

            ;
        
    }
}
