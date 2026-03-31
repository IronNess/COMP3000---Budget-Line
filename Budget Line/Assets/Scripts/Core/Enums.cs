/// <summary>
/// Represents the current time of day in the gameplay loop.
/// 
/// YAGNI:
/// - An enum is enough for the current project.
/// - No need for a more complex time-state system.
/// </summary>
public enum TimeBlock
{
    Morning,
    Afternoon,
    Evening,
    Night
}

/// <summary>
/// Represents the current day of the week.
/// 
/// Why this supports DRY:
/// - Shared enum avoids repeated string-based day logic across systems.
/// </summary>
public enum WeekDay
{
    Mon,
    Tue,
    Wed,
    Thu,
    Fri,
    Sat,
    Sun
}