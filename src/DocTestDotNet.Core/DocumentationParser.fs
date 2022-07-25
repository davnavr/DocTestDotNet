module DocTestDotNet.DocumentationParser

open System.Xml

let parseTestsFromReader (source: XmlReader) =
    if isNull source then nullArg (nameof source)
    try
        let tests = ResizeArray<DocumentationTest>()
        failwith "TODO"
        tests
    finally
        source.Close()
