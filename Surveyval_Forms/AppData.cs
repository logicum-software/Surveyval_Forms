﻿using System;
using System.Collections.Generic;

namespace Surveyval_Forms
{
    [Serializable]
    class AppData
    {
        public List<Fragebogen> appFrageboegen { get; set; }
        public List<Frage> appFragen { get; set; }

        public AppData()
        {
            appFrageboegen = new List<Fragebogen>();
            appFragen = new List<Frage>();
        }

        /*internal void save()
        {
            FileStream fs = new FileStream("udata1.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, appFrageboegen);
            }
            catch (SerializationException ec)
            {
                MessageBox.Show(ec.Message, "Speicherfehler", MessageBoxButtons.OK);
                //Console.WriteLine("Failed to serialize. Reason: " + ec.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            fs = new FileStream("udata2.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, appFragen);
            }
            catch (SerializationException ec)
            {
                MessageBox.Show(ec.Message, "Speicherfehler", MessageBoxButtons.OK);
                //Console.WriteLine("Failed to serialize. Reason: " + ec.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }*/

        internal Boolean isContaining(Fragebogen tmp)
        {
            foreach (Fragebogen item in appFrageboegen)
                if (tmp.strName.Equals(item.strName))
                    return true;

            return false;
        }

        internal Boolean isContaining(Frage tmp)
        {
            foreach (Frage item in appFragen)
                if (tmp.strFragetext.Equals(item.strFragetext))
                    return true;

            return false;
        }

        internal void removeFragebogen(Fragebogen tmp)
        {
            foreach (Fragebogen item in appFrageboegen)
            {
                if (item.strName.Equals(tmp.strName))
                    appFrageboegen.Remove(item);
            }
        }
    }
}
