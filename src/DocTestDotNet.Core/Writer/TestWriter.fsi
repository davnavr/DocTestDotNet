namespace DocTestDotNet.Writer

open System.Collections.Generic
open System.Collections.Immutable

[<Interface>]
type ITestWriter =
    abstract ProjectFileExtension : string

    abstract SourceFileExtension : string

    abstract WriteSourceCode : code: SourceCode * destination: System.IO.TextWriter -> unit

    abstract WriteProjectFile :
        sources: ImmutableArray<string> *
        references: IReadOnlyCollection<ProjectReference> *
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

    type WriterLookup = IReadOnlyDictionary<string, ITestWriter>

    val defaultWriterLookup : WriterLookup

    val writeTestsToPath :
        path: string ->
        references: IReadOnlyCollection<ProjectReference> ->
        testTargetFramework: string ->
        writers: WriterLookup ->
        tests: DocTestDotNet.Xml.ParserOutput ->
        IReadOnlyCollection<string>
