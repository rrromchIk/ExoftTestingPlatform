namespace TestingApi.Models; 

public abstract class BaseEntity {
    public Guid Id { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Modified { get; set; }

    public Guid CreatedBy { get; set; }
    
    public Guid? ModifiedBy { get; set; }
}