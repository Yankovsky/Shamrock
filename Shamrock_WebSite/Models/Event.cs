using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataTypeAttribute(typeof(Event.EventMetadata))]
    public partial class Event
    {
        public Event_Locale Locale
        {
            get
            {
                return Events_Locale.Single(el => el.Culture == SupportedCulture.Current);
            }
        }

        internal sealed class EventMetadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "Date")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yy}")]
            public DateTime Date { get; set; }

            [Display(ResourceType = typeof(Resource), Name = "EventType")]
            public EventType EventType { get; set; }
        }
    }
}