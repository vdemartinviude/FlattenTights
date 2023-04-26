using System.ComponentModel.DataAnnotations;

namespace api.Request;

public class FlattenDataRequest
{
    [Required]
    public DateTime date { get; set; }
}
