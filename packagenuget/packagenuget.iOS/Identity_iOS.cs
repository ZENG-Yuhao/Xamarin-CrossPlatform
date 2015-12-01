
using packagenuget.iOS;
using packagenuget;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(Identity_iOS))]
namespace packagenuget.iOS
{
    public class Identity_iOS : IIdentification
    {
        public Identity_iOS() { }

        public string whoAmI()
        {
            return "I am IOS.";
        }
    }
}
