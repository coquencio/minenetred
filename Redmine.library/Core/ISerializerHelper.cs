using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redmine.Library.Core
{
    public interface  ISerializerHelper
    {
        JsonSerializerSettings SerializerSettings();
    }
}
