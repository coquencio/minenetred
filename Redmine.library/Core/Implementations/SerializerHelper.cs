using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Redmine.Library.Core
{
    public class SerializerHelper : ISerializerHelper
    {
        private JsonSerializerSettings _serializerSettings;
        public SerializerHelper()
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            var settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
            _serializerSettings = settings;
        }
        public JsonSerializerSettings SerializerSettings()
        {
            return _serializerSettings;
        }
    }
}