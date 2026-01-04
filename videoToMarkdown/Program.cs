using System.CommandLine;
using videoToMarkdown.Abstraction;

var youtubeService = new YoutubeExplodeService();
var audioConverter = new FfmpegAudioConverter();

var urlOption = new Option<string>("--url")
{
    Description = "URL del video de YouTube"
};

var rootCommand = new RootCommand("Descarga audio de videos de YouTube");
rootCommand.Options.Add(urlOption);

rootCommand.SetAction(parseResult =>
{
    var url = parseResult.GetValue(urlOption);

    if (string.IsNullOrEmpty(url))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("✗ Error: Debes especificar la URL con --url <url>");
        Console.ResetColor();
        Environment.Exit(1);
    }

    try
    {
        Console.WriteLine($"Obteniendo información del video: {url}");
        var videoInfo = youtubeService.GetVideoInfoAsync(url).GetAwaiter().GetResult();

        Console.WriteLine();
        Console.WriteLine($"✓ Título: {videoInfo.Title}");
        Console.WriteLine($"✓ Autor: {videoInfo.Author}");
        Console.WriteLine($"✓ Duración: {videoInfo.Duration}");
        Console.WriteLine();

        var webmPath = Path.Combine(Directory.GetCurrentDirectory(), "audio.webm");
        Console.WriteLine($"Descargando audio a: {webmPath}");

        youtubeService.DownloadAudioAsync(url, webmPath).GetAwaiter().GetResult();

        Console.WriteLine($"✓ Descarga completada");

        var wavPath = Path.Combine(Directory.GetCurrentDirectory(), "audio.wav");
        Console.WriteLine($"Convirtiendo audio a WAV: {wavPath}");

        audioConverter.ConvertWebmToWavAsync(webmPath, wavPath).GetAwaiter().GetResult();

        File.Delete(webmPath);
        Console.WriteLine($"✓ WebM borrado (archivo temporal)");

        Console.WriteLine($"✓ Converión completada");
        Console.WriteLine($"Tamaño del archivo WAV: {new FileInfo(wavPath).Length:N0} bytes");
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ Error: {ex.Message}");
        Console.ResetColor();
        Environment.Exit(1);
    }
});

return rootCommand.Parse(args).Invoke();