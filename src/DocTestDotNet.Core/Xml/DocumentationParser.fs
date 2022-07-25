module DocTestDotNet.Xml.DocumentationParser

open System.Xml

let parseTestsFromReader (reader: XmlReader) =
    if isNull reader then nullArg (nameof reader)
    use src = reader // TODO: Have an XML parser that ignores unknown elements/attributes
    let tests = ResizeArray()
    { AssemblyName = failwith "TODO"
      Tests = tests }

let parseTestsFromString (xml: string) =
    XmlReader.Create(xml, null, null) |> parseTestsFromReader
