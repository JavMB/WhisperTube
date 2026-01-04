namespace videoToMarkdown.Models;

public record VideoInfo(string Title, string Url, string? Author, TimeSpan Duration);