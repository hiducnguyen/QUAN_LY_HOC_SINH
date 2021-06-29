using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Repositories.Enums
{
    public enum Semester
    {
        First = 1,
        Second = 2
    }
    public class SemesterHelper
    {
        public static SelectList GetAllSemesters()
        {
            IEnumerable<SelectListItem> allSemesters = new List<SelectListItem>(Enumerable.Range(1, 2)
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.ToString()
                })
            );

            return new SelectList(allSemesters.OrderBy(x => x.Text), "Value", "Text");
        }
    }
}