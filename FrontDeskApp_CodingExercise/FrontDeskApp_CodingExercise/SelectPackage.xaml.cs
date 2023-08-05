using Db_FrontDeskApp.DataConfig;
using Db_FrontDeskApp.Model;
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

namespace FrontDeskApp_CodingExercise
{
    /// <summary>
    /// Interaction logic for SelectPackage.xaml
    /// </summary>
    public partial class SelectPackage : Window
    {
        private List<Facility> facilitiesList = new List<Facility>();
        public SelectPackage()
        {
            InitializeComponent();

            FillFacilityList();
            FillFacilityListBox();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        private void btn_AddFacility_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new AddFacility();
            newWindow.Show();
            this.Close();
        }

        private void FillFacilityList() 
        {
            try {
                using var context = new DbConfig();
                var fcl = context.Facilities.ToList();
                facilitiesList.AddRange(fcl);

            } catch (Exception ex) {
                MessageBox.Show($"Error:\n{ex}");
            }
        }

        private void FillFacilityListBox() 
        {
            lstBox_Facilities.Items.Clear();

            if (facilitiesList.Count == 0) grid_FacilityDetails.IsEnabled = false;
            foreach (var facility in facilitiesList)
            {
                lstBox_Facilities.Items.Add(facility.FacilityName);
            }

            lstBox_Facilities.SelectedIndex = 0;
        }

        private void lstBox_Facilities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int itemIndex = lstBox_Facilities.SelectedIndex;

            lblFacilityName.Content = facilitiesList[itemIndex].FacilityName;
            lblSmall.Content = facilitiesList[itemIndex].SmallRemainingCapacity;
            lblMedium.Content = facilitiesList[itemIndex].MediumRemainingCapacity;
            lblLarge.Content = facilitiesList[itemIndex].LargeRemainingCapacity;
        }

        private void btn_Small_Click(object sender, RoutedEventArgs e)
        {
            int itemIndex = lstBox_Facilities.SelectedIndex;
            if (facilitiesList[itemIndex].SmallRemainingCapacity > 0) AddPackageToStore("Small");
            else MessageBox.Show($"{facilitiesList[itemIndex].FacilityName} Small Storage is now Full, kindly select another Storage.");
        }

        private void btn_Medium_Click(object sender, RoutedEventArgs e)
        {
            int itemIndex = lstBox_Facilities.SelectedIndex;
            if (facilitiesList[itemIndex].MediumRemainingCapacity > 0) AddPackageToStore("Medium");
            else MessageBox.Show($"{facilitiesList[itemIndex].FacilityName} Medium Storage is now Full, kindly select another Storage.");
        }

        private void btn_Large_Click(object sender, RoutedEventArgs e)
        {
            int itemIndex = lstBox_Facilities.SelectedIndex;
            if (facilitiesList[itemIndex].LargeRemainingCapacity > 0) AddPackageToStore("Large");
            else MessageBox.Show($"{facilitiesList[itemIndex].FacilityName} Large Storage is now Full, kindly select another Storage.");
        }

        //-- HELPER METHODS --
        public void AddPackageToStore(string packageSize) 
        {
            var newWindow = new AddPackage(facilitiesList[lstBox_Facilities.SelectedIndex].FacilityName, packageSize);
            newWindow.Show();
            this.Close();
        }
    }
}
