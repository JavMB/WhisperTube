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
                Arguments = $"-i \"{inputPath}\" \"{outputPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };
        process.Start();
        await process.WaitForExitAsync(cancellationToken);

        if (process.ExitCode != 0)
        {
            throw new Exception($"ffmpeg falló con código de salida {process.ExitCode}");
        }
    }
}