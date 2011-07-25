using System.ComponentModel.DataAnnotations;
using Shamrock_WebSite.App_GlobalResources;

namespace Shamrock_WebSite.Models
{
    [MetadataTypeAttribute(typeof(Comment.CommentMetadata))]
    public partial class Comment
    {
        internal sealed class CommentMetadata
        {
            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [MinLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MinLength")]
            [Display(ResourceType = typeof(Resource), Name = "Author")]
            [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string Author { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [RegularExpression(@"(?i)\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "InvalidEmail")]
            [Display(ResourceType = typeof(Resource), Name = "Email")]
            [MaxLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string Email { get; set; }

            [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
            [MinLength(5, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MinLength")]
            [DataType(DataType.MultilineText)]
            [Display(ResourceType = typeof(Resource), Name = "CommentBody")]
            [MaxLength(4000, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "MaxLength")]
            public string Body { get; set; }
        }
    }
}