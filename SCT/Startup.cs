using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SCT.Startup))]
namespace SCT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
