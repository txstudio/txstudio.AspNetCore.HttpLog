using System;

namespace HttpLogData
{
    public sealed class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class LoginResult
    {
        public bool IsLogin { get; set; }
        public string Email { get; set; }

        public Nullable<Guid> rowguid { get; set; }
    }

}
