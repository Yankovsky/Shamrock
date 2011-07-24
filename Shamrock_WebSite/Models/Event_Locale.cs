using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataTypeAttribute(typeof(Event_Locale.Event_LocaleMetadata))]
    public partial class Event_Locale
    {
        internal sealed class Event_LocaleMetadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "DisplayName")]
            [MaxLength(40, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string DisplayName { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [DataType(DataType.MultilineText)]
            [Display(ResourceType = typeof(Resource), Name = "Description")]
            [MaxLength(4000, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string Description { get; set; }
        }
    }
}