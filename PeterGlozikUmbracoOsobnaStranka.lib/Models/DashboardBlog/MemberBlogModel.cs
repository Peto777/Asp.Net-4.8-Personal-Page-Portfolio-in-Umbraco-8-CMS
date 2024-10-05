using NPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PeterGlozikUmbracoOsobnaStranka.lib.Repositories;
using System.Web.Mvc;
using dufeksoft.lib.ParamSet;
using PeterGlozikUmbracoOsobnaStranka.lib.Util;
using System.Xml;
using dufeksoft.lib.UI;
using dufeksoft.lib.Model.Grid;
using System.Web;

namespace PeterGlozikUmbracoOsobnaStranka.lib.Models.DashboardBlog
{
    public class MemberBlogModel : _BaseModel
    {
        [Required(ErrorMessage = "Názov authora musí byť zadaný")]
        [Display(Name = "Názov authora")]
        public string BlogName { get; set; }

        [Display(Name = "Webová stránka")]
        public string BlogWeb { get; set; }

        [AllowHtml]
        [Display(Name = "Popis authora")]
        public string BlogDescription { get; set; }

        public HttpPostedFileBase BlogImage { get; set; }
        public string BlogImagePath { get; set; }

        public void CopyDataFrom(OsobnaStrankaBlogPost src)
        {
            this.pk = src.pk;
            this.BlogName = src.BlogName;
            this.BlogDescription = src.BlogDescription;
            this.BlogWeb = src.BlogWeb;
            this.BlogImagePath = src.BlogImagePath;
        }

        public void CopyDataTo(OsobnaStrankaBlogPost trg)
        {
            trg.pk = this.pk;
            trg.BlogName = this.BlogName;
            trg.BlogDescription = this.BlogDescription;
            trg.BlogWeb = this.BlogWeb;
            trg.BlogImagePath = this.BlogImagePath;
        }

        public static MemberBlogModel CreateCopyFrom(OsobnaStrankaBlogPost src)
        {
            MemberBlogModel trg = new MemberBlogModel();
            trg.CopyDataFrom(src);

            return trg;
        }

        public static OsobnaStrankaBlogPost CreateCopyFrom(MemberBlogModel src)
        {
            OsobnaStrankaBlogPost trg = new OsobnaStrankaBlogPost();
            src.CopyDataTo(trg);

            return trg;
        }
    }

    public class ProducerListModel : List<MemberBlogModel>
    {
        public HttpRequest CurrentRequest { get; private set; }
        public string SessionId { get; set; }
        public int PageSize { get; private set; }

        private GridPagerModel currentPager;
        public GridPagerModel Pager
        {
            get
            {
                return GetPager();
            }
        }

        public ProducerListModel(HttpRequest request, int pageSize = 25)
        {
            this.CurrentRequest = request;
            this.PageSize = pageSize;
        }

        public List<MemberBlogModel> GetPageItems()
        {
            GridPageInfo cpi = this.Pager.GetCurrentPageInfo();

            List<MemberBlogModel> resultList = new List<MemberBlogModel>();
            for (int i = cpi.FirsItemIndex; i < this.Count && i < cpi.LastItemIndex + 1; i++)
            {
                resultList.Add(this[i]);
            }

            return resultList;
        }

        GridPagerModel GetPager()
        {
            if (this.currentPager == null || this.currentPager.ItemCnt != this.Count)
            {
                this.currentPager = new GridPagerModel(this.CurrentRequest, this.Count, this.PageSize);
            }

            return this.currentPager;
        }
    }

    public class BlogPagingListModel : _PagingModel
    {
        public List<MemberBlogModel> Items { get; set; }

        public static BlogPagingListModel CreateCopyFrom(Page<OsobnaStrankaBlogPost> srcArray)
        {
            BlogPagingListModel trgArray = new BlogPagingListModel();
            trgArray.ItemsPerPage = (int)srcArray.ItemsPerPage;
            trgArray.TotalItems = (int)srcArray.TotalItems;
            trgArray.Items = new List<MemberBlogModel>(srcArray.Items.Count + 1);

            foreach (OsobnaStrankaBlogPost src in srcArray.Items)
            {
                trgArray.Items.Add(MemberBlogModel.CreateCopyFrom(src));
            }

            return trgArray;
        }
    }

    public class BlogFilterModel : _BaseUserPropModel
    {

        [Display(Name = "Vyhľadávanie (názov, popis, web, ...)")]
        public string SearchText { get; set; }


        public BlogFilterModel()
        {
            this.PropId = ConfigurationUtil.PropId_BlogFilterModel;
        }

        public static BlogFilterModel CreateCopyFrom(OsobnaStrankaUserProp src)
        {
            BlogFilterModel trg = new BlogFilterModel();
            if (src != null)
            {
                trg.CopyDataFrom(src);
            }
            trg.UpdateBeforeEdit();

            return trg;
        }

        public static OsobnaStrankaUserProp CreateCopyFrom(BlogFilterModel src)
        {
            src.UpdateAfterEdit();
            OsobnaStrankaUserProp trg = new OsobnaStrankaUserProp();
            src.CopyDataTo(trg);

            return trg;
        }


        public void UpdateBeforeEdit()
        {
            LoadPropValue(this.PropValue);
        }

        public void UpdateAfterEdit()
        {
            this.PropValue = SavePropValue();
        }

        private string SavePropValue()
        {
            // Create XML document
            XmlDocument doc = new XmlDocument();
            // Create main element
            XmlElement mainNode = doc.CreateElement("BlogFilterModel");
            mainNode.SetAttribute("version", "1.0");
            doc.AppendChild(mainNode);

            // Search text
            XmlParamSet.SaveItem(doc, mainNode, "SearchText", this.SearchText);

            return doc.InnerXml;
        }

        private void LoadPropValue(string propValue)
        {
            XmlDocument doc = new XmlDocument();
            if (!string.IsNullOrEmpty(propValue))
            {
                doc.LoadXml(propValue);

                string fullParent = "BlogFilterModel";

                // Search text
                this.SearchText = XmlParamSet.LoadItem(doc, fullParent, "SearchText", string.Empty);
            }
        }
    }
    public class ProducerPagerModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class ProducerDropDown : CmpDropDown
    {
        public ProducerDropDown()
        {
        }

        public static ProducerDropDown CreateFromRepository(bool allowNull, string emptyText = "[ nezadané ]")
        {
            BlogPostRepository repository = new BlogPostRepository();
            return ProducerDropDown.CreateCopyFrom(repository.GetPage(1, _PagingModel.AllItemsPerPage), allowNull, emptyText);
        }

        public static ProducerDropDown CreateCopyFrom(Page<OsobnaStrankaBlogPost> dataList, bool allowNull, string emptyText)
        {
            ProducerDropDown ret = new ProducerDropDown();

            if (allowNull)
            {
                ret.AddItem(emptyText, Guid.Empty.ToString(), null);
            }
            foreach (OsobnaStrankaBlogPost dataItem in dataList.Items)
            {
                MemberBlogModel dataModel = MemberBlogModel.CreateCopyFrom(dataItem);
                ret.AddItem(dataModel.BlogName, dataModel.pk.ToString(), dataModel);
            }

            return ret;
        }
    }
}
