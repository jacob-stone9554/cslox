namespace cslox;

public class Lox
{
    private static bool hadError = false;
    public static void Main(string[] args)
    {

        switch (args.Length)
        {
            case > 1:
                Console.WriteLine("Usage: cslox <file>");
                Environment.Exit(64);
                break;
            case 1:
                RunFile(args[0]);
                break;
            default:
                RunPrompt();
                break;
        }
    }
    
    private static void RunFile(string path)
    {
        string source = File.ReadAllText(path);
        Run(source);
        
        if(hadError) Environment.Exit(65);
    }

    private static void RunPrompt()
    {
        var line = Console.ReadLine();

        for (;;)
        {
            Console.WriteLine("> ");
            if (line == null) break;
            Run(line);
            hadError = false;
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

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine($"[Line {line}] Error {where}: {message}");
        hadError = true;
    }
}