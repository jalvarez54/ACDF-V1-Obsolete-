using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IkoulaACDF.Startup))]
namespace IkoulaACDF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
