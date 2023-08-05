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
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        string FacilityName { get; set; }
        string PackageType { get; set; }
        public AddCustomer(string facilityName, string packageType)
        {
            InitializeComponent();

            FacilityName = facilityName;
            PackageType = packageType;
        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            goBack();
        }

        private void btn_AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            //VALIDATES FIELDS
            if (HasEmptyFields() || NotValidPhone(txtPhone.Text)) return;

            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string phone = txtPhone.Text;

            //CHECKS IF CUSTOMER EXISTS
            try
            {
                using var context = new DbConfig();
                var cst = context.Customers
                    .Where(a => a.FirstName.ToLower() == firstName.ToLower() && a.LastName.ToLower() == lastName.ToLower())
                    .Count();

                if (cst > 0) {
                    MessageBox.Show("This customer already exist");
                    txtFirstName.Text = string.Empty;
                    txtLastName.Text = string.Empty;
                    txtFirstName.Focus();

                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n {ex}");
            }

            //2ND VERIFICATION TO ADD
            if (MessageBox.Show($"DO YOU WANT TO ADD THIS CUSTOMER?\n\n " +
                $"- CUSTOMER INFORMATION -\n" +
                $"First Name: {firstName}\n " +
                $"Last Nmae:  {lastName}\n" +
                $"Phone: {phone}"
                , "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            
            //ADD CUSTOMER TO DATABASE
            try
            {
                using var context = new DbConfig();
                var cst = new Customer()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = phone,
                };

                context.Add(cst);
                context.SaveChanges();

                MessageBox.Show("CUSTOMER Successfully Added");
                goBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n {ex}");
            }

        }

        //-- HELPING METHODS --

        private bool HasEmptyFields()
        {
            string emptyFields = "";
            if (txtFirstName.Text.Trim().Length == 0 || string.IsNullOrEmpty(txtFirstName.Text)) emptyFields += "First Name\n";
            if (txtLastName.Text.Trim().Length == 0 || string.IsNullOrEmpty(txtLastName.Text)) emptyFields += "Last Name\n";
            if (txtPhone.Text.Trim().Length == 0 || string.IsNullOrEmpty(txtPhone.Text)) emptyFields += "Phone\n";

            if (emptyFields == "") return false;

            MessageBox.Show($"Field/s are empty: \n{emptyFields}");
            return true;
        }

        private bool NotValidPhone(string phone)
        {
            string pattern1 = @"^\+63\d{10}$";
            string pattern2 = @"^\0\d{10}$";

            if (Regex.IsMatch(phone, pattern1) || Regex.IsMatch(phone, pattern2)) return false;

            MessageBox.Show("Invalid Phone number." +
                "\nOnly accepts:" +
                "\n* Philippine Standard" +
                "\n* Starts with +63 or 0" +
                "\n* does not contain spaces");
            return true;
        }

        private void goBack() {
            var newWindow = new AddPackage(FacilityName, PackageType);
            newWindow.Show();
            this.Close();
        }
    }
}
