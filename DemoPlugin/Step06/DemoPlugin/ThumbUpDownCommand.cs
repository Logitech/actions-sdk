namespace Loupedeck.DemoPlugin
{
    using System;

    public class ThumbUpDownCommand : PluginDynamicCommand
    {
        private Boolean _toggleState = false;

        private readonly String _imageResourcePathThumbUp;
        private readonly String _imageResourcePathThumbDown;

        public ThumbUpDownCommand() : base(displayName: "Thumb up/down", description: null, groupName: "Switches")
        {
            this._imageResourcePathThumbUp = EmbeddedResources.FindFile("ThumbUp.png");
            this._imageResourcePathThumbDown = EmbeddedResources.FindFile("ThumbDown.png");
        }

        protected override void RunCommand(String actionParameter)
        {
            this._toggleState = !this._toggleState;
            this.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            var resourcePath = this._toggleState ? this._imageResourcePathThumbDown : this._imageResourcePathThumbUp;
            return EmbeddedResources.ReadImage(resourcePath);
        }
    }
}
