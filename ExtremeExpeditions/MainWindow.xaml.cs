using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExtremeExpeditions.Repositories;

namespace ExtremeExpeditions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExpeditionRepository db = new ExpeditionRepository();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                var peak = db.GetPeak(1);
                var peaks = db.GetPeaks();
                lstPeaks.ItemsSource = peaks;
                peak = peaks[0];
                peak.Elevation = 999;

                peak = peaks[1];
                peak.Elevation = null;

                db.UpdatePeaksWithTransaction(peaks);
                
                peak = new Peak
                {
                    PeakName = "Lillsjöhögen",
                    RangeId=1
                };

                peak = db.AddPeak(peak);

                */

                var result = db.DeletePeak(6);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
