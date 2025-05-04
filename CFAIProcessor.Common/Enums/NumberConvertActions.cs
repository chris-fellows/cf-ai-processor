using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAIProcessor.Enums
{
    public enum NumberConvertActions
    {
        None,
        ModuloRoundDown,      // Modulo round down. E.g. With Modulo 10000 then round 25000 to 20000
        ModuloRoundUp         // Modulo round up. E.g. With Modulo 10000 then round 25000 to 30000
    }
}
