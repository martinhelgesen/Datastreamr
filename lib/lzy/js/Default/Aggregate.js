result.StartBlock("Namespace Aggregates");
result.StartBlock("Friend Class {0}Aggregate", [data.TableName]);

result.Comment("Skriv dine egene methoder for å hente data.. ");

result.Comment("Validering");
_.each(["ValidateCreateEntity", "ValidateUpdateEntity", "ValidateDeleteEntity"], function (e) {
    result.StartBlock("Private Sub {0}(o as Entities.{1})", [e, data.TableName]);
    result.EndBlock("End Sub");
});

result.EndBlock("End Class");
result.EndBlock("End Namespace");

