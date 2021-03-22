using System;
using System.Collections.Generic;
using System.Text;

namespace FileDiff
{
    //create an enumerator, to help convert the detected change to the corresponding colour
    public enum DifferenceType
    {
        Same = 0,
        Added = 1,
        Removed = 2,
        Changed = 3
    }


    //set up the parameters of a difference object, all values required to create full report
    public class Difference
    {
        public string stringSnip { get; set; }
        public int position { get; set; }
        public DifferenceType diffType { get; set; }
        public int lineNumber { get; set; }
    }
}
