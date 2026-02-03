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

            var psi = request.Language switch
            {
                "java" => new ProcessStartInfo
                {
                    FileName = "bash",
                    Arguments = $"/runners/java/run.sh",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                },
                "python" => new ProcessStartInfo
                {
                    FileName = "bash",
                    Arguments = $"/runners/python/run.sh",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                },
                _ => throw new Exception("Unsupported language")
            };

            // write code to file before execution
            var workDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(workDir);

            if (request.Language == "java")
                await File.WriteAllTextAsync(Path.Combine(workDir, "Main.java"), request.Code);
            else
                await File.WriteAllTextAsync(Path.Combine(workDir, "main.py"), request.Code);

            psi.WorkingDirectory = workDir;


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

