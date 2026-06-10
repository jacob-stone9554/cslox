namespace Tooling;

public class GenerateAst
{
    public static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Usage: generate_ast <output directory>");
            Environment.Exit(64);
        }
        
        var outputDirectory = args[0];
        List<string> types = new List<string>();
        
        types.Add("Binary   : Expr left, Token op, Expr right");
        types.Add("Grouping : Expr expression");
        types.Add("Literal  : object value");
        types.Add("Unary    : Token op, Expr right");
        
        DefineAst(outputDirectory, "Expr", types);
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        var path = outputDir + Path.DirectorySeparatorChar + baseName + ".cs";
        using (var writer = new StreamWriter(path))
        {
            writer.WriteLine("namespace cslox;");
            writer.WriteLine($"public abstract class {baseName}");
            writer.WriteLine("{");
            writer.WriteLine("}");
            writer.WriteLine();

            foreach (var type in types)
            {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                
                DefineType(writer, baseName, className, fields);
            }
        }
    }

    private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
    {
        writer.WriteLine($"     public class {className} : {baseName}");
        writer.WriteLine("      {");
        
        string[] fields = fieldList.Split(",");

        foreach (var field in fields)
        {
            writer.WriteLine($"     public readonly {field.Trim()};");
        }
        
        writer.WriteLine();
        
        writer.WriteLine($"     public {className}({fieldList})");
        writer.WriteLine("      {");

        foreach (var field in fields)
        {
            string[] parts = field.Trim().Split(' ');
            string name = parts[1];
            writer.WriteLine($"         this.{name} = {name};");
        }
        
        writer.WriteLine("          }");
        writer.WriteLine("      }");
        writer.WriteLine();
    }
}