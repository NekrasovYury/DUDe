using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DUDe.Startup))]
namespace DUDe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
