using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileDiff
{
    class TxtFile
    {
        //protected string array containg the contents of the file, protected variables can be acessed by child objects
        protected string[] fileContents;
        public string fileName = "";

        //Initialisation method checks the file exists before reading its contents into the file contents array
        public bool Initialise(string filePath)
        {
            bool filesExist = false;
            //becomes true when no errors are found to continue process

            fileName = filePath;

            if (File.Exists(filePath))
            {
                //read in first file
                fileContents = File.ReadAllLines(filePath);
                filesExist = true;
            }
            else
            {
                //Write error to user if file does not exist
                Console.WriteLine(filePath + " does not exist");
            }

            return filesExist;
            //returns whether or not the file exists, used for excpetion handling
        }

        //private method for returning the length of a file
        private int NumberOfLines()
        {
            return fileContents.Length;
        }

        //public method that returns a string array of the files contents
        public string[] GetFileContents()
        {
            return fileContents;
        }

        //Empty constructor to return null if no parameters given.
        public TxtFile()
        {
            fileContents = null;
        }
    }
}
