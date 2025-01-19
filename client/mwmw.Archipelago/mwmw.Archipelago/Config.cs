using System.Text.Json.Serialization;

namespace mwmw.Archipelago;

public class Config {
    [JsonInclude] public bool SomeSetting = true;
}
