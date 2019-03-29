using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Surveyval_Forms
{
    public partial class Manager : Form
    {
        private AppData appData;

        public Manager()
        {
            InitializeComponent();

            appData = new AppData();

            //Daten einlesen aus Datei "udata.dat"
            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream("udata.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
                appData = (AppData)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message, "Dateifehler", MessageBoxButtons.OK);
                //throw;
            }

            refreshLists();
        }

        private void refreshLists()
        {
            foreach (ListViewItem item in listView1.Items)
                item.Remove();

            foreach (Fragebogen item in appData.appFrageboegen)
                listView1.Items.Add(new ListViewItem(item.strName));

            foreach (ListViewItem item in listView2.Items)
                item.Remove();

            foreach (Frage item in appData.appFragen)
                listView2.Items.Add(new ListViewItem(item.strFragetext));

            foreach (ListViewItem item in listView3.Items)
                item.Remove();

            if (listView1.Items.Count > 0)
            {
                // ÜBERPRÜFEN !!!
                listView1.Items[0].Selected = true;
                foreach (Frage item in appData.appFrageboegen[0].Fragen)
                    listView3.Items.Add(new ListViewItem(item.strFragetext));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NeueFrage dlgNeueFrage = new NeueFrage();

            dlgNeueFrage.textBox1.Text = "Neue Frage eingeben...";
            dlgNeueFrage.textBox1.Focus();
            dlgNeueFrage.button1.Enabled = false;

            dlgNeueFrage.ShowDialog();
            if (dlgNeueFrage.DialogResult == DialogResult.OK)
            {
                appData.appFragen.Add(new Frage(dlgNeueFrage.textBox1.Text, 0));
                saveData();
                refreshLists();
            }
            else
            {

            }
        }

        private void saveData()
        {
            FileStream fs = new FileStream("udata.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, appData);
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NeuerFragebogen dlgNeuerFragebogen = new NeuerFragebogen();

            dlgNeuerFragebogen.textBox1.Text = "Bitte Namen eingeben...";
            dlgNeuerFragebogen.textBox1.Focus();

            dlgNeuerFragebogen.ShowDialog();
            if (dlgNeuerFragebogen.DialogResult == DialogResult.OK)
            {
                appData.appFrageboegen.Add(new Fragebogen(dlgNeuerFragebogen.textBox1.Text, new List<Frage>()));
                saveData();
                refreshLists();
            }
            else
            {
            }
        }
    }
}
