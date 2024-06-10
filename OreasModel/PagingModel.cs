using System;
using System.Collections.Generic;
using System.Text;

namespace OreasModel
{
    public class PagedData<T> where T : class
    {
        public PagedData()
        {
            PageSize = 5;
        }

        public IEnumerable<T> Data { get; set; }
        public object otherdata { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }


    }
}
