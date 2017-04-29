// Copyright 2015 Google Inc. All Rights Reserved.
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
using Android.Widget;
using Android.Net;
using Android.Support.V7.App;

using Android.Support.CustomTabs;
using HolisticWare.Android.Support.CustomTabs.Chromium.SharedUtilities;

namespace DemosCustomTabsTest
{
	/// <summary>
	/// The simplest way to use Chrome Custom Tabs. Without any customization or speeding process.
    /// 
    /// c# Attributes derived from
    ///          ./OriginalPortedFiles/AndroidManifest.xml 
	/// </summary>
    [
        Activity
            (
                Label = "@string/title_activity_simple_chrome_tab",
                Theme = "@style/AppTheme"
			)
    ]
	[MetaData("android.support.PARENT_ACTIVITY", Value = ".DemoListActivity")]
	public class SimpleCustomTabActivity : AppCompatActivity, View.IOnClickListener
	{
		private EditText mUrlEditText;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_simple_custom_tab);

            FindViewById<Button>(Resource.Id.start_custom_tab).SetOnClickListener(this);

			mUrlEditText = FindViewById<EditText>(Resource.Id.url);
		}

		public void OnClick(View v)
		{
			int viewId = v.Id;

			switch (viewId)
			{
				case Resource.Id.start_custom_tab:
					string url = mUrlEditText.Text.ToString();
					CustomTabsIntent customTabsIntent = (new CustomTabsIntent.Builder()).Build();
                    new CustomTabActivityHelper().LaunchUrlWithCustomTabsOrFallback
                                                        (
                                                            this, 
                                                            customTabsIntent, 
                                                            Uri.Parse(url),
                                                            new WebViewFallback()
                                                        );
					break;
				default:
					//Unknown View Clicked
			break;
			}
		}
	}

}