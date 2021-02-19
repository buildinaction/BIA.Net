// <copyright file="DefaultViewDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Domain.Dto.View
{
    public class DefaultViewDto
    {
        public int Id { get; set; }

        public string TableId { get; set; }

        public bool IsDefault { get; set; }
    }
}