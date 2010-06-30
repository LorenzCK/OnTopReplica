using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace OnTopReplica {
    class MessagePumpManager : IDisposable {

        Dictionary<Type, IMessagePumpProcessor> _processors = new Dictionary<Type, IMessagePumpProcessor>();

        /// <summary>
        /// Instantiates all message pump processors and registers them on the main form.
        /// </summary>
        /// <param name="form"></param>
        public void Initialize(MainForm form) {
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes()) {
                if (typeof(IMessagePumpProcessor).IsAssignableFrom(t) && !t.IsAbstract) {
                    var instance = (IMessagePumpProcessor)Activator.CreateInstance(t);
                    instance.Initialize(form);

                    _processors.Add(t, instance);
                    
                    Console.WriteLine("Registered message pump processor: {0}", t);
                }
            }
        }

        /// <summary>
        /// Run the registered message pump processors.
        /// </summary>
        /// <param name="msg">Message to process.</param>
        public void PumpMessage(Message msg) {
            foreach (var processor in _processors.Values) {
                processor.Process(msg);
            }
        }

        /// <summary>
        /// Get the instance of a registered message pump processor.
        /// Throw if instance not found.
        /// </summary>
        public T Get<T>() {
            return (T)_processors[typeof(T)];
        }

        #region IDisposable Members

        public void Dispose() {
            foreach (var processor in _processors.Values) {
                processor.Dispose();
            }
            _processors.Clear();
        }

        #endregion
    }
}
