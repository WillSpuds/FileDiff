using System;

namespace FileDiff
{

    //this is a console command
    //ensure you are in the same directory as the exe and the files you want to comapre. 
    //in this case; the netcoreapp3.1 folder within the project file
    //in the command promt type: filediff :and then the two file you want to compare. For example,
    //filediff 2a.txt 2b.txt


    class Program
    {
        static void Main(string[] args)
        {
            string fileAName = "";
            string fileBName = "";

            //error checking the users input on the command console
            if (args == null || args.Length != 2)
            {
                //if command is inputted incorrectly display error
                Console.WriteLine("Please give two file names as parameters to this command");
            }
            else
            {
                //first argument will be the first file, second will be the second file
                fileAName = args[0];
                fileBName = args[1];

                //make the first file a smarttxtfile
                SmartTxtFile fileA = new SmartTxtFile();

                if (fileA.Initialise(fileAName))
                {
                    //make the second just a normal txtfile
                    TxtFile fileB = new TxtFile();
                    if (fileB.Initialise(fileBName))
                    {
                        //if both desired files exist compare and output results
                        fileA.CompareWith(fileB);
                        fileA.OutputResult();
                    }
                }
            }
            
            

        }
    }
}