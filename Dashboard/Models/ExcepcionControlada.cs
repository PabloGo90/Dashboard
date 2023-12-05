using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard.Models
{

    public class ExcepcionControlada : Exception
    {
        public ExcepcionControlada(string message) : base(message)
        {
        }
    }
    public class ExcepcionCambioPassWord : Exception
    {
        public ExcepcionCambioPassWord(string message) : base(message)
        {
        }
    }
    public class ExcepcionFinSession : Exception
    {
        public ExcepcionFinSession(string message) : base(message)
        {
        }
    }

}