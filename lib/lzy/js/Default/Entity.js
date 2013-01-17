result.StartBlock("Namespace Entities");
result.StartBlock("Public Class {0}", [data.Table.Name]);
result.WriteComment("Har kan du legge inn dine spessielle ting for denne klassen");

result.EndBlock("End Class");
result.EndBlock("End Namespace");

