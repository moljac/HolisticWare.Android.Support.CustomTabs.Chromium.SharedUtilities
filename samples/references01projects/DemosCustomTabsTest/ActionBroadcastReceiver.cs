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

using Android.Content;
using Android.Widget;

using DemosCustomTabsTest;

namespace DemosCustomTabsTest
{
    /// <summary>
    /// A BroadcastReceiver that handles the Action Intent from the Custom Tab and shows the Url
    /// in a Toast.
    /// </summary>
    [BroadcastReceiver]
	public class ActionBroadcastReceiver : BroadcastReceiver
	{
		public const string KEY_ACTION_SOURCE = "org.chromium.customtabsdemos.ACTION_SOURCE";
		public const int ACTION_ACTION_BUTTON = 1;
		public const int ACTION_MENU_ITEM = 2;
		public const int ACTION_TOOLBAR = 3;

		public override void OnReceive(Context context, Intent intent)
		{
			string url = intent.DataString;
			if (url != null)
			{
				string toastText = getToastText(context, intent.GetIntExtra(KEY_ACTION_SOURCE, -1), url);
				Toast.MakeText(context, toastText, ToastLength.Short).Show();
			}
		}

		private string getToastText(Context context, int actionId, string url)
		{
			switch (actionId)
			{
				case ACTION_ACTION_BUTTON:
					return context.GetString(Resource.String.action_button_toast_text, url);
				case ACTION_MENU_ITEM:
					return context.GetString(Resource.String.menu_item_toast_text, url);
				case ACTION_TOOLBAR:
					return context.GetString(Resource.String.toolbar_toast_text, url);
				default:
					return context.GetString(Resource.String.unknown_toast_text, url);
			}
		}
	}

}