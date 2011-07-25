using System;
using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataTypeAttribute(typeof(TableReservation.TableReservationMetadata))]
    public partial class TableReservation
    {
        internal sealed class TableReservationMetadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Range(1, 38)]
            public int TableId { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "Date")]
            public DateTime Date { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "DisplayName")]
            [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string Name { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "PhoneNumber")]
            [MaxLength(20, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "Time")]
            [MaxLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "InvalidTime")]
            [MinLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "InvalidTime")]
            public string Time { get; set; }

            [Display(ResourceType = typeof(Resource), Name = "Wishes")]
            [MaxLength(400, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            [DataType(DataType.MultilineText)]
            public string Wishes { get; set; }
        }
    }
}