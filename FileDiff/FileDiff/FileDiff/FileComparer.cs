using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace FileDiff
{
    class FileComparer
    {
        //Holds string arrays of the files
        private string[] fileA;
        private string[] fileB;
        

        //Initialiser makes sure the files exist before comparing, not needed, but makes main code more readable and easier to debug.
        //it checks if both the files exist before perfomrming any actions on them to stop crashing and errors at run time.
        public bool Initialise(string fileAPath, string fileBPath)
        {
            bool filesExist = false;
            //becomes true when no errors are found to continue process


            if(File.Exists(fileAPath))
            {
                //read in first file
                fileA = File.ReadAllLines(fileAPath);
                
                if(File.Exists(fileBPath))
                {
                    //read in second file
                    fileB = File.ReadAllLines(fileBPath);
                    filesExist = true;
                }
                else
                {
                    //Write error to user
                    Console.WriteLine(fileBPath + "does not exist");
                }
            }
            else
            {
                //Write error to user
                Console.WriteLine(fileAPath + " does not exist");
            }

            return filesExist;    
        }


        public void Compare()
        {

            bool filesAreTheSame = true;

            int i = 0;

            //check iff both files are the same length, if they are not, they are not the same file
            if(fileA.Length == fileB.Length)
            {

                //as long as the files are comparing the same and the index is within the length of file a
                while (filesAreTheSame && i < fileA.Length)
                {
                    if (fileA[i] == fileB[i])
                    {

                    }
                    else
                    {
                        //if the element at the current index is not the same, it is not the same file
                        filesAreTheSame = false;
                    }

                    i++;
                }
            }
            else
            {
                filesAreTheSame = false;
            }
            

            //Display result
            if (filesAreTheSame)
            {
                Console.WriteLine("They are the same");
            }
            else
            {
                Console.WriteLine("They are different");
            }
        }


    }
}
