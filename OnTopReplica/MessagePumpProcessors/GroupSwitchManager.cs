using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using OnTopReplica.Native;

namespace OnTopReplica.MessagePumpProcessors {
    
    class GroupSwitchManager : BaseMessagePumpProcessor {

        public GroupSwitchManager() {
            _hookMsgId = HookMethods.RegisterWindowMessage("SHELLHOOK");
            if (_hookMsgId == 0)
                Console.Error.WriteLine("Failed to register SHELLHOOK Windows message.");
        }

        uint _hookMsgId;
        bool _active = false;
        List<WindowHandleWrapper> _lruHandles;

        /// <summary>
        /// Enables group switch mode.
        /// </summary>
        /// <param name="handles">List of window handles to track.</param>
        public void EnableGroupMode(IList<WindowHandle> handles) {
            if (handles == null || handles.Count == 0)
                return;

            //Enable new hook
            if (!_active) {
                if (!HookMethods.RegisterShellHookWindow(Form.Handle)) {
                    Console.Error.WriteLine("Failed to register shell hook window.");
                    return;
                }
            }

            //Okey dokey, will now track handles
            TrackHandles(handles);
            _active = true;
        }

        /// <summary>
        /// Initializes the LRU sorted list of window handles.
        /// </summary>
        private void TrackHandles(IList<WindowHandle> handles) {
            _lruHandles = new List<WindowHandleWrapper>(handles.Count);
            var now = DateTime.Now;

            foreach(var h in handles){
                _lruHandles.Add(new WindowHandleWrapper {
                    WindowHandle = h,
                    LastTimeUsed = now
                });
            }
        }

        /// <summary>
        /// Disables group switch mode.
        /// </summary>
        public void Disable() {
            if (!_active)
                return;

            if (!HookMethods.DeregisterShellHookWindow(Form.Handle))
                Console.Error.WriteLine("Failed to deregister shell hook window.");

            _lruHandles = null;
            _active = false;
        }

        /// <summary>
        /// Processes the message pump.
        /// </summary>
        public override void Process(Message msg) {
            if (_active && msg.Msg == _hookMsgId) {
                int hookCode = msg.WParam.ToInt32();
                if (hookCode == HookMethods.HSHELL_WINDOWACTIVATED ||
                    hookCode == HookMethods.HSHELL_RUDEAPPACTIVATED) {
         
                    IntPtr activeHandle = msg.LParam;
                    HandleForegroundWindowChange(activeHandle);
                }
            }
        }

        private void HandleForegroundWindowChange(IntPtr activeWindow) {
#if DEBUG
            Console.Write("New active window (h {0}). ", activeWindow);
#endif

            //Seek window in tracked handles
            WindowHandleWrapper activated = null;
            foreach (var i in _lruHandles) {
                if (i.WindowHandle.Handle == activeWindow)
                    activated = i;
            }

            if (activated == null) {
#if DEBUG
                //new foreground window is not tracked
                Console.WriteLine("Active window is not tracked.");
#endif
                return;
            }

            //Update tracked handle
            activated.LastTimeUsed = DateTime.Now;
            _lruHandles.Sort(new LruDateTimeComparer());

            //Get least recently used
            var next = _lruHandles[0];

#if DEBUG
            Console.WriteLine("Tracked. Switching to {0} (last use: {1}).", next.WindowHandle.Title, next.LastTimeUsed);
#endif

            Form.SetThumbnail(next.WindowHandle, null);
        }

        protected override void Shutdown() {
            Disable();
        }

        /// <summary>
        /// Gets whether the group switch manager ia active.
        /// </summary>
        public bool IsActive {
            get {
                return _active;
            }
        }

        #region List sorting stuff

        class WindowHandleWrapper {
            public WindowHandle WindowHandle { get; set; }
            public DateTime LastTimeUsed { get; set; }
        }

        class LruDateTimeComparer : IComparer<WindowHandleWrapper> {

            #region IComparer<WindowHandleWrapper> Members

            public int Compare(WindowHandleWrapper x, WindowHandleWrapper y) {
                return x.LastTimeUsed.CompareTo(y.LastTimeUsed);
            }

            #endregion
        }

        #endregion

    }

}
