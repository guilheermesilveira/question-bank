using QuestionBank.Application.DTOs.Test;

namespace QuestionBank.Application.Contracts.Services;

public interface ITestService
{
    Task<TestDto?> Create(CreateTestDto dto);
    Task<TestDto?> Finish(FinishTestDto dto);
}
