using System;
using System.Collections.Generic;
using System.Threading;

namespace CSVParser
{
    public static class CParser
    {
        public static void CSVParse()
        {
            string data = CSVSampleData();
            //var test = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // List that will hold the split data
            List<string> splitData = new List<string>();

            // List that will hold the incides of the dropped escaped chars while parsing
            List<int> droppedEscapes = new List<int>();

            // Parse bools
            bool csvDataCurrupt = false;
            bool inDoubleQuotationMode = false;
            bool curCharIsQuote = false;
            bool curCharIsDelimiter = false;
            bool nextCharIsQuote = false;
            bool nextCharIsDelimiter = false;
            bool lastChar = false;

            // Parse indices
            int valueStartIndex = 0;
            int valueEndIndex = 0;

            // Loop over data and keep splitting by delimiter
            while (data.Length > 0 && !csvDataCurrupt)
            {
                // Reset parse variables
                inDoubleQuotationMode = false;
                valueStartIndex = -1;
                valueEndIndex = -1;
                droppedEscapes.Clear();

                // Loop through data to find start and end indices for next value to parse
                for (int i = 0; i < data.Length; i++)
                {
                    #region Reset loop variables
                    nextCharIsQuote = false;
                    nextCharIsDelimiter = false;
                    #endregion

                    #region Evalutate current loop char
                    curCharIsQuote = data[i] == '"';
                    curCharIsDelimiter = data[i] == ',';
                    lastChar = i == data.Length - 1;
                    #endregion

                    #region Check for curruption
                    // If current char is double quote and it's the last char in the string, CSV data is currupt
                    if (curCharIsQuote && lastChar)
                    {
                        csvDataCurrupt = true;
                        break;
                    }
                    #endregion

                    #region Evalute next char
                    if (!lastChar)
                    {
                        nextCharIsQuote = data[i + 1] == '"';
                        nextCharIsDelimiter = data[i + 1] == ',';
                    }
                    #endregion

                    #region Switch quotation mode state machine
                    if (!inDoubleQuotationMode)
                    {
                        // Check if current char is double quote, if so switch to double quotation mode
                        if (curCharIsQuote)
                        {
                            inDoubleQuotationMode = true;

                            // Capture next char as our first

                            // Record current index as a dropped escape char
                            droppedEscapes.Add(i);
                        }
                        // Check if current char is our delimiter
                        else if (curCharIsDelimiter)
                        {

                        }
                    }
                    #endregion
                }
            }
        }

        public static string CSVSampleData()
        {
            return $"One,Two";
        }
    }
}
