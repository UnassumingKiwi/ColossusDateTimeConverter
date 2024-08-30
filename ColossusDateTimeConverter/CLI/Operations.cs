using System.Reflection;

namespace ColossusDateTimeConverter.CLI;

public static class Operations
{
    private static Dictionary<string, IOperation> operations = new Dictionary<string, IOperation>();

    public static List<IOperation> GetOperations()
    {
        return operations.Values.ToList();
    }

    public static List<IOperation> GetAll()
    {
        return operations.Values.ToList();
    }

    public static IOperation GetOperation(string name)
    {
        return operations[name];
    }

    public static bool TryGetOperation(string name, out IOperation? operation)
    {
        return operations.TryGetValue(name, out operation);
    }

    public static void Execute(string name, params string[] arguments)
    {
        IOperation op;

        try
        {
            op = GetOperation(name);
        }
        catch
        {
            Console.WriteLine($"The operation '{name}' does not exist.");
            return;
        }

        op.Execute(arguments);
    }

    public static void Add(IOperation operation)
    {
        operations.Add(operation.Name, operation);
    }

    public static void AddAll()
    {
        var assembly = Assembly.GetAssembly(typeof(Operations));
        if (assembly is null)
            throw new ApplicationException("Assembly not found.");

        var types = assembly.GetTypes()
            .Where(t => typeof(IOperation).IsAssignableFrom(t))
            .Where(t => !t.IsInterface && !t.IsAbstract)
            .ToList();

        foreach (var type in types)
        {
            var instance = (IOperation)Activator.CreateInstance(type);
            if (instance is null)
                throw new ApplicationException($"Could not create instance of {type.FullName}.");

            Add(instance);
        }
    }
}
