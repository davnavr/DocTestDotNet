namespace DocTestDotNet.Runner

open System.Runtime.CompilerServices

[<IsReadOnly; Struct; RequireQualifiedAccess; NoComparison; StructuralEquality>]
type Verbosity =
    | Quiet
    | Minimal
    | Normal

    static member Default = Quiet

    override this.ToString() =
        match this with
        | Quiet -> "quiet"
        | Minimal -> "minimal"
        | Normal -> "normal"

/// <summary>Indicates the path to the <c>dotnet</c> command line.</summary>
[<RequireQualifiedAccess; NoComparison; StructuralEquality>]
type DotNetPath =
    | Inferred
    | Explicit of path: string

    override this.ToString() =
        match this with
        | Inferred -> "dotnet"
        | Explicit path -> path

[<Sealed>]
type Options () =
    member val Verbosity = Verbosity.Default with get, set
    member val DotNetPath = DotNetPath.Inferred with get, set
