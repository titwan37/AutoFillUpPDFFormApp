using ActiveDeskRoboSet;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PostulationDatabankLibrary
{
    public class PostulationReportClass
    {
        public static PostulationReportClass InitPostulationReport(string name, string AVS, string period)
        {
            return new PostulationReportClass() { Name = name, AHVNb = AVS, Period = period, LineCountMax = 14 };
        }

        #region Properties

        public string Name { get; set; }
        public string AHVNb { get; set; }
        public string Period { get; set; }
        public int LineCountMax { get; set; } = 1; // divide per '0'
        public int LineCountFound { get; set; }

        public List<PostulationReportEntry> Entries { get; set; }
            = new List<PostulationReportEntry>();

        public DateTime DateStart { get; set; } = DateTime.Today;
        public DateTime DateStop { get; set; } = DateTime.MinValue;

        public List<PostulationReportEntry> SelectedEntriesByMonth { get; set; }
            = new List<PostulationReportEntry>();

        public Dictionary<int, List<PostulationReportEntry>> SelectedEntriesByPages { get; set; }
            = new Dictionary<int, List<PostulationReportEntry>>();

        public int SelectedEntriesCount { get; set; }
        public int SelectedEntriesPageCount { get; set; }

        #endregion Properties

        /// <summary>
        /// Calculate Date Start / Stop
        /// </summary>
        /// <param name="entry"></param>
        internal void CalculateDateStartStop(PostulationReportEntry entry)
        {
            try
            {
                if (DateTime.MinValue < entry.Date)
                    this.DateStart = (entry.Date < this.DateStart) ? entry.Date : this.DateStart;
                this.DateStop = (this.DateStop < entry.Date) ? entry.Date : this.DateStop;
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.ToString());
            }
        }

        /// <summary>
        /// CalculateReportPageCount
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        internal int CalculateReportPageCount(int records)
        {
            int recordsPerPage = LineCountMax;
            int pageCount = (records + (recordsPerPage - 1)) / recordsPerPage;
            return pageCount;
        }

        /// <summary>
        /// SelectByDate
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public List<PostulationReportEntry> SelectByDate(DateTime date)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            SelectedEntriesByMonth = new List<PostulationReportEntry>(
                    this.Entries.Where(p => DateBetween(p.Date, firstDayOfMonth, lastDayOfMonth)));

            SelectedEntriesCount = SelectedEntriesByMonth.Count;
            SelectedEntriesPageCount = CalculateReportPageCount(SelectedEntriesCount);

            // initialize the dictionary
            SelectedEntriesByPages = new Dictionary<int, List<PostulationReportEntry>>();
            for (int i = 1; i <= SelectedEntriesPageCount; i++)
            {
                SelectedEntriesByPages.Add(i, new List<PostulationReportEntry>());
            }
            // populate the dictionary
            for (int i = 1; i <= SelectedEntriesCount; i++)
            {
                int pageNumber = 1 + ((i - 1) / LineCountMax);
                var item = SelectedEntriesByMonth[i - 1];

                SelectedEntriesByPages[pageNumber].Add(item);
                //list.Add(item);
            }

            return SelectedEntriesByMonth;
        }

        internal static bool DateBetween(DateTime pDate, DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        {
            return (pDate >= firstDayOfMonth && lastDayOfMonth >= pDate);
        }

        public void Report(List<PostulationReportEntry> toBeReported, int lapstime)
        {
            // first Header report
            ActiveDeskRobot.ReportHeader(lapstime, this.Name, this.AHVNb, this.Period);

            // second line report
            toBeReported.ForEach(p => p.Report(lapstime));
        }
    }
}