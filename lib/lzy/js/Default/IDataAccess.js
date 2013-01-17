//
result.StartBlock("Namespace DataAccess",[]);
result.StartBlock("Public Interface I{0}",[data.TableName]);

result.WriteFormatLine("Function GetEntity(ByVal sourceName As String, ByRef o As Entities.{0}) As Boolean", [data.TableName]);
result.WriteFormatLine("Function GetAll(ByVal sourceName As String, ByRef o As Entities.{0}Collection) As Boolean", [data.TableName]);
result.WriteFormatLine("Function Create(ByVal sourceName As String, ByRef o As Entities.{0}) As Boolean", [data.TableName]);
result.WriteFormatLine("Function Update(ByVal sourceName As String, ByRef o As Entities.{0}) As Boolean", [data.TableName]);
result.WriteFormatLine("Function Delete(ByVal sourceName As String, ByRef o As Entities.{0}) As Boolean", [data.TableName]);

result.EndBlock("End Interface");
result.EndBlock("End Namespace");

