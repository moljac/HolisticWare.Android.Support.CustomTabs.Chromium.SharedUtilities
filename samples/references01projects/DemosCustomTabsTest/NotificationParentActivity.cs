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
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Net;
using Android.Support.V7.App;

using Android.Support.CustomTabs;
using Android.Support.CustomTabs.Chromium.SharedUtilities;

namespace DemosCustomTabsTest
{
	/// <summary>
	/// This is the Activity that the Notification takes the user to.
	/// 
	/// The Notification sends the user to this activity with an EXTRA_URL. Use it to call Chrome Custom
	/// Tabs. The user will return to it when coming back from the web content.
    /// 
    /// c# Attributes derived from
    ///          ./OriginalPortedFiles/AndroidManifest.xml 
	/// </summary>
    [
        Activity
        (
            Label = "@string/title_activity_notification_parent",
            Theme = "@style/AppTheme"
		)
    ]
	[MetaData("android.support.PARENT_ACTIVITY", Value = ".DemoListActivity")]
	public class NotificationParentActivity : AppCompatActivity, View.IOnClickListener
	{
		private const int NOTIFICATION_ID = 1;
		public const string EXTRA_URL = "extra.url";

		private View mMessageTextView;
		private View mCreateNotificationButton;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_notification_parent);
			mMessageTextView = FindViewById<TextView>(Resource.Id.back_to_app_message);
			mCreateNotificationButton = FindViewById<TextView>(Resource.Id.create_notification);
            mCreateNotificationButton.SetOnClickListener(this);
			startChromeCustomTab(Intent);
		}

		protected override void OnNewIntent(Intent intent)
		{
			startChromeCustomTab(intent);
		}

		public void OnClick(View v)
		{
			int viewId = v.Id;
			switch (viewId)
			{
				case Resource.Id.create_notification:
					CreateAndShowNotification();
					Finish();
					break;
				default:
					//Unknown view clicked
			break;
			}
		}

		private void CreateAndShowNotification()
		{
			NotificationCompat.Builder mBuilder = 
                (Android.Support.V7.App.NotificationCompat.Builder)
                (new NotificationCompat.Builder(this))
                        .SetSmallIcon(Resource.Drawable.abc_popup_background_mtrl_mult)
                        .SetContentTitle(GetString(Resource.String.notification_title))
                        .SetContentText(GetString(Resource.String.notification_text));

			Intent resultIntent = new Intent(this.ApplicationContext, typeof(NotificationParentActivity));

			resultIntent.PutExtra(NotificationParentActivity.EXTRA_URL, GetString(Resource.String.notification_sample_url));

            resultIntent.SetAction(Intent.ActionView);

			PendingIntent pendingIntent = PendingIntent.GetActivity(this.ApplicationContext, 0, resultIntent, 0);

            mBuilder.SetContentIntent(pendingIntent);
            mBuilder.SetAutoCancel(true);
			NotificationManager mNotificationManager = (NotificationManager) GetSystemService(Context.NotificationService);
			// mId allows you to update the notification later on.
			mNotificationManager.Notify(NOTIFICATION_ID, mBuilder.Build());
		}

		private void startChromeCustomTab(Intent intent)
		{
			string url = intent.GetStringExtra(EXTRA_URL);
			if (url != null)
			{
				Uri uri = Uri.Parse(url);

				int tabcolor = Resources.GetColor(Resource.Color.primaryColor);
				CustomTabsIntent customTabsIntent = (new CustomTabsIntent.Builder()).SetToolbarColor(tabcolor).Build();
				CustomTabActivityHelper.OpenCustomTab(this, customTabsIntent, uri, new WebviewFallback());

				mMessageTextView.Visibility = ViewStates.Visible;
				mCreateNotificationButton.Visibility = ViewStates.Gone;
			}
			else
			{
				mMessageTextView.Visibility = ViewStates.Gone;
				mCreateNotificationButton.Visibility = ViewStates.Visible;
			}
		}
	}

}