namespace TravelOrganization2.Model.Dto.Common
{
    public class ResultDto
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class ResultDto<TData>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public TData Data { get; set; }
    }
}
