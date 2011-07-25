using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataType(typeof(StaffMember.StaffMember_Metadata))]
    public partial class StaffMember
    {
        internal sealed class StaffMember_Metadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [MaxLength(400, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            [Display(ResourceType = typeof(Resource), Name = "DisplayName")]
            public string Name { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [Display(ResourceType = typeof(Resource), Name = "PhoneNumber")]
            [RegularExpression(@"79\d{9}", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "InvalidPhone")]
            public string Phone { get; set; }
        }
    }
}