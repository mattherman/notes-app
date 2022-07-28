# Notes

Notes is a suite of tools for parsing, transforming, and searching notes written in a Markdown-like text format.

## Writing Notes

The text format is designed to support most of the common Markdown syntax, but not necessarily correspond to the CommonMark spec:
```
# Headings
*bold*
_italic_
* bullet
* points
1. numbered
2. lists
[links](https://github.com/mattherman)
\`monospaced text\`
\`\`\`monospaced block\`\`\`
> blockquotes
```

It supports some additional syntax for Notes-specific features:
```
! This is an action item
: This is a tag
[ ] This is an unchecked list item
[X] This is a checked list item
[[Link to a different note]]
```

## Transformation

The CLI can be used to transform Notes to the following formats:
* *HTML*
* *JSON*
* *Markdown* (any non-standard syntax will be stripped from the text)

## Indexing

The CLI can be used to index Notes in a SQLite database. It will store the original text along with metadata gathered by parsing the note such as a table of contents, action items, links between notes, etc. The CLI can perform basic queries and full-text searching against this data.

## Web Interface

An API and web interface is included that allows you to view and search your Notes.

## Architecture

* Notes
* Notes.Cli
* Notes.Web
* Notes.Indexing

## CLI

```
notes convert MyNote.md --format html --output MyNote.html
```

```
echo "# Hello, World!" > notes convert --format json
```

```
notes add MyNote.md --store MySQLiteInstance.db
```

```
notes query --tag "Programming" --tag "C#" --text "some text"
```

```
notes todo
```

```
notes watch
```

```
notes serve
```
