using System.Linq;

namespace Shamrock_WebSite.Models
{
    public partial class PhotoAlbum
    {
        public PhotoAlbum_Locale Locale
        {
            get
            {
                return PhotoAlbums_Locale.Single(el => el.Culture == SupportedCulture.Current);
            }
        }
    }
}