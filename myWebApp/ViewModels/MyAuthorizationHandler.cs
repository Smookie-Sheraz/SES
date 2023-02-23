//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;

//namespace myWebApp.ViewModels
//{
//    public class MyAuthorizationHandler : AuthorizationHandler<MyAuthorizationRequirement>
//    {
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;

//        public MyAuthorizationHandler(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//        }

//        protected override async ValueTask HandleRequirementAsync(AuthorizationHandlerContext context, MyAuthorizationRequirement requirement)
//        {
//            var user = await _userManager.GetUserAsync(context.User);
//            if (user == null)
//            {
//                context.Fail();
//                return;
//            }

//            // Get the user's roles
//            var roles = await _userManager.GetRolesAsync(user);

//            // Check if the user has the required role
//            if (!roles.Contains(requirement.RequiredRole))
//            {
//                context.Fail();
//                return;
//            }
//            foreach (var permission in requirement.RequiredPermission)
//            {
//                if (!await _userManager.IsInRoleAsync(user, requirement.RequiredRole) || !await _userManager.CheckPasswordAsync(user, permission))
//                {
//                    context.Fail();
//                    return;
//                }
//            }
//            // Check if the user has the required permission

//            context.Succeed(requirement);
//        }
//    }

//}
