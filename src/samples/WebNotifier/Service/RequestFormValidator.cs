//using FluentValidation;

//namespace WebNotifier.Service
//{
//    public class RequestFormValidator : AbstractValidator<RequestFormModel>
//    {
//        public RequestFormValidator()
//        {
//            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Please specify first name");
//            RuleFor(x => x.LastName).NotEmpty().WithMessage("Please specify last name");
//            RuleFor(x => x.Email).EmailAddress().WithMessage("Please specify valid email");
//            RuleFor(x => x.ServiceDescription).NotEmpty().WithMessage("Please specify a service description");
//        }

//    }
//}