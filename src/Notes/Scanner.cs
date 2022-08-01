namespace Notes;

public class Scanner
{
    private string _text;
    private IList<Token> _tokens;
    private Action<int, string>? ReportError;
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    private bool IsAtEnd() => _current >= _text.Length;
    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return _text[_current];
    }

    public Scanner(string text) : this(text, null) {}

    public Scanner(string text, Action<int, string>? errorCallback)
    {
        _text = text ?? throw new ArgumentNullException(nameof(text));
        _tokens = new List<Token>();
        ReportError = errorCallback;
    }

    private char Advance()
    {
        if (!IsAtEnd()) _current++;
        return _text[_current - 1];
    }

    public IList<Token> ScanTokens()
    {
        while (!IsAtEnd())
        {
            _start = _current;
            ScanToken();
        }

        AddToken(TokenType.EOF);
        return _tokens;
    }

    private void ScanToken()
    {
        var currentChar = Advance();
        switch (currentChar)
        {
            case '(':
                AddToken(TokenType.LEFT_PAREN);
                break;
            case ')':
                AddToken(TokenType.RIGHT_PAREN);
                break;
            case '[':
                AddToken(Match('[') ?
                    TokenType.DOUBLE_LEFT_BRACKET :
                    TokenType.LEFT_BRACKET);
                break;
            case ']':
                AddToken(Match(']') ?
                    TokenType.DOUBLE_RIGHT_BRACKET :
                    TokenType.RIGHT_BRACKET);
                break;
            case '>':
                AddToken(TokenType.GREATER);
                break;
            case '#':
                AddToken(TokenType.OCTOTHORPE);
                break;
            case '*':
                AddToken(TokenType.STAR);
                break;
            case '_':
                AddToken(TokenType.UNDERSCORE);
                break;
            case '`':
                AddToken(TokenType.BACKTICK);
                break;
            case ' ':
            case '\r':
            case '\t':
                break;
            case '\n':
                NextLine();
                break;
            default:
                if (IsAlphaNumeric(currentChar))
                    Text();
                else
                    Error(_line, "Unexpected character.");
                break;
        }
    }

    private void Error(int line, string message)
    {
        if (ReportError is null) return;
        ReportError(line, message);
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddTokens(TokenType type, int count)
    {
        foreach (var _ in Enumerable.Range(1, count))
        {
            AddToken(type);
        }
    }

    private void AddToken(TokenType type, string? lexeme)
    {
        _tokens.Add(new Token(type, lexeme));
    }

    private void NextLine()
    {
        _line++;
    }

    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (_text[_current] != expected) return false;

        Advance();
        return true;
    }

    private bool IsAlpha(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    private bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    private bool IsDigit(char c)
    {
        return Char.IsDigit(c);
    }

    private void Text()
    {
        while (IsAlphaNumeric(Peek()))
        {
            Advance();
        }

        var text = _text.Substring(_start, _current - _start);
        AddToken(TokenType.TEXT, text);
    }
}
