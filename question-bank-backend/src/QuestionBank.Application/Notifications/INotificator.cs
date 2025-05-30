using FluentValidation.Results;

namespace QuestionBank.Application.Notifications;

public interface INotificator
{
    void Handle(string message);
    void Handle(List<ValidationFailure> failures);
    void HandleNotFoundResource();
    IEnumerable<string> GetNotifications();

    bool HasNotification { get; }
    bool IsNotFoundResource { get; }
}