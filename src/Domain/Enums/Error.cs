namespace AlphaVisa.Domain.Enums;

public enum ErrorTypes
{
    // Negative values represent for system errors
    None = 0,
    // Positive values represent for business errors
    Failure = 1,
    Validation = 2,
    NotFound = 3,
    Conflict = 4,
}

public enum ErrorCodes
{
    None = 0,
    NullValue = 1,
}
