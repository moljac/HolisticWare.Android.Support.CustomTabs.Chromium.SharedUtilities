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

using Android.Content;
using Android.Widget;


namespace
    //Xamarin.Android.Support.CustomTabs.Chromium.SharedUtilities
    HolisticWare.Android.Support.CustomTabs.Chromium.SharedUtilities
{
    /// <summary>
    /// A BroadcastReceiver that handles the Action Intent from the Custom Tab and shows the Url
    /// in a Toast.
    /// </summary>
    [BroadcastReceiver]
    public class CustomTabsActionsBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();

            string url = intent.DataString;
            if (url != null)
            {
                string toastText = GetToastText
                                    (
                                        context,
                                        intent.GetIntExtra
                                        (
                                            KEY_ACTION_SOURCE,
                                            -1
                                        ),
                                        url
                                    );
                Toast.MakeText(context, toastText, ToastLength.Short).Show();
            }

            return;
        }

        public const string KEY_ACTION_SOURCE = "org.chromium.customtabsdemos.ACTION_SOURCE";
        public const int ACTION_ACTION_BUTTON = 1;
        public const int ACTION_MENU_ITEM = 2;
        public const int ACTION_TOOLBAR = 3;

        private static string GetToastText(Context context, int actionId, string url)
        {
            switch (actionId)
            {
                case ACTION_ACTION_BUTTON:
                    return
                        //context.GetString(Resource.String.action_button_toast_text, url)
                        ActionButtonToastText
                        ;
                case ACTION_MENU_ITEM:
                    return
                        //context.GetString(Resource.String.menu_item_toast_text, url)
                        MenuItemToastText
                        ;
                case ACTION_TOOLBAR:
                    return
                        //context.GetString(Resource.String.toolbar_toast_text, url)
                        ToolBarToastText
                        ;
                default:
                    return
                        //context.GetString(Resource.String.unknown_toast_text, url)
                        UnknownToastText
                        ;
            }

            return "Something is wrong?!?!";
        }

        public static string ActionButtonToastText
        {
            get;
            set;
        } = "ActionButtonToastText";

        public static string MenuItemToastText
        {
            get;
            set;
        } = "MenuItemToastText";

        public static string ToolBarToastText
        {
            get;
            set;
        } = "ToolBarToastText";

        public static string UnknownToastText
        {
            get;
            set;
        } = "";

    }

}