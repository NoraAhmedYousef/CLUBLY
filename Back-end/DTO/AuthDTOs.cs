namespace ClubManagementAPI.DTOs;

// ─────────────────────────────────────────────────
//  SIGN IN  (Admin / Trainer / Member)
// ─────────────────────────────────────────────────

public class SignInRequest
{
    /// <summary>User email address</summary>
    public string Email    { get; set; } = string.Empty;

    /// <summary>User password</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Must be: Admin | Trainer | Member  (Guest cannot sign in)</summary>
    public string Role     { get; set; } = string.Empty;
}

public class SignInResponse
{
    public string  Token      { get; set; } = string.Empty;
    public string  Role       { get; set; } = string.Empty;
    public int     UserId     { get; set; }
    public string  FullName   { get; set; } = string.Empty;
    public string  Email      { get; set; } = string.Empty;
    public string  DashboardUrl { get; set; } = string.Empty;   // redirect hint for the frontend
    public DateTime ExpiresAt { get; set; }
}

// ─────────────────────────────────────────────────
//  SIGN UP  (Guest only)
// ─────────────────────────────────────────────────

public class SignUpRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName  { get; set; } = string.Empty;
    public string Email     { get; set; } = string.Empty;
    public string Phone     { get; set; } = string.Empty;
    public string Password  { get; set; } = string.Empty;
}

public class SignUpResponse
{
    public int    UserId    { get; set; }
    public string FullName  { get; set; } = string.Empty;
    public string Email     { get; set; } = string.Empty;
    public string Status    { get; set; } = "Pending";  // always Pending until admin approves
    public string Message   { get; set; } = string.Empty;
}

// ─────────────────────────────────────────────────
//  SHARED WRAPPER
// ─────────────────────────────────────────────────

public class ApiResponse<T>
{
    public bool         Success { get; set; }
    public string       Message { get; set; } = string.Empty;
    public T?           Data    { get; set; }
    public List<string> Errors  { get; set; } = new();

    public static ApiResponse<T> Ok(T data, string message = "Success") =>
        new() { Success = true, Message = message, Data = data };

    public static ApiResponse<T> Fail(string message, List<string>? errors = null) =>
        new() { Success = false, Message = message, Errors = errors ?? new() };
}
