using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(archiwum_wea.Startup))]
namespace archiwum_wea
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
