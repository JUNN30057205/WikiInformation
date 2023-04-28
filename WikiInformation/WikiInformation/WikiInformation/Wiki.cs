﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// ID: 30057205
// NAME: Jun Sumida
// Assessment 2 "Wiki Application" 
namespace WikiInformation
{
    public partial class WikiApplication : Form
    {
        public WikiApplication()
        {
            InitializeComponent();
        }
        //6.2 a global List<T> of type Information called Wiki.
        List<Information> Wiki = new List<Information>();

        //6.3 a button method to ADD a new item to the list (TextBox for the Name input),
        //ComboBox for the Category Radio group for the Structure, Multiline TextBox for the Definition.      
        #region Add
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            //create object for adding new information.
            Information newInfo = new Information();
            //add newName
            if (!ValidName(TextBoxName.Text))
            {
                MessageBox.Show("This name already exists in the data. \nPlease enter a different name.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                newInfo.SetName(TextBoxName.Text);
                //Information addName = new Information();
                //addName.SetName(TextBoxName.Text);
                //addName.SetCategory(ComboBox.SelectedItem.ToString());
                //addName.SetStructure(GetStructureRadioButton());
                //addName.SetDefinition(TextBoxDefitnition.Text);
                //WikiData.Add(addName);
            }
            //add newSelectedCategory
            newInfo.SetCategory(ComboBox.SelectedItem.ToString());
            //add newSelectedRadioButton
            if (RadioButtonLinear.Checked)
            {
                newInfo.SetStructure(RadioButtonLinear.Text);
            }
            else if (RadioButtonNonLinear.Checked)
            {
                newInfo.SetStructure(RadioButtonNonLinear.Text);
            }
            else
            {
                newInfo.SetStructure(default);
            }
            //add newDefinition
            newInfo.SetDefinition(TextBoxDefitnition.Text);
            //add into WikiData
            Wiki.Add(newInfo);            
            DisplayListView();
            ClearResetInput();
        }
        //Display in the ListView 
        private void DisplayListView()
        {
            ListView.Items.Clear();
            Wiki.Sort();
            foreach (var info in Wiki)
            {
                ListViewItem listView = new ListViewItem(info.GetName());
                listView.SubItems.Add(info.GetCategory());
                listView.SubItems.Add(info.GetStructure());
                listView.SubItems.Add(info.GetDefinition());
                ListView.Items.Add(listView);
            }
        }

        private string GetStructureRadioButton()
        {
            string rbValue = "";
            foreach(RadioButton radioButton in GroupBox.Controls.OfType<RadioButton>())
            {
                if (radioButton.Checked)
                {
                    rbValue = radioButton.Text;
                    break;
                }
                else
                {
                    rbValue = "other";
                }
            }
            return rbValue;        
        }
       
        private void SetStructureRadioButton(int item)
        {
            foreach(RadioButton radioButton in GroupBox.Controls.OfType<RadioButton>())
            {
                if(radioButton.Text == Wiki[item].GetStructure())
                {
                    radioButton.Checked = true;
                }
                else
                {
                    radioButton.Checked = false;
                }
            }
        }
        #endregion

        //6.4 ComboBox when the Foam Load method is called.
        //The six categories must be read from a simple text file ("categories.txt").
        #region Wiki_Load
        private void Wiki_Load(object sender, EventArgs e)
        {            
            FillComboBox();
        }
        private void FillComboBox()
        {
            string[] categories = File.ReadAllLines("categories.txt");
            ComboBox.Items.AddRange(categories);
            ComboBox.SelectedIndex = 0;            
        }
        #endregion

