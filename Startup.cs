using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppUsers.Startup))]
namespace WebAppUsers
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
