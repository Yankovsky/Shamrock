using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataTypeAttribute(typeof(Dish_Locale.Dish_LocaleMetadata))]
    public partial class Dish_Locale
    {
        internal sealed class Dish_LocaleMetadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [MaxLength(64, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            [Display(ResourceType = typeof(Resource), Name = "DisplayName")]
            public string DisplayName { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [MaxLength(16, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            [Display(ResourceType = typeof(Resource), Name = "Portion")]
            public string Portion { get; set; }
        }
    }
}