using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Models.DashboardBlog
{
    public class CKEditorModel
    {
        public string CKEditorClass { get; set; }
        public string CKEditorHeight { get; set; }

        public CKEditorModel(string css, string height)
        {
            this.CKEditorClass = css;
            this.CKEditorHeight = height;
        }
    }
}
