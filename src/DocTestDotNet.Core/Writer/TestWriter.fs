namespace DocTestDotNet.Writer

open System.Collections.Generic
open System.Collections.Immutable
open System.IO
open System.Xml

open DocTestDotNet.Xml

[<Interface>]
type ITestWriter =
    abstract ProjectFileExtension : string

    abstract SourceFileExtension : string

    abstract WriteSourceCode : code: SourceCode * destination: TextWriter -> unit

    abstract WriteProjectFile :
        sources: ImmutableArray<string> *
        references: IReadOnlyCollection<ProjectReference> *
        targetFramework: string *
        destination: XmlWriter -> unit

[<AutoOpen>]
module Helpers =
    let writeProjectProperties tfm (xml: XmlWriter) =
        xml.WriteStartElement "PropertyGroup"
        xml.WriteElementString("TargetFramework", tfm)
        xml.WriteEndElement()

    let writeProjectReferences (references: IReadOnlyCollection<ProjectReference>) (xml: XmlWriter) =
        if references.Count > 0 then
            xml.WriteStartElement "ItemGroup"
            for r in references do
                match r with
                | ProjectReference.Dll path ->
                    xml.WriteStartElement "Reference"
                    xml.WriteAttributeString("Include", path)
                xml.WriteEndElement()
            xml.WriteEndElement()

[<Sealed>]
type CSharpTestWriter private () =
    static member val Instance = CSharpTestWriter()

    interface ITestWriter with
        member _.ProjectFileExtension = "csproj"
        member _.SourceFileExtension = "cs"

        member _.WriteSourceCode(code, destination) =
            raise(System.NotImplementedException())

        member _.WriteProjectFile(_, references, tfm, destination) =
            // No need to explicitly include sources, since C# projects automatically include source files in the same directory.
            writeProjectProperties tfm destination
            writeProjectReferences references destination

[<Sealed>]
type FSharpTestWriter private () =
    static member val Instance = FSharpTestWriter()

    interface ITestWriter with
        member _.ProjectFileExtension = "fsproj"
        member _.SourceFileExtension = "fs"

        member _.WriteSourceCode(code, destination) =
            raise(System.NotImplementedException())

        member _.WriteProjectFile(sources, references, tfm, destination) =
            for path in sources do
                destination.WriteStartElement "Compile"
                destination.WriteAttributeString("Include", path)
                destination.WriteEndElement()

            writeProjectProperties tfm destination
            writeProjectReferences references destination

module TestWriter =
    let csharp = CSharpTestWriter.Instance :> ITestWriter
    let fsharp = FSharpTestWriter.Instance :> ITestWriter

    type WriterLookup = System.Collections.Generic.IReadOnlyDictionary<string, ITestWriter>

    let defaultWriterLookup =
        ImmutableDictionary.Empty.Add("csharp", csharp).Add("fsharp", fsharp) :> WriterLookup

    let writeTestsToPath path references testTargetFramework (writers: WriterLookup) (tests: ParserOutput) =
        Directory.CreateDirectory path |> ignore

        let projectFilePaths = List()

        for mber in tests.Members do
            for test in mber.Tests do
                // TODO: Have a helper to get a safe test file name
                let safeTestName = test.Name

                let writer = writers[test.SourceLanguage]

                let testProjectDirectory = Path.Combine(path, safeTestName)
                let testProjectPath = Path.Combine(testProjectDirectory, safeTestName + "." + writer.ProjectFileExtension)
                let testSourcePath = Path.Combine(testProjectDirectory, safeTestName + "." + writer.SourceFileExtension)

                Directory.CreateDirectory testProjectDirectory |> ignore

                do
                    use testProjectWriter = XmlWriter.Create testProjectPath

                    testProjectWriter.WriteStartElement "Project"
                    testProjectWriter.WriteAttributeString("Sdk", "Microsoft.NET.Sdk")

                    writer.WriteProjectFile(
                        ImmutableArray.Create testSourcePath,
                        references,
                        testTargetFramework,
                        testProjectWriter
                    )

                    testProjectWriter.WriteEndElement()

                do
                    let source =
                        { SourceCode.Code = test.Code
                          SourceCode.Kind = if test.OmitMainMethod then SourceCodeKind.Full else SourceCodeKind.WithMain }

                    use testSourceWriter = new StreamWriter(testSourcePath)
                    writer.WriteSourceCode(source, testSourceWriter)

                projectFilePaths.Add testProjectPath

        projectFilePaths :> IReadOnlyCollection<_>
