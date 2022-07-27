module DocTestDotNet.Xml.DocumentationParser

open System.Collections.Immutable
open System.Xml

type ParseException (message: string) =
    inherit System.Exception(message)

let parseFailed format = Printf.kprintf (fun msg -> raise(ParseException msg)) format

let parseTestsFromReader (reader: XmlReader) =
    if isNull reader then nullArg (nameof reader)
    use xml = reader

    xml.ReadStartElement "doc"
    xml.ReadStartElement "assembly"
    xml.ReadStartElement "name"
    let parsedAssemblyName = xml.ReadString()
    xml.ReadEndElement()
    xml.ReadEndElement()
    xml.ReadStartElement "members"

    let members = ImmutableArray.CreateBuilder()
    let tests = ImmutableArray.CreateBuilder()

    while xml.Read() do
        if xml.NodeType = XmlNodeType.Element && xml.Name = "member" then
            let rawMemberName = xml.GetAttribute "name"

            tests.Clear()

            while xml.ReadToDescendant "code" do
                let docTestName = xml.GetAttribute "testName"
                if docTestName <> null then
                    let language =
                        match xml.GetAttribute "testLanguage" with
                        | "cs" | "csharp" -> SourceLanguage.CSharp
                        | "fs" | "fsharp" -> SourceLanguage.FSharp
                        | bad -> parseFailed "invalid testLang attribute value %s" bad

                    tests.Add { Language = language; Name = docTestName; Code = xml.ReadString() }
                        
            members.Add { Member = rawMemberName; Tests = tests.ToImmutable() }

    { Assembly = parsedAssemblyName; Members = members.ToImmutable() }

let parseTestsFromString (xml: string) =
    XmlReader.Create(new System.IO.StringReader(xml)) |> parseTestsFromReader
