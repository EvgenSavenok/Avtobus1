using Avtobus1.Application.DTO;
using Avtobus1.Application.Services;
using Avtobus1.Domain.CustomExceptions;
using Avtobus1.Domain.Entities;
using Avtobus1.Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Avtobus1Tests;

public class UrlServiceTests
{
    private readonly Mock<IRepositoryManager> _mockRepoManager;
    private readonly Mock<IUrlRepository> _mockUrlRepo;
    private readonly Mock<IValidator<UrlRecord>> _mockValidator;
    private readonly UrlService _service;

    public UrlServiceTests()
    {
        _mockRepoManager = new Mock<IRepositoryManager>();
        _mockUrlRepo = new Mock<IUrlRepository>();
        _mockValidator = new Mock<IValidator<UrlRecord>>();

        _mockRepoManager.Setup(m => m.Url).Returns(_mockUrlRepo.Object);

        _service = new UrlService(_mockRepoManager.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task CreateShortUrlAsync_ValidData_ReturnsDtoAndSavesToDb()
    {
        // Arrange
        var inputDto = new UrlDto { OriginalUrl = "https://google.com" };

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<UrlRecord>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // Код уникален
        _mockUrlRepo
            .Setup(r => r.IsCodeUniqueAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateShortUrlAsync(inputDto);

        // Assert 
        result.Should().NotBeNull();
        result.OriginalUrl.Should().Be(inputDto.OriginalUrl);
        
        result.ShortCode.Should().NotBeNullOrEmpty()
            .And.HaveLength(7)
            .And.MatchRegex("^[a-zA-Z0-9]*$"); 

        _mockUrlRepo.Verify(r => r.CreateAsync(It.IsAny<UrlRecord>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepoManager.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateShortUrlAsync_InvalidData_ThrowsValidationException()
    {
        // Arrange
        var inputDto = new UrlDto { OriginalUrl = "" };

        // Валидатор возвращает ошибку
        var failures = new List<ValidationFailure> { new("OriginalUrl", "Url cannot be empty") };
        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<UrlRecord>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(failures));

        _mockUrlRepo.Setup(r => r.IsCodeUniqueAsync(It.IsAny<string>(), default)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _service.CreateShortUrlAsync(inputDto);

        // Assert
        await act.Should().ThrowAsync<ValidationException>()
            .Where(e => e.Errors.Any(x => x.ErrorMessage == "Url cannot be empty")); 

        _mockUrlRepo.Verify(r => r.CreateAsync(It.IsAny<UrlRecord>(), default), Times.Never);
        _mockRepoManager.Verify(m => m.SaveAsync(default), Times.Never);
    }

    [Fact]
    public async Task GetAllUrlsAsync_ReturnsListOfDtos()
    {
        // Arrange
        var records = new List<UrlRecord>
        {
            new() { Id = Guid.NewGuid(), OriginalUrl = "http://a.com", ShortCode = "1111111" },
            new() { Id = Guid.NewGuid(), OriginalUrl = "http://b.com", ShortCode = "2222222" }
        };

        _mockUrlRepo
            .Setup(r => r.FindAllAsync(false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(records);

        // Act
        var result = await _service.GetAllUrlsAsync();

        // Assert
        result.Should().NotBeNull()
            .And.HaveCount(2)
            .And.BeInDescendingOrder(x => x.CreatedAt); 

        result[0].OriginalUrl.Should().Be("http://a.com");
    }

    [Fact]
    public async Task GetOriginalUrlAsync_ExistingCode_IncrementsClicksAndReturnsUrl()
    {
        // Arrange
        var shortCode = "AbCd123";
        var record = new UrlRecord 
        { 
            OriginalUrl = "https://mysite.com", 
            ClickCount = 0, 
            ShortCode = shortCode 
        };

        _mockUrlRepo
            .Setup(r => r.GetByCodeAsync(shortCode, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(record);

        // Act
        var resultUrl = await _service.GetOriginalUrlAsync(shortCode);

        // Assert
        resultUrl.Should().Be("https://mysite.com");
        
        record.ClickCount.Should().Be(1);

        _mockRepoManager.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetOriginalUrlAsync_NonExistingCode_ThrowsNotFoundException()
    {
        // Arrange
        var shortCode = "NonExistent";

        _mockUrlRepo
            .Setup(r => r.GetByCodeAsync(shortCode, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UrlRecord?)null);

        // Act
        Func<Task> act = async () => await _service.GetOriginalUrlAsync(shortCode);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Short URL not found"); 

        _mockRepoManager.Verify(m => m.SaveAsync(default), Times.Never);
    }

    [Fact]
    public async Task DeleteUrlAsync_ExistingId_DeletesAndSaves()
    {
        // Arrange
        var id = Guid.NewGuid();
        var record = new UrlRecord { Id = id };

        _mockUrlRepo
            .Setup(r => r.GetByIdAsync(id, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(record);

        // Act
        await _service.DeleteUrlAsync(id);

        // Assert
        _mockUrlRepo.Verify(r => r.DeleteAsync(record, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepoManager.Verify(m => m.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}