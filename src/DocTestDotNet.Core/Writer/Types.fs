namespace DocTestDotNet.Writer

[<RequireQualifiedAccess; NoComparison; StructuralEquality>]
type SourceCodeKind =
    | Full
    | WithMain

[<NoComparison; NoEquality>]
type SourceCode =
    { Kind: SourceCodeKind
      Code: string }

[<RequireQualifiedAccess; NoComparison; StructuralEquality>]
type ProjectReference =
    | Dll of path: string
