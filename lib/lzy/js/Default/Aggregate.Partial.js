result.StartBlock("Imports LazyFramework");
result.StartBlock("Namespace Aggregates");
result.StartBlock("Partial Class {0}Aggregate", [data.TableName]);
result.WriteFormatLine("Inherits CreditCardBaseAggregate(Of {0}, {1})", ["DataAccess.I" + data.TableName, "DataAccess." + data.TableName]);
result.WriteLine("");

//GetInstance
result.Write("Public Function GetInstance(");
_.each(data.Table.Columns,function(c,i) {
    if(c.Identity) {
        if(i)result.write(",");
        result.WriteFormat("{0}{1} As {2}",[Utils.toParamName(data.TableName),c.Name,c.NetRuntimeType]);
    }
});
result.StartBlock(") As Entities.{0}",[data.TableName]);

result.WriteFormatLine("Dim retObj As New Entities.{0}", [data.TableName]);
_.each(data.Table.Columns, function (c, i) {
    if(c.Identity) result.WriteFormatLine("retObj.{1} = {0}{1}", [Utils.toParamName(data.TableName), c.Name]);
});

result.WriteComment("Check if me.CurrentUser is allowed to this");

result.WriteLine("Repository.GetEntity(Me.DbName, retObj)");

result.WriteLine("Return retObj ");

result.EndBlock("End Function");

//GetAllInstances
result.StartBlock("Public Function GetAllInstances() As Entities.{0}Collection",[data.TableName]);
result.WriteFormatLine("Dim retObj As New Entities.{0}Collection",[data.TableName]);
result.WriteComment("Check if me.CurrentUser is allowed to this");

result.WriteLine("Repository.GetAll(Me.DbName, retObj)");
result.WriteLine("Return retObj");
result.EndBlock("End Function");

//CreateInstance
result.StartBlock("Public Function CreateInstance(ByVal o As Entities.{0}) As Boolean",[data.TableName]);
result.Comment("Validate the object");
result.Comment("Check if me.CurrentUser is allowed to this");
result.Comment("Setting default values for the object");
_.each(data.Table.Columns, function (c, i) {
    if(c.DefaultValue) result.WriteFormatLine('SetDefaultValue(o, "{0}",{1} )',[c.Name,c.DefaultValueCode]);
});
result.WriteLine("ValidateCreateEntity(o)");

result.WriteLine("Return Repository.Create(Me.DbName, o)");
result.EndBlock("End Function");


//UpdateInstance
result.StartBlock("Public Function UpdateInstance(ByVal o As Entities.{0}) As Boolean",[data.TableName]);
result.Comment("Check if me.CurrentUser is allowed to this");
result.Comment("Validate the object");
result.WriteLine("Me.ValidateUpdateEntity(o)");


result.WriteLine("Return Repository.Update(DbName, o)");
result.EndBlock("End Function");

//Delete Instance
result.StartBlock("Public Function DeleteInstance(ByVal o As Entities.{0} ) as Boolean", [data.TableName]);
result.Comment("Check if me.CurrentUser is allowed to this");
result.Comment("Validate the object");
result.WriteLine("Me.ValidateDeleteEntity(o)");

result.WriteLine("Return Repository.Delete(Me.DbName, o)");
result.EndBlock("End Function");

result.EndBlock("End Class");
result.EndBlock("End Namespace");

