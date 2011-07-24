using System.ComponentModel.DataAnnotations;
using System.Linq;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    public partial class DishCategory
    {
        public DishCategory_Locale Locale
        {
            get
            {
                return DishCategories_Locale.Single(el => el.Culture == SupportedCulture.Current);
            }
        }
    }
}