result.StartBlock("Imports LazyFramework");
result.StartBlock("Namespace Entities");
result.StartBlock("Partial Class {0}", [data.Table.Name])
result.WriteLine("Inherits LazyBaseclass")

//Field property
result.WriteReadOnlyProp("Fields", "String()", "{" + _.map(data.Table.Columns, function (o) { return '"' + o.Name + '"' }).join(',') + "}", "Overrides");

//Private variables
_.each(data.Table.Columns, function(o) {
    result.WriteFormatLine("Private _{0} as {1}{2}", [o.ParamName, o.NetRuntimeType, o.Nullable && o.IsValueType ? '?' : '']);
});

//Properies
_.each(data.Table.Columns, function (o) {
    result.StartBlock("Public Property {0} As {1}{2}", [o.Name, o.NetRuntimeType, o.Nullable && o.IsValueType ? '?' : '']);
    result.StartBlock("Get");
    result.WriteFormatLine("Return _{0}", [o.ParamName]);
    result.EndBlock("End Get");
    result.StartBlock("Set(value as {0}{1})", [o.NetRuntimeType, o.Nullable && o.IsValueType ? '?' : '']);

    if (o.Nullable && o.IsValueType) {
        result.WriteFormatLine("If _{0} Is Nothing AndAlso value Is Nothing Then Return", [o.ParamName]);
        result.StartBlock("If _{0} <> value OrElse (_{0} <> value) Is Nothing Then", [o.ParamName]);
    } else {
        result.StartBlock("if _{0} <> value Then", [o.ParamName]);
    }
    result.WriteLine("Dirty = True");
    result.WriteFormatLine('AddChangedValue("{0}",value)', [o.Name]);
    result.WriteFormatLine("_{0} = value", [o.ParamName]);
    result.EndBlock("End If");
    result.EndBlock("End Set");
    result.EndBlock("End Property");
});

//GetValue
result.StartBlock("Public Overrides Function GetValue(ByVal fieldName As String, ByRef value As Object) As Boolean");
result.StartBlock("Select Case fieldName.ToUpper");
_.each(data.Table.Columns, function (o) {
    result.StartBlock('Case "{0}"', [o.NameToUpper]);

    if (o.Nullable && o.IsValueType) {
        result.WriteFormatLine("If _{0} IsNot Nothing AndAlso _{0}.HasValue Then", [o.ParamName]);
        result.PushTab();
        result.WriteFormatLine("value = _{0}.Value", [o.ParamName]);
        result.PopTab();
        result.WriteLine("Else");
        result.PushTab();
        result.WriteLine("value = DBNull.Value");
        result.PopTab();
        result.WriteLine("End If");
    }
    else {
        if (o.IsValueType) {
            result.WriteFormatLine("value = _{0}", [o.ParamName]);
        } else {
            result.StartBlock("If _{0} IsNot nothing then", [o.ParamName]);
            result.WriteFormatLine("value = _{0}", [o.ParamName]);
            result.EndBlock("Else");
            result.StartBlock("value = DbNull.value");
            result.EndBlock("End If");
        }
    }

    result.EndBlock("");
});
result.StartBlock("Case Else");
result.WriteLine("Return False");
result.EndBlock("End Select");
result.WriteLine("Return True");
result.EndBlock("End Function");

//SetValue
result.StartBlock("Public Overrides Function SetValue(ByVal fieldName As String, ByVal value As Object) As Boolean");
result.StartBlock("If Not value Is Nothing And Not IsDBNull(value) Then");
result.StartBlock("Select Case fieldName.ToUpper");

_.each(data.Table.Columns, function (o) {
    result.StartBlock('Case "{0}"', [o.NameToUpper]);
    result.WriteFormatLine("_{0} = Ctype(value,{1}{2})", [o.ParamName, o.NetRuntimeType, o.Nullable && o.ValueType ? '?' : '']);
    result.EndBlock("");
});
result.StartBlock("Case Else");
result.WriteLine("Return False");
result.EndBlock("");

result.EndBlock("End Select");
result.WriteLine("Return True");
result.EndBlock("End If");
result.WriteLine("Return True");
result.EndBlock("End Function");

result.EndBlock("End Class");
result.EndBlock("End Namespace");


