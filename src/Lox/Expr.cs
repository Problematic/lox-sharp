namespace Lox
{
  public abstract class Expr
  {
    public interface Visitor<R>
    {
      R VisitBinaryExpr(Binary expr);
      R VisitGroupingExpr(Grouping expr);
      R VisitLiteralExpr(Literal expr);
      R VisitUnaryExpr(Unary expr);
    }

    public class Binary : Expr
    {
      public readonly Expr left;
      public readonly Token op;
      public readonly Expr right;

      public Binary(Expr left, Token op, Expr right)
      {
        this.left = left;
        this.op = op;
        this.right = right;
      }

      public override R Accept<R>(Visitor<R> visitor)
      {
        return visitor.VisitBinaryExpr(this);
      }
    }

    public class Grouping : Expr
    {
      public readonly Expr expression;

      public Grouping(Expr expression)
      {
        this.expression = expression;
      }

      public override R Accept<R>(Visitor<R> visitor)
      {
        return visitor.VisitGroupingExpr(this);
      }
    }

    public class Literal : Expr
    {
      public readonly object value;

      public Literal(object value)
      {
        this.value = value;
      }

      public override R Accept<R>(Visitor<R> visitor)
      {
        return visitor.VisitLiteralExpr(this);
      }
    }

    public class Unary : Expr
    {
      public readonly Token op;
      public readonly Expr right;

      public Unary(Token op, Expr right)
      {
        this.op = op;
        this.right = right;
      }

      public override R Accept<R>(Visitor<R> visitor)
      {
        return visitor.VisitUnaryExpr(this);
      }
    }

    public abstract R Accept<R>(Visitor<R> visitor);
  }
}
