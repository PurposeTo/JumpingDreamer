// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

using GoogleMobileAds;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.Api
{
    public class RewardBasedVideoAd
    {
        private IRewardBasedVideoAdClient client;

        public static RewardBasedVideoAd Instance { get; } = new RewardBasedVideoAd();

        // Creates a Singleton RewardBasedVideoAd.
        private RewardBasedVideoAd()
        {
            this.client = MobileAds.GetClientFactory().BuildRewardBasedVideoAdClient();
            client.CreateRewardBasedVideoAd();

            this.client.OnAdLoaded += (sender, args) =>
            {
                this.OnAdLoaded?.Invoke(this, args);
            };

            this.client.OnAdFailedToLoad += (sender, args) =>
            {
                this.OnAdFailedToLoad?.Invoke(this, args);
            };

            this.client.OnAdOpening += (sender, args) =>
            {
                this.OnAdOpening?.Invoke(this, args);
            };

            this.client.OnAdStarted += (sender, args) =>
            {
                this.OnAdStarted?.Invoke(this, args);
            };

            this.client.OnAdClosed += (sender, args) =>
            {
                this.OnAdClosed?.Invoke(this, args);
            };

            this.client.OnAdLeavingApplication += (sender, args) =>
            {
                this.OnAdLeavingApplication?.Invoke(this, args);
            };

            this.client.OnAdRewarded += (sender, args) =>
            {
                this.OnAdRewarded?.Invoke(this, args);
            };

            this.client.OnAdCompleted += (sender, args) =>
            {
                this.OnAdCompleted?.Invoke(this, args);
            };
        }

        // These are the ad callback events that can be hooked into.
        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        public event EventHandler<EventArgs> OnAdOpening;

        public event EventHandler<EventArgs> OnAdStarted;

        public event EventHandler<EventArgs> OnAdClosed;

        public event EventHandler<Reward> OnAdRewarded;

        public event EventHandler<EventArgs> OnAdLeavingApplication;

        public event EventHandler<EventArgs> OnAdCompleted;

        // Loads a new reward based video ad request
        public void LoadAd(AdRequest request, string adUnitId)
        {
            client.LoadAd(request, adUnitId);
        }

        // Determines whether the reward based video has loaded.
        public bool IsLoaded()
        {
            return client.IsLoaded();
        }

        // Shows the reward based video.
        public void Show()
        {
            client.ShowRewardBasedVideoAd();
        }

        // Sets the user id of current user.
        public void SetUserId(string userId)
        {
            client.SetUserId(userId);
        }

        // Returns the mediation adapter class name.
        public string MediationAdapterClassName()
        {
            return this.client.MediationAdapterClassName();
        }
    }
}
