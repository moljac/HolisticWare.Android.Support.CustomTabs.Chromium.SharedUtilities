﻿// Copyright 2015 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Support.V7.App;

namespace DemosCustomTabsTest
{
	/// <summary>
	/// This Activity is used as a fallback when there is no browser installed that supports
	/// Chrome Custom Tabs
    /// 
    /// c# Attributes derived from
    ///          ./OriginalPortedFiles/AndroidManifest.xml 
	/// </summary>
    [
        Activity
        (
            Label = "@string/title_activity_webview",
            Theme = "@style/AppTheme"
		)
    ]
	[MetaData("android.support.PARENT_ACTIVITY", Value = ".DemoListActivity")]
	public class WebviewActivity : AppCompatActivity
	{
		public const string EXTRA_URL = "extra.url";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_webview);
			string url = Intent.GetStringExtra(EXTRA_URL);
			WebView webView = FindViewById<WebView>(Resource.Id.webview);
            webView.SetWebViewClient(new WebViewClient());
			WebSettings webSettings = webView.Settings;
			webSettings.JavaScriptEnabled = true;
			Title = url;
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			webView.LoadUrl(url);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				// Respond to the action bar's Up/Home button
				case Android.Resource.Id.Home:
					Finish();
					return true;
			}
			return base.OnOptionsItemSelected(item);
		}
	}

}