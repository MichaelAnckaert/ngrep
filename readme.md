# NGrep 
Simple grep like utility program written in C# and DotNet Core. 

## Building

```
dotnet build
```

## Usage

```
Description:
  A simple grep-like utility

Usage:
  ngrep <input-file> <pattern> [options]

Arguments:
  <input-file>  The file to search
  <pattern>     The pattern to search for

Options:
  -B, --before <before>  The number of lines to print before the match [default: 0]
  -A, --after <after>    The number of lines to print after the match [default: 0]
  --version              Show version information
  -?, -h, --help         Show help and usage information
  ```

