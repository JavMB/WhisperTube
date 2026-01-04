using videoToMarkdown.Models;


namespace videoToMarkdown.Abstraction;

public interface IYoutubeService
{
    Task<VideoInfo> GetVideoInfoAsync(string url, CancellationToken cancellationToken = default);
    
    Task<string> DownloadAudioAsync(string url, string outputPath, CancellationToken cancellationToken = default);
}