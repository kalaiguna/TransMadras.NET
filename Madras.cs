using System.Text.RegularExpressions;

/// <summary>
/// Author: C.Gunasekaran
/// Date: 14.09.07
/// Purpose: Tamil Transliteration
///
/// Original implementation (Sep 2007): ~300 sequential String.Replace() calls per method.
/// Optimised (Oct 2026): single-pass Regex.Replace() using a compiled regex built from
/// CONSONANTS × VOWELS tables in a static constructor. Public API unchanged.
/// </summary>
public class Madras
{
    // [tamil, ISO-15919]
    private static readonly (string Tamil, string Latin)[] Consonants =
    [
        ("க",  "k" ), ("ங",  "ṅ" ), ("ச",  "c" ), ("ஞ",  "ñ" ),
        ("ட",  "ṭ" ), ("ண",  "ṇ" ), ("த",  "t" ), ("ந",  "n" ),
        ("ப",  "p" ), ("ம",  "m" ), ("ய",  "y" ), ("ர",  "r" ),
        ("ல",  "l" ), ("வ",  "v" ), ("ழ",  "ḻ" ), ("ள",  "ḷ" ),
        ("ற",  "ṟ" ), ("ன",  "ṉ" ), ("ஜ",  "j" ), ("ஷ",  "ṣ" ),
        ("ஸ",  "s" ), ("ஹ",  "h" ),
    ];

    // [vowel diacritic, ISO-15919 suffix] — empty diacritic = inherent /a/
    private static readonly (string Diacritic, string Vowel)[] Vowels =
    [
        ("",   "a" ),
        ("ா",  "ā" ),
        ("ி",  "i" ),
        ("ீ",  "ī" ),
        ("ு",  "u" ),
        ("ூ",  "ū" ),
        ("ெ",  "e" ),
        ("ே",  "ē" ),
        ("ை",  "ai"),
        ("ொ",  "o" ),
        ("ோ",  "ō" ),
        ("ௌ",  "au"),
    ];

    private static readonly Dictionary<string, string> StyleMap;
    private static readonly Regex StyleRegex;
    private static readonly Dictionary<string, string> UniMap;
    private static readonly Regex UniRegex;

    static Madras()
    {
        (StyleMap, StyleRegex) = BuildTable(StyleEntries());
        (UniMap, UniRegex) = BuildTable(UniEntries());
    }

    private static IEnumerable<(string Key, string Value)> StyleEntries()
    {
        // special ligature — its length puts it first after sort
        yield return ("ஸ்ரீ", "šrī");
        yield return ("ஃ",    "ḵ"  );

        // soul series (standalone vowels)
        yield return ("அ", "a" ); yield return ("ஆ", "ā" );
        yield return ("இ", "i" ); yield return ("ஈ", "ī" );
        yield return ("உ", "u" ); yield return ("ஊ", "ū" );
        yield return ("எ", "e" ); yield return ("ஏ", "ē" );
        yield return ("ஐ", "ai"); yield return ("ஒ", "o" );
        yield return ("ஓ", "ō" ); yield return ("ஔ", "au");

        // consonant + virama (mei / final-consonant forms)
        foreach (var (t, l) in Consonants)
            yield return (t + "்", l);

        // consonant + vowel diacritic (12 vowel series, bare /a/ included)
        foreach (var (t, l) in Consonants)
            foreach (var (d, v) in Vowels)
                yield return (t + d, l + v);
    }

    private static IEnumerable<(string Key, string Value)> UniEntries()
    {
        yield return ("šrī", "ஸ்ரீ");
        yield return ("ḵ",   "ஃ"  );

        // consonant + vowel (12 vowel series, bare /a/ included)
        foreach (var (t, l) in Consonants)
            foreach (var (d, v) in Vowels)
                yield return (l + v, t + d);

        // standalone consonants → mei forms (virama)
        foreach (var (t, l) in Consonants)
            yield return (l, t + "்");

        // soul series — regex sort by length handles au/ai before a/i automatically
        yield return ("au", "ஔ"); yield return ("ai", "ஐ");
        yield return ("ā",  "ஆ"); yield return ("ī",  "ஈ");
        yield return ("ū",  "ஊ"); yield return ("ē",  "ஏ");
        yield return ("ō",  "ஓ"); yield return ("a",  "அ");
        yield return ("i",  "இ"); yield return ("u",  "உ");
        yield return ("e",  "எ"); yield return ("o",  "ஒ");
    }

    private static (Dictionary<string, string> Map, Regex Regex) BuildTable(
        IEnumerable<(string Key, string Value)> entries)
    {
        var map = entries.ToDictionary(e => e.Key, e => e.Value);
        var pattern = string.Join("|",
            map.Keys
               .OrderByDescending(k => k.Length)
               .Select(Regex.Escape));
        return (map, new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant));
    }

    public static string Transtyle(string srcTxt) =>
        StyleRegex.Replace(srcTxt, m => StyleMap[m.Value]);

    public static string Trans2Uni(string srcTxt) =>
        UniRegex.Replace(srcTxt, m => UniMap[m.Value]);
}
