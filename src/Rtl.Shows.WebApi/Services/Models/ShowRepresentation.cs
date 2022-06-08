namespace Rtl.Shows.WebApi.Services.Models;

public class ShowRepresentation
{
    public int Id { get; set; }

    public string Name { get; set; }

    public IList<PersonRepresentation> Cast { get; set; }
}