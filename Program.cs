using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "TransMadras — Tamil Transliteration API";
    options.Theme = Scalar.AspNetCore.ScalarTheme.Purple;
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/api/transliterate/to-style", (TransliterationRequest req) =>
    Results.Ok(new TransliterationResponse(Madras.Transtyle(req.Text))))
    .WithName("ToStyle")
    .WithSummary("Tamil Unicode → ISO 15919 transliteration")
    .WithDescription("Converts Tamil Unicode text (e.g. தமிழ்) to ISO 15919 Latin transliteration (e.g. tamiḻ).");

app.MapPost("/api/transliterate/to-unicode", (TransliterationRequest req) =>
    Results.Ok(new TransliterationResponse(Madras.Trans2Uni(req.Text))))
    .WithName("ToUnicode")
    .WithSummary("ISO 15919 transliteration → Tamil Unicode")
    .WithDescription("Converts ISO 15919 Latin transliteration (e.g. tamiḻ) back to Tamil Unicode (e.g. தமிழ்).");

app.Run();

record TransliterationRequest(string Text);
record TransliterationResponse(string Result);
