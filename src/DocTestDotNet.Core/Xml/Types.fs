namespace DocTestDotNet.Xml

open System.Collections.Immutable

[<NoComparison; NoEquality>]
type ParsedTest =
    { SourceLanguage: string
      Name: string
      /// <summary>
      /// If set to <see langword="true"/>, indicates that an entry point method should not be automatically generated.
      /// </summary>
      OmitMainMethod: bool
      Code: string }

[<NoComparison; NoEquality>]
type ParsedMember =
    { Member: string // MemberName
      Tests: ImmutableArray<ParsedTest> }

[<NoComparison; NoEquality>]
type ParserOutput =
    { Assembly: string
      Members: ImmutableArray<ParsedMember> }
