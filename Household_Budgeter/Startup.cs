using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Household_Budgeter.Startup))]
namespace Household_Budgeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
