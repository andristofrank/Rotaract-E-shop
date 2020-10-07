using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RotaractServer.ResourseParameters
{
    public class PageResourseParameters
    {
        const int maxPageSize = 60;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public string Fields { get; set; }
    }
}