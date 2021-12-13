using AutoMapper;
using OpsMain.Shared;
using OpsMain.Shared.Dto;
using OpsMain.StorageLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpsMain._3rdService.Mapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<SysMenu, SysMenuDto>().ReverseMap();
            CreateMap<SysUser, SysUserDto>().ForMember(dest=>dest.Password,opt=>opt.Ignore()).ReverseMap();
            CreateMap<SysRole, SysRoleDto>().ReverseMap();
            
        }
    }
}
