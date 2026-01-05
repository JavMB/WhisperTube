using System.Diagnostics;

namespace videoToMarkdown.Abstraction;

public class FfmpegAudioConverter
{
    public async Task ConvertWebmToWavAsync(string inputPath, string outputPath,
        CancellationToken cancellationToken = default)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-i \"{inputPath}\" -ar 16000 -ac 1 \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        
        var errorTask = process.StandardError.ReadToEndAsync(cancellationToken);
        
        await process.WaitForExitAsync(cancellationToken);
        
        await errorTask;

        if (process.ExitCode != 0)
        {
            throw new Exception($"ffmpeg falló con código de salida {process.ExitCode}");
        }
    }
}