Imports NUnit.Framework

<TestFixture> Public Class MappingTest

    <Test> Public Sub Mapping_WithoutRules()
        'Arrange
        Dim container As New DataContainer With {
                                        .Data = New List(Of Dictionary(Of String, Object)),
                                        .MetaData = New List(Of PropertyDesc)
                                        }
        Dim dic = New Dictionary(Of String, Object) From {
                                        {"A", 1},
                                        {"B", 2},
                                        {"C", 3}
                                        }
        container.Data.Add(dic)
        Dim mapcol As New List(Of MapInfo) From {
            New MapInfo With {.FromName = "A", .ToName = "AA"},
            New MapInfo With {.FromName = "B", .ToName = "BB"}
            }

        'Act
        Dim newContainer = Mapper.Map(container, mapcol)
        Dim newData = newContainer.Data

        'Assert
        Assert.AreEqual(1, newData.Count)
        Assert.AreEqual(2, newData(0).Count)
        Assert.AreEqual(1, newData(0)("AA"))
        Assert.AreEqual(2, newData(0)("BB"))
    End Sub

    <Test> Public Sub Mapping_WithRules()
        'Arrange
        Dim container As New DataContainer With {
                                        .Data = New List(Of Dictionary(Of String, Object)),
                                        .MetaData = New List(Of PropertyDesc)
                                        }
        Dim dic = New Dictionary(Of String, Object) From {
                                        {"A", 1},
                                        {"B", "original text"},
                                        {"C", 3}
                                        }
        container.Data.Add(dic)
        Dim mapcol As New List(Of MapInfo) From {
            New MapInfo With {.FromName = "A", .ToName = "AA", .Rule = "return originalValue*100;"},
            New MapInfo With {.FromName = "B", .ToName = "BB", .Rule = "return 'special text';"}
            }

        'Act
        Dim newContainer = Mapper.Map(container, mapcol)
        Dim newData = newContainer.Data

        'Assert
        Assert.AreEqual(1, newData.Count)
        Assert.AreEqual(2, newData(0).Count)
        Assert.AreEqual(100, newData(0)("AA"))
        Assert.AreEqual("special text", newData(0)("BB"))
    End Sub

End Class