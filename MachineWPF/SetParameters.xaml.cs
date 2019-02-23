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
using System.Windows.Shapes;

namespace MachineWPF
{
    /// <summary>
    /// Interaction logic for SetParameters.xaml
    /// </summary>
    public partial class SetParameters : Window
    {
        public bool IsClosed { get; set; } = false;
        public SetParameters()
        {
            Closed += SetParameters_Closed;
            InitializeComponent();
        }

        private void SetParameters_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if(From.Text != "")
            {
                MainWindow.param.MinX = Convert.ToDouble(From.Text);
            }
            if (To.Text != "")
            {
                MainWindow.param.MaxX =  Convert.ToDouble(To.Text);
            }
            if (Size.Text != "")
            {
                MainWindow.param.Size = Convert.ToInt16(Size.Text);
            }

            var xPoints = new double[MainWindow.param.Size];
            if(XPoints.Text != "")
            {
                var sxPoints =  XPoints.Text.Split(',');
                int i = 0;
                foreach (var point in sxPoints)
                {
                    xPoints[i] = Convert.ToDouble(point);
                    i++;
                }
            }

            if (LSN.Text != "")
            {
                MainWindow.param.Lsn = Convert.ToInt16(LSN.Text);
            }
            MainWindow.param.XValues = xPoints;
            Close();
        }
    }
}
