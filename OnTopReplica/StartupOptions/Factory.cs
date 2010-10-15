using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using OnTopReplica.Properties;

namespace OnTopReplica.StartupOptions {
    class Factory {

        static Factory() {
            //Custom type conversion attributes
            TypeDescriptor.AddAttributes(typeof(Size), new TypeConverterAttribute(typeof(SizeConverter)));
            TypeDescriptor.AddAttributes(typeof(ScreenPosition), new TypeConverterAttribute(typeof(ScreenPositionConverter)));
            TypeDescriptor.AddAttributes(typeof(Rectangle), new TypeConverterAttribute(typeof(RectangleConverter)));
        }

        public static Options CreateOptions(string[] args) {
            var options = new Options();

            LoadSettings(options);

            ParseCommandLine(args, options);

            return options;
        }

        private static void LoadSettings(Options options) {
            if (Settings.Default.RestoreSizeAndPosition) {
                options.StartLocation = Settings.Default.RestoreLastPosition;
                options.StartSize = Settings.Default.RestoreLastSize;
            }
        }

        private static void ParseCommandLine(string[] args, Options options) {
            var cmdOptions = new NDesk.Options.OptionSet()
                .Add<long>("windowId=", "Window handle ({HWND}) to be cloned.", id => {
                    options.WindowId = new IntPtr(id);
                })
                .Add<string>("windowTitle=", "{TITLE} of the window to be cloned.", s => {
                    options.WindowTitle = s;
                })
                .Add<string>("windowClass=", "{CLASS} of the window to be cloned.", s => {
                    options.WindowClass = s;
                })
                .Add("v|visible", "If set, only clones windows that are visible.", s => {
                    options.MustBeVisible = true;
                })
                .Add<Size>("size=", "Target {SIZE} of the cloned thumbnail.", s => {
                    options.StartSize = s;
                })
                .Add<Size>("position=", "Target {COORDINATES} of the OnTopReplica window.", s => {
                    options.StartLocation = new Point(s.Width, s.Height);
                    options.StartScreenPosition = null;
                })
                .Add<ScreenPosition>("screenPosition=", "Resolution independent window position on current screen, with locking (TR|TL|C|BR|BL).", pos => {
                    options.StartLocation = null;
                    options.StartScreenPosition = pos;
                })
                .Add<Rectangle>("r|region=", "Region {BOUNDS} of the original window.", region => {
                    options.Region = region;
                })
                .Add<byte>("o|opacity=", "Opacity of the window (0-255).", opacity => {
                    options.Opacity = opacity;
                })
                .Add("clickForwarding", "Enables click forwarding.", s => {
                    options.EnableClickForwarding = true;
                })
                .Add("chromeOff", "Disables the window's chrome (border).", s => {
                    options.DisableChrome = true;
                })
                .Add("h|help|?", "Show command line help.", s => {
                    options.Status = CliStatus.Information;
                });

            List<string> values;
            try {
                values = cmdOptions.Parse(args);
            }
            catch (NDesk.Options.OptionException ex) {
                options.DebugMessageWriter.WriteLine(ex.Message);
                options.DebugMessageWriter.WriteLine("Try 'OnTopReplica /help' for more information.");
                options.Status = CliStatus.Error;
            }

            if (options.Status == CliStatus.Information) {
                cmdOptions.WriteOptionDescriptions(options.DebugMessageWriter);
            }
        }

    }
}
