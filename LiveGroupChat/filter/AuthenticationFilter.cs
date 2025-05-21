// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;
//
// namespace LiveGroupChat.filter;
//
// public class AuthenticationFilter:IAuthorizationFilter
// {
// public void OnAuthorization(AuthorizationFilterContext context)
// {
//     var user = context.HttpContext.User;
//
//     if (!user.Identity?.IsAuthenticated ?? true)
//     {
//         context.Result = new JsonResult(new
//         {
//             error = "."
//         })
//         {
//             StatusCode = StatusCodes.Status401Unauthorized
//         };
//     }
// }
// }