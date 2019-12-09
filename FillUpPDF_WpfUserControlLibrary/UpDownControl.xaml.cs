using System.Windows;
using System.Windows.Controls;

namespace FillUpPDFUserControlLibrary
{
    /// <summary>
    /// Interaction logic for UpDownControl.xaml
    /// </summary>
    public partial class UpDownControl : UserControl
    {
        #region Custom Properties

        public int ValueMin
        {
            get { return (int)GetValue(ValueMinProperty); }
            set { SetValue(ValueMinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMinProperty =
            DependencyProperty.Register("ValueMin", typeof(int), typeof(UpDownControl), new PropertyMetadata(1));

        public int ValueMax
        {
            get { return (int)GetValue(ValueMaxProperty); }
            set { SetValue(ValueMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMaxProperty =
            DependencyProperty.Register("ValueMax", typeof(int), typeof(UpDownControl), new PropertyMetadata(10));

        #endregion Custom Properties

        #region Custom Events

        // Create a custom routed event by first registering a RoutedEventID
        // This event uses the bubbling routing strategy
        public static readonly RoutedEvent UpDownValueChangedEvent = EventManager.RegisterRoutedEvent(
            "UpDownValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UpDownControl));

        // Provide CLR accessors for the event
        public event RoutedEventHandler UpDownValueChanged
        {
            add { AddHandler(UpDownValueChangedEvent, value); }
            remove { RemoveHandler(UpDownValueChangedEvent, value); }
        }

        // This method raises the Tap event
        private void RaiseUpDownValueChangedEventEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(UpDownControl.UpDownValueChangedEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion Custom Events

        public int NumValue
        {
            get { return _numValue; }
            set
            {
                if (ValueMin <= value && value <= ValueMax)
                {
                    _numValue = value;
                    txtNum.Text = value.ToString();
                    RaiseUpDownValueChangedEventEvent();
                }
            }
        }

        private int _numValue = 1;

        public UpDownControl()
        {
            InitializeComponent();

            txtNum.Text = NumValue.ToString();
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            int temp = 0;
            if (!int.TryParse(txtNum.Text, out temp))
            {
                NumValue = temp;
            }
        }
    }
}