using System;
using System.Collections.Generic;
using DemosCustomTabsTest;

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
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace DemosCustomTabsTest
{
    /// <summary>
    /// Demo list activity.
    /// 
    /// c# Attributes derived from
    ///          ./OriginalPortedFiles/AndroidManifest.xml 
    /// </summary>
	[
        Activity
            (
                Label = "DemoListActivity", 
                Name = "net.holisticware.customtabsdemos.DemoListActivity", 
                MainLauncher = true, 
                Icon = "@mipmap/ic_launcher",
                //Theme = "@style/AppTheme"
            )
    ]
	[IntentFilter(new[] { "android.intent.action.MAIN" })]
	public class DemoListActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            IList<ActivityDesc> activityDescList = new List<ActivityDesc>();
            ActivityListAdapter listAdapter = new ActivityListAdapter(this, activityDescList);

            ActivityDesc activityDesc = CreateActivityDesc
                                            (
                                                Resource.String.title_activity_simple_chrome_tab, 
                                                Resource.String.description_activity_simple_chrome_tab, 
                                                //typeof(SimpleCustomTabActivity)
                                                new SimpleCustomTabActivity()
                                            );
            activityDescList.Add(activityDesc);

            activityDesc = CreateActivityDesc
                                            (
                                                Resource.String.title_activity_service_connection, 
                                                Resource.String.description_activity_service_connection, 
                                                // typeof(ServiceConnectionActivity)
                                               new ServiceConnectionActivity()
                                            );
            activityDescList.Add(activityDesc);

            activityDesc = CreateActivityDesc
                                            (
                                                Resource.String.title_activity_customized_chrome_tab, 
                                                Resource.String.description_activity_customized_chrome_tab, 
                                                //typeof(CustomUIActivity)
                                                new CustomUIActivity()
                                            );
            activityDescList.Add(activityDesc);

            activityDesc = CreateActivityDesc
                                            (
                                                Resource.String.title_activity_notification_parent, 
                                                Resource.String.title_activity_notification_parent, 
                                                //typeof(NotificationParentActivity)
                                                new NotificationParentActivity()
                                            );
            activityDescList.Add(activityDesc);

            RecyclerView recyclerView = FindViewById<RecyclerView>(Android.Resource.Id.List);
            recyclerView.SetAdapter(listAdapter);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));
        }

        private ActivityDesc CreateActivityDesc(int titleId, int descriptionId, Activity activity) //where T1 : Android.App.Activity
        {
            ActivityDesc activityDesc = new ActivityDesc();
            activityDesc.mTitle = GetString(titleId);
            activityDesc.mDescription = GetString(descriptionId);
            activityDesc.mActivity = activity;
            return activityDesc;
        }

        private class ActivityDesc
        {
            internal string mTitle;
            internal string mDescription;
            //JAVA TO C# CONVERTER TODO TASK: Java wildcard generics are not converted to .NET:
            //ORIGINAL LINE: Class<? extends Android.App.Activity> mActivity;
            internal Activity mActivity;
        }

        private class ViewHolder : RecyclerView.ViewHolder
        {
            /* package */
            internal TextView mTitleTextView;
            /* package */
            internal TextView mDescriptionTextView;
            /* package */
            internal int mPosition;

            public ViewHolder(View itemView) : base(itemView)
            {
                this.mTitleTextView = itemView.FindViewById<TextView>(Resource.Id.title);
                this.mDescriptionTextView = itemView.FindViewById<TextView>(Resource.Id.description);
            }
        }

        private class ActivityListAdapter : RecyclerView.Adapter/*<ViewHolder>*/, View.IOnClickListener
        {
			internal Context mContext;
            internal LayoutInflater mLayoutInflater;
            internal IList<ActivityDesc> mActivityDescs;

            public ActivityListAdapter(Context context, IList<ActivityDesc> activityDescs)
            {
                this.mActivityDescs = activityDescs;
                this.mContext = context;
                mLayoutInflater = LayoutInflater.From(context);
            }

            public void OnClick(View v)
            {
                int position = ((ViewHolder)v.Tag).mPosition;
                ActivityDesc activityDesc = mActivityDescs[position];
                Type t = ((object) (activityDesc.mActivity)).GetType();
                Intent intent = new Intent(mContext, t);
                mContext.StartActivity(intent);
            }


			public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
			{
				View v = mLayoutInflater.Inflate(Resource.Layout.item_example_description, parent, false);
				ViewHolder viewHolder = new ViewHolder(v);
				v.SetOnClickListener(this);
				v.Tag = viewHolder;

				return viewHolder;
			}

			public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                ViewHolder vh = viewHolder as ViewHolder;

                //JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                //ORIGINAL LINE: final ActivityDesc activityDesc = mActivityDescs.get(position);
                ActivityDesc activityDesc = mActivityDescs[position];
                string title = activityDesc.mTitle;
                string description = activityDesc.mDescription;

                vh.mTitleTextView.Text = title;
                vh.mDescriptionTextView.Text = description;
                vh.mPosition = position;
            }

            public override int ItemCount
            {
                get
                {
                    return mActivityDescs.Count;
                }
            }
        }
    }

}