using Xunit;

public class TransstyleTests
{
    [Fact]
    public void EmptyString_ReturnsEmpty() =>
        Assert.Equal("", Madras.Transtyle(""));

    [Theory]
    [InlineData("தமிழ்",     "tamiḻ"    )]
    [InlineData("தமிழ்நாடு", "tamiḻnāṭu")]
    [InlineData("ஓம்",       "ōm"       )]
    [InlineData("வணக்கம்",   "vaṇakkam" )]
    public void KnownWords(string tamil, string expected) =>
        Assert.Equal(expected, Madras.Transtyle(tamil));

    [Fact]
    public void SriLigature() =>
        Assert.Equal("šrī", Madras.Transtyle("ஸ்ரீ"));

    [Fact]
    public void Visarga() =>
        Assert.Equal("ḵ", Madras.Transtyle("ஃ"));

    [Theory]
    [InlineData("அ", "a" )]
    [InlineData("ஆ", "ā" )]
    [InlineData("இ", "i" )]
    [InlineData("ஈ", "ī" )]
    [InlineData("உ", "u" )]
    [InlineData("ஊ", "ū" )]
    [InlineData("எ", "e" )]
    [InlineData("ஏ", "ē" )]
    [InlineData("ஐ", "ai")]
    [InlineData("ஒ", "o" )]
    [InlineData("ஓ", "ō" )]
    [InlineData("ஔ", "au")]
    public void StandaloneVowels(string tamil, string expected) =>
        Assert.Equal(expected, Madras.Transtyle(tamil));

    [Theory]
    [InlineData("கா", "kā" )]
    [InlineData("மி", "mi" )]
    [InlineData("தூ", "tū" )]
    [InlineData("கை", "kai")]
    [InlineData("கௌ", "kau")]
    public void ConsonantVowelSeries(string tamil, string expected) =>
        Assert.Equal(expected, Madras.Transtyle(tamil));

    [Theory]
    [InlineData("க்", "k")]
    [InlineData("ழ்", "ḻ")]
    [InlineData("ன்", "ṉ")]
    public void MeiForms(string tamil, string expected) =>
        Assert.Equal(expected, Madras.Transtyle(tamil));

    [Theory]
    [InlineData("க",  "ka" )]
    [InlineData("ழ",  "ḻa" )]
    [InlineData("ஜ",  "ja" )]
    [InlineData("ஷ",  "ṣa" )]
    public void BareConsonants(string tamil, string expected) =>
        Assert.Equal(expected, Madras.Transtyle(tamil));

    [Fact]
    public void PassthroughLatin() =>
        Assert.Equal("hello 123", Madras.Transtyle("hello 123"));
}

public class Trans2UniTests
{
    [Fact]
    public void EmptyString_ReturnsEmpty() =>
        Assert.Equal("", Madras.Trans2Uni(""));

    [Theory]
    [InlineData("tamiḻ",     "தமிழ்"    )]
    [InlineData("tamiḻnāṭu", "தமிழ்நாடு")]
    [InlineData("ōm",        "ஓம்"      )]
    [InlineData("vaṇakkam",  "வணக்கம்"  )]
    public void KnownWords(string latin, string expected) =>
        Assert.Equal(expected, Madras.Trans2Uni(latin));

    [Fact]
    public void SriLigature() =>
        Assert.Equal("ஸ்ரீ", Madras.Trans2Uni("šrī"));

    [Fact]
    public void Visarga() =>
        Assert.Equal("ஃ", Madras.Trans2Uni("ḵ"));

    [Theory]
    [InlineData("a",  "அ")]
    [InlineData("ā",  "ஆ")]
    [InlineData("i",  "இ")]
    [InlineData("ī",  "ஈ")]
    [InlineData("u",  "உ")]
    [InlineData("ū",  "ஊ")]
    [InlineData("e",  "எ")]
    [InlineData("ē",  "ஏ")]
    [InlineData("ai", "ஐ")]
    [InlineData("o",  "ஒ")]
    [InlineData("ō",  "ஓ")]
    [InlineData("au", "ஔ")]
    public void StandaloneVowels(string latin, string expected) =>
        Assert.Equal(expected, Madras.Trans2Uni(latin));

    [Theory]
    [InlineData("kā",  "கா")]
    [InlineData("mi",  "மி")]
    [InlineData("tū",  "தூ")]
    [InlineData("kai", "கை")]
    [InlineData("kau", "கௌ")]
    public void ConsonantVowelSeries(string latin, string expected) =>
        Assert.Equal(expected, Madras.Trans2Uni(latin));

    [Fact]
    public void LongerSuffixBeatsShort_kau() =>
        // must not parse as 'ka' + 'u'
        Assert.Equal("கௌ", Madras.Trans2Uni("kau"));

    [Fact]
    public void LongerSuffixBeatsShort_kai() =>
        // must not parse as 'ka' + 'i'
        Assert.Equal("கை", Madras.Trans2Uni("kai"));

    [Theory]
    [InlineData("k", "க்")]
    [InlineData("ḻ", "ழ்")]
    [InlineData("ṉ", "ன்")]
    public void StandaloneConsonants(string latin, string expected) =>
        Assert.Equal(expected, Madras.Trans2Uni(latin));
}

public class RoundTripTests
{
    [Theory]
    [InlineData("தமிழ்"      )]
    [InlineData("தமிழ்நாடு"  )]
    [InlineData("ஓம்"        )]
    [InlineData("ஸ்ரீ"       )]
    [InlineData("வணக்கம்"    )]
    [InlineData("இந்தியா"    )]
    [InlineData("காலை வணக்கம்")]
    public void Tamil_RoundTrip(string tamil) =>
        Assert.Equal(tamil, Madras.Trans2Uni(Madras.Transtyle(tamil)));

    [Theory]
    [InlineData("tamiḻ"          )]
    [InlineData("tamiḻnāṭu"      )]
    [InlineData("šrī"            )]
    [InlineData("vaṇakkam"       )]
    [InlineData("kālai vaṇakkam" )]
    public void Latin_RoundTrip(string latin) =>
        Assert.Equal(latin, Madras.Transtyle(Madras.Trans2Uni(latin)));
}
