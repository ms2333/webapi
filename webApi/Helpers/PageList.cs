using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApi.Helpers
{
    public class PageList<T>:List<T>
    {
        public int CurrentPage {private set; get; }//当前页
        public int TotalPages {private set; get; }//总页数
        public int PageSize {private set; get; }//单页内容数量
        public int TotalCount {private set; get; }//数据总数

        public bool HasPrevious => CurrentPage > 1;//是否有前一页
        public bool HasNext => CurrentPage < TotalPages;//是否有前一页

        public PageList(List<T> items, int count ,int pageNumber,int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling((double)count / pageSize);//取最大整数
            AddRange(items);//****************************************************************************************
        }

        public static async Task<PageList<T>>CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count =await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();//取从(pageNumber - 1) * pageSize向后pageSize个数据
            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}
