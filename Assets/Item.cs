using System.Collections.Generic;

public class Storage
{
    public List<Clothe> Clothes { get; set; }
}
public class Clothe
{
    public string Name { get; set; }

    public string Type { get; set; }
    public string Description { get; set; }
    public bool IsWearing { get; set; }

    public Clothe(string name, string type, string description, bool isWearing)
    {
        Name = name;
        Type = type;
        Description = description;
        IsWearing = isWearing;
    }

        
}

public class NoticeStorage
{
    public List<Notice> notices { get; set; }
}
public class Notice
{
    public string id { get; set; }
    public bool is_active { get; set; }
    public string start_date { get; set; }
    public string end_date { get; set; }
    public List<NoticeBody> bodies { get; set; }
}

public class NoticeBody
{
    public string content { get; set; }
    public string subject { get; set; }
    public string language { get; set; }
}

public class MailBox
{
    public List<MailBoxBody> bodies { get; set; }
    public List<MailBoxItem> items { get; set; }
}

public class MailBoxBody
{
    public string content { get; set; }
    public string subject { get; set; }
    public string language { get; set; }
    
}

public class MailBoxItem
{
    public string id { get; set; }
    public int quantity { get; set; }
}