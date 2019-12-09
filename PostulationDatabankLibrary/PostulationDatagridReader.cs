using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace PostulationDatabankLibrary
{
    public class PostulationDatagridReader
    {
        /// <summary>
        /// ConvertToDate
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static DateTime ConvertToDate(string request)
        {
            DateTime res = DateTime.MinValue;
            if (!string.IsNullOrEmpty(request))
                DateTime.TryParse(request, out res);

            return res;
        }

        /// <summary>
        /// ConvertToBoolean
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static bool ConvertToBoolean(string request)
        {
            bool ret = false;

            if (string.IsNullOrEmpty(request))
            {
                return false;
            }
            else if (request == "1")
            {
                return true;
            }
            else
            {
                bool.TryParse(request, out ret);
                return ret;
            }
        }

        /// <summary>
        /// Fid Postulation Report entry
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="field"></param>
        private static void FidPostulationReportEntry(PostulationReportEntry entry, string[] field)
        {
            entry.Declared = ConvertToBoolean(field[0]);
            entry.Date = ConvertToDate(field[1]);
            entry.FirstContactPerson = Convert.ToString(field[2]);
            entry.CompagnyName = Convert.ToString(field[3]);
            entry.CompagnyAddress = Convert.ToString(field[4]);
            entry.WorkplaceLocation = Convert.ToString(field[5]);
            entry.PositionTitle = Convert.ToString(field[6]);
            entry.PositionDetails = Convert.ToString(field[7]);
            entry.PostulationTelephon = ConvertToBoolean(field[8]);
            entry.HadInterview = ConvertToBoolean(field[9]);
            entry.PostulationWritten = ConvertToBoolean(field[10]);
            entry.FullTime = ConvertToBoolean("true");
            entry.PartTime = ConvertToBoolean("false");
            entry.PositionAssigned = ConvertToBoolean(field[11]);
            entry.PostulationPerso = ConvertToBoolean(field[12]);
        }

        /// <summary>
        /// Read postulation report
        /// </summary>
        /// <param name="initFolder">folder</param>
        /// <param name="initFile">file</param>
        /// <param name="report">report</param>
        /// <returns></returns>
        public static PostulationReportClass Read(string initFolder, string initFile, PostulationReportClass report)
        {
            var path = Path.Combine(initFolder, initFile);
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                // Declared,Date,Contact,Compagny,Address,Workplace,Position,Project,Telephone,Interview,Written,Note,URL

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] field = csvParser.ReadFields();

                    var entry = new PostulationReportEntry();

                    try
                    {
                        FidPostulationReportEntry(entry, field);
                    }
                    catch (Exception exc)
                    {
                        System.Diagnostics.Debug.WriteLine(exc.ToString());
                    }
                    finally
                    {
                        if (entry.Valid)
                        {
                            report.Entries.Add(entry);
                            report.LineCountFound = report.Entries.Count;
                            report.CalculateDateStartStop(entry);
                        }
                    }
                }
                return report;
            }
        }
    }
}