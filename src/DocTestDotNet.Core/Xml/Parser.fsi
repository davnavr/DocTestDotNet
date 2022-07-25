/// Helper module used to parse XML documentation files generated by .NET compilers.
[<RequireQualifiedAccess>]
module DocTestDotNet.Xml.Parser

open System.Collections.Generic
open System.Xml

[<Class>]
type ParseException =
    inherit System.Exception

val parseTestsFromReader : reader: XmlReader -> ParserOutput

val parseTestsFromString : xml: string -> ParserOutput
