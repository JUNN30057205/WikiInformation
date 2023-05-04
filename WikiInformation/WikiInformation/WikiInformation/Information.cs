using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WikiInformation
{
    /*6.1 a separate class file to hold the four data items of the Data Structure
      Use private properties for the fields which must be of type "string".
      The class file must have separate setters and getters, add an appropriate IComparable for the Name attribute.
      Save the file class as "Infromation.cs".
    */
    internal class Information: IComparable<Information>
    {
        const string Default_value = "--";
        //private properties for the fields.
        private string name;
        private string category;
        private string structure;
        private string definition;
        //separate setters and getters.
        public string GetName() { return name; }
        public string GetCategory() { return category; }
        public string GetStructure() { return structure; }
        public string GetDefinition() { return definition; }

        public void SetName(string newName) 
        { 
            if (string.IsNullOrEmpty(newName))
                name = Default_value;
            else
                name = newName; 
        }
        public void SetCategory(string newCategory)
        { 
            category = newCategory;
        }
        public void SetStructure(string newStructure)
        {
            structure = newStructure; 
        }
        public void SetDefinition(string newDefinition)
        { 
            definition = newDefinition; 
        }

        public int CompareTo(Information other)
        {
            return string.Compare(this.name, other.GetName(), false) ;
        }
    }   
      
}
