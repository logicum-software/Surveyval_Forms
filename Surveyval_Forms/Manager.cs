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

            // Initialize listViews nur 1 und 2, da für 3 Selection erforderlich
            listView1_refresh();
            listView2_refresh();
        }

        private void listView1_refresh()
        {
            foreach (ListViewItem item in listView1.Items)
                item.Remove();

            foreach (Fragebogen item in appData.appFrageboegen)
                listView1.Items.Add(new ListViewItem(item.strName));
        }

        private void listView2_refresh()
        {
            foreach (ListViewItem item in listView2.Items)
                item.Remove();

            foreach (Frage item in appData.appFragen)
                listView2.Items.Add(new ListViewItem(item.strFragetext));
        }

        private void listView3_refresh()
        {
            foreach (ListViewItem item in listView3.Items)
                item.Remove();

            if (appData.appFrageboegen[listView1.SelectedIndices[0]].Fragen.Count > 0)
            {
                foreach (Frage item in appData.appFrageboegen[listView1.SelectedIndices[0]].Fragen)
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
                foreach (Frage item in appData.appFragen)
                {
                    if (String.Compare(item.strFragetext, dlgNeueFrage.textBox1.Text, true) > -1 &&
                        String.Compare(item.strFragetext, dlgNeueFrage.textBox1.Text, true) < 1)
                    {
                        if (MessageBox.Show("Die eingegebene Frage: \n\n" + dlgNeueFrage.textBox1.Text +
                            "\n\nscheint schon zu existieren.\n\nTrotzdem speichern?",
                            "Frage bereits vorhanden", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }
                }

                if (dlgNeueFrage.radioButton2.Checked)
                    appData.appFragen.Add(new Frage(dlgNeueFrage.textBox1.Text, 1));
                else
                    appData.appFragen.Add(new Frage(dlgNeueFrage.textBox1.Text, 0));

                saveData();
                listView2_refresh();
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
                listView1_refresh();
            }
            else
            {
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count > 0)
            {
                appData.appFrageboegen[listView1.SelectedIndices[0]].Fragen.Add(appData.appFragen[listView2.SelectedIndices[0]]);
                MessageBox.Show("Die Frage wurde hinzugefügt", "Frage hinzugefügt", MessageBoxButtons.OK);
                saveData();
                listView3_refresh();
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                button2.Enabled = false;
            else
            {
                button2.Enabled = true;
                button4.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;

                listView3_refresh();
            }
        }

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView2.SelectedItems.Count < 1)
            {
                button4.Enabled = false;
                button7.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
                button2.Enabled = false;
                button7.Enabled = true;
                button8.Enabled = false;
            }
        }

        private void listView3_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView3.SelectedItems.Count < 1)
                button8.Enabled = false;
            else
            {
                button2.Enabled = false;
                button4.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                appData.appFrageboegen[listView1.SelectedIndices[0]].Fragen.RemoveAt(listView3.SelectedIndices[0]);
                MessageBox.Show("Die Frage wurde aus dem Fragebogen entfernt.", "Frage entfernt", MessageBoxButtons.OK);
                saveData();
                listView3_refresh();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Möchten Sie die Frage: \n\n" + "'" + listView2.SelectedItems[0].Text + "'" +
                "\n\nwirklich dauerhaft löschen?",
                "Frage löschen", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            else
            {
                appData.appFragen.RemoveAt(listView2.SelectedIndices[0]);
                saveData();
                listView2_refresh();
            }
            button4.Enabled = false;
        }
    }
}
