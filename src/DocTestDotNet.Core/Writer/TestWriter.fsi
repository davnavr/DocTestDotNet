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
        targetFramework: string *
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
    val csharp : ITestWriter
    val fsharp : ITestWriter

    type WriterLookup = System.Collections.Generic.IReadOnlyDictionary<string, ITestWriter>

    val defaultWriterLookup : WriterLookup

    val writeTestsToPath :
        path: string ->
        references: ImmutableArray<ProjectReference> ->
        testTargetFramework: string ->
        writers: WriterLookup ->
        tests: DocTestDotNet.Xml.ParserOutput ->
        ImmutableArray<string>
