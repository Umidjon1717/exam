using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq; // Fixed typo "Lin q" to "Linq"
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Load data into the 'dbDataSet.tbCountry' table
                this.tbCountryTableAdapter.Fill(this.dbDataSet.tbCountry);

                // Load data into the 'dbDataSet.tbTeacher' table
                this.tbTeacherTableAdapter.Fill(this.dbDataSet.tbTeacher);
            }
            catch (Exception ex)
            {
                // Show error message if data loading fails
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveData();
        }

        private void saveData()
        {
            if (this.Validate())
            {
                try
                {
                    // End editing and save data to the database
                    this.tbTeacherBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.dbDataSet);
                    MessageBox.Show("Data saved successfully!");
                }
                catch (Exception ex)
                {
                    // Show error message if saving fails
                    MessageBox.Show($"Error saving data: {ex.Message}");
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Validate())
            {
                this.tbTeacherBindingSource.EndEdit();

                // Check if there are any unsaved changes
                if (dbDataSet.HasChanges())
                {
                    // Prompt user to save changes
                    if (MessageBox.Show("Do you want to save changes?", "Save", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        saveData();
                    }
                }
            }
            else
            {
                // Cancel closing if validation fails
                e.Cancel = true;
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            // Move to the first record
            tbTeacherBindingSource.MoveFirst();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // Move to the previous record
            tbTeacherBindingSource.MovePrevious();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Move to the next record
            tbTeacherBindingSource.MoveNext();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            // Move to the last record
            tbTeacherBindingSource.MoveLast();
        }

        private void EnableDisableButtons()
        {
            // Disable/Enable navigation buttons based on the position
            btnFirst.Enabled = tbTeacherBindingSource.Position > 0;
            btnPrevious.Enabled = tbTeacherBindingSource.Position > 0;
            btnNext.Enabled = tbTeacherBindingSource.Position < tbTeacherBindingSource.Count - 1;
            btnLast.Enabled = tbTeacherBindingSource.Position < tbTeacherBindingSource.Count - 1;
        }

        private void tbTeacherBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            // Update navigation buttons when the current position changes
            EnableDisableButtons();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbTeacherBindingSource.Count == 0)
            {
                // Inform user if there are no records to delete
                MessageBox.Show("No records available to delete.");
            }
            else
            {
                // Confirm deletion
                var userResponse = MessageBox.Show("Are you sure you want to delete this record?", "Delete", MessageBoxButtons.YesNo);
                if (userResponse == DialogResult.Yes)
                {
                    tbTeacherBindingSource.RemoveCurrent();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Add a new record with the provided values
                var selectedCountry = ((DataRowView)cbxNewCountry.SelectedItem).Row;
                dbDataSet.tbTeacher.AddtbTeacherRow(
                    tbxNewFirstName.Text,
                    tbxNewLastName.Text,
                    dtpNewDob.Value,
                    tbxNewPhone.Text,
                    Convert.ToInt32(nudNewGrade.Value),
                    chbNewisActive.Checked,
                    (dbDataSet.tbCountryRow)selectedCountry);

                MessageBox.Show("New record added!");
            }
            catch (Exception ex)
            {
                // Show error message if adding fails
                MessageBox.Show($"Error adding record: {ex.Message}");
            }
        }

        private void tbxFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Apply a filter to the binding source based on the entered text
                tbTeacherBindingSource.Filter = $"lastname LIKE '{tbxFilter.Text}%'";
            }
            catch (Exception ex)
            {
                // Handle invalid filter expression
                MessageBox.Show($"Error filtering data: {ex.Message}");
            }
        }

        private void firstNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            // Ensure the first name field is not empty
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                MessageBox.Show("First name cannot be empty.");
                e.Cancel = true;
            }
        }
    }
}