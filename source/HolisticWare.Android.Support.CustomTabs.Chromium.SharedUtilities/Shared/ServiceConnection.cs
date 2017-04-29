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

namespace
	//Xamarin.Android.Support.CustomTabs.Chromium.SharedUtilities
	HolisticWare.Android.Support.CustomTabs.Chromium.SharedUtilities
{

	/// <summary>
	/// Implementation for the CustomTabsServiceConnection that avoids leaking the
	/// ServiceConnectionCallback
	/// </summary>
	public class ServiceConnection : global::Android.Support.CustomTabs.CustomTabsServiceConnection
    {
		// A weak reference to the ServiceConnectionCallback to avoid leaking it.
		private System.WeakReference<IServiceConnectionCallback> mConnectionCallback;

		public ServiceConnection(IServiceConnectionCallback connectionCallback)
		{
			mConnectionCallback = new System.WeakReference<IServiceConnectionCallback>(connectionCallback);
		}

		public override void OnCustomTabsServiceConnected
                                (
                                    global::Android.Content.ComponentName name, 
                                    global::Android.Support.CustomTabs.CustomTabsClient client
                                )
		{
            IServiceConnectionCallback connectionCallback = null; //mConnectionCallback.Get();
            mConnectionCallback.TryGetTarget(out connectionCallback);//.Get();

			if (connectionCallback != null)
			{
				connectionCallback.OnServiceConnected(client);
			}
		}

		public override void OnServiceDisconnected(global::Android.Content.ComponentName name)
		{
            IServiceConnectionCallback connectionCallback = null; // mConnectionCallback.Get();
            mConnectionCallback.TryGetTarget(out connectionCallback);

			if (connectionCallback != null)
			{
				connectionCallback.OnServiceDisconnected();
			}
		}
	}

}