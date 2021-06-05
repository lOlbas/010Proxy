using System;
using System.Collections.Generic;

namespace _010Proxy.Templates
{
    public abstract class IProtocolConfig
    {
        public abstract Dictionary<Type, List<object>> OpCodesIgnoreList { get; set; }
    }
}
