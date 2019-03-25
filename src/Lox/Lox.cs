using System;
using System.Collections.Generic;

namespace Lox
{
  class Lox
  {
    static readonly Interpreter interpreter = new Interpreter();
    static bool hadError = false;
    static bool hadRuntimeError = true;

    static void Main(string[] args)
    {
      if (args.Length > 1)
      {
        Console.WriteLine("Usage: lox-sharp [script]");
        Environment.Exit(64);
      }
      else if (args.Length == 1)
      {
        throw new Exception("runFile not implemented yet");
        // runFile(args[0]);
      }
      else
      {
        RunPrompt();
      }
    }

    private static void RunPrompt()
    {
      while (true)
      {
        Console.Write("> ");
        Run(Console.ReadLine());
        hadError = false;
        hadRuntimeError = false;
      }
    }

    private static void Run(String source)
    {
      var scanner = new Scanner(source);
      List<Token> tokens = scanner.ScanTokens();
      var parser = new Parser(tokens);
      Expr expr = parser.Parse();

      if (hadError) return;

      interpreter.Interpret(expr);
    }

    public static void Error(Token token, string message)
    {
      if (token.type == TokenType.EOF)
      {
        Report(token.line, " at end", message);
      }
      else
      {
        Report(token.line, " at '" + token.lexeme + "'", message);
      }
    }

    public static void Error(int line, string message)
    {
      Report(line, "", message);
    }

    public static void RuntimeError(RuntimeError error)
    {
      Console.Error.WriteLine(error.Message + "\n[line " + error.token.line + "]");
      hadRuntimeError = true;
    }

    private static void Report(int line, string where, string message)
    {
      hadError = true;
      Console.Error.WriteLine(string.Format("[line {0}] Error{1}: {2}", line, where, message));
    }
  }
}
