using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
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

        /* ABOUT TraceListener:
         * TraceListener classes: used for logging and debugging purposes. 
           - System.Diagnostics namespace at the top
           Create an instance of the 'TraceListener' class to define how to output the tracing messages.
           The built-in Listener classes (TextWriterTraceListener) or custom listener.
           Trace.Listeners.Add() method is call once in an application.
           Trace.WriteLine("..."); to write messages at the right place in code.
           ***Trace.Close(); methods need to call at the end in the form closing method.
         */
        static Stream myTraceTestFile = File.Create("TestFile.txt");
        TextWriterTraceListener myTraceListener = new TextWriterTraceListener(myTraceTestFile);        

        //6.2 a global List<T> of type Information called Wiki.
        List<Information> Wiki = new List<Information>();
        string defaultFileName = "DefaultWiki.bin";
        //6.3 a button method to ADD a new item to the list (TextBox for the Name input),
        //ComboBox for the Category Radio group for the Structure, Multiline TextBox for the Definition.      
        #region Add
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            Trace.Listeners.Add(myTraceListener);
            //create object for adding new information.
            Information newInfo = new Information();           

            //ckeck null or empty in boxes
            if (string.IsNullOrEmpty(TextBoxName.Text))
            {
                toolStripStatusLabel.Text = "Please enter items in TextBoxes.";
            }            
            //add newName
            if (!ValidName(TextBoxName.Text))
            {
                toolStripStatusLabel.Text = "This name already exists in the data.";                
                return;
            }
            else
            {                
                newInfo.SetName(TextBoxName.Text);                
            }
            //add newSelectedCategory            
            newInfo.SetCategory(ComboBox.Text);
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
                return;
            }
            //add newDefinition            
            newInfo.SetDefinition(TextBoxDefitnition.Text);
            //add into WikiData
            Wiki.Add(newInfo);
            toolStripStatusLabel.Text = "Information added to Wiki Database";
            
            Trace.WriteLine(DateTime.Now.ToString() + " ButtonAdd Clicked.");
            ClearResetInput();
            DisplayListView();
            ButtonSave.Enabled = true;
            ButtonSearch.Enabled = true;
            ButtonEdit.Enabled = true;
            ButtonDelete.Enabled = true;
        }
        //Display in the ListView 
        private void DisplayListView()
        {
            ListView.Items.Clear();            
            SortAndDisplay();            
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
            string timeNow = DateTime.Now.ToString();
            Trace.TraceInformation(timeNow + " Wiki Application Form.");            

            FillComboBox();
            ButtonSave.Enabled = false;
            ButtonSearch.Enabled = false;
            ButtonEdit.Enabled = false;
            ButtonDelete.Enabled = false;
        }
        private void FillComboBox()
        {
            string[] categories = File.ReadAllLines("categories.txt");
            for(int x = 0; x < categories.Length; x++)
            {
                ComboBox.Items.Add(categories[x]);
            }                      
        }
        #endregion
        //6.5 a custom ValidName method which will take a parameter string value from the TextBox Name and return a Boolean 
        //after checking for a duplicates. Use the built in List<T> method "Exixts" to answer this requirement.
        #region ValidName method
        private bool ValidName(string name)
        {
            bool validName = Wiki.Exists(Information => Information.GetName() == name);
            Trace.Listeners.Add(myTraceListener);
            Trace.WriteLine(DateTime.Now.ToString() + "Method ValidName.");            
            if (validName)
            {
                Trace.WriteLine("This is false");
                return false;
            }
            return true;
        }
        #endregion
        //6.6 two methods to highligh and return the values from the Radio button GroupBox.
        //the first method must return a string value from the selected radio button (Linear or Non-Linear).
        #region RadioButton Highlights
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
        #endregion
        //6.7 a button method that will delete the currently selected record in the ListView.
        //Ensure the user has the option to backout of this action by using a dialog box.
        //Display on update version of the sorted list of the end of this process.
        #region Button Delete
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            Trace.Listeners.Add(myTraceListener);
            try
            {
                Trace.WriteLine(DateTime.Now.ToString() + "ButtonDelete Clicked.");
                if (ListView.SelectedItems.Count == 0)
                {
                    toolStripStatusLabel.Text = "Please select an item to delete.";
                    return;
                }
                if(ListView.SelectedItems.Count > 0)
                {
                    DialogResult = MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);                    
                    if (DialogResult == DialogResult.Yes)
                    {
                        //get the index of the first selected item
                        int selectedIndex = ListView.SelectedIndices[0];
                        //remove the selected items from the ListView
                        ListView.Items.RemoveAt(selectedIndex);
                        //remove the selected items (the same index: from corresponding Information object from the Wiki.)
                        Wiki.RemoveAt(selectedIndex);                        
                        toolStripStatusLabel.Text = "Deleted items in Wiki Database.";
                        Trace.WriteLine("Deleted items in Wiki Database");
                    }
                    else
                    {
                        Trace.WriteLine("Canceled the Deletioin");
                        toolStripStatusLabel.Text = "Canceled the Deletion.";
                    }
                }
                
                ClearResetInput();
                DisplayListView();
            }
            catch (Exception)
            {
                toolStripStatusLabel.Text = "Error occured while deleting the items.";
                //Trace.WriteLine("Error occured while deliting the items.");
            }
        }     
        #endregion
        //6.8 a button method that will save the edited record of the currently selected item in the ListView. 
        //All the changes in the input controls will be written back to the list.
        //Display an updated version of the sorted list at the end of this process.
        #region Button Edit
        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            Trace.Listeners.Add(myTraceListener);
            try
            {
                Information currentItem = new Information();
                currentItem.SetName(TextBoxName.Text);
                currentItem.SetStructure(GetStructureRadioButton());
                currentItem.SetCategory(ComboBox.Text);
                currentItem.SetDefinition(TextBoxDefitnition.Text);

                Trace.WriteLine(DateTime.Now.ToString() + "ButtonEdit clicked.");
                if (ListView.SelectedItems.Count == 0)
                {
                    toolStripStatusLabel.Text = "Please select an item to edit";
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

                    toolStripStatusLabel.Text = "Edited the information in Wiki Database.";
                    Trace.WriteLine("Edit the information in Database.");
                    DisplayListView();
                    ClearResetInput();
                }
            }
            catch (Exception)
            {
                toolStripStatusLabel.Text = "Error occured while editing the items";
                //Trace.WriteLine("Error occured while editing the items");
            }                    
        }
        #endregion
        //6.9 a single custom method that will sort and then display the Name and Category from the wiki information in the list.
        #region Sort and Display
        private void SortAndDisplay()
        {
            ListView.Items.Clear();
            Wiki.Sort();
            foreach (Information info in Wiki)
            {
                ListViewItem items = new ListViewItem(info.GetName());
                items.SubItems.Add(info.GetCategory());
                ListView.Items.Add(items);                
            }

        }
        #endregion
        //6.10 a button method that will use the built-in binary search to find a Data Structure name.
        //If the record is found the associated details will populate the appropriate input controls and highlight the name in the ListView.
        //At the end of the search process the search input TextBox must be cliear.
        #region Built in binary search
        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextBoxSearch.Text))
            {
                Information findName = new Information();
                findName.SetName(TextBoxSearch.Text);
                int found = Wiki.BinarySearch(findName);
                Trace.Listeners.Add(myTraceListener);
                Trace.WriteLine("Found" + found);
                if (found >= 0)
                {
                    ListView.SelectedItems.Clear();
                    ListView.Items[found].Selected = true;                    
                    ListView.Focus();
                    TextBoxName.Text = Wiki[found].GetName();
                    ComboBox.Text = Wiki[found].GetCategory();
                    SetStructureRadioButton(found);
                    TextBoxDefitnition.Text = Wiki[found].GetDefinition();                    
                    TextBoxSearch.Clear();
                    TextBoxSearch.Focus();
                    toolStripStatusLabel.Text = "Items found in Wiki Database";
                }
                else
                {
                    Trace.WriteLine("Items Not Found" + found);
                    toolStripStatusLabel.Text = "Cannot find the Items in Wiki Database";
                    TextBoxSearch.Clear();
                    TextBoxSearch.Focus();
                }
            }
            else
            {
                toolStripStatusLabel.Text = "Please enter item into SearchBox";
                TextBoxSearch.Clear();
                TextBoxSearch.Focus();
            }
        }
        #endregion
        //6.11 a ListView event so a user can select a Data Structure Name from the list of Names and the associated information will be displayed 
        //in the related text boxes combo box and radio button
        #region ListView Mouse Click
        private void ListView_MouseClick(object sender, MouseEventArgs e)
        {
            int selectedItem = ListView.SelectedIndices[0];
            TextBoxName.Text = Wiki[selectedItem].GetName();
            ComboBox.Text = Wiki[selectedItem].GetCategory();
            SetStructureRadioButton(selectedItem);
            TextBoxDefitnition.Text = Wiki[selectedItem].GetDefinition();

        }
        #endregion
        //6.12 a custom method that will clear and reset the TextBox, Combobox, and Radio button.
        #region Clear and Reset 
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
        #endregion
        //6.13 a double click event on the Name TextBox to clear the TextBoxes, ComboBox, and Radio button.
        #region Clear method (Mouse Double Click)
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

                toolStripStatusLabel.Text = "Items Cleared in all Textboxes.";
            }
        }
        #endregion
        //6.14 two buttons for the manual open and save option; this must use a dialog box to selet a file or rename saved file.
        //All Wiki data is stored/retrieved using a binary reader/writer file format
        #region Button Save and Open
        private void ButtonSave_Click(object sender, EventArgs e)
        {            
            string fileName = ".bin";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "Binary files (*.bin, *.dat)| *.bin; *.dat";
            saveFileDialog.Title = "Save your dat file";

            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
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
                toolStripStatusLabel.Text = "Wiki information saved";
            }                
            catch (IOException)
            {
                toolStripStatusLabel.Text = "Could not save Wiki information";                
            }
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {

            string fileName = ".bin";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "Binary files (*.bin, *.dat)| *.bin; *.dat";
            openFileDialog.Title = "Open a vinary file.";
            DialogResult result = openFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                toolStripStatusLabel.Text = "Open the file successfully";
            }
            else if (result == DialogResult.Cancel)
            {
                return;
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
                ButtonEdit.Enabled = true;
                ButtonDelete.Enabled = true;
                ButtonSave.Enabled = true;
                ButtonSearch.Enabled = true;                
            }
            catch (IOException)
            {
                toolStripStatusLabel.Text = "Could not open Wiki Information file";                
            }            
        }
        #endregion
        //6.15 The Wiki application will save data when the form closes
        //**When Form closing: Set the Saved file name as "default File Name" to differentiate from original one.
        #region Form Closing
        private void Wiki_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = MessageBox.Show("Do you want to save the file?","Confirm Save",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(DialogResult == DialogResult.Yes)
            {
                SaveFile(defaultFileName);
            }
            Trace.Close();
        }
        #endregion
    }
}
