using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using MicroBee.Droid;
using Plugin.CurrentActivity;

namespace MicroBee.Droid
{
    [Activity(Label = "MicroBee", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
			// plugin initialization
	        Xamarin.Essentials.Platform.Init(this, bundle);
	        CrossCurrentActivity.Current.Init(this, bundle);
			global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

	    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
	    {
		    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		    Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
	    }
	}
}

