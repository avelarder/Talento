using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Talento.Startup))]
namespace Talento
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
