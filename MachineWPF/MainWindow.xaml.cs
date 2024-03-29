﻿using MachineWPF.Practice2;
using MachineWPF.Practice3.Views;
using MachineWPF.Practice4.Views;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SetParameters setParametersWindow;
        public static Parameters param;

        public MainWindow()
        {
            param = new Parameters();
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Plot plot = new Plot(1);

            plot.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Plot plot = new Plot(3);

            plot.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if ((setParametersWindow == null) || (setParametersWindow.IsClosed))
            {
                setParametersWindow = new SetParameters();
                setParametersWindow.Show();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Plot plot = new Plot(2);

            plot.Show();
        }


        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            PlotPractice2 plot = new PlotPractice2(6);

            plot.Show();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            PlotPractice3 plot = new PlotPractice3(7);

            plot.Show();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            PlotPractice3 plot = new PlotPractice3(8);

            plot.Show();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            PlotPractice4 plot = new PlotPractice4(9);

            plot.Show();
        }
    }
}
