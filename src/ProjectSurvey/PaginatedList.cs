using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectSurvey.Models;

public class PaginatedList<T> : List<T>
{
    public  int PageIndex { get;private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        this.AddRange(items);
    }

    public bool HasPreviousPage(int PageIndex)
    {
  
            return (PageIndex > 1);
        
    }

    public bool HasNextPage(int PageIndex)
    {
      
            return (PageIndex < TotalPages);
        
    }

    public static PaginatedList<T> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, bool isUncompletedQuestions)
    {
        var count =  source.Count();
        var items = isUncompletedQuestions ? source.ToList() : source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}