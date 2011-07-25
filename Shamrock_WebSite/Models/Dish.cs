using System.ComponentModel.DataAnnotations;
using System.Linq;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataTypeAttribute(typeof(Dish.DishMetadata))]
    public partial class Dish
    {
        public Dish_Locale Locale
        {
            get
            {
                return Dishes_Locale.Single(dl => dl.Culture == SupportedCulture.Current);
            }
        }

        internal sealed class DishMetadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [MaxLength(16, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            [Display(ResourceType = typeof(Resource), Name = "Cost")]
            public string Cost { get; set; }
        }
    }
}