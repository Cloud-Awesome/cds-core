namespace CloudAwesome.Xrm.Core.PluginModel
{
    public class EntityImage
    {
        private string name;

        public EntityImage()
        {
        }

        public EntityImage(EntityImageType imageType, params string[] attributes)
        {
            this.ImageType = imageType;
            this.Attributes = attributes;
        }

        public string[] Attributes { get; set; }

        public EntityImageType ImageType { get; set; }

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = ImageType.ToString();
                }

                return name;
            }
            set => name = value;
        }
    }
}

public enum EntityImageType
{
    PreEntityImage = 0, PostEntityImage = 1
}