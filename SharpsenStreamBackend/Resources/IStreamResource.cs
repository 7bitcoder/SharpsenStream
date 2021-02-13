﻿using SharpsenStreamBackend.Classes.Dto;
using System.Threading.Tasks;

namespace SharpsenStreamBackend.Resources
{
    public interface IStreamResource
    {
        Task<StreamDto> getStream(string streamName);
    }
}