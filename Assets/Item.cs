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