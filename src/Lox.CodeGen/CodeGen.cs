﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Lox.CodeGen
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length != 1)
      {
        Console.Error.WriteLine("Usage: code_gen <output directory>");
        Environment.Exit(1);
      }

      var outputDir = args[0];
      DefineAst(outputDir, "Expr", new List<string> {
        "Binary   : Expr left, Token op, Expr right",
        "Grouping : Expr expression",
        "Literal  : object value",
        "Unary    : Token op, Expr right",
      });
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
      using (var writer = new StreamWriter(outputDir + "/" + baseName + ".cs"))
      {
        writer.WriteLine("namespace Lox");
        writer.WriteLine("{");
        writer.WriteLine("  public abstract class " + baseName);
        writer.WriteLine("  {");

        DefineVisitor(writer, baseName, types);

        foreach (var type in types)
        {
          var className = type.Split(':')[0].Trim();
          var fields = type.Split(':')[1].Trim();

          DefineType(writer, baseName, className, fields);
        }

        writer.WriteLine("    public abstract R Accept<R>(Visitor<R> visitor);");

        writer.WriteLine("  }");
        writer.WriteLine("}");
      }
    }

    private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
    {
      writer.WriteLine("    public interface Visitor<R>");
      writer.WriteLine("    {");

      foreach (var type in types)
      {
        var typeName = type.Split(':')[0].Trim();
        writer.WriteLine("      R Visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
      }

      writer.WriteLine("    }");
      writer.WriteLine();
    }

    private static void DefineType(StreamWriter writer, string baseName, string className, string fieldList)
    {
      var fields = fieldList.Split(", ");

      writer.WriteLine("    public class " + className + " : " + baseName);
      writer.WriteLine("    {");
      foreach (var field in fields)
      {
        writer.WriteLine("      public readonly " + field + ";");
      }
      writer.WriteLine();
      writer.WriteLine("      public " + className + "(" + fieldList + ")");
      writer.WriteLine("      {");
      foreach (var field in fields)
      {
        var name = field.Split(" ")[1];
        writer.WriteLine("        this." + name + " = " + name + ";");
      }
      writer.WriteLine("      }");
      writer.WriteLine();
      writer.WriteLine("      public override R Accept<R>(Visitor<R> visitor)");
      writer.WriteLine("      {");
      writer.WriteLine("        return visitor.Visit" + className + baseName + "(this);");
      writer.WriteLine("      }");
      writer.WriteLine("    }");
      writer.WriteLine();
    }
  }
}
