using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HospitalProject_W2022.Startup))]
namespace HospitalProject_W2022
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
