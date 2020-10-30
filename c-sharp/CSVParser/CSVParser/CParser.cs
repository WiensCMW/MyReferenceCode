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

            bool inQuotationMode = false;
            bool curCharIsQuote = false;
            bool curCharIsDelimiter = false;
            bool csvDataCurrupt = false;

            int quotationIndexStart = 0;
            int quotationIndexEnd = 0;

            // Loop over data and keep splitting by delimiter
            while (data.Length > 0 && !csvDataCurrupt)
            {
                // Reset parse variables
                quotationIndexStart = -1;
                quotationIndexEnd = -1;

                // Loop through data to find start and end indices for next value to parse
                for (int i = 0; i < data.Length; i++)
                {
                    // Check if current loop char is double quote
                    curCharIsQuote = data[i] == '"';
                    curCharIsDelimiter = data[i] == ',';

                    // If current char is double quote and it's the last char in the string, CSV data is currupt
                    if (curCharIsQuote && i == data.Length - 1)
                    {
                        csvDataCurrupt = true;
                        break;
                    }

                    if (!inQuotationMode)
                    {
                        // We're currently NOT in quotation mode, so check if we should be
                        if (curCharIsQuote)
                        {
                            // Switch to quotation mode
                            inQuotationMode = true;

                            // Set the quotation mode start index to next char index
                            quotationIndexStart = i + 1;

                            // We've found our start index, so break
                            break;
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        public static string CSVSampleData()
        {
            return $"One,Two";
        }
    }
}
