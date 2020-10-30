using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace CSVParser
{
    public static class CParser
    {
        public static void CSVParse()
        {
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"CSV Data.csv");
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"CSV Data.csv");
            string[] csvData = File.ReadAllLines(path);

            string data = ""; //SVSampleData();
            data = string.Join(",", csvData);
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
            bool nextCharIsDelimiterAfterQuote = false;
            bool lastChar = false;

            // Parse indices
            int valueEndIndex = 0;

            // Loop over data and keep splitting by delimiter
            while (data.Length > 0 && !csvDataCurrupt)
            {
                // Reset parse variables
                inDoubleQuotationMode = false;
                int charIndex = 0;
                valueEndIndex = -1;
                droppedEscapes.Clear();

                #region Loop through data to find start and end incides for next value to parse
                while (true)
                {
                    #region Reset loop variables
                    nextCharIsQuote = false;
                    nextCharIsDelimiter = false;
                    nextCharIsDelimiterAfterQuote = false;
                    #endregion

                    #region Evalutate current loop char
                    lastChar = charIndex == data.Length - 1;
                    curCharIsQuote = data.Substring(charIndex, 1) == "\"";
                    curCharIsDelimiter = data.Substring(charIndex, 1) == ",";
                    #endregion

                    #region Check for curruption
                    // If we're not in quotation mode and the last char is a double quote, file is corrupt
                    if (!inDoubleQuotationMode && curCharIsQuote && lastChar)
                    {
                        csvDataCurrupt = true;
                        break;
                    }
                    #endregion

                    #region Evalute next chars
                    // If not at last char, get next char
                    if (!lastChar)
                    {
                        nextCharIsQuote = data.Substring(charIndex + 1, 1) == "\"";
                        nextCharIsDelimiter = data.Substring(charIndex + 1, 1) == ",";
                    }

                    // If we have at least two more chars left, get the char after next
                    if ((data.Length - 1) - charIndex >= 2)
                    {
                        nextCharIsDelimiterAfterQuote = data.Substring(charIndex + 2, 1) == ",";
                    }
                    #endregion

                    #region Switch quotation mode state machine
                    if (!inDoubleQuotationMode)
                    {
                        // Check if current char is double quote, if so switch to double quotation mode
                        if (curCharIsQuote)
                        {
                            // Switch to double quotation mode
                            inDoubleQuotationMode = true;
                        }
                        // Check if current char is our delimiter
                        else if (curCharIsDelimiter)
                        {
                            // Delimiter found while NOT in quote mode, so record delimiter as end index of current value
                            valueEndIndex = charIndex;

                            // Break loop so we can extract our current value
                            break;
                        }
                    }
                    else
                    {
                        // Check if next char is double quote, and one after that is delimiter
                        if (nextCharIsQuote && nextCharIsDelimiterAfterQuote)
                        {
                            // Delimiter found while in quote mode 2 chars after current char. Record that char
                            // as end index for current value
                            valueEndIndex = charIndex + 2;

                            // Break loop so we can extract our current value
                            break;
                        }
                    }
                    #endregion

                    // Break if we're on the last char, else increase it by one
                    if (lastChar)
                    {
                        valueEndIndex = charIndex;
                        break;
                    }
                    else
                        charIndex++;
                }
                #endregion

                #region Extract substring from data based on end index found in preceeding loop
                string extractedValue = "";
                if (valueEndIndex >= 0 && valueEndIndex <= data.Length - 1)
                {
                    // Extract value from data based on calculated end index
                    extractedValue = data.Substring(0, valueEndIndex + 1);

                    // Remove extracted data from data string
                    if (extractedValue.Length == data.Length)
                    {
                        data = "";
                    }
                    else
                    {
                        int start = extractedValue.Length;
                        int length = data.Length - extractedValue.Length;

                        data = data.Substring(start, length);
                    }
                }

                // Remove trailing delimiter
                if (extractedValue.Length > 1)
                {
                    if (extractedValue.Substring(extractedValue.Length - 1, 1) == ",")
                    {
                        extractedValue = extractedValue.Substring(0, extractedValue.Length - 1);
                    }
                }

                // Add extracted value to results
                splitData.Add(extractedValue);
                #endregion
            }

            // Pring split values to console
            for (int i = 0; i < splitData.Count; i++)
            {
                Console.WriteLine(splitData[i]);
            }
        }

        public static string CSVSampleData()
        {
            return $"One,Two";
        }
    }
}
