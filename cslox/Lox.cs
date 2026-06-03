namespace cslox;

public class Lox
{
    public static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: cslox [script]");
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        
    }
    
    private static void RunFile(string path)
    {
        string source = File.ReadAllText(path);
        //Run(source);
    }

    private static void RunPrompt()
    {
        string line = Console.ReadLine();

        for (;;)
        {
            Console.WriteLine("> ");
            if (line == null) break;
            // Run(line);
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }
}