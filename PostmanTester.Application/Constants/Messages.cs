namespace PostmanTester.Application.Constants
{
    public static class Messages
    {
        #region SUCCESS
        public static string[] Success = { "success", "Operation has successfully completed." };
        #endregion

        #region ERROR
        public static string[] UnknownError = { "unknown_error", "An unkown error has occured." };
        public static string[] TokenError = { "token_error", "Invalid token is entered." };
        public static string[] TokenExpire = { "token_expire", "Token is expired." };
        public static string[] AddFailed = { "add_failed", "An error has occured during adding operation." };
        public static string[] DataNotFound = { "data_not_found", "The data is not found." };
        public static string[] DataExists = { "data_exists", "The data has already exists." };
        public static string[] InvalidPassword = { "invalid_password", "The password is invalid." };
        public static string[] DataNotActive = { "data_not_active", "The data is not active." };
        #endregion
    }
}
