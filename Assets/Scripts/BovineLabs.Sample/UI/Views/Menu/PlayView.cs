// <copyright file="PlayView.cs" company="BovineLabs">
//     Copyright (c) BovineLabs. All rights reserved.
// </copyright>

namespace BovineLabs.Sample.UI.Views.Menu
{
    using BovineLabs.Anchor;
    using BovineLabs.Sample.UI.ViewModels.Menu;
    using Unity.Properties;
    using UnityEngine.UIElements;
    using Button = Unity.AppUI.UI.Button;

    public class PlayView : MenuBaseView<HomeViewModel>
    {
        public const string UssClassName = "bl-play-view";

        private const string PrivateText = "@UI:privateGame";
        private const string PrivateSubText = "@UI:privateGameSub";
        private const string HostText = "@UI:hostGame";
        private const string HostSubText = "@UI:hostGameSub";
        private const string JoinText = "@UI:joinGame";
        private const string JoinSubText = "@UI:joinGameSub";

        public PlayView(HomeViewModel viewModel)
            : base(viewModel)
        {
            this.AddToClassList(UssClassName);
            
        }

        private void PrivateGame()
        {
            this.ViewModel.Value.Play.TryProduce();
            this.ToGoLoading();
        }

        private void ToGoLoading()
        {
            this.Navigate(Actions.go_to_loading);
        }
    }
}
