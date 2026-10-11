# TransMadras

Tamil Unicode ↔ ISO 15919 transliteration, wrapped in a .NET 10 minimal Web API.

The core transliteration logic is the `Madras` class, originally written by **C. Gunasekaran** in September 2007. This repository restores that class, optimises it with a single-pass compiled regex, and wraps it in a minimal API with a browser test UI.

A client-side-only companion app is available at [transmadras.vercel.app](https://transmadras.vercel.app/) — source at [TransMadras.SK](https://github.com/kalaiguna/TransMadras.SK) (SvelteKit + Vercel).

## What it does

| Direction | Input | Output |
|-----------|-------|--------|
| `Transtyle` | Tamil Unicode — `தமிழ்` | ISO 15919 Latin — `tamiḻ` |
| `Trans2Uni` | ISO 15919 Latin — `tamiḻ` | Tamil Unicode — `தமிழ்` |

The ISO 15919 scheme uses diacritics to distinguish Tamil phonemes that have no direct Latin equivalent (ā, ī, ū, ē, ō, ṭ, ṇ, ḻ, ḷ, ṟ, ṉ, ṣ, ṅ, ñ).

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
dotnet run
```

The app starts on `http://localhost:5000` by default.

| URL | Description |
|-----|-------------|
| `http://localhost:5000` | HTML test page |
| `http://localhost:5000/scalar/v1` | Interactive API reference (Scalar) |

## API

### `POST /api/transliterate/to-style`

Converts Tamil Unicode text to ISO 15919 transliteration.

**Request**
```json
{ "text": "தமிழ்" }
```

**Response**
```json
{ "result": "tamiḻ" }
```

---

### `POST /api/transliterate/to-unicode`

Converts ISO 15919 transliteration back to Tamil Unicode.

**Request**
```json
{ "text": "tamiḻ" }
```

**Response**
```json
{ "result": "தமிழ்" }
```

## Tests

```bash
dotnet test TransMadras.Tests/
```

73 xUnit tests covering all vowel series, round-trips, priority ordering, and edge cases.

## Project structure

```
transmadras/
├── Madras.cs                          # Transliteration class (C. Gunasekaran, 2007)
├── Program.cs                         # Minimal API entry point
├── TransMadras.csproj                 # .NET 10 project file
├── TransMadras.Tests/
│   ├── MadrasTests.cs                 # xUnit tests
│   └── TransMadras.Tests.csproj
└── wwwroot/
    └── index.html                     # Browser test page
```

## License

MIT — see [LICENSE](LICENSE).
