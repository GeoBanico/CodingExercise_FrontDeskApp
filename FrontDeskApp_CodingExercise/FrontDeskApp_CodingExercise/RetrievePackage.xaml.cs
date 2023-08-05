using Db_FrontDeskApp.DataConfig;
using Db_FrontDeskApp.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
    /// Interaction logic for RetrievePackage.xaml
    /// </summary>
    public partial class RetrievePackage : Window
    {
        List<StoredPackage> packageList;
        List<Customer> customerList;
        List<Facility> facilityList;

        public RetrievePackage()
        {
            InitializeComponent();

            FillPackageList("");
            if (packageList.Count == 0) {
                btn_RetrievePackage.Visibility = Visibility.Hidden;
                txtSearchFacility.Text = "No items to retrieve";
                txtSearchFacility.IsEnabled = false;
            }
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            goBack();
        }

        private void txtSearchFacility_SelectionChanged(object sender, RoutedEventArgs e)
        {
            FillPackageList(txtSearchFacility.Text);
        }

        private void btn_RetrievePackage_Click(object sender, RoutedEventArgs e)
        {
            int itemIndex = lstbox_Customer.SelectedIndex;

            var facilityDetails = facilityList.First(a => a.FacilityId == packageList[itemIndex].FacilityId);
            var customerDetails = customerList.First(a => a.CustomerId == packageList[itemIndex].CustomerId);

            //CONFIRMATION TO RETRIEVE PACKAGE
            if (MessageBox.Show($"DO YOU WANT TO RETRIEVE THIS PACKAGE FROM THE FACILITY?\n\n " +
                $"- PACKAGE INFORMATION -\n" +
                $"Facility Name: {facilityDetails.FacilityName}\n " +
                $"Package Size:  {packageList[itemIndex].PackageSize}\n\n" +
                $"- CUSTOMER INFORMATION -\n" +
                $"Name:  {customerDetails.FirstName} {customerDetails.LastName}\n" +
                $"Phone: {customerDetails.Phone}\n"
                , "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            //ADD FACILITY TO DATABASE
            try
            {
                using var context = new DbConfig();
                var updatePackage= context.StoredPackages.First(a => a.StoredPackageId == packageList[itemIndex].StoredPackageId);

                updatePackage.RetrievedDate = DateAndTime.Now;
                updatePackage.RetrievedPackage = true;

                var updateFacility = context.Facilities.First(a => a.FacilityId == facilityDetails.FacilityId);

                if (updatePackage.PackageSize == PackageSize.Small) updateFacility.SmallRemainingCapacity++;
                else if (updatePackage.PackageSize == PackageSize.Medium) updateFacility.MediumRemainingCapacity++;
                else if (updatePackage.PackageSize == PackageSize.Large) updateFacility.LargeRemainingCapacity++;

                context.Update(updatePackage);
                context.Update(updateFacility);
                context.SaveChanges();

                MessageBox.Show("PACKAGE Successfully Retrieved");
                goBack();
            }
            catch (Exception i)
            {
                MessageBox.Show($"Error: \n {i}");
            }
        }

        private void lstbox_Customer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int itemIndex = lstbox_Customer.SelectedIndex;
            if (itemIndex == -1) return;

            var facilityDetails = facilityList.First(a => a.FacilityId == packageList[itemIndex].FacilityId);
            var customerDetails = customerList.First(a => a.CustomerId == packageList[itemIndex].CustomerId);

            lblFacility.Content = facilityDetails.FacilityName;
            lblPackageType.Content = packageList[itemIndex].PackageSize.ToString();
            lblFirstName.Content = customerDetails.FirstName;
            lblLastName.Content = customerDetails.LastName;
            lblPhone.Content = customerDetails.Phone;
        }

        //-- HELPER METHODS --
        private void FillPackageList(string name) {
            packageList = new List<StoredPackage>();
            customerList = new List<Customer>();
            facilityList = new List<Facility>();

            using var context = new DbConfig();
            packageList.AddRange(context.StoredPackages.Where(a => a.RetrievedPackage == false).ToList());
            customerList.AddRange(context.Customers.ToList());
            facilityList.AddRange(context.Facilities.Where(a => a.FacilityName.ToLower().Contains(name.ToLower())).ToList());

            lstbox_Customer.Items.Clear();
            if (facilityList.Count == 0) return;

            foreach (StoredPackage package in packageList)
            {
                var customerDetails = customerList.First(a => a.CustomerId == package.CustomerId);
                var facilityDetails = facilityList.First(a => a.FacilityId == package.FacilityId);
                lstbox_Customer.Items.Add($"{facilityDetails.FacilityName}: {package.StoredDate}\n {customerDetails.FirstName} {customerDetails.LastName}");
            }

            lstbox_Customer.SelectedIndex = 0;
        }

        private void goBack() {
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }
    }
}
