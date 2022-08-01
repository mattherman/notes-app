namespace Notes.Tests;

public class ScannerTests
{
    private void AssertTokens(IList<Token> actual, Token[] expected)
    {
        Assert.Equal(expected.Length + 1, actual.Count);
        for (int i = 0; i < expected.Length - 1; i++)
        {
            Assert.Equal(expected[i].Type, actual[i].Type);
            Assert.Equal(expected[i].Lexeme, actual[i].Lexeme);
        }
        Assert.Equal(TokenType.EOF, actual.Last().Type);
    }

    [Fact]
    public void Heading1() => Scan(
        @"# Heading 1",
        new []
        {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.TEXT, "1"),
        }
    );

    [Fact]
    public void Heading2() => Scan(
        @"## Heading 2",
        new []
        {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.TEXT, "2"),
        }
    );

    [Fact]
    public void Heading3() => Scan(
        @"### Heading 3",
        new []
        {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.TEXT, "3"),
        }
    );

    [Fact]
    public void Heading4() => Scan(
        @"#### Heading 4",
        new []
        {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.TEXT, "4"),
        }
    );

    [Fact]
    public void Heading5() => Scan(
        @"##### Heading 5",
        new []
        {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.TEXT, "5"),
        }
    );

    [Fact]
    public void Heading6() => Scan(
        @"###### Heading 6",
        new []
        {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.TEXT, "6"),
        }
    );

    [Fact]
    public void Bold() => Scan(
        @"*bold*",
        new []
        {
            new Token(TokenType.STAR, null),
            new Token(TokenType.TEXT, "bold"),
            new Token(TokenType.STAR, null)
        }
    );

    [Fact]
    public void Italics() => Scan(
        @"_italicized_",
        new []
        {
            new Token(TokenType.UNDERSCORE, null),
            new Token(TokenType.TEXT, "italicized"),
            new Token(TokenType.UNDERSCORE, null)
        }
    );

    [Fact]
    public void Link() => Scan(
        @"[text](link)",
        new []
        {
            new Token(TokenType.LEFT_BRACKET, null),
            new Token(TokenType.TEXT, "text"),
            new Token(TokenType.RIGHT_BRACKET, null),
            new Token(TokenType.LEFT_PAREN, null),
            new Token(TokenType.TEXT, "link"),
            new Token(TokenType.RIGHT_PAREN, null)
        }
    );

    [Fact]
    public void DocumentLink() => Scan(
        @"[[file]]",
        new []
        {
            new Token(TokenType.DOUBLE_LEFT_BRACKET, null),
            new Token(TokenType.TEXT, "file"),
            new Token(TokenType.DOUBLE_RIGHT_BRACKET, null)
        }
    );

    [Fact]
    public void Backticks() => Scan(
        @"`code`",
        new []
        {
            new Token(TokenType.BACKTICK, null),
            new Token(TokenType.TEXT, "code"),
            new Token(TokenType.BACKTICK, null)
        }
    );

    [Fact]
    public void Greater() => Scan(
        @"> blockquote",
        new []
        {
            new Token(TokenType.GREATER, null),
            new Token(TokenType.TEXT, "blockquote")
        }
    );

    public void Bang() => Scan(
        @"! todo",
        new []
        {
            new Token(TokenType.BANG, null),
            new Token(TokenType.TEXT, "todo")
        }
    );

    public void Backslash() => Scan(
        @"\`",
        new []
        {
            new Token(TokenType.TEXT, "`"),
            new Token(TokenType.BACKTICK, null)
        }
    );

    [Fact]
    public void MultiLine() => Scan(
        @"# Heading
        *bold* something
        some more text",
        new []
        {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.STAR, null),
            new Token(TokenType.TEXT, "bold"),
            new Token(TokenType.STAR, null),
            new Token(TokenType.TEXT, "something"),
            new Token(TokenType.TEXT, "some"),
            new Token(TokenType.TEXT, "more"),
            new Token(TokenType.TEXT, "text")
        }
    );

    private void Scan(string text, Token[] expectedTokens)
    {
        var tokens = new Scanner(text, FailTestOnError).ScanTokens();
        AssertTokens(tokens, expectedTokens);
    }

    private void FailTestOnError(int line, string message) => throw new Exception($"[Line {line}] {message}");
}