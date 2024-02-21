namespace TestingApi.Dto.Response;

public class BaseResponseDto
{
    public Guid Id { get; set; }

    public DateTime CreatedTimestamp { get; set; }

    public DateTime? ModifiedTimestamp { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? ModifiedBy { get; set; }
}