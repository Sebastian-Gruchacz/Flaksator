using System;
using System.Collections.Generic;
using System.Text;
using ObscureWare.Randomization;

// Author: gruchs01 
// Date Creation: 8/26/2008 11:54:58 AM
// Description: 

namespace SharpDevs.Randomization
{
    public enum RandomizationEngine { System, MersenneTwister };

    public sealed class Rnd //: IDebugLog
    {
        #region Singleton Implementation

        private static object _lockObj = new object(); // lock object
        private static volatile Rnd _instance; // instance

        /// <summary>
        /// The one instance of Rnd singleton 
        /// </summary>
        public static Rnd Instance
        {
            get
            {
                // do safe create singleton instance of the object
                if (_instance == null)
                    lock (_lockObj)
                    {
                        if (_instance == null)
                            _instance = new Rnd();
                    }

                return _instance;
            }
        }

        // Private hidden ctor
        private Rnd()
        {
            InitializeInstance(); // this is done to put init code outside singleton region
        }

        #endregion

        private void InitializeInstance()
        {
            // Please provide here Singleton initialization code
            DateTime dt = DateTime.Now;

            systemEngine = new SystemRandomWrapper(dt.Millisecond);
            twister = new MersenneTwister(dt.Millisecond);


            // by default
            selectedEngine = twister;
        }

        SystemRandomWrapper systemEngine = null;
        MersenneTwister twister = null;

        IRandomizer selectedEngine = null;

        public void SelectEngine(RandomizationEngine engine)
        {
            switch (engine)
            {
                case RandomizationEngine.System:
                    selectedEngine = systemEngine;

                    //LogDebug("Selected Random engine is System's one.");

                    break;
                case RandomizationEngine.MersenneTwister:
                    selectedEngine = twister;

                    //LogDebug("Selected Random engine is Mersenne Twister.");

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set your own engine to use in app
        /// </summary>
        /// <param name="engine"></param>
        public void SetCustomRandomizer(IRandomizer engine)
        {
            if (engine != null)
            {
                selectedEngine = engine;

                //LogDebug(string.Format("Selected Random engine is custom engine: {0}", engine.GetType().FullName));
            }
        }

        public IRandomizer Engine
        {
            get
            {
                return selectedEngine;
            }
        }

        #region IDebugLog Members

        //internal void LogDebug(string errMsg) // these are internal, because can be used by Textlibrary also...
        //{
        //    if (LogDebugMessage != null)
        //        LogDebugMessage(errMsg);
        //}

        //internal void LogError(string errMsg, ErrorSeverity severity) // these are internal, because can be used by Textlibrary also...
        //{
        //    if (LogErrorMessage != null)
        //        LogErrorMessage(errMsg, severity);
        //}

        //public event SharpDevs.Debugging.InvocationOfString LogDebugMessage;

        //public event SharpDevs.Debugging.InvocationOfSeverityString LogErrorMessage;

        #endregion
    }
}
