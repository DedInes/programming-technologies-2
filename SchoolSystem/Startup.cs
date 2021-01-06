using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchoolSystem.Startup))]
namespace SchoolSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
