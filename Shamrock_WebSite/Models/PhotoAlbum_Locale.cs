using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataType(typeof(PhotoAlbum_Locale.PhotoAlbum_LocaleMetadata))]
    public partial class PhotoAlbum_Locale
    {
        internal class PhotoAlbum_LocaleMetadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "DisplayName")]
            [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string DisplayName { get; set; }
        }
    }
}