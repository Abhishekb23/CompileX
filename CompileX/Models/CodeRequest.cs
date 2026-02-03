namespace CompileX.Models;

public class CodeRequest
{
    public string Code { get; set; } = string.Empty;
    public string Language { get; set; } = "java"; // java | python

}
public class CodeResponse
{
    public string Output { get; set; } = "";
    public string Error { get; set; } = "";
}