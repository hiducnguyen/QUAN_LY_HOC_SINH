using Resources;
using System.Collections.Generic;

namespace Repositories.Enums
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }
    public class GenderHelper
    {
        public static List<Gender> GetAllGenders()
        {
            return new List<Gender>
            {
                Gender.Male,
                Gender.Female,
                Gender.Other
            };
        }

        /// <summary>
        /// Transfer the enum Gender to the text to display (ex: Gender.Male => "Nam")
        /// </summary>
        /// <param name="gender"></param>
        /// <returns></returns>
        public static string GetText(Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return Resource.Male;
                case Gender.Female:
                    return Resource.Female;
                default:
                    return Resource.Other;
            }
        }

        public static Gender ToGender(string s)
        {
            switch (s)
            {
                case "Male":
                    return Gender.Male;
                case "Female":
                    return Gender.Female;
                case "Other":
                    return Gender.Other;
                default:
                    return Gender.Other;
            }
        }
    }
}