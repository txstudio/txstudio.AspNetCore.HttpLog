using System;
using System.Threading.Tasks;
using HttpLogData;
using Microsoft.AspNetCore.Mvc;

namespace HttpLogServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AccountController : ControllerBase
    {
        const string _default_email = "boss@txstudio.tw";
        const string _default_password = "Pa$$w0rd";
        const string _default_guid = "bd0ed836-d6a1-4ddd-ba8d-ee7e42cfc781";

        [Route("login")]
        public async Task<ActionResult<LoginResult>> Login([FromBody]LoginViewModel model)
        {
            return await Task.Run<LoginResult>(() => {

                LoginResult _result;

                _result = new LoginResult();

                var _comparsion = StringComparison.OrdinalIgnoreCase;

                _result.Email = model.Email;
                _result.IsLogin = false;

                if (string.Equals(model.Email, _default_email, _comparsion) == true
                    && string.Equals(model.Password, _default_password, _comparsion) == true)
                {
                    _result.rowguid = Guid.Parse(_default_guid);
                    _result.IsLogin = true;
                }

                return _result;
            });
        }
    }
}
