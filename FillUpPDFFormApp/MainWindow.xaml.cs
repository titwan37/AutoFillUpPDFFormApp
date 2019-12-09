using FillUpPDF_WpfUserControlLibrary;
using PostulationDatabankLibrary;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FillUpPDFFormApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        private int Lapstime = Settings1.Default.Lapstime;
        public PostulationReportClass _postulationReport = null;

        public string InitFolder { get; set; } = Settings1.Default.InitFolder; //@"C:\Users\titwa\Downloads";
        public string InitFile { get; set; } = Settings1.Default.InitFile; //"2019 RAV - Feuille de Suivi - 2019-Suivi.csv";

        public string FamilyName { get; set; } = Settings1.Default.Name;
        public string AVS { get; set; } = Settings1.Default.AVS;
        public string Period { get; set; }

        public int ReadEntriesCount { get; set; }
        public int SelectedEntriesCount { get; set; }
        public int SelectedEntriesPageCount { get; set; }

        public DateTime DisplayDateDay { get; set; } = FirstMonthsDay(DateTime.Now);
        public DateTime DisplayDateStart { get; set; } = FirstMonthsDay(DateTime.Now.AddDays(-365)); //= new DateTime(2019, 01, 01);
        public DateTime DisplayDateStop { get; set; } = DateTime.Now.AddDays(365);

        #endregion Properties

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            _postulationReport = PostulationReportClass.InitPostulationReport(FamilyName, AVS, Period);
            PostulationDatagridReader.Read(InitFolder, InitFile, _postulationReport);

            UpdateUI(_postulationReport);
            UpdatePostulationDisplay(_postulationReport);
        }

        private void UpdateUI(PostulationReportClass postulationReport)
        {
            ReadEntriesCount = postulationReport.LineCountFound;

            Box_DateStart.Text = postulationReport.DateStart.ToString("G");
            Box_DateStop.Text = postulationReport.DateStop.ToString("G");
            Box_Entries.Text = ReadEntriesCount.ToString();

            System.Diagnostics.Debug.WriteLine("System parsed a lince count=" + ReadEntriesCount);
        }

        #region DragNDrop

        private void Panel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Object"))
            {
                // These Effects values are used in the drag source's
                // GiveFeedback event handler to determine which cursor to display.
                if (e.KeyStates == DragDropKeyStates.ControlKey)
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        private void Panel_Drop(object sender, DragEventArgs e)
        {
            // If an element in the panel has already handled the drop,
            // the panel should not also handle it.
            if (e.Handled == false)
            {
                Panel _panel = (Panel)sender;
                UIElement _element = (UIElement)e.Data.GetData("Object");

                if (_panel != null && _element != null)
                {
                    // Get the panel that the element currently belongs to,
                    // then remove it from that panel and add it the Children of
                    // the panel that its been dropped on.
                    Panel _parent = (Panel)VisualTreeHelper.GetParent(_element);

                    if (_parent != null)
                    {
                        if (e.KeyStates == DragDropKeyStates.ControlKey &&
                            e.AllowedEffects.HasFlag(DragDropEffects.Copy))
                        {
                            CircleUC _circle = new CircleUC((CircleUC)_element);
                            _panel.Children.Add(_circle);
                            // set the value to return to the DoDragDrop call
                            e.Effects = DragDropEffects.Copy;
                        }
                        else if (e.AllowedEffects.HasFlag(DragDropEffects.Move))
                        {
                            _parent.Children.Remove(_element);
                            _panel.Children.Add(_element);
                            // set the value to return to the DoDragDrop call
                            e.Effects = DragDropEffects.Move;
                        }
                    }
                }
            }
        }

        #endregion DragNDrop

        private void UpdatePostulationDisplay(PostulationReportClass postulationReport)
        {
            if (postulationReport != null)
            {
                postulationReport.Period = this.Period;
                postulationReport.SelectByDate(FirstMonthsDay(datePicker.SelectedDate.GetValueOrDefault()));

                this.SelectedEntriesCount = postulationReport.SelectedEntriesCount;
                this.SelectedEntriesPageCount = postulationReport.SelectedEntriesPageCount;

                this.Box_SelectedEntries.Text = this.SelectedEntriesCount.ToString();
                this.Box_SelectedEntriesPages.Text = this.SelectedEntriesPageCount.ToString();
                this.UpDown_SelectedPage.ValueMax = this.SelectedEntriesPageCount;

                int selectedPage = UpDown_SelectedPage.NumValue;
                this.UpdateListView(postulationReport, selectedPage);
            }
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Box_Period.Text = this.Period = DisplayPeriodFormat(datePicker.SelectedDate);

            UpdatePostulationDisplay(_postulationReport);
        }

        private void UpDown_SelectedPage_UpDownValueChanged(object sender, RoutedEventArgs e)
        {
            int selectedPage = UpDown_SelectedPage.NumValue;
            this.UpdateListView(_postulationReport, selectedPage);
        }

        private void UpdateListView(PostulationReportClass postulationReport, int selectedPage)
        {
            if (_postulationReport != null || ListView_SelectedEntries != null)
            {
                if (1 <= selectedPage && selectedPage <= _postulationReport.SelectedEntriesPageCount)
                {
                    ListView_SelectedEntriesPerPage.ItemsSource = _postulationReport.SelectedEntriesByPages[selectedPage];
                    ListView_SelectedEntries.ItemsSource = _postulationReport.SelectedEntriesByMonth;
                }
                else
                {
                    ListView_SelectedEntriesPerPage.ItemsSource = null;
                    ListView_SelectedEntries.ItemsSource = _postulationReport.SelectedEntriesByMonth;
                }
            }
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.Sleep(new TimeSpan(0, 0, 7));

            if (_postulationReport != null || ListView_SelectedEntries != null)
            {
                int selectedPage = UpDown_SelectedPage.NumValue;
                if (1 <= selectedPage && selectedPage <= _postulationReport.SelectedEntriesPageCount)
                    _postulationReport.Report(_postulationReport.SelectedEntriesByPages[selectedPage], Lapstime);
                else
                    _postulationReport.Report(_postulationReport.SelectedEntriesByMonth, Lapstime);
            }
        }

        /// <summary>
        /// DisplayPeriodFormat  "November 2019"
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <returns>"November 2019"</returns>
        private string DisplayPeriodFormat(DateTime? selectedDate)
        {
            return string.Format("{0:Y}", selectedDate);
        }

        /// <summary>
        /// FirstMonthsDay
        /// </summary>
        /// <param name="date"></param>
        /// <returns>the first day of a month</returns>
        private static DateTime FirstMonthsDay(DateTime date)
        {
            return date.AddDays(+1 - date.Day);
        }
    }
}