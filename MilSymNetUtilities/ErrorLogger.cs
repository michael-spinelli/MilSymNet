using System;
using System.Diagnostics;

namespace MilSymNetUtilities

{
    public class ErrorLogger
    {
        

        //private static String logPath = null;

        private static ErrorLogger _instance = null;
        private static object syncLock = new object();

        private static ErrorLogger getInstance()
        {
            if (_instance == null)
            {
                lock (syncLock)
                {
                    if (_instance == null)
                        _instance = new ErrorLogger();
                }
            }

            return _instance;
        }

        /// <summary>
        /// LogMessage - Logs custom error message
        /// </summary>
        /// <param name="className"></param>
        /// <param name="functionName"></param>
        /// <param name="message"></param>
        public static void LogMessage(String className, String functionName, String message)
        {
            if (_instance == null)
                _instance = getInstance();

            Debug.WriteLine(className + " - " + functionName + ":");
            Debug.WriteLine(message);
        }


        /// <summary>
        /// LogException
        /// </summary>
        /// <param name="className"></param>
        /// <param name="functionName"></param>
        /// <param name="exc"></param>
        public static void LogException(String className, String functionName, Exception exc)
        {
            LogException(className, functionName, exc, ErrorLevel.INFO);
        }

        /// <summary>
        /// LogException - Logs program error
        /// </summary>
        /// <param name="className"></param>
        /// <param name="functionName"></param>
        /// <param name="exc"></param>
        /// <param name="ErrorLevel"></param>
        public static void LogException(String className, String functionName, Exception exc, int ErrorLevel)
        {
            if (_instance == null)
                _instance = getInstance();

            Debug.WriteLine(exc.Message);
            Debug.WriteLine(exc.StackTrace);
        }
    }

    /// <summary>
    /// ErrorLevel
    /// </summary>
    public class ErrorLevel
    {
        public static int OFF = int.MaxValue;
        public static int SEVERE = 1000;
        public static int WARNING = 900;
        public static int INFO = 800;
        public static int CONFIG = 700;
        public static int FINE = 500;
        public static int FINER = 400;
        public static int FINEST = 300;
        public static int ALL = int.MinValue;
    }
}
