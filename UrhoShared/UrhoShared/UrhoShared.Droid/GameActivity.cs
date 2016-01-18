using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Urho.Droid;
using Org.Libsdl.App;
using Android.Graphics;
using Android.Util;

namespace UrhoShared.Droid
{
    [Activity(Label = "UrhoSharp",
            Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
            ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.Orientation,
            ScreenOrientation = ScreenOrientation.Landscape)]
    public class GameActivity : Activity
    {
        private SDLSurface surface;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var mLayout = new FrameLayout(this);

            surface = UrhoSurface.CreateSurface(this, typeof(MySample));
            //surface.Background.SetAlpha(10);
            
            TextView txtView = new TextView(this);
            txtView.SetBackgroundColor(Color.Violet);
            txtView.SetWidth(10);
            txtView.SetHeight(10);
            
            txtView.Text = "HAHAH";
            //txtView.Text = surface.Background.ToString();
            Button btn = new Button(this);
            btn.SetBackgroundColor(Color.Transparent);
            btn.Text = "Button";
            btn.SetHeight(100);
            btn.SetWidth(100);
            btn.SetX(30);
            btn.SetY(30);
            btn.TextAlignment = TextAlignment.Center;
            mLayout.AddView(txtView);
            mLayout.AddView(btn);
            mLayout.AddView(surface);
            SetContentView(mLayout);
        }

        protected override void OnResume()
        {
            UrhoSurface.OnResume();
            base.OnResume();
        }

        protected override void OnPause()
        {
            UrhoSurface.OnPause();
            base.OnPause();
        }

        public override void OnLowMemory()
        {
            UrhoSurface.OnLowMemory();
            base.OnLowMemory();
        }

        protected override void OnDestroy()
        {
            UrhoSurface.OnDestroy();
            base.OnDestroy();
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (!UrhoSurface.DispatchKeyEvent(e))
                return false;
            return base.DispatchKeyEvent(e);
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            UrhoSurface.OnWindowFocusChanged(hasFocus);
            base.OnWindowFocusChanged(hasFocus);
        }
    }
}