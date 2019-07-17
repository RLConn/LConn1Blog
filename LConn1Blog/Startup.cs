using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LConn1Blog.Startup))]
namespace LConn1Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
