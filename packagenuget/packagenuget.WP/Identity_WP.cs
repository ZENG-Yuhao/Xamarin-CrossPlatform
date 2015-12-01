using packagenuget.WP;
using packagenuget;

[assembly: Xamarin.Forms.Dependency(typeof(Identity_WP))]
namespace packagenuget.WP
{
    public class Identity_WP : IIdentification
    {
        public Identity_WP() { }
        public string whoAmI()
        {
            return "I am Windows Phone.";
        }
    }
}
