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
            
            DefineVisitor(writer, baseName, types);
            
            writer.WriteLine();
            writer.WriteLine("   public abstract T Accept<T>(IVisitor<T> visitor);");
            
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
        writer.WriteLine();
        writer.WriteLine($"      public override T Accept<T>(IVisitor<T> visitor) => visitor.Visit{className}{baseName}(this);");
        
        writer.WriteLine("   }");
        writer.WriteLine();
    }

    private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
    {
        writer.WriteLine("   public interface IVisitor<T> {");
        
        foreach(var type in types)
        {
            var typeName = type.Split(":")[0].Trim();
            writer.WriteLine($"   T Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
        }
        
        writer.WriteLine("  }");
    }
}