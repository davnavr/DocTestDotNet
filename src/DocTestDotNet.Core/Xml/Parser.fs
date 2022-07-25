module DocTestDotNet.Xml.Parser

open System.Xml

type ParseException (message: string) =
    inherit System.Exception(message)

let parseTestsFromReader (reader: XmlReader) =
    if isNull reader then nullArg (nameof reader)
    use src = reader
    let tests = ResizeArray()
    
    { AssemblyName = failwith "TODO"
      Tests = tests }

let parseTestsFromString (xml: string) =
    XmlReader.Create(xml, null, null) |> parseTestsFromReader
