namespace DocTestDotNet.Writer

open System.Collections.Immutable

[<Interface>]
type ITestWriter =
    abstract ProjectFileExtension : string

    abstract SourceFileExtension : string

    abstract WriteSourceCode : code: SourceCode * destination: System.IO.TextWriter -> unit

    abstract WriteProjectFile :
        sources: ImmutableArray<string> *
        references: ImmutableArray<ProjectReference> *
        destination: System.Xml.XmlWriter -> unit

[<Sealed; Class>]
type CSharpTestWriter =
    interface ITestWriter

    static member Instance : CSharpTestWriter

[<Sealed; Class>]
type FSharpTestWriter =
    interface ITestWriter

    static member Instance : FSharpTestWriter

[<RequireQualifiedAccess>]
module TestWriter =
    val csharp : CSharpTestWriter
    val fsharp : FSharpTestWriter

    type WriterLookup = System.Collections.Generic.IReadOnlyDictionary<string, ITestWriter>

    val defaultWriterLookup : WriterLookup

    val writeTestsToPath :
        path: string ->
        references: ImmutableArray<ProjectReference> ->
        writers: WriterLookup ->
        tests: DocTestDotNet.Xml.ParserOutput ->
        ImmutableArray<string>
