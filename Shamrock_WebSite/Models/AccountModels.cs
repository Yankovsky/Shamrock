using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    public class LogOnModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }
    }
}