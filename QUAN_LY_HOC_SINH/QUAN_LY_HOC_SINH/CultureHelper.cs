using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace QUAN_LY_HOC_SINH
{
    public class CultureHelper
    {
        private static readonly List<string> cultures = new List<string> {
            "vi",
            "en"
        };

        /// <summary>
        /// Returns a valid culture name based on "name" parameter. If "name" is not valid, it returns the default culture "vi-VN"
        /// </summary>
        /// <param name="name" />Culture's name (e.g. vi-VN)</param>
        public static string GetImplementedCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return GetDefaultCulture();
            }

            if (cultures.Where(c => c.Equals(name, StringComparison.InvariantCultureIgnoreCase)).Count() > 0)
            {
                return name;
            }

            string neutralCulture = GetNeutralCulture(name);
            foreach (string culture in cultures)
            {
                if (culture.StartsWith(neutralCulture))
                {
                    return culture;
                }
            }

            return GetDefaultCulture();
        }
        public static string GetDefaultCulture()
        {
            return cultures[0];
        }
        public static string GetCurrentNeutralCulture()
        {
            return GetNeutralCulture(Thread.CurrentThread.CurrentCulture.Name);
        }
        public static string GetNeutralCulture(string name)
        {
            if (!name.Contains("-")) return name;

            return name.Split('-')[0]; // Read first part only. E.g. "en", "es"
        }

        public static SelectList GetSelectListOfAllCultures()
        {
            IEnumerable<SelectListItem> allCultures = new List<SelectListItem>(cultures
                   .Select(x => new SelectListItem
                   {
                       Value = x,
                       Text = GetDisplayNameOfCulture(x)
                   })
               );

            return new SelectList(allCultures.OrderBy(x => x.Text), "Value", "Text",
                GetCurrentNeutralCulture());
        }

        private static string GetDisplayNameOfCulture(string name)
        {
            switch (name)
            {
                case "en":
                    return "English";
                case "vi":
                    return "Tiếng Việt";
                default:
                    return "Tiếng Việt";
            }
        }
    }
}