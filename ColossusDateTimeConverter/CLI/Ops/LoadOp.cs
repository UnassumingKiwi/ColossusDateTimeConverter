using ColossusDateTimeConverter.PersistenceObjects;
using YamlDotNet.Serialization;

namespace ColossusDateTimeConverter.CLI.Ops;

public class LoadOp : IOperation
{
    public string Name { get; } = "load";
    public string Usage { get; } = "load culture {path}";
    public string Description { get; } = "Loads a culture from a YAML file.";

    private IDeserializer deserializer;

    public LoadOp()
    {
        deserializer = new DeserializerBuilder().Build();
    }

    public void Execute(params string[] arguments)
    {
        if (arguments.Length < 2)
        {
            Operations.Execute("print", "Usage:", Usage);
            return;
        }

        var type = arguments[0];
        var path = arguments[1];

        string yaml;
        try
        {
            yaml = File.ReadAllText(path);
        } catch(Exception e)
        {
            Operations.Execute("print", "FILE EXCEPTION:", e.Message);
            return;
        }

        switch (type)
        {
            case ("culture"): LoadCulture(yaml); break;
            default: Operations.Execute("print", $"Cannot load {type}."); break;
        }
    }

    private void LoadCulture(string yaml)
    {
        var culture = deserializer.Deserialize<CulturePersistanceObject>(yaml);
        Cultures.AddCulture(culture.ToCulture());

        Operations.Execute("print", culture.Name, "loaded successfully.");
    }
}
