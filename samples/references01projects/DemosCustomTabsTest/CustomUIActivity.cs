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
using Android.Graphics;
using Android.Net;
using Android.Util;
using Android.Support.CustomTabs;

using Android.Support.V7.App;
using HolisticWare.Android.Support.CustomTabs.Chromium.SharedUtilities;

namespace DemosCustomTabsTest
{
	/// <summary>
	/// Opens Chrome Custom Tabs with a customized UI.
    /// 
    /// c# Attributes derived from
    ///          ./OriginalPortedFiles/AndroidManifest.xml 
	/// </summary>
    [
        Activity
        (
            Label = "@string/title_activity_customized_chrome_tab",
            Theme = "@style/AppTheme"
		)
    ]
	[MetaData("android.support.PARENT_ACTIVITY", Value = ".DemoListActivity")]
	public class CustomUIActivity : AppCompatActivity, View.IOnClickListener
	{
		private const string TAG = "CustChromeTabActivity";
		private const int TOOLBAR_ITEM_ID = 1;

		private EditText mUrlEditText;
		private EditText mCustomTabColorEditText;
		private EditText mCustomTabSecondaryColorEditText;
		private CheckBox mShowActionButtonCheckbox;
		private CheckBox mAddMenusCheckbox;
		private CheckBox mShowTitleCheckBox;
		private CheckBox mCustomBackButtonCheckBox;
		private CheckBox mAutoHideAppBarCheckbox;
		private CheckBox mAddDefaultShareCheckbox;
		private CheckBox mToolbarItemCheckbox;

        private CustomTabActivityHelper custom_tab_activity_helper;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
            SetContentView (Resource.Layout.activity_custom_ui);

			custom_tab_activity_helper = new CustomTabActivityHelper();
            FindViewById<Button>(Resource.Id.start_custom_tab).SetOnClickListener(this);

			mUrlEditText = FindViewById<EditText>(Resource.Id.url);
			mCustomTabColorEditText = FindViewById<EditText>(Resource.Id.custom_toolbar_color);
			mCustomTabSecondaryColorEditText = FindViewById<EditText>(Resource.Id.custom_toolbar_secondary_color);
			mShowActionButtonCheckbox = FindViewById<CheckBox>(Resource.Id.custom_show_action_button);
			mAddMenusCheckbox = FindViewById<CheckBox>(Resource.Id.custom_add_menus);
			mShowTitleCheckBox = FindViewById<CheckBox>(Resource.Id.show_title);
			mCustomBackButtonCheckBox = FindViewById<CheckBox>(Resource.Id.custom_back_button);
			mAutoHideAppBarCheckbox = FindViewById<CheckBox>(Resource.Id.auto_hide_checkbox);
			mAddDefaultShareCheckbox = FindViewById<CheckBox>(Resource.Id.add_default_share);
			mToolbarItemCheckbox = FindViewById<CheckBox>(Resource.Id.add_toolbar_item);
		}

		protected override void OnStart()
		{
			base.OnStart();
			custom_tab_activity_helper.BindCustomTabsService(this);
		}

		protected override void OnStop()
		{
			base.OnStop();
			custom_tab_activity_helper.UnbindCustomTabsService(this);
		}

		public void OnClick(View v)
		{
			int viewId = v.Id;
			switch (viewId)
			{
				case Resource.Id.start_custom_tab:
					OpenCustomTab();
					break;
				default:
					//Unknown View Clicked
			break;
			}
		}

        private int GetColor(EditText editText)
		{
			try
			{
				return Color.ParseColor(editText.Text.ToString());
			}
			catch (System.FormatException)
			{
				Log.Info(TAG, "Unable to parse Color: " + editText.Text);
				return Color.LightGray;
			}
		}

        private void OpenCustomTab()
		{
			string url = mUrlEditText.Text.ToString();

			int color = GetColor(mCustomTabColorEditText);
			int secondaryColor = GetColor(mCustomTabSecondaryColorEditText);

			CustomTabsIntent.Builder intentBuilder = new CustomTabsIntent.Builder();
            intentBuilder.SetToolbarColor(color);
            intentBuilder.SetSecondaryToolbarColor(secondaryColor);

			if (mShowActionButtonCheckbox.Checked)
			{
				//Generally you do not want to decode bitmaps in the UI thread. Decoding it in the
				//UI thread to keep the example short.
				string actionLabel = GetString(Resource.String.label_action);
				Bitmap icon = BitmapFactory.DecodeResource(Resources, Android.Resource.Drawable.IcMenuShare);
				PendingIntent pendingIntent = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_ACTION_BUTTON);
				intentBuilder.SetActionButton(icon, actionLabel, pendingIntent);
			}

			if (mAddMenusCheckbox.Checked)
			{
				string menuItemTitle = GetString(Resource.String.menu_item_title);
				PendingIntent menuItemPendingIntent = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_MENU_ITEM);
				intentBuilder.AddMenuItem(menuItemTitle, menuItemPendingIntent);
			}

			if (mAddDefaultShareCheckbox.Checked)
			{
				intentBuilder.AddDefaultShareMenuItem();
			}

			if (mToolbarItemCheckbox.Checked)
			{
				//Generally you do not want to decode bitmaps in the UI thread. Decoding it in the
				//UI thread to keep the example short.
				string actionLabel = GetString(Resource.String.label_action);
				Bitmap icon = BitmapFactory.DecodeResource(Resources, Android.Resource.Drawable.IcMenuShare);
                PendingIntent pendingIntent = CreatePendingIntent(CustomTabsActionsBroadcastReceiver.ACTION_TOOLBAR);
				intentBuilder.AddToolbarItem(TOOLBAR_ITEM_ID, icon, actionLabel, pendingIntent);
			}

            intentBuilder.SetShowTitle(mShowTitleCheckBox.Checked);

			if (mAutoHideAppBarCheckbox.Checked)
			{
				intentBuilder.EnableUrlBarHiding();
			}

			if (mCustomBackButtonCheckBox.Checked)
			{
                intentBuilder.SetCloseButtonIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ic_arrow_back));
			}

			intentBuilder.SetStartAnimations(this, Resource.Animation.slide_in_right, Resource.Animation.slide_out_left);
			intentBuilder.SetExitAnimations(this, Android.Resource.Animation.SlideInLeft, Android.Resource.Animation.SlideOutRight);

            new CustomTabActivityHelper().LaunchUrlWithCustomTabsOrFallback
                                    (
                                       this, 
                                       intentBuilder.Build(), 
                                       Uri.Parse(url), 
                                       new WebViewFallback()
                                    );

            return;
		}

		private PendingIntent CreatePendingIntent(int actionSourceId)
		{
			Intent actionIntent = new Intent(this.ApplicationContext, typeof(CustomTabsActionsBroadcastReceiver));
			actionIntent.PutExtra(CustomTabsActionsBroadcastReceiver.KEY_ACTION_SOURCE, actionSourceId);
			return PendingIntent.GetBroadcast(ApplicationContext, actionSourceId, actionIntent, 0);
		}
	}

}