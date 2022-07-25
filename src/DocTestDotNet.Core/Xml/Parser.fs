module DocTestDotNet.Xml.Parser

open System.Xml

let parseTestsFromReader (reader: XmlReader) =
    if isNull reader then nullArg (nameof reader)
    use src = reader
    let tests = ResizeArray<DocumentationTest>()
    failwith "TODO"
    tests

let parseTestsFromString (xml: string) =
    XmlReader.Create(xml, null, null) |> parseTestsFromReader
