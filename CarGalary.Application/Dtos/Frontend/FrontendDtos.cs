namespace CarGalary.Application.Dtos.Frontend
{
    public class FrontendUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Nickname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? AvatarUrl { get; set; }
        public bool ProfileComplete { get; set; }
    }

    public class FrontendSignupRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class FrontendFillProfileRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Nickname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class FrontendUpdateProfileRequest
    {
        public string? FullName { get; set; }
        public string? Nickname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class FrontendCarDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageKey { get; set; } = string.Empty;
        public List<string> SliderImageKeys { get; set; } = new();
        public string Price { get; set; } = "0.00";
        public int NumReviews { get; set; }
        public decimal Rating { get; set; }
        public int NumSolds { get; set; }
        public string CategoryId { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string BrandLogoKey { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Colors { get; set; } = new();
    }

    public class FrontendCarQueryDto
    {
        public string? Search { get; set; }
        public string? CategoryId { get; set; }
        public string? SortBy { get; set; }
        public string? MinPrice { get; set; }
        public string? MaxPrice { get; set; }
        public string? Limit { get; set; }
        public string? Offset { get; set; }
    }

    public class FrontendCarsResponseDto
    {
        public List<FrontendCarDto> Cars { get; set; } = new();
        public int Total { get; set; }
    }

    public class FrontendCarResponseDto
    {
        public FrontendCarDto Car { get; set; } = new();
    }

    public class FrontendInquiryDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CarId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
    }

    public class FrontendInquiryWithCarDto : FrontendInquiryDto
    {
        public FrontendCarDto? Car { get; set; }
    }

    public class FrontendInquiriesResponseDto
    {
        public List<FrontendInquiryWithCarDto> Inquiries { get; set; } = new();
    }

    public class FrontendInquiryResponseDto
    {
        public FrontendInquiryDto Inquiry { get; set; } = new();
    }

    public class FrontendCreateInquiryRequestDto
    {
        public string CarId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
