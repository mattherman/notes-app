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
    public void Heading() => Scan(
        @"# Heading 1",
        new [] {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.TEXT, "1"),
        }
    );

    [Fact]
    public void SubHeading() => Scan(
        @"## Heading 2",
        new [] {
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.OCTOTHORPE, null),
            new Token(TokenType.TEXT, "Heading"),
            new Token(TokenType.TEXT, "2"),
        }
    );

    [Fact]
    public void Bold() => Scan(
        @"*bold*",
        new [] {
            new Token(TokenType.STAR, null),
            new Token(TokenType.TEXT, "bold"),
            new Token(TokenType.STAR, null)
        }
    );

    [Fact]
    public void Italics() => Scan(
        @"_italicized_",
        new [] {
            new Token(TokenType.UNDERSCORE, null),
            new Token(TokenType.TEXT, "italicized"),
            new Token(TokenType.UNDERSCORE, null)
        }
    );

    [Fact]
    public void Link() => Scan(
        @"[text](link)",
        new [] {
            new Token(TokenType.LEFT_BRACKET, null),
            new Token(TokenType.TEXT, "text"),
            new Token(TokenType.RIGHT_BRACKET, null),
            new Token(TokenType.LEFT_PAREN, null),
            new Token(TokenType.TEXT, "link"),
            new Token(TokenType.RIGHT_PAREN, null)
        }
    );

    private void Scan(string text, Token[] expectedTokens)
    {
        var tokens = new Scanner(text, FailTestOnError).ScanTokens();
        AssertTokens(tokens, expectedTokens);
    }

    private void FailTestOnError(int line, string message) => throw new Exception($"[Line {line}] {message}");
}