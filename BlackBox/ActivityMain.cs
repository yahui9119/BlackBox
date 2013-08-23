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
using Java.Interop;
using Android.Webkit;

namespace BlackBox
{
    [Activity(Label = "黑匣子", MainLauncher = true, Icon = "@drawable/icon")]
    public class ActivityMain : Activity
    {
        const string html = @"
	<html>
	<body>
	<p>This is a paragraph.</p>
	<button type=""button"" onClick=""Foo.bar('test message')"">Click Me!</button>
	</body>
	</html>";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.WebMain);


            WebView view = FindViewById<WebView>(Resource.Id.webViewMain);
            view.Settings.JavaScriptEnabled = true;
            view.SetWebChromeClient(new WebChromeClient());
            view.AddJavascriptInterface(new Foo(this), "Foo");
            view.LoadData(html, "text/html", null);

        }
    }
    /// <summary>
    /// js调用的类
    /// </summary>
    class Foo : Java.Lang.Object
    {
        public Foo(Context context)
        {
            this.context = context;
        }

        public Foo(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        Context context;

        [Export("bar")]
        // to become consistent with Java/JS interop convention, the argument cannot be System.String.
        public void Bar(Java.Lang.String message)
        {
            Console.WriteLine("Foo.Bar invoked!");
            Toast.MakeText(context, "This is a Toast from C#! " + message, ToastLength.Short).Show();
        }
    }
}