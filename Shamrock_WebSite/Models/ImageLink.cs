using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataType(typeof(ImageLink.ImageLinkMetadata))]
    public partial class ImageLink
    {
        internal sealed class ImageLinkMetadata
        {
            [RegularExpression(@"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "InvalidUrl")]
            [Display(ResourceType = typeof(Resource), Name = "Link")]
            [MaxLength(400, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string Link { get; set; }

            [Display(ResourceType = typeof(Resource), Name = "Image")]
            public string ImagePath { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "Alternate")]
            [MaxLength(400, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string AlternateText { get; set; }

            [Display(ResourceType = typeof(Resource), Name = "ImageLinkCategory")]
            public ImageLinkCategory ImageLinkCategory { get; set; }
        }
    }
}