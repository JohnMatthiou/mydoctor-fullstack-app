using AutoMapper;
using MyDoctorApp.Repositories;

namespace MyDoctorApp.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public UserService UserService => new(_unitOfWork, _mapper);

        public PatientService PatientService => new(_unitOfWork, _mapper);

        public DoctorService DoctorService => new(_unitOfWork, _mapper);
    }
}
