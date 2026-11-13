namespace GymManagementSystem.BLL.Common;

public sealed record Result(bool success, string? message = null, ResultKind Kind = ResultKind.Ok)
{
    public static Result OK()=> new Result(true);    
    public static Result Fail(string error,ResultKind kind = ResultKind.Conflict) => new Result(false, error, kind);
    public static Result NotFound(string error= "Not Found") => new Result(false,error,ResultKind.NotFound);
    public static Result Validation(string error) => new Result(false, error, ResultKind.NotFound);
}
public sealed record Result<T>(bool success, T? Value ,string? message = null, ResultKind Kind = ResultKind.Ok)
{
    public static Result<T> OK(T value) => new Result<T>(true, value);
    public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict) => new Result<T>(false, default, error, kind);
    public static Result<T> NotFound(string error = "NotFound") => new Result<T>(false,default,error,ResultKind.NotFound);
}
