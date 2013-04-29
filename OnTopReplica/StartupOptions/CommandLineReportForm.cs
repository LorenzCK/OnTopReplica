using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OnTopReplica.StartupOptions {
    public partial class CommandLineReportForm : Form {

        public CommandLineReportForm(CliStatus status, string message) {
            InitializeComponent();

            switch (status) {
                case CliStatus.Information:
                    labelInstruction.Text = "Command line help";
                    break;

                case CliStatus.Error:
                    labelInstruction.Text = "Command line parsing error";
                    break;
            }

            txtDescription.Text = message;

            txtCliArgs.Text = Environment.CommandLine;
        }

    }
}
