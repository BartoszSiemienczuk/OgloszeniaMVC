using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ogloszenia.Startup))]
namespace Ogloszenia
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
