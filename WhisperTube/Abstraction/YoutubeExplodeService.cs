using WhisperTube.Models;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace WhisperTube.Abstraction;

public class YoutubeExplodeService : IYoutubeService
{
    public async Task<VideoInfo> GetVideoInfoAsync(string url, CancellationToken cancellationToken = default)
    {
        using var youtube = new YoutubeClient();
        var video = await youtube.Videos.GetAsync(url, cancellationToken);

        return new VideoInfo(
            Title: video.Title,
            Url: video.Url,
            Author: video.Author.ChannelTitle,
            Duration: video.Duration ?? TimeSpan.Zero
        );
    }

    public async Task<string> DownloadAudioAsync(string url, string outputPath,
        CancellationToken cancellationToken = default)
    {
        using var youtube = new YoutubeClient();

        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(url, cancellationToken);

        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();

        await youtube.Videos.Streams.DownloadAsync
            (streamInfo, outputPath, cancellationToken: cancellationToken);

        return outputPath;
    }
}