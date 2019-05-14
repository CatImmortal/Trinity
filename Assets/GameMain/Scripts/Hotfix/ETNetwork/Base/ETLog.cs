using System;

namespace Trinity.Hotfix
{
    public static class ETLog
    {
        public static void Trace(string msg)
        {
            ETModel.ETLog.Trace(msg);
        }

        public static void Warning(string msg)
        {
            ETModel.ETLog.Warning(msg);
        }

        public static void Info(string msg)
        {
            ETModel.ETLog.Info(msg);
        }

        public static void Error(Exception e)
        {
            ETModel.ETLog.Error(e.ToStr());
        }

        public static void Error(string msg)
        {
            ETModel.ETLog.Error(msg);
        }

        public static void Debug(string msg)
        {
            ETModel.ETLog.Debug(msg);
        }

        public static void Trace(string message, params object[] args)
        {
            ETModel.ETLog.Trace(message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            ETModel.ETLog.Warning(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            ETModel.ETLog.Info(message, args);
        }

        public static void Debug(string message, params object[] args)
        {
            ETModel.ETLog.Debug(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            ETModel.ETLog.Error(message, args);
        }

        public static void Fatal(string message, params object[] args)
        {
            ETModel.ETLog.Fatal(message, args);
        }

        public static void Msg(object msg)
        {
            Debug(Dumper.DumpAsString(msg));
        }
    }
}