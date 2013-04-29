using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using OnTopReplica.Properties;
using OnTopReplica.WindowSeekers;
using System.Windows.Forms;

namespace OnTopReplica.StartupOptions {
    class Factory {

        static Factory() {
            //Custom type conversion attributes
            TypeDescriptor.AddAttributes(typeof(Size), new TypeConverterAttribute(typeof(SizeConverter)));
            TypeDescriptor.AddAttributes(typeof(ScreenPosition), new TypeConverterAttribute(typeof(ScreenPositionConverter)));
            TypeDescriptor.AddAttributes(typeof(Rectangle), new TypeConverterAttribute(typeof(RectangleConverter)));
            TypeDescriptor.AddAttributes(typeof(Padding), new TypeConverterAttribute(typeof(PaddingConverter)));
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

                System.Diagnostics.Trace.WriteLine(string.Format("Restoring window at {0} size {1}.", Settings.Default.RestoreLastPosition, Settings.Default.RestoreLastSize));
            }

            if (Settings.Default.RestoreLastWindow) {
                var handle = Settings.Default.RestoreLastWindowHwnd;
                var title = Settings.Default.RestoreLastWindowTitle;
                var className = Settings.Default.RestoreLastWindowClass;

                var seeker = new RestoreWindowSeeker(new IntPtr(handle), title, className);
                seeker.SkipNotVisibleWindows = true;
                seeker.Refresh();
                var resultHandle = seeker.Windows.FirstOrDefault();

                if (resultHandle != null) {
                    //Load window
                    options.WindowId = resultHandle.Handle;
                }
                else {
                    System.Diagnostics.Trace.WriteLine("Couldn't find window to restore.");
                }
            }
        }

        private static void ParseCommandLine(string[] args, Options options) {
            var cmdOptions = new NDesk.Options.OptionSet()
                .Add<long>("windowId=", "Window handle ({HWND}) to be cloned.", id => {
                    options.WindowId = new IntPtr(id);
                })
                .Add<string>("windowTitle=", "Partial {TITLE} of the window to be cloned.", s => {
                    options.WindowTitle = s;
                })
                .Add<string>("windowClass=", "{CLASS} of the window to be cloned.", s => {
                    options.WindowClass = s;
                })
                .Add("v|visible", "If set, only clones windows that are visible.", s => {
                    options.MustBeVisible = true;
                })
                .Add<Size>("size=", "Target {WIDTH,HEIGHT} of the cloned thumbnail.", s => {
                    options.StartSize = s;
                })
                .Add<Size>("position=", "Target {X,Y} of the OnTopReplica window.", s => {
                    options.StartLocation = new Point(s.Width, s.Height);
                    options.StartPositionLock = null;
                })
                .Add<ScreenPosition>("screenPosition=", "Resolution independent window position on current screen, with locking. Values: {TR|TL|C|BR|BL}.", pos => {
                    options.StartLocation = null;
                    options.StartPositionLock = pos;
                })
                .Add<Rectangle>("r|region=", "Region {X,Y,W,H} of the cloned window.", region => {
                    options.Region = new ThumbnailRegion(region);
                })
                .Add<System.Windows.Forms.Padding>("p|padding=", "Region padding {LEFT,TOP,RIGHT,BOTTOM} of the clone.", padding => {
                    options.Region = new ThumbnailRegion(padding);
                })
                .Add<byte>("o|opacity=", "Opacity of the window: {0-255}.", opacity => {
                    options.Opacity = opacity;
                })
                .Add("clickForwarding", "Enables click forwarding.", s => {
                    options.EnableClickForwarding = true;
                })
                .Add("chromeOff", "Disables the window's chrome (border).", s => {
                    options.DisableChrome = true;
                })
                .Add("fs|fullscreen", "Starts up in fullscreen mode.", s => {
                    options.Fullscreen = true;
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
