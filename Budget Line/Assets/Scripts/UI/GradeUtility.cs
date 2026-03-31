// GradeUtility.cs
// Purpose:
// A tiny shared helper used by multiple UI scripts.
// This improves DRY because the "number -> grade letter" rule lives in one place.

public static class GradeUtility
{
    // Caller:
    // Any script that needs to turn a numeric grade score into A/B/C/D.
    public static string ToLetter(int score)
    {
        if (score >= 80) return "A";
        if (score >= 60) return "B";
        if (score >= 40) return "C";
        return "D";
    }
}