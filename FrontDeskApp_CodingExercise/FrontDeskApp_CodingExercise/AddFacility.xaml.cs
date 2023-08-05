using Db_FrontDeskApp.DataConfig;
using Db_FrontDeskApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddFacility.xaml
    /// </summary>
    public partial class AddFacility : Window
    {
        public AddFacility()
        {
            InitializeComponent();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            goBack();
        }

        private void btn_AddFacility_Click(object sender, RoutedEventArgs e)
        {
            string facilityName = txtFacilityName.Text;

            //DOES NOT ACCEPT EMPTY FACILITY NAME
            if(string.IsNullOrEmpty(facilityName))
            {
                MessageBox.Show("Empty Facility Name");
                return;
            }

            int smallCapacity = Convert.ToInt32(txtSmallStorage.Text);
            int mediumCapacity = Convert.ToInt32(txtMediumStorage.Text);
            int largeCapacity = Convert.ToInt32(txtLargeStorage.Text);

            //CHECKS IF INPUTED FACILITY ALREADY EXISTS
            try
            {
                using var context = new DbConfig();
                var fcl = context.Facilities
                    .Where(a => a.FacilityName.ToLower() == facilityName.ToLower())
                    .Count();
                if (fcl > 0)
                {
                    MessageBox.Show("Facility Already Exists");
                    txtFacilityName.Text = string.Empty;
                    txtFacilityName.Focus();

                    return;
                }
            }
            catch (Exception i)
            {
                MessageBox.Show($"Error: \n {i}");
            }

            //DISPLAY OVERALL NEW FACILITY INFO
            if (MessageBox.Show($"DO YOU WANT TO ADD THIS FACILITY?\n\n " +
                $"Facility Name: {facilityName}\n " +
                $"- CAPACITY-\n" +
                $"Small:  {smallCapacity} boxes\n" +
                $"Medium: {mediumCapacity} boxes\n" +
                $"Large:  {largeCapacity} boxes"
                , "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            //ADD FACILITY TO DATABASE
            try
            {
                using var context = new DbConfig();
                var fcl = new Facility()
                {
                    FacilityName = facilityName,
                    SmallCapacity = smallCapacity,
                    SmallRemainingCapacity = smallCapacity,
                    MediumCapacity = mediumCapacity,
                    MediumRemainingCapacity = mediumCapacity,
                    LargeCapacity = largeCapacity,
                    LargeRemainingCapacity = largeCapacity,
                };

                context.Add(fcl);
                context.SaveChanges();

                MessageBox.Show("Facility Successfully Added");
                goBack();
            }
            catch (Exception i)
            {
                MessageBox.Show($"Error: \n {i}");
            }
        }

        //-- HELPER METHOD --
        private void ValidateWholeNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CheckCapacity(int num, TextBox toAction) {
            if (num == 0) {
                MessageBox.Show("ERROR: \nValue must not be 0");
                toAction.Focus();
            }
        }

        private void goBack()
        {
            var newWindow = new SelectPackage();
            newWindow.Show();
            this.Close();
        }

        private void txtSmallStorage_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckCapacity(Convert.ToInt32(txtSmallStorage.Text), txtSmallStorage);
        }

        private void txtMediumStorage_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckCapacity(Convert.ToInt32(txtMediumStorage.Text), txtMediumStorage);
        }

        private void txtLargeStorage_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckCapacity(Convert.ToInt32(txtLargeStorage.Text), txtLargeStorage);
        }
    }
}
