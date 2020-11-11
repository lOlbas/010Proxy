using System;
using System.Reflection;

namespace _010Proxy.Utils
{
    public class Singleton<T> where T : class
    {
        private static volatile T _instance;
        private static object _syncRoot = new object();

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            ConstructorInfo constructorInfo = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                            _instance = (T)constructorInfo.Invoke(new object[0]);
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
