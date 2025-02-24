using System.Collections.Generic;

public class UserInfo
{
    public string userName;
    public int  age;
    public string email;
    public string location;

    public UserInfo(string userName, int age, string email, string location)
    {
        this.userName = userName;
        this.age = age;
        this.email = email;
        this.location = location;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>
        {
            { "name", userName },
            { "age", age },
            { "email", email },
            { "location", location }
        };
    }
}
