using System;
using System.Collections.Generic;
using System.Text;

namespace FileDiff
{
    //Inherits from TxtFile
    class SmartTxtFile : TxtFile
    {
        private string[] secondFileContents;

        //create a new reporter object to take notes on differences found
        private Reporter reporter = new Reporter();

        private int maxLineWordCount = 0;
        private string secondFileName = "";

        //when called, can compare the current smart file with another txt file
        public void CompareWith(TxtFile secondFile)
        {
            secondFileContents = secondFile.GetFileContents();
            secondFileName = secondFile.fileName;

            int lineNumber = 0;

            //Look ath which file has the most lines, and make that the maxLines variable
            int maxLines = 0;
            maxLines = fileContents.Length;
            if(secondFileContents.Length > maxLines)
            {
                maxLines = secondFileContents.Length;
            }

            string line = "";

            //iterates thorugh the length of file Contents
            for (int i = 0; i < maxLines; i++)
            {
                //If the line exists in the original file, return line, if not set it to empty
                //preventing out of bound errors
                if(i <= fileContents.Length)
                {
                    line = fileContents[i];
                }
                else
                {
                    line = "";
                }

                string lineToCompare = secondFileContents[lineNumber];

                //split
                List<string> fileContentsLineWords = new List<string>(line.Split(" "));
                List<string> lineToCompareWords = new List<string>(lineToCompare.Split(" "));

                //similar precedure to maxLines above, this time on word counts in a line
                maxLineWordCount = fileContentsLineWords.Count;
                if (lineToCompareWords.Count > maxLineWordCount)
                {
                    maxLineWordCount = lineToCompareWords.Count;
                }

                //Call the Checkline method, passing in the start of the line, current line number and the contents of both files
                CheckLine(0, 0, lineNumber, fileContentsLineWords, lineToCompareWords);

                //add a 'difference' into the list to mark a new line and increase line number before next loop
                reporter.addDifference(Environment.NewLine, 0, DifferenceType.Same, lineNumber);
                lineNumber++;
            }
        }

        private void CheckLine(int fileAWordIndex, int fileBWordIndex, int fileALineNumber, List<string> fileContentsLineWords, List<string> lineToCompareWords)
        {
            //create two new strings to hold the current word
            string fileAWord = "";
            string fileBWord = "";

            //create variables to hold the number of words in a line by getting the file count - 1 as its an array
            int numberOfWordsInALine = fileContentsLineWords.Count - 1;
            int numberOfWordsInBLine = lineToCompareWords.Count - 1;

            int nextFileAWordIndex = 0;
            int nextFileBWordIndex = 0;

            //First check to see that fileAWordIndex is in range
            if (fileAWordIndex <= numberOfWordsInALine)
            {
                //Same test for fileB
                if (fileBWordIndex <= numberOfWordsInBLine)
                { 
                    //get the current word
                    fileAWord = fileContentsLineWords[fileAWordIndex];
                    fileBWord = lineToCompareWords[fileBWordIndex];

                    if (fileAWord == fileBWord)
                    {
                        //MATCH
                        //report a new 'difference' to the list marking the word as the same
                        reporter.addDifference(fileAWord, fileAWordIndex, DifferenceType.Same, fileALineNumber );
                        
                        //increase index to search by 1 for both files
                        nextFileAWordIndex = fileAWordIndex + 1;
                        nextFileBWordIndex = fileBWordIndex + 1;

                        //call checkline once again
                        CheckLine(nextFileAWordIndex, nextFileBWordIndex, fileALineNumber, fileContentsLineWords, lineToCompareWords);

                    }
                    else
                    {
                        //NOT A MATCH
                        //log file number
                        reporter.addChangedLineNumber(fileALineNumber);

                        //check to see if added
                        //check that there is another word in lineB
                        if ((fileBWordIndex < numberOfWordsInBLine) && (fileAWord == lineToCompareWords[fileBWordIndex + 1]))
                        {
                            //report difference as added word
                            reporter.addDifference( fileBWord, fileAWordIndex, DifferenceType.Added, fileALineNumber );
                            
                            //icrease the search index by 1 for file B to see if that word also matches
                            nextFileAWordIndex = fileAWordIndex;
                            nextFileBWordIndex = fileBWordIndex + 1;
                        }
                        else if ((fileAWordIndex < numberOfWordsInALine)&&(fileContentsLineWords[fileAWordIndex + 1] == fileBWord))
                        {
                            //check to see if removed
                            //check that there is another word in lineA

                            //report difference as removed word
                            reporter.addDifference(fileAWord, fileAWordIndex, DifferenceType.Removed, fileALineNumber);

                            //icrease the search index by 1 for file A to see if that word also matches
                            nextFileAWordIndex = fileAWordIndex + 1;
                            nextFileBWordIndex = fileBWordIndex;
                        }
                        else
                        {
                            //if there are exactly the same amount of words in both lines, report difference as changed
                            reporter.addDifference(fileAWord + "-->" + fileBWord, fileAWordIndex, DifferenceType.Changed, fileALineNumber);
                            
                            //increase both indexs as normal
                            nextFileAWordIndex = fileAWordIndex + 1;
                            nextFileBWordIndex = fileBWordIndex + 1;
                        }
                        
                        //call Checkline again, with new updated indexs
                        CheckLine(nextFileAWordIndex, nextFileBWordIndex, fileALineNumber, fileContentsLineWords, lineToCompareWords);

                    }
                }
                else
                {
                    string fileAWordRestOfWords = "";

                    for(int i = fileAWordIndex; i <= numberOfWordsInALine; i++)
                    {
                        fileAWordRestOfWords = fileAWordRestOfWords + " " + fileContentsLineWords[fileAWordIndex];
                    }

                    //Handle out of range
                    //Add rest of words from LineA if there are any to report as removed words
                    reporter.addDifference(fileAWordRestOfWords, fileAWordIndex, DifferenceType.Removed, fileALineNumber );
                    reporter.addChangedLineNumber(fileALineNumber);
                }

            }
            else
            {
                string fileBWordRestOfWords = "";

                for (int i = fileBWordIndex; i <= numberOfWordsInBLine; i++)
                {
                    fileBWordRestOfWords = fileBWordRestOfWords + " " + lineToCompareWords[fileBWordIndex];
                }

                //Handle out of range
                //Add rest of words from LineB if there are any to report as added  words
                reporter.addDifference(fileBWordRestOfWords, fileAWordIndex, DifferenceType.Added, fileALineNumber);
            }
        }

        public void OutputResult()
        {
            //calls the two reporter methods to output all the results
            reporter.displayResultsToScreen();
            reporter.displayResultsToLogFile(fileName, secondFileName);
        }
    }
}
