using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Wpf.CustomerManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$");
    private static readonly Regex PhonePattern = new(@"^\d{10}$");

    public MainWindow()
    {
        InitializeComponent();
    }

    // Plain code-behind event handler - this is the WinForms-style way of doing things.
    // Day 2 replaces this entire method with a bound ICommand on a ViewModel; keep this
    // version around mentally as the "before" picture once you get there.
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var error = Validate();
        if (error is not null)
        {
            ShowStatus(error, isError: true);
            return;
        }

        var customerType = ((System.Windows.Controls.ComboBoxItem)cboCustomerType.SelectedItem).Content;
        ShowStatus(
            $"Saved: {txtFirstName.Text} {txtLastName.Text} ({customerType}) — DOB {dpDateOfBirth.SelectedDate:d}",
            isError: false);
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        txtFirstName.Clear();
        txtLastName.Clear();
        txtEmail.Clear();
        txtPhone.Clear();
        dpDateOfBirth.SelectedDate = null;
        cboCustomerType.SelectedIndex = 0;
        borderStatus.Visibility = Visibility.Collapsed;
    }

    // Returns the first validation failure, or null if everything's fine. A real app would
    // surface per-field errors (that's what INotifyDataErrorInfo buys you in Day 2/3) - this
    // single-message version is deliberately the simplest thing that works, for Day 1.
    private string? Validate()
    {
        if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            return "First name is required.";
        if (string.IsNullOrWhiteSpace(txtLastName.Text))
            return "Last name is required.";
        if (string.IsNullOrWhiteSpace(txtEmail.Text) || !EmailPattern.IsMatch(txtEmail.Text))
            return "Enter a valid email address.";
        if (string.IsNullOrWhiteSpace(txtPhone.Text) || !PhonePattern.IsMatch(txtPhone.Text))
            return "Phone must be exactly 10 digits.";
        if (dpDateOfBirth.SelectedDate is null)
            return "Date of birth is required.";
        if (dpDateOfBirth.SelectedDate > DateTime.Today.AddYears(-18))
            return "Customer must be at least 18 years old.";

        return null;
    }

    private void ShowStatus(string message, bool isError)
    {
        txtStatus.Text = message;
        borderStatus.Background = isError
            ? new SolidColorBrush(Color.FromRgb(0xFE, 0xE2, 0xE2))
            : new SolidColorBrush(Color.FromRgb(0xDC, 0xFC, 0xE7));
        txtStatus.Foreground = isError
            ? new SolidColorBrush(Color.FromRgb(0x99, 0x1B, 0x1B))
            : new SolidColorBrush(Color.FromRgb(0x14, 0x53, 0x2D));
        borderStatus.Visibility = Visibility.Visible;
    }
}
