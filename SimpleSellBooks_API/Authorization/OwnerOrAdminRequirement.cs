using Microsoft.AspNetCore.Authorization;

namespace SimpleSellBooks_API.Authorization
{
    public class OwnerOrAdminRequirement : IAuthorizationRequirement { }
}
