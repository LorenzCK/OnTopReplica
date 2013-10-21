using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OnTopReplica.MessagePumpProcessors;
using OnTopReplica.Native;

namespace OnTopReplica {
    class MessagePumpManager : IDisposable {

        Dictionary<Type, IMessagePumpProcessor> _processors = new Dictionary<Type, IMessagePumpProcessor>();

        public MainForm Form { get; private set; }

        private void Register(IMessagePumpProcessor processor, MainForm form) {
            _processors[processor.GetType()] = processor;
            processor.Initialize(form);

            Log.Write("Registered message pump processor: {0}", processor.GetType());
        }

        /// <summary>
        /// Instantiates all message pump processors and registers them on the main form.
        /// </summary>
        /// <param name="form"></param>
        public void Initialize(MainForm form) {
            Form = form;

            //Register window shell hook
            if (!HookMethods.RegisterShellHookWindow(form.Handle)) {
                Log.Write("Failed to register shell hook window");
            }
            else {
                Log.Write("Shell hook window registered successfully");
            }

            //Register message pump processors
            Register(new WindowKeeper(), form);
            Register(new HotKeyManager(), form);
            Register(new GroupSwitchManager(), form);
        }

        /// <summary>
        /// Run the registered message pump processors.
        /// </summary>
        /// <param name="msg">Message to process.</param>
        /// <returns>True if the message has been handled internally.</returns>
        public bool PumpMessage(ref Message msg) {
            foreach (var processor in _processors.Values) {
                if (processor.Process(ref msg))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get the instance of a registered message pump processor.
        /// Throws if instance not found.
        /// </summary>
        public T Get<T>() {
            return (T)_processors[typeof(T)];
        }

        #region IDisposable Members

        public void Dispose() {
            if (!HookMethods.DeregisterShellHookWindow(Form.Handle)) {
                Log.Write("Failed to deregister shell hook window");
            }
            else {
                Log.Write("Deregistered shell hook window successfully");
            }

            foreach (var processor in _processors.Values) {
                processor.Dispose();
            }
            _processors.Clear();
        }

        #endregion

    }

}
