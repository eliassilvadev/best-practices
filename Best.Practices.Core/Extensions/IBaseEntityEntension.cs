using Best.Practices.Core.Configurations.JsonSerializar;
using Best.Practices.Core.Domain.Models.Interfaces;
using Newtonsoft.Json;

namespace Best.Practices.Core.Extensions
{
    public static class IBaseEntityEntension
    {
        public static IBaseEntity DeepClone(this IBaseEntity self)
        {
            var serialized = JsonConvert.SerializeObject(
                self,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return (IBaseEntity)JsonConvert.DeserializeObject(
                serialized,
                self.GetType(),
                new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
    }
}