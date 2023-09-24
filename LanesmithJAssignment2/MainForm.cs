using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

namespace LanesmithJAssignment2
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

            // Set custom formt for the DateTimePicker field
            dtpApptDate.CustomFormat = "dd MMM yyyy";
            dtpApptDate.Format = DateTimePickerFormat.Custom;
        }

        private void brnPreFill_Click(object sender, EventArgs e)
        {
            // Set all field values with example valid data
            txtName.Text = "Bob Smith";
            txtAddress.Text = "123 Anywhere St.";
            txtCity.Text = "Waterloo";
            txtProvince.Text = "ON";
            txtPostalCode.Text = "H0H 0H0";
            txtHomePhone.Text = "555-345-1234";
            txtCellPhone.Text = "555-678-0987";
            txtEmail.Text = "example@test.com";
            txtMakeModel.Text = "Toyota Corolla";
            txtYear.Text = "2020";
            dtpApptDate.Value = DateTime.Today.AddDays(10);
            txtProblems.Text = "Loud noise when I apply the brakes\r\nWinshield wipers squeak every time I use them";//.Replace("\n", Environment.NewLine);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Call method to reset all fields
            ResetFields();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the application window
            this.Close();
        }

        private void btnBookAppt_Click(object sender, EventArgs e)
        {
            // Instantiate list of KeyValuePairs to track error messages and the primary form control it is related to
            List<KeyValuePair<Control, string>> errorMessages = new List<KeyValuePair<Control, string>>();

            // Save all field values into variables
            string name = txtName.Text.Trim();
            string address = ValidationHelper.Capitalize(txtAddress.Text.Trim());
            string city = ValidationHelper.Capitalize(txtCity.Text.Trim());
            string provCode = txtProvince.Text.Trim().ToUpper();
            string postalCode = txtPostalCode.Text.Trim().ToUpper();
            string homePhone = txtHomePhone.Text.Trim();
            string cellPhone = txtCellPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string makeModel = txtMakeModel.Text.Trim();
            string year = txtYear.Text.Trim();
            DateTime apptDate = dtpApptDate.Value.Date;
            string problemNotes = txtProblems.Text.Trim();

            // Declare all bool variables
            bool validEntry = true;
            bool validProvCode = ValidationHelper.IsValidProvinceCode(provCode);
            bool validPostalCode = ValidationHelper.IsValidPostalCode(postalCode);
            bool validHomePhone = ValidationHelper.IsValidPhoneNumber(homePhone);
            bool validCellPhone = ValidationHelper.IsValidPhoneNumber(cellPhone);

            // Check if name was given
            if (string.IsNullOrEmpty(name))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtName, "Name field is required."));
            }
            else
            {
                // Capitalize name
                name = ValidationHelper.Capitalize(txtName.Text);
            }
            
            // Check if user missed entering full address and email
            if (string.IsNullOrEmpty(email) && (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(provCode) || string.IsNullOrEmpty(postalCode)))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtAddress, "Must provide an email or a full postal address."));
            }

            // Check if the province code was given and if it is valid
            if (!validProvCode && !string.IsNullOrEmpty(provCode))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtProvince, "Invalid province code entered - Must be 2 letter code such as ON, AB, etc.."));
            }

            // Check if postal code was given and if it is valid
            if (!validPostalCode && !string.IsNullOrEmpty(postalCode))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtPostalCode, "Invalid postal code entered. Expected format - L0L 0L0."));
            }
            else if (validPostalCode && postalCode.Length == 6)
            {
                // Insert space and the middle of postal code if it doesn't already exist
                postalCode = postalCode.Insert(3, " ");
            }

            // Check if user missed entering any phone number
            if (string.IsNullOrEmpty(homePhone) && string.IsNullOrEmpty(cellPhone))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtHomePhone, "One valid phone number is required."));
            }

            // Check if home phone number is valid
            if (validHomePhone)
            {
                // Make sure phone number is formated with dashes 
                homePhone = ValidationHelper.FormatPhoneNumber(homePhone);
            }
            else if (!string.IsNullOrEmpty(homePhone))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtHomePhone, "Invalid home phone number entered. Expected format - ###-###-####"));
            }

            // Check if cell phone number is valid
            if (validCellPhone)
            {
                // Make sure phone number is formated with dashes 
                cellPhone = ValidationHelper.FormatPhoneNumber(cellPhone);
            }
            else if (!string.IsNullOrEmpty(cellPhone))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtCellPhone, "Invalid cell phone number entered. Expected format - ###-###-####"));
            }

            // Check if an email was given and if it is valid or not
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$") && !string.IsNullOrEmpty(email))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtEmail, "Invalid email entered. Expected format - example@host.com"));
            }

            // Check if make and model was given
            if (string.IsNullOrEmpty(makeModel))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtMakeModel, "Make and Model field is required."));
            }
            else
            {
                // Capitalize make and model
                makeModel = ValidationHelper.Capitalize(txtMakeModel.Text);
            }

            // Check if year was given and it is in the proper range
            if (!string.IsNullOrEmpty(year) && (!int.TryParse(year, out int yrInt) || !(yrInt >= 1900 && yrInt<= 2022)))
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(txtYear, "Invalid year entered, must be from 1900 to 2022."));
            }

            // Check if appointment date is valid
            if (DateTime.Compare(apptDate, DateTime.Today) < 0)
            {
                // Set valid entry as false and new error message to the list
                validEntry = false;
                errorMessages.Add(new KeyValuePair<Control, string>(dtpApptDate, "Invalid appointment date. Appointment date cannot be in the past."));
            }

            // Set message label as a blank string
            lblMessage.Text = "";

            // Check if any validation errors occured
            if (!validEntry)
            {
                // Loop through list of error messages and add them to the message label
                foreach (KeyValuePair<Control, string> message in errorMessages)
                {
                    lblMessage.Text += message.Value + "\n";
                }

                // Set form focus to field associated with first error message
                errorMessages[0].Key.Focus();
            }
            else
            {
                // Replace new line characters with '~' to create single line string for writing to .txt file
                problemNotes = problemNotes.Replace("\r\n", "~");

                // Define file path for appointments text file
                const string Path = @"..\..\..\appointments.txt";

                // Open text file using stream writer
                using (StreamWriter textIn = new StreamWriter(Path, true))
                {
                    // Append line of appointment details to end of text file
                    textIn.WriteLine($"{name}|{address}|{city}|{provCode}|{postalCode}|{homePhone}|{cellPhone}|{email}|{makeModel}|{year}|{apptDate.ToString("dd MMM yyyy")}|{problemNotes}");
                }

                // Set message label with success message identifying name of person
                lblMessage.Text += $"Appointment successfully booked for {name}!";
            }

            // Set message label as visible and reset all form fields
            lblMessage.Visible = true;
            ResetFields();
        }

        private void ResetFields()
        {
            // Reset all fields to blank values
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtProvince.Text = string.Empty;
            txtPostalCode.Text = string.Empty;
            txtHomePhone.Text = string.Empty;
            txtCellPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMakeModel.Text = string.Empty;
            txtYear.Text = string.Empty;
            dtpApptDate.Value = DateTime.Now;
            txtProblems.Text = string.Empty;
        }

    }
}