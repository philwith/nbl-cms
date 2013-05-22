using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;
using System.ComponentModel;
using System.Web.Mvc;

namespace NBL.CMS.Models
{

#region BIMObject

    public abstract class BIMObjectViewModel
    {
        [ScaffoldColumn(false)]
        public int ID { get; set; }
        [Required(ErrorMessage = "A name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "A description is required")]
        public string Description { get; set; }
        //public BIMObjectShortUrlViewModel ShortUrl { get; set; }
    }

    public class BIMObjectEditViewModel : BIMObjectViewModel
    {
        [Required(ErrorMessage = "A description is required")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string HtmlDescription { get; set; }
        public virtual BIMObjectShortUrlViewModel ShortUrl { get; set; }
        public virtual BIMThumbnailViewModel BIMThumbnail { get; set; }
    }

    public class BIMObjectListViewModel : BIMObjectViewModel
    {
        [DisplayName("State")]
        public int StateFK { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0}")]
        public double Version { get; set; }
    }

    public class BIMObjectCreateViewModel : BIMObjectViewModel
    {
        public BIMObjectCreateViewModel()
        {
            CreatedOn = DateTime.Now;
            CreatedBy = Membership.GetUser().Email;
            StateFK = 4;
            Version = 1;
        }

        [Required(ErrorMessage = "An identifier is required")]
        public string Identifier { get; set; }
        [Required(ErrorMessage = "A description is required")]
        public string HtmlDescription { get; set; }
        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? UpdatedOn { get; set; }
        [ScaffoldColumn(false)]
        public String CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public String UpdatedBy { get; set; }
        [ScaffoldColumn(false)]
        public int StateFK { get; set; }
        [ScaffoldColumn(false)]
        public double Version { get; set; }
    }

    public class BIMObjectDetailsViewModel : BIMObjectViewModel
    {     
        public string Identifier { get; set; }
        public string HtmlDescription { get; set; }
        [DisplayName("Created On")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Updated On")]
        [DisplayFormat(NullDisplayText="Never")]
        public DateTime? UpdatedOn { get; set; }
        [DisplayName("Created By")]
        public String CreatedBy { get; set; }
        [DisplayName("Updated By")]
        [DisplayFormat(NullDisplayText = "Nobody")]
        public String UpdatedBy { get; set; }
        [DisplayName("State")]
        public int StateFK { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0}")]
        public double Version { get; set; }
    }

#endregion

#region ShortUrl
    public class BIMObjectShortUrlViewModel
    {
        //[ScaffoldColumn(false)]
        //public int BIMObjectFK { get; set; }
        [DisplayName("Short Url")]
        [Required(ErrorMessage = "A url is required")]
        public string Url { get; set; }
    }
#endregion


#region BIMThumbnail
    public class BIMThumbnailViewModel
    {
        //[ScaffoldColumn(false)]
        //public int BIMObjectFK { get; set; }
        //[DisplayName("Thumbnail")]
        //public HttpPostedFileBase PostedFile { get; set; }
        [DisplayName("Thumbnail")]
        public Byte[] FileData { get; set; }
        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? UpdatedOn { get; set; }
        [ScaffoldColumn(false)]
        public String CreatedBy { get; set; }
        [ScaffoldColumn(false)]
        public String UpdatedBy { get; set; }

    }
#endregion

}