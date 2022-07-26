namespace DocTestDotNet.Xml

open System.Collections.Immutable
open System.Runtime.CompilerServices

[<RequireQualifiedAccess; IsReadOnly; Struct; NoComparison; StructuralEquality>]
type SourceLanguage =
    | CSharp
    | FSharp

[<NoComparison; NoEquality>]
type ParsedTest =
    { Language: SourceLanguage
      Name: string
      Code: string }

[<NoComparison; NoEquality>]
type ParsedMember =
    { Member: string // MemberName
      Tests: ImmutableArray<ParsedTest> }

[<NoComparison; NoEquality>]
type ParserOutput =
    { Assembly: string
      Members: ImmutableArray<ParsedMember> }
