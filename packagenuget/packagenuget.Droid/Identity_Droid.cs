using packagenuget.Droid;
using packagenuget;

[assembly: Xamarin.Forms.Dependency(typeof(Identity_Droid))]

namespace packagenuget.Droid
{
    public class Identity_Droid : Java.Lang.Object, IIdentification
    {
        public Identity_Droid() { }
        public string whoAmI()
        {
            return "I am Android.";
           
        }
    }
}