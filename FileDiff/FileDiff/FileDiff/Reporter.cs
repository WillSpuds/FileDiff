using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FileDiff
{
    class Reporter
    {
        //create a list that holds all lines with differences, to cross refference when printing
        private HashSet<int> linesWithDifferences = new HashSet<int>();

        //create a new list of difference objects
        private List<Difference> differencesList = new List<Difference>();

        public void addChangedLineNumber(int lineNumber)
        {
            linesWithDifferences.Add(lineNumber);
        }

        public void addDifference(string stringsnip_, int position_, DifferenceType diffType_, int lineNumber_)
        {
            //creates a new Difference object and adds it to the list, using the values reported in the SmartFile
            differencesList.Add(new Difference() { stringSnip = stringsnip_, position = position_, diffType = diffType_, lineNumber = lineNumber_ });
        }
        
        public void displayResultsToScreen()
        {
            bool testBool = true;

            //going through the difference list
            foreach (Difference difference in differencesList)
            {
                //checking if that item is also in a line with a difference
                if (linesWithDifferences.Contains(difference.lineNumber))
                {
                    if (testBool)
                    {
                        //write the line number only once per line
                        Console.WriteLine("");
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(" Line: " + difference.lineNumber + " ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        testBool = false;
                    }

                    //switch case using the diff type to get the colour correct
                    switch (difference.diffType)
                    {
                        case DifferenceType.Same:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        case DifferenceType.Added:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;

                        case DifferenceType.Removed:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;

                        case DifferenceType.Changed:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                    }
                    //write the word
                    Console.Write(difference.stringSnip + " ");
                }
                else
                {
                    testBool = true;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void displayResultsToLogFile(string fileAName, string fileBName)
        {
            //similar to the above display results to screen but for the log file
            //create a list to hold all the individual words
            List<string> allStrings = new List<string>();
            
            //create the file name by concatinating the two original file names
            string logFileName = Path.GetFileNameWithoutExtension(fileAName) + Path.GetFileNameWithoutExtension(fileBName) + ".log";

            string stringToWrite = "";
            int previousLineNumber = 0;
            
            allStrings.Add(" *** Comparrison Report *** ");
            allStrings.Add("File A: " + fileAName);
            allStrings.Add("File B: " + fileBName);
            allStrings.Add("********************");
            allStrings.Add(Environment.NewLine);

            //once again going through the difference list
            foreach (Difference difference in differencesList)
            {
                //start of a new line print the line number
                if(previousLineNumber != difference.lineNumber)
                {
                    allStrings.Add(" Line: " + (previousLineNumber + 1) + " ");

                    stringToWrite = stringToWrite.Trim();

                    //if the line is empty
                    if(stringToWrite.Length == 0 || stringToWrite == " ")
                    {
                        stringToWrite = "[ This line is blank ]";
                    }
                    allStrings.Add(stringToWrite);
                    stringToWrite = "";
                }
               
                //switch case, but instead of changing colour, a text prompt shows the reader what the difference is
                switch (difference.diffType)
                {
                    case DifferenceType.Same:
                        //stringToWrite = allStrings + "Word the same.";
                        stringToWrite = stringToWrite + difference.stringSnip + " ";
                        break;

                    case DifferenceType.Added:
                        if(difference.stringSnip.Length > 0)
                        {
                            stringToWrite = stringToWrite + "[ Added to B: " + difference.stringSnip + " ] ";
                        }
                        break;

                    case DifferenceType.Removed:
                        stringToWrite = stringToWrite + "[ Removed from A: " + difference.stringSnip + " ] "; 
                        break;

                    case DifferenceType.Changed:
                        stringToWrite = stringToWrite + "[ Changed: " + difference.stringSnip + " ] "; 
                        break;
                }

                if (previousLineNumber != difference.lineNumber)
                {
                    previousLineNumber = difference.lineNumber;
                }
            }

            allStrings.Add(" Line: " + (previousLineNumber + 1) + " ");

            stringToWrite = stringToWrite.Trim();
            if (stringToWrite.Length == 0 || stringToWrite == " ")
            {
                stringToWrite = "[ This line is blank ]";
            }
            allStrings.Add(stringToWrite);

            allStrings.Add(Environment.NewLine);
            allStrings.Add("********************");

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(logFileName))
                {
                    File.Delete(logFileName);
                }

                File.WriteAllLines(logFileName, allStrings.ToArray());

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
    }

        
}
