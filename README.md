# WhisperTube

CLI tool for downloading YouTube videos and transcribing them to text using Whisper.

## Features

- Download audio from YouTube videos
- Transcribe audio to text using Whisper (runs locally)
- Multi-language support (auto-detection)
- Clean transcription output for LLM processing

## Requirements

- .NET 10 SDK
- ffmpeg (installed in PATH)
  - macOS: `brew install ffmpeg`
  - Linux: `sudo apt install ffmpeg`
  - Windows: [ffmpeg.org](https://ffmpeg.org/download.html)

## Installation

```bash
git clone <repo-url>
cd WhisperTube
dotnet restore
```

## Usage

```bash
dotnet run -- --url <youtube_video_url>
```

### Publish as standalone executable

```bash
dotnet publish -c Release -r <runtime> --self-contained
./bin/Release/net10.0/<runtime>/publish/WhisperTube --url <youtube_video_url>
```

### Create shell alias (optional)

Add to `~/.bashrc` or `~/.zshrc`:
```bash
alias transcribir='dotnet run --project /path/to/WhisperTube --'
```

Then use:
```bash
transcribir --url "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
```

## Output Files

- `transcripcion.txt` - Transcribed text
- `ggml-base.bin` - Whisper model (~140MB, downloaded on first run)


