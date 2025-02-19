using System;

namespace Loupedeck.DemoPlugin
{
    public class ThumbUpDownCommand : PluginDynamicCommand
    {
        private bool isThumbsDown = false;
        private readonly string imageResourcePathThumbUp;
        private readonly string imageResourcePathThumbDown;

        public ThumbUpDownCommand() : base(
            displayName: "Thumb up/down",
            description: null,
            groupName: "Switches")
        {
            this.imageResourcePathThumbUp = EmbeddedResources.FindFile("ThumbUp.png");
            this.imageResourcePathThumbDown = EmbeddedResources.FindFile("ThumbDown.png");
        }

        protected override void RunCommand(string actionParameter)
        {
            this.isThumbsDown = !isThumbsDown;
            base.ActionImageChanged();
        }

        protected override BitmapImage GetCommandImage(string actionParameter, PluginImageSize imageSize)
        {
            var resourcePath = isThumbsDown ? imageResourcePathThumbDown : imageResourcePathThumbUp;
            return EmbeddedResources.ReadImage(resourcePath);
        }
    }
}
