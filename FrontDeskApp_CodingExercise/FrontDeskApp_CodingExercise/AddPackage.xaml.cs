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
    /// Interaction logic for AddPackage.xaml
    /// </summary>
    public partial class AddPackage : Window
    {
        string FacilityName { get; set; }
        string PackageType { get; set; }
        List<Customer> CustomerList { get; set; }

        public AddPackage(string facilityName, string packageType)
        {
            InitializeComponent();

            FacilityName = facilityName;
            PackageType = packageType;
            FillFacilityDetails();
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            goBack();
        }

        private void btn_AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new AddCustomer(FacilityName, PackageType);
            newWindow.Show();
            this.Close();
        }

        private void list_CustomerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int itemIndex = list_CustomerName.SelectedIndex;
            if (itemIndex == -1) return;

            lblCustomerName.Content = $"{CustomerList[0].FirstName} {CustomerList[0].LastName}";
            lblPhone.Content = CustomerList[0].Phone;
        }

        private void btn_AddPackage_Click(object sender, RoutedEventArgs e)
        {
            //2ND VERIFICATION  TO ADD PACKAGE TO STORE
            var packageCustomerToAdd = CustomerList[list_CustomerName.SelectedIndex];
            if (MessageBox.Show($"DO YOU WANT TO STORE THIS PACKAGE?\n\n" +
               $"- PACKAGE INFORMATION -\n" +
               $"Facility: {FacilityName}\n" +
               $"Size of Package: {PackageType}\n\n" +
               $"- CUSTOMER INFORMATION -\n" +
               $"First Name: {packageCustomerToAdd.FirstName}\n" +
               $"Last Nmae:  {packageCustomerToAdd.LastName}\n" +
               $"Phone: {packageCustomerToAdd.Phone}"
               , "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            //STORE PACKAGE IN THE DATABASE
            try
            {
                using var context = new DbConfig();
                var getCustomer = context.Customers
                    .First(a => a.FirstName == packageCustomerToAdd.FirstName);
                var getFacility = context.Facilities
                    .First(a => a.FacilityName == FacilityName);
                var packageSize = SelectPackageSize(PackageType);

                if (packageSize == PackageSize.Small) getFacility.SmallRemainingCapacity--;
                else if (packageSize == PackageSize.Medium) getFacility.MediumRemainingCapacity--;
                else if (packageSize == PackageSize.Large) getFacility.LargeRemainingCapacity--;

                var pkg = new StoredPackage()
                {
                    PackageSize = packageSize,
                    StoredDate = DateTime.Now,
                    RetrievedPackage = false,
                    CustomerId = getCustomer.CustomerId,
                    FacilityId = getFacility.FacilityId
                };

                context.Update(getFacility);
                context.Add(pkg);
                context.SaveChanges();

                MessageBox.Show("PACKAGE Successfully Added");
                goBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex}");
            }
        }

        private void txtSearchCustomer_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchCustomer = txtSearchCustomer.Text;

            lblCustomerName.Content = string.Empty;
            lblPhone.Content = string.Empty;

            FillCustomerList(searchCustomer);
        }

        //-- HELPING METHODS --
        private void FillFacilityDetails()
        {
            lblFacility.Content = FacilityName;
            lblPackageType.Content = PackageType;

            FillCustomerList("");
        }

        private void FillCustomerList(string name) {
            using var context = new DbConfig();
            CustomerList = new List<Customer>();
            CustomerList.AddRange(context.Customers
                .Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name))
                .ToList());

            list_CustomerName.Items.Clear();
            foreach (var customer in CustomerList)
            {
                list_CustomerName.Items.Add($"{customer.FirstName} {customer.LastName}");
            }

            list_CustomerName.SelectedIndex = 0;
        }

        public PackageSize SelectPackageSize(string packageType) {
            return packageType switch
            {
                "Small" => PackageSize.Small,
                "Medium" => PackageSize.Medium,
                "Large" => PackageSize.Large,
                _ => PackageSize.Small,
            };
        }

        public void goBack() {
            var newWindow = new SelectPackage();
            newWindow.Show();
            this.Close();
        }
    }
}
