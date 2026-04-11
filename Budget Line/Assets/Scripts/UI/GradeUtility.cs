// Optional helper if you want letter bands (A/B/C/D) somewhere; HUD and grade labels use raw 0–100 scores.

public static class GradeUtility
{
    public static string ToLetter(int score)
    {
        if (score >= 80) return "A";
        if (score >= 60) return "B";
        if (score >= 40) return "C";
        return "D";
    }
}