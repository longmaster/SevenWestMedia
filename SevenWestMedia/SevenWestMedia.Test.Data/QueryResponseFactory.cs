using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Users.Queries;

namespace SevenWestMedia.Test.Data;

public static class QueryResponseFactory
{
    public static GetUserSummaryQueryResponse CreateGetUserSummaryQueryResponse()
    {
        return new GetUserSummaryQueryResponse { 
            FirstName = "Test FirstName",
            UserFullName = CreateListUserFullName,
            GenderPerAges = CreateListGenderPerAge
        };
    }

    public static List<string> CreateListUserFullName => new List<string>() { "Bill", "Adam", "Peter" };

    public static List<GenderPerAge> CreateListGenderPerAge => new List<GenderPerAge>() { new GenderPerAge()
                {
                     Age = 20, Female = 1, Male = 1,
                } };
}
