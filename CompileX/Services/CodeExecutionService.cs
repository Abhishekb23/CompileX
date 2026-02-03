using CompileX.Models;
using System.Diagnostics;

namespace CompileX.Services;
public class CodeExecutionService
{
    public async Task<CodeResponse> ExecuteAsync(CodeRequest request)
    {
        var response = new CodeResponse();

        try
        {
            var escapedCode = request.Code
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "")
                .Replace("\n", "\\n");

            string image = request.Language switch
            {
                "java" => "java-runner",
                "python" => "python-runner",
                _ => throw new Exception("Unsupported language")
            };

            var psi = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments =
                    $"run --rm " +
                    $"--memory=256m --cpus=0.5 --network=none " +
                    $"-e CODE=\"{escapedCode}\" " +
                    $"{image}",
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using var process = Process.Start(psi)!;

            response.Output = await process.StandardOutput.ReadToEndAsync();
            response.Error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            response.Error = ex.Message;
        }

        return response;
    }
}

