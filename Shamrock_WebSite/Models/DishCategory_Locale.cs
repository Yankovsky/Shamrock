using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataTypeAttribute(typeof(DishCategory_Locale.DishCategory_LocaleMetadata))]
    public partial class DishCategory_Locale
    {
        internal sealed class DishCategory_LocaleMetadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [MaxLength(32, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            [Display(ResourceType = typeof(Resource), Name = "DisplayName")]
            public string DisplayName { get; set; }
        }
    }
}