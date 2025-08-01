using ASCOM.Utilities;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ASCOM.photonTouptekAFW.FilterWheel
{
    [ComVisible(false)] // Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private const string NO_PORTS_MESSAGE = "No COM ports found";
        private TraceLogger tl; // Holder for a reference to the driver's trace logger

        public SetupDialogForm(TraceLogger tlDriver)
        {
            InitializeComponent();

            // Save the provided trace logger for use within the setup dialogue
            tl = tlDriver;

            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void CmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here and update the state variables with results from the dialogue

            tl.Enabled = chkTrace.Checked;

            // Update the COM port variable if one has been selected
            if (comboBoxComPort.SelectedItem is null) // No COM port selected
            {
                tl.LogMessage("Setup OK", $"New configuration values - COM Port: Not selected");
            }
            else if (comboBoxComPort.SelectedItem.ToString() == NO_PORTS_MESSAGE)
            {
                tl.LogMessage("Setup OK", $"New configuration values - NO COM ports detected on this PC.");
            }
            else // A valid COM port has been selected
            {
                FilterWheelHardware.comPort = (string)comboBoxComPort.SelectedItem;
                FilterWheelHardware.IsBidirectional = chkBidirectional.Checked;
                if (cntSlots.SelectedValue != null && int.TryParse(cntSlots.SelectedValue.ToString(), out int slots))
                {
                    FilterWheelHardware.slots = (short)slots;
                }
                tl.LogMessage("Setup OK", $"New configuration values - COM Port: {comboBoxComPort.SelectedItem}");
            }
        }

        private void CmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("https://ascom-standards.org/");
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            // Set the trace checkbox
            chkTrace.Checked = tl.Enabled;
            comboBoxComPort.Items.Clear(); // Clear any existing entries
            Toupcam.DeviceV2[] arr = Toupcam.EnumV2();

            foreach (Toupcam.DeviceV2 cam in arr)
            {
                if ((cam.model.flag & (ulong)Toupcam.eFLAG.FLAG_FILTERWHEEL) != 0)
                {
                    comboBoxComPort.Items.Add(cam.id);
                }
            }

            // select the current port if possible
            if (comboBoxComPort.Items.Contains(FilterWheelHardware.comPort))
            {
                comboBoxComPort.SelectedItem = FilterWheelHardware.comPort;
            }

            chkBidirectional.Checked = FilterWheelHardware.IsBidirectional;

            try
            {
                if (FilterWheelHardware.slots == 5)
                {
                    cntSlots.SelectedIndex = 0;
                }
                else if (FilterWheelHardware.slots == 7)
                {
                    cntSlots.SelectedIndex = 1;
                }
                else if (FilterWheelHardware.slots == 8)
                {
                    cntSlots.SelectedIndex = 2;
                }
            }
            catch (Exception ex)
            {
                tl.LogMessage("InitUI", $"Error setting slots: {ex.Message}");
                cntSlots.SelectedIndex = -1; // No valid selection
            }

            tl.LogMessage("InitUI", $"Set UI controls to Trace: {chkTrace.Checked}, COM Port: {comboBoxComPort.SelectedItem}");
        }

        private void SetupDialogForm_Load(object sender, EventArgs e)
        {
            // Bring the setup dialogue to the front of the screen
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            else
            {
                TopMost = true;
                Focus();
                BringToFront();
                TopMost = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}