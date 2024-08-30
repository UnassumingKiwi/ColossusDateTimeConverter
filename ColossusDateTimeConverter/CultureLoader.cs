using ColossusDateTimeConverter.PersistenceObjects;
using YamlDotNet.Serialization;

namespace ColossusDateTimeConverter;

public class CultureLoader
{
    public Culture Load(string path)
    {
        var yaml = File.ReadAllText(path);

        var deserializer = new DeserializerBuilder().Build();
        var culturePO = deserializer.Deserialize<CulturePersistanceObject>(yaml);

        return culturePO.ToCulture();
    }
}
