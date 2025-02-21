using NLog.Common;
using System;

namespace Infrastructure.Services.Logging
{
    public class Logger
    {
        private static object _lock = new object();
        private static NLog.Logger _shadowNLogger;
        private static NLog.Logger _nLogger
        {
            get
            {
                lock (_lock)
                {
                    if (_shadowNLogger == null)
                    {
                        _shadowNLogger = NLog.LogManager.GetCurrentClassLogger();

                        InternalLogger.LogFile = "nlog-start.log";
                        InternalLogger.LogLevel = NLog.LogLevel.Trace;
                    }

                    return _shadowNLogger;
                }
            }
        }

        public enum Levels : int
        {
            Trace = 0,
            Debug = 1,
            Info = 2,
            Warn = 3,
            Error = 4,
            Fatal = 5
        }

        public static void Log(Levels level,
            string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            _nLogger.Log(getNLogLevel(level),
                message
                + " >--> " + memberName
                + " >--> " + sourceFilePath
                + " >--> " + sourceLineNumber);
        }

        public static void Log(Exception exception,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(Levels.Error,
                "Exception: " + exception?.Message,
                memberName,
                sourceFilePath,
                sourceLineNumber);
        }

        public static void LogDebug(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(Levels.Debug,
                message
                + " >--> " + memberName
                + " >--> " + sourceFilePath
                + " >--> " + sourceLineNumber);
        }
        public static void LogTrace(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(Levels.Trace,
                message
                + " >--> " + memberName
                + " >--> " + sourceFilePath
                + " >--> " + sourceLineNumber);
        }
        public static void Log(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(Levels.Trace,
                message,
                memberName,
                sourceFilePath,
                sourceLineNumber);
        }
        public static void LogInfo(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(Levels.Info,
                message
                + " >--> " + memberName
                + " >--> " + sourceFilePath
                + " >--> " + sourceLineNumber);
        }
        public static void LogWarning(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(Levels.Warn,
                message
                + " >--> " + memberName
                + " >--> " + sourceFilePath
                + " >--> " + sourceLineNumber);
        }
        public static void LogError(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(Levels.Error,
                message
                + " >--> " + memberName
                + " >--> " + sourceFilePath
                + " >--> " + sourceLineNumber);
        }
        public static void LogFatal(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            Log(Levels.Fatal,
                message
                + " >--> " + memberName
                + " >--> " + sourceFilePath
                + " >--> " + sourceLineNumber);
        }

        #region > Helpers
        private static NLog.LogLevel getNLogLevel(Levels level)
        {
            switch (level)
            {
                default:
                case Levels.Trace:
                    return NLog.LogLevel.Trace;

                case Levels.Debug:
                    return NLog.LogLevel.Debug;

                case Levels.Info:
                    return NLog.LogLevel.Info;

                case Levels.Warn:
                    return NLog.LogLevel.Warn;

                case Levels.Error:
                    return NLog.LogLevel.Error;

                case Levels.Fatal:
                    return NLog.LogLevel.Fatal;
            }
        }
        #endregion
    }
}
