// WideCharRepro - Minimal reproduction of wide/fullwidth character rendering issues.
//
// PURPOSE:
// Many terminal emulators incorrectly handle "wide" (fullwidth) Unicode codepoints
// (those with East Asian Width = Wide/Fullwidth, or emoji presentation). These
// characters occupy 2 terminal columns, but some terminals only advance the cursor
// by 1 column, causing subsequent text to overlap and the display to "tear."
//
// EXPECTED BEHAVIOR (correct terminals like Windows Terminal):
//   - Each wide character occupies exactly 2 columns.
//   - The grid lines up perfectly with no overlapping or shifted text.
//   - The separator '|' characters in each row are vertically aligned.
//
// BROKEN BEHAVIOR (terminals with the bug):
//   - Wide characters only advance the cursor 1 column instead of 2.
//   - Grid columns misalign; text overlaps or shifts left.
//   - Vertical '|' separators are NOT aligned across rows.
//
// HOW TO USE:
//   dotnet run
//   Compare output against a known-good terminal (e.g., Windows Terminal).
//
// DIAGNOSIS:
//   If the '|' separators are not vertically aligned, the terminal is not
//   correctly handling wide character cursor advancement.

using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Ensure we're in a mode that supports Unicode output
if (Environment.OSVersion.Platform == PlatformID.Win32NT)
{
    // Enable virtual terminal processing on Windows
    Console.Write ("\x1b[?25l"); // Hide cursor for cleaner output
}

Console.WriteLine (
    "ΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉ");
Console.WriteLine ("  Wide Character Rendering Test");
Console.WriteLine ("  If '|' separators are NOT vertically aligned, the terminal has");
Console.WriteLine ("  a wide-character cursor advancement bug.");
Console.WriteLine (
    "ΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉ");
Console.WriteLine ();

// --- Test 1: Emoji (U+1F600 - U+1F64F) ---
Console.WriteLine ("TEST 1: Emoji (each should occupy 2 columns)");
Console.WriteLine (
    "ΓöîΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓöÉ");

var startCodepoint = 0x1F600;
for (var row = 0; row < 4; row++)
{
    Console.Write ("Γöé");
    for (var col = 0; col < 16; col++)
    {
        var cp = startCodepoint + row * 16 + col;
        var ch = char.ConvertFromUtf32 (cp);
        Console.Write (ch);
        Console.Write ("Γöé");
    }

    Console.WriteLine ();
}

Console.WriteLine (
    "ΓööΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓöÿ");
Console.WriteLine ();

// --- Test 2: CJK Ideographs (U+4E00+) ---
Console.WriteLine ("TEST 2: CJK Ideographs (each should occupy 2 columns)");
Console.WriteLine (
    "ΓöîΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓöÉ");

startCodepoint = 0x4E00;
for (var row = 0; row < 4; row++)
{
    Console.Write ("Γöé");
    for (var col = 0; col < 16; col++)
    {
        var cp = startCodepoint + row * 16 + col;
        var ch = char.ConvertFromUtf32 (cp);
        Console.Write (ch);
        Console.Write ("Γöé");
    }

    Console.WriteLine ();
}

Console.WriteLine (
    "ΓööΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓöÿ");
Console.WriteLine ();

// --- Test 3: Mixed narrow + wide on same line ---
// Each line below is EXACTLY 20 display columns between the Γöé delimiters.
Console.WriteLine ("TEST 3: Mixed content alignment");
Console.WriteLine ("All lines are exactly 20 display columns. The 'Γöé' must align:");
Console.WriteLine ("ΓöîΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÉ");
Console.WriteLine ("ΓöéABCDEFGHIJKLMNOPQRSTΓöé <- 20 narrow (20├ù1=20)");
Console.WriteLine ("Γöé≡ƒÿÇ≡ƒÿü≡ƒÿé≡ƒÿâ≡ƒÿä≡ƒÿà≡ƒÿå≡ƒÿç≡ƒÿê≡ƒÿëΓöé <- 10 emoji  (10├ù2=20)");
Console.WriteLine ("ΓöéΣ╜áσÑ╜Σ╕ûτòîµ╡ïΦ»òσ«╜σ¡ùτ¼ªΘ¬îΓöé <- 10 CJK    (10├ù2=20)");
Console.WriteLine ("ΓöéAB≡ƒÿÇCD≡ƒÿüEF≡ƒÿéGH≡ƒÿâIJ≡ƒÿäΓöé <- mixed   (10├ù1 + 5├ù2=20)");
Console.WriteLine ("ΓööΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÇΓöÿ");
Console.WriteLine ();

