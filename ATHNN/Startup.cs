using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ATHNN.Startup))]
namespace ATHNN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
