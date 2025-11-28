using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Person
        CreateMap<PersonCreateDto, Person>();
        CreateMap<Person, PersonReadDto>()
            .ForMember(dest => dest.FullName, opt =>
                opt.MapFrom(src => $"{src.Name} {src.LastName} {src.SecondLastName}"));

        // Loan
        CreateMap<LoanCreateDto, Loan>();
        CreateMap<Loan, LoanReadDto>()
            .ForMember(dest => dest.PersonName, opt =>
                opt.MapFrom(src => $"{src.Person.Name} {src.Person.LastName}"))
            .ForMember(dest => dest.PaidInstallments, opt =>
                opt.MapFrom(src => src.Installments.Count(i => i.Status == InstallmentStatus.Paid)))
            .ForMember(dest => dest.PendingInstallments, opt =>
                opt.MapFrom(src => src.Installments.Count(i => i.Status != InstallmentStatus.Paid)));
        
        CreateMap<Loan, LoanDetailDto>()
            .ForMember(dest => dest.PersonName, opt =>
                opt.MapFrom(src => $"{src.Person.Name} {src.Person.LastName}"))
            .ForMember(dest => dest.Installments, opt =>
                opt.MapFrom(src => src.Installments));

        // Installment
        CreateMap<Installment, InstallmentReadDto>();

        // Payment
        CreateMap<PaymentCreateDto, Payment>();
        CreateMap<Payment, PaymentReadDto>();
    }
}
