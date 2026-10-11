# TransMadras

Tamil Unicode ↔ ISO 15919 transliteration, wrapped in a .NET 10 minimal Web API.

The core transliteration logic is the `Madras` class, originally written by **C. Gunasekaran** in September 2007. This repository restores that class as-is and adds a simple API and test UI around it.

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

## Project structure

```
transmadras/
├── Madras.cs               # Original transliteration class (C. Gunasekaran, 2007)
├── Program.cs              # Minimal API entry point
├── TransMadras.csproj      # .NET 10 project file
└── wwwroot/
    └── index.html          # Browser test page
```

## License

MIT — see [LICENSE](LICENSE).
