using System;
using System.Windows.Forms;

namespace EntityFrameworkVisualizer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button "Copy" click event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void ButtonCopyClick(object sender, EventArgs e)
        {
            Clipboard.SetText(string.Join(Environment.NewLine, richTextBox.Lines));
        }

        /// <summary>
        /// Button "Copy" click event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void ButtonCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Form load event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Size = Settings.Default.WindowSize;
            WindowState = Settings.Default.WindowState;
            Location = Settings.Default.Location;
        }

        /// <summary>
        /// Form closing event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event data.</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (WindowState != FormWindowState.Maximized)
            {
                Settings.Default.WindowSize = Size;
                Settings.Default.Location = Location;
            }

            Settings.Default.WindowState = WindowState;
            Settings.Default.Save();
        }
    }
}
