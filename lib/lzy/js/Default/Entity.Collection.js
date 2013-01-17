result.StartBlock("Imports LazyFramework");
result.StartBlock("Namespace Entities");
result.StartBlock("Public Class {0}Collection", [data.TableName]);
result.WriteFormatLine("Inherits LazyList(Of {0})", [data.TableName]);
result.Comment("Basic list for your object");
result.EndBlock("End Class");
result.EndBlock("End Namespace");

