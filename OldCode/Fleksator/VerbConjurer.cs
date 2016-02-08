using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDevs.Fleksator
{
    public class VerbConjurer
    {
        #region Singleton Implementation

        private static volatile VerbConjurer _decliner = null;
        private static object _lockObject = new object();

        public static VerbConjurer Conjurer
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new VerbConjurer();
                    }
                }

                return _decliner;
            }
        }

        private VerbConjurer()
        { }

        #endregion



    }
}
