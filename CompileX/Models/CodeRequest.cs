namespace CompileX.Models;

public class CodeRequest
{
    public string Language { get; set; } = "";
    public string Code { get; set; } = "";
    public string? Input { get; set; }
}

public class CodeResponse
{
    public string Output { get; set; } = "";
    public string Error { get; set; } = "";
}