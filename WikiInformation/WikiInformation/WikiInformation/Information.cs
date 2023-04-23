using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WikiInformation
{
    internal class Information: IComparable<Information>
    {
        const string Default_value = "--";
        //private properties for the fields.
        private string name;
        private string category;
        private string structure;
        private string definition;

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
        public void SetCategory(string newCategory) { category = newCategory; }
        public void SetStructure(string newStructure) { structure = newStructure; }
        public void SetDefinition(string newDefinition) { definition = newDefinition; }

        public int CompareTo(Information other)
        {
            return string.Compare(this.name, other.GetName(), false) ;
        }
    }   
      
}
