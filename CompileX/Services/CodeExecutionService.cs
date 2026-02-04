using CompileX.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CompileX.Services;

public class CodeExecutionService
{
    public async Task<CodeResponse> ExecuteAsync(CodeRequest request)
    {
        var response = new CodeResponse();

        try
        {
            var workDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(workDir);

            ProcessStartInfo psi;

            if (request.Language == "java")
            {
                // Detect class containing main method
                var match = Regex.Match(
                    request.Code,
                    @"class\s+([A-Za-z_][A-Za-z0-9_]*)[\s\S]*?static\s+void\s+main\s*\("
                );

                if (!match.Success)
                    throw new Exception("No class with main method found.");

                var className = match.Groups[1].Value;

                // Check if that class is public
                bool isPublic = Regex.IsMatch(
                    request.Code,
                    $@"public\s+class\s+{className}\b"
                );

                var fileName = isPublic ? $"{className}.java" : "Program.java";
                await File.WriteAllTextAsync(
                    Path.Combine(workDir, fileName),
                    request.Code
                );

                psi = new ProcessStartInfo
                {
                    FileName = "sh",
                    Arguments = "/runners/java/run.sh",
                    WorkingDirectory = workDir,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                };

                // ✅ pass main class explicitly
                psi.Environment["MAIN_CLASS"] = className;
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
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                };
            }
            else
            {
                throw new Exception("Unsupported language");
            }

            using var process = Process.Start(psi)!;

            // 3️⃣ Pass user input (stdin)
            if (!string.IsNullOrWhiteSpace(request.Input))
            {
                await process.StandardInput.WriteAsync(request.Input);
                process.StandardInput.Close();
            }

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