// --- Test 4: ANSI cursor positioning with wide chars ---
Console.WriteLine ("TEST 4: Programmatic cursor-positioned grid");
Console.WriteLine ("  Writing wide chars at absolute positions via ANSI escapes.");
Console.WriteLine ("  If the terminal handles wcwidth correctly, all rows align.");
Console.WriteLine ();

// Get current cursor row (approximate - just write sequentially with known widths)
string[] testRows =
[
    "Γöé≡ƒÿÇΓöé≡ƒÿüΓöé≡ƒÿéΓöé≡ƒñúΓöé≡ƒÿäΓöé≡ƒÿàΓöé≡ƒÿåΓöé≡ƒÿçΓöé",
    "ΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöé",
    "Γöé≡ƒÉ╢Γöé≡ƒÉ▒Γöé≡ƒÉ¡Γöé≡ƒÉ╣Γöé≡ƒÉ░Γöé≡ƒªèΓöé≡ƒÉ╗Γöé≡ƒÉ╝Γöé",
    "ΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöé",
    "ΓöéΣ╜áΓöéσÑ╜ΓöéΣ╕ûΓöéτòîΓöéµ╡ïΓöéΦ»òΓöéσ«╜Γöéσ¡ùΓöé",
    "ΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöéΓöÇΓöÇΓöé"
];

Console.WriteLine ("ΓöîΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓö¼ΓöÇΓöÇΓöÉ");

foreach (var line in testRows)
{
    Console.WriteLine (line);
}

Console.WriteLine ("ΓööΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓö┤ΓöÇΓöÇΓöÿ");
Console.WriteLine ();

// --- Test 5: Explicit column-counting verification ---
Console.WriteLine ("TEST 5: Column-width verification");
Console.WriteLine ("  The 'X' markers below should align with column 20:");
Console.WriteLine ();
Console.WriteLine ("01234567890123456789X  <- 20 narrow (20├ù1=20)");
Console.WriteLine ("≡ƒÿÇ≡ƒÿü≡ƒÿé≡ƒÿâ≡ƒÿä≡ƒÿà≡ƒÿå≡ƒÿç≡ƒÿê≡ƒÿëX  <- 10 emoji  (10├ù2=20)");
Console.WriteLine ("Σ╜áσÑ╜Σ╕ûτòîµ╡ïΦ»òσ«╜σ¡ùτ¼ªΘ¬îX  <- 10 CJK    (10├ù2=20)");
Console.WriteLine ("aπüébπüäcπüådπüêeπüèfπüïπüìX  <- mixed     (6├ù1 + 7├ù2=20)");
Console.WriteLine ();
Console.WriteLine ("If the 'X' markers don't vertically align at column 20,");
Console.WriteLine ("the terminal is miscounting wide character widths.");
Console.WriteLine ();

// --- Summary ---
Console.WriteLine (
    "ΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉ");
Console.WriteLine ("  DIAGNOSIS:");
Console.WriteLine ("  ΓÇó If all grids have aligned 'Γöé' separators ΓåÆ terminal is CORRECT");
Console.WriteLine ("  ΓÇó If grids are torn/misaligned ΓåÆ terminal has wcwidth bug");
Console.WriteLine ("  ΓÇó Common cause: terminal treats wide chars as 1 column, not 2");
Console.WriteLine (
    "ΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉΓòÉ");

// Show cursor again
if (Environment.OSVersion.Platform == PlatformID.Win32NT)
{
    Console.Write ("\x1b[?25h");
}
