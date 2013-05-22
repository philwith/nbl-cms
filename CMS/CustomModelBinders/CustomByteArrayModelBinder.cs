using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBL.CMS.CustomModelBinders
{
    public class CustomByteArrayModelBinder : ByteArrayModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var file = controllerContext.HttpContext.Request.Files[bindingContext.ModelName];

            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileBytes = new byte[file.ContentLength];
                    file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                    return fileBytes;
                }

                return null;
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}