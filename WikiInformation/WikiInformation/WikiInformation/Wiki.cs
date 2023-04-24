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

namespace WikiInformation
{
    public partial class Wiki : Form
    {
        public Wiki()
        {
            InitializeComponent();
        }
        //6.2 a global List<T> of type Information called Wiki.
        List<Information> WikiData = new List<Information>();

        //6.3 a button method to ADD a new item to the list (TextBox for the Name input),
        //ComboBox for the Category Radio group for the Structure, Multiline TextBox for the Definition.      
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
            WikiData.Add(newInfo);
            DisplayListView();
            ClearResetInput();
        }
        //Display in the ListView 
        private void DisplayListView()
        {
            ListView.Items.Clear();
            WikiData.Sort();
            foreach (var information in WikiData)
            {
                ListViewItem listView = new ListViewItem(information.GetName());
                listView.SubItems.Add(information.GetCategory());
                //listView.SubItems.Add(information.GetStructure());
                //listView.SubItems.Add(information.GetDefinition());
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
                if(radioButton.Text == WikiData[item].GetStructure())
                {
                    radioButton.Checked = true;
                }
                else
                {
                    radioButton.Checked = false;
                }
            }
        }

        //6.4 ComboBox when the Foam Load method is called.
        //The six categories must be read from a simple text file.
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

        //6.5 a custom ValidName method which will take a parameter string value from the TextBox Name and return a Boolean 
        //after checking for a duplicates. Use the built in List<T> method "Exixts" to answer this requirement.
        private bool ValidName(string name)
        {
            bool validName = WikiData.Exists(Information => Information.GetName() == name);
            if (validName)
            {
                return false;
            }
            return true;
        }
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
                foreach (ListViewItem item in ListView.SelectedItems)
                {


                }
            }
            DisplayListView();

        }

        //6.8 a button method that will save the edited record of the currently selected item inthe ListView. 
        //All the changes in the input controls will be written back to the list.
        //Display an updated version of the sorted list at the endo of this process.

        private void ButtonEdit_Click(object sender, EventArgs e)
        {

        }

        //6.9 a single custom method that will sort and then display the Name and Category from the wiki information in the list.
        private void SortAndDisplay()
        {
            WikiData.Sort();
            ListView.Items.Clear();
            foreach (Information information in WikiData)
            {
                ListViewItem item = new ListViewItem(new String[]
                   {information.GetName(), information.GetCategory()});
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
                int found = WikiData.BinarySearch(findName);
                if (found >= 0)
                {
                    ListView.SelectedItems.Clear();
                    ListView.Items[found].Selected = true;
                    ListView.Focus();
                    TextBoxName.Text = WikiData[found].GetName();
                    ComboBox.Text = WikiData[found].GetCategory();
                    SetStructureRadioButton(found);
                    TextBoxDefitnition.Text = WikiData[found].GetDefinition();
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
        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ListView.SelectedItems.Count > 0)
            {
                               
                

            }
        }



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

        //6.15 The Wiki application will save data when the form closes

    }
}
