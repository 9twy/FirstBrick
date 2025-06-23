using System.Security.Claims;

namespace AccountService.Extensions
{
    public static class ClaimsPrincipalExtensions
    {

        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out var id) ? id : (int?)null;
        }

    }
}
