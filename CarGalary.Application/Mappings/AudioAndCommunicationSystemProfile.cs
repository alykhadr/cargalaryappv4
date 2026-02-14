using AutoMapper;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Command;
using CarGalary.Application.Dtos.AudioAndCommunicationSystem.Query;
using CarGalary.Domain.Entities;

namespace CarGalary.Application.Mappings
{
    public class AudioAndCommunicationSystemProfile : Profile
    {
        public AudioAndCommunicationSystemProfile()
        {
            CreateMap<AudioAndCommunicationSystem, AudioAndCommunicationSystemResponseDto>();
            CreateMap<CreateAudioAndCommunicationSystemRequestDto, AudioAndCommunicationSystem>();
            CreateMap<UpdateAudioAndCommunicationSystemRequestDto, AudioAndCommunicationSystem>();
        }
    }
}

