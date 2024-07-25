using Shared.Result;

namespace Auth.Constants;

public static class Errors
{
    public static class AddUserToRoleErrors
    {
        public static readonly Error UserNotFound = new Error(
            "AddUserToRoleErrors.UserNotFound",
            "User doesn't exist.");

        public static readonly Error FailedOperation = new Error(
            "AddUserToRoleErrors.FailedOperation",
            "Operation Failed Unexpectedly.");
    }
}
