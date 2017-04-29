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
    /// This Activity connect to the Chrome Custom Tabs Service on startup, and allows you to decide
    /// when to call mayLaunchUrl.
    /// 
    /// c# Attributes derived from
    ///          ./OriginalPortedFiles/AndroidManifest.xml 
    /// </summary>
    [
        Activity
        (
            Label = "@string/title_activity_service_connection",
            Theme = "@style/AppTheme"
        )
    ]
    [MetaData("android.support.PARENT_ACTIVITY", Value = ".DemoListActivity")]
    public class ServiceConnectionActivity : AppCompatActivity, View.IOnClickListener, IConnectionCallback
    {
        private EditText mUrlEditText;
        private View mMayLaunchUrlButton;
        private CustomTabActivityHelper customTabActivityHelper;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_serviceconnection);

            customTabActivityHelper = new CustomTabActivityHelper();
            customTabActivityHelper.ConnectionCallback = this; //IConnectionCallback

            mUrlEditText = FindViewById<EditText>(Resource.Id.url);
            mMayLaunchUrlButton = FindViewById<Button>(Resource.Id.button_may_launch_url);
            mMayLaunchUrlButton.Enabled = false;
            mMayLaunchUrlButton.SetOnClickListener(this);

            FindViewById<Button>(Resource.Id.start_custom_tab).SetOnClickListener(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            customTabActivityHelper.ConnectionCallback = null;
        }

        public virtual void OnCustomTabsConnected()
        {
            mMayLaunchUrlButton.Enabled = true;
        }

        public virtual void OnCustomTabsDisconnected()
        {
            mMayLaunchUrlButton.Enabled = false;
        }

        protected override void OnStart()
        {
            base.OnStart();
            customTabActivityHelper.BindCustomTabsService(this);
        }

        protected override void OnStop()
        {
            base.OnStop();
            customTabActivityHelper.UnbindCustomTabsService(this);
            mMayLaunchUrlButton.Enabled = false;
        }

        public void OnClick(View view)
        {
            int viewId = view.Id;
            Uri uri = Uri.Parse(mUrlEditText.Text.ToString());
            switch (viewId)
            {
                case Resource.Id.button_may_launch_url:
                    customTabActivityHelper.MayLaunchUrl(uri, null, null);
                    break;
                case Resource.Id.start_custom_tab:
                    CustomTabsIntent customTabsIntent = (new CustomTabsIntent.Builder(customTabActivityHelper.Session)).Build();
                    new CustomTabActivityHelper().LaunchUrlWithCustomTabsOrFallback
                                                        (
                                                            this, 
                                                            customTabsIntent, 
                                                            uri, 
                                                            new WebViewFallback()
                                                        );
                    break;
                default:
                    //Unkown View Clicked
                    break;
            }
        }
    }

}