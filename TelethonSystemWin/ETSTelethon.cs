using ETSLib.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelethonSystemWin
{
    public partial class ETSTelethon : Form
    {
        //Library and objects
        ETSManager manager = new ETSManager();

        //sponsor
        string sponsorFirstName;
        string sponsorLastName;
        string sponsorID;
        double sponsorTotalPrizeValue;

        //donor
        string donorFirstName;
        string donorLastName;
        string donorID;
        string donorAddress;
        string donorPhone;
        char donorCardType;
        string donorCardNumber;
        string donorCardExpiry;

        //prize
        string prizeID;
        string prizeDescription;
        double prizeValue;
        double prizeDonationLimit;
        int prizeOriginalAvailable;
        int prizeCurrentAvailable;
        string prizeSponsorID;

        //donation
        string donationID;
        string donationDate;
        string donationDonorID;
        double donationAmount;
        string donationPrizeID;
        public ETSTelethon()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sponsorAddSponsorButton_Click(object sender, EventArgs e)
        {
            if (!SponsorEmptyFieldChecker())
            {
                MessageBox.Show("All boxes must be filled out before adding the sponsor");
            }
            else
            {
                try
                {
                    sponsorFirstName = sponsorFirstNameBox.Text;
                    sponsorLastName = sponsorLastNameBox.Text;
                    sponsorID = sponsorSponsorIDBox.Text;
                    prizeValue = Convert.ToDouble(sponsorPrizeValueBox.Text);
                    manager.AddSponsor(sponsorFirstName, sponsorLastName, sponsorID, prizeValue);
                    MessageBox.Show("Sponsor successfully added!");
                }
                catch(Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }


        }

        private void sponsorAddPrizeButton_Click(object sender, EventArgs e)
        {
            if (!SponsorEmptyFieldChecker())
            {
                MessageBox.Show("All boxes must be filled out before adding a prize!");
            }
            else
            {
                try
                {
                    sponsorTotalPrizeValue = Convert.ToDouble(sponsorPrizeValueBox.Text);
                    prizeID = sponsorPrizeIDBox.Text;
                    prizeDescription = sponsorDescriptionBox.Text;
                    prizeDonationLimit = Convert.ToDouble(sponsorMinDonationLimitBox.Text);
                    prizeOriginalAvailable = Convert.ToInt32(sponsorHowManyBox.Text);

                    prizeSponsorID = sponsorSponsorIDBox.Text;

                    manager.AddPrize(prizeID, prizeDescription, sponsorTotalPrizeValue, prizeDonationLimit, prizeOriginalAvailable, sponsorID);
                    MessageBox.Show("Prize successfully added!");
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void donorAddDonationButton_Click(object sender, EventArgs e)
        {
            Regex validateExpiry = new Regex(@"^((0[1-9])|(1[0-2]))[\/\.\-]*((2[2-9])|(3[1-9]))$");

            if(donorDonorIDBox.Text == "" || donorFirstNameBox.Text == "" || donorAddressBox.Text == "" || donorPhoneBox.Text == "")
            {
                MessageBox.Show("Error in Donor Information subsection.\nPlease make sure all boxes are formatted properly.");
            }

            else if (donorDonationIDBox.Text == "" || donorAmountBox.Text == "")
            {
                MessageBox.Show("Error in Donation Information subsection.\nPlease make sure all boxes are formatted properly.");

            }

            else if (!validateExpiry.IsMatch(donorCCExpiryBox.Text) || !CreditCardVerifyer() || donorCCNumberBox.Text == "" || donorCCExpiryBox.Text == "")
            {
                MessageBox.Show("Error in Credit Card Information subsection.\nPlease make sure all boxes are formatted properly.");

            }

            else if(donorPrizeIDBox.Text == "" || donorPrizeNumberBox.Text == "")
            {
                MessageBox.Show("Error in Award Prize subsection.\nPlease make sure all boxes are formatted properly.");

            }
            else if (donorVisaRadioButton.Checked == false && donorMCRadioButton.Checked == false && donorAmexRadionButton.Checked == false)
            {
                MessageBox.Show("Error in Card Type subsection.\nPlease make sure all boxes are formatted properly.");

            }
            else
            {
                try
                {
                    donorFirstName = donorFirstNameBox.Text;
                    donorLastName = donorLastNameBox.Text;
                    donorID = donorDonorIDBox.Text;
                    donorAddress = donorAddressBox.Text;
                    donorPhone = donorPhoneBox.Text;
                    char cardType = 'E';
                    if (donorVisaRadioButton.Checked)
                    {
                        cardType = 'V';
                    }
                    if (donorMCRadioButton.Checked)
                    {
                        cardType = 'M';
                    }
                    if (donorAmexRadionButton.Checked)
                    {
                        cardType = 'A';
                    }

                    manager.AddDonor(donorID, donorFirstName, donorLastName, donorAddress, donorPhone, cardType, donorCardNumber, donorCardExpiry);
                    manager.AddDonation(donationID, donationDate, donorID, donationAmount, prizeID);
                    MessageBox.Show("Donor and Donation info successfully added!");
                }
                catch(Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }

        }

        private bool SponsorEmptyFieldChecker()
        {
            return !((sponsorFirstNameBox.Text == "") || (sponsorLastNameBox.Text == "") || (sponsorSponsorIDBox.Text == "") || (sponsorPrizeIDBox.Text == "") || (sponsorDescriptionBox.Text == "") || (sponsorHowManyBox.Text == "") || (sponsorMinDonationLimitBox.Text == "") || (sponsorPrizeValueBox.Text == ""));
        }

        private void sponsorViewSponsorButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(manager.ListSponsors());
        }

        private void sponsorViewPrizeButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(manager.ListPrizes());
        }

        private void donorCCNumberBox_TextChanged(object sender, EventArgs e)
        {
            if (CreditCardVerifyer())
            {
                creditCardVerifye.Visible = false;
                ccVerifyer.Visible = false;
            }
            else
            {
                creditCardVerifye.Visible = true;
                ccVerifyer.Visible = true;
            }
        }
        private bool CreditCardVerifyer()
        {
            Regex regex = new Regex(@"[0-9]{16}");
            return regex.IsMatch(donorCCNumberBox.Text);
        }
        private bool CreditCardExpiryVerifyer()
        {
            Regex regex = new Regex(@"[0-9]{4}");
            return regex.IsMatch(donorCCExpiryBox.Text);
        }

        private void donorCCExpiryBox_TextChanged(object sender, EventArgs e)
        {
            if (CreditCardExpiryVerifyer())
            {
                expiryLabel1.Visible = false;
                expiryLabel2.Visible = false;
            }
            else
            {
                expiryLabel1.Visible = true;
                expiryLabel2.Visible = true;
            }
        }

        private void donorListDonationButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(manager.ListDonations());
        }

        private void donorListDonorsButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(manager.ListDonors());
        }

        private void donorSaveDonorInfoButton_Click(object sender, EventArgs e)
        {
            if (manager.WriteDonors())
            {
                MessageBox.Show("Donors successfully written to file.");
            }
            else
            {
                MessageBox.Show("Error while writing donors to file.");
            }
        }

        private void donorShowPrizesButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(manager.ListPrizes());
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                manager.GetDonors();
            }
            catch(Exception er)
            {
                MessageBox.Show("No donors found!", er.Message);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.WriteDonors();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
