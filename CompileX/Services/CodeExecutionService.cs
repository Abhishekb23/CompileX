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
            // Create isolated working directory
            var workDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(workDir);

            ProcessStartInfo psi;

            if (request.Language == "java")
            {
                await File.WriteAllTextAsync(
                    Path.Combine(workDir, "Main.java"),
                    request.Code
                );

                psi = new ProcessStartInfo
                {
                    FileName = "sh",
                    Arguments = "/runners/java/run.sh",
                    WorkingDirectory = workDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
            }
            else if (request.Language == "python")
            {
                await File.WriteAllTextAsync(
                    Path.Combine(workDir, "main.py"),
                    request.Code
                );

                psi = new ProcessStartInfo
                {
                    FileName = "sh",
                    Arguments = "/runners/python/run.sh",
                    WorkingDirectory = workDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
            }
            else
            {
                throw new Exception("Unsupported language");
            }

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