        //6.5 a custom ValidName method which will take a parameter string value from the TextBox Name and return a Boolean 
        //after checking for a duplicates. Use the built in List<T> method "Exixts" to answer this requirement.
        #region ValidName method
        private bool ValidName(string name)
        {
            bool validName = Wiki.Exists(Information => Information.GetName() == name);
            if (validName)
            {
                return false;
            }
            return true;
        }
        #endregion
        //6.6 two methods to highligh and return the values from the Radio button GroupBox.
        //the first method must return a string value from the selected radio button (Linear or Non-Linear).
        private string GetSelectedRadioButton()
        {
            if (RadioButtonLinear.Checked)
            {
                return "Linear";
            }
            else if (RadioButtonNonLinear.Checked)
            {
                return "Non-Linear";
            }
            else
            {
                return "";
            }
        }
        private void HighlightSelectedRadioButton()
        {
            if (RadioButtonLinear.Checked)
            {
                RadioButtonLinear.ForeColor = Color.OrangeRed;
                RadioButtonNonLinear.ForeColor = Color.Black;
            }
            else if (RadioButtonNonLinear.Checked)
            {
                RadioButtonNonLinear.ForeColor = Color.OrangeRed;
                RadioButtonLinear.ForeColor = Color.Black;
            }
            else
            {
                RadioButtonLinear.ForeColor = Color.Black;
                RadioButtonNonLinear.ForeColor = Color.Black;
            }
        }
        //the second method send on integer index which will highlight on an appropriate rado button.
        private void HighlightRadioButtonIndex(int index)
        {
            if(index == 0)
            {
                RadioButtonLinear.Checked = true;
                RadioButtonLinear.ForeColor = Color.OrangeRed;
                RadioButtonNonLinear.ForeColor = Color.Black;
            }
            else if (index == 1)
            {
                RadioButtonNonLinear.Checked = true;
                RadioButtonNonLinear.ForeColor= Color.OrangeRed;
                RadioButtonLinear.ForeColor= Color.Black;
            }
            else
            {
                RadioButtonLinear.ForeColor = Color.Black;
                RadioButtonNonLinear.ForeColor = Color.Black;
            }
        }
        //6.7 a button method that will delete the currently selected record in the ListView.
        //Ensure the user has the option to backout of this action by using a dialog box.
        //Display on update version of the sorted list of the end of this process.

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (ListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }
            DialogResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.Yes)
            {
                //get the selected item(current item)
                ListViewItem currentItem = ListView.SelectedItems[0];
                //remove current items from the listview
                ListView.Items.Remove(currentItem);
                foreach (Information info in Wiki)
                {
                    if(info.GetName() == currentItem.SubItems[0].Text)
                    {
                        Wiki.Remove(info);
                        break;
                    }
                }
            }
            ClearResetInput();
            DisplayListView();
        }

        //6.8 a button method that will save the edited record of the currently selected item in the ListView. 
        //All the changes in the input controls will be written back to the list.
        //Display an updated version of the sorted list at the end of this process.
        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            Information currentItem = new Information();
            currentItem.SetName(TextBoxName.Text);
            currentItem.SetStructure(GetStructureRadioButton());
            currentItem.SetCategory(ComboBox.Text);
            currentItem.SetDefinition(TextBoxDefitnition.Text);

            if(ListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an item to edit");
                return;
            }
            else
            {
                int index = ListView.SelectedIndices[0];
                //Update properites with new values (values get from set property)
                Wiki[index].SetName(currentItem.GetName());
                Wiki[index].SetCategory(currentItem.GetCategory());
                Wiki[index].SetStructure(GetStructureRadioButton());
                Wiki[index].SetDefinition(TextBoxDefitnition.Text);
                
                DisplayListView();
                ClearResetInput();
            }            
        }

        //6.9 a single custom method that will sort and then display the Name and Category from the wiki information in the list.
        private void SortAndDisplay()
        {
            Wiki.Sort();
            ListView.Items.Clear();
            foreach (Information info in Wiki)
            {
                ListViewItem items = new ListViewItem(info.GetName());
                items.SubItems.Add(info.GetCategory());
                ListView.Items.Add(items);                
            }

        }

        //6.10 a button method that will use the built-in binary search to find a Data Structure name.
        //If the record is found the associated details will populate the appropriate input controls and highlight the name in the ListView.
        //At the end of the search process the search input TextBox must be cliear.
        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxSearch.Text))
            {
                Information findName = new Information();
                findName.SetName(TextBoxSearch.Text);
                int found = Wiki.BinarySearch(findName);
                if (found >= 0)
                {
                    ListView.SelectedItems.Clear();
                    ListView.Items[found].Selected = true;
                    ListView.Focus();
                    TextBoxName.Text = Wiki[found].GetName();
                    ComboBox.Text = Wiki[found].GetCategory();
                    SetStructureRadioButton(found);
                    TextBoxDefitnition.Text = Wiki[found].GetDefinition();
                }
                else
                {
                    MessageBox.Show("Cannot find the Items");
                    TextBoxSearch.Clear();
                    TextBoxSearch.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please enter item into SearchBox");
                TextBoxSearch.Clear();
                TextBoxSearch.Focus();
            }
        }
        //6.11 a ListView event so a user can select a Data Structure Name from the list of Names and the associated information will be displayed 
        //in the related text boxes combo box and radio button
        private void ListView_MouseClick(object sender, MouseEventArgs e)
        {
            int selectedItem = ListView.SelectedIndices[0];
            TextBoxName.Text = Wiki[selectedItem].GetName();
            ComboBox.Text = Wiki[selectedItem].GetCategory();
            SetStructureRadioButton(selectedItem);
            TextBoxDefitnition.Text = Wiki[selectedItem].GetDefinition();

        }
        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(ListView.SelectedItems.Count > 0)
            //{
            //    int selectedItem = ListView.SelectedIndices[0];
            //    TextBoxName.Text = WikiData[selectedItem].GetName();
            //    ComboBox.Text= WikiData[selectedItem].GetCategory();
            //    SetStructureRadioButton(selectedItem);
            //    TextBoxDefitnition.Text = WikiData[selectedItem].GetDefinition(); 
            //}
        }
        //*** difference between IndexChange and MouseClick
        //    MouseClick event occour before selectedIndexChanged event 
        //    IndexChanged event handler can be respond to a change in selection 
        //    and do some processing on the selected item.


        //6.12 a custom method that will clear and reset the TextBox, Combobox, and Radio button.
        private void ClearResetInput()
        {
            TextBoxName.Clear();            
            ComboBox.SelectedIndex = -1;              
            foreach(RadioButton rb in GroupBox.Controls.OfType<RadioButton>())
            {
                rb.Checked = false;
            }
            TextBoxDefitnition.Clear();

        }

        //6.13 a double click event on the Name TextBox to clear the TextBoxes, ComboBox, and Radio button.
        private void TextBoxName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to clear the input?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                TextBoxName.Clear();
                ComboBox.SelectedIndex = -1;
                RadioButtonLinear.Checked = false;
                RadioButtonNonLinear.Checked = false;
                TextBoxDefitnition.Clear();
                TextBoxName.Focus();
            }           
        }
        //6.14 two buttons for the manual open and save option; this must use a dialog box to selet a file or rename saved file.
        //All Wiki data is stored/retrieved using a binary reader/writer file format
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            string fileName = "WikiInfromation.bin";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "Binary files (*.bin, *.dat)| *.bin; *.dat";
            saveFileDialog.Title = "Save your dat file";

            DialogResult result = saveFileDialog.ShowDialog();            
            if (result == DialogResult.Cancel)
            {
                fileName = saveFileDialog.FileName;
            }
            if(result == DialogResult.OK)
            {
                SaveFile(fileName);
            }
            
        }
        private void SaveFile(string fileName)
        {
            try
            {
                using (var stream = File.Open(fileName, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        foreach (var info in Wiki)
                        {
                            writer.Write(info.GetName());
                            writer.Write(info.GetCategory());
                            writer.Write(info.GetStructure());
                            writer.Write(info.GetDefinition());
                        }
                    }

                }

            }
            catch (IOException)
            {
                MessageBox.Show("Could not save Wiki information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            string fileName = "WikiInformation.bin";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "Binary files (*.bin, *.dat)| *.bin; *.dat";
            openFileDialog.Title = "Open a vinary file.";
            DialogResult result = openFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
            }
            try
            {
                Wiki.Clear();
                using(Stream stream = File.Open(fileName, FileMode.Open))
                {
                    using(var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        while(stream.Position < stream.Length)
                        {
                            Information info = new Information();
                            info.SetName(reader.ReadString());
                            info.SetCategory(reader.ReadString());
                            info.SetStructure(reader.ReadString());
                            info.SetDefinition(reader.ReadString());
                            Wiki.Add(info);
                        }
                    }
                }
                DisplayListView();
            }
            catch (IOException)
            {
                MessageBox.Show("Could not open Wiki Information","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        //6.15 The Wiki application will save data when the form closes
        private void Wiki_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFile("WikiInformation.bin");
        }
    }
}
