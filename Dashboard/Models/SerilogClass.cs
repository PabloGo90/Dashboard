using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{
    public static class SerilogClass
    {
        public static readonly Serilog.ILogger _log;
        static SerilogClass()
        {
            _log = new LoggerConfiguration().
                    ReadFrom.AppSettings().
                    CreateLogger();
        }
    }
}