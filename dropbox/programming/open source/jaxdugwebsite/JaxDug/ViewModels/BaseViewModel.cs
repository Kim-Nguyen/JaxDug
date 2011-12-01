using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JaxDug.Models;

namespace JaxDug.ViewModels
{
    public class BaseViewModel
    {
        public ErrorDisplay ErrorDisplay = null;
        public UserState UserState = null;
        public string baseUrl = HttpContext.Current.Request.ApplicationPath;
        public string PageTitle = null;
        public dynamic ViewModel = null;

        public PagingDetails Paging = null;
    }

    /// <summary>
    /// Contains information 
    /// </summary>
    public class PagingDetails
    {
        public bool RenderPager = true;

        public int Page = 1;
        public int PageCount = 1;
        public int PageSize = 20;
        public int TotalPages = 1;
        public int TotalItems = 0;

        public int MaxPageButtons = 10;

        /// <summary>
        /// Client handler function called on POST operation with page number as parameter
        /// </summary>
        public string ClientPageClickHandler = "pageClick";
    }
}
