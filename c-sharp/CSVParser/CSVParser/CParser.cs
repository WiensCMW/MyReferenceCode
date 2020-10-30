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
            string[] csvStringArrayData = File.ReadAllLines(@"CSV Data.csv");
            string csvData = string.Join(",", csvStringArrayData);

            // List that will hold the split data
            List<string> splitData = new List<string>();

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
            while (csvData.Length > 0 && !csvDataCurrupt)
            {
                // Reset parse variables
                inDoubleQuotationMode = false;
                int charIndex = 0;
                valueEndIndex = -1;

                #region Loop through data to find start and end incides for next value to parse
                while (true)
                {
                    #region Reset loop variables
                    nextCharIsQuote = false;
                    nextCharIsDelimiter = false;
                    nextCharIsDelimiterAfterQuote = false;
                    #endregion

                    #region Evalutate current loop char
                    lastChar = charIndex == csvData.Length - 1;
                    curCharIsQuote = csvData.Substring(charIndex, 1) == "\"";
                    curCharIsDelimiter = csvData.Substring(charIndex, 1) == ",";
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
                        nextCharIsQuote = csvData.Substring(charIndex + 1, 1) == "\"";
                        nextCharIsDelimiter = csvData.Substring(charIndex + 1, 1) == ",";
                    }

                    // If we have at least two more chars left, get the char after next
                    if ((csvData.Length - 1) - charIndex >= 2)
                    {
                        nextCharIsDelimiterAfterQuote = csvData.Substring(charIndex + 2, 1) == ",";
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
                if (valueEndIndex >= 0 && valueEndIndex <= csvData.Length - 1)
                {
                    // Extract value from data based on calculated end index
                    extractedValue = csvData.Substring(0, valueEndIndex + 1);

                    // Remove extracted data from data string
                    if (extractedValue.Length == csvData.Length)
                    {
                        csvData = "";
                    }
                    else
                    {
                        int start = extractedValue.Length;
                        int length = csvData.Length - extractedValue.Length;

                        csvData = csvData.Substring(start, length);
                    }
                }

                #region Remove trailing delimiter from extracted value
                if (extractedValue.Length > 1)
                {
                    if (extractedValue.Substring(extractedValue.Length - 1, 1) == ",")
                    {
                        extractedValue = extractedValue.Substring(0, extractedValue.Length - 1);
                    }
                }
                #endregion

                #region Remove encasing double quotes
                if (extractedValue.Length >= 3)
                {
                    if (extractedValue.Substring(0, 1) == "\""
                        && extractedValue.Substring(extractedValue.Length - 1, 1) == "\"")
                    {
                        // Remove double quote from start of extracted
                        extractedValue = extractedValue.Substring(1, extractedValue.Length - 1);

                        // Remove double quote from end of extracted
                        extractedValue = extractedValue.Substring(0, extractedValue.Length - 1);
                    }
                }
                #endregion

                #region Remove extra escaped double quotes
                if (extractedValue.Length >= 1)
                {
                    charIndex = 0;
                    while (true)
                    {
                        #region Evalutate current and next loop chars
                        lastChar = charIndex == extractedValue.Length - 1;
                        curCharIsQuote = extractedValue.Substring(charIndex, 1) == "\"";
                        nextCharIsQuote = false;

                        if (!lastChar)
                        {
                            nextCharIsQuote = extractedValue.Substring(charIndex + 1, 1) == "\"";
                        }
                        #endregion

                        // If current and next chars are both double quotes, get rid of one
                        if (curCharIsQuote && nextCharIsQuote)
                        {
                            // Remove current loop char
                            extractedValue = extractedValue.Remove(charIndex, 1);

                            // Recheck if we're on he last char after removing a char
                            lastChar = charIndex == extractedValue.Length - 1;
                        }

                        if (lastChar)
                            break;
                        else
                            charIndex++;
                    }
                }
                #endregion

                // Add extracted value to results
                splitData.Add(extractedValue);
                #endregion
            }

            // Ptrint split values to console
            for (int i = 0; i < splitData.Count; i++)
            {
                Console.WriteLine(splitData[i]);
            }
        }
    }
}
