﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surveyval_Forms
{
    [Serializable]
    class Frage
    {
        public String strFragetext { get; set; }
        public int nAntwortart { get; set; }

        public Frage()
        {
            strFragetext = "";
            nAntwortart = 0;
        }

        public Frage(String fragetext, int antwortart)
        {
            strFragetext = fragetext;
            nAntwortart = antwortart;
        }
    }
}
