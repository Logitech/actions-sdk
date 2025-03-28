namespace Loupedeck.DemoPlugin
{
    using System;

    public class ThumbUpDownCommand : PluginDynamicCommand
    {
        private Boolean _isThumbDown = false;

        private readonly String _imageResourcePathThumbUp;
        private readonly String _imageResourcePathThumbDown;

        public ThumbUpDownCommand() : base(displayName: "Thumb up/down", description: null, groupName: "Switches")
        {
            this._imageResourcePathThumbUp = PluginResources.FindFile("ThumbUp.png");
            this._imageResourcePathThumbDown = PluginResources.FindFile("ThumbDown.png");
        }

        protected override void RunCommand(String actionParameter)
        {
            this._isThumbDown = !this._isThumbDown;
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var resourcePath = this._isThumbDown ? this._imageResourcePathThumbDown : this._imageResourcePathThumbUp;
            return PluginResources.ReadImage(resourcePath);
        }
    }
}
