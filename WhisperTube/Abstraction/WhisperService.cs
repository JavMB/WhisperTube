using System.IO;
using System.Threading.Tasks;
using Whisper.net;
using Whisper.net.Ggml;

namespace WhisperTube.Abstraction;

public class WhisperService
{
    private readonly string _modelFileName = "ggml-base.bin";
    private readonly GgmlType _ggmlType = GgmlType.Base;

    public async Task TranscribeToTextAsync(string wavFilePath, string outputPath)
    {
        if (!File.Exists(wavFilePath))
        {
            throw new FileNotFoundException($"Archivo WAV no encontrado: {wavFilePath}");
        }

        await DownloadModelIfNeeded();

        using var whisperFactory = WhisperFactory.FromPath(_modelFileName);

        await using var processor = whisperFactory.CreateBuilder()
            .WithLanguage("auto")
            .Build();

        await using var fileStream = File.OpenRead(wavFilePath);

        var transcription = new System.Text.StringBuilder();

        await foreach (var result in processor.ProcessAsync(fileStream))
        {
            transcription.AppendLine(result.Text);
        }

        await File.WriteAllTextAsync(outputPath, transcription.ToString());
    }

    private async Task DownloadModelIfNeeded()
    {
        if (File.Exists(_modelFileName))
        {
            return;
        }

        Console.WriteLine($"Descargando modelo {_modelFileName}...");
        await using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(_ggmlType);
        await using var fileWriter = File.OpenWrite(_modelFileName);
        await modelStream.CopyToAsync(fileWriter);
        Console.WriteLine($"✓ Modelo descargado");
    }
}