using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InvoiceSystem.Startup))]
namespace InvoiceSystem
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
