using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineWPF
{
    abstract class AbstractSingleton<T>
    {
        private static T _instance = default(T);

        private static readonly object Locker = new object();

        internal static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (Locker)
                {
                    if (_instance != null)
                        return _instance;
                    return _instance = Activator.CreateInstance<T>();
                }
            }
        }
    }
}
