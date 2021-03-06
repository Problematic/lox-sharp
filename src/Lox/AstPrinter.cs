using System.Text;

namespace Lox
{
  public class AstPrinter : Expr.Visitor<string>
  {
    public string Print(Expr expr)
    {
      return expr.Accept(this);
    }

    public string VisitBinaryExpr(Expr.Binary expr)
    {
      return Parenthesize(expr.op.lexeme, expr.left, expr.right);
    }

    public string VisitGroupingExpr(Expr.Grouping expr)
    {
      return Parenthesize("group", expr.expression);
    }

    public string VisitLiteralExpr(Expr.Literal expr)
    {
      if (expr.value == null) return "nil";
      return expr.value.ToString();
    }

    public string VisitUnaryExpr(Expr.Unary expr)
    {
      return Parenthesize(expr.op.lexeme, expr.right);
    }

    private string Parenthesize(string name, params Expr[] exprs)
    {
      var sb = new StringBuilder();

      sb.Append("(").Append(name);
      foreach (var expr in exprs)
      {
        sb.Append(" ");
        sb.Append(expr.Accept(this));
      }
      sb.Append(")");

      return sb.ToString();
    }
  }
}
