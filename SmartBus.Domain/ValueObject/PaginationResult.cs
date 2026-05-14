using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.ValueObject
{
    public class PaginationResult<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }  
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNext  => PageNumber < TotalPages;
        public bool HasPrevious => PageNumber > 1;
        public PaginationResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);


        }
    }
}
