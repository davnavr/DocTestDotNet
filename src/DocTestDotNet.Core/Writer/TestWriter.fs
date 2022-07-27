namespace DocTestDotNet.Writer

open System.Collections.Immutable
open System.IO
open System.Xml

[<Interface>]
type ITestWriter =
    abstract ProjectFileExtension : string

    abstract SourceFileExtension : string

    abstract WriteSourceCode : code: SourceCode * destination: TextWriter -> unit

    abstract WriteProjectFile :
        sources: ImmutableArray<string> *
        references: ImmutableArray<ProjectReference> *
        destination: XmlWriter -> unit

[<AutoOpen>]
module Helpers =
    let writeProjectReferences (references: ImmutableArray<ProjectReference>) (xml: XmlWriter) =
        if not references.IsDefaultOrEmpty then
            xml.WriteStartElement "ItemGroup"
            for r in references do
                match r with
                | ProjectReference.Dll path ->
                    //xml.WriteStartElement ""
                    failwith "TODO: Add reference to assembly"

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

        member _.WriteProjectFile(_, references, destination) =
            // No need to explicitly include sources, since C# projects automatically include source files in the same directory.
            writeProjectReferences references destination

[<Sealed>]
type FSharpTestWriter private () =
    static member val Instance = FSharpTestWriter()

    interface ITestWriter with
        member _.ProjectFileExtension = "fsproj"
        member _.SourceFileExtension = "fs"

        member _.WriteSourceCode(code, destination) =
            raise(System.NotImplementedException())

        member _.WriteProjectFile(sources, references, destination) =
            for path in sources do
                destination.WriteStartElement "Compile"
                destination.WriteAttributeString("Include", path)
                destination.WriteEndElement()

            writeProjectReferences references destination

module TestWriter =
    let csharp = CSharpTestWriter.Instance
    let fsharp = FSharpTestWriter.Instance
