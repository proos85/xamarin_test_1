using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(XamarinForms1.Backend.Startup))]

namespace XamarinForms1.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}