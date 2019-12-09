using ActiveDeskRoboSet;
using System;

namespace PostulationDatabankLibrary
{
    public class PostulationReportEntry
    {
        #region Properties

        public DateTime Date { get; set; }
        public string FirstContactPerson { get; set; } = string.Empty;
        public string CompagnyName { get; set; } = string.Empty;
        public string CompagnyAddress { get; set; } = string.Empty;
        public string WorkplaceLocation { get; set; } = string.Empty;
        public string PositionTitle { get; set; } = string.Empty;
        public string PositionDetails { get; set; } = string.Empty;
        public bool PostulationPerso { get; set; }
        public bool PositionAssigned { get; set; }
        public bool FullTime { get; set; }
        public bool PartTime { get; set; }
        public bool PostulationWritten { get; set; }
        public bool PostulationTelephon { get; set; }

        public bool StillOpen { get; set; }
        public bool HadInterview { get; set; }
        public bool Hired { get; set; }
        public bool Fired { get; set; }

        public string HiringProcess_Notes { get; set; } = string.Empty;

        #endregion Properties

        public HiringProcessStatusEnum HiringProcess_Status { get; set; }
        public bool Declared { get; set; } = false;
        public bool Valid { get { return IsValid(); } }

        internal bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.PositionTitle);
        }

        public string FirmaAddressContact { get { return string.Format("{0},{1},{2}", CompagnyName, CompagnyAddress, FirstContactPerson); } }
        public string PositionTitleLocation { get { return string.Format("{0},{1},{2}", PositionTitle, WorkplaceLocation, PositionDetails); } }

        public override string ToString()
        {
            return string.Format("{0:d}|{1}|{2}", Date, CompagnyName, PositionTitle);
        }

        private string ToFullString()
        {
            return string.Join("/",
                this.Date, this.FirmaAddressContact, this.PositionTitleLocation,
                PositionAssigned, FullTime, PartTime, PostulationWritten, PostulationPerso, PostulationTelephon,
                StillOpen, HadInterview, Hired, Fired, HiringProcess_Notes
                );
        }

        public enum HiringProcessStatusEnum { Ongoing, Interview, Hired, Canceled }

        public static PostulationReportEntry Postulation
            = new PostulationReportEntry()
            {
                Date = new DateTime(2019, 01, 22)
                ,
                FirstContactPerson = ""
                ,
                CompagnyName = ""
                ,
                CompagnyAddress = ""
                ,
                WorkplaceLocation = ""
                ,
                PositionTitle = ""
                ,
                PositionDetails = ""
                ,
                PositionAssigned = false
                ,
                FullTime = true
                ,
                PartTime = false
                ,
                PostulationWritten = true
                ,
                PostulationPerso = false
                ,
                PostulationTelephon = false
                ,
                HadInterview = false
                ,
                StillOpen = true
                ,
                HiringProcess_Status = HiringProcessStatusEnum.Ongoing
                ,
                HiringProcess_Notes = ""
            };

        internal void Report(int Lapstime)
        {
            Console.WriteLine(this.ToFullString());
            System.Diagnostics.Debug.WriteLine(this.ToFullString());
            ActiveDeskRobot.ReportEntry(Lapstime, this.Date, this.FirmaAddressContact, this.PositionTitleLocation,
                PositionAssigned, FullTime, PartTime, PostulationWritten, PostulationPerso, PostulationTelephon,
                StillOpen, HadInterview, Hired, Fired, HiringProcess_Notes);
        }
    }
}