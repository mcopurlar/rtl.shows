namespace Rtl.Shows.Repository;

public class ShowPerson 
{
    public int ShowId { get; set; }
    public Show Show { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
}